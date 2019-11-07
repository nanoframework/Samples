using System;
using System.Text;

namespace NumberParserTestShared
{
    public abstract class PerformTestBase
    {
        public string TestName { get; set; }

        public static TestBase[] tests;

        public abstract bool PerformParse(string testString, out object value);

        public abstract bool PerformCompare(object value, object expectedValue);

        public void RunTest(Boolean showOnlyFails = false)
        {
            int _fails = 0;
            int _testCount = 0;

            StringBuilder testReport = new StringBuilder();

            testReport.AppendLine();
            testReport.AppendLine($"*** {TestName} parsing ***");
            testReport.AppendLine();

            foreach (var test in tests)
            {
                _testCount++;

                bool parseOk = PerformParse(test.InputString, out object value);

                bool correctValue = PerformCompare(value, test.Result);

                if (correctValue)
                {
                    if (!showOnlyFails)
                    {
                        testReport.AppendLine("Parsing " + test.InputString + ": passed");
                    }
                }
                else
                {
                    testReport.AppendLine("Parsing " + test.InputString + ": failed");
                }

                if (parseOk == test.ThrowsException)
                {
                    _fails++;
                }
            }

            Console.WriteLine(testReport.ToString());
            Console.WriteLine($"{TestName} tests: " + _testCount + " fails: " + _fails);
            Console.WriteLine("");
        }

    }
}
