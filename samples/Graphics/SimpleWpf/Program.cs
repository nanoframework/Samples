// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// !!!----------- SAMPLE - ENSURE YOU CHOOSE THE CORRECT TARGET HERE --------------!!!
#define STM32F769I_DISCO
// !!!-----------------------------------------------------------------------------!!!

using nanoFramework.Presentation;
using nanoFramework.UI;
using nanoFramework.UI.Input;
using SimpleWPF;
using System;
using System.Threading;

namespace SimpleWpf
{
    /// <summary>
    /// Demonstrates the Windows Presentation Foundation functionality in .NET 
    /// NanoFramework.
    /// </summary>
    public class MySimpleWPFApplication : Application
    {
        // Fields to hold the fonts used by this demo.   
        static public Font NinaBFont;
        static public Font SmallFont;
        static public Bitmap Leaf;

        // Declare the main window.
        static Window mainWindow;
        
        /// <summary>
        /// The executable entry point.
        /// </summary>
        public static void Main()
        {

            // Create the object that handles the buttons presses on GPIO pins.
            GPIOButtonInputProvider inputProvider = new GPIOButtonInputProvider(null);

#if ESP32   // This is an example mapping, work them out for your needs!
            int backLightPin = 32;
            int chipSelect = 14;
            int dataCommand = 27;
            int reset = 33;
            // Add the nanoFramework.Hardware.Esp32 to the solution
            Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);
            // Adjust as well the size of your screen and the position of the screen on the driver
            DisplayControl.Initialize(new SpiConfiguration(1, chipSelect, dataCommand, reset, backLightPin), new ScreenConfiguration(0, 0, 320, 240));
            // Depending on you ESP32, you may also have to use either PWM either GPIO to set the backlight pin mode on
            // GpioController().OpenPin(backLightPin, PinMode.Output);
            // GpioController().Write(backLightPin, PinValue.High);
            // Assign GPIO / Key functions to GPIOButtonInputProvider
            // Esp32
            inputProvider.AddButton(12, Button.VK_LEFT, true);
            inputProvider.AddButton(13, Button.VK_RIGHT, true);
            inputProvider.AddButton(34, Button.VK_UP, true);
            inputProvider.AddButton(35, Button.VK_SELECT, true);
            inputProvider.AddButton(36, Button.VK_DOWN, true);
#elif STM32F769I_DISCO // This is an example (working) button map, work the actual pins out for your need!
            //WARNING: Invalid pin mappings will never be returned, and may need you to reflash the device!
            inputProvider.AddButton(PinNumber('J', 0), Button.VK_LEFT, true);
            inputProvider.AddButton(PinNumber('J', 1), Button.VK_RIGHT, true);
            inputProvider.AddButton(PinNumber('J', 3), Button.VK_UP, true);
            inputProvider.AddButton(PinNumber('J', 4), Button.VK_DOWN, true);
            inputProvider.AddButton(PinNumber('A', 6), Button.VK_SELECT, true);

            DisplayControl.Initialize(new SpiConfiguration(), new ScreenConfiguration());
#else
            throw new System.Exception("Unknown button and/or display mapping!");
#endif

            // Create application object
            MySimpleWPFApplication myApplication = new MySimpleWPFApplication();

            // Add main window to application
            mainWindow = new MainMenuWindow(myApplication);

            // Load the fonts resources used by demo.
            NinaBFont = Resource.GetFont(Resource.FontResources.NinaB);
            SmallFont = Resource.GetFont(Resource.FontResources.small);

            // Start the application running.
            myApplication.Run(mainWindow);
        }

        /// <summary>
        /// Sets focus on the main window.
        /// </summary>
        public void GoHome()
        {
            Buttons.Focus(MySimpleWPFApplication.mainWindow);
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
