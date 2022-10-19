using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using GiantGecko = nanoFramework.Hardware.GiantGecko;

namespace PowerMode
{
    public class Program
    {
        private static GpioController _gpioController;

        public static void Main()
        {
            _gpioController = new GpioController();

            // Silabs SLSTK3701A: LED1 PH14 is LLED1
            GpioPin led = _gpioController.OpenPin(PinNumber('H', 14), PinMode.Output);

            // start a thread blinking the LED to check that something is happening 
            new Thread(() => {
                while (true)
                {
                    Thread.Sleep(125);
                    led.Toggle();
                }
            }).Start();

            // sleep here for 5 seconds to allow the LED to blink
            Thread.Sleep(5_000);

            Debug.WriteLine($"Going to standby mode now...");

            //////////////////////////////////////
            // *** these calls never return *** //
            //////////////////////////////////////
            
            /////////////////////////////////////////////////////////////////////////////////
            // Uncomment one of the following according to the power mode you want to test //
            /////////////////////////////////////////////////////////////////////////////////

            // after this the target will enter EM4 hibernate mode (RTC will be on and counting)
            GiantGecko.Power.EnterHibernateMode();

            // after this the target will enter EM4 shutoff mode: nothing is running on the device and everything is shutoff 
            // GiantGecko.Power.EnterShutoffMode();
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
