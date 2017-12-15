using System;
using System.Threading;
using Windows.Devices.Gpio;

namespace Gpio_Events.Test
{
	public class Program
    {
        private static GpioPin _greenLED;
        private static GpioPin _redLED;
        private static GpioPin _userButton;

        public static void Main()
        {
            var gpioController = GpioController.GetDefault();

            // setup green LED
            // F4-Discovery -> Off board LED is @ PB7
            // F429I-Discovery -> Off board LED is @ PG13
            // F769I-DISCO -> LED2_GREEN is @ PJ5
            // F746ZG-NUCLEO -> Off board LED is @ PC10
            _greenLED = gpioController.OpenPin(PinNumber('J', 5));
            _greenLED.SetDriveMode(GpioPinDriveMode.Output);

            // setup red LED
            // F769I-DISCO -> LED2_RED is @ PJ13
            _redLED = gpioController.OpenPin(PinNumber('J', 13));
            _redLED.SetDriveMode(GpioPinDriveMode.Output);

            // setup user button
            // F769I-DISCO -> USER_BUTTON is @ PA0 
            _userButton = gpioController.OpenPin(PinNumber('A', 0));
            _userButton.SetDriveMode(GpioPinDriveMode.Input);
            _userButton.ValueChanged += UserButton_ValueChanged;

            for (; ; )
            {
                _redLED.Write(GpioPinValue.High);
                Thread.Sleep(1000);
                _redLED.Write(GpioPinValue.Low);
                Thread.Sleep(1000);
            }
        }

        private static void UserButton_ValueChanged(object sender, GpioPinValueChangedEventArgs e)
        {
            Console.WriteLine("USER BUTTON: " + e.Edge.ToString());

            if (e.Edge == GpioPinEdge.RisingEdge)
            {
                _greenLED.Write(GpioPinValue.High);
            }
            else
            {
                _greenLED.Write(GpioPinValue.Low);
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
