//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Device.Gpio;

namespace Hosting
{
    public interface IHardwareService
    {
        GpioController GpioController { get; }
    }
}
