//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Threading;

namespace ToStringTest
{
    public class Program
    {
        public static void Main()
        {
            TestCode.Output();

            for (; ; )
            {
                Thread.Sleep(10000);
            }
        }
    }
}
