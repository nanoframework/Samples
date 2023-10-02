//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Presentation.Media;
using nanoFramework.Runtime.Native;
using nanoFramework.UI;
using nanoFramework.UI.GraphicDrivers;

namespace UsingGenericDriver
{
    /// <summary>
    /// You **MUST** have a build of nanoFramework with a generic graphic driver
    /// </summary>
    public class Program
    {
        private const int ChipSelect = 5;
        private const int DataCommand = 23;
        private const int Reset = 18;

        public static void Main()
        {
            Debug.WriteLine("Hello from and generic grahic drivers!");

            // You **MUST** have a build of nanoFramework with a generic graphic driver

            // If you're using an ESP32, don't forget to set the pins for the screen
            // Set the pins for the screen
            Configuration.SetPinFunction(15, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(13, DeviceFunction.SPI1_CLOCK);
            // This is not used but must be defined
            Configuration.SetPinFunction(4, DeviceFunction.SPI1_MISO);

            var displaySpiConfig = new SpiConfiguration(
                1,
                ChipSelect,
                DataCommand,
                Reset,
                -1);

            // Get the generic driver
            GraphicDriver graphicDriver = St7735.GraphicDriver;

            // As optional, you can adjust anything
            graphicDriver.OrientationPortrait = new byte[]
            {
                (byte)GraphicDriverCommandType.Command, 2, 0x36, 0x88,
            };

            var screenConfig = new ScreenConfiguration(
                26,
                1,
                80,
                160,
                graphicDriver);

            var init = DisplayControl.Initialize(
                displaySpiConfig,
                screenConfig,
            1024);

            Debug.WriteLine($"Screen initialized");          

            ushort[] toSend = new ushort[100];
            var blue = Color.Blue.ToBgr565();
            var red = Color.Red.ToBgr565();
            var green = Color.Green.ToBgr565();
            var white = Color.White.ToBgr565();

            for (int i = 0; i < toSend.Length; i++)
            {
                toSend[i] = blue;
            }

            DisplayControl.Write(0, 0, 10, 10, toSend);

            for (int i = 0; i < toSend.Length; i++)
            {
                toSend[i] = red;
            }

            DisplayControl.Write(69, 0, 10, 10, toSend);

            for (int i = 0; i < toSend.Length; i++)
            {
                toSend[i] = green;
            }

            DisplayControl.Write(0, 149, 10, 10, toSend);

            for (int i = 0; i < toSend.Length; i++)
            {
                toSend[i] = white;
            }

            DisplayControl.Write(69, 149, 10, 10, toSend);


            Thread.Sleep(Timeout.Infinite);
        }
    }
}
