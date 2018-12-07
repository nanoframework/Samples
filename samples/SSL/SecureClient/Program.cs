//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Runtime.Events;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace SecureClient
{
    public class Program
    {
        private const string c_SSID = "myssid";
        private const string c_AP_PASSWORD = "mypassword";

        static private ManualResetEvent _ipAddressAvailable = new ManualResetEvent(false);

        public static void Main()
        {
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangedCallback);

            SetupAndConnectNetwork();

            Console.WriteLine("Waiting for network up and IP address...");

            _ipAddressAvailable.WaitOne();

            X509Certificate letsEncryptCACert = new X509Certificate(letsEncryptCACertificate);

            SetDateTime();

            Console.WriteLine("Opening socket...");
            using (Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4))
            {
                try
                {
                    // get host entry for test site
                    IPHostEntry hostEntry = Dns.GetHostEntry("www.howsmyssl.com");

                    // need an IPEndPoint from that one above
                    IPEndPoint ep = new IPEndPoint(hostEntry.AddressList[0], 443);

                    Console.WriteLine("Connecting...");

                    // connect socket
                    mySocket.Connect(ep);

                    Console.WriteLine("Authenticating with server...");

                    // setup SSL stream
                    SslStream ss = new SslStream(mySocket);

                    // setup authentication, including CA certificate validation
                    ss.AuthenticateAsClient("www.howsmyssl.com", letsEncryptCACert, SslVerification.CertificateRequired, SslProtocols.TLSv11);

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

        private static void SetDateTime()
        {
            Console.WriteLine("Setting up system clock...");

            // set system date time (needs to be accurate to the day in order to be able to validate a certificate)
            //Rtc.SetSystemTime(new DateTime(2018, 08, 02));

            // if SNTP is available and enabled on target device this can be skipped because we should have a valid date & time
            while (DateTime.UtcNow.Year < 2018)
            {
                Console.WriteLine("Waiting for valid date time...");
                // wait for valid date & time
                Thread.Sleep(1000);
            }
        }

        public static void SetupAndConnectNetwork()
        {
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            if (nis.Length > 0)
            {
                // get the first interface
                NetworkInterface ni = nis[0];

                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    // network interface is Wi-Fi
                    Console.WriteLine("Network connection is: Wi-Fi");

                    Wireless80211Configuration wc = Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];
                    if (wc.Ssid != c_SSID && wc.Password != c_AP_PASSWORD)
                    {
                        // have to update Wi-Fi configuration
                        wc.Ssid = c_SSID;
                        wc.Password = c_AP_PASSWORD;
                        wc.SaveConfiguration();
                    }
                    else
                    {   // Wi-Fi configuration matches
                    }
                }
                else
                {
                    // network interface is Ethernet
                    Console.WriteLine("Network connection is: Ethernet");

                    ni.EnableAutomaticDns();
                    ni.EnableDhcp();
                }

                // check if we have an IP
                CheckIP();
            }
            else
            {
                throw new NotSupportedException("ERROR: there is no network interface configured.\r\nOpen the 'Edit Network Configuration' in Device Explorer and configure one.");
            }
        }

        static void CheckIP()
        {
            Console.WriteLine("Checking for IP");

            NetworkInterface ni = NetworkInterface.GetAllNetworkInterfaces()[0];
            if (ni.IPv4Address != null && ni.IPv4Address.Length > 0)
            {
                if (ni.IPv4Address[0] != '0')
                {
                    Console.WriteLine($"We have and IP: {ni.IPv4Address}");
                    _ipAddressAvailable.Set();
                }
            }
        }

        static void AddressChangedCallback(object sender, EventArgs e)
        {
            CheckIP();
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
