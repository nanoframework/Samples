using System;
using System.Diagnostics;
using System.Threading;
using System.Device.Wifi;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Runtime.Native;

using nanoFramework.Device.Bluetooth.Spp;

namespace BluetoothLESerial
{
    public static class Program
    {
        static NordicSpp spp;
        static WifiAdapter wifi;

        public static void Main()
        {
            Debug.WriteLine("\nSerial Terminal over Bluetooth LE Sample");

            // Create Instance of Bluetooth Serial profile
            spp = new NordicSpp();

            // Add event handles for received data and Connections 
            spp.ReceivedData += Spp_ReceivedData;
            spp.ConnectedEvent += Spp_ConnectedEvent;

            // Start Advertising SPP service
            spp.Start("nanoFrameworkSerial");

            while (true)
            {
                Thread.Sleep(10000);
                if (spp.IsConnected && !spp.SendString($"Current device time:{DateTime.UtcNow}\n"))
                {
                    Debug.WriteLine($"Send failed!");
                }
            }
        }


        private static void Spp_ConnectedEvent(IBluetoothSpp sender, EventArgs e)
        {
            if (spp.IsConnected)
            {
                spp.SendString($"Welcome to Bluetooth Serial sample\n");
                spp.SendString($"Send 'help' for options\n");
            }

            Debug.WriteLine($"Client connected:{sender.IsConnected}");
        }

        private static void Spp_ReceivedData(IBluetoothSpp sender, SppReceivedDataEventArgs ReadRequestEventArgs)
        {
            string message = ReadRequestEventArgs.DataString;
            Debug.WriteLine($"Received=>{message}");

            string[] args = message.Trim().Split(' ');
            if (args.Length != 0)
            {
                switch (args[0].ToLower())
                {
                    // Scan for wifi networks
                    case "scan":
                        InitWifiScan();
                        sender.SendString("Scanning Networks\n");
                        wifi.ScanAsync();
                        break;

                    // Dummy set Wifi credentials
                    case "wifi":
                        if (args.Length != 3)
                        {
                            sender.SendString("Wrong number of arguments\n");
                            break;
                        }
                        sender.SendString("Set Wifi credentials\n");

                        // Save credentials Here

                        break;

                    // Send current ESP32 native memory
                    case "mem":
                        uint totalSize, totalFreeSize, largestBlock;
                        NativeMemory.GetMemoryInfo(NativeMemory.MemoryType.All, out totalSize, out totalFreeSize, out largestBlock);
                        sender.SendString($"Native memory - total:{totalSize} Free:{totalFreeSize} largest:{largestBlock}\n");
                        break;

                    // Reboot device
                    case "reboot":
                        sender.SendString("Rebooting now\n");
                        Thread.Sleep(100);
                        Power.RebootDevice();
                        break;

                    // Some help
                    case "help":
                        sender.SendString("Help\n");
                        sender.SendString("-------------------------------------------\n");
                        sender.SendString("'scan' - Scan Wifi networks\n");
                        sender.SendString("'mem' - Show native free memory\n");
                        sender.SendString("'reboot' - Reboot device\n");
                        sender.SendString("'wifi ssid password' - Set WiFI credentials\n");
                        sender.SendString("-------------------------------------------\n");
                        break;

                }
            }
        }

        #region Wifi Scanning
        private static void InitWifiScan()
        {
            if (wifi == null)
            {
                // Get the first WiFI Adapter
                wifi = WifiAdapter.FindAllAdapters()[0];

                // Set up the AvailableNetworksChanged event to pick up when scan has completed
                wifi.AvailableNetworksChanged += Wifi_AvailableNetworksChanged;
            }
        }

        private static void Wifi_AvailableNetworksChanged(WifiAdapter sender, object e)
        {
            if (spp.IsConnected)
            {
                // Get Report of all scanned Wifi networks
                WifiNetworkReport report = sender.NetworkReport;

                // Enumerate though networks and send to client
                foreach (WifiAvailableNetwork net in report.AvailableNetworks)
                {
                    // Show all networks found
                    if (spp.IsConnected)
                    {
                        spp.SendString($"Net SSID :{net.Ssid},  BSSID : {net.Bsid},  rssi : {net.NetworkRssiInDecibelMilliwatts.ToString()},  signal : {net.SignalBars.ToString()}\n");
                    }
                }
            }
        }
        #endregion
    }
}
