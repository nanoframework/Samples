//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;

namespace Retrieving_data_from_threads
{
    public class Program
    {
        public static void Main()
        {
            // Supply the state information required by the task.
            ThreadWithState tws = new ThreadWithState(
                "This report displays the number",
                42,
                new ExampleCallback(ResultCallback)
            );

            Thread t = new Thread(new ThreadStart(tws.ThreadProc));
            t.Start();

            Debug.WriteLine("Main thread does some work, then waits.");

            t.Join();

            Debug.WriteLine(
                "Independent task has completed; main thread ends.");

            Thread.Sleep(Timeout.Infinite);
        }

        // The callback method must match the signature of the
        // callback delegate.
        public static void ResultCallback(int lineCount)
        {
            Debug.WriteLine(
                $"Independent task printed {lineCount} lines.");
        }
    }

    // Delegate that defines the signature for the callback method.
    public delegate void ExampleCallback(int lineCount);

    // The ThreadWithState class contains the information needed for
    // a task, the method that executes the task, and a delegate
    // to call when the task is complete.
    public class ThreadWithState
    {
        // State information used in the task.
        private readonly string _boilerplate;
        private readonly int _numberValue;

        // Delegate used to execute the callback method when the
        // task is complete.
        private readonly ExampleCallback _callback;

        // The constructor obtains the state information and the
        // callback delegate.
        public ThreadWithState(
            string text, 
            int number,
            ExampleCallback callbackDelegate)
        {
            _boilerplate = text;
            _numberValue = number;
            _callback = callbackDelegate;
        }

        // The thread procedure performs the task, such as
        // formatting and printing a document, and then invokes
        // the callback delegate with the number of lines printed.
        public void ThreadProc()
        {
            int counter;

            for (counter = 0; counter < 5; counter++)
            {
                Debug.WriteLine(
                    $"Report processing iteration #{counter}.");
            }

            Debug.WriteLine(
                $"{counter}: {_boilerplate} {_numberValue}.");

            _callback?.Invoke(counter);
        }
    }
}
