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

namespace SecureClient
{
    public class Program
    {
        public static void Main()
        {
            NetworkHelpers.SetupAndConnectNetwork(true);

            Debug.WriteLine("Waiting for network up and IP address...");

            NetworkHelpers.IpAddressAvailable.WaitOne();

            Debug.WriteLine("Waiting for valid Date & Time...");

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
            //IPHostEntry hostEntry = Dns.GetHostEntry("www.howsmyssl.com");
            // get host entry for Global Root test site
            IPHostEntry hostEntry = Dns.GetHostEntry("global-root-ca.chain-demos.digicert.com");

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
                    using (SslStream stream = new SslStream(mySocket))
                    {
                        ///////////////////////////////////////////////////////////////////////////////////
                        // Authenticating using a client certificate stored in the device is possible by
                        // setting the UseStoredDeviceCertificate property. 
                        // 
                        // In practice it's equivalent to providing a client certificate in
                        // the 'clientCertificate' parameter when calling AuthenticateAsClient(...)
                        //
                        /////////////////////////////////////////////////////////////////////////////////// 
                        stream.UseStoredDeviceCertificate = true;

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
                        stream.AuthenticateAsClient("global-root-ca.chain-demos.digicert.com", null, digiCertGlobalRootCACert, SslProtocols.Tls12);

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
                        stream.Write(buffer, 0, buffer.Length);

                        Debug.WriteLine($"Wrote {buffer.Length} bytes");

                        // setup buffer to read data from socket
                        buffer = new byte[1024];

                        // trying to read from socket
                        int bytes = stream.Read(buffer, 0, buffer.Length);

                        Debug.WriteLine($"Read {bytes} bytes");

                        if (bytes > 0)
                        {
                            // we have data!
                            // output as string
                            Debug.WriteLine(new String(Encoding.UTF8.GetChars(buffer)));
                        }
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

        // Let’s Encrypt Authority X3 (IdenTrust cross-signed)
        // from https://letsencrypt.org/certificates/

        // X509 RSA key PEM format 2048 bytes
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
