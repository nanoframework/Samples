//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Drivers;
using System;
using System.Threading;
using Windows.Devices.Gpio;

namespace Spi.DemoApp
{
    public class Program
    {
        static L3GD20 _gyro;
        public static GpioPin _touchInterrupt;

        public static void Main()
        {
            // L3GD20 MEMS gyroscope in STM32F429 DISCOVERY board is connected to SPI5...
            // ... and its chip select it's connected to PC1
            _gyro = new L3GD20("SPI5", GpioController.GetDefault().OpenPin(PinNumber('C', 1)));

            // initialize L3GD20
            _gyro.Initialize();

            // output the ID and revision of the device
            Console.WriteLine("ChipID " + _gyro.ChipID.ToString("X2"));

            // infinite loop to keep main thread active
            for (; ; )
            {
                Thread.Sleep(1000);

                var readings = _gyro.GetXYZ();

                Console.WriteLine("X: " + readings[0]);
                Console.WriteLine("Y: " + readings[1]);
                Console.WriteLine("Z: " + readings[2]);
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
