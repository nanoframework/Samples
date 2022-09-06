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
/// Bluetooth Sample 1 is a custom service which shows the use of:
/// 
/// - Read static value 
/// - Read a dynamic value using an event 
/// - Notifying clients of changed value
/// 
/// You will be able to connect to the service and read values or subscribe to Notified every 10 seconds.
/// Suitable Phone apps: "LightBlue" or "nRF Connect"
/// </summary>
namespace BluetoothLESample1
{
    public class Program
    {
        static GattLocalCharacteristic _readCharacteristic;
        static GattLocalCharacteristic _readWriteCharacteristic;

        // Read/Write Characteristic value
        static byte _redValue = 128;
        static byte _greenValue = 128;
        static byte _blueValue = 128;

        public static void Main()
        {
            Debug.WriteLine("Hello from Bluetooth Sample 1");

            // Define some custom Uuids
            Guid serviceUuid = new Guid("A7EEDF2C-DA87-4CB5-A9C5-5151C78B0057");
            Guid readCharUuid = new Guid("A7EEDF2C-DA88-4CB5-A9C5-5151C78B0057");
            Guid readStaticCharUuid = new Guid("A7EEDF2C-DA89-4CB5-A9C5-5151C78B0057");
            Guid readWriteCharUuid = new Guid("A7EEDF2C-DA8A-4CB5-A9C5-5151C78B0057");

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

            #region Static read characteristic
            // Now we add an characteristic to service
            // If the read value is not going to change then you can just use a Static value
            DataWriter sw = new DataWriter();
            sw.WriteString("This is Bluetooth sample 1");

            GattLocalCharacteristicResult characteristicResult = service.CreateCharacteristic(readStaticCharUuid,
                 new GattLocalCharacteristicParameters()
                 {
                     CharacteristicProperties = GattCharacteristicProperties.Read,
                     UserDescription = "My Static Characteristic",
                     StaticValue = sw.DetachBuffer()
                 });
            ;

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }
            #endregion

            #region Create Characteristic for dynamic Reads 

            // Add Read Characteristic for data that changes to service
            // We also want the connected client to be notified when value changes so we add the notify property
            characteristicResult = service.CreateCharacteristic(readCharUuid,
                new GattLocalCharacteristicParameters()
                {
                    CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Notify,
                    UserDescription = "My Read Characteristic"
                });

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }

            // Get reference to our read Characteristic  
            _readCharacteristic = characteristicResult.Characteristic;

            // Every time the value is requested from a client this event is called 
            _readCharacteristic.ReadRequested += ReadCharacteristic_ReadRequested;

            // To notify client when time changes we are going to use a timer
            Timer notifyTimer = new Timer(NotifyCallBack, null, 10000, 10000);

            #endregion

            #region Create Characteristic for RGB read/write
            characteristicResult = service.CreateCharacteristic(readWriteCharUuid,
                new GattLocalCharacteristicParameters()
                {
                    CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Write,
                    UserDescription = "My Read/Write Characteristic"
                });

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }

            // Get reference to our read Characteristic  
            _readWriteCharacteristic = characteristicResult.Characteristic;

            // Every time the value is requested from a client this event is called 
            _readWriteCharacteristic.WriteRequested += _readWriteCharacteristic_WriteRequested;
            _readWriteCharacteristic.ReadRequested += _readWriteCharacteristic_ReadRequested;

            #endregion

            #region Start Advertising
            // Once all the Characteristics have been created you need to advertise the Service so 
            // other devices can see it. Here we also say the device can be connected too and other
            // devices can see it. 
            serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters()
            {
                DeviceName = "Sample1",
                IsConnectable = true,
                IsDiscoverable = true
            });
            #endregion 

            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        /// Timer callback for notifying any connected clients who have subscribed to this 
        /// characteristic of changed value.
        /// </summary>
        /// <param name="state">Not used</param>
        private static void NotifyCallBack(object state)
        {
            if (_readCharacteristic.SubscribedClients.Length > 0)
            {
                _readCharacteristic.NotifyValue(GetTimeBuffer());
            }
        }

        /// <summary>
        /// Event handler for Read characteristic.
        /// </summary>
        /// <param name="sender">GattLocalCharacteristic object</param>
        /// <param name="ReadRequestEventArgs"></param>
        private static void ReadCharacteristic_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
        {
            GattReadRequest request = ReadRequestEventArgs.GetRequest();

            // Get Buffer with hour/minute/second
            request.RespondWithValue(GetTimeBuffer());
        }

        // Separate out the Creation of Time buffer so it can be used by read characteristic and Notify
        private static Buffer GetTimeBuffer()
        {
            // Create DataWriter and write the data into buffer
            // Write Hour/minute/second of current time
            DateTime dt = DateTime.UtcNow;

            // Write data in a Buffer object using DataWriter
            DataWriter dw = new DataWriter();
            dw.WriteByte((Byte)dt.Hour);
            dw.WriteByte((Byte)dt.Minute);
            dw.WriteByte((Byte)dt.Second);

            // Detach Buffer object from DataWriter
            return dw.DetachBuffer();
        }

        /// <summary>
        /// Read event handler for Read/Write characteristic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ReadRequestEventArgs"></param>
        private static void _readWriteCharacteristic_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
        {
            GattReadRequest request = ReadRequestEventArgs.GetRequest();

            DataWriter dw = new DataWriter();
            dw.WriteByte((Byte)_redValue);
            dw.WriteByte((Byte)_greenValue);
            dw.WriteByte((Byte)_blueValue);

            request.RespondWithValue(dw.DetachBuffer());

            Debug.WriteLine($"RGB read");
        }

        /// <summary>
        /// Write handler for  Read/Write characteristic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="WriteRequestEventArgs"></param>
        private static void _readWriteCharacteristic_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs)
        {
            GattWriteRequest request = WriteRequestEventArgs.GetRequest();

            // Check expected data length, we are expecting 3 bytes
            if (request.Value.Length != 3)
            {
                request.RespondWithProtocolError((byte)BluetoothError.NotSupported);
                return;
            }

            // Unpack data from buffer
            DataReader rdr = DataReader.FromBuffer(request.Value);
            _redValue = rdr.ReadByte();
            _greenValue = rdr.ReadByte();
            _blueValue = rdr.ReadByte();

            // Respond if Write requires response
            if (request.Option == GattWriteOption.WriteWithResponse)
            {
                request.Respond();
            }

            // Print new values, better to set a RGB led
            Debug.WriteLine($"Received RGB={_redValue}/{_greenValue}/{_blueValue}");
        }
    }
}
