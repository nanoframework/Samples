//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;

using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

/// <summary>
/// Bluetooth Sample 2 is a custom service which shows the use of:
/// 
/// - Adding security to characteristics
/// - 1st characteristic you can read & write without pairing
/// - 2nd characteristic you can read & write but will required device to paired so encryption is enabled. If you try to access this characteristic
///       a pairing will be forced.  You can use a just works type of pairing for this.
/// - 3nd characteristic you can read & write but will required device to paired and authenticated. If you try to access this characteristic
///       a pairing will be forced.  A pin number of 654321 will need to be entered to pair successfully.
///       
/// - All characteristics read/write same value.
/// 
/// You will be able to connect to the service and read values or subscribe to Notified ever 10 seconds.
/// Suitable Phone apps: "LightBlue" or "nRF Connect".  If using a smaller then six digits for pin then use leading zeros in these apps.
/// </summary>
namespace BluetoothLESample2
{
    public class Program
    {
        static GattLocalCharacteristic _readWriteCharacteristic1;
        static GattLocalCharacteristic _readWriteCharacteristic2;
        static GattLocalCharacteristic _readWriteCharacteristic3;

        // value used to read/write
        static Int32 _value = 57;

        // Default pin
        const int PASSKEY = 654321;
        const ushort APPEARANCE_SPORTS_WATCH = 0x00C1;

        public static void Main()
        {
            Console.WriteLine();
            Console.WriteLine("Hello from Bluetooth Sample 2");

            Guid serviceUuid = new Guid("A7EEDF2C-DA8C-4CB5-A9C5-5151C78B0057");
            Guid plain_CharUuid1 = new Guid("A7EEDF2C-DA8D-4CB5-A9C5-5151C78B0057");
            Guid encrypt_CharUuid2 = new Guid("A7EEDF2C-DA8E-4CB5-A9C5-5151C78B0057");
            Guid auth_CharUuid3 = new Guid("A7EEDF2C-DA8F-4CB5-A9C5-5151C78B0057");

            // BluetoothLEServer is a singleton object so gets its instance. The Object is created when you first access it
            // and can be disposed to free up memory.
            BluetoothLEServer server = BluetoothLEServer.Instance;
            
            // Give device a name and appearance.
            server.DeviceName = "Sample2";

            // Set appearance (optional)
            server.Appearance = APPEARANCE_SPORTS_WATCH;

            // Set up an event handler for handling pairing requests
            server.Pairing.PairingRequested += Pairing_PairingRequested;
            server.Pairing.PairingComplete += Pairing_PairingComplete;
            
            // Set up event for a session status change, client connects/disconnects
            server.Session.SessionStatusChanged += Session_SessionStatusChanged;

            // The IOCapabilities define the input /output capabilities of the device and the type of pairings that is available.
            // See Bluetooth pairing matrix for more information. 
            // With NoInputNoOutput on both ends then only "Just works" pairing is available and only first 2 characteristics will be accesable.
            
            //server.Pairing.IOCapabilities = DevicePairingIOCapabilities.NoInputNoOutput;

            // By making it a display we force an Authentication.
            server.Pairing.IOCapabilities = DevicePairingIOCapabilities.DisplayOnly;

            // Start the Bluetooth server. 
            server.Start();

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
            GattLocalCharacteristicResult characteristicResult = service.CreateCharacteristic(plain_CharUuid1,
                new GattLocalCharacteristicParameters()
                {
                    CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Write,
                    UserDescription = "My Read/Write Characteristic 1",
                    ReadProtectionLevel = GattProtectionLevel.Plain,
                    WriteProtectionLevel = GattProtectionLevel.Plain
                });
            ;

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }

            // Get reference to our read Characteristic  
            _readWriteCharacteristic1 = characteristicResult.Characteristic;

            // Set up event handlers for read/write #1
            _readWriteCharacteristic1.WriteRequested += _writeCharacteristic_WriteRequested;
            _readWriteCharacteristic1.ReadRequested += _readWriteCharacteristic_ReadRequested;
            #endregion

            #region Characteristic 2 - Encryption required
            // Add Read Characteristic for data that changes to service
            // We also want the connected client to be notified when value changes so we add the notify property
             characteristicResult = service.CreateCharacteristic(encrypt_CharUuid2,
                new GattLocalCharacteristicParameters()
                {
                    CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Write,
                    UserDescription = "My Read/Write Characteristic 2",
                    ReadProtectionLevel = GattProtectionLevel.EncryptionRequired,
                    WriteProtectionLevel = GattProtectionLevel.EncryptionRequired
                });

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }

            // Get reference to our read Characteristic  
            _readWriteCharacteristic2 = characteristicResult.Characteristic;

            // Set up event handlers for read/write #2
            _readWriteCharacteristic2.WriteRequested += _writeCharacteristic_WriteRequested;
            _readWriteCharacteristic2.ReadRequested += _readWriteCharacteristic_ReadRequested;
            #endregion

            #region Characteristic 3 - Authentication & Encryption required
            // Add Read Characteristic for data that changes to service
            // We also want the connected client to be notified when value changes so we add the notify property
            characteristicResult = service.CreateCharacteristic(auth_CharUuid3,
               new GattLocalCharacteristicParameters()
               {
                   CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Write,
                   UserDescription = "My Read/Write Characteristic 3",
                   ReadProtectionLevel = GattProtectionLevel.EncryptionAndAuthenticationRequired,
                   WriteProtectionLevel = GattProtectionLevel.EncryptionAndAuthenticationRequired
               });

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }

            // Get reference to our read Characteristic  
            _readWriteCharacteristic3 = characteristicResult.Characteristic;

            // Set up event handlers for read/write #3
            _readWriteCharacteristic3.WriteRequested += _writeCharacteristic_WriteRequested;
            _readWriteCharacteristic3.ReadRequested += _readWriteCharacteristic_ReadRequested;
            #endregion

            // Once all the Characteristics have been created you need to advertise the Service so 
            // other devices can see it. Here we also say the device can be connected too and other
            // devices can see it. 
            serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters()
            {
                IsConnectable = true,
                IsDiscoverable = true
            });

            Console.WriteLine($"Sample 2 Advertising");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Session_SessionStatusChanged(object sender, GattSessionStatusChangedEventArgs args)
        {
            Console.WriteLine($"Session_SessionStatusChanged status->{args.Status} Error->{args.Error}");
            if (args.Status == GattSessionStatus.Active)
            {
                Console.WriteLine($"Client connected, address {BluetoothLEServer.Instance.Session.DeviceId:X}");
            }
            else
            {
                Console.WriteLine("Client disconnected");
            }
        }

        private static void Pairing_PairingComplete(object sender, DevicePairingEventArgs args)
        {
            DevicePairing dp = sender as DevicePairing;
         
            Console.WriteLine($"PairingComplete:{args.Status} IOCaps:{dp.IOCapabilities} IsPaired:{dp.IsPaired} IsAuthenticated:{dp.IsAuthenticated}");
        }

        private static void Pairing_PairingRequested(object sender, DevicePairingRequestedEventArgs args)
        {
            Console.WriteLine($"CustomPairing_PairingRequested {args.PairingKind}");

            switch (args.PairingKind)
            {
                // Passkey displayed on current device or just a know secret passkey
                // Tell BLE what passkey is, so it can be checked against what has been entered on other device
                case DevicePairingKinds.DisplayPin:
                    Console.WriteLine("DisplayPin");

                    // We don't actually display pin here but just tell Bluetooth what the pin is so it can 
                    // compare with pin supplied by client
                    args.Accept(PASSKEY);
                    break;
            }
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

            Console.WriteLine($"Received value={_value}");
        }
    }
}
