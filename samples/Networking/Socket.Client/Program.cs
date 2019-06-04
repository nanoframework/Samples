//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Networking;
using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SecureClient
{
    public class Program
    {
        public static void Main()
        {
            NetworkHelpers.SetupAndConnectNetwork(false);

            Console.WriteLine("Waiting for network up and IP address...");

            NetworkHelpers.IpAddressAvailable.WaitOne();

            // get host entry for How's my SSL test site
            IPHostEntry hostEntry = Dns.GetHostEntry("httpbin.org");

            // need an IPEndPoint from that one above
            IPEndPoint ep = new IPEndPoint(hostEntry.AddressList[0], 80);

            Console.WriteLine("Opening socket...");
            using (Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    Console.WriteLine("Connecting...");

                    // connect socket
                    mySocket.Connect(ep);

                    byte[] buffer = Encoding.UTF8.GetBytes("GET / HTTP/1.0\r\n\r\n");

                    mySocket.Send(buffer);

                    Console.WriteLine($"Wrote {buffer.Length} bytes");

                    // setup buffer to read data from socket
                    buffer = new byte[1024];

                    // trying to read from socket
                    int bytes = mySocket.Receive(buffer);

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
    }
}
