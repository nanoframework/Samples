using nanoFramework.Runtime.Native;
using System;
using System.Threading;

namespace ExecutionConstraintDemo
{
    public class Program
    {
        public static void Main()
        {
            new Thread(Thread_Execute_TwoSeconds).Start();
            new Thread(Thread_Execute_TenSeconds).Start();

            Thread.Sleep(Timeout.Infinite);
        }

        public static void Thread_Execute_TwoSeconds()
        {
            DateTime start = DateTime.UtcNow;

            Console.WriteLine($"Thread 1 starting @ {start}.");

            // install the execution constraint by specifying a timeout in milliseconds
            ExecutionConstraint.Install((int)new TimeSpan(0, 0, 5).TotalMilliseconds, 0);

            for(int i = 0; i < 20 ; i++)
            {
                Thread.Sleep(100);
            }

            // remove the execution constraint by calling the method with -1
            ExecutionConstraint.Install(-1, 0);

            var end = DateTime.UtcNow - start;

            Console.WriteLine($"Thread 1 end after {end.TotalMilliseconds.ToString("N0")} milliseconds.");
        }

        public static void Thread_Execute_TenSeconds()
        {
            DateTime start = DateTime.UtcNow;

            Console.WriteLine($"Thread 2 starting @ {start}.");

            try
            {
                ///////////////////////////////////////////
                // this WILL THROW a ConstraintException //
                ///////////////////////////////////////////

                // install the execution constraint by specifying a timeout in milliseconds
                ExecutionConstraint.Install((int)new TimeSpan(0, 0, 5).TotalMilliseconds, 0);

                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(100);
                }

                ExecutionConstraint.Install(-1, 0);
            }
            catch(Exception ex)
            {
                Console.WriteLine(">>> As the prophecy foretold: a ConstraintException was thrown!");
            }

            var end = DateTime.UtcNow - start;

            Console.WriteLine($"Thread 2 end after {end.TotalMilliseconds.ToString("N0")} milliseconds.");
        }
    }
}
