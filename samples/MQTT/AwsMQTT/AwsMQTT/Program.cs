using nanoFramework.Networking;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AwsMQTT
{

    public class Program
    {
        private static readonly string awsHost = "<endpoint>.<region>.amazonaws.com"; //make sure to add your AWS endpoint and region.
        private static readonly string clientId = Guid.NewGuid().ToString(); //This should really be persisted across reboots, but an auto generated GUID is fine for testing.
        private static readonly string clientRsaSha256Crt = //Device Certificate copied from AWS (this is a non working example)
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


        private static readonly string clientRsaKey = //Device private key copied from AWS (this is a non working example)
@"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAuIZoxC4TEjQSpjSkXPi58/5uerxBSgvjmKZ70VyTGeq7lNoi
YLjtrgpSVrqJ3+Pr2ozT0zmAE9y6iYBGNsHy4Rmgz3X60EWKWKUnRwAOUTlxycux
IZa5pPsIcGKEcqMGfUuIvA09neIoEIOBMfZY2R4ynG8i/r0rAWmstUn8P6xm2e/f
CWefSjaV2X1bRezkqUTEoqwH4W2aIZPAIebFMegYdaOjP6venpEsWyMv/BIluwwa
r+8sdmXRxfYkX+BYEwKgNUfB4A9qBKmdOHS/JLSUlKoZ7PAlR1DopAndkP6TPNev
8/6detAi1Z6k8dhRyndPck+M9vrKAxT9Xv2fLQIDAQABAoIBADXCw0dQU9Ib5csS
z0hfFx5lZJ7Rtlvyds8EwlJPHHSSlTBbFWUEvArW6wJlusHGT/MO0LBbslsXFin1
e398ply68MVA5GBFwnbtNzJSa9lyWRqoA+V7Wn8cvGqx6VDU+pEKrr3LRcZ4G6Ak
EEIUOfKX/7rgDlwVlTAGL9Fpytppytb86AHItiTbMrh8cj8bGqTBMBBex0Dnf0W7
qtSDrTVN8X6xHTxgjCvwZGpHLYOsADif3Y364I/BEp2QVAsADTDXAmO8KkfTbXys
8+E3odUNqrugYl4Zd/TSCgNlDJT4mySmHLxSOX7wFlg9zdemugrBWFmmvQB2mrBZ
SLoTRMECgYEA8/qOfoqc2VAV35/BY7zGPMFq94iisKjUKCAVKXbBS47/bKOWEAr6
KLZ9Y+00KbB6JQTiwyKDlRa8FrVQGoAdkqgLtmrMB8hFPjFMXuFSdip2sYgUpmmq
a83Ixv1ndHUtN2FQ6H6ep+mVHcEVxD4gwTG0TJIuka6cynFD0+o5EUkCgYEAwZ3t
2IkUTNxfTZJr8Sjr9ju6vSBF5LMh7gi/qQi/7ntQnv6cZCBDp9lgEOaNO/Cr7XNB
9Ju07BmFSNpIU5HjwC1Ql4pO61oGpccM5jK54Z/bKHSAq37SxOTYIqaRCPNrPzPd
NhA3hm57+YTHb3GqMDtLPe9YUYCu77xz6awqwsUCgYABEJDaoIQ6tozB4xKW+tXq
ofVzixcaqkHywuEUwz3otIEM2lHnVATvdIxriTd3DxwZWUIcE+R7HBRKDRSCaRi3
8R/L9f9Z8VfMA68PovAXL+xArhVY/JEP02AS7jwrV++QlE4kFZlfpjdOX+9WYecG
sM7WnrSxUg/BGmlvXVBOcQKBgHhOO4X610RTAlzfCHdW6BeeUZBGx2ct731KrlzH
9QqoURYaOu70JVXcehbGSyfdidcHcQoe3jJ+QRVdnOdglVXKUnN3G0aeL9c+ccNv
7ZRGkhT3HyRwr2Jsl+gf+6rGJfOltGRtezLq39nRKWMUC53gmgYn/IbYINsSDCw8
MG3JAoGBAJBEoBbSDqt/3jmSzo4TP7wkHjABtwDdDfinNGYelXzQvlsxTO3O9s4G
cROQClDCjQM6He0G4lN8Q2RWxNMYICc+x2Ts6W1ufGxnoewO5qp/62ojCza33EW9
p00NvgdEFrMJ7MvHQ053np3fDZ5x5c/Lc4AVpuHXvwID86I/Lux6
-----END RSA PRIVATE KEY-----
";
        private static MqttClient client;


        public static void Main()
        {

            // if we are using TLS it requires valid date & time
            NetworkHelpers.SetupAndConnectNetwork(true);

            Console.WriteLine("Waiting for network up and IP address...");
            NetworkHelpers.IpAddressAvailable.WaitOne();

            Console.WriteLine("Waiting for valid Date & Time...");
            NetworkHelpers.DateTimeAvailable.WaitOne();

            SetupMqtt();
        
            Thread.Sleep(Timeout.Infinite);
        }

        static void SetupMqtt()
        {
            X509Certificate caCert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.AwsCAroot));
            X509Certificate2 clientCert = new X509Certificate2(clientRsaSha256Crt, clientRsaKey, ""); //make sure to add a correct pfx certificate

            client = new MqttClient(awsHost, 8883, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);

            // register to message received 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            client.Connect(clientId);

            // subscribe to the topic with QoS 2 
            client.Subscribe(new string[] { "devices/nanoframework/sys" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            Thread telemetryThread = new Thread(new ThreadStart(TelemetryLoop));
            telemetryThread.Start();

        }

        static void TelemetryLoop()
        {
            while (true)
            {
                string SampleData = $"{{\"MQTT on Nanoframework\" : {DateTime.UtcNow.ToString("u")}}}";
                client.Publish("devices/nanoframework/data", Encoding.UTF8.GetBytes(SampleData), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
                System.Console.WriteLine("Message sent: " + SampleData);
                Thread.Sleep(3000);
            }
        }

        static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string Message = new string(Encoding.UTF8.GetChars(e.Message));
            System.Console.WriteLine("Message received: " + Message);
        }
    }
}
