using System;

namespace NumberParserTestShared
{
    public abstract class TestBase
    {
        public string InputString { get; set; }
        public bool ThrowsException { get; set; }

        public object Result { get; set; }

        protected TestBase(string inputString, bool throwsException = false)
        {
            InputString = inputString;
            ThrowsException = throwsException;
        }
    }
}
