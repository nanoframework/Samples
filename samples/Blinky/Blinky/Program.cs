//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Device.Gpio;
using System;
using System.Threading;

namespace Blinky
{
    public class Program
    {
        private static GpioController s_GpioController;
        public static void Main()
        {
            s_GpioController = new GpioController();

            // pick a board, uncomment one line for GpioPin; default is STM32F769I_DISCO

            // DISCOVERY4: PD15 is LED6 
            //GpioPin led = s_GpioController.OpenPin(PinNumber('D', 15), PinMode.Output);

            // ESP32 DevKit: 4 is a valid GPIO pin in, some boards like Xiuxin ESP32 may require GPIO Pin 2 instead.
            //GpioPin led = s_GpioController.OpenPin(4, PinMode.Output);

            // FEATHER S2: 
            //GpioPin led = s_GpioController.OpenPin(13, PinMode.Output);

            // F429I_DISCO: PG14 is LEDLD4 
            //GpioPin led = s_GpioController.OpenPin(PinNumber('G', 14), PinMode.Output);

            // NETDUINO 3 Wifi: A10 is LED onboard blue
            //GpioPin led = s_GpioController.OpenPin(PinNumber('A', 10), PinMode.Output);

            // QUAIL: PE15 is LED1  
            //GpioPin led = s_GpioController.OpenPin(PinNumber('E', 15), PinMode.Output);

            // STM32F091RC: PA5 is LED_GREEN
            //GpioPin led = s_GpioController.OpenPin(PinNumber('A', 5), PinMode.Output);

            // STM32F746_NUCLEO: PB75 is LED2
            //GpioPin led = s_GpioController.OpenPin(PinNumber('B', 7), PinMode.Output);

            //STM32F769I_DISCO: PJ5 is LD2
            GpioPin led = s_GpioController.OpenPin(PinNumber('J', 5), PinMode.Output);

            // ST_B_L475E_IOT01A: PB14 is LD2
            //GpioPin led = s_GpioController.OpenPin(PinNumber('B', 14), PinMode.Output);

            // STM32L072Z_LRWAN1: PA5 is LD2
            //GpioPin led = s_GpioController.OpenPin(PinNumber('A', 5), PinMode.Output);

            // TI CC13x2 Launchpad: DIO_07 it's the green LED
            //GpioPin led = s_GpioController.OpenPin(7, PinMode.Output);

            // TI CC13x2 Launchpad: DIO_06 it's the red LED  
            //GpioPin led = s_GpioController.OpenPin(6, PinMode.Output);

            // ULX3S FPGA board: for the red D22 LED from the ESP32-WROOM32, GPIO5
            //GpioPin led = s_GpioController.OpenPin(5, PinMode.Output);

            // Silabs SLSTK3701A: LED1 PH14 is LLED1
            //GpioPin led = s_GpioController.OpenPin(PinNumber('H', 14), PinMode.Output);


            led.Write(PinValue.Low);

            while (true)
            {
                led.Toggle();
                Thread.Sleep(125);
                led.Toggle();
                Thread.Sleep(125);
                led.Toggle();
                Thread.Sleep(125);
                led.Toggle();
                Thread.Sleep(525);
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
