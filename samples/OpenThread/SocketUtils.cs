//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Samples
{
    internal class NetUtils
    {
        private static Socket socket;

        /// <summary>
        ///  Open a new UDP socket
        /// </summary>
        /// <param name="remoteAdr"></param>
        /// <param name="port"></param>
        public static void OpenUdpSocket(String remoteAdr, int port, IPAddress endpoint)
        {
            socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);

            // Interface / port to receive on
            IPEndPoint ep = new IPEndPoint(endpoint, port);
            socket.Bind(ep);

            if (remoteAdr.Length > 0)
            {
                // Set remote address
                var address = IPAddress.Parse(remoteAdr);
                IPEndPoint rep = new IPEndPoint(address, port);
                socket.Connect(rep);
            }
        }

        /// <summary>
        /// Close open socket
        /// </summary>
        public static void CloseUdpSocket()
        {
            socket.Close();
            socket = null;
        }

        /// <summary>
        /// Send message to specific target / port
        /// </summary>
        /// <param name="port">Port number to send to</param>
        /// <param name="targetAdr">Target IP address</param>
        /// <param name="message">MEssage to send</param>
        public static void SendMessageSocketTo(int port, string targetAdr, string message)
        {
            var data = Encoding.UTF8.GetBytes(message);

            var address = IPAddress.Parse(targetAdr);
            IPEndPoint ep = new IPEndPoint(address, port);

            socket.SendTo(data, ep);
        }

        /// <summary>
        /// Send message to connected target, target specified in Open
        /// </summary>
        /// <param name="message"></param>
        public static void SendMessage(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            socket.Send(data);
        }

        /// <summary>
        /// Method for receiving and displaying messages from UDP socket. 
        /// If respond param true will respond with generic message (like a server)
        /// </summary>
        /// <param name="respond"></param>
        public static void ReceiveUdpMessages(bool respond = false)
        {
            Display.Log($"Receive thread for UDP messages started");

            while (true)
            {
                byte[] data = new byte[256];
                EndPoint remoteEp = new IPEndPoint(0, 0);

                int length = socket.ReceiveFrom(data, ref remoteEp);

                var message = Encoding.UTF8.GetString(data, 0, length);

                Display.Log($"UDP message(sock) >{message}< received from {remoteEp}");

                Program._led.SetRxTX();

                if (respond)
                {
                    IPEndPoint rp = remoteEp as IPEndPoint;
                    SendMessageSocketTo(rp.Port, rp.Address.ToString(), $"Server response {DateTime.UtcNow}");
                    Display.Log($"UDP message(sock) >{message}< respond to {rp.Address} {rp.Port}");
                }
            }
        }
    }
}
