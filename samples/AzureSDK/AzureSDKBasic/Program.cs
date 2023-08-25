//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Azure.Devices.Client;
using nanoFramework.Azure.Devices.Shared;
using nanoFramework.Networking;
using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using nanoFramework.Json;
using System.Security.Cryptography.X509Certificates;
using nanoFramework.Hardware.Esp32;
using AzureSDKBasic;

const string DeviceID = "YourDeviceId";
const string IotBrokerAddress = "yourIOTHub.azure-devices.net";
const string SasKey = "the_SAS_token";
const string Ssid = "YouSsid";
const string Password = "YouWifiPassword";

bool ShoudIStop = false;

// If you haven't uploaded the Azure certificate into your device, use this line:
DeviceClient azureIoT = new DeviceClient(IotBrokerAddress, DeviceID, SasKey, azureCert: new X509Certificate(Resource.GetString(Resource.StringResources.AzureRootCerts)));
// Otherwise you can just use this line:
//DeviceClient azureIoT = new DeviceClient(IotBrokerAddress, DeviceID, SasKey);

try
{
wifiRetry:
    // Step 1: we must have a proper wifi connection
    if (!ConnectToWifi())
    {
        // If we are not properly connected we will rety. Waiting 1 second and then we will try again.
        // You may in a real project want to adjust this pattern a bit.
        Thread.Sleep(1000);
        Console.WriteLine("Trying again to connect to your wifi. Please make sure that the information is correct.");
        goto wifiRetry;
    }

    // Step 2: setting up events and call backs.
    // We are subscribing to twin update, statuc changes and cloud to device.
    // Adjust by removing what you don't need.
    azureIoT.TwinUpdated += TwinUpdatedEvent;
    azureIoT.StatusUpdated += StatusUpdatedEvent;
    azureIoT.CloudToDeviceMessage += CloudToDeviceMessageEvent;

    // Adding few direct methods as well.
    // Adjust by removing what you don't need.
    azureIoT.AddMethodCallback(MethodCalbackTest);
    azureIoT.AddMethodCallback(MakeAddition);
    azureIoT.AddMethodCallback(RaiseExceptionCallbackTest);

    // Step 3: trying to open Azure IoT
    ConnectAzureIot();

    // Ste 4: get the twin configuration.
    // Important: this require to have an Azure IoT 
    var twin = azureIoT.GetTwin(new CancellationTokenSource(5000).Token);
    if (twin == null)
    {
        // We will just display an error message here.
        Console.WriteLine($"Can't get the twins");
        // As an alternative, you can decide to close the connectin, go to sleep
        // And wake up after a while by uncommenting the following lines:
        //azureIoT.Close();
        //Thread.Sleep(100);
        //Sleep.EnableWakeupByTimer(new TimeSpan(0, 0, 0, 10));
        //Sleep.StartDeepSleep(); ;
    }

    Console.WriteLine($"Twin DeviceID: {twin.DeviceId}, #desired: {twin.Properties.Desired.Count}, #reported: {twin.Properties.Reported.Count}");

    // Step 5: we can report our twin
    TwinCollection reported = new TwinCollection();
    // This is just a basic example, you can adjust and report anything including real classes
    reported.Add("firmware", "myNano");
    reported.Add("sdk", 0.2);
    azureIoT.UpdateReportedProperties(reported);

    // Step 6: we will publish some telemetry
    while (!ShoudIStop)
    {
        // Step 6: publishing the telemetry
        // This is just a static example but in general, you will have a real sensor and you will publish the data
        // Note that the standard format used in Azure IoT is Json.
        if (azureIoT.IsConnected)
        {
            // We only publish if we are connected!
            Console.WriteLine("Sending telemetry to Azure IoT");
            azureIoT.SendMessage($"{{\"Temperature\":42,\"Pressure\":1023}}");
        }
        else
        {
            Console.WriteLine("We are not connected, can't send telemetry");
        }
        // Here we are waiting 20 seconds, you can adjust for any value. You can as well wait for a specific event.
        Thread.Sleep(20_000);
    }

    Console.WriteLine("A stop have been requested");
    Thread.Sleep(Timeout.Infinite);

}
catch (Exception ex)
{
    // This global try catch is to make sure whatever happen, we will safely be able to handle anything.
    Console.WriteLine($"Global exception: {ex.Message})");
    // In this example, we will sleep a bit and then reboot.
    CloseAndGoToSleep();
}

Thread.Sleep(Timeout.InfiniteTimeSpan);

void ConnectAzureIot()
{
azureOpenRetry:
    bool isOpen = azureIoT.Open();
    Console.WriteLine($"Connection is open: {isOpen}");
    if (!isOpen)
    {
        azureIoT.Close();
        // If we can't open, let's wait for 1 second and retry. You may want to adjust this time and used incremental waiting time.
        // You may in a real project want to adjust this pattern a bit.
        Thread.Sleep(1_000);
        Console.WriteLine("Trying to reconnect to Azure IoT. Please check that all the credentials are correct.");
        goto azureOpenRetry;
    }
}

bool ConnectToWifi()
{
    Console.WriteLine("Program Started, connecting to Wifi.");

    // As we are using TLS, we need a valid date & time
    // We will wait maximum 1 minute to get connected and have a valid date
    var success = WifiNetworkHelper.ConnectDhcp(Ssid, Password, requiresDateTime: true, token: new CancellationTokenSource(60_000).Token);
    if (!success)
    {
        Console.WriteLine($"Can't connect to wifi: {WifiNetworkHelper.Status}");
        if (WifiNetworkHelper.HelperException != null)
        {
            Console.WriteLine($"WifiNetworkHelper.HelperException");
        }
    }

    Console.WriteLine($"Date and time is now {DateTime.UtcNow}");
    return success;
}

void CloseAndGoToSleep()
{
    azureIoT?.Close();
    Thread.Sleep(1000);
    // Setup sleep for 10 seconds and then wake up.
    Sleep.EnableWakeupByTimer(new TimeSpan(0, 0, 0, 10));
    Sleep.StartDeepSleep();
}

void TwinUpdatedEvent(object sender, TwinUpdateEventArgs e)
{
    Console.WriteLine($"Twin update received:  {e.Twin.Count}");
}

void StatusUpdatedEvent(object sender, StatusUpdatedEventArgs e)
{
    Console.WriteLine($"Status changed: {e.IoTHubStatus.Status}, {e.IoTHubStatus.Message}");
    // You may want to reconnect or use a similar retry mechanism
    if (e.IoTHubStatus.Status == Status.Disconnected)
    {
        Console.WriteLine("Being disconnected from Azure IoT");
        // Trying to reconnect
        ConnectAzureIot();
    }
}

string MethodCalbackTest(int rid, string payload)
{
    Console.WriteLine($"Call back called :-) rid={rid}, payload={payload}");
    return "{\"Yes\":\"baby\",\"itisworking\":42}";
}

string MakeAddition(int rid, string payload)
{
    Hashtable variables = (Hashtable)JsonConvert.DeserializeObject(payload, typeof(Hashtable));
    int arg1 = (int)variables["arg1"];
    int arg2 = (int)variables["arg2"];
    return $"{{\"result\":{arg1 + arg2}}}";
}

string RaiseExceptionCallbackTest(int rid, string payload)
{
    throw new Exception("I got you, it's to test the 504");
}

void CloudToDeviceMessageEvent(object sender, CloudToDeviceMessageEventArgs e)
{
    Console.WriteLine($"Message arrived: {e.Message}");
    foreach (string key in e.Properties.Keys)
    {
        Debug.Write($"  Key: {key} = ");
        if (e.Properties[key] == null)
        {
            Console.WriteLine("null");
        }
        else
        {
            Console.WriteLine((string)e.Properties[key]);
        }
    }

    // Example of a simple stop message, in this case, we are stopping to send the telemetry.
    if (e.Message == "stop")
    {
        ShoudIStop = true;
    }
    else if (e.Message == "reboot42")
    {
        // Here, an example where we will just reboot the device.
        Console.WriteLine("Reboot requested");
        CloseAndGoToSleep();
    }
}
