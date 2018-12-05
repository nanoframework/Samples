using nanoFramework.Devices.OneWire;
using System;
using System.Collections;
using System.Threading;

namespace OneWire.TestApp
{
    public class Program
    {
        public static void Main()
        {
            // get controller for OneWire
            OneWireController oneWire = new OneWireController();

            ////////////////////////////////////////////////////////////////////
            // Sample code to read serial number for a devices present in bus
            // 
            // With this approach, three steps are needed:
            // - Reset 1-Wire bus
            // - Transmit Read ROM command
            // - Read 8 bytes (serial number)
            ////////////////////////////////////////////////////////////////////

            byte[] state = new byte[13];
            // check for devices present with a bus reset
            if (oneWire.TouchReset())
            {
                Console.WriteLine("Device present");

                // tx READ ROM
                var res = oneWire.WriteByte(0x33);

                // now read 8 byte for SN
                // and output serial number nicelly formated in hexa
                for (int i = 0; i < 8; i++)
                {
                    // read byte
                    state[i] = oneWire.TouchByte(0xFF);

                    // output byte
                    Console.Write(state[i].ToString("X2"));
                }

                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("!! No devices found !!");
            }

            ////////////////////////////////////////////////////////////////////
            // Sample code to read serial number for a devices present in bus
            // 
            // Just call FindFirstDevice and it will fill OneWire SerialNumber 
            // buffer with the 1-Wire device serial number present in bus
            ////////////////////////////////////////////////////////////////////

            if (oneWire.FindFirstDevice(true, false))
            {
                Console.WriteLine("device found:");

                // output serial number nicelly formated in hexa
                for (int i = 0; i < 8; i++)
                {
                    Console.Write(oneWire.SerialNumber[i].ToString("X2"));
                }

                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("!! No devices found !!");
            }


            ////////////////////////////////////////////////////////////////////
            // Sample code to read serial number for all devices present in bus
            // 
            // Just call FindAllDevices and it will return an array list with 
            // all 1-Wire devices serial numbers present in bus
            ////////////////////////////////////////////////////////////////////

            ArrayList snList = oneWire.FindAllDevices();

            if (snList.Count > 0)
            {
                Console.WriteLine(String.Format("{0} devices found", snList.Count));

                foreach (byte[] sn in snList)
                {
                    // output serial number nicelly formated in hexa
                    for (int i = 0; i < 8; i++)
                    {
                        Console.Write(sn[i].ToString("X2"));
                    }

                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("!! No devices found !!");
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
