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

namespace GenericDriver
{
    /// <summary>
    /// You **MUST** have a build of nanoFramework with a generic graphic driver
    /// </summary>
    public class Program
    {
        private const int ChipSelect = 5;
        private const int DataCommand = 23;
        private const int Reset = 18;

        // Raw registers from the ST7735 datasheet
        private enum St7735Reg
        {
            NOP = 0x00,
            SOFTWARE_RESET = 0x01,
            POWER_STATE = 0x10,
            Sleep_Out = 0x11,
            Invertion_Off = 0x20,
            Invertion_On = 0x21,
            Gamma_Set = 0x26,
            Display_OFF = 0x28,
            Display_ON = 0x29,
            Column_Address_Set = 0x2A,
            Page_Address_Set = 0x2B,
            Memory_Write = 0x2C,
            Colour_Set = 0x2D,
            Memory_Read = 0x2E,
            Partial_Area = 0x30,
            Memory_Access_Control = 0x36,
            Pixel_Format_Set = 0x3A,
            Memory_Write_Continue = 0x3C,
            Write_Display_Brightness = 0x51,
            Frame_Rate_Control_Normal = 0xB1,
            Frame_Rate_Control_2 = 0xB2,
            Frame_Rate_Control_3 = 0xB3,
            Invert_On = 0xB4,
            Display_Function_Control = 0xB6,
            Entry_Mode_Set = 0xB7,
            Power_Control_1 = 0xC0,
            Power_Control_2 = 0xC1,
            Power_Control_3 = 0xC2,
            Power_Control_4 = 0xC3,
            Power_Control_5 = 0xC4,
            VCOM_Control_1 = 0xC5,
            VCOM_Control_2 = 0xC7,
            Power_Control_A = 0xCB,
            Power_Control_B = 0xCF,
            Positive_Gamma_Correction = 0xE0,
            Negative_Gamma_Correction = 0XE1,
            Driver_Timing_Control_A = 0xE8,
            Driver_Timing_Control_B = 0xEA,
            Power_On_Sequence = 0xED,
            Enable_3G = 0xF2,
            Pump_Ratio_Control = 0xF7,
            Power_Control_6 = 0xFC,
        };

        [Flags]
        private enum St7735Orientation
        {
            MADCTL_MH = 0x04, // sets the Horizontal Refresh, 0=Left-Right and 1=Right-Left
            MADCTL_ML = 0x10, // sets the Vertical Refresh, 0=Top-Bottom and 1=Bottom-Top
            MADCTL_MV = 0x20, // sets the Row/Column Swap, 0=Normal and 1=Swapped
            MADCTL_MX = 0x40, // sets the Column Order, 0=Left-Right and 1=Right-Left
            MADCTL_MY = 0x80, // sets the Row Order, 0=Top-Bottom and 1=Bottom-Top

            MADCTL_BGR = 0x08 // Blue-Green-Red pixel order
        };

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

            GraphicDriver graphicDriver = new GraphicDriver()
            {
                MemoryWrite = (byte)St7735Reg.Memory_Write,
                SetColumnAddress = (byte)St7735Reg.Column_Address_Set,
                SetRowAddress = (byte)St7735Reg.Page_Address_Set,
                InitializationSequence = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 1, (byte)St7735Reg.SOFTWARE_RESET,
                            // Sleep for 50 ms
                            (byte)GraphicDriverCommandType.Sleep, 5,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)St7735Reg.Sleep_Out,
                            // Sleep for 500 ms
                            (byte)GraphicDriverCommandType.Sleep, 50,
                            (byte)GraphicDriverCommandType.Command, 4, (byte)St7735Reg.Frame_Rate_Control_Normal, 0x01, 0x2C, 0x2D,
                            (byte)GraphicDriverCommandType.Command, 4, (byte)St7735Reg.Frame_Rate_Control_2, 0x01, 0x2C, 0x2D,
                            (byte)GraphicDriverCommandType.Command, 7, (byte)St7735Reg.Frame_Rate_Control_3, 0x01, 0x2C, 0x2D, 0x01, 0x2C, 0x2D,
                            (byte)GraphicDriverCommandType.Command, 2, (byte)St7735Reg.Invert_On, 0x07,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)St7735Reg.Invertion_On,
                            // 0x55 -> 16 bit
                            (byte)GraphicDriverCommandType.Command, 2, (byte)St7735Reg.Pixel_Format_Set, 0x55,
                            (byte)GraphicDriverCommandType.Command, 4, (byte)St7735Reg.Power_Control_1, 0xA2, 0x02, 0x84,
                            (byte)GraphicDriverCommandType.Command, 2, (byte)St7735Reg.Power_Control_2, 0xC5,
                            (byte)GraphicDriverCommandType.Command, 3, (byte)St7735Reg.Power_Control_3, 0x0A, 0x00,
                            (byte)GraphicDriverCommandType.Command, 3, (byte)St7735Reg.Power_Control_4, 0x8A, 0x2A,
                            (byte)GraphicDriverCommandType.Command, 3, (byte)St7735Reg.Power_Control_5, 0x8A, 0xEE,
                            (byte)GraphicDriverCommandType.Command, 4, (byte)St7735Reg.VCOM_Control_1, 0x0E, 0xFF, 0xFF,
                            (byte)GraphicDriverCommandType.Command, 17, (byte)St7735Reg.Positive_Gamma_Correction, 0x02, 0x1c, 0x7, 0x12, 0x37, 0x32, 0x29, 0x2d, 0x29, 0x25, 0x2B, 0x39, 0x00, 0x01, 0x03, 0x10,
                            (byte)GraphicDriverCommandType.Command, 17, (byte)St7735Reg.Negative_Gamma_Correction, 0x03, 0x1d, 0x07, 0x06, 0x2E, 0x2C, 0x29, 0x2D, 0x2E, 0x2E, 0x37, 0x3F, 0x00, 0x00, 0x02, 0x1,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)St7735Reg.Sleep_Out,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)St7735Reg.Display_ON,
                            // Sleep 100 ms
                            (byte)GraphicDriverCommandType.Sleep, 10,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)St7735Reg.NOP,
                            // Sleep 20 ms
                            (byte)GraphicDriverCommandType.Sleep, 2,
                        },
                OrientationLandscape = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)St7735Reg.Memory_Access_Control, (byte)(St7735Orientation.MADCTL_MY | St7735Orientation.MADCTL_MX | St7735Orientation.MADCTL_BGR),
                        },
                PowerModeNormal = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 3, (byte)St7735Reg.POWER_STATE, 0x00, 0x00,
                        },
                PowerModeSleep = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 3, (byte)St7735Reg.POWER_STATE, 0x00, 0x01,
                        },
                DefaultOrientation = DisplayOrientation.Landscape,
                Brightness = (byte)St7735Reg.Write_Display_Brightness,
                SetWindowType = SetWindowType.X16bitsY16Bit,
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

            Debug.WriteLine($"init screen initialized");

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
