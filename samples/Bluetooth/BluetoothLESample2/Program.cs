//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;

using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

/// <summary>
/// Bluetooth Sample 2 is a custom service which shows the use of:
/// 
/// - Adding security to characteristics
/// - 1st characteristic you can read without pairing but will need to be paired to write
/// - 2nd characteristic you can write without pairing but will need to be paired to Read
/// - Both characteristic read/write same value.
/// 
/// You will be able to connect to the service and read values or subscribe to Notified ever 10 seconds.
/// Suitable Phone apps: "LightBlue" or "nRF Connect"
/// </summary>
namespace BluetoothLESample2
{
    public class Program
    {
        static GattLocalCharacteristic _readWriteCharacteristic1;
        static GattLocalCharacteristic _readWriteCharacteristic2;

        // value used to read/write
        static Int32 _value;

        public static void Main()
        {
            Debug.WriteLine("Hello from Bluetooth Sample 2");

            Guid serviceUuid = new Guid("A7EEDF2C-DA8C-4CB5-A9C5-5151C78B0057");
            Guid writeCharUuid1 = new Guid("A7EEDF2C-DA8D-4CB5-A9C5-5151C78B0057");
            Guid writeCharUuid2 = new Guid("A7EEDF2C-DA8E-4CB5-A9C5-5151C78B0057");

            //The GattServiceProvider is used to create and advertise the primary service definition.
            //An extra device information service will be automatically created.
            GattServiceProviderResult result = GattServiceProvider.Create(serviceUuid);
            if (result.Error != BluetoothError.Success)
            {
                return;
            }

            GattServiceProvider serviceProvider = result.ServiceProvider;

            // Get created Primary service from provider
            GattLocalService service = serviceProvider.Service;

            #region Characteristic 1
            // Add Read Characteristic for data that changes to service
            // We also want the connected client to be notified when value changes so we add the notify property
            GattLocalCharacteristicResult characteristicResult = service.CreateCharacteristic(writeCharUuid1,
                new GattLocalCharacteristicParameters()
                {
                    CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Write,
                    UserDescription = "My Read/Write Characteristic 1",
                    ReadProtectionLevel = GattProtectionLevel.Plain,
                    WriteProtectionLevel = GattProtectionLevel.EncryptionRequired
                });
            ;

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }

            // Get reference to our read Characteristic  
            _readWriteCharacteristic1 = characteristicResult.Characteristic;

            // Set up event handlers for read/write
            _readWriteCharacteristic1.WriteRequested += _writeCharacteristic_WriteRequested;
            _readWriteCharacteristic1.ReadRequested += _readWriteCharacteristic_ReadRequested;
            #endregion

            #region Characteristic 1
            // Add Read Characteristic for data that changes to service
            // We also want the connected client to be notified when value changes so we add the notify property
             characteristicResult = service.CreateCharacteristic(writeCharUuid2,
                new GattLocalCharacteristicParameters()
                {
                    CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Write,
                    UserDescription = "My Read/Write Characteristic 2",
                    ReadProtectionLevel = GattProtectionLevel.EncryptionRequired,
                    WriteProtectionLevel = GattProtectionLevel.Plain
                });
            ;

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }

            // Get reference to our read Characteristic  
            _readWriteCharacteristic2 = characteristicResult.Characteristic;

            // Set up event handlers for read/write
            _readWriteCharacteristic2.WriteRequested += _writeCharacteristic_WriteRequested;
            _readWriteCharacteristic2.ReadRequested += _readWriteCharacteristic_ReadRequested;
            #endregion

            // Once all the Characteristics have been created you need to advertise the Service so 
            // other devices can see it. Here we also say the device can be connected too and other
            // devices can see it. 
            serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters()
            {
                DeviceName = "Sample2",
                IsConnectable = true,
                IsDiscoverable = true
            });

            Debug.WriteLine($"Sample 2 now Advertising");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void _readWriteCharacteristic_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
        {
            GattReadRequest request = ReadRequestEventArgs.GetRequest();

            DataWriter dw = new();
            dw.WriteInt32(_value);

            request.RespondWithValue(dw.DetachBuffer());
        }

        private static void _writeCharacteristic_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs)
        {
            GattWriteRequest request = WriteRequestEventArgs.GetRequest();

            // Check expected data length, we are expecting 4 bytes (Int32)
            if (request.Value.Length != 4)
            {
                request.RespondWithProtocolError((byte)BluetoothError.NotSupported);
                return;
            }

            // Unpack data from buffer
            DataReader rdr = DataReader.FromBuffer(request.Value);
            _value  = rdr.ReadInt32();

            // Respond if Write requires response
            if (request.Option == GattWriteOption.WriteWithResponse)
            {
                request.Respond();
            }

            Debug.WriteLine($"Received value={_value}");
        }

    }
}
