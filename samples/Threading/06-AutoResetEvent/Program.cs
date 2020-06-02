//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;

namespace _AutoResetEvent
{
    public class Program
    {
        private static AutoResetEvent event_1 = new AutoResetEvent(true);
        private static AutoResetEvent event_2 = new AutoResetEvent(false);

        static void Main()
        {
            Debug.WriteLine("Next three threads will be created and started.\r\n" +
                              "The threads wait on AutoResetEvent #1, which was created\r\n" +
                              "in the signaled state, so the first thread is released.\r\n" +
                              "This puts AutoResetEvent #1 into the unsignaled state.");


            for (int i = 1; i < 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Start();
            }

            Thread.Sleep(250);

            for (int i = 0; i < 2; i++)
            {
                Debug.WriteLine("Releasing another thread.");
                Thread.Sleep(1000);

                event_1.Set();
                Thread.Sleep(250);
            }

            Debug.WriteLine("");
            Debug.WriteLine("");
            Debug.WriteLine("All threads are now waiting on AutoResetEvent #2.");

            Thread.Sleep(1000);

            for (int i = 0; i < 3; i++)
            {
                Debug.WriteLine("Releasing another thread.");
                Thread.Sleep(1000);

                event_2.Set();
                Thread.Sleep(250);
            }


            Thread.Sleep(Timeout.Infinite);
        }

        static void ThreadProc()
        {
            var id = Thread.CurrentThread.ManagedThreadId;

            Debug.WriteLine(
                $"{id} waits on AutoResetEvent #1.");
            event_1.WaitOne();

            Debug.WriteLine(
                $"{id} is released from AutoResetEvent #1.");

            Debug.WriteLine(
                $"{id} waits on AutoResetEvent #2.");
            event_2.WaitOne();

            Debug.WriteLine(
                $"{id} is released from AutoResetEvent #2.");

            Debug.WriteLine($"{id} ends.");
        }
    }
}
