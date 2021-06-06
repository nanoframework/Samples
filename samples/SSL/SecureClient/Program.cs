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
using Windows.Devices.WiFi;
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
            success = NetworkHelper.ConnectWifiDhcp(MySsid, MyPassword, setDateTime: true, token: cs.Token);
#else
            success = NetworkHelper.WaitForValidIPAndDate(true, System.Net.NetworkInformation.NetworkInterfaceType.Ethernet, cs.Token);
#endif
            if (!success)
            {
                Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {NetworkHelper.ConnectionError.Error}.");
                if (NetworkHelper.ConnectionError.Exception != null)
                {
                    Debug.WriteLine($"Exception: {NetworkHelper.ConnectionError.Exception}");
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
MIIDSjCCAjKgAwIBAgIQRK+wgNajJ7qJMDmGLvhAazANBgkqhkiG9w0BAQUFADA/
MSQwIgYDVQQKExtEaWdpdGFsIFNpZ25hdHVyZSBUcnVzdCBDby4xFzAVBgNVBAMT
DkRTVCBSb290IENBIFgzMB4XDTAwMDkzMDIxMTIxOVoXDTIxMDkzMDE0MDExNVow
PzEkMCIGA1UEChMbRGlnaXRhbCBTaWduYXR1cmUgVHJ1c3QgQ28uMRcwFQYDVQQD
Ew5EU1QgUm9vdCBDQSBYMzCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEB
AN+v6ZdQCINXtMxiZfaQguzH0yxrMMpb7NnDfcdAwRgUi+DoM3ZJKuM/IUmTrE4O
rz5Iy2Xu/NMhD2XSKtkyj4zl93ewEnu1lcCJo6m67XMuegwGMoOifooUMM0RoOEq
OLl5CjH9UL2AZd+3UWODyOKIYepLYYHsUmu5ouJLGiifSKOeDNoJjj4XLh7dIN9b
xiqKqy69cK3FCxolkHRyxXtqqzTWMIn/5WgTe1QLyNau7Fqckh49ZLOMxt+/yUFw
7BZy1SbsOFU5Q9D8/RhcQPGX69Wam40dutolucbY38EVAjqr2m7xPi71XAicPNaD
aeQQmxkqtilX4+U9m5/wAl0CAwEAAaNCMEAwDwYDVR0TAQH/BAUwAwEB/zAOBgNV
HQ8BAf8EBAMCAQYwHQYDVR0OBBYEFMSnsaR7LHH62+FLkHX/xBVghYkQMA0GCSqG
SIb3DQEBBQUAA4IBAQCjGiybFwBcqR7uKGY3Or+Dxz9LwwmglSBd49lZRNI+DT69
ikugdB/OEIKcdBodfpga3csTS7MgROSR6cz8faXbauX+5v3gTt23ADq1cEmv8uXr
AvHRAosZy5Q6XkjEGB5YGV8eAlrwDPGxrancWYaLbumR9YbK+rlmM6pZW87ipxZz
R8srzJmwN0jP41ZL9c8PDHIyh8bwRLtTcm1D9SZImlJnt1ir/md2cXjbDaJWFBM5
JDGFoqgCWjBH4d1QB7wCCZAA62RjYJsWvIjJEubSfZGL+T0yjWW06XyxV3bqxbYo
Ob8VZRzI9neWagqNdwvYkQsEjgfbKbYK7p2CNTUQ
-----END CERTIFICATE-----";

    }
}
