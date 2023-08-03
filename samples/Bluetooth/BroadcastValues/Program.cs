//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;

namespace BroadcastValues
{
    public class Program
    {
        private static Random rnd = new Random();
        private static Byte ValueCount = 1;

        public static void Main()
        {
            Console.WriteLine("Broadcast Values in Advertisement");

            try
            {
                // Loop updating values every 1 second
                while (true)
                {
                    BluetoothLEAdvertisement advertisement = new BluetoothLEAdvertisement();
                    advertisement.LocalName = "MyValues";

                    // Create ManufacturerData buffer with values we want to broadcast
                    DataWriter writer = new DataWriter();

                    // Add 1 byte count and an integer to buffer. This could be any data you want to broadcast, string, int, double.
                    // The only restriction is the advertisement can not be greater then 31 bytes length.
                    // For each data section in advertisement there is an overhead of 2 bytes (Length & Type)
                    // This advertisement will comprise of LocalName(10 bytes) and ManufacturerData(9 bytes)
                    writer.WriteByte(ValueCount++);
                    writer.WriteInt32(GetValue());

                    // Add Manufacturer Data section to advertisement using uknown manufacturer ID
                    advertisement.ManufacturerData.Add(new BluetoothLEManufacturerData(0xfffe, writer.DetachBuffer()));

                    // Create Publisher and start advertising data values
                    BluetoothLEAdvertisementPublisher publisher = new BluetoothLEAdvertisementPublisher(advertisement);
                    publisher.Start();

                    // Wait 1 seconds 
                    Thread.Sleep(1000);

                    publisher.Stop();
                }
            }
            catch (Exception) { }

            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        /// Dummy method to provide next value to broadcast. This could be reading a temperature sensor or some device
        /// </summary>
        /// <returns>Integer value.</returns>
        public static int GetValue()
        {
            int newValue = rnd.Next();

            return newValue;
        }
    }
}
