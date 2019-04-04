//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Networking;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace HttpSamples.HttpWebRequestSample
{
    public class Program
    {
        public static void Main()
        {
            var networkHerlpers = new NetworkHelpers();

            // if we are using TLS it requires date & time
            networkHerlpers.SetupAndConnectNetwork(true);

            Console.WriteLine("Waiting for network up and IP address...");
            NetworkHelpers.IpAddressAvailable.WaitOne();

            Console.WriteLine("Waiting for valid Date & Time...");
            NetworkHelpers.DateTimeAvailable.WaitOne();

            // follow some test URLs and root CA certificates

            /////////////////////////////////////////////////////////////////////////////////////
            //add certificate in PEM format(as a string in the app)
            X509Certificate rootCACert = new X509Certificate(letsEncryptCACertificate);

            //test URL for Let's encrypt certificate
            string url = "https://valid-isrgrootx1.letsencrypt.org";

            /////////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////////////
            //// add certificate from resources in CRT format 
            //X509Certificate rootCACert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DigiCertGlobalRootCA));

            //// test URL for DigiCert certificate
            //string url = "https://global-root-ca.chain-demos.digicert.com";

            /////////////////////////////////////////////////////////////////////////////////////


            Console.WriteLine($"Performing Http request to: {url}");

            using (var httpWebRequest = (HttpWebRequest)WebRequest.Create(url))
            {
                httpWebRequest.Method = "GET";
                
                // if the request is to a secured server need to:
                // 1. provide the root CA certificate 
                // 2. the device has already stored a root CA bundle that will use when performing the authentication
                httpWebRequest.HttpsAuthentCert = rootCACert;

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var stream = httpWebRequest.GetResponse().GetResponseStream())
                {
                    // read response in chunks of 1k

                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;

                    Console.WriteLine("Http response follows");
                    Console.WriteLine(">>>>>>>>>>>>>");

                    do
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        Console.Write(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    }
                    while (bytesRead >= buffer.Length);
                }

                Console.WriteLine(">>>>>>>>>>>>>");
                Console.WriteLine("End of Http response");
            }

            Thread.Sleep(Timeout.Infinite);
        }


        // Let’s Encrypt Authority X3 (IdenTrust cross-signed)
        // from https://letsencrypt.org/certificates/

        // X509 RSA key PEM format
        private const string letsEncryptCACertificate =
@"-----BEGIN CERTIFICATE-----
MIIEkjCCA3qgAwIBAgIQCgFBQgAAAVOFc2oLheynCDANBgkqhkiG9w0BAQsFADA/
MSQwIgYDVQQKExtEaWdpdGFsIFNpZ25hdHVyZSBUcnVzdCBDby4xFzAVBgNVBAMT
DkRTVCBSb290IENBIFgzMB4XDTE2MDMxNzE2NDA0NloXDTIxMDMxNzE2NDA0Nlow
SjELMAkGA1UEBhMCVVMxFjAUBgNVBAoTDUxldCdzIEVuY3J5cHQxIzAhBgNVBAMT
GkxldCdzIEVuY3J5cHQgQXV0aG9yaXR5IFgzMIIBIjANBgkqhkiG9w0BAQEFAAOC
AQ8AMIIBCgKCAQEAnNMM8FrlLke3cl03g7NoYzDq1zUmGSXhvb418XCSL7e4S0EF
q6meNQhY7LEqxGiHC6PjdeTm86dicbp5gWAf15Gan/PQeGdxyGkOlZHP/uaZ6WA8
SMx+yk13EiSdRxta67nsHjcAHJyse6cF6s5K671B5TaYucv9bTyWaN8jKkKQDIZ0
Z8h/pZq4UmEUEz9l6YKHy9v6Dlb2honzhT+Xhq+w3Brvaw2VFn3EK6BlspkENnWA
a6xK8xuQSXgvopZPKiAlKQTGdMDQMc2PMTiVFrqoM7hD8bEfwzB/onkxEz0tNvjj
/PIzark5McWvxI0NHWQWM6r6hCm21AvA2H3DkwIDAQABo4IBfTCCAXkwEgYDVR0T
AQH/BAgwBgEB/wIBADAOBgNVHQ8BAf8EBAMCAYYwfwYIKwYBBQUHAQEEczBxMDIG
CCsGAQUFBzABhiZodHRwOi8vaXNyZy50cnVzdGlkLm9jc3AuaWRlbnRydXN0LmNv
bTA7BggrBgEFBQcwAoYvaHR0cDovL2FwcHMuaWRlbnRydXN0LmNvbS9yb290cy9k
c3Ryb290Y2F4My5wN2MwHwYDVR0jBBgwFoAUxKexpHsscfrb4UuQdf/EFWCFiRAw
VAYDVR0gBE0wSzAIBgZngQwBAgEwPwYLKwYBBAGC3xMBAQEwMDAuBggrBgEFBQcC
ARYiaHR0cDovL2Nwcy5yb290LXgxLmxldHNlbmNyeXB0Lm9yZzA8BgNVHR8ENTAz
MDGgL6AthitodHRwOi8vY3JsLmlkZW50cnVzdC5jb20vRFNUUk9PVENBWDNDUkwu
Y3JsMB0GA1UdDgQWBBSoSmpjBH3duubRObemRWXv86jsoTANBgkqhkiG9w0BAQsF
AAOCAQEA3TPXEfNjWDjdGBX7CVW+dla5cEilaUcne8IkCJLxWh9KEik3JHRRHGJo
uM2VcGfl96S8TihRzZvoroed6ti6WqEBmtzw3Wodatg+VyOeph4EYpr/1wXKtx8/
wApIvJSwtmVi4MFU5aMqrSDE6ea73Mj2tcMyo5jMd6jmeWUHK8so/joWUoHOUgwu
X4Po1QYz+3dszkDqMp4fklxBwXRsW10KXzPMTZ+sOPAveyxindmjkW8lGy+QsRlG
PfZ+G6Z6h7mjem0Y+iWlkYcV4PIWL1iwBi8saCbGS5jN2p8M+X+Q7UNKEkROb3N6
KOqkqm57TH2H3eDJAkSnh6/DNFu0Qg==
-----END CERTIFICATE-----";
    }
}
