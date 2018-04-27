//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Windows.Devices.Gpio;
using System;
using System.Threading;

namespace Blinky
{
	public class Program
    {
        public static void Main()
        {
            // mind to set a pin that exists on the board being tested
            // PJ5 is LD2 in STM32F769I_DISCO
            GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('J', 5));
            // PD15 is LED6 in DISCOVERY4
            //GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('D', 15));
            // PE15 is LED1 in QUAIL
            //GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('E', 15));
            // PB75 is LED2 in STM32F746_NUCLEO
            //GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('B', 7));

            led.SetDriveMode(GpioPinDriveMode.Output);

            for (; ; )
            {
                led.Write(GpioPinValue.High);
                Thread.Sleep(125);
                led.Write(GpioPinValue.Low);
                Thread.Sleep(125);
                led.Write(GpioPinValue.High);
                Thread.Sleep(125);
                led.Write(GpioPinValue.Low);
                Thread.Sleep(125 + 750);
            }
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
