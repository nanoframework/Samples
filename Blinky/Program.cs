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
        static GpioPinValue _pin = GpioPinValue.High;

        public static void Main()
        {
            // mind to set a pin that exists on the board being tested
            // PJ5 is LD2 in STM32F769I_DISCO
            //GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('J', 5));
            // PD15 is LED6 in DISCOVERY4
            //GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('D', 15));
            // PG14 is LEDLD4 in F429I_DISCO
            GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('G', 14));
            // PE15 is LED1 in QUAIL
            //GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('E', 15));
            // PB75 is LED2 in STM32F746_NUCLEO
            //GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('B', 7));
            // 4 is a valid GPIO pin in ESP32 DevKit
            //GpioPin led = GpioController.GetDefault().OpenPin(4);
            // PA5 is LED_GREEN in STM32F091RC
            //GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('A', 5));
            // PA5 is LD2 in STM32L072Z_LRWAN1
            //GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('A', 5));

            led.SetDriveMode(GpioPinDriveMode.Output);

            while (true)
            {
                led.Write(GpioPinValue.High);
                Thread.Sleep(125);
                led.Toggle();
                Thread.Sleep(125);
                led.Toggle();
                Thread.Sleep(125);
                led.Toggle();
                Thread.Sleep(525);

                Console.WriteLine("Hello from nanoFramework!");
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
