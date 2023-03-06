//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using nanoFramework.Hardware.Esp32.Rmt;

namespace InfraredRemote
{
    /// <summary>
    /// This class is a listener that uses Esp32.Rmt to receive infrared signals
    /// and publish them as an event.
    /// </summary>
    public class InfraredListener
    {
        private ReceiverChannel _rxChannel;
        private Thread _t;
        private int _receiveTimeoutMs=60;

        /// <summary>
        /// Create an instance of InfraredListener device class.
        /// </summary>
        /// <param name="pinNumber">GPIO pin number for IR receiver.</param>
        /// </summary>
        public InfraredListener(int pinNumber)
        {
            var settings = new ReceiverChannelSettings(pinNumber)
            {
                // filter out 100Us / noise
                EnableFilter = true,
                FilterThreshold = 100,
                // 1us clock ( 80Mhz / 80 ) = 1Mhz
                ClockDivider = 80,
                // 40ms based on 1us clock
                IdleThreshold = 40000,
                // 60 millisecond timeout
                ReceiveTimeout = TimeSpan.FromMilliseconds(_receiveTimeoutMs),

            };
            _rxChannel = new ReceiverChannel(settings);       
        }

        /// <summary>
        /// Event handler for new signal.
        /// </summary>
        /// <param name="sender">Sender of event.</param>
        /// <param name="signal">Signal representation.</param>
        public delegate void SignalEventHandler(object sender, RmtCommand[] signal);

        /// <summary>
        /// Event raised when new signal arrives.
        /// </summary>
        public event SignalEventHandler? SignalEvent;

        /// <summary>
        /// Starts listener.
        /// </summary>
        public void Start()
        {
            _t = new Thread(Run);
            _t.Start();
        }

        /// <summary>
        /// Stops listener.
        /// </summary>
        public void Stop()
        {
            _t.Abort();
            _rxChannel.Stop();
        }

        private void Run()
        {
            _rxChannel.Start(true);
            while (true)
            {
                var response = _rxChannel.GetAllItems();
                if (response != null)
                {
                    SignalEvent?.Invoke(this, response);
                }
            }
        }
    }
}
