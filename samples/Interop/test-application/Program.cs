using System;
using System.Diagnostics;
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

            Debug.WriteLine("cpu serial number: " + serialNumber);

            // test complicated calculation
            NF.AwesomeLib.Math math = new NF.AwesomeLib.Math();
            double result = math.SuperComplicatedCalculation(11.12);

            Debug.WriteLine("calculation result: " + result);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
