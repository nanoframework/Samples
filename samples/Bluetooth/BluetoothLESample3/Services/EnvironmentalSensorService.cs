// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.Services
{
    public class EnvironmentalSensorService
    {
        private readonly GattLocalService _service;
        private readonly ArrayList _sensors;

        public enum SensorType
        {
            Temperature,
            Humidity,
            Pressure,
            Rainfall
        };

        public enum Sampling : byte
        {
            Unspecified = 0x00,
            Instantaneous = 0x01,
            ArithmeticMean = 0x02,
            RMS = 0x03,
            Maximum = 0x04,
            Minimum = 0x05,
            Accumulated = 0x06,
            Count = 0x07
        };

        struct sensorItem
        {
            public SensorType sensorType;
            public GattLocalCharacteristic sensorChar;
            public Buffer dataBuffer;
        };

        public EnvironmentalSensorService(GattServiceProvider provider)
        {
            _service = provider.AddService(GattServiceUuids.EnvironmentalSensing);
            _sensors = new();
        }

        /// <summary>
        /// Add the sensor to the Environmental Sensor Service.
        /// </summary>
        /// <param name="sType">Type of Sensor</param>
        /// <param name="description">Description / location</param>
        /// <param name="sampling">Sampling function</param>
        /// <returns></returns>
        public int AddSensor(SensorType sType, string description, Sampling sampling = Sampling.Unspecified)
        {
            GattLocalCharacteristicResult result =
                    _service.CreateCharacteristic(GetUuidForType(sType), new GattLocalCharacteristicParameters()
                    {
                        UserDescription = description,
                        CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Notify
                    });

            GattLocalCharacteristic sensor = result.Characteristic;
            sensor.ReadRequested += Sensor_ReadRequested;

            // Add descriptors
            AddMeasurementDescriptor(sensor, sampling, 0);

            sensorItem si = new() { sensorType = sType, sensorChar = sensor, dataBuffer = null };

            return _sensors.Add(si);
        }

        /// <summary>
        /// Update the Sensor value. If any device is subscribed to this sensor 
        /// it will be notified.
        /// </summary>
        /// <param name="sensorIndex">Index number to sensor returned from AddSensor()</param>
        /// <param name="value">New value for sensor</param>
        public void UpdateValue(int sensorIndex, float value)
        {
            bool updated = false;

            // Let it throw exception if invalid index
            sensorItem si = (sensorItem)_sensors[sensorIndex];

            DataWriter writer = new();
            switch (si.sensorType)
            {
                case SensorType.Temperature:
                    // Temperature in Celsius
                    // uint16 - hundreds of C, 9543 = 95.43c
                    short temp = (short)(value * 100);
                    writer.WriteInt16(temp);                  // Temperature 
                    updated = true;
                    break;

                case SensorType.Humidity:
                    // Humidity percentage
                    // uint16 - hundreds of %, 9543 = 95.43%
                    ushort humidity = (ushort)(value * 100);
                    writer.WriteUInt16(humidity);
                    updated = true;
                    break;

                case SensorType.Pressure:
                    // Pressure Pascal
                    uint pa = (uint)(value * 10);
                    writer.WriteUInt32(pa);
                    updated = true;
                    break;

                case SensorType.Rainfall:
                    // Rainfall in mm
                    writer.WriteUInt16((ushort)value);
                    updated = true;
                    break;
            }

            if (updated)
            {
                si.dataBuffer = writer.DetachBuffer();
                _sensors[sensorIndex] = si;

                si.sensorChar.NotifyValue(si.dataBuffer);
            }
        }

        private void Sensor_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
        {
            GattReadRequest request = ReadRequestEventArgs.GetRequest();

            foreach (sensorItem si in _sensors)
            {
                if (si.sensorChar == sender && si.dataBuffer != null)
                {
                    request.RespondWithValue(si.dataBuffer);
                    return;
                }
            }

            request.RespondWithProtocolError(GattProtocolError.AttributeNotFound);
        }

        private Guid GetUuidForType(SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.Temperature:
                    return GattCharacteristicUuids.Temperature;

                case SensorType.Humidity:
                    return GattCharacteristicUuids.Humidity;

                case SensorType.Pressure:
                    return GattCharacteristicUuids.Pressure;

                case SensorType.Rainfall:
                    return GattCharacteristicUuids.Rainfall;
            }

            throw new ArgumentOutOfRangeException();
        }

        private void AddMeasurementDescriptor(GattLocalCharacteristic sensor, Sampling sampleFunction, byte application)
        {
            // Set up Descriptors - ESS Measurement
            DataWriter essmWriter = new();
            essmWriter.WriteInt16(0);       // Flags reserved
            essmWriter.WriteByte((byte)sampleFunction);        // Sampling -Unspecified
            essmWriter.WriteBytes(new byte[3] { 0, 0, 0 }); // Int24 - Measurement period - 0 Not in use
            essmWriter.WriteBytes(new byte[3] { 0, 0, 0 }); // Int24 - Update period - 0 Not in use
            essmWriter.WriteByte(application);        // Application - 0 = Unspecified
            essmWriter.WriteByte(0xff);     // Measurement Uncertainty - 0xff = Information not available 

            sensor.CreateDescriptor(GattDescriptorUuids.EssMeasurement, new GattLocalDescriptorParameters
            {
                StaticValue = essmWriter.DetachBuffer()
            });
        }
    }
}
