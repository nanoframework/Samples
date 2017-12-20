//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Threading;

namespace ToStringTest
{
    public class Program
    {
        public static void Main()
        {
            // workaround to allow debug engine to load before a breakpoint is hit
            Thread.Sleep(5000);

            TestCode.Output();

            for (; ; )
            {
                Thread.Sleep(10000);
            }
        }
    }
}
