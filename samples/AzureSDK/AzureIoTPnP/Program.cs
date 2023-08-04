// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using AzureIoTPnP;
using nanoFramework.Azure.Devices.Client;
using nanoFramework.Azure.Devices.Shared;
using nanoFramework.Json;
using nanoFramework.Networking;

const string TargetTemerature = "targetTemperature";
const string MaxTempSinceLastReboot = "maxTempSinceLastReboot";

// Connect to wifi
if (!ConnectToWifi())
{
    Debug.WriteLine("Can't connect to wifi");
    return;
}

// Create and Azure IoT connection
// You will find the model here: https://github.com/Azure/iot-plugandplay-models/blob/main/dtmi/com/example/thermostat-1.json
DeviceClient azureIoT = new DeviceClient(Secrets.IotHub, Secrets.DeviceName, Secrets.SasKey, azureCert: new X509Certificate(Resource.GetString(Resource.StringResources.AzureRootCerts)), modelId: "dtmi:com:example:Thermostat;1");
azureIoT.TwinUpdated += AzureTwinUpdated;
azureIoT.AddMethodCallback(getMaxMinReport);

if (!azureIoT.Open())
{
    Debug.WriteLine("ERROR: Not connected to Azure IoT");
    return;
}


Debug.WriteLine("Connected");

TwinCollection twin = new();
twin.Add(MaxTempSinceLastReboot, 42.42);
azureIoT.UpdateReportedProperties(twin);

// Just as an example send some temperature, increasing them to see the result
int inc = 10;
while (true)
{
    azureIoT.SendMessage($"{{\"temperature\":{inc++}}}");
    inc = inc > 40 ? 10 : inc;
    Thread.Sleep(10000);
}

Thread.Sleep(Timeout.Infinite);

bool ConnectToWifi()
{
    Debug.WriteLine("Program Started, connecting to Wifi.");

    // As we are using TLS, we need a valid date & time
    // We will wait maximum 1 minute to get connected and have a valid date
    var success = WifiNetworkHelper.ConnectDhcp(Secrets.Ssid, Secrets.Passwd, requiresDateTime: true, token: new CancellationTokenSource(60000).Token);
    if (!success)
    {
        Debug.WriteLine($"Can't connect to wifi: {WifiNetworkHelper.Status}");
        if (WifiNetworkHelper.HelperException != null)
        {
            Debug.WriteLine($"WifiNetworkHelper.HelperException");
        }
    }

    Debug.WriteLine($"Date and time is now {DateTime.UtcNow}");
    return success;
}

void AzureTwinUpdated(object sender, TwinUpdateEventArgs e)
{
    if (e.Twin.Contains(TargetTemerature))
    {
        // We got an update for the target temperature
        var target = e.Twin[TargetTemerature];
        Debug.WriteLine($"Target temperature updated: {target}");
        PropertyAcknowledge targetReport = new() { Version = (int)e.Twin.Version, Status = PropertyStatus.Completed, Description = "All perfect", Value = target };
        TwinCollection twin = new TwinCollection();
        twin.Add(TargetTemerature, targetReport.BuildAcknowledge());
        azureIoT.UpdateReportedProperties(twin);
    }
}

string getMaxMinReport(int rid, string payload)
{
    // The payload contains the parameter, in this case a date
    Debug.WriteLine($"Payload is a date: {payload}");
    // The expected answer is an object of this type, it is specific to your DTDL
    TemperatureReporting reporting = new() { avgTemp = 20, maxTemp = 42, minTemp = 12.34, startTime = DateTime.UtcNow.AddDays(-10), endTime = DateTime.UtcNow };
    return JsonConvert.SerializeObject(reporting);
}
