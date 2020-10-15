//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.Gpio;
using System.Threading;

namespace Gpio_Events.Test
{
	public class Program
    {
        static GpioController s_GpioController;
        static int s_GreenPinNumber;
        static int s_RedPinNumber;
        static int s_UserButtonPinNumber;

        public static void Main()
        {
            s_GpioController = new GpioController();

            // F4-Discovery -> LD4 LED is @ PD12
            // F4-Discovery -> LD5 is @ PD14
            // F4-Discovery -> USER_BUTTON is @ PA0 (input only)
            //var s_GreenPinNumber = PinNumber('D', 12);
            //var s_RedPinNumber = PinNumber('D', 14);
            //var s_UserButtonPinNumber = PinNumber('A', 0);

            // F429I-Discovery -> LD3 is @ PG13
            // F429I-Discovery -> LD4 is @ PG14
            // F429I-DISCO -> USER_BUTTON is @ PA0 (input only)
            s_GreenPinNumber = PinNumber('G', 13);
            s_RedPinNumber = PinNumber('G', 14);
            s_UserButtonPinNumber = PinNumber('A', 0);

            // F769I-DISCO -> LED2_GREEN is @ PJ5
            // F769I-DISCO -> LED2_RED is @ PJ13
            // F769I-DISCO -> USER_BUTTON is @ PA0 (input only)
            //var s_GreenPinNumber = PinNumber('J', 5);
            //var s_RedPinNumber = PinNumber('J', 13);
            //var s_UserButtonPinNumber = PinNumber('A', 0);

            // F746ZG-NUCLEO -> Off board LED is @ PC10
            //var s_GreenPinNumber = PinNumber('C', 10);
            //var s_UserButtonPinNumber = PinNumber('A', 0);

            // TI CC13x2 Launchpad: DIO_07 it's the green LED
            // TI CC13x2 Launchpad: DIO_06 it's the red LED  
            // TI CC13x2 Launchpad: DIO_15 it's BTN-1 (input requiring pull-up)
            //var s_GreenPinNumber = 7;
            //var s_RedPinNumber = 6;
            //var s_UserButtonPinNumber = 15;

            // ESP32 DevKit: 4 is a valid GPIO pin in, some boards like Xiuxin ESP32 may require GPIO Pin 2 instead.
            //var s_GreenPinNumber = 4; 

            /////////////////////
            // setup green LED //
            /////////////////////
            s_GpioController.OpenPin(s_GreenPinNumber, PinMode.Output);

            ///////////////////
            // setup red LED //
            ///////////////////
            s_GpioController.OpenPin(s_RedPinNumber, PinMode.Output);

            ///////////////////////
            // setup user button //
            ///////////////////////
            s_GpioController.OpenPin(s_UserButtonPinNumber, PinMode.Input);

            s_GpioController.RegisterCallbackForPinValueChangedEvent(
                PinNumber('A', 0),
                PinEventTypes.Falling | PinEventTypes.Rising,
                UserButton_ValueChanged);

            for (; ; )
            {
                s_GpioController.Write(s_RedPinNumber, PinValue.High);
                Thread.Sleep(1000);
                s_GpioController.Write(s_RedPinNumber, PinValue.Low);
                Thread.Sleep(1000);
            }
        }

        private static void UserButton_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            // read Gpio pin value from event
            Console.WriteLine("USER BUTTON (event) : " + e.ChangeType.ToString());

            if (e.ChangeType == PinEventTypes.Rising)
            {
                s_GpioController.Write(s_GreenPinNumber, PinValue.High);
            }
            else
            {
                s_GpioController.Write(s_GreenPinNumber, PinValue.Low);
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
