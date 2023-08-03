//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.Device.Bluetooth.Advertisement;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Class to create an Apple iBeacon.
    /// </summary>
    public class iBeacon
    {
        private Guid _proximityUuid;
        private ushort _major;
        private ushort _minor;
        private sbyte _txPower;

        private BluetoothLEAdvertisementPublisher _publisher;

        /// <summary>
        /// Constructor for iBeacon.
        /// The ProximityUuid, Major and Minor values identify Beacon.
        /// </summary>
        /// <param name="ProximityUuid">The Proximity UUID. 
        /// Each manufacturer uses a different UUID.
        /// Esimotes IBeacons use B9407F30-F5F8-466E-AFF9-25556B57FE6D.
        /// Apple use E2C56DB5-DFFB-48D2-B060-D0F5A71096E0.
        /// </param>
        /// <param name="major">Major value of beacon.</param>
        /// <param name="minor">Minor value of beacon.</param>
        /// <param name="txPower">Transmit power in DB.</param>
        public iBeacon(Guid ProximityUuid, ushort major, ushort minor, sbyte txPower)
        {
            _proximityUuid = ProximityUuid;
            _major = major;
            _minor = minor;
            _txPower = txPower;
            _publisher = null;
        }

        /// <summary>
        /// Gets or Sets the Proximity Uuid.
        /// </summary>
        public Guid ProximityUuid { get => _proximityUuid; set => _proximityUuid = value; }

        /// <summary>
        /// Gets or Sets the Major value.
        /// </summary>
        public ushort Major { get => _major; set => _major = value; }

        /// <summary>
        /// Gets or Sets Minor value.
        /// </summary>
        public ushort Minor { get => _minor; set => _minor = value; }

        /// <summary>
        /// Start advertising the Beacon.
        /// </summary>
        public void Start()
        {
            // Create and initialize a new publisher instance.
            _publisher = new BluetoothLEAdvertisementPublisher();

            _publisher.Advertisement.Flags = BluetoothLEAdvertisementFlags.GeneralDiscoverableMode |
                                             BluetoothLEAdvertisementFlags.DualModeControllerCapable |
                                             BluetoothLEAdvertisementFlags.DualModeHostCapable;

            // Create a manufacturer data section:
            BluetoothLEManufacturerData manufacturerData = new BluetoothLEManufacturerData();

            // Set the company ID for the manufacturer data.
            // 0x004C   Apple, Inc.
            manufacturerData.CompanyId = 0x004c;

            // Create payload
            DataWriter writer = new();

            // last 2 bytes of Apple's iBeacon
            writer.WriteBytes(new byte[] { 0x02, 0x15 });

            // Write Proximity UUID
            writer.WriteUuid2(ProximityUuid);

            // Write Major/Minor
            writer.WriteByte((byte)(_major / 256));
            writer.WriteByte((byte)(_major & 0xff));
            writer.WriteByte((byte)(_minor / 256));
            writer.WriteByte((byte)(_minor & 0xff));

            writer.WriteByte((byte)_txPower);

            manufacturerData.Data = writer.DetachBuffer();

            // Add the manufacturer data to the advertisement publisher:
            _publisher.Advertisement.ManufacturerData.Add(manufacturerData);

            _publisher.Start();
        }

        /// <summary>
        /// Stop advertising.
        /// </summary>
        public void Stop()
        {
            _publisher?.Stop();
            _publisher = null;
        }
    }
}
