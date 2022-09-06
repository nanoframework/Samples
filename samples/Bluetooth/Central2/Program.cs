
//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using System.Collections;

using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace Central2
{
    /// <summary>
    /// Sample to collect temperature values from a collection of
    /// Environmental Sensor devices. Designed to work with 
    /// the Bluetooth Sample 3.
    /// 
    /// It will first watch for advertisements from Sensor devices 
    /// for 15 seconds after finding first device.
    /// Then stop watcher and connect and collect temperature
    /// changes from the found devices.
    /// 
    /// Note: You can not run watcher and connect to devices at same time.
    /// </summary>
    public static class Program
    {
        // Devices found by watcher
        private readonly static Hashtable s_foundDevices = new();

        // Devices to collect from. Added when connected
        private readonly static Hashtable s_dataDevices = new();

        public static void Main()
        {
            Console.WriteLine("Sample Client/Central 2 : Collect data from Environmental sensors");
            Console.WriteLine("Searching for Environmental Sensors");

            // Create a watcher
            BluetoothLEAdvertisementWatcher watcher = new();
            watcher.ScanningMode = BluetoothLEScanningMode.Active;
            watcher.Received += Watcher_Received;

            while (true)
            {
                Console.WriteLine("Starting BluetoothLEAdvertisementWatcher");
                watcher.Start();

                // Run until we have found some devices to connect to
                while (s_foundDevices.Count == 0)
                {
                    Thread.Sleep(10000);
                }

                Console.WriteLine("Stopping BluetoothLEAdvertisementWatcher");

                // We can't connect if watch running so stop it.
                watcher.Stop();

                Console.WriteLine($"Devices found {s_foundDevices.Count}");
                Console.WriteLine("Connecting and Reading Sensors");

                foreach (DictionaryEntry entry in s_foundDevices)
                {
                    BluetoothLEDevice device = entry.Value as BluetoothLEDevice;

                    // Connect and register notify events
                    if (ConnectAndRegister(device))
                    {
                        if (s_dataDevices.Contains(device.BluetoothAddress))
                        {
                            s_dataDevices.Remove(device.BluetoothAddress);
                        }
                        s_dataDevices.Add(device.BluetoothAddress, device);
                    }
                }
                s_foundDevices.Clear();
            }
        }

        /// <summary>
        /// Check fir device with correct Service UUID in advert and not already found
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool IsValidDevice(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (args.Advertisement.ServiceUuids.Length > 0 &&
                args.Advertisement.ServiceUuids[0].Equals(new Guid("A7EEDF2C-DA87-4CB5-A9C5-5151C78B0057")))
            {
                if (!s_foundDevices.Contains(args.BluetoothAddress))
                {
                    return true;
                }
            }

            return false;
        }

        private static void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            // Print information about received advertisement
            // You don't receive all information in 1 event and it can be split across 2 events
            // AdvertisementTypes 0 and 4
            Console.WriteLine($"Received advertisement address:{args.BluetoothAddress:X}/{args.BluetoothAddressType} Name:{args.Advertisement.LocalName}  Advert type:{args.AdvertisementType}  Services:{args.Advertisement.ServiceUuids.Length}");

            if (args.Advertisement.ServiceUuids.Length > 0)
            {
                Console.WriteLine($"Advert Service UUID {args.Advertisement.ServiceUuids[0]}");
            }

            // Look for advert with our primary service UUID from Bluetooth Sample 3
            if (IsValidDevice(args))
            {
                Console.WriteLine($"Found an Environmental test sensor :{args.BluetoothAddress:X}");

                // Add it to list as a BluetoothLEDevice
                s_foundDevices.Add(args.BluetoothAddress, BluetoothLEDevice.FromBluetoothAddress(args.BluetoothAddress, args.BluetoothAddressType));
            }
        }


        /// <summary>
        /// Connect and set-up Temperature Characteristics for value 
        /// changed notifications.
        /// </summary>
        /// <param name="device">Bluetooth device</param>
        /// <returns>True if device connected</returns>
        private static bool ConnectAndRegister(BluetoothLEDevice device)
        {
            bool result = false;

            GattDeviceServicesResult sr = device.GetGattServicesForUuid(GattServiceUuids.EnvironmentalSensing);
            if (sr.Status == GattCommunicationStatus.Success)
            {
                // Connected and services read
                result = true;

                // Pick up all temperature characteristics
                foreach (GattDeviceService service in sr.Services)
                {
                    Console.WriteLine($"Service UUID {service.Uuid}");

                    GattCharacteristicsResult cr = service.GetCharacteristicsForUuid(GattCharacteristicUuids.Temperature);
                    if (cr.Status == GattCommunicationStatus.Success)
                    {
                        //Temperature characteristics found now read value and 
                        //set up notify for value changed
                        foreach (GattCharacteristic gc in cr.Characteristics)
                        {
                            // Read current temperature
                            GattReadResult rr = gc.ReadValue();
                            if (rr.Status == GattCommunicationStatus.Success)
                            {
                                // Read current value and output
                                OutputTemp(gc, ReadTempValue(rr.Value));

                                // Set up a notify value changed event
                                gc.ValueChanged += TempValueChanged;
                                // and configure CCCD for Notify
                                gc.WriteClientCharacteristicConfigurationDescriptorWithResult(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private static void Device_ConnectionStatusChanged(object sender, EventArgs e)
        {
            BluetoothLEDevice dev = (BluetoothLEDevice)sender;
            if (dev.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
            {
                Console.WriteLine($"Device {dev.BluetoothAddress:X} disconnected");

                // Remove device. We get picked up again once advert seen.
                s_dataDevices.Remove(dev.BluetoothAddress);
                dev.Dispose();
            }
        }

        private static float ReadTempValue(Buffer value)
        {
            DataReader rdr = DataReader.FromBuffer(value);
            return (float)rdr.ReadInt16() / 100;
        }

        private static void OutputTemp(GattCharacteristic gc, float value)
        {
            Console.WriteLine($"New value => Device:{gc.Service.Device.BluetoothAddress:X} Sensor:{gc.UserDescription,-20}  Current temp:{value}");
        }

        private static void TempValueChanged(GattCharacteristic sender, GattValueChangedEventArgs valueChangedEventArgs)
        {
            OutputTemp(sender,
                ReadTempValue(valueChangedEventArgs.CharacteristicValue));
        }
    }
}
