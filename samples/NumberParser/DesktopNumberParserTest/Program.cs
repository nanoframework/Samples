using System;
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

            TestByte.RunTest(true);
            TestSByte.RunTest(true);

            TestUInt16.RunTest(true);
            TestInt16.RunTest(true);

            TestUInt32.RunTest(true);
            TestInt32.RunTest(true);

            TestUInt64.RunTest(true);
            TestInt64.RunTest(true);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
