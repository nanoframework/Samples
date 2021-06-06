﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Runtime.Events;
using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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

            // get host entry for How's my SSL test site
            IPHostEntry hostEntry = Dns.GetHostEntry("httpbin.org");

            // need an IPEndPoint from that one above
            IPEndPoint ep = new IPEndPoint(hostEntry.AddressList[0], 80);

            Debug.WriteLine("Opening socket...");
            using (Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    Debug.WriteLine("Connecting...");

                    // connect socket
                    mySocket.Connect(ep);

                    byte[] buffer = Encoding.UTF8.GetBytes("GET / HTTP/1.0\r\n\r\n");

                    mySocket.Send(buffer);

                    Debug.WriteLine($"Wrote {buffer.Length} bytes");

                    // setup buffer to read data from socket
                    buffer = new byte[1024];

                    // trying to read from socket
                    int bytes = mySocket.Receive(buffer);

                    Debug.WriteLine($"Read {bytes} bytes");

                    if (bytes > 0)
                    {
                        // we have data!
                        // output as string
                        Debug.WriteLine(new String(Encoding.UTF8.GetChars(buffer)));
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
                finally
                {
                    Debug.WriteLine("Closing socket");
                    mySocket.Close();
                }
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
