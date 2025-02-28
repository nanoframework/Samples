// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace NumberParserTestShared
{
    public class TestSByte : PerformTestBase
    {
        public TestSByte()
        {
            TestName = "TestSByte";

            tests = new Test[]
            {
                new Test("0", 0),
                new Test("1", 1),
                new Test("-1"),

                new Test("255", SByte.MaxValue, true),
                new Test("-128"),
                new Test("127"),

                new Test("65535", true),
                new Test("-32768", true),
                new Test("32767", true),

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
            public Test(string inputString, sbyte result, bool throwsException = false)
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
                Result = (sbyte)0;
            }
        }

        public override bool PerformParse(string testString, out object value)
        {
            value = (sbyte)0;

            try
            {
                value = sbyte.Parse(testString);

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
            return ((sbyte)value).Equals((sbyte)expectedValue);
        }
    }
}
