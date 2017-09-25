using System;

namespace ToStringTest
{
	public class Program
    {
        public static void Main()
        {
            int i = 12345;
            double d = 123.45;
            float f = 456.78F;
            string strINTEGER = i.ToString();
            Console.WriteLine(strINTEGER);

            string strDOUBLE = d.ToString();
            Console.WriteLine(strDOUBLE);

            string strFLOAT = f.ToString();
            Console.WriteLine(strFLOAT);
        }
    }
}
