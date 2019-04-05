//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Networking;
using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace SecureClient
{
    public class Program
    {
        public static void Main()
        {
            NetworkHelpers.SetupAndConnectNetwork(true);

            Console.WriteLine("Waiting for network up and IP address...");

            NetworkHelpers.IpAddressAvailable.WaitOne();

            Console.WriteLine("Waiting for valid Date & Time...");

            NetworkHelpers.DateTimeAvailable.WaitOne();

            /////////////////////////////////////////////////////////////////////////////////////
            // add certificate in PEM format (as a string in the app)
            X509Certificate letsEncryptCACert = new X509Certificate(letsEncryptCACertificate);
            /////////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////////////
            // add certificate in CER format (as a managed resource)
            X509Certificate digiCertGlobalRootCACert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DigiCertGlobalRootCA));
            /////////////////////////////////////////////////////////////////////////////////////


            // get host entry for How's my SSL test site
            IPHostEntry hostEntry = Dns.GetHostEntry("www.howsmyssl.com");
            // get host entry for Global Root test site
            //IPHostEntry hostEntry = Dns.GetHostEntry("global-root-ca.chain-demos.digicert.com");

            // need an IPEndPoint from that one above
            IPEndPoint ep = new IPEndPoint(hostEntry.AddressList[0], 443);

            Console.WriteLine("Opening socket...");
            using (Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    Console.WriteLine("Connecting...");

                    // connect socket
                    mySocket.Connect(ep);

                    Console.WriteLine("Authenticating with server...");

                    // setup SSL stream
                    SslStream ss = new SslStream(mySocket);

                    ///////////////////////////////////////////////////////////////////////////////////
                    // Authenticating the server can be handled in one of three ways:
                    //
                    // 1. By providing the root CA certificate of the server being connected to.
                    // 
                    // 2. Having the target device preloaded with the root CA certificate.
                    // 
                    // !! NOT SECURED !! NOT RECOMENDED !!
                    // 3. Forcing the authentication workflow to NOT validate the server certificate.
                    //
                    /////////////////////////////////////////////////////////////////////////////////// 

                    // option 1 
                    // setup authentication (add CA root certificate to the call)
                    // Let's encrypt test certificate
                    ss.AuthenticateAsClient("www.howsmyssl.com", null, letsEncryptCACert, SslProtocols.TLSv11);
                    // GlobalRoot CA cert from resources
                    //ss.AuthenticateAsClient("global-root-ca.chain-demos.digicert.com", null, digiCertGlobalRootCACert, SslProtocols.TLSv11);

                    // option 2
                    // setup authentication (without providing root CA certificate)
                    // this requires that the trusted root CA certificates are available in the device certificate store
                    //ss.AuthenticateAsClient("www.howsmyssl.com", SslProtocols.TLSv11);
                    //ss.AuthenticateAsClient("global-root-ca.chain-demos.digicert.com", SslProtocols.TLSv12);

                    // option 3
                    // disable certificate validation
                    //ss.SslVerification = SslVerification.NoVerification;
                    //ss.AuthenticateAsClient("www.howsmyssl.com", SslProtocols.TLSv11);

                    Console.WriteLine("SSL handshake OK!");

                    // write an HTTP GET request to receive data
                    byte[] buffer = Encoding.UTF8.GetBytes("GET / HTTP/1.0\r\n\r\n");
                    ss.Write(buffer, 0, buffer.Length);

                    Console.WriteLine($"Wrote {buffer.Length} bytes");

                    // setup buffer to read data from socket
                    buffer = new byte[1024];

                    // trying to read from socket
                    int bytes = ss.Read(buffer, 0, buffer.Length);

                    Console.WriteLine($"Read {bytes} bytes");

                    if (bytes > 0)
                    {
                        // we have data!
                        // output as string
                        Console.WriteLine(new String(Encoding.UTF8.GetChars(buffer)));
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"** Socket exception occurred: {ex.Message} error code {ex.ErrorCode}!**");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"** Exception occurred: {ex.Message}!**");
                }
                finally
                {
                    Console.WriteLine("Closing socket");
                    mySocket.Close();
                }
            }

            Thread.Sleep(Timeout.Infinite);
        }

        // Let’s Encrypt Authority X3 (IdenTrust cross-signed)
        // from https://letsencrypt.org/certificates/

        // X509 RSA key PEM format 2048 bytes
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
