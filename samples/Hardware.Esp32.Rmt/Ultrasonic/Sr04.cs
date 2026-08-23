//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using nanoFramework.Hardware.Esp32.Rmt;

namespace nanoFramework.device.sensor
{
    /// <summary>
    /// This class is used to interface with SR04 devices and measure distance.
    /// HC-SR04 , SN-SR04 etc.
    /// 
    /// Range is 2cm to 450cm
    /// 
    /// When connecting to ESP32 be aware that the output is 5V and a level shifter should be used
    /// As a minimum connect a resistor between SR04 output(ECHO) and ESP32 pin. (10K) to limit current.
    /// For testing it can be connected directly.
    /// 
    /// If a 3.3V SR04 device is used then no resistor/level shifter is required ( HC-SR04P )
    /// </summary>
    public class Sr04
    {
        ReceiverChannel _rxChannel;
        TransmitterChannel _txChannel;
        RmtSymbols _txPulse;

        const float _speedOfSound = 340.29F;
        const int _resolutionHz = 1_000_000;

        /// <summary>
        /// Create an instance of the SR04 device class
        /// </summary>
        /// <param name="TxPin">GPIO pin number for trigger pin</param>
        /// <param name="RxPin">GPIO pin number of echo pin</param>
        public Sr04(int TxPin, int RxPin)
        {
            // Set-up TX & RX channels
            // We need to send a 10us pulse to initiate measurement
            var txChannelSettings = new TransmitChannelSettings(pinNumber: TxPin)
            {
                // 1us clock ( 1Mhz resolution )
                ResolutionHz = _resolutionHz,
                EnableCarrierWave = false,
                IdleLevel = false,
            };

            _txChannel = new TransmitterChannel(txChannelSettings);

            // we only need 1 pulse of 10 us high
            _txPulse = new RmtSymbols();
            _txPulse.Add(new RmtSymbol(10, true, 0, false));

            // The received echo pulse width represents the distance to obstacle
            // 150us to 38ms
            var rxChannelSettings = new ReceiverChannelSettings(pinNumber: RxPin)
            {
                // 1us clock ( 1Mhz resolution )
                ResolutionHz = _resolutionHz,

                // filter out 50ns / noise
                FilterThreshold = 50,

                // 30ms 
                IdleThreshold = 30_000_000,

                // 60 millisecond timeout is enough
                ReceiveTimeout = TimeSpan.FromMilliseconds(60)
            };

            _rxChannel = new ReceiverChannel(rxChannelSettings);
        }


        /// <summary>
        /// Get the distance of object from SR04 device
        /// </summary>
        /// <returns>Distance in meters or -1 if out of range</returns>
        public float GetDistance()
        {
            RmtSymbols response = null;

            _rxChannel.Start();

            // Send 10us pulse
            _txChannel.Send(_txPulse, true);

            // Try 5 times to get valid response
            for (int count = 0; count < 5; count++)
            {
                response = _rxChannel.TryGetReceivedSymbols();
                if (response != null)
                    break;

                // Retry every 60 ms
                Thread.Sleep(60);
            }

            _rxChannel.Stop();

            if (response == null)
                return -1;

            // Echo pulse width in micro seconds
            int duration = response[0].Duration0;

            // Calculate distance in meters
            // Distance calculated as  (speed of sound) * duration(meters) / 2 
            return _speedOfSound * duration / (1000000 * 2);
        }
    }
}
