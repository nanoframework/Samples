using System;
using System.Threading;
using NF.AwesomeLib;

namespace Test.Interop
{
    public class Program
    {
        public static void Main()
        {
            // testing cpu serial number
            string serialNumber = "";

            foreach (byte b in Utilities.HardwareSerial)
            {
                serialNumber += b.ToString("X2");
            }

            Console.WriteLine("cpu serial number: " + serialNumber);

            // test complicated calculation
            NF.AwesomeLib.Math math = new NF.AwesomeLib.Math();
            double result = math.SuperComplicatedCalculation(11.12);

            Console.WriteLine("calculation result: " + result);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
