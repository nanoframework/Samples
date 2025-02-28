// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Threading;
using NumberParserTestShared;

namespace DesktopNumberParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Change current culture to Invariant culture to match nanoFramework culture setting
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var testByte = new TestByte();
            testByte.RunTest();

            var testSByte = new TestSByte();
            testSByte.RunTest();

            var testUInt16 = new TestUInt16();
            testUInt16.RunTest();

            var testInt16 = new TestInt16();
            testInt16.RunTest();

            var testUInt32 = new TestUInt32();
            testUInt32.RunTest();

            var testInt32 = new TestInt32();
            testInt32.RunTest();

            var testUInt64 = new TestUInt64();
            testUInt64.RunTest();

            var testInt64 = new TestInt64();
            testInt64.RunTest();

            var testFloat = new TestFloat();
            testFloat.RunTest();

            var testDouble = new TestDouble();
            testDouble.RunTest();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
