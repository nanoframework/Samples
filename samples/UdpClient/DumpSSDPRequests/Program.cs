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

namespace DumpSSDPRequests
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


            Debug.WriteLine("Starting SSD monitor");
            byte[] buffer = new byte[2048];
            IPAddress ipSSD = IPAddress.Parse("239.255.255.250");
            IPEndPoint iPEndpoint = new IPEndPoint(IPAddress.Any, 1900);

            UdpClient client = new UdpClient(iPEndpoint);
            client.JoinMulticastGroup(ipSSD);
            try
            {
                while (true)
                {
                    IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    int length = client.Receive(buffer, ref remote);
                    string result = Encoding.UTF8.GetString(buffer, 0, length);
                    Debug.WriteLine($"{DateTime.UtcNow} <- {remote}");
                    Debug.WriteLine(result);
                }
            }
            finally
            {
                client.DropMulticastGroup(ipSSD);
                Thread.Sleep(Timeout.Infinite);
            }

            Thread.Sleep(Timeout.Infinite);

        }
    }
}
