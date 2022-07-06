//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Threading;
using System.Device.Gpio;

using nanoFramework.Hosting;

namespace Hosting
{
    internal class LedService : BackgroundService
    {
        private readonly IHardwareService _hardware;

        public LedService(IHardwareService hardware)
        {
            _hardware = hardware;
        }

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {
            var ledPin = 16;

            GpioPin led = _hardware.GpioController.OpenPin(ledPin, PinMode.Output);

            while (!cancellationToken.IsCancellationRequested)
            {
                led.Toggle();
                Thread.Sleep(1000);
            }
        }
    }
}
