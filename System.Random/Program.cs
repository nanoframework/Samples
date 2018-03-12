using System;
using System.Threading;

namespace SystemRandom.Sample
{
    public class Program
    {
        public static void Main()
        {
            // instantiate random generator
            Random randomGenerator = new Random();

            for (; ; )
            {
                int counter = 0;

                // generate block of 10 random integers between 0 and 100
                Console.WriteLine("");
                Console.WriteLine("-- 10 random integers between 0 and 100 --");
                while (counter++ < 10)
                {
                    var value = randomGenerator.Next(100);
                    Console.WriteLine(value.ToString());
                }

                counter = 0;

                // generate block of 10 random integers between 0 and 999999
                Console.WriteLine("");
                Console.WriteLine("-- 10 random integers between 0 and 999999 --");
                while (counter++ < 10)
                {
                    var value = randomGenerator.Next(999999);
                    Console.WriteLine(value.ToString());
                }

                counter = 0;

                // generate block of 10 random doubles
                Console.WriteLine("");
                Console.WriteLine("-- 10 random doubles --");
                while (counter++ < 10)
                {
                    var value = randomGenerator.NextDouble();
                    Console.WriteLine(value.ToString());
                }

                // fill byte array with 10 random numbers and output
                Console.WriteLine("");
                Console.WriteLine("-- 10 random numbers from array --");

                byte[] buffer = new byte[10];
                randomGenerator.NextBytes(buffer);

                counter = 0;
                while (counter < 10)
                {
                    var value = randomGenerator.NextDouble();
                    Console.WriteLine(buffer[counter++].ToString());
                }

                Thread.Sleep(500);
            }
        }
    }
}
