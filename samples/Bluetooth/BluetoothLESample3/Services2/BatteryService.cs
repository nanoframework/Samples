// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.Services
{
    class BatteryService
    {
        GattLocalService _batteryService;
        GattLocalCharacteristic _batteryLevelCharacteristic;
        byte _batteryLevel;

        public BatteryService(GattServiceProvider provider)
        {
            // Add new Battery service to provider
            _batteryService = provider.AddService(GattServiceUuids.Battery);


            GattLocalCharacteristicResult result =
                _batteryService.CreateCharacteristic(GattCharacteristicUuids.BatteryLevel, new GattLocalCharacteristicParameters()
                {
                    UserDescription = "Battery level %",
                    CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Notify
                });

            _batteryLevelCharacteristic = result.Characteristic;
            _batteryLevelCharacteristic.ReadRequested += BatteryLevelCharacteristic_ReadRequested;

            // Set default values
            BatteryLevel = 100;
            DeviceInError = false;
        }

        /// <summary>
        /// Get or Set current battery level.
        /// </summary>
        public byte BatteryLevel
        {
            get => _batteryLevel;
            set
            {
                if (_batteryLevel != value)
                {
                    _batteryLevelCharacteristic.NotifyValue(GetBatteryLevel());
                }
                _batteryLevel = value;
            }
        }

        /// <summary>
        /// Set if Battery not connected or error reading battery level.
        /// </summary>
        public bool DeviceInError { get; set; }

        /// <summary>
        /// Read event handler.
        /// </summary>
        /// <param name="sender">GattLocalCharacteristic sender</param>
        /// <param name="ReadRequestEventArgs">Request args</param>
        private void BatteryLevelCharacteristic_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
        {
            GattReadRequest request = ReadRequestEventArgs.GetRequest();

            if (DeviceInError)
            {
                request.RespondWithProtocolError((byte)BluetoothError.DeviceNotConnected);
            }
            else
            {
                request.RespondWithValue(GetBatteryLevel());
            }
        }

        private Buffer GetBatteryLevel()
        {
            DataWriter writer = new DataWriter();
            writer.WriteByte(BatteryLevel);
            return writer.DetachBuffer();
        }
    }
}
