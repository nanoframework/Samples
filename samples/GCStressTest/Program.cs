//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;
using System.Device.Gpio;

namespace GCStressTest
{
    public class Program
    {
        private static Timer _timer0;
        private static Timer _timer1;
        private static Timer _timer2;
        private static Timer _timer3;
        private static Timer _timer4;
        private static Timer _timer5;

        private static Random _randomizer;

        static GpioPin _led;

        private static long _timer0Counter;
        private static long _timer1Counter;
        private static long _timer2Counter;
        private static long _dummyThreadCounter;
        private static long _dummyThreadTimerCounter;

        public static void Main()
        {
            _randomizer = new Random();

            // timer start times (all timers are due to start in the first 2 seconds)
            int start0 = _randomizer.Next(1000 * 2);
            int start1 = _randomizer.Next(1000 * 2);
            int start2 = _randomizer.Next(1000 * 2);
            int start3 = _randomizer.Next(1000 * 2);
            int start4 = _randomizer.Next(1000 * 2);
            int start5 = _randomizer.Next(1000 * 2);


            // timer intervals 
            // half of the timers will have a short period
            int period0 = _randomizer.Next(1000);
            int period1 = _randomizer.Next(1000);
            int period2 = _randomizer.Next(1000);

            // the other half of the timers will have a long period
            int period3 = _randomizer.Next(1000 * 3);
            int period4 = _randomizer.Next(1000 * 3);
            int period5 = _randomizer.Next(1000 * 3);

            _led = new GpioController().OpenPin(PinNumber('G', 14), PinMode.Output);

            // quick timers

            Debug.WriteLine("Starting timer 0 with " + period0 + "ms period, due in " + start0 + "ms.");
            _timer0 = new Timer(new TimerCallback(TimerHandler0), null, start0, period0);
            Thread.Sleep(1);

            Debug.WriteLine("Starting timer 1 with " + period1 + "ms period, due in " + start1 + "ms.");
            _timer1 = new Timer(new TimerCallback(TimerHandler1), null, start1, period1);
            Thread.Sleep(1);

            Debug.WriteLine("Starting timer 2 with " + period2 + "ms period, due in " + start2 + "ms.");
            _timer2 = new Timer(new TimerCallback(TimerHandler2), null, start2, period2);
            Thread.Sleep(1);

            // long timers
            Debug.WriteLine("Starting timer 3 with " + period3 + "ms period, due in " + start3 + "ms.");
            _timer3 = new Timer(new TimerCallback(TimerHandler0), null, start3, period3);
            Thread.Sleep(1);

            Debug.WriteLine("Starting timer 4 with " + period4 + "ms period, due in " + start4 + "ms.");
            _timer4 = new Timer(new TimerCallback(TimerHandler1), null, start4, period4);
            Thread.Sleep(1);

            Debug.WriteLine("Starting timer 5 with " + period5 + "ms period, due in " + start5 + "ms.");
            _timer5 = new Timer(new TimerCallback(TimerHandler2), null, start5, period5);
            Thread.Sleep(1);

            for (; ; )
            {
                Thread.Sleep(100);
                _led.Toggle();
            }
        }

        private static void TimerHandler0(object status)
        {
            if(status == null)
            {
                Debug.WriteLine("Hello from timer 0 [" + _timer0Counter++ + "]");

            }
            else
            {
                Debug.WriteLine("Hello from dummy thread timer [" + _dummyThreadTimerCounter++ + "]");
            }

            // create 100 integers interleaved with bytes
            for (int i = 0; i < 100; i++)
            {
                int dummyInt = 0;
                dummyInt += i;

                byte dummyByte = 5;
                dummyByte = (byte)(dummyByte + i);
            }
        }

        private static void TimerHandler1(object status)
        {
            Debug.WriteLine("Hello from timer 1 [" + _timer1Counter++ + "]");

            // create 10 huge arrays
            for (int i = 0; i < 10; i++)
            {
                // create array with random size up to 10.000 elements
                byte[] array = new byte[_randomizer.Next(10000)];

                Debug.WriteLine("Created array with " + array.Length + " elements");

                // do something with each array element
                for (int j = 0; j < array.Length; j++)
                {
                    array[j] += (byte)i;
                }

                Thread.Sleep(10);
            }
        }

        private static void TimerHandler2(object status)
        {
            Debug.WriteLine("Hello from timer 2 [" + _timer2Counter++ + "]");

            // spawn 10 Threads 
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyThread);
                t.Start();
            }
        }

        private static void DummyThread()
        {
            Debug.WriteLine("Hello from dummy thread [" + _dummyThreadCounter++ + "]");

            String dummyString = "";

            for (int i = 0; i < 10; i++)
            {
                String stringBit = "block" + i.ToString();

                dummyString += stringBit;

                Thread.Sleep(_randomizer.Next(500));

                //

                if((i % 2) == 0)
                {
                    // on even pass, spawn a new thread
                    Thread t = new Thread(() => {
                        TimerHandler0(_dummyThreadTimerCounter);
                    });
                    t.Start();

                }
            }

            // make sure the string remains until the end
            dummyString.IndexOf("x");
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
