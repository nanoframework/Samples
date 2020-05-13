//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Runtime.Native;
using System.Threading;

namespace _ManualResetEvent
{
    public class Program
    {
        // ManualResetEvent is used to block and release threads manually. It is
        // created in the unsignaled state.
        private static ManualResetEvent mre = new ManualResetEvent(false);


        public static void Main()
        {
            Debug.WriteLine("Start 3 named threads that block on a ManualResetEvent:");
            Debug.WriteLine("");

            for (int i = 0; i <= 2; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Start();
            }

            Thread.Sleep(1000);

            Debug.WriteLine("");
            Debug.WriteLine("All three threads should have started, calling Set()" +
                              "to release all the threads.");
            mre.Set();

            Thread.Sleep(2000);

            Debug.WriteLine("");
            Debug.WriteLine("When a ManualResetEvent is signaled, threads that call WaitOne() do not block.");

            for (int i = 3; i <= 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Start();
            }

            Thread.Sleep(2000);

            Debug.WriteLine("");
            Debug.WriteLine("Calling Reset(), so that threads once again block when they call WaitOne().");

            mre.Reset();

            // Start a thread that waits on the ManualResetEvent.
            Thread t5 = new Thread(ThreadProc);
            t5.Start();

            Thread.Sleep(2000);

            Debug.WriteLine("");
            Debug.WriteLine("Call Set() and conclude the demo.");

            mre.Set();



            Thread.Sleep(Timeout.Infinite);
        }

        private static void ThreadProc()
        {
            Debug.WriteLine(
                $"{Thread.CurrentThread.ManagedThreadId} starts and calls mre.WaitOne()");

            mre.WaitOne();

            Debug.WriteLine($"{Thread.CurrentThread.ManagedThreadId} ends.");
        }

    }
}
