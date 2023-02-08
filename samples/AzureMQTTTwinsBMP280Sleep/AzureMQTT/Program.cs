//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

///
/// NOTE: this demo uses the information outlined in https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-mqtt-support
///

using AzureMQTT;
using Iot.Device.Bmxx80;
using Microsoft.Extensions.Logging;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Json;
using nanoFramework.Logging.Serial;
using nanoFramework.Networking;
using System;
using System.Device.I2c;
using System.IO.Ports;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;

const string DeviceID = "nanoDeepSleep";
const string IotBrokerAddress = "YOURIOTHUB.azure-devices.net";
const string SasKey = "a valid SAS token";
const string Ssid = "yourWifi";
const string Password = "youWifiPassowrd";

string telemetryTopic = $"devices/{DeviceID}/messages/events/";
const string TwinReportedPropertiesTopic = "$iothub/twin/PATCH/properties/reported/";
const string TwinDesiredPropertiesTopic = "$iothub/twin/GET/";

// One minute unit
DateTime allupOperation = DateTime.UtcNow;
int sleepTimeMinutes = 60000;
int minutesToGoToSleep = 2;
SerialLogger logger = null;
ushort messageID = ushort.MaxValue;
bool twinReceived = false;
bool messageReceived = false;

try
{
    // Use Trace to show messages in serial COM2 as debug won't work when the device will wake up from deep sleep
    // Set the GPIO 16 and 17 for the serial port COM2
    Configuration.SetPinFunction(16, DeviceFunction.COM2_RX);
    Configuration.SetPinFunction(17, DeviceFunction.COM2_TX);
    SerialPort serial = new("COM2");
    serial.BaudRate = 115200;
    logger = new SerialLogger(ref serial, "My logger");
    logger.MinLogLevel = LogLevel.Debug;
    Trace("Program Started, connecting to Wifi.");

    // As we are using TLS, we need a valid date & time
    // We will wait maximum 1 minute to get connected and have a valid date
    CancellationTokenSource cs = new(sleepTimeMinutes);
    var success = WifiNetworkHelper.ConnectDhcp(Ssid, Password, requiresDateTime: true, token: cs.Token);
    if (!success)
    {
        Trace($"Can't connect to wifi: {NetworkHelper.Status}");
        if (WifiNetworkHelper.HelperException != null)
        {
            Trace($"WifiNetworkHelper.HelperException");
        }

        // This prevent to debug, once in deep sleep, you won't be able to connect to the device
        // So comment to, start with and check what's happening.
        GoToSleep();
    }

    // Reset the time counter if the previous date was not valid
    if (allupOperation.Year < 2018)
    {
        allupOperation = DateTime.UtcNow;
    }

    Trace($"Date and time is now {DateTime.UtcNow}");

    // nanoFramework socket implementation requires a valid root CA to authenticate with.
    // This can be supplied to the caller (as it's doing on the code bellow) or the Root CA has to be stored in the certificate store
    // Root CA for Azure from here: https://github.com/Azure/azure-iot-sdk-c/blob/master/certs/certs.c
    // We are storing this certificate in the resources

    // Keep in mind the current IoTHub "Hub root certificate" is near to expire 
    // Old Baltimore
    // X509Certificate azureRootCACert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.BaltimoreCyberTrustRoot));
    // New DigiCert
    X509Certificate azureRootCACert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DigiCertGlobalRootG2));

    // Creates MQTT Client with default port 8883 using TLS protocol
    MqttClient mqttc = new MqttClient(
        IotBrokerAddress,
        8883,
        true,
        azureRootCACert,
        null,
        MqttSslProtocols.TLSv1_2);

    // Handler for received messages on the subscribed topics
    mqttc.MqttMsgPublishReceived += ClientMqttMsgReceived;
    // Handler for publisher
    mqttc.MqttMsgPublished += ClientMqttMsgPublished;

    // Now connect the device
    MqttReasonCode code = mqttc.Connect(
        DeviceID,
        $"{IotBrokerAddress}/{DeviceID}/api-version=2020-09-30",
        GetSharedAccessSignature(null, SasKey, $"{IotBrokerAddress}/devices/{DeviceID}", new TimeSpan(24, 0, 0)),
        false,
        MqttQoSLevel.ExactlyOnce,
        false, "$iothub/twin/GET/?$rid=999",
        "Disconnected",
        false,
        60
        );

    //If we are connected, we can move forward
    if (mqttc.IsConnected)
    {
        Trace("subscribing to topics");
        mqttc.Subscribe(
            new[] {
                $"devices/{DeviceID}/messages/devicebound/#",
                "$iothub/twin/res/#"
            },
            new[] {
                    MqttQoSLevel.AtLeastOnce,
                    MqttQoSLevel.AtLeastOnce
            }
        );

        Trace("Getting twin properties");
        mqttc.Publish($"{TwinDesiredPropertiesTopic}?$rid={Guid.NewGuid()}", Encoding.UTF8.GetBytes(""), null, null, MqttQoSLevel.AtLeastOnce, false);

        CancellationTokenSource cstwins = new(10000);
        CancellationToken tokentwins = cstwins.Token;
        while (!twinReceived && !tokentwins.IsCancellationRequested)
        {
            tokentwins.WaitHandle.WaitOne(200, true);
        }

        if (tokentwins.IsCancellationRequested)
        {
            Trace("No twin received on time");
        }

        Trace("Sending twin properties");
        mqttc.Publish($"{TwinReportedPropertiesTopic}?$rid={Guid.NewGuid()}", Encoding.UTF8.GetBytes($"{{\"Firmware\":\"nanoFramework\",\"TimeToSleep\":{minutesToGoToSleep}}}"), null, null, MqttQoSLevel.AtLeastOnce, false);

        // I2C#	Data	Clock
        // I2C1	GPIO 18	GPIO 19
        const int busId = 1;
        I2cConnectionSettings i2cSettings = new(busId, Bmp280.DefaultI2cAddress);
        I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
        var i2CBmp280 = new Bmp280(i2cDevice);
        // set higher sampling
        i2CBmp280.TemperatureSampling = Sampling.LowPower;
        i2CBmp280.PressureSampling = Sampling.UltraHighResolution;

        var readResult = i2CBmp280.Read();
        // Print out the measured data
        Trace($"Temperature: {readResult.Temperature.DegreesCelsius}\u00B0C");
        Trace($"Pressure: {readResult.Pressure.Hectopascals}hPa");

        //Publish telemetry data
        messageID = mqttc.Publish(telemetryTopic, Encoding.UTF8.GetBytes($"{{\"Temperature\":{readResult.Temperature.DegreesCelsius},\"Pressure\":{readResult.Pressure.Hectopascals}}}"), null, null, MqttQoSLevel.ExactlyOnce, false);
        Trace($"Message ID for telemetry: {messageID}");

        // Wait for the message or cancel if waiting for too long
        CancellationToken token = new CancellationTokenSource(5000).Token;
        while (!messageReceived && !token.IsCancellationRequested)
        {
            token.WaitHandle.WaitOne(200, true);
        }

        if (token.IsCancellationRequested)
        {
            Trace("No telemetry confirmation received");
        }
    }
}
catch
{
    // We won't do anything
    // This global try catch is to make sure whatever happen, we will safely be able to go
    // To sleep
}


// This prevent to debug, once in deep sleep, you won't be able to connect to the device
// So comment to, start with and check what's happening.
// And un comment the next line
////Thread.Sleep(Timeout.Infinite);
// Just go to sleep when we will arrive at this point
GoToSleep();

void GoToSleep()
{
    Trace($"Full operation took: {DateTime.UtcNow - allupOperation}");
    Trace($"Set wakeup by timer for {minutesToGoToSleep} minutes to retry.");
    Sleep.EnableWakeupByTimer(new TimeSpan(0, 0, minutesToGoToSleep, 0));
    Trace("Deep sleep now");
    Sleep.StartDeepSleep();
}

void ClientMqttMsgReceived(object sender, MqttMsgPublishEventArgs e)
{
    try
    {
        Trace($"Message received on topic: {e.Topic}");
        string message = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
        Trace($"and message length: {message.Length}");

        if (e.Topic.StartsWith("$iothub/twin/res/204"))
        {
            Trace("and received confirmation for desired properties.");
        }
        else if (e.Topic.StartsWith("$iothub/twin/"))
        {
            if (e.Topic.IndexOf("res/400/") > 0 || e.Topic.IndexOf("res/404/") > 0 || e.Topic.IndexOf("res/500/") > 0)
            {
                Trace("and was in the error queue.");
            }
            else
            {
                Trace("and was in the success queue.");
                if (message.Length > 0)
                {
                    // skip if already received in this session
                    if (!twinReceived)
                    {
                        try
                        {
                            TwinProperties twin = (TwinProperties)JsonConvert.DeserializeObject(message, typeof(TwinProperties));
                            minutesToGoToSleep = twin.desired.TimeToSleep != 0 ? twin.desired.TimeToSleep : minutesToGoToSleep;
                            twinReceived = true;
                        }
                        catch
                        {
                            // We will ignore
                        }
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        Trace($"Exception in event: {ex}");
    }
}

void ClientMqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
{
    Trace($"Response from publish with message id: {e.MessageId}");
    if (e.MessageId == messageID)
    {
        messageReceived = true;
    }
}

string GetSharedAccessSignature(string keyName, string sharedAccessKey, string resource, TimeSpan tokenTimeToLive)
{
    // http://msdn.microsoft.com/en-us/library/azure/dn170477.aspx
    // the canonical Uri scheme is http because the token is not amqp specific
    // signature is computed from joined encoded request Uri string and expiry string

    var exp = DateTime.UtcNow.ToUnixTimeSeconds() + (long)tokenTimeToLive.TotalSeconds;

    string expiry = exp.ToString();
    string encodedUri = HttpUtility.UrlEncode(resource);

    var hmacsha256 = new HMACSHA256(Convert.FromBase64String(sharedAccessKey));
    byte[] hmac = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(encodedUri + "\n" + expiry));
    string sig = Convert.ToBase64String(hmac);

    if (keyName != null)
    {
        return String.Format(
        "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
        encodedUri,
        HttpUtility.UrlEncode(sig),
        HttpUtility.UrlEncode(expiry),
        HttpUtility.UrlEncode(keyName));
    }
    else
    {
        return String.Format(
            "SharedAccessSignature sr={0}&sig={1}&se={2}",
            encodedUri,
            HttpUtility.UrlEncode(sig),
            HttpUtility.UrlEncode(expiry));
    }
}

void Trace(string message)
{
    logger?.LogDebug(message);
}

