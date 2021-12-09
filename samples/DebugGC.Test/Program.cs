//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Runtime.Native;
using GC = nanoFramework.Runtime.Native.GC;

namespace DebugGC.Test
{
    public class Program
    {
        public static void Main()
        {
            GC.EnableGCMessages(true);

            Debug.WriteLine($"{ SystemInfo.TargetName } running on { SystemInfo.Platform }.");

            Debug.WriteLine($"Initial managed heap size: {GC.Run(false)} bytes");

            GpioController gpioController = new();

            // mind to set a pin that exists on the board being tested
            // PJ5 is LD2 in STM32F769I_DISCO
            //GpioPin led = gpioController.OpenPin(PinNumber('J', 5), PinMode.Output);
            // PD15 is LED6 in DISCOVERY4
            //GpioPin led = gpioController.OpenPin(PinNumber('D', 15), PinMode.Output);
            // PE15 is LED1 in QUAIL
            //GpioPin led = gpioController.OpenPin(PinNumber('E', 15), PinMode.Output);
            // PG13 is LD3 in F429I-DISCO
            //GpioPin led = gpioController.OpenPin(PinNumber('G', 14), PinMode.Output);
            // ESP32
            GpioPin led = gpioController.OpenPin(4, PinMode.Output);

            int i = 0;

            for (; ; )
            {
                i++;

                int[] array = new int[4096];

                led.Toggle();
                Thread.Sleep(100);
                led.Toggle();
                Thread.Sleep(400);

                Debug.WriteLine(">> " + i.ToString() + " free memory: " + GC.Run(true) + " bytes");

                Thread.Sleep(1000);
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
