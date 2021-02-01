//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Runtime.Events;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;

namespace nanoFramework.Networking
{
    public class NetworkHelpers
    {
        private static bool _requiresDateTime;

        static public ManualResetEvent IpAddressAvailable = new ManualResetEvent(false);
        static public ManualResetEvent DateTimeAvailable = new ManualResetEvent(false);

        internal static void SetupAndConnectNetwork(bool requiresDateTime = false)
        {
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangedCallback);

            _requiresDateTime = requiresDateTime;

            new Thread(WorkingThread).Start();
        }

        internal static void WorkingThread()
        {
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();

            if (nis.Length > 0)
            {
                // get the first interface
                NetworkInterface ni = nis[0];

                ni.EnableAutomaticDns();
                ni.EnableDhcp();

                // check if we have an IP
                CheckIP();

                if (_requiresDateTime)
                {
                    IpAddressAvailable.WaitOne();

                    SetDateTime();
                }
            }
            else
            {
                throw new NotSupportedException("ERROR: there is no network interface configured.\r\nOpen the 'Edit Network Configuration' in Device Explorer and configure one.");
            }
        }

        public static void SetDateTime()
        {
            Debug.WriteLine("Setting up system clock...");

            // if SNTP is available and enabled on target device this can be skipped because we should have a valid date & time
            while (DateTime.UtcNow.Year < 2018)
            {
                Debug.WriteLine("Waiting for valid date time...");
                // wait for valid date & time
                Thread.Sleep(1000);
            }


            Debug.WriteLine($"System time is: {DateTime.UtcNow.ToString()}");

            DateTimeAvailable.Set();
        }

        public static bool CheckIP()
        {
            Debug.WriteLine("Checking for IP");

            NetworkInterface ni = NetworkInterface.GetAllNetworkInterfaces()[0];
            if (ni.IPv4Address != null && ni.IPv4Address.Length > 0)
            {
                if (ni.IPv4Address[0] != '0')
                {
                    Debug.WriteLine($"We have and IP: {ni.IPv4Address}");
                    IpAddressAvailable.Set();
                    return true;
                }
            }

            return false;
        }

        static void AddressChangedCallback(object sender, EventArgs e)
        {
            CheckIP();
        }
    }
}
