//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Networking;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

#if HAS_WIFI
using System.Device.Wifi;
#endif

namespace HttpWebRequestSample
{
    public class Program
    {
#if HAS_WIFI
        private static string MySsid = "ssid";
        private static string MyPassword = "password";      
#endif

        public static void Main()
        {
            Debug.WriteLine("Waiting for network up, IP address and valid date & time...");

            bool success;
            CancellationTokenSource cs = new(60000);
#if HAS_WIFI

            // if the device doesn't have the Wifi credentials stored
            //success = WifiNetworkHelper.ConnectDhcp(MySsid, MyPassword, requiresDateTime: true, token: cs.Token);

            /////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////
            // if the device has the Wifi credentials already stored, this call it's faster    //
            success = WifiNetworkHelper.Reconnect(requiresDateTime: true, token: cs.Token); //
            /////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////

#else
            success = NetworkHelper.SetupAndConnectNetwork(cs.Token, true);
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

            // follow some test URLs and root CA certificates

            /////////////////////////////////////////////////////////////////////////////////////
            ////add certificate in PEM format(as a string in the app)
            //X509Certificate rootCACert = new X509Certificate(letsEncryptCACertificate);
            ////test URL for Let's encrypt certificate
            //string url = "https://www.howsmyssl.com";
            /////////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////////////////////////
            // add certificate from resources in CRT format 
            X509Certificate rootCACert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DigiCertGlobalRootCA));
            // test URL for DigiCert certificate
            string url = "https://global-root-ca.chain-demos.digicert.com";
            ///////////////////////////////////////////////////////////////////////////////////


            Debug.WriteLine($"Performing Http request to: {url}");

            // perform the request as a HttpWebRequest
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";

            //////////////////////////////////////////////////////////////////////
            // need to set the SSL protocol that the connection is going to use //
            // *** this MANDATORY otherwise the authentication will fail ***    //
            //////////////////////////////////////////////////////////////////////
            httpWebRequest.SslProtocols = System.Net.Security.SslProtocols.Tls12;

            // if the request is to a secured server we need to make sure that we either:
            // 1. provide the root CA certificate 
            // 2. the device has already stored a root CA bundle that will use when performing the authentication
            httpWebRequest.HttpsAuthentCert = rootCACert;

            int totalBytesRead = 0;

            // get the response as a HttpWebResponse
            // wrap the response object with a using statement to make sure that it's disposed
            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                // wrap the response stream on a using statement to make sure that it's disposed
                using (var stream = httpWebResponse.GetResponseStream())
                {
                    // read response in chunks of 1k

                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;

                    Debug.WriteLine("Http response follows");
                    Debug.WriteLine(">>>>>>>>>>>>>");

                    do
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);

                        if (bytesRead > 0)
                        {
                            totalBytesRead += bytesRead;
                            Debug.Write(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                        }
                    }
                    while (totalBytesRead < httpWebResponse.ContentLength);
                }

                Debug.WriteLine(">>>>>>>>>>>>>");
                Debug.WriteLine("End of Http response");
                Debug.WriteLine($"Read {totalBytesRead} bytes");
            }

            Thread.Sleep(Timeout.Infinite);
        }


        // Let’s Encrypt Authority X3 (IdenTrust cross-signed)
        // from https://letsencrypt.org/certificates/

        // X509 RSA key PEM format
        private const string letsEncryptCACertificate =
@"-----BEGIN CERTIFICATE-----
MIIFjTCCA3WgAwIBAgIRANOxciY0IzLc9AUoUSrsnGowDQYJKoZIhvcNAQELBQAw
TzELMAkGA1UEBhMCVVMxKTAnBgNVBAoTIEludGVybmV0IFNlY3VyaXR5IFJlc2Vh
cmNoIEdyb3VwMRUwEwYDVQQDEwxJU1JHIFJvb3QgWDEwHhcNMTYxMDA2MTU0MzU1
WhcNMjExMDA2MTU0MzU1WjBKMQswCQYDVQQGEwJVUzEWMBQGA1UEChMNTGV0J3Mg
RW5jcnlwdDEjMCEGA1UEAxMaTGV0J3MgRW5jcnlwdCBBdXRob3JpdHkgWDMwggEi
MA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCc0wzwWuUuR7dyXTeDs2hjMOrX
NSYZJeG9vjXxcJIvt7hLQQWrqZ41CFjssSrEaIcLo+N15Obzp2JxunmBYB/XkZqf
89B4Z3HIaQ6Vkc/+5pnpYDxIzH7KTXcSJJ1HG1rrueweNwAcnKx7pwXqzkrrvUHl
Npi5y/1tPJZo3yMqQpAMhnRnyH+lmrhSYRQTP2XpgofL2/oOVvaGifOFP5eGr7Dc
Gu9rDZUWfcQroGWymQQ2dYBrrErzG5BJeC+ilk8qICUpBMZ0wNAxzY8xOJUWuqgz
uEPxsR/DMH+ieTETPS02+OP88jNquTkxxa/EjQ0dZBYzqvqEKbbUC8DYfcOTAgMB
AAGjggFnMIIBYzAOBgNVHQ8BAf8EBAMCAYYwEgYDVR0TAQH/BAgwBgEB/wIBADBU
BgNVHSAETTBLMAgGBmeBDAECATA/BgsrBgEEAYLfEwEBATAwMC4GCCsGAQUFBwIB
FiJodHRwOi8vY3BzLnJvb3QteDEubGV0c2VuY3J5cHQub3JnMB0GA1UdDgQWBBSo
SmpjBH3duubRObemRWXv86jsoTAzBgNVHR8ELDAqMCigJqAkhiJodHRwOi8vY3Js
LnJvb3QteDEubGV0c2VuY3J5cHQub3JnMHIGCCsGAQUFBwEBBGYwZDAwBggrBgEF
BQcwAYYkaHR0cDovL29jc3Aucm9vdC14MS5sZXRzZW5jcnlwdC5vcmcvMDAGCCsG
AQUFBzAChiRodHRwOi8vY2VydC5yb290LXgxLmxldHNlbmNyeXB0Lm9yZy8wHwYD
VR0jBBgwFoAUebRZ5nu25eQBc4AIiMgaWPbpm24wDQYJKoZIhvcNAQELBQADggIB
ABnPdSA0LTqmRf/Q1eaM2jLonG4bQdEnqOJQ8nCqxOeTRrToEKtwT++36gTSlBGx
A/5dut82jJQ2jxN8RI8L9QFXrWi4xXnA2EqA10yjHiR6H9cj6MFiOnb5In1eWsRM
UM2v3e9tNsCAgBukPHAg1lQh07rvFKm/Bz9BCjaxorALINUfZ9DD64j2igLIxle2
DPxW8dI/F2loHMjXZjqG8RkqZUdoxtID5+90FgsGIfkMpqgRS05f4zPbCEHqCXl1
eO5HyELTgcVlLXXQDgAWnRzut1hFJeczY1tjQQno6f6s+nMydLN26WuU4s3UYvOu
OsUxRlJu7TSRHqDC3lSE5XggVkzdaPkuKGQbGpny+01/47hfXXNB7HntWNZ6N2Vw
p7G6OfY+YQrZwIaQmhrIqJZuigsrbe3W+gdn5ykE9+Ky0VgVUsfxo52mwFYs1JKY
2PGDuWx8M6DlS6qQkvHaRUo0FMd8TsSlbF0/v965qGFKhSDeQoMpYnwcmQilRh/0
ayLThlHLN81gSkJjVrPI0Y8xCVPB4twb1PFUd2fPM3sA1tJ83sZ5v8vgFv2yofKR
PB0t6JzUA81mSqM3kxl5e+IZwhYAyO0OTg3/fs8HqGTNKd9BqoUwSRBzp06JMg5b
rUCGwbCUDI0mxadJ3Bz4WxR6fyNpBK2yAinWEsikxqEt
-----END CERTIFICATE-----";
    }
}
