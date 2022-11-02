using System;
using System.Device.Usb;
using System.Diagnostics;
using System.Threading;

namespace UsbStream
{
    public class Program
    {
        private static Guid deviceInterfaceId = new Guid("946FA842-0137-4C1F-A04C-368A619FFFDC");
        private static string deviceDescription = "nano USB device";

        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            // create USB Stream
            var usbStream = UsbClient.CreateUsbStream(
                deviceInterfaceId,
                deviceDescription);

            usbStream.ReadTimeout = -1;

            while (!usbStream.IsConnected)
            {
                Thread.Sleep(2000);
            }

            // buffer with dummy data 
            var bufer = new byte[] { 1, 2, 3 };

            usbStream.Write(bufer, 0, bufer.Length);

            usbStream.ReadTimeout = 2000;
            var count = usbStream.Read(bufer, 0, bufer.Length);


            Thread.Sleep(Timeout.Infinite);
        }
    }
}
