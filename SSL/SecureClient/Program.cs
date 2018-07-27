using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace SecureClient
{
    public class Program
    {
        private const string c_SSID = "myssid";
        private const string c_AP_PASSWORD = "mypassword";

        public static void Main()
        {
            Console.WriteLine("Setting up network and connecting...");
            SetupAndConnectNetwork();

            Console.WriteLine("Opening socket...");
            using (Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4))
            {
                try
                {
                    // get host entry for test site
                    IPHostEntry hostEntry = Dns.GetHostEntry("https://www.howsmyssl.com");

                    // need an IPEndPoint from that one above
                    IPEndPoint ep = new IPEndPoint(hostEntry.AddressList[0], 443);

                    Console.WriteLine("Connecting...");

                    // connect socket
                    mySocket.Connect(ep);

                    SslStream ss = new SslStream(mySocket);
                    ss.AuthenticateAsClient("www.howsmyssl.com", SslProtocols.TLSv1); //| SslProtocols.TLSv11| SslProtocols.TLSv12);

                    Console.WriteLine("SSL handshake OK!");
                    //Console.WriteLine("SSL isServer:" + ss.IsServer.ToString());

                    //while (true)
                    //{
                    //    byte[] buffer = new byte[1024];

                    //    int bytes = ss.Read(buffer, 0, buffer.Length);

                    //    Console.WriteLine("Read bytes" + bytes.ToString());

                    //}
                }
                catch (Exception)
                {
                    mySocket.Close();
                }
            }

            Thread.Sleep(Timeout.Infinite);
        }

        public static void SetupAndConnectNetwork()
        {
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            if (nis.Length > 0)
            {
                // get the first interface
                NetworkInterface ni = nis[0];

                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    // network interface is Wi-Fi
                    Console.WriteLine("Network connection is: Wi-Fi");

                    Wireless80211Configuration wc = Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];
                    if (wc.Ssid != c_SSID && wc.Password != c_AP_PASSWORD)
                    {
                        // have to update Wi-Fi configuration
                        wc.Ssid = c_SSID;
                        wc.Password = c_AP_PASSWORD;
                        wc.SaveConfiguration();
                    }
                    else
                    {   // Wi-Fi configuration matches
                    }
                }
                else
                {
                    // network interface is Ethernet
                    Console.WriteLine("Network connection is: Ethernet");

                    ni.EnableDhcp();
                }

                // wait for DHCP to complete
                WaitIP();
            }
            else
            {
                throw new NotSupportedException("ERROR: there is no network interface configured.\r\nOpen the 'Edit Network Configuration' in Device Explorer and configure one.");
            }
        }

        static void WaitIP()
        {
            Console.WriteLine("Wait for IP");

            while (true)
            {
                NetworkInterface ni = NetworkInterface.GetAllNetworkInterfaces()[0];
                if (ni.IPv4Address != null && ni.IPv4Address.Length > 0)
                {
                    if (ni.IPv4Address[0] != '0')
                    {
                        Console.WriteLine($"We have and IP: {ni.IPv4Address}");
                        break;
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
