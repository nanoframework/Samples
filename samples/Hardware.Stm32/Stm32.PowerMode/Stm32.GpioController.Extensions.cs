//
// Copyright (c) 2018 Eclo Solutions
// See LICENSE file in the project root for full license information.
//

using System;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// using the same namespace makes the extensions usable out of the box, without requiring adding other using //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace Windows.Devices.Gpio
{
    public static class GpioControllerExtensions
    {
        /// <summary>
        /// Opens a connection to the specified general-purpose I/O (GPIO) pin of a STM32 MCU in exclusive mode.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="port">The port letter, as it's refereed to in the product documentation.</param>
        /// <param name="pin">The pin number of the specified port.</param>
        /// <returns>The opened GPIO pin.</returns>
        /// <remarks>
        /// This extension is valid for STM32 MCUs and relies on the port naming used by STM, such as PA2 or PB9.
        /// </remarks>
        public static Windows.Devices.Gpio.GpioPin OpenStm32Pin(
            this Windows.Devices.Gpio.GpioController controller, 
            char port, 
            byte pin)
        {
            return controller.OpenStm32Pin(port, pin, Windows.Devices.Gpio.GpioSharingMode.Exclusive);
        }

        /// <summary>
        /// Opens a connection to the specified general-purpose I/O (GPIO) pin of a STM32 target in exclusive mode.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="port">The port letter, as it's refereed to in the product documentation.</param>
        /// <param name="pin">The pin number of the specified port.</param>
        /// <param name="sharingMode">The mode in which you want to open the GPIO pin, which determines whether other connections to the pin can be opened while you have the pin open.</param>
        /// <returns>The opened GPIO pin.</returns>
        /// <remarks>
        /// This extension is valid for STM32 MCUs and relies on the port naming used by STM, such as PA2 or PB9.
        /// </remarks>
        public static Windows.Devices.Gpio.GpioPin OpenStm32Pin(
            this Windows.Devices.Gpio.GpioController controller, 
            char port, 
            byte pin,
            Windows.Devices.Gpio.GpioSharingMode sharingMode)
        {
            if ((port < 'A' || port > 'J') ||
                (pin > 15))
            {
                throw new ArgumentException();
            }

            return controller.OpenPin(((port - 'A') * 16) + pin, sharingMode);
        }
    }
}
