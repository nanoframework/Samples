// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using nanoFramework.Runtime.Native;

namespace nanoFramework.Device.Bluetooth.Services
{
    /// <summary>
    /// Current Time Service.
    /// <para>
    /// Exposes the current time of the device to a connected client.
    /// A client can subscribe to notification and an updated time will be notified every 1 minute.
    /// </para>
    /// <para>
    /// If writes are allowed the time on the device can be updated.
    /// </para>
    /// </para>
    /// This implementation doesn't support the optional "Local Time Information" or "Reference Time Information" characteristics.
    /// </para>
    /// </summary>
    class CurrentTimeService
    {
        private readonly GattLocalService _currentTimeService;
        private readonly GattLocalCharacteristic _currentTimeCharacteristic;
        private Timer _notifyTimer;
        private UpdateReason lastReason = UpdateReason.UPDATE_REASON_UNKNOWN;

        public enum UpdateReason : byte
        {
            UPDATE_REASON_UNKNOWN = 0,
            UPDATE_REASON_MANUAL = 1,
            UPDATE_REASON_EXTERNAL_REF = 2,
            UPDATE_REASON_TIME_ZONE_CHANGE = 4,
            UPDATE_REASON_DAYLIGHT_SAVING = 8
        };

        public CurrentTimeService(GattServiceProvider provider, bool AllowWrite)
        {
            // Add new Current Time service to provider
            _currentTimeService = provider.AddService(GattServiceUuids.CurrentTime);

            GattCharacteristicProperties cprop = GattCharacteristicProperties.Read | GattCharacteristicProperties.Notify;
            if (AllowWrite)
            {
                cprop |= GattCharacteristicProperties.Write;
            }

            GattLocalCharacteristicResult result =
                _currentTimeService.CreateCharacteristic(GattCharacteristicUuids.CurrentTime, new GattLocalCharacteristicParameters()
                {
                    UserDescription = "Current Time",
                    CharacteristicProperties = cprop
                });

            _currentTimeCharacteristic = result.Characteristic;
            _currentTimeCharacteristic.ReadRequested += _currentTimeCharacteristic_ReadRequested;
            _currentTimeCharacteristic.SubscribedClientsChanged += _currentTimeCharacteristic_SubscribedClientsChanged; 
            
            if (AllowWrite)
            {
                _currentTimeCharacteristic.WriteRequested += _currentTimeCharacteristic_WriteRequested;
            }
        }

        private void _currentTimeCharacteristic_SubscribedClientsChanged(GattLocalCharacteristic sender, object args)
        {
            if (_currentTimeCharacteristic.SubscribedClients.Length > 0)
            {
                // Start notify timer
                // every 60 seconds 
                _notifyTimer = new Timer(TimerExpired, null, 60000, 60000);
            }
            else
            {
                // Stop timer if running
                _notifyTimer?.Dispose();
                _notifyTimer = null;
            }
        }

        private void TimerExpired(Object stateInfo)
        {
            Notify(lastReason);
        }

        private void _currentTimeCharacteristic_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
        {
            GattReadRequest request = ReadRequestEventArgs.GetRequest();

            request.RespondWithValue(GetDateTimeBuffer(DateTime.UtcNow, UpdateReason.UPDATE_REASON_EXTERNAL_REF));
        }

        /// <summary>
        /// Notify clients of data time change.
        /// </summary>
        public void Notify(UpdateReason reason)
        {
            lastReason = reason;

            _currentTimeCharacteristic.NotifyValue(GetDateTimeBuffer(DateTime.UtcNow, lastReason));
        }

        /// <summary>
        /// Format date time as buffer object.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        private Buffer GetDateTimeBuffer(DateTime dt, UpdateReason reason)
        {
            DataWriter writer = new DataWriter();

            writer.WriteInt16((Int16)dt.Year);
            writer.WriteByte((byte)dt.Month);
            writer.WriteByte((byte)dt.Day);
            writer.WriteByte((byte)dt.Hour);

            writer.WriteByte((byte)dt.Minute);
            writer.WriteByte((byte)dt.Second);
            writer.WriteByte((byte)dt.Hour);

            writer.WriteByte((byte)dt.DayOfWeek);
            writer.WriteByte((byte)(dt.Millisecond / 3.90625F));
            writer.WriteByte((byte)reason);

            return writer.DetachBuffer();
        }

        /// <summary>
        /// Handle a Write request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="WriteRequestEventArgs"></param>
        private void _currentTimeCharacteristic_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs)
        {
            GattWriteRequest request = WriteRequestEventArgs.GetRequest();

            if (request.Value.Length < 7)
            {
                request.RespondWithProtocolError(GattProtocolError.InvalidAttributeValueLength);
            }
            else
            {
                DataReader reader = DataReader.FromBuffer(request.Value);

                Int16 year = reader.ReadInt16();
                Int16 month = reader.ReadByte();
                Int16 day = reader.ReadByte();
                Int16 hour = reader.ReadByte();
                Int16 minute = reader.ReadByte();
                Int16 second = reader.ReadByte();
                Int16 millisecs = (Int16)((float)reader.ReadByte() * 3.90625F);

               
                try
                {
                    // Create DateTime & validate parameters 
                    DateTime newDT = new DateTime(year, month, day, hour, minute, second, millisecs);
                    Rtc.SetSystemTime(newDT);

                    // Respond if Write requires response
                    if (request.Option == GattWriteOption.WriteWithResponse)
                    {
                        request.Respond();
                    }

                    // Notify date / time changed
                    Notify(lastReason);
                }
                catch (Exception)
                {
                    request.RespondWithProtocolError(GattProtocolError.InvalidPdu);
                }
            }
        }
    }
}
