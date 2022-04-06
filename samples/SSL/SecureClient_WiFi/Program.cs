//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

#if HAS_WIFI
using System.Device.WiFi;
#endif

namespace SecureClient
{
    public class Program
    {
#if HAS_WIFI
        private static string MySsid = "ssid";
        private static string MyPassword = "password";      
#endif

        public static void Main()
        {
            Debug.WriteLine("Waiting for network up and IP address...");
            
            bool success;
            
            CancellationTokenSource cs = new(60000);

#if HAS_WIFI
            success = WiFiNetworkHelper.ConnectDhcp(MySsid, MyPassword, requiresDateTime: true, token: cs.Token);
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

            /////////////////////////////////////////////////////////////////////////////////////
            // add certificate in PEM format (as a string in the app)
            X509Certificate letsEncryptCACert = new X509Certificate(_dstRootCAX3);
            /////////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////////////
            // add certificate in CER format (as a managed resource)
            X509Certificate digiCertGlobalRootCACert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DigiCertGlobalRootCA));
            /////////////////////////////////////////////////////////////////////////////////////

            // get host entry for How's my SSL test site
            //IPHostEntry hostEntry = Dns.GetHostEntry("www.howsmyssl.com");
            // get host entry for Global Root test site
            IPHostEntry hostEntry = Dns.GetHostEntry("https://global-root-ca.chain-demos.digicert.com");

            // need an IPEndPoint from that one above
            IPEndPoint ep = new IPEndPoint(hostEntry.AddressList[0], 443);

            Debug.WriteLine("Opening socket...");
            using (Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    Debug.WriteLine("Connecting...");

                    // connect socket
                    mySocket.Connect(ep);

                    Debug.WriteLine("Authenticating with server...");

                    // setup SSL stream
                    using (SslStream sslStream = new SslStream(mySocket))
                    {
                        ///////////////////////////////////////////////////////////////////////////////////
                        // Authenticating using a client certificate stored in the device is possible by
                        // setting the UseStoredDeviceCertificate property. 
                        // 
                        // In practice it's equivalent to providing a client certificate in
                        // the 'clientCertificate' parameter when calling AuthenticateAsClient(...)
                        //
                        /////////////////////////////////////////////////////////////////////////////////// 
                        //stream.UseStoredDeviceCertificate = true;

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
                        //stream.AuthenticateAsClient("www.howsmyssl.com", null, letsEncryptCACert, SslProtocols.Tls11);
                        // GlobalRoot CA cert from resources
                        sslStream.AuthenticateAsClient("global-root-ca.chain-demos.digicert.com", null, digiCertGlobalRootCACert, SslProtocols.Tls12);

                        // option 2
                        // setup authentication (without providing root CA certificate)
                        // this requires that the trusted root CA certificates are available in the device certificate store
                        //stream.AuthenticateAsClient("www.howsmyssl.com", SslProtocols.Tls11);
                        //stream.AuthenticateAsClient("global-root-ca.chain-demos.digicert.com", SslProtocols.Tls12);

                        // option 3
                        // disable certificate validation
                        //stream.SslVerification = SslVerification.NoVerification;
                        //stream.AuthenticateAsClient("www.howsmyssl.com", SslProtocols.TLSv11);

                        Debug.WriteLine("SSL handshake OK!");

                        // write an HTTP GET request to receive data
                        byte[] buffer = Encoding.UTF8.GetBytes("GET / HTTP/1.0\r\n\r\n");
                        sslStream.Write(buffer, 0, buffer.Length);

                        Debug.WriteLine($"Wrote {buffer.Length} bytes");

                        int bytesCounter = 0;

                        do
                        {
                            var bufferLenght = sslStream.Length;

                            // if available length is 0, need to read at least 1 byte to get it started
                            if(bufferLenght == 0)
                            {
                                bufferLenght = 1;
                            }

                            // setup buffer to read data from socket
                            buffer = new byte[bufferLenght];

                            // trying to read from socket
                            int bytes = sslStream.Read(buffer, 0, buffer.Length);

                            bytesCounter += bytes;

                            if (bytes > 0)
                            {
                                // data was read!
                                // output as string
                                // mind to use only the amount of data actually read because it could be less than the requested count
                                Debug.Write(new String(Encoding.UTF8.GetChars(buffer, 0, bytes)));
                            }
                        }
                        while (sslStream.DataAvailable);

                        Debug.WriteLine($"Read {bytesCounter} bytes");
                    }
                }
                catch (SocketException ex)
                {
                    Debug.WriteLine($"** Socket exception occurred: {ex.Message} error code {ex.ErrorCode}!**");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"** Exception occurred: {ex.Message}!**");
                }
            }

            Thread.Sleep(Timeout.Infinite);
        }

        // Identrust DST Root CA X3
        // from https://www.identrust.com/dst-root-ca-x3

        private const string _dstRootCAX3 =
@"-----BEGIN CERTIFICATE-----
MIIFazCCA1OgAwIBAgIRAIIQz7DSQONZRGPgu2OCiwAwDQYJKoZIhvcNAQELBQAw
TzELMAkGA1UEBhMCVVMxKTAnBgNVBAoTIEludGVybmV0IFNlY3VyaXR5IFJlc2Vh
cmNoIEdyb3VwMRUwEwYDVQQDEwxJU1JHIFJvb3QgWDEwHhcNMTUwNjA0MTEwNDM4
WhcNMzUwNjA0MTEwNDM4WjBPMQswCQYDVQQGEwJVUzEpMCcGA1UEChMgSW50ZXJu
ZXQgU2VjdXJpdHkgUmVzZWFyY2ggR3JvdXAxFTATBgNVBAMTDElTUkcgUm9vdCBY
MTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAK3oJHP0FDfzm54rVygc
h77ct984kIxuPOZXoHj3dcKi/vVqbvYATyjb3miGbESTtrFj/RQSa78f0uoxmyF+
0TM8ukj13Xnfs7j/EvEhmkvBioZxaUpmZmyPfjxwv60pIgbz5MDmgK7iS4+3mX6U
A5/TR5d8mUgjU+g4rk8Kb4Mu0UlXjIB0ttov0DiNewNwIRt18jA8+o+u3dpjq+sW
T8KOEUt+zwvo/7V3LvSye0rgTBIlDHCNAymg4VMk7BPZ7hm/ELNKjD+Jo2FR3qyH
B5T0Y3HsLuJvW5iB4YlcNHlsdu87kGJ55tukmi8mxdAQ4Q7e2RCOFvu396j3x+UC
B5iPNgiV5+I3lg02dZ77DnKxHZu8A/lJBdiB3QW0KtZB6awBdpUKD9jf1b0SHzUv
KBds0pjBqAlkd25HN7rOrFleaJ1/ctaJxQZBKT5ZPt0m9STJEadao0xAH0ahmbWn
OlFuhjuefXKnEgV4We0+UXgVCwOPjdAvBbI+e0ocS3MFEvzG6uBQE3xDk3SzynTn
jh8BCNAw1FtxNrQHusEwMFxIt4I7mKZ9YIqioymCzLq9gwQbooMDQaHWBfEbwrbw
qHyGO0aoSCqI3Haadr8faqU9GY/rOPNk3sgrDQoo//fb4hVC1CLQJ13hef4Y53CI
rU7m2Ys6xt0nUW7/vGT1M0NPAgMBAAGjQjBAMA4GA1UdDwEB/wQEAwIBBjAPBgNV
HRMBAf8EBTADAQH/MB0GA1UdDgQWBBR5tFnme7bl5AFzgAiIyBpY9umbbjANBgkq
hkiG9w0BAQsFAAOCAgEAVR9YqbyyqFDQDLHYGmkgJykIrGF1XIpu+ILlaS/V9lZL
ubhzEFnTIZd+50xx+7LSYK05qAvqFyFWhfFQDlnrzuBZ6brJFe+GnY+EgPbk6ZGQ
3BebYhtF8GaV0nxvwuo77x/Py9auJ/GpsMiu/X1+mvoiBOv/2X/qkSsisRcOj/KK
NFtY2PwByVS5uCbMiogziUwthDyC3+6WVwW6LLv3xLfHTjuCvjHIInNzktHCgKQ5
ORAzI4JMPJ+GslWYHb4phowim57iaztXOoJwTdwJx4nLCgdNbOhdjsnvzqvHu7Ur
TkXWStAmzOVyyghqpZXjFaH3pO3JLF+l+/+sKAIuvtd7u+Nxe5AW0wdeRlN8NwdC
jNPElpzVmbUq4JUagEiuTDkHzsxHpFKVK7q4+63SM1N95R1NbdWhscdCb+ZAJzVc
oyi3B43njTOQ5yOf+1CceWxG1bQVs5ZufpsMljq4Ui0/1lvh+wjChP4kqKOJ2qxq
4RgqsahDYVvTH9w7jXbyLeiNdd8XM2w9U/t7y0Ff/9yi0GE44Za4rF2LN9d11TPA
mRGunUHBcnWEvgJBQl9nJEiU0Zsnvgc/ubhPgXRR4Xq37Z0j4r7g1SgEEzwxA57d
emyPxgcYxn/eR44/KJ4EBs+lVDR3veyJm+kXQ99b21/+jh5Xos1AnX5iItreGCc=
-----END CERTIFICATE-----";

    }
}
