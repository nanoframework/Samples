//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;
using System.Device.Gpio;

namespace TimerSample
{
    public class Program
    {
        static GpioPin _led;

        public static void Main()
        {
            // mind to set a pin that exists on the board being tested
            // PJ5 is LD2 in STM32F769I_DISCO
            _led = GpioController.OpenPin(PinNumber('J', 5), PinMode.Output);
            // PG14 is LEDLD4 in F429I_DISCO
            //_led = GpioController.OpenPin(PinNumber('G', 14), PinMode.Output);

            // create timer
            Debug.WriteLine(DateTime.UtcNow.ToString() + ": creating timer, due in 1 second");

            Timer testTimer = new Timer(CheckStatusTimerCallback, null, 1000, 1000);

            // let it run for 5 seconds (will blink 5 times)
            Thread.Sleep(5000);

            Debug.WriteLine(DateTime.UtcNow.ToString() + ": changing period to 2 seconds");

            // change timer period
            testTimer.Change(0, 2000);

            // let it run for 10 seconds (will blink 5 times)
            Thread.Sleep(10000);

            Debug.WriteLine(DateTime.UtcNow.ToString() + ": destroying timer");

            // dispose timer
            testTimer.Dispose();

            // loop forever
            Thread.Sleep(Timeout.Infinite);
        }

        private static void CheckStatusTimerCallback(object state)
        {
            Debug.WriteLine(DateTime.UtcNow.ToString() + ": blink");

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
