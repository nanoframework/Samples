using System;
using System.Threading;

namespace RTC.Test
{
    public class Program
    {
        public static void Main()
        {
            // nasty hack to be able to set a breakpoint
            Thread.Sleep(1500);

            Console.WriteLine("system time is: " + DateTime.UtcNow);

            // set RTC
            nanoFramework.DateTime.RTC.SetSystemTime(new DateTime(2018, 2, 28, 10, 20, 30));

            Console.WriteLine("system time is: " + DateTime.UtcNow);

            while (true)
            {
                Thread.Sleep(5000);
            }
        }
    }
}
