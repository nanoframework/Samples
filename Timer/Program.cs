//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using Windows.Devices.Gpio;

namespace TimerSample
{
    public class Program
    {
        static GpioPin _led;

        public static void Main()
        {
            // mind to set a pin that exists on the board being tested
            // PJ5 is LD2 in STM32F769I_DISCO
            _led = GpioController.GetDefault().OpenPin(PinNumber('J', 5));

            _led.SetDriveMode(GpioPinDriveMode.Output);


            // create timer
            Console.WriteLine(DateTime.UtcNow.ToString() + ": creating timer, due in 1 second");

            Timer testTimer = new Timer(CheckStatusTimerCallback, null, 1000, 1000);

            // let it run for 5 seconds (will blink 5 times)
            Thread.Sleep(5000);

            Console.WriteLine(DateTime.UtcNow.ToString() + ": changing period to 2 seconds");

            // change timer period
            testTimer.Change(0, 2000);

            // let it run for 10 seconds (will blink 5 times)
            Thread.Sleep(10000);

            Console.WriteLine(DateTime.UtcNow.ToString() + ": destroying timer");

            // dispose timer
            testTimer.Dispose();

            // loop forever
            while (true) { Thread.Sleep(1000); }
        }

        private static void CheckStatusTimerCallback(object state)
        {
            Console.WriteLine(DateTime.UtcNow.ToString() + ": blink");

            _led.Write(GpioPinValue.High);
            Thread.Sleep(125);
            _led.Write(GpioPinValue.Low);
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
