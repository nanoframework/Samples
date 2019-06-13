using System;

namespace ConverterTestShared
{
    public class TestByte
    {
        class Test
        {
            public string InputString { get; set; }
            public bool ThrowsException { get; set; }
            public Byte Result { get; set; }

            public Test(string inputString, bool throwsException, Byte result)
            {
                InputString = inputString;
                ThrowsException = throwsException;
                Result = result;
            }

            public Test(string inputString, Byte result)
            {
                InputString = inputString;
                ThrowsException = false;
                Result = result;
            }

            public Test(string inputString)
            {
                InputString = inputString;
                ThrowsException = true;
            }
        }

        static Test[] tests =
        {
            new Test("0", 0),
            new Test("1", 1),
            new Test("-1"),

            new Test("255", Byte.MaxValue),
            new Test("-128"),
            new Test("127", 127),

            new Test("65535"),
            new Test("-32768"),
            new Test("32767"),

            new Test("4294967295"),
            new Test("-2147483648"),
            new Test("2147483647"),

            new Test("18446744073709551615"),
            new Test("-9223372036854775808"),
            new Test("9223372036854775807"),

            new Test("NaN"),
            new Test("null"),
            new Test("123.1"),
            new Test("123,1"),
            new Test("1string"),
            new Test("string1"),
            new Test(""),
            new Test(" "),
            new Test("+123", 123),
            new Test(" 26", 26),
            new Test("27 ", 27),
            new Test(" 28 " , 28),
            new Test("true"),
            new Test("false"),
            new Test("1,0e+1"),
            new Test("1.0e+1"),
            new Test("0123", 123),
            new Test("0x123")
        };

        public static void RunTest(Boolean showOnlyFails = false)
        {
            int _fails = 0;
            int _testCount = 0;
            foreach (var test in tests)
            {
                _testCount++;
                bool exception = false;
                bool correctValue = true;

                try
                {
                    var val = Byte.Parse(test.InputString);
                    correctValue = (val == test.Result);
                }
                catch
                {
                    exception = true;
                }

                if (exception == test.ThrowsException && correctValue)
                {
                    if (!showOnlyFails)
                    {
                        Console.WriteLine("Parsing " + test.InputString + ": passed");
                    }
                }
                else
                {
                    _fails++;
                    Console.WriteLine("Parsing " + test.InputString + ": failed");
                }

            }

            Console.WriteLine("TestByte Tests: " + _testCount + " Fails: " + _fails);

        }

    }
}
