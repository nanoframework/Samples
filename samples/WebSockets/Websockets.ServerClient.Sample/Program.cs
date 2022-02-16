using System.Net.WebSockets;
using System.Net.WebSockets.Server;
using nanoFramework.Networking;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace NFWebSockets.Example
{


    public class Program
    {
        public static void Main()
        {
            //Nanoframework makes it easy to create a websocket server. And to create a new WebSocket Client. Here's a quick example of how easy.
            //use the URL provided in the debug output and connect your own client. Or use your device to connect to your websocket server in the cloud.   

            NetworkHelpers.SetupAndConnectNetwork(true);

            Debug.WriteLine("Waiting for network up and IP address...");
            NetworkHelpers.IpAddressAvailable.WaitOne();

            Debug.WriteLine("Waiting for valid date&time...");
            NetworkHelpers.DateTimeAvailable.WaitOne();

            Debug.WriteLine("All set!");
            string ip = "127.0.0.1";
            //Lets create a new default webserver.
            WebSocketServer webSocketServer = new WebSocketServer();
            webSocketServer.Start();
            //Let's echo all incomming messages from clients to all connected clients including the sender. 
            webSocketServer.MessageReceived += WebSocketServer_MesageReceived;
            Debug.WriteLine($"WebSocket server is up and running, connect on: ws://{ip}:{webSocketServer.Port}{webSocketServer.Prefix}");

            //Now let's also attach a local websocket client. Just because we can :-)
            ClientWebSocket client = new ClientWebSocket();
            
            ///////////////////////////////////////////
            // secure connection config
            // OK to override the setting here to 
            client.SslVerification = System.Net.Security.SslVerification.NoVerification;
            client.CACertificate = new X509Certificate(_dstRootCAX3);

            //connect to the local client and write the messages to the debug output
            client.Connect("wss://echo.websocket.org");
            //client.Connect("ws://127.0.0.1");
            client.MessageReceived += Client_MessageReceived;

            int helloCounter = 0;

            //While the client is connected will send a friendly hello every second.
            while (client.State == System.Net.WebSockets.WebSocketFrame.WebSocketState.Open)
            {
                client.SendString($"hello {helloCounter++}");

                Thread.Sleep(100);
            }

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Frame.MessageType == System.Net.WebSockets.WebSocketFrame.WebSocketMessageType.Text)
            {
                Debug.WriteLine($"websocket message: {Encoding.UTF8.GetString(e.Frame.Buffer, 0, e.Frame.Buffer.Length)}");
            }
            else
            {
                Debug.WriteLine("websocket binary data");
            }
        }

        private static void WebSocketServer_MesageReceived(object sender, MessageReceivedEventArgs e)
        {

            var webSocketServer = (WebSocketServer)sender;

            var buffer = e.Frame.Buffer;
            if (e.Frame.MessageType == System.Net.WebSockets.WebSocketFrame.WebSocketMessageType.Text)
            {
                webSocketServer.BroadCast(Encoding.UTF8.GetString(buffer, 0, buffer.Length));

            }
            else
            {
                webSocketServer.BroadCast(e.Frame.Buffer);
            }
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
