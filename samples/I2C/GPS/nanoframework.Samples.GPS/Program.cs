using System;
using System.Diagnostics;
using System.Threading;
using nanoframework.Drivers.GPS;

namespace nanoframework.Samples.GPS
{
    public class Program
    {

        public static void Main()
        {
            //Configured for use on the STM32F769I-Discovery
            //NOTE:- this shield is not pin compatible and must be wired as follows:
            //SDA = CN9 PIN D14
            //SDC = CN9 PIN D15
            //VIN = CN11 PIN 5 (5V)
            //GND = CN11 PIN 6 (GND)
            IesShieldGps gps = new IesShieldGps("I2C1", 0x68);

            Console.WriteLine("Application started...awaiting GPS and Acceleration data..");

            Thread.Sleep(3000); // Wait 3 seconds for DS-GPAM to initialise

            for ( ; ; )
            {
                for (int x = 0; x <= 29; x++)
                {
                    Console.WriteLine(gps.GetDateTime().ToString("u"));
                    Thread.Sleep(1000);
                }

                Console.Write("Latitude: ");
                Console.WriteLine(gps.GetLatitude().ToString("N6"));

                Console.Write("Longitude: ");
                Console.WriteLine(gps.GetLongitude().ToString("N6"));

                Console.Write("Heading: ");
                Console.WriteLine(gps.GetHeading().ToString("N2"));

                Console.Write("Speed: ");
                Console.WriteLine(gps.GetSpeed().ToString("N2"));

                Console.Write("Pitch: ");
                Console.WriteLine(gps.GetPitch().ToString());

                Console.Write("Roll:");
                Console.WriteLine(gps.GetRoll().ToString());
            }

            //Thread.Sleep(Timeout.Infinite);
        }
    }
}



