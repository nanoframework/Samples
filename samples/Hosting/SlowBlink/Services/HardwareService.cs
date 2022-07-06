//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.Gpio;

namespace Hosting
{
    internal class HardwareService : IHardwareService, IDisposable
    {
        private readonly GpioController _gpioController;

        public HardwareService()
        {
            _gpioController = new GpioController();
        }

        public GpioController GpioController { get { return _gpioController; } }

        public void Dispose()
        {
            _gpioController.Dispose();
        }
    }
}