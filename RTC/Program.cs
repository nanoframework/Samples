using nanoFramework.Runtime.Native;
using System;
using System.Threading;

namespace RTCSample
{
    public class Program
    {
        public static void Main()
        {
            // nasty hack to be able to set a breakpoint
            Thread.Sleep(1500);

            Console.WriteLine("system time is: " + DateTime.UtcNow);

            // set RTC
            RTC.SetSystemTime(new DateTime(2018, 2, 28, 10, 20, 30));

            Console.WriteLine("system time is: " + DateTime.UtcNow);

            while (true)
            {
                Thread.Sleep(5000);
            }
        }
    }
}
