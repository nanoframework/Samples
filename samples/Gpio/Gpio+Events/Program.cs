//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

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
        private static GpioPin _exposedPad;

        public static void Main()
        {
            var gpioController = GpioController.GetDefault();

            /////////////////////
            // setup green LED //
            /////////////////////

            // F4-Discovery -> LD4 LED is @ PD12
            // F429I-Discovery -> LD3 is @ PG13
            //_greenLED = gpioController.OpenPin(PinNumber('G', 13));
            // F769I-DISCO -> LED2_GREEN is @ PJ5
            //_greenLED = gpioController.OpenPin(PinNumber('J', 5));
            // F746ZG-NUCLEO -> Off board LED is @ PC10
            // TI CC13x2 Launchpad: DIO_07 it's the green LED
            _greenLED = GpioController.GetDefault().OpenPin(7);

            _greenLED.SetDriveMode(GpioPinDriveMode.Output);

            ///////////////////
            // setup red LED //
            ///////////////////
            
            // F4-Discovery -> LD5 is @ PD14
            // F429I-Discovery -> LD4 is @ PG14
            //_redLED = gpioController.OpenPin(PinNumber('G', 14));
            // F769I-DISCO -> LED2_RED is @ PJ13
            //_redLED = gpioController.OpenPin(PinNumber('J', 13));
            // TI CC13x2 Launchpad: DIO_06 it's the red LED  
            _redLED = GpioController.GetDefault().OpenPin(6);

            _redLED.SetDriveMode(GpioPinDriveMode.Output);

            ///////////////////////
            // setup user button //
            ///////////////////////

            // F4-Discovery -> USER_BUTTON is @ PA0 (input only)
            // F769I-DISCO -> USER_BUTTON is @ PA0 (input only)
            //_userButton = gpioController.OpenPin(PinNumber('A', 0));
            //_userButton.SetDriveMode(GpioPinDriveMode.Input);

            // TI CC13x2 Launchpad: DIO_15 it's BTN-1 (input requiring pull-up)
            _userButton = GpioController.GetDefault().OpenPin(15);
            _userButton.SetDriveMode(GpioPinDriveMode.InputPullUp);

            _userButton.ValueChanged += UserButton_ValueChanged;

            //////////////////////
            // setup other GPIO //
            //////////////////////

            // F769I-DISCO -> using PA7 (input only)
            //_exposedPad = gpioController.OpenPin(PinNumber('A', 7));
            //_exposedPad.SetDriveMode(GpioPinDriveMode.InputPullUp);

            // TI CC13x2 Launchpad: DIO_14 it's BTN-2 (input requiring pull-up)
            _exposedPad = GpioController.GetDefault().OpenPin(14);
            _exposedPad.SetDriveMode(GpioPinDriveMode.InputPullUp);

            // add a debounce timeout 
            _exposedPad.DebounceTimeout = new TimeSpan(0, 0, 0, 0, 100);
            _exposedPad.ValueChanged += ExposedPad_ValueChanged;

            for (; ; )
            {
                _redLED.Toggle();
                Thread.Sleep(1000);
            }
        }

        private static void UserButton_ValueChanged(object sender, GpioPinValueChangedEventArgs e)
        {
            // read Gpio pin value from event
            Console.WriteLine("USER BUTTON (event) : " + e.Edge.ToString());

            // direct read Gpio pin value
            Console.WriteLine("USER BUTTON (direct): " + _userButton.Read());

            if (e.Edge == GpioPinEdge.RisingEdge)
            {
                _greenLED.Write(GpioPinValue.High);
            }
            else
            {
                _greenLED.Write(GpioPinValue.Low);
            }
        }

        private static void ExposedPad_ValueChanged(object sender, GpioPinValueChangedEventArgs e)
        {
            // read Gpio pin value from event
            Console.WriteLine("PAD (event) : " + e.Edge.ToString());

            // direct read Gpio pin value
            Console.WriteLine("PAD (direct): " + _exposedPad.Read());

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
