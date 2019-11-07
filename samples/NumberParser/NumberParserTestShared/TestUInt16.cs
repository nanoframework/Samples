using System;

namespace NumberParserTestShared
{
    public class TestUInt16 : PerformTestBase
    {
        public TestUInt16()
        {
            TestName = "TestUInt16";

            tests = new Test[]
            {
                new Test("0", 0),
                new Test("1", 1),
                new Test("-1", true),

                new Test("255", UInt16.MaxValue),
                new Test("-128", true),
                new Test("127", 127),

                new Test("65535"),
                new Test("-32768", true),
                new Test("32767"),

                new Test("4294967295", true),
                new Test("-2147483648", true),
                new Test("2147483647", true),

                new Test("18446744073709551615", true),
                new Test("-9223372036854775808", true),
                new Test("9223372036854775807", true),

                new Test("NaN", true),
                new Test("null", true),
                new Test("123.1", true),
                new Test("123,1", true),
                new Test("1string", true),
                new Test("string1", true),
                new Test("", true),
                new Test(" ", true),
                new Test("+123", 123),
                new Test(" 26", 26),
                new Test("27 ", 27),
                new Test(" 28 " , 28),
                new Test("true", true),
                new Test("false", true),
                new Test("1,0e+1", true),
                new Test("1.0e+1", true),
                new Test("0123", 123),
                new Test("0x123", true)
            };

        }

        class Test : TestBase
        {
            public Test(string inputString, ushort result, bool throwsException = false)
                : base(inputString, throwsException)
            {
                InputString = inputString;
                ThrowsException = throwsException;
                Result = result;
            }

            public Test(string inputString, bool throwsException = false)
                : base(inputString, throwsException)
            {
                InputString = inputString;
                ThrowsException = throwsException;
                Result = (ushort)0;
            }
        }

        public override bool PerformParse(string testString, out object value)
        {
            value = (ushort)0;

            try
            {
                value = ushort.Parse(testString);

                return true;
            }
            catch
            {
                // just want to catch the exception
            }

            return false;
        }

        public override bool PerformCompare(object value, object expectedValue)
        {
            return ((ushort)value).Equals((ushort)expectedValue);
        }
    }
}
