//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;

namespace Passing_Parameters
{
    public class Program
    {
        public static void Main()
        {
            // Supply the state information required by the task.
            ThreadWithState tws = new ThreadWithState(
                "This report displays the number", 42);

            // Create a thread to execute the task, and then...

            Thread t = new Thread(new ThreadStart(tws.ThreadProc));

            // ...start the thread
            t.Start();

            Debug.WriteLine("Main thread does some work, then waits.");

            t.Join();

            Debug.WriteLine(
                "Independent task has completed; main thread ends.");

            Thread.Sleep(Timeout.Infinite);
        }

        // The ThreadWithState class contains the information needed for
        // a task, and the method that executes the task.
        public class ThreadWithState
        {
            // State information used in the task.
            private readonly string _boilerplate;
            private readonly int _numberValue;

            // The constructor obtains the state information.
            public ThreadWithState(string text, int number)
            {
                _boilerplate = text;
                _numberValue = number;
            }

            // The thread procedure performs the task, such as formatting
            // and printing a document.
            public void ThreadProc()
            {
                Debug.WriteLine(
                    $"{_boilerplate} {_numberValue}.");
            }
        }
    }
}
