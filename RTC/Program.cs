//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//


using nanoFramework.Runtime.Native;
using System;
using System.Threading;

namespace RTCSample
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("system time is: " + DateTime.UtcNow);

            // set RTC
            Rtc.SetSystemTime(new DateTime(2018, 2, 28, 10, 20, 30));

            Console.WriteLine("system time is: " + DateTime.UtcNow);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
