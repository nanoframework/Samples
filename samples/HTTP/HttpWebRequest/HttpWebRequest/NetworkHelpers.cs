//
// Copyright (c) .NET Foundation and Contributors
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
        private const string c_SSID = "<replace-with-valid-ssid";
        private const string c_AP_PASSWORD = "<replace-with-valid-password>";

        private static bool _requiresDateTime;

        static public ManualResetEvent IpAddressAvailable = new ManualResetEvent(false);
        static public ManualResetEvent DateTimeAvailable = new ManualResetEvent(false);

        internal void SetupAndConnectNetwork(bool requiresDateTime = false)
        {
            NetworkChange.NetworkAddressChanged += AddressChangedCallback;
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;

            _requiresDateTime = requiresDateTime;
            new Thread(WorkingThread).Start();
        }

        private static void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Debug.WriteLine("Network availability changed");
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
                    Debug.WriteLine("Network connection is: Wi-Fi");

                    //Wireless80211Configuration wc = Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];

                    //// note on checking the 802.11 configuration
                    //// on secure devices (like the TI CC3220SF) the password can't be read
                    //// so we can't use the code block bellow to automatically set the profile
                    //if ( (wc.Ssid != c_SSID && wc.Password != c_AP_PASSWORD) &&
                    //     (wc.Ssid != "" && wc.Password != ""))
                    //{
                    //    // have to update Wi-Fi configuration
                    //    wc.Ssid = c_SSID;
                    //    wc.Password = c_AP_PASSWORD;
                    //    wc.SaveConfiguration();
                    //}
                    //else
                    //{
                    //    // Wi-Fi configuration matches
                    //    // (or can't be validated)
                    //}
                }
                else
                {
                    // network interface is Ethernet
                    Debug.WriteLine("Network connection is: Ethernet");
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
            int retryCount = 30;

            Debug.WriteLine("Waiting for a valid date & time...");

            // if SNTP is available and enabled on target device this can be skipped because we should have a valid date & time
            while ((DateTime.UtcNow.Year < 2018) || (DateTime.UtcNow.Year > 2099))
            {
                // force update if we haven't a valid time after 30 seconds
                if (retryCount-- == 0)
                {
                    Debug.WriteLine("Forcing SNTP update...");

                    Sntp.UpdateNow();

                    // reset counter
                    retryCount = 30;
                }

                // wait for valid date & time
                Thread.Sleep(1000);
            }

            Debug.WriteLine($"We have valid date & time: {DateTime.UtcNow.ToString()}");

            DateTimeAvailable.Set();
        }

        private static bool CheckIP()
        {
            Debug.WriteLine("Checking for IP");

            var ni = NetworkInterface.GetAllNetworkInterfaces()[0];

            if (ni.IPv4Address != null && ni.IPv4Address.Length > 0)
            {
                if (ni.IPv4Address[0] != '0')
                {
                    Debug.WriteLine($"We have and IP: {ni.IPv4Address}");
                    IpAddressAvailable.Set();

                    return true;
                }
            }

            Debug.WriteLine("NO IP");

            return false;
        }

        private static void AddressChangedCallback(object sender, EventArgs e)
        {
            CheckIP();
        }
    }
}
