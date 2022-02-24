using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.ReadResult;
using nanoFramework.Azure.Devices.Client;
using nanoFramework.Azure.Devices.Shared;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Networking;
using System;
using System.Device.I2c;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

const string DeviceID = "nanoEdgeTwin";
const string IotHubAddress = "youriothub.azure-devices.net";
const string SasKey = "yoursaskey";
const string Ssid = "your wifi";
const string Password = "your wifi password";

int sleepTimeMinutes = 60000;
int minutesToGoToSleep = 2;

try
{
    // Measuring the temperature and pressure
    Bmp280ReadResult readResult = null;
    // On an ESP32
    // I2C#	Data	Clock
    // I2C1	GPIO 18	GPIO 19
    const int busId = 1;
    I2cConnectionSettings i2cSettings = new(busId, Bmp280.DefaultI2cAddress);
    I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
    var i2CBmp280 = new Bmp280(i2cDevice);
    // set higher sampling
    i2CBmp280.TemperatureSampling = Sampling.LowPower;
    i2CBmp280.PressureSampling = Sampling.UltraHighResolution;

    readResult = i2CBmp280.Read();

    ConnectToWifi();
    // If you have uploaded the Azure Certificate on the device, just use:
    //DeviceClient azureIoT = new(IotHubAddress, DeviceID, SasKey);
    // Otherwise, use:
    DeviceClient azureIoT = new(IotHubAddress, DeviceID, SasKey, azureCert: new X509Certificate(AzureSDKSleepBMP280.Resources.GetBytes(AzureSDKSleepBMP280.Resources.BinaryResources.AzureRoot)));
    azureIoT.Open();

    // Gets the twin
    var twin = azureIoT.GetTwin(new CancellationTokenSource(5000).Token);
    if ((twin != null) && (twin.Properties.Desired.Contains("TimeToSleep")))
    {
        minutesToGoToSleep = (int)twin.Properties.Desired["TimeToSleep"];
    }

    // Report the twins
    TwinCollection reported = new();
    reported.Add("TimeToSleep", minutesToGoToSleep);
    reported.Add("Firmware", "nanoFramework");
    azureIoT.UpdateReportedProperties(reported, new CancellationTokenSource(5000).Token);

    if (readResult != null)
    {
        //Publish telemetry data if we have a measure
        azureIoT.SendMessage($"{{\"Temperature\":{readResult.Temperature.DegreesCelsius},\"Pressure\":{readResult.Pressure.Hectopascals}}}", new CancellationTokenSource(2000).Token);
    }
}
catch
{
    // We won't do anything
    // This global try catch is to make sure whatever happen, we will safely be able to go
    // To sleep
}

// Just go to sleep when we will arrive at this point
GoToSleep();

void GoToSleep()
{
    Sleep.EnableWakeupByTimer(TimeSpan.FromMinutes(minutesToGoToSleep));
    Sleep.StartDeepSleep();
}

void ConnectToWifi()
{
    // As we are using TLS, we need a valid date & time
    // We will wait maximum 1 minute to get connected and have a valid date
    CancellationTokenSource cs = new(sleepTimeMinutes);
    var success = WiFiNetworkHelper.ConnectDhcp(Ssid, Password, requiresDateTime: true, token: cs.Token);
    if (!success)
    {
        GoToSleep();
    }
}
