using nanoFramework.Runtime.Native;
using Windows.Devices.Gpio;
using System;
using System.Threading;

namespace DebugGC.Test
{
    public class Program
    {
        public static void Main()
        {
            Debug.EnableGCMessages(true);

            // mind to set a pin that exists on the board being tested
            // PJ5 is LD2 in STM32F769I_DISCO
            GpioPin led = GpioController.GetDefault().OpenPin(PinNumber('J', 5));

            led.SetDriveMode(GpioPinDriveMode.Output);

            for(; ; )
            {
                led.Write(GpioPinValue.High);
                Thread.Sleep(1000);
                led.Write(GpioPinValue.Low);
                Thread.Sleep(1000);
                Debug.GC(true);
            }
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
