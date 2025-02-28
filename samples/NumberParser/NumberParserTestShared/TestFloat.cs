// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace NumberParserTestShared
{
    public class TestFloat : PerformTestBase
    {
        public TestFloat()
        {
            TestName = "TestFloat";

            tests = new Test[]
            {
                new Test("0", 0),
                new Test("1", 1),
                new Test("-1", -1),

                new Test("255", Byte.MaxValue),
                new Test("-128", -128),
                new Test("127", 127),

                new Test("65535", UInt16.MaxValue),
                new Test("-32768", -32768),
                new Test("32767"),

                new Test("4294967295", UInt32.MaxValue),
                new Test("-2147483648", -2147483648),
                new Test("2147483647", Int32.MaxValue),

                new Test("18446744073709551615", UInt64.MaxValue),
                new Test("-9223372036854775808", -9223372036854775808),
                new Test("9223372036854775807", Int64.MaxValue),

                new Test("18446744073709551616"),

                new Test("NaN", float.NaN),
                new Test("Infinity", float.PositiveInfinity),
                new Test("-Infinity", float.NegativeInfinity),
                new Test("1.401298E-45", float.Epsilon),

                new Test("null", true),
                new Test("123.1"),
                new Test("123,1"),
                new Test("1string", true),
                new Test("string1", true),
                new Test("", true),
                new Test(" ", true),
                new Test("+123", 123),
                new Test(" 26", 26),
                new Test("27 ", 27),
                new Test(" 28 ", 28),
                new Test("true", true),
                new Test("false", true),
                new Test("1,0e+1"),
                new Test("1.0e+1"),
                new Test("0123", 123),
                new Test("0x123", true)
            };

        }

        class Test : TestBase
        {
            public Test(string inputString, float result, bool throwsException = false)
                :base(inputString, throwsException)
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
                Result = 0.0f;
            }
        }

        public override bool PerformParse(string testString, out object value)
        {
            value = 0.0f;

            try
            {
                value = float.Parse(testString);

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
            return ((float)value).CompareTo((float)expectedValue) == 0;
        }
    }
}
