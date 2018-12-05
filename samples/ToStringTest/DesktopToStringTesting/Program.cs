//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Globalization;
using System.Threading;

namespace ToStringTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Change current culture to Invariant culture to match nanoFramework culture setting
            CultureInfo dft = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            TestCode.Output();
        }
    }
}
