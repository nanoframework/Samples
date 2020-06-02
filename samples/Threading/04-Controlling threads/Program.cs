//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;

namespace Controlling_threads
{
    public class Program
    {
        public static void Main()
        {
            // create and start a thread
            var sleepingThread1 = new Thread(RunIndefinitely);
            sleepingThread1.Start();

            Thread.Sleep(2000);

            // suspend 1st thread
            sleepingThread1.Suspend();

            Thread.Sleep(1000);

            // create and start 2nd thread
            var sleepingThread2 = new Thread(RunIndefinitely);
            sleepingThread2.Start();

            Thread.Sleep(2000);

            // abort 2nd thread
            sleepingThread2.Abort();

            // abort 1st thread
            sleepingThread1.Abort();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void RunIndefinitely()
        {
            Debug.WriteLine(
                $"Thread {Thread.CurrentThread.ManagedThreadId} about to sleep indefinitely.");
            
            try
            {
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadAbortException)
            {
                Debug.WriteLine(
                    $"Thread {Thread.CurrentThread.ManagedThreadId} aborted.");
            }
            finally
            {
                Debug.WriteLine(
                    $"Thread {Thread.CurrentThread.ManagedThreadId} executing finally block.");
            }

            Debug.WriteLine(
                $"Thread {Thread.CurrentThread.ManagedThreadId} finishing normal execution.");

            Debug.WriteLine("");
        }
    }
}
