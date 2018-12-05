//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using STM32 = nanoFramework.Hardware.Stm32;

namespace Stm32.TestAlarms
{
    public class Program
    {
        public static void Main()
        {
            DateTime alarmTime = new DateTime(2018, 10, 29, 23, 30, 15);

            Console.WriteLine($"Set alarm time to {alarmTime.ToString("u")}");

            STM32.RTC.SetAlarm(alarmTime);

            // wait a couple of seconds...
            Thread.Sleep(2000);

            // read back alarm time
            DateTime alarmTimeReadBack = STM32.RTC.GetAlarm();

            Console.WriteLine($"Alarm was set to {alarmTimeReadBack.ToString("u")}");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
