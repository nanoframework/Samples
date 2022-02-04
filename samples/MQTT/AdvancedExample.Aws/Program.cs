//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

// !!!----------- SAMPLE - ENSURE YOU CHOOSE THE CORRECT NETWORK TYPE HERE --------------!!!
// #define HAS_WIFI // Uncomment if you use WiFi instead of Ethernet.
// !!!-----------------------------------------------------------------------------------!!!

using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;

#if HAS_WIFI
using Windows.Devices.WiFi;
#endif

namespace AwsMQTT
{
    public class Program
    {
#if HAS_WIFI
        private static string MySsid = "ssid";
        private static string MyPassword = "password";      
#endif
        ////////////////////////////////////////////////////////////////////////////////
        // make sure to add your AWS endpoint and region!!! 
        private static readonly string awsHost = "<endpoint>.<region>.amazonaws.com";
        ////////////////////////////////////////////////////////////////////////////////

        //This should really be persisted across reboots, but an auto generated GUID is fine for testing.
        private static readonly string clientId = Guid.NewGuid().ToString();

        //////////////////////////////////////////////////////////////////////////////////
        // make sure to respect the formating bellow to have a correct certificate & key
        // - NO identation
        // - start tag '-----BEGIN(...)' right after the string open double quote
        // - end tag '-----END(...)' immediately before the string closing double quote
        //////////////////////////////////////////////////////////////////////////////////

        //Device Certificate copied from AWS (this is a non working example)
        private static readonly string clientRsaSha256Crt =
@"-----BEGIN CERTIFICATE-----
MIIDWTCCAkGgAwIBAgIUc6HFS8S8bwgCRVvMKLuebp6I1f0wDQYJKoZIhvcNAQEL
BQAwTTFLMEkGA1UECwxCQW1hem9uIFdlYiBTZXJ2aWNlcyBPPUFtYXpvbi5jb20g
SW5jLiBMPVNlYXR0bGUgU1Q9V2FzaGluZ3RvbiBDPVVTMB4XDTE5MDEyNDE1NTcw
NFoXDTQ5MTIzMTIzNTk1OVowHjEcMBoGA1UEAwwTQVdTIElvVCBDZXJ0aWZpY2F0
ZTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBALiGaMQeExI0EqY0pFz4
ufP+bnq8QUoL45ime9Fckxnqu5TaImC47a4KUla6id/j69qM09M5gBPcuomARjbB
8uEZoM91+tBFililJ0cADlE5ccnLsSGWuaT7CHBihHKjBn1LiLwNPZ3iKBCDgTH2
WNkeMpxvIv69KwFprLVJ/D+sZtnv3wlnn0o2ldl9W0Xs5KlExKKsB+FtmiGTwCHm
xTHoGHWjoz+r3p6RLFsjL/wSJbsMGq/vLHZl0cX2JF/gWBMCoDVHweAPagSpnTh0
vyS0lJSqGezwJUdQ6KQJ3ZD+kzzXr/P+nXrQItWepPHYUcp3T3JPjPb6ygMU/V79
ny0CAwEAAaNgMF4wHwYDVR0jBBgwFoAUE9W40t+6tZ281RkUSL7Y73NUNqAwHQYD
VR0OBBYEFAATQTjtjLRtN+TVr7OdEUwL01TOMAwGA1UdEwEB/wQCMAAwDgYDVR0P
AQH/BAQDAgeAMA0GCSqGSIb3DQEBCwUAA4IBAQBbzCI7EgcarrfZdzTmgwuN1OTA
sFfKMFqkUgVFmzLJ5H4y/ly/KJETkHaEwJuxk4IWKAibpMxRniUULCv9MCnOSGQx
2Mm3iq68+N6RK5Dxq/mlzhzFFXqKr5oKK7RCXkJv3/eTPDxSWxcqofj1ZXPN2Clf
JqrEG7khbMN55htBNVfcDAozw/VbOVCZS+hlcLTQTBR/RPel8odDsVnLcGLBcDEx
SWARVMI0+jWe39zbsapLmjjdAwOdj042+UqdiDUv/26EUdOYImmt2NLFmDY9r3sd
BmoSHrBhVE6z/qdjOCOeUsJ/pzXc34y8OIxFdwlTIfRQx8S3espqc4HpRQCx
-----END CERTIFICATE-----";

        //Device private key copied from AWS (this is a non working example)
        private static readonly string clientRsaKey =
@"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAzZwEiJh/cHTDsBKT9mqgB6eYMkoycBWn3CNrRQ2sNo24my9C
3GfmyXsDhmXNxlLsnEitXJmbFThe5q3wvPreXNZgjgUA/VpXoprdCH+5NCoEgvYK
zznHlE0BUViZymqytfW5OugK0QIUI9E+74nCmi/C/XnlPY46FdFHsjwjTYfB0oFS
P7JfN15mTu7ortPmAoDUA+wPQZpGvZJvB6NktetAsQTmRMWuaX+Hgi6TItmcAe/1
MSUdzLPY/B/nCczj/buZyV5sqfawYPt0o5usEwxeJ3P7id7S0aEFjZBvNkq3cuRP
Zi3cO9yM/KexCiJNq/n2SvxHiRCgY1i80YIxgQIDAQABAoIBAAPj59IN0Jt4GhvC
vjnzWoPKj/6jmMC2KC7qHKV51MBTfiKxijpRXPSC91YbpwERoJh0Z5NQ9LY6EtGa
iOuKc5qeE8WcIqCojO/uri/y+rYZ9Wvk699v4G7V5ih73K0Px8HZnF+Y9FtwBqGG
3AH8QUpZjP7ux3+aqU6wXwLoUGKvHjT3lBhVI6thKAGPiVyCHKW1ZjP4VtdVq09F
OYcW3jVjWaWJBZfPkbcTgULRCojzlk9EcF/wNVi+k6t0LP++dioqXxUP7ksrE2Us
IZjOeN0sr1wo4aQahV0RUBYdEux78E1tPxo+kSZSUFb1xu0x1/Dcupyuiw8kxgAb
MMi9OIECgYEA6GNeEs3SQIbzgbLdNKI5+ySv0SuImFy33AwgSRx3X5UOA7Ub+4r3
f2jEUdFE6+AMfR4q7M3g/Xw9wQhku2eC7CDmQcLuvLfAaIgQehXeGWZqQylyPSl5
s7KR/pFLiGOiQzc72zHPHYs4BW5d9llfcvtuR7LUy1KfkWeWuNhetaUCgYEA4oAc
bA6GG0deofv1Bct4pYkpZAH61Rp03UlIsWpDs1IXPnPtOdTRK/qy4c4qnPCbuY1R
fmZy7pG3Pyvq+Ua8XHtHDCr5ut4cJsOLcHNM6xvMHHgtI79UTXOQcM6cORT1F3X4
cqh20YAH5xYb66AWBrl9wHiI+91KAmoriPFK3a0CgYEA40V7FWzReWYB9BBXokgd
6G4ivLCUsF3NOplpYddDL+l4gUu4iDOKhcKSbWn6u6ysyhic5mca6Q1+37Azw8wi
EIjEaAAat9oFhLW9V4jXY4Pz3KdGIGbVrVawzYSPmF3IrW/xTBfUdRJYwYcEwg75
+FvJqLlOv2KYx/3FPBXv2jkCgYEA2qe4SGyA9Cai6ZdVQ8HYd12BUqVCo6UFunY7
seIW9y7Bd63sDk8vmthLBgfERXtVqfwN9wsp2rta/qYEEZ9CybjMrqdyK/6tiJJv
sx/r2nAcTEOLuB3FYXu1reEXGVfs/zgIn4+YHMkPV/uU+pOxj85T4pG6FALdppUd
7/aYQoUCgYBga6jUIadvsAzsq+4kWYrtcBf6jE5Rq6s/yPy8UE8zwT6nLlveOvdz
/JU9knOXn6S2d0L1NRG5PCVtY6ArmWTaOLBJGJ2fH/rNn9wpwta92JbxsrMaIF1j
lIBzJXkrbY11FY6TNX4YFU2McIX2Ge0058Pozx6tumJ4KxvB9Ges8g==
-----END RSA PRIVATE KEY-----";


        private static MqttClient client;

        public static void Main()
        {
            SetupNetwork();

            Debug.WriteLine($"Time after network available: {DateTime.UtcNow.ToString("o")}");

            SetupMqtt();

            Thread.Sleep(Timeout.Infinite);
        }

        static void SetupNetwork()
        {
            Debug.WriteLine("Waiting for network up and IP address...");
            bool success;
            CancellationTokenSource cs = new(60000);
#if HAS_WIFI
            success = WiFiNetworkHelper.ScanAndConnectDhcp(MySsid, MyPassword, requiresDateTime: true, token: cs.Token);
#else
            success = NetworkHelper.SetupAndConnectNetwork(requiresDateTime: true, token: cs.Token);
#endif
            if (!success)
            {
                Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {NetworkHelper.Status}.");
                if (NetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"Exception: {NetworkHelper.HelperException}");
                }
                return;
            }
        }

        static void SetupMqtt()
        {
            // setup CA root certificate and...
            X509Certificate caCert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.AwsCAroot));

            // ... client certificate
            // make sure to add a correct pfx certificate, including the RSA key
            X509Certificate2 clientCert = new X509Certificate2(clientRsaSha256Crt, clientRsaKey, "");

            // TLS 1.2 is mandatory for AWS
            client = new MqttClient(awsHost, 8883, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);

            // subscribe handler for message received 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            // connect MQTT client
            client.Connect(clientId);

            // subscribe to the topic with QoS 2 
            client.Subscribe(new string[] { "devices/nanoframework/sys" }, new MqttQoSLevel[] { MqttQoSLevel.AtMostOnce });

            // launch telemetry thread
            Thread telemetryThread = new Thread(new ThreadStart(TelemetryLoop));
            telemetryThread.Start();
        }

        static void TelemetryLoop()
        {
            while (true)
            {
                string SampleData = $"{{\"MQTT on Nanoframework\" : {DateTime.UtcNow.ToString("u")}}}";
                client.Publish("devices/nanoframework/data", Encoding.UTF8.GetBytes(SampleData), MqttQoSLevel.AtMostOnce, false);

                Debug.WriteLine("Message sent: " + SampleData);

                Thread.Sleep(3000);
            }
        }

        static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string Message = new string(Encoding.UTF8.GetChars(e.Message));
            Debug.WriteLine("Message received: " + Message);
        }
    }
}
