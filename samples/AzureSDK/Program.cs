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
using Iot.Device.Bmxx80;
using System.Device.I2c;

const string DeviceID = "nanoEdgeTwin";
const string IotBrokerAddress = "youriothub.azure-devices.net";
const string SasKey = "yoursaskey";
const string Ssid = "your wifi";
const string Password = "your wifi password";

// One minute unit
int sleepTimeMinutes = 60000;

// If you don't have a BMP280, comment this part:
const int busId = 1;
I2cConnectionSettings i2cSettings = new(busId, Bmp280.DefaultI2cAddress);
I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
Bmp280 bmp280 = new Bmp280(i2cDevice);
// Up to here!

bool ShoudIStop = false;

DeviceClient azureIoT = new DeviceClient(IotBrokerAddress, DeviceID, SasKey, 0);

try
{
    if (!ConnectToWifi()) return;

    azureIoT.TwinUpated += TwinUpdatedEvent;
    azureIoT.StatusUpdated += StatusUpdatedEvent;
    azureIoT.CloudToDeviceMessage += CloudToDeviceMessageEvent;
    azureIoT.AddMethodCallback(MethodCalbackTest);
    azureIoT.AddMethodCallback(MakeAddition);
    azureIoT.AddMethodCallback(RaiseExceptionCallbackTest);
    var isOpen = azureIoT.Open();
    Debug.WriteLine($"Connection is open: {isOpen}");

    var twin = azureIoT.GetTwin(new CancellationTokenSource(20000).Token);
    if (twin == null)
    {
        Debug.WriteLine($"Can't get the twins");
        azureIoT.Close();
        return;
    }

    Debug.WriteLine($"Twin DeviceID: {twin.DeviceId}, #desired: {twin.Properties.Desired.Count}, #reported: {twin.Properties.Reported.Count}");

    TwinCollection reported = new TwinCollection();
    reported.Add("firmware", "myNano");
    reported.Add("sdk", 0.2);
    azureIoT.UpdateReportedProperties(reported);

    // set higher sampling
    bmp280.TemperatureSampling = Sampling.LowPower;
    bmp280.PressureSampling = Sampling.UltraHighResolution;

    while (!ShoudIStop)
    {
        // If you don't have a BMP280, comment this part:
        var values = bmp280.Read();
        azureIoT.SendMessage($"{{\"Temperature\":{values.Temperature.DegreesCelsius},\"Pressure\":{values.Pressure.Hectopascals}}}");
        // Up to here!
        // And uncomment the following line:
        // azureIoT.SendMessage($"{{\"Temperature\":42,\"Pressure\":1023}}");
        Thread.Sleep(20000);
    }

}
catch (Exception ex)
{
    // We won't do anything
    // This global try catch is to make sure whatever happen, we will safely be able to
    // reboot or do anything else.
    Debug.WriteLine(ex.ToString());
}

Thread.Sleep(Timeout.InfiniteTimeSpan);

bool ConnectToWifi()
{
    Debug.WriteLine("Program Started, connecting to WiFi.");

    // As we are using TLS, we need a valid date & time
    // We will wait maximum 1 minute to get connected and have a valid date
    var success = NetworkHelper.ConnectWifiDhcp(Ssid, Password, setDateTime: true, token: new CancellationTokenSource(sleepTimeMinutes).Token);
    if (!success)
    {
        Debug.WriteLine($"Can't connect to wifi: {NetworkHelper.ConnectionError.Error}");
        if (NetworkHelper.ConnectionError.Exception != null)
        {
            Debug.WriteLine($"NetworkHelper.ConnectionError.Exception");
        }
    }

    Debug.WriteLine($"Date and time is now {DateTime.UtcNow}");
    return success;
}

void TwinUpdatedEvent(object sender, TwinUpdateEventArgs e)
{
    Debug.WriteLine($"Twin update received:  {e.Twin.Count}");
}

void StatusUpdatedEvent(object sender, StatusUpdatedEventArgs e)
{
    Debug.WriteLine($"Status changed: {e.IoTHubStatus.Status}, {e.IoTHubStatus.Message}");
    // You may want to reconnect or use a similar retry mechanism
    if (e.IoTHubStatus.Status == Status.Disconnected)
    {
        Debug.WriteLine("Stoppped!!!");
    }
}

string MethodCalbackTest(int rid, string payload)
{
    Debug.WriteLine($"Call back called :-) rid={rid}, payload={payload}");
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
    Debug.WriteLine($"Message arrived: {e.Message}");
    foreach (string key in e.Properties.Keys)
    {
        Debug.Write($"  Key: {key} = ");
        if (e.Properties[key] == null)
        {
            Debug.WriteLine("null");
        }
        else
        {
            Debug.WriteLine((string)e.Properties[key]);
        }
    }

    if (e.Message == "stop")
    {
        ShoudIStop = true;
    }
}
