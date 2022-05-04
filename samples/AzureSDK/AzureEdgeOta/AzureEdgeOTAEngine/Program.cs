// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.

using AzureEdgeOTAEngine;
using nanoFramework.Azure.Devices.Client;
using nanoFramework.Azure.Devices.Shared;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Json;
using nanoFramework.M2Mqtt.Messages;
using nanoFramework.Networking;
using System;
using System.Buffers.Binary;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

const string RootPath = "I:\\";
const string Version = RootPath + "version";
const string UdatedProperty = "CodeUpdated";
const string CodeRunning = "Code Running";
const string CodeNotRunning = "Code NOT Running";
const string IntegrityError = "Integrity error";
const string OtaRunnerName = "CountMeasurement.OtaRunner";

// One minute unit
int sleepTimeMinutes = 60000;
int millisecondsToSleep = 500;
ArrayList assemblies = new();
int version = -1;
MethodInfo stop = null;
MethodInfo twinUpated = null;
DeviceClient azure;
Assembly toRun = null;
bool isRunning = false;
FileSettings[] filesToDownload = null;
HttpClient httpClient = new HttpClient();

try
{
    Trace("Program Started.");

    Trace("Connecting to wifi.");
    // As we are using TLS, we need a valid date & time
    // We will wait maximum 1 minute to get connected and have a valid date
    CancellationTokenSource cs = new(sleepTimeMinutes);
    var success = WifiNetworkHelper.ConnectDhcp(Secrets.Ssid, Secrets.Password, requiresDateTime: true, token: cs.Token);
    if (!success)
    {
        Trace($"Can't connect to wifi: {WifiNetworkHelper.Status}");
        if (WifiNetworkHelper.HelperException != null)
        {
            Trace($"{WifiNetworkHelper.HelperException}");
        }

        GoToSleep();
    }

    Trace($"Date and time is now {DateTime.UtcNow}");

    // First let's check if we have a version
    if (File.Exists(Version))
    {
        using FileStream fs = new FileStream(Version, FileMode.Open, FileAccess.Read);
        byte[] buff = new byte[fs.Length];
        fs.Read(buff, 0, buff.Length);
        // This if the version we have stored
        version = BinaryPrimitives.ReadInt32BigEndian(buff);
    }

    // We are storing this certificate in the resources
    X509Certificate azureRootCACert = new X509Certificate(Resource.GetBytes(Resource.BinaryResources.AzureRoot));
    azure = new(Secrets.IotBrokerAddress, Secrets.DeviceID, Secrets.SasKey, MqttQoSLevel.AtLeastOnce, azureRootCACert);
    if (!azure.Open())
    {
        Trace($"Arg, not open!");
    }

    //If we are connected, we can move forward
    if (azure.IsConnected)
    {
        // Gets the twins and check the version of the desired properties
        int retry = 0;
    RetryTwins:
        var twins = azure.GetTwin(new CancellationTokenSource(10000).Token);
        if (twins == null)
        {
            if (retry++ > 3)
            {
                GoToSleep();
            }

            Thread.Sleep(2000);
            goto RetryTwins;
        }

        ProcessTwinAndDownloadFiles(twins.Properties.Desired);
    }

    // Subscribe to the twin change
    azure.TwinUpated += TwinUpdated;
}
catch (Exception ex)
{
    // We won't do anything
    // This global try catch is to make sure whatever happen, we will safely be able to go
    // To sleep
    Trace(ex.ToString());
    GoToSleep();
}

Thread.Sleep(Timeout.InfiniteTimeSpan);

void ProcessTwinAndDownloadFiles(TwinCollection desired)
{
    int codeVersion = 0;
    codeVersion = (int)desired["CodeVersion"];
    string[] files;
    TwinCollection reported = new TwinCollection();
    // If the version is the same as the stored one, no changes, we can load the code
    // Otherwise we have to download a new version
    if (codeVersion != version)
    {
        // Stop the previous instance
        stop?.Invoke(null, null);
        // Let's first clean all the pe files
        // We keep any other file
        files = Directory.GetFiles(RootPath);
        foreach (var file in files)
        {
            if (file.EndsWith(".pe"))
            {
                File.Delete(file);
            }
        }

        // Now download all the files from the twin
        string token = (string)desired["Token"];
        var desiredFiles = desired["Files"] as ArrayList;
        filesToDownload = new FileSettings[desiredFiles.Count];
        int inc = 0;
        foreach (var singleFile in desiredFiles)
        {
            FileSettings file = (FileSettings)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(singleFile), typeof(FileSettings));
            filesToDownload[inc++] = file;
            DownloadBinaryFile(file.FileName, token);
        }

        using FileStream fs = new FileStream(Version, FileMode.Create, FileAccess.Write);
        byte[] buff = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(buff, codeVersion);
        fs.Write(buff, 0, buff.Length);
        fs.Flush();
        fs.Dispose();

        reported.Add(UdatedProperty, true);
        // If we had a previous instance, we will reboot
        // this will allow to have the previous code cleared
        if (isRunning)
        {
            if (azure.IsConnected)
            {
                azure.UpdateReportedProperties(reported);
                // Make sure we report
                Thread.Sleep(100);
            }

            azure.Close();
            // We go to sleep and we don't reboot
            // Going to sleep will clean as well native part
            GoToSleep();
        }

        // Reconnect if disconnected
        if (!azure.IsConnected)
        {
            azure.Open();
        }
    }

    // Now load the assemblies, they must be on the disk
    if (!isRunning)
    {
        LoadAssemblies();

        isRunning = false;
        if (toRun != null)
        {
            Type typeToRun = toRun.GetType(OtaRunnerName);
            var start = typeToRun.GetMethod("Start");
            stop = typeToRun.GetMethod("Stop");
            twinUpated = typeToRun.GetMethod("TwinUpdated");

            if (start != null)
            {
                try
                {
                    // See if all goes right
                    start.Invoke(null, new object[] { azure });
                    isRunning = true;
                }
                catch (Exception)
                {
                    isRunning = false;
                }
            }
        }
    }
    // And update the reported twin
    string status = isRunning ? CodeRunning : CodeNotRunning;
    reported.Add(CodeRunning, status);
    azure.UpdateReportedProperties(reported);
}

void TwinUpdated(object sender, TwinUpdateEventArgs e)
{
    ProcessTwinAndDownloadFiles(e.Twin);
    twinUpated?.Invoke(null, new object[] { e.Twin });
}

void LoadAssemblies()
{
    // Now load all the assemblies we have on the storage
    var files = Directory.GetFiles(RootPath);
    foreach (var file in files)
    {
        if (file.EndsWith(".pe"))
        {
            using FileStream fspe = new FileStream(file, FileMode.Open, FileAccess.Read);
            Trace($"{file}: {fspe.Length}");
            var buff = new byte[fspe.Length];
            fspe.Read(buff, 0, buff.Length);
            // Needed as so far, there seems to be an issue when loading them too fast
            fspe.Close();
            fspe.Dispose();
            Thread.Sleep(20);
            bool integrity = true;
            string strsha = string.Empty;
            // Check integrity if we just downloaded it
            if (filesToDownload != null)
            {
                integrity = false;
                var sha256 = SHA256.Create().ComputeHash(buff);
                strsha = BitConverter.ToString(sha256);
                var fileName = file.Substring(file.LastIndexOf('\\') + 1);
                foreach (FileSettings filesetting in filesToDownload)
                {
                    if (filesetting.FileName.Substring(filesetting.FileName.LastIndexOf('/') + 1) == fileName)
                    {
                        if (strsha == filesetting.Signature)
                        {
                            integrity = true;
                            break;
                        }
                    }
                }
            }

            if (!integrity)
            {
                Trace("Error with file signature");
                TwinCollection reported = new();
                reported.Add(CodeRunning, $"{IntegrityError}:{file} - {strsha}");
                azure.UpdateReportedProperties(reported);
                break;
            }

            var ass = Assembly.Load(buff);
            var typeToRun = ass.GetType(OtaRunnerName);
            if (typeToRun != null)
            {
                toRun = ass;
            }
        }
    }
}

void GoToSleep()
{
    Trace($"Set wakeup by timer for {millisecondsToSleep} minutes to retry.");
    Sleep.EnableWakeupByTimer(new TimeSpan(0, 0, millisecondsToSleep, 0));
    Trace("Deep sleep now");
    Sleep.StartDeepSleep();
}

void DownloadBinaryFile(string url, string sas)
{
    string fileName = url.Substring(url.LastIndexOf('/') + 1);
    // If we are connected to Azure, we will disconnect as small devices only have limited memory
    if (azure.IsConnected)
    {
        azure.Close();
    }

    httpClient.DefaultRequestHeaders.Add("x-ms-blob-type", "BlockBlob");
    // this example uses Tls 1.2 with Azure
    httpClient.SslProtocols = System.Net.Security.SslProtocols.Tls12;
    // use the pem certificate we created earlier
    httpClient.HttpsAuthentCert = new X509Certificate(Resource.GetBytes(Resource.BinaryResources.azurePEMCertBaltimore));
    HttpResponseMessage response = httpClient.Get($"{url}?{sas}");
    response.EnsureSuccessStatusCode();

    using FileStream fs = new FileStream($"{RootPath}{fileName}", FileMode.Create, FileAccess.Write);
    response.Content.ReadAsStream().CopyTo(fs);
    fs.Flush();
    fs.Close();
    response.Dispose();
}

void Trace(string message)
{
    Debug.WriteLine(message);
}
