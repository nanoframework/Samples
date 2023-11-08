//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//
//

using System;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;

namespace WatcherFilters
{
    /// <summary>
    /// Watcher filter are useful to reduce the number of advertisements to be processed in application
    /// and reduce memory used by nanoFramework keeping track of device in and out of range when using SignalFilter.
    /// 
    /// With the right filter you will not need to have extra checks in received advertisement event handler. 
    /// 
    /// Use the BluetoothSample2 sample as a test device for this example.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Demonstrate Watcher filters");

            BluetoothLEAdvertisementWatcher watcher = new BluetoothLEAdvertisementWatcher();
            watcher.ScanningMode = BluetoothLEScanningMode.Active;
            watcher.Received += OnAdvertisementReceived;

            // AdvertisementFilter - Data Section filter
            // An advertisements contains a number of data sections of different types. e.g Local Name or
            // service UUID. See Bluetooth supplement to the Bluetooth core specification for section layout and the
            // Bluetooth assigned numbers for data types values.

            // The AdvertisementFilter selects advertisements based on having the same data in
            // the AdvertisementFilter.Advertisement and the AdvertisementFilter.BytePatterns.
            // AdvertisementFilter.BytePatterns select on partial data at an offset in a data section.
            // All data has to match for advert to be accepted.

            // For simplicity of this example we are only selecting advert based on the Complete Local name
            // which is a data type of 9 in the advertisement. We are using both the DataSection and Byte patterns
            // for the example. Normally you won't do both on same section, it wouldn't make sense.

            // Only select advertisements with local name equal to "Sample2"
            // Setting the LocalName property will create a data section in the Advertisement for the local name.
            watcher.AdvertisementFilter.Advertisement.LocalName = "Sample2";

            // Selecting by Byte pattern
            // Select all Advertisements with an "a" in 2nd position of Local name
            BluetoothLEAdvertisementBytePattern pattern1 = new BluetoothLEAdvertisementBytePattern()
            {
                DataType = (byte)BluetoothLEAdvertisementDataSectionType.CompleteLocalName,
                Data = new Buffer(new Byte[] { (Byte)'a' }),
                Offset = 1
            };

            watcher.AdvertisementFilter.BytePatterns.Add(pattern1);

            // 2nd byte pattern
            // Select all Advertisements with an "e" in 5th position of Local name
            BluetoothLEAdvertisementBytePattern pattern2 = new BluetoothLEAdvertisementBytePattern()
            {
                DataType = (byte)BluetoothLEAdvertisementDataSectionType.CompleteLocalName,
                Data = new Buffer(new Byte[] { (Byte)'e' }),
                Offset = 5
            };

            watcher.AdvertisementFilter.BytePatterns.Add(pattern2);

            // If you want to match a specific whole data section of a type
            // you can add the data as a specific data section like appearance
            Buffer apbuffer = new Buffer(new Byte[] { 1 });
            watcher.AdvertisementFilter.Advertisement.DataSections.Add(
                new BluetoothLEAdvertisementDataSection((byte)BluetoothLEAdvertisementDataSectionType.Appearance, apbuffer));

            // Display contents of the filter
            DisplayFilter(watcher.AdvertisementFilter);

            // Signal Strength Filter
            // The signal filter is used to filter advertisements based on there RSSI signal strength which determines
            // if device is in or out of range.
            // See Microsoft documentation on BluetoothSignalStrengthFilter Class for more details.
            // Setting the InRangeThresholdInDBm != -127 will enable the Signal Strength Filter.
            
            //watcher.SignalStrengthFilter.InRangeThresholdInDBm = -90;
            //watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -95;
            //watcher.SignalStrengthFilter.OutOfRangeTimeout = new TimeSpan(0,0,10);

            // If a device is out of range for OutOfRangeTimeout then an event will be fired for device with
            // a RSSI value of 

            watcher.Start();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, nanoFramework.Device.Bluetooth.Advertisement.BluetoothLEAdvertisementReceivedEventArgs args)
        {
            DisplayAdvert(args);
        }

        static void DisplayFilter(BluetoothLEAdvertisementFilter filter)
        {
            Console.WriteLine($"== Advertisement Filter ==");

            // Display data sections
            Console.WriteLine($"Data sections");
            foreach (BluetoothLEAdvertisementDataSection ds in filter.Advertisement.DataSections)
            {
                Console.WriteLine($"Type:{ds.DataType} Data Length:{ds.Data.Length}");
            }

            Console.WriteLine($"Byte Patterns");
            foreach (BluetoothLEAdvertisementBytePattern bp in filter.BytePatterns)
            {
                Console.WriteLine($"Type:{bp.DataType} Data Length:{bp.Data.Length} Offset:{bp.Offset}");
            }
            Console.WriteLine($"== Advertisement Filter End ==");
        }

        static void DisplayAdvert(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            BluetoothLEAdvertisement adv = args.Advertisement;

            Console.WriteLine($"== {DateTime.UtcNow} == ");
            Console.WriteLine($"Advert received - type {args.AdvertisementType} - RSSI {args.RawSignalStrengthInDBm}");
            Console.WriteLine($"Address:{args.BluetoothAddress:X}");
            Console.WriteLine($"Local name:{adv.LocalName}");
            Console.WriteLine($"Manufacturers Data:{adv.ManufacturerData.Count}");

            //Manufacturer Data 
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

            Console.WriteLine($"Service UUIDS:{adv.ServiceUuids.Length}");

            // Any Service Uuids
            foreach (Guid uuid in adv.ServiceUuids)
            {
                Console.WriteLine($" - Advertised service:{uuid}");
            }
        }
    }
}
