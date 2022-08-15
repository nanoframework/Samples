
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
    /// the Server Sample 3.
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
        // Devices to collect from. Added by watcher
        private readonly static ArrayList s_foundDevices = new();

        public static void Main()
        {
            Console.WriteLine("Sample Client/Central 2 : Collect data from Environmental sensors");

            Console.WriteLine("Searching for Environmental Sensors");

            // Find all advertising Environmental sensors (Sample 3)
            FindDevices();

            Console.WriteLine($"Devices found {s_foundDevices.Count}");

            Console.WriteLine("Connecting and Reading Sensors");

            // Connect and collect temperatures
            DataCollector();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void FindDevices()
        {
            BluetoothLEAdvertisementWatcher watcher = new();
            watcher.ScanningMode = BluetoothLEScanningMode.Active;
            watcher.Received += Watcher_Received;

            Console.WriteLine("Starting BluetoothLEAdvertisementWatcher");

            watcher.Start();

            // Run until we have found some devices
            while (s_foundDevices.Count == 0)
            {
                Thread.Sleep(15000);
            }

            Console.WriteLine("Stopping BluetoothLEAdvertisementWatcher");

            watcher.Stop();
        }

        private static void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (args.Advertisement.LocalName.StartsWith("Sample"))
            {
                Console.WriteLine($"Found Environmental sensor :{args.BluetoothAddress:X}");

                // Check we haven't already found this device
                foreach (BluetoothLEDevice dev in s_foundDevices)
                {
                    if (dev.BluetoothAddress == args.BluetoothAddress)
                    {
                        // found ignore
                        return;
                    }
                }

                // Add it to list as a BluetoothLEDevice
                s_foundDevices.Add(BluetoothLEDevice.FromBluetoothAddress(args.BluetoothAddress));
            }
        }


        private static void DataCollector()
        {
            bool collectorRunning = true;

            foreach (BluetoothLEDevice device in s_foundDevices)
            {
                // Monitor status
                device.ConnectionStatusChanged += Device_ConnectionStatusChanged;

                ConnectAndRegister(device);
            }

            while (collectorRunning)
            {
                Thread.Sleep(30000);
                foreach (BluetoothLEDevice device in s_foundDevices)
                {
                    if (device.ConnectionStatus != BluetoothConnectionStatus.Connected)
                    {
                        Console.WriteLine($"Device {device.BluetoothAddress:X} is disconnected, try reconnect");
                        // try to reconnect
                        if (ConnectAndRegister(device))
                        {
                            Console.WriteLine($"Device {device.BluetoothAddress:X} re-connected");
                        }
                        else
                        {
                            Console.WriteLine($"Device {device.BluetoothAddress:X} unable to reconnect");
                        }
                    }
                }
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

            // Get Environmental Sensor service which contain Temperature Characteristics
            GattDeviceServicesResult sr = device.GetGattServicesForUuid(GattServiceUuids.EnvironmentalSensing);
            if (sr.Status == GattCommunicationStatus.Success)
            {
                // Connected and services read
                result = true;

                // Pick up all temperature characteristics
                foreach (GattDeviceService service in sr.Services)
                {
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

                // Reconnect in main loop
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
