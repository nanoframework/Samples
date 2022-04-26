//
// Copyright (c) 2018 Eclo Solutions
// See LICENSE file in the project root for full license information.
//

using System;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// using the same namespace makes the extensions usable out of the box, without requiring adding other using //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System.Device.Gpio
{
    public static class GpioControllerExtensions
    {
        /// <summary>
        /// Opens a connection to the specified general-purpose I/O (GPIO) pin of a STM32 MCU.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="port">The port letter, as it's refereed to in the product documentation.</param>
        /// <param name="pin">The pin number of the specified port.</param>
        /// <returns>The opened GPIO pin.</returns>
        /// <remarks>
        /// This extension is valid for STM32 MCUs and relies on the port naming used by STM, such as PA2 or PB9.
        /// </remarks>
        public static System.Device.Gpio.GpioPin OpenStm32Pin(
            this System.Device.Gpio.GpioController controller, 
            char port, 
            byte pin)
        {
            if ((port < 'A' || port > 'J') ||
                (pin > 15))
            {
                throw new ArgumentException("Invalid Pin");
            }

            return controller.OpenPin(((port - 'A') * 16) + pin);
        }
    }
}
