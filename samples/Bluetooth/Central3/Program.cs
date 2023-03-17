//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;

using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace Central3
{
    /// <summary>
    /// Sample to show how to Watch for advertisements and connect to a device which requires 
    /// pairing and authentication to access the Characteristics value.
    /// Pairing can be done before or ad hoc when the accessing the Characteristic.
    /// If Characteristic requires security or Authentication when accessed the system will automatically start a pairing operation.
    /// </summary>
    public static class Program
    {
        public static bool s_deviceFound = false;
        public static BluetoothLEDevice s_device;

        public const int PASSKEY = 654321;

        public static void Main()
        {
            Console.WriteLine("Central: Simple Bluetooth LE watcher");

            // Create a BluetoothLEAdvertisementWatcher to look for Bluetooth adverts.
            BluetoothLEAdvertisementWatcher watcher = new();

            // Get Scan response for Advertisement 
            watcher.ScanningMode = BluetoothLEScanningMode.Active;

            // Set up event to monitor received adverts
            watcher.Received += Watcher_Received;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Starting Bluetooth advert watcher ====");
                MemoryCheck("before");

                watcher.Start();

                MemoryCheck("start");

                // Wait for device to be found
                while (s_deviceFound == false)
                {
                    Thread.Sleep(100);
                }

                Console.WriteLine();
                Console.WriteLine("=== Stopping Bluetooth advert watcher ====");
                watcher.Stop();

                MemoryCheck("stop");

                // Now Connect to device

                TestConnectAndPair(s_device);

                
                s_device.Dispose();
                s_device = null;

                MemoryCheck("connect");

                Thread.Sleep(4000);

                s_deviceFound = false;
            }
        }

        /// <summary>
        /// With low memory devices, no spiram you can have problems with low native memory
        /// </summary>
        /// <param name="info"></param>
        static void MemoryCheck(string info)
        {
            uint manMem = nanoFramework.Runtime.Native.GC.Run(true);

            uint total;
            uint free;
            uint largest;

            nanoFramework.Hardware.Esp32.NativeMemory.GetMemoryInfo( nanoFramework.Hardware.Esp32.NativeMemory.MemoryType.All, out total, out free, out largest);
            Console.WriteLine($"Memory All ({info}) Managed:{manMem} Native:{total}/{free}/{largest}");

            nanoFramework.Hardware.Esp32.NativeMemory.GetMemoryInfo(nanoFramework.Hardware.Esp32.NativeMemory.MemoryType.Internal, out total, out free, out largest);
            Console.WriteLine($"Memory Internal ({info}) Managed:{manMem} Native:{total}/{free}/{largest}");
        }

        /// <summary>
        /// Check for device for correct Service UUID in Advertisement
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool IsValidDevice(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (args.Advertisement.ServiceUuids.Length > 0 &&
                args.Advertisement.ServiceUuids[0].Equals(new Guid("A7EEDF2C-DA8C-4CB5-A9C5-5151C78B0057")))
            {
                return true;
            }

            return false;
        }

        private static void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            // Ignore any further events while found device queried
            // Sometimes you will get a previously queued event after Watcher has been stopped
            if (Program.s_deviceFound == true)
            {
                return;
            }

            DisplayAdvert(args);

            // Look for advert with our primary service UUID from Bluetooth Sample 3
            if (IsValidDevice(args) && Program.s_deviceFound == false)
            {
                Console.WriteLine($"Found device with service :{args.BluetoothAddress:X}");

                Program.s_device = BluetoothLEDevice.FromBluetoothAddress(args.BluetoothAddress, args.BluetoothAddressType);
                Program.s_deviceFound = true;
            }
        }

        static void DisplayAdvert(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            BluetoothLEAdvertisement adv = args.Advertisement;

            Console.WriteLine();
            Console.WriteLine($"=== Advert received ====");
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

        /// <summary>
        /// Method to connect to device
        /// - (Optional pair) commented out as it will automatically pair on Read value
        /// - Connect
        /// - Get services & Characteristics
        /// - Find Characteristic which authentication and read value
        /// </summary>
        /// <param name="device"></param>
        static void TestConnectAndPair(BluetoothLEDevice device)
        {
            // Set up some events to handle
            // - PairingRequested : Event for providing/Displaying Passkey
            // - PairingComple    : Pairing operation has completed (with or without error)
            device.Pairing.PairingRequested += Pairing_PairingRequested;
            device.Pairing.PairingComplete += Pairing_PairingComplete;

            // Set IOCapabilities and ProtectionLevel for device
            device.Pairing.IOCapabilities = DevicePairingIOCapabilities.KeyboardOnly;
            device.Pairing.ProtectionLevel = DevicePairingProtectionLevel.EncryptionAndAuthentication;

            // Pair with found device
            Console.WriteLine($"=== Pair & Connect to {device.BluetoothAddress:X} ====");
            DevicePairingResult pairResult = device.Pairing.Pair();

            // Display result of pairing, if successful then IsPaired and optionally IsAuthenticated will be true
            Console.WriteLine($"Pairing result:{pairResult.Status}");
            Console.WriteLine($" -IsAuthenticated:{device.Pairing.IsAuthenticated}");
            Console.WriteLine($" -IsPaired:{device.Pairing.IsPaired}");
            Console.WriteLine($" -IoCaps:{device.Pairing.IOCapabilities}");

            if (pairResult.Status == DevicePairingResultStatus.Paired)
            {
                Console.WriteLine($"Connection status {device.ConnectionStatus}");

                Console.WriteLine($"From Generic Access service for connect device");
                Console.WriteLine($"- Device name {device.Name}");
                Console.WriteLine($"- Appearance {device.Appearance:X}");

                GattDeviceServicesResult srvsResult = device.GetGattServices();
                if (srvsResult.Status == GattCommunicationStatus.Success)
                {
                    GattDeviceService[] Services = srvsResult.Services;

                    // List available services
                    Console.WriteLine($"=== Device available services ====");
                    foreach (GattDeviceService srv in Services)
                    {
                        Console.WriteLine($" Service:{srv.Uuid}");

                        // For each service get available Characteristics
                        GattCharacteristicsResult cr = srv.GetCharacteristics();
                        if (cr.Status == GattCommunicationStatus.Success)
                        {
                            foreach(GattCharacteristic characteristic in cr.Characteristics)
                            {
                                Console.WriteLine($"Characteristic -> {characteristic.Uuid}");

                                // If Characteristic has this UUID (from sample 2) then try to access data
                                if (characteristic.Uuid.Equals(new Guid("A7EEDF2C-DA8F-4CB5-A9C5-5151C78B0057")))
                                {
                                    Console.WriteLine($"Characteristic found access, read value");

                                    // Reading a value that requires Authentication will start a pairing
                                    // PairingRequested event handler supplies Authentication pin.
                                    var res = characteristic.ReadValue();
                                    if (res.Status == GattCommunicationStatus.Success)
                                    {
                                        DataReader dr = DataReader.FromBuffer(res.Value);
                                        int value = dr.ReadInt32();

                                        Console.WriteLine($"Chr value = {value}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Error access chr !!! {res.Status}");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Close connection to free up resources
            device.Close();
        }

        /// <summary>
        /// Event called when a pairing operation has completed
        /// </summary>
        /// <param name="sender">DevicePairing object</param>
        /// <param name="args">DevicePairingEventArgs args</param>
        private static void Pairing_PairingComplete(object sender, DevicePairingEventArgs args)
        {
            // Pick up DevicePairing from sender or just use it directly
            DevicePairing pairing = (DevicePairing)sender;

            if (args.Status == DevicePairingResultStatus.Paired)
            {
                Console.WriteLine($"PairingComplete:{args.Status} IOCaps:{pairing.IOCapabilities} IsPaired:{pairing.IsPaired} IsAuthenticated:{pairing.IsAuthenticated}");
            }
            else
            {
                Console.WriteLine($"PairingComplete failed - status = {args.Status}");
            }
        }

        /// <summary>
        /// Event called when passkey is required, check args.PairingKind for type of request.
        /// </summary>
        /// <param name="sender">DevicePairing object</param>
        /// <param name="args">DevicePairingRequestedEventArgs object</param>
        private static void Pairing_PairingRequested(object sender, DevicePairingRequestedEventArgs args)
        {
            Console.WriteLine($"Pairing_PairingRequested:{args.PairingKind}");

            switch (args.PairingKind)
            {
                case DevicePairingKinds.ProvidePin:
                    // Provide valid passcode
                    args.Accept(PASSKEY);
                    break;

                default:
                    Console.WriteLine($"Unhandled Pairing request");
                    break;
            }
        }
    }
}
