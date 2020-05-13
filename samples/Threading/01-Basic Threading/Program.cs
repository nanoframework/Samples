//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Runtime.Native;
using System.Threading;

namespace Basic_Threading
{
    public class Program
    {
        public static void Main()
        {
            ServerClass serverObject = new ServerClass();

            // Create the thread object, passing in the
            // serverObject.InstanceMethod method using a
            // ThreadStart delegate.
            Thread instanceCaller = new Thread(
                new ThreadStart(serverObject.InstanceMethod));

            // Start the thread.
            instanceCaller.Start();

            Debug.WriteLine(
                "The Main() thread calls this after "
                + "starting the new InstanceCaller thread.");

            // Create the thread object, passing in the
            // serverObject.StaticMethod method using a
            // ThreadStart delegate.
            Thread staticCaller = new Thread(
                new ThreadStart(ServerClass.StaticMethod));

            // Start the thread.
            staticCaller.Start();

            Debug.WriteLine(
                "The Main() thread calls this after "
                + "starting the new StaticCaller thread.");

            // Create another thread object, using a lambda expression.
            // Without retaining any reference to it and starting it immidiatly

            new Thread(() => 
                {
                    Debug.WriteLine(
                        ">>>>>> This inline code is running on another thread.");

                    // Pause for a moment to provide a delay to make
                    // threads more apparent.
                    Thread.Sleep(6000);

                    Debug.WriteLine(
                        ">>>>>> The inline code by the worker thread has ended.");

                }).Start();

            Debug.WriteLine(
                "The Main() thread calls this after "
                + "starting the new inline thread with the lambda expression.");

            Thread.Sleep(Timeout.Infinite);
        }

        public class ServerClass
        {
            // The method that will be called when the thread is started.
            public void InstanceMethod()
            {
                Debug.WriteLine(
                    ">> ServerClass.InstanceMethod is running on another thread.");

                // Pause for a moment to provide a delay to make
                // threads more apparent.
                Thread.Sleep(3000);

                Debug.WriteLine(
                    ">> The instance method called by the worker thread has ended.");
            }

            public static void StaticMethod()
            {
                Debug.WriteLine(
                    ">>>> ServerClass.StaticMethod is running on another thread.");

                // Pause for a moment to provide a delay to make
                // threads more apparent.
                Thread.Sleep(5000);

                Debug.WriteLine(
                    ">>>> The static method called by the worker thread has ended.");
            }
        }
    }
}
