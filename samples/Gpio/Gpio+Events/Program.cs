//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

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
            var gpioController = new GpioController();

            /////////////////////
            // setup green LED //
            /////////////////////

            // F4-Discovery -> LD4 LED is @ PD12
            // F429I-Discovery -> LD3 is @ PG13
            //_greenLED = gpioController.OpenPin(PinNumber('G', 13), PinMode.Output);
            // F769I-DISCO -> LED2_GREEN is @ PJ5
            _greenLED = gpioController.OpenPin(PinNumber('J', 5), PinMode.Output);
            // F746ZG-NUCLEO -> Off board LED is @ PC10
            // TI CC13x2 Launchpad: DIO_07 it's the green LED
            _greenLED = gpioController.OpenPin(7, PinMode.Output);

            ///////////////////
            // setup red LED //
            ///////////////////

            // F4-Discovery -> LD5 is @ PD14
            // F429I-Discovery -> LD4 is @ PG14
            //_redLED = gpioController.OpenPin(PinNumber('G', 14), PinMode.Output);
            // F769I-DISCO -> LED2_RED is @ PJ13
            _redLED = gpioController.OpenPin(PinNumber('J', 13), PinMode.Output);
            // TI CC13x2 Launchpad: DIO_06 it's the red LED  
            _redLED = gpioController.OpenPin(6, PinMode.Output);

            ///////////////////////
            // setup user button //
            ///////////////////////

            // F4-Discovery -> USER_BUTTON is @ PA0 (input only)
            // F769I-DISCO -> USER_BUTTON is @ PA0 (input only)
            _userButton = gpioController.OpenPin(PinNumber('A', 0), PinMode.Output);

            // TI CC13x2 Launchpad: DIO_15 it's BTN-1 (input requiring pull-up)
            _userButton = gpioController.OpenPin(15, PinMode.Input);

            _userButton.ValueChanged += UserButton_ValueChanged;

            //////////////////////
            // setup other GPIO //
            //////////////////////

            // F769I-DISCO -> using PA7 (input only)
            _exposedPad = gpioController.OpenPin(PinNumber('A', 7), PinMode.Output);

            // TI CC13x2 Launchpad: DIO_14 it's BTN-2 (input requiring pull-up)
            _exposedPad = gpioController.OpenPin(14, PinMode.Input);

            // add a debounce timeout 
            _exposedPad.DebounceTimeout = new TimeSpan(0, 0, 0, 0, 100);
            _exposedPad.ValueChanged += ExposedPad_ValueChanged;

            for (; ; )
            {
                _redLED.Toggle();
                Thread.Sleep(1000);
            }
        }

        private static void UserButton_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            // read Gpio pin value from event
            Debug.WriteLine("USER BUTTON (event) : " + e.ChangeType.ToString());

            // direct read Gpio pin value
            Debug.WriteLine("USER BUTTON (direct): " + _userButton.Read());

            if (e.ChangeType ==  PinEventTypes.Rising)
            {
                _greenLED.Write(PinValue.High);
            }
            else
            {
                _greenLED.Write(PinValue.Low);
            }
        }

        private static void ExposedPad_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            // read Gpio pin value from event
            Debug.WriteLine("PAD (event) : " + e.ChangeType.ToString());

            // direct read Gpio pin value
            Debug.WriteLine("PAD (direct): " + _exposedPad.Read());

            if (e.ChangeType == PinEventTypes.Rising)
            {
                _greenLED.Write(PinValue.High);
            }
            else
            {
                _greenLED.Write(PinValue.Low);
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
