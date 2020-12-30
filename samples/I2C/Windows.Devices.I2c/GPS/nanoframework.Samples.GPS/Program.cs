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

            Debug.WriteLine("Application started...awaiting GPS and Acceleration data..");

            Thread.Sleep(3000); // Wait 3 seconds for DS-GPAM to initialise

            for ( ; ; )
            {
                for (int x = 0; x <= 29; x++)
                {
                    Debug.WriteLine(gps.GetDateTime().ToString("u"));
                    Thread.Sleep(1000);
                }

                Debug.Write("Latitude: ");
                Debug.WriteLine(gps.GetLatitude().ToString("N6"));

                Debug.Write("Longitude: ");
                Debug.WriteLine(gps.GetLongitude().ToString("N6"));

                Debug.Write("Heading: ");
                Debug.WriteLine(gps.GetHeading().ToString("N2"));

                Debug.Write("Speed: ");
                Debug.WriteLine(gps.GetSpeed().ToString("N2"));

                Debug.Write("Pitch: ");
                Debug.WriteLine(gps.GetPitch().ToString());

                Debug.Write("Roll:");
                Debug.WriteLine(gps.GetRoll().ToString());
            }

            //Thread.Sleep(Timeout.Infinite);
        }
    }
}



