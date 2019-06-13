using ConverterTestShared;
using System;
using System.Threading;

namespace ConverterTest
{
    public class Program
    {
        public static void Main()
        {
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
