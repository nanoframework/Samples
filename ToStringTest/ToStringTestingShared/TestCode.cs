//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace ToStringTest
{
    public class TestCode
    {
        public static void Output()
        {
            int zero = 0;
            int i = 12345;
            int iNeg = -12345;
            double d = 123.45;
            float f = 456.78F;
            var i64 = Convert.ToInt64("01010101", 2);// this is 85 (decimal)
            long i64a = 1234567;
            ulong i64b = 200;

            ////////////////////////////////////////////
            Console.WriteLine("*************************");
            Console.WriteLine("* plain ToString() test *");
            Console.WriteLine("*************************");

            Console.WriteLine("integer '0': " + zero.ToString());
            Console.WriteLine("integer '12345': " + i.ToString());
            Console.WriteLine("integer '-12345': " + iNeg.ToString());
            Console.WriteLine("double '123.45': " + d.ToString());
            Console.WriteLine("float '456.78F': " + f.ToString());
            Console.WriteLine("long '85': " + i64.ToString());
            Console.WriteLine("long '1234567': " + i64a.ToString());
            Console.WriteLine("ulong '200': " + i64b.ToString());

            ////////////////////////////////////////////
            Console.WriteLine("");
            Console.WriteLine("**********************");
            Console.WriteLine(@"* ToString(""X"") test *");
            Console.WriteLine("**********************");

            Console.WriteLine("integer '0': " + zero.ToString("X"));
            Console.WriteLine("integer '12345': " + i.ToString("X"));
            Console.WriteLine("integer '-12345': " + iNeg.ToString("X"));
            Console.WriteLine("long '85': " + i64.ToString("X"));
            Console.WriteLine("long '1234567': " + i64a.ToString("X"));
            Console.WriteLine("ulong '200': " + i64b.ToString("X"));

            ////////////////////////////////////////////
            Console.WriteLine("");
            Console.WriteLine("**********************");
            Console.WriteLine(@"* ToString(""X2"") test *");
            Console.WriteLine("**********************");

            Console.WriteLine("integer '0': " + zero.ToString("X2"));
            Console.WriteLine("integer '12345': " + i.ToString("X2"));
            Console.WriteLine("integer '-12345': " + iNeg.ToString("X2"));
            Console.WriteLine("long '85': " + i64.ToString("X2"));
            Console.WriteLine("long '1234567': " + i64a.ToString("X2"));
            Console.WriteLine("ulong '200': " + i64b.ToString("X2"));

            ////////////////////////////////////////////
            Console.WriteLine("");
            Console.WriteLine("**********************");
            Console.WriteLine(@"* ToString(""X0"") test *");
            Console.WriteLine("**********************");

            Console.WriteLine("integer '0': " + zero.ToString("X0"));
            Console.WriteLine("integer '12345': " + i.ToString("X0"));
            Console.WriteLine("integer '-12345': " + iNeg.ToString("X0"));
            Console.WriteLine("long '85': " + i64.ToString("X0"));
            Console.WriteLine("long '1234567': " + i64a.ToString("X0"));
            Console.WriteLine("ulong '200': " + i64b.ToString("X0"));

            ////////////////////////////////////////////
            Console.WriteLine("");
            Console.WriteLine("**********************");
            Console.WriteLine(@"* ToString(""N0"") test *");
            Console.WriteLine("**********************");

            Console.WriteLine("integer '0': " + zero.ToString("N0"));
            Console.WriteLine("integer '12345': " + i.ToString("N0"));
            Console.WriteLine("integer '-12345': " + iNeg.ToString("N0"));
            Console.WriteLine("long '85': " + i64.ToString("N0"));
            Console.WriteLine("long '1234567': " + i64a.ToString("N0"));
            Console.WriteLine("ulong '200': " + i64b.ToString("N0"));
        }
    }
}
