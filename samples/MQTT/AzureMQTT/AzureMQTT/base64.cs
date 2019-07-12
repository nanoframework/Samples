using System;
using System.Text;

namespace AzureMQTT
{
    public static class base64
    {
        public static string Encode(byte[] data)

        {

            int length = data == null ? 0 : data.Length;

            if (length == 0)

                return null;



            int padding = length % 3;

            if (padding > 0)

                padding = 3 - padding;

            int blocks = (length - 1) / 3 + 1;



            char[] encodedData = new char[blocks * 4];



            for (int i = 0; i < blocks; i++)

            {

                bool finalBlock = i == blocks - 1;

                bool pad2 = false;

                bool pad1 = false;

                if (finalBlock)

                {

                    pad2 = padding == 2;

                    pad1 = padding > 0;

                }



                int index = i * 3;

                byte b1 = data[index];

                byte b2 = pad2 ? (byte)0 : data[index + 1];

                byte b3 = pad1 ? (byte)0 : data[index + 2];



                byte temp1 = (byte)((b1 & 0xFC) >> 2);



                byte temp = (byte)((b1 & 0x03) << 4);

                byte temp2 = (byte)((b2 & 0xF0) >> 4);

                temp2 += temp;



                temp = (byte)((b2 & 0x0F) << 2);

                byte temp3 = (byte)((b3 & 0xC0) >> 6);

                temp3 += temp;



                byte temp4 = (byte)(b3 & 0x3F);



                index = i * 4;

                encodedData[index] = SixBitToChar(temp1);

                encodedData[index + 1] = SixBitToChar(temp2);

                encodedData[index + 2] = pad2 ? '=' : SixBitToChar(temp3);

                encodedData[index + 3] = pad1 ? '=' : SixBitToChar(temp4);

            }

            return new string(encodedData);

        }





        private static byte CharToSixBit(char c)

        {

            byte b;

            if (c >= 'A' && c <= 'Z')

            {

                b = (byte)((int)c - (int)'A');

            }

            else if (c >= 'a' && c <= 'z')

            {

                b = (byte)((int)c - (int)'a' + 26);

            }

            else if (c >= '0' && c <= '9')

            {

                b = (byte)((int)c - (int)'0' + 52);

            }

            else if (c == CHAR_PLUS_SIGN)

            {

                b = (byte)62;

            }

            else

            {

                b = (byte)63;

            }

            return b;

        }



        private static char SixBitToChar(byte b)

        {

            char c;

            if (b < 26)

            {

                c = (char)((int)b + (int)'A');

            }

            else if (b < 52)

            {

                c = (char)((int)b - 26 + (int)'a');

            }

            else if (b < 62)

            {

                c = (char)((int)b - 52 + (int)'0');

            }

            else if (b == 62)

            {

                c = CHAR_PLUS_SIGN;

            }

            else

            {

                c = CHAR_SLASH;

            }

            return c;

        }


        private const int MIME_LINE_LENGTH = 76;

        private const char CHAR_PLUS_SIGN = '+';

        private const char CHAR_SLASH = '/';
    }
}
