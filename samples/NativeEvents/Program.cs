//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Runtime.Events;
using System;
using System.Threading;

namespace NativeEvents
{
    public class Program
    {
        public static void Main()
        {
            // setup event handler for custom event
            CustomEvent.CustomEventPosted += CustomEventHandler;

            /////////////////////////////////////////////////////////////////////////////////////////
            // VERY VERY VERY IMPORTANT                                                            //
            // The above WON'T produce any effect unless you have a call in native code like this: //
            //                                                                                     //
            // PostManagedEvent( EVENT_CUSTOM, 0, 1111, 2222 );                                    //
            //                                                                                     //
            /////////////////////////////////////////////////////////////////////////////////////////
            
            Thread.Sleep(Timeout.Infinite);
        }

        private static void CustomEventHandler(object sender, CustomEventArgs e)
        {
            Console.WriteLine($"Custom event received. Data1: { e.Data1 } Data2: { e.Data2 }.");
        }
    }
}
