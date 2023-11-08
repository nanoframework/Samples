// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;

using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;

namespace Central1
{
    /// <summary>
    /// Simple Bluetooth watcher 
    /// </summary>
    public static class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Central: Simple Bluetooth LE watcher");

            // Create a BluetoothLEAdvertisementWatcher to look for Bluetooth adverts.
            BluetoothLEAdvertisementWatcher watcher = new();

            // Use active scans to get extra information from devices, scan responses.
            watcher.ScanningMode = BluetoothLEScanningMode.Active;

            // Set up event to monitor received adverts
            watcher.Received += Watcher_Received;

            Console.WriteLine("=== Starting Bluetooth advert watcher ====");
            Console.WriteLine("Will run for 2 minutes and close down");
            watcher.Start();

            Thread.Sleep(120000);

            Console.WriteLine();
            Console.WriteLine("=== Stopping Bluetooth advert watcher ====");
            watcher.Stop();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            BluetoothLEAdvertisement adv = args.Advertisement;

            Console.WriteLine();
            Console.WriteLine($"=== Advert received ==== {DateTime.UtcNow}");
            Console.WriteLine($"Address:{args.BluetoothAddress:X} RSSI:{args.RawSignalStrengthInDBm}");
            Console.WriteLine($"Local name:{adv.LocalName}");

            // List Manufacturers data
            Console.WriteLine($"Manufacturers Data:{adv.ManufacturerData.Count}");
            foreach (BluetoothLEManufacturerData md in adv.ManufacturerData)
            {
                Console.WriteLine($"-- Company:{md.CompanyId} Length:{md.Data.Length}");
                DataReader dr = DataReader.FromBuffer(md.Data);
                byte[] bytes = new byte[md.Data.Length];
                dr.ReadBytes(bytes);

                foreach (byte b in bytes)
                {
                    Console.Write($"{b:X}");
                }
                Console.WriteLine();
            }

            // List Service UUIDS in Advertisement
            Console.WriteLine($"Service UUIDS:{adv.ServiceUuids.Length}");

            // There is limited space in adverts you may not get any service UUIDs
            // Maybe just the primary service.
            foreach(Guid uuid in adv.ServiceUuids)
            {
                Console.WriteLine($" - Advertised service:{uuid}");
            }
        }
    }
}
