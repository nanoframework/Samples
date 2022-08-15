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
            Console.WriteLine($"=== Advert received ====");
            Console.WriteLine($"Address:{args.BluetoothAddress:X}");
            Console.WriteLine($"Local name:{adv.LocalName}");
            Console.WriteLine($"Manufacturers Data:{adv.ManufacturerData.Count}");
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
