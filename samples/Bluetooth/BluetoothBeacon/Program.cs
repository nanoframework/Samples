//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;

namespace BluetoothBeacon
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Beacon Sample");

            beacon();

            Thread.Sleep(Timeout.Infinite);
        }

        public static void beacon()
        {
            Guid proximityUUID = new Guid("E2C56DB5-DFFB-48D2-B060-D0F5A71096E0");

            iBeacon beacon = new iBeacon(proximityUUID, 0, 1, -59);
            beacon.Start();

            Thread.Sleep((int)Timeout.Infinite);

            beacon.Stop();
        }

        private static void Publisher_StatusChanged(object sender, BluetoothLEAdvertisementPublisherStatusChangedEventArgs args)
        {
            BluetoothLEAdvertisementPublisher pub = sender as BluetoothLEAdvertisementPublisher;

            Console.WriteLine($"Status:{args.Status} Error:{args.Error} TxPowerLevel:{args.SelectedTransmitPowerLevelInDBm}");
        }
    }
}
