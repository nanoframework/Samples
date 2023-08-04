//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

///
/// NOTE: this demo uses the information outlined in https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-mqtt-support
///

using nanoFramework.Networking;
using nanoFramework.Runtime.Events;
using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;
using AdvancedExample.Azure;

#if HAS_WIFI
using System.Device.Wifi;
#endif

namespace AzureMQTT
{
    public partial class Program
    {
        static readonly string deviceID = "[enter device id]";
        static readonly string iotBrokerAddress = "[enter iothub url]";
        static readonly string SasKey = "[enter sas key]";

#if HAS_WIFI
        private static string MySsid = "ssid";
        private static string MyPassword = "password";      
#endif
        static string telemetryTopic = "";
        const string twinReportedPropertiesTopic = "$iothub/twin/PATCH/properties/reported/";
        const string twinDesiredPropertiesTopic = "$iothub/twin/GET/";

        // This method is run when the mainboard is powered up or reset.   
        public static void Main()
        {
            // Use Trace to show messages in Visual Studio's "Output" window during debugging.
            Trace("Program Started");

            telemetryTopic = String.Format("devices/{0}/messages/events/", deviceID);

            Debug.WriteLine("Waiting for network up and IP address...");
            bool success;
            CancellationTokenSource cs = new(60000);
#if HAS_WIFI
            success = WifiNetworkHelper.ConnectDhcp(MySsid, MyPassword, requiresDateTime: true, token: cs.Token);
#else
            success = NetworkHelper.SetupAndConnectNetwork(cs.Token, true);
#endif
            if (!success)
            {
                Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {WifiNetworkHelper.Status}.");
                if (WifiNetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"Exception: {WifiNetworkHelper.HelperException}");
                }
                return;
            }

            Thread.Sleep(3000); //used to reliably allow redeployment in VS2019

            DoMqttStuff();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Client_MqttMsgReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Trace(String.Format("Message received on topic: {0}", e.Topic));
            Trace(String.Format("The message was: {0}", new string(Encoding.UTF8.GetChars(e.Message))));

            if (e.Topic.StartsWith("$iothub/twin/PATCH/properties/desired/"))
            {
                Trace("and received desired properties.");
            }
            else if (e.Topic.StartsWith("$iothub/twin/"))
            {

                if (e.Topic.IndexOf("res/400/") > 0 || e.Topic.IndexOf("res/404/") > 0 || e.Topic.IndexOf("res/500/") > 0)
                    Trace("and was in the error queue.");
                else
                    Trace("and was in the success queue.");
            }
            else if (e.Topic.StartsWith("$iothub/methods/POST/"))
            {
                Trace("and was a method.");
            }
            else if (e.Topic.StartsWith(String.Format("devices/{0}/messages/devicebound/", deviceID)))
            {
                Trace("and was a message for the device.");
            }
            else if (e.Topic.StartsWith("$iothub/clientproxy/"))
            {
                Trace("and the device has been disconnected.");
            }
            else if (e.Topic.StartsWith("$iothub/logmessage/Info"))
            {
                Trace("and was in the log message queue.");
            }
            else if (e.Topic.StartsWith("$iothub/logmessage/HighlightInfo"))
            {
                Trace("and was in the Highlight info queue.");
            }
            else if (e.Topic.StartsWith("$iothub/logmessage/Error"))
            {
                Trace("and was in the logmessage error queue.");
            }
            else if (e.Topic.StartsWith("$iothub/logmessage/Warning"))
            {
                Trace("and was in the logmessage warning queue.");
            }
        }

        private static void Client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Trace(String.Format("Response from publish with message id: {0}", e.MessageId.ToString()));
        }

        private static void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Trace(String.Format("Response from subscribe with message id: {0}", e.MessageId.ToString()));
        }
        private static void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Trace(String.Format("Response from unsubscribe with message id: {0}", e.MessageId.ToString()));
        }

        private static void Client_ConnectionClosed(object sender, EventArgs e)
        {
            Trace("Connection closed");
        }


        static string GetSharedAccessSignature(string keyName, string sharedAccessKey, string resource, TimeSpan tokenTimeToLive)
        {
            // http://msdn.microsoft.com/en-us/library/azure/dn170477.aspx
            // the canonical Uri scheme is http because the token is not amqp specific
            // signature is computed from joined encoded request Uri string and expiry string

            var exp = DateTime.UtcNow.ToUnixTimeSeconds() + (long)tokenTimeToLive.TotalSeconds;

            string expiry = exp.ToString();
            string encodedUri = WebUtility.UrlEncode(resource);

            
            byte[] hmac = SHA.computeHMAC_SHA256(Convert.FromBase64String(sharedAccessKey), Encoding.UTF8.GetBytes(encodedUri + "\n" + expiry));
            string sig = Convert.ToBase64String(hmac);

            if (keyName != null)
            {
                return String.Format(
                "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                encodedUri,
                WebUtility.UrlEncode(sig),
                WebUtility.UrlEncode(expiry),
                WebUtility.UrlEncode(keyName));
            }
            else
            {
                return String.Format(
                    "SharedAccessSignature sr={0}&sig={1}&se={2}",
                    encodedUri,
                    WebUtility.UrlEncode(sig),
                    WebUtility.UrlEncode(expiry));
            }
        }




        private static void DoMqttStuff()
        {
            // nanoFramework socket implementation requires a valid root CA to authenticate with a
            // this can be supplied to the caller (as it's doing on the code bellow) or the Root CA has to be stored in the certificate store
            // Root CA for Azure from here: https://github.com/Azure/azure-iot-sdk-c/blob/master/certs/certs.c

            X509Certificate azureRootCACert = new X509Certificate(Resource.GetString(Resource.StringResources.AzureRootCerts));

            //Create MQTT Client with default port 8883 using TLS protocol
            MqttClient mqttc = new MqttClient(
                iotBrokerAddress, 
                8883, 
                true, 
                azureRootCACert, 
                null, 
                MqttSslProtocols.TLSv1_2);

            // event when connection has been dropped
            mqttc.ConnectionClosed += Client_ConnectionClosed;

            // handler for received messages on the subscribed topics
            mqttc.MqttMsgPublishReceived += Client_MqttMsgReceived;

            // handler for publisher
            mqttc.MqttMsgPublished += Client_MqttMsgPublished;

            // handler for subscriber 
            mqttc.MqttMsgSubscribed += Client_MqttMsgSubscribed;

            // handler for unsubscriber
            mqttc.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;

            MqttReasonCode code = mqttc.Connect(
                deviceID,
                String.Format("{0}/{1}/api-version=2018-06-30", iotBrokerAddress, deviceID),
                GetSharedAccessSignature(null, SasKey, String.Format("{0}/devices/{1}", iotBrokerAddress, deviceID), new TimeSpan(24, 0, 0)),
                false,
                MqttQoSLevel.AtLeastOnce,
                false, "$iothub/twin/GET/?$rid=999",
                "Disconnected",
                false,
                60
                );

            if (mqttc.IsConnected)
            {
                Trace("subscribing to topics");
                mqttc.Subscribe(
                    new[] {
                "$iothub/methods/POST/#",
                String.Format("devices/{0}/messages/devicebound/#", deviceID),
                "$iothub/twin/PATCH/properties/desired/#",
                "$iothub/twin/res/#"
                    },
                    new[] {
                    MqttQoSLevel.AtLeastOnce,
                    MqttQoSLevel.AtLeastOnce,
                    MqttQoSLevel.AtLeastOnce,
                    MqttQoSLevel.AtLeastOnce
                    }
                    );


                Trace("Sending twin properties");
                mqttc.Publish(String.Format("{0}?$rid={1}", twinReportedPropertiesTopic, Guid.NewGuid()), Encoding.UTF8.GetBytes("{ \"Firmware\": \"nanoFramework\"}"), null, null, MqttQoSLevel.AtLeastOnce, false);


                Trace("Getting twin properties");
                mqttc.Publish(String.Format("{0}?$rid={1}", twinDesiredPropertiesTopic, Guid.NewGuid()), Encoding.UTF8.GetBytes(""), null, null, MqttQoSLevel.AtLeastOnce, false);


                Trace("[MQTT Client] Start to send telemetry");
            }

            int temp = 10;
            while (mqttc.IsConnected)
            {

                // get temperature value... 
                temp = temp + 1;
                // ...publish it to the broker 



                //Publish telemetry data using AT LEAST ONCE QOS Level
                mqttc.Publish(telemetryTopic, Encoding.UTF8.GetBytes("{ Temperature: " + temp + "}"), null, null, MqttQoSLevel.AtLeastOnce, false);

                Trace(String.Format("{0} [MQTT Client] Sent telemetry {{ Temperature: {1} }}", DateTime.UtcNow.ToString("u"), temp.ToString()));

                Thread.Sleep(1000 * 60);
                if (temp > 30) temp = 10;

            }

            Trace(String.Format("{0} [MQTT Client]" + " is Disconnected", DateTime.UtcNow.ToString("u")));
        }


        //[Conditional("DEBUG")]
        static void Trace(string message)
        {
            Debug.WriteLine(message);
        }

    }
}
