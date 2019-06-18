using System;
using System.Text;
#if (!NANOFRAMEWORK_1_0)
using System.Linq;
#endif


namespace Base64TestingShared
{
    public class TestCode
    {
        static byte[] base64TestDecode =
        {
            0x24, 0x48, 0x6E, 0x56, 0x87, 0x62, 0x5A, 0xBD,
            0xBF, 0x17, 0xD9, 0xA2, 0xC4, 0x17, 0x1A, 0x01,
            0x94, 0xED, 0x8F, 0x1E, 0x11, 0xB3, 0xD7, 0x09,
            0x0C, 0xB6, 0xE9, 0x10, 0x6F, 0x22, 0xEE, 0x13,
            0xCA, 0xB3, 0x07, 0x05, 0x76, 0xC9, 0xFA, 0x31,
            0x6C, 0x08, 0x34, 0xFF, 0x8D, 0xC2, 0x6C, 0x38,
            0x00, 0x43, 0xE9, 0x54, 0x97, 0xAF, 0x50, 0x4B,
            0xD1, 0x41, 0xBA, 0x95, 0x31, 0x5A, 0x0B, 0x97
        };

        static char[] encoding_table = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
                                'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                                'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
                                'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
                                'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
                                'w', 'x', 'y', 'z', '0', '1', '2', '3',
                                '4', '5', '6', '7', '8', '9', '+', '/'};

        static string base64TestEncode = "JEhuVodiWr2/F9mixBcaAZTtjx4Rs9cJDLbpEG8i7hPKswcFdsn6MWwINP+Nwmw4AEPpVJevUEvRQbqVMVoLlw==";

        /// <summary>
        /// Test base 64 convertion
        /// </summary>
        public static void Output()
        {

            Console.WriteLine("  Base64 encoding test: ");

            // encode a base64 test decode array
            var base64ConvertionResult = Convert.ToBase64String(base64TestDecode, Base64FormattingOptions.InsertLineBreaks);
            // compare strings, they should match
            if (!base64ConvertionResult.Equals(BuildExpectedString(Base64FormattingOptions.InsertLineBreaks)))
            {
                // no match, something went wrong
                Console.WriteLine("failed\n");
                // exit
                return;
            }
            // convert passed
            Console.WriteLine("passed\n  Base64 decoding test: ");

            // decode a base64 test encode string
            var base64DecodeConvertionResult = Convert.FromBase64String(base64TestEncode);
            // compare arrays, they should match
#if (NANOFRAMEWORK_1_0)
            if (base64DecodeConvertionResult.GetHashCode() != base64TestDecode.GetHashCode())
#else
            var result = base64DecodeConvertionResult.Except(base64TestDecode);
            if (result.Count() > 0)
#endif
            {
                // no match, something went wrong
                Console.WriteLine("failed\n");
                // exit
                return;
            }
            // convert passed
            Console.WriteLine("passed\n\n");
        }

        private static object BuildExpectedString(Base64FormattingOptions insertLineBreaks)
        {
            if (insertLineBreaks != Base64FormattingOptions.InsertLineBreaks)
                return base64TestEncode;

            // get line break count (line break every 76 chars)
            int lineBreakCount = base64TestEncode.Length / 76;

            StringBuilder st = new StringBuilder();
            int outputLength = base64TestEncode.Length;
            int offset = 0;
            for (int i = 0; i <= lineBreakCount; i++)
            {
                // how many chars to copy
                int count;
                if (outputLength > 76)
                {
                    // first/next 76 chars
                    count = 76;
                }
                else
                {
                    // last outputLength chars
                    count = outputLength;
                }

                // copy first/next count chars
                // because we are using same offset for both arrays, we need to discount line break for tmp array
                st.Append(base64TestEncode, offset, count);

                // move offset for next copy
                offset += count;

                // remove copied chars from original output length if more than 76 chars still to be copied
                if (outputLength >= 76)
                {
                    // more chars

                    // adjust output length
                    outputLength -= 76;
                    // add line break
                    st.AppendLine();
                }
            }
            return st.ToString();
        }
    }
}
