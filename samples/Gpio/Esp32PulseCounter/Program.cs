// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.

using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Diagnostics;
using System.Threading;

namespace TestEsp32Counter
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello pulse counter!");

            Configuration.SetPinFunction(27, DeviceFunction.PWM1);
            PwmChannel pwm = PwmChannel.CreateFromPin(27, 1, 0.5);
            pwm.Start();

            GpioPulseCounter counter = new GpioPulseCounter(26);
            Debug.Write("Starting PWM and Counter. With 1 Hz and counting on rising");
            counter.Polarity = GpioPulsePolarity.Rising;
            counter.FilterPulses = 0;

            counter.Start();
            int inc = 0;
            GpioPulseCount counterCount;
            while (inc++ < 20)
            {
                counterCount = counter.Read();
                Console.WriteLine($"{counterCount.RelativeTime}: {counterCount.Count}");
                Thread.Sleep(1000);
            }
            
            counterCount = counter.Reset();
            Console.WriteLine($"{counterCount.RelativeTime}: {counterCount.Count}");
            Console.WriteLine("Reset read, that should then restart at 0");
            inc = 0;
            while (inc++ < 20)
            {
                counterCount = counter.Read();
                Console.WriteLine($"{counterCount.RelativeTime}: {counterCount.Count}");
                Thread.Sleep(1000);
            }

            counter.Stop();
            counter.Dispose();
            pwm.Stop();

            Console.WriteLine("Using 2 pins for a rotary encoder, changing the direction will show increase and decreas in the count.");
            GpioPulseCounter encoder = new GpioPulseCounter(12, 14);
            encoder.Polarity = GpioPulsePolarity.Rising;
            encoder.Start();
            int incEncod = 0;
            GpioPulseCount counterCountEncode;
            while (incEncod++ < 20)
            {
                counterCountEncode = encoder.Read();
                Console.WriteLine($"{counterCountEncode.RelativeTime}: {counterCountEncode.Count}");
                Thread.Sleep(1000);
            }

            encoder.Stop();
            encoder.Dispose();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
