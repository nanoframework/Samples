///
/// NOTE: this demo uses the information outlined in https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-mqtt-support
///

using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Security.Cryptography;

using nanoFramework.Runtime.Events;
using nanoFramework.Networking;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;




namespace AzureMQTT
{
    public partial class Program
    {

        static readonly string deviceID = "[enter device id]";
        static readonly string iotBrokerAddress = "[enter iothub url]";
        static readonly string SasKey = "[enter sas key]";

        static string telemetryTopic = "";
        const string twinReportedPropertiesTopic = "$iothub/twin/PATCH/properties/reported/";
        const string twinDesiredPropertiesTopic = "$iothub/twin/GET/";

        //private static bool timeSynchronized;

        // This method is run when the mainboard is powered up or reset.   
        public static void Main()
        {
            // Use Trace to show messages in Visual Studio's "Output" window during debugging.
            Trace("Program Started");

            telemetryTopic = String.Format("devices/{0}/messages/events/", deviceID);

            // if we are using TLS it requires date & time
            NetworkHelpers.SetupAndConnectNetwork(true);

            Console.WriteLine("Waiting for network up and IP address...");
            NetworkHelpers.IpAddressAvailable.WaitOne();


            Console.WriteLine("Waiting for valid Date & Time...");
            NetworkHelpers.DateTimeAvailable.WaitOne();

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
            //Create MQTT Client with default port 8883 using TLS protocol
            MqttClient mqttc = new MqttClient(iotBrokerAddress, 8883, true, null, null, MqttSslProtocols.TLSv1_2);


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


            byte code = mqttc.Connect(
                deviceID,
                String.Format("{0}/{1}/api-version=2018-06-30", iotBrokerAddress, deviceID),
                GetSharedAccessSignature(null, SasKey, String.Format("{0}/devices/{1}", iotBrokerAddress, deviceID), new TimeSpan(24, 0, 0)),
                false,
                MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
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
                    MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
                    MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
                    MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
                    MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE
                    }
                    );


                Trace("Sending twin properties");
                mqttc.Publish(String.Format("{0}?$rid={1}", twinReportedPropertiesTopic, Guid.NewGuid()), Encoding.UTF8.GetBytes("{ \"Firmware\": \"NetMF\"}"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);


                Trace("Getting twin properties");
                mqttc.Publish(String.Format("{0}?$rid={1}", twinDesiredPropertiesTopic, Guid.NewGuid()), Encoding.UTF8.GetBytes(""), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);


                Trace("[MQTT Client] Start to send telemetry");
            }

            int temp = 10;
            while (mqttc.IsConnected)
            {

                // get temperature value... 
                temp = temp + 1;
                // ...publish it to the broker 



                //Publish telemetry data using AT LEAST ONCE QOS Level
                mqttc.Publish(telemetryTopic, Encoding.UTF8.GetBytes("{ Temperature: " + temp + "}"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);

                Trace(String.Format("{0} [MQTT Client] Sent telemetry { Temperature: {1} }", DateTime.UtcNow.ToString(), temp.ToString()));

                Thread.Sleep(1000 * 60);
                if (temp > 30) temp = 10;

            }

            Trace(String.Format("{0} [MQTT Client]" + " is Disconnected", DateTime.UtcNow.ToString()));
        }


        [Conditional("DEBUG")]
        static void Trace(string message)
        {
            if (Debugger.IsAttached)
                Console.WriteLine(message);
        }

    }
}
