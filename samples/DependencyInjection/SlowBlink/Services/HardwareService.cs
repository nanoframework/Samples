//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using System.Device.Gpio;

using nanoFramework.Logging.Debug;

using Microsoft.Extensions.Logging;

namespace nanoFramework.SlowBlink
{
    internal class HardwareService : IHardwareService, IDisposable
    {
        private Thread _thread;
        private readonly ILogger _logger;
        private readonly GpioController _gpioController;

        public HardwareService()
        {
            _gpioController = new GpioController();

            var loggerFactory = new DebugLoggerFactory();
            _logger = loggerFactory.CreateLogger(nameof(HardwareService));
        }

        public HardwareService(ILoggerFactory loggerFactory)
        {
            _gpioController = new GpioController();
            _logger = loggerFactory.CreateLogger(nameof(HardwareService));
        }

        public void StartBlinking(int ledPin)
        {
            GpioPin led = _gpioController.OpenPin(ledPin, PinMode.Output);
            led.Write(PinValue.Low);

            _thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(2000);

                    led.Write(PinValue.High);
                    _logger.LogInformation("Led status: on");

                    Thread.Sleep(2000);

                    led.Write(PinValue.Low);
                    _logger.LogInformation("Led status: off");
                }
            });

            _thread.Start();
        }

        public void Dispose()
        {
            _gpioController.Dispose();
        }
    }
}
