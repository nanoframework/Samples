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

namespace QOTDClient
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
                Debug.WriteLine($"{DateTime.UtcNow} Network connected");
            }

            UdpClient udpClient = new UdpClient("djxmmx.net", 17); // Quote of the day public server
            udpClient.Client.ReceiveTimeout = 5000; // 5 sec time out

            byte[] buffer = new byte[1024];
            IPEndPoint ipEndpoint = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                Debug.WriteLine("Getting quote of the day");
                udpClient.Send(Encoding.UTF8.GetBytes(" "));
                try
                {
                    int length = udpClient.Receive(buffer, ref ipEndpoint);
                    Debug.WriteLine(Encoding.UTF8.GetString(buffer, 0, length));
                    Thread.Sleep(5000);
                }
                catch (SocketException ex) when (ex.ErrorCode == (int)SocketError.TimedOut)
                {
                    Debug.WriteLine("Time out!");
                }
            }
        }
    }
}
