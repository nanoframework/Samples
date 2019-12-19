﻿//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Runtime.Events;
using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace nanoFramework.Networking
{
    internal class NetworkHelpers
    {
        private const string c_SSID = "REPLACE-WITH-YOUR-SSID";
        private const string c_AP_PASSWORD = "REPLACE-WITH-YOUR-WIFI-KEY";

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

                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    // network interface is Wi-Fi
                    Console.WriteLine("Network connection is: Wi-Fi");

                    Wireless80211Configuration wc = Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];

                    // note on checking the 802.11 configuration
                    // on secure devices (like the TI CC3220SF) the password can't be read
                    // so we can't use the code block bellow to automatically set the profile
                    if ((wc.Ssid != c_SSID && wc.Password != c_AP_PASSWORD) &&
                         (wc.Ssid != "" && wc.Password == ""))
                    {
                        // have to update Wi-Fi configuration
                        wc.Ssid = c_SSID;
                        wc.Password = c_AP_PASSWORD;
                        wc.SaveConfiguration();
                    }
                    else
                    {
                        // Wi-Fi configuration matches
                        // (or can't be validated)
                    }
                }
                else
                {
                    // network interface is Ethernet
                    Console.WriteLine("Network connection is: Ethernet");
                }

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

        private static void SetDateTime()
        {
            Console.WriteLine("Setting up system clock...");

            // if SNTP is available and enabled on target device this can be skipped because we should have a valid date & time
            while (DateTime.UtcNow.Year < 2018)
            {
                Console.WriteLine("Waiting for valid date time...");
                // wait for valid date & time
                Thread.Sleep(1000);
            }

            DateTimeAvailable.Set();
        }

        private static void CheckIP()
        {
            Console.WriteLine("Checking for IP");

            NetworkInterface ni = NetworkInterface.GetAllNetworkInterfaces()[0];
            if (ni.IPv4Address != null && ni.IPv4Address.Length > 0)
            {
                if (ni.IPv4Address[0] != '0')
                {
                    Console.WriteLine($"We have and IP: {ni.IPv4Address}");
                    IpAddressAvailable.Set();
                }
            }
        }

        static void AddressChangedCallback(object sender, EventArgs e)
        {
            CheckIP();
        }
    }
}
