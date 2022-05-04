//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

//#define HAS_WIFI

using System;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using nanoFramework.Networking;

namespace UdpEchoServer
{
    public class Program
    {
        public static void Main()
        {
#if HAS_WIFI
            string MySsid = "ssid";
            string MyPassword = "password";
#endif
            Debug.WriteLine("Hello from nanoFramework!");

            CancellationTokenSource cs = new(10 * 1000); //10 seconds
            bool success;

#if HAS_WIFI
            success = WifiNetworkHelper.ConnectDhcp(MySsid, MyPassword, requiresDateTime: true, token: cs.Token);
#else
            success = NetworkHelper.SetupAndConnectNetwork(cs.Token, true);
#endif
            if (!success)
            {
                Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {NetworkHelper.Status}.");
                if (NetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"ex: {NetworkHelper.HelperException}");
                }

                return;
            }
            else
            {
                // Start a DNS/TCP connection toward google server to get your local IP address and display it
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    IPEndPoint epGoogleDNS = new IPEndPoint(new IPAddress(new byte[] { 8, 8, 8, 8 }), 53);
                    sock.Connect(epGoogleDNS);
                    Debug.WriteLine($"{DateTime.UtcNow} Network connected on: {(sock.LocalEndPoint as IPEndPoint).Address}");
                }
            }

            // Run echo protocol on port 7 (RFC 862)
            UdpClient udpClient = new UdpClient(7); 

            // We limit ourself to a 1024 bytes buffer
            byte[] buffer = new byte[1024];
            IPEndPoint endpointClient = new IPEndPoint(IPAddress.Any, 0);

            // We send back every request we get.
            while (true)
            {
                Debug.WriteLine("Waiting for client request");
                int length = udpClient.Receive(buffer, ref endpointClient);
                try
                {
                    Debug.WriteLine($"Got message: {Encoding.UTF8.GetString(buffer, 0, buffer.Length)}");
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Got message: {BitConverter.ToString(buffer)}");
                }
                udpClient.Send(buffer,endpointClient);
            }

        }
    }
}
