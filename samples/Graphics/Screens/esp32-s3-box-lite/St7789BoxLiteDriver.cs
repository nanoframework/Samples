//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.UI;

namespace Esp32S3BoxLite
{
    /// <summary>
    /// Managed generic driver for the ST7789(V) display controller used on the
    /// Espressif ESP32-S3-BOX-Lite (2.4" 320x240 LCD).
    ///
    /// The registers and initialization sequence match the datasheet and the
    /// driver shipped in the nanoFramework.Graphics.St7789 NuGet package. The
    /// orientation is tuned for the ESP32-S3-BOX-Lite, matching the esp-box-lite
    /// BSP (swap_xy = true, mirror_x = false, mirror_y = true, invert_color = true).
    /// </summary>
    public static class St7789BoxLiteDriver
    {
        /// <summary>
        /// Builds the <see cref="GraphicDriver"/> for the ESP32-S3-BOX-Lite panel.
        /// </summary>
        public static GraphicDriver GetDriver()
        {
            return new GraphicDriver()
            {
                MemoryWrite = (byte)St7789Reg.Memory_Write,
                SetColumnAddress = (byte)St7789Reg.Column_Address_Set,
                SetRowAddress = (byte)St7789Reg.Row_Address_Set,
                InitializationSequence = new byte[]
                {
                    (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Reg.Software_Reset,
                    // Sleep 50 ms
                    (byte)GraphicDriverCommandType.Sleep, 5,
                    // 0x55 -> 16 bit color (RGB565)
                    (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Reg.Pixel_Format_Set, 0x55,
                    (byte)GraphicDriverCommandType.Command, 6, (byte)St7789Reg.Porch_Setting, 0x0C, 0x0C, 0x00, 0x33, 0x33,
                    (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Reg.Gate_Control, 0x35,
                    (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Reg.VCOMS_Setting, 0x2B,
                    (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Reg.LCM_Control, 0x2C,
                    (byte)GraphicDriverCommandType.Command, 3, (byte)St7789Reg.VDV_VRH_Command_Enable, 0x01, 0xFF,
                    (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Reg.VRH_Set, 0x11,
                    (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Reg.VDV_Set, 0x20,
                    (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Reg.Frame_Rate_Control, 0x0F,
                    (byte)GraphicDriverCommandType.Command, 3, (byte)St7789Reg.Power_Control_1, 0xA4, 0xA1,
                    (byte)GraphicDriverCommandType.Command, 15, (byte)St7789Reg.Positive_Voltage_Gamma, 0xD0, 0x00, 0x05, 0x0E, 0x15, 0x0D, 0x37, 0x43, 0x47, 0x09, 0x15, 0x12, 0x16, 0x19,
                    (byte)GraphicDriverCommandType.Command, 15, (byte)St7789Reg.Negative_Voltage_Gamma, 0xD0, 0x00, 0x05, 0x0D, 0x0C, 0x06, 0x2D, 0x44, 0x40, 0x0E, 0x1C, 0x18, 0x16, 0x19,
                    // The BOX-Lite panel needs color inversion turned on.
                    (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Reg.Invertion_On,
                    (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Reg.Sleep_Out,
                    // Sleep 20 ms
                    (byte)GraphicDriverCommandType.Sleep, 2,
                    (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Reg.Display_On,
                    // Sleep 20 ms
                    (byte)GraphicDriverCommandType.Sleep, 2,
                    (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Reg.NOP,
                    // Sleep 20 ms
                    (byte)GraphicDriverCommandType.Sleep, 2,
                },
                OrientationLandscape = new byte[]
                {
                    // The ESP32-S3-BOX-Lite panel is used in landscape with the
                    // row/column axes swapped and mirrored on Y. This matches the
                    // esp-box-lite BSP (swap_xy = true, mirror_x = false, mirror_y = true).
                    // MADCTL_MV performs the swap; without it the output is sheared
                    // (the "off by a rotation" artifact).
                    (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Reg.Memory_Access_Control, (byte)(St7789Orientation.MADCTL_MV | St7789Orientation.MADCTL_MY | St7789Orientation.MADCTL_BGR),
                },
                PowerModeNormal = new byte[]
                {
                    // SLPOUT (0x11) wakes the panel. Single-byte command, no parameters.
                    (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Reg.Sleep_Out,
                },
                PowerModeSleep = new byte[]
                {
                    // SLPIN (0x10) puts the panel to sleep. Single-byte command, no parameters.
                    (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Reg.Sleep_In,
                },
                DefaultOrientation = DisplayOrientation.Landscape,
                Brightness = (byte)St7789Reg.Write_Display_Brightness,
                SetWindowType = SetWindowType.X16bitsY16Bit,
            };
        }

        // Registers are kept matching the native/managed ST7789 driver naming.
        private enum St7789Reg
        {
            NOP = 0x00,
            Software_Reset = 0x01,
            Sleep_In = 0x10,
            Sleep_Out = 0x11,
            Invertion_Off = 0x20,
            Invertion_On = 0x21,
            Display_Off = 0x28,
            Display_On = 0x29,
            Column_Address_Set = 0x2A,
            Row_Address_Set = 0x2B,
            Memory_Write = 0x2C,
            Memory_Read = 0x2E,
            Partial_Area = 0x30,
            Memory_Access_Control = 0x36,
            Pixel_Format_Set = 0x3A,
            Memory_Write_Continue = 0x3C,
            Write_Display_Brightness = 0x51,
            Porch_Setting = 0xB2,
            Gate_Control = 0xB7,
            VCOMS_Setting = 0xBB,
            LCM_Control = 0xC0,
            VDV_VRH_Command_Enable = 0xC2,
            VRH_Set = 0xC3,
            VDV_Set = 0xC4,
            Frame_Rate_Control = 0xC6,
            Power_Control_1 = 0xD0,
            Positive_Voltage_Gamma = 0xE0,
            Negative_Voltage_Gamma = 0xE1,
        }

        [System.Flags]
        private enum St7789Orientation
        {
            MADCTL_MH = 0x04, // Horizontal Refresh, 0=Left-Right and 1=Right-Left
            MADCTL_ML = 0x10, // Vertical Refresh, 0=Top-Bottom and 1=Bottom-Top
            MADCTL_MV = 0x20, // Row/Column Swap, 0=Normal and 1=Swapped
            MADCTL_MX = 0x40, // Column Order, 0=Left-Right and 1=Right-Left
            MADCTL_MY = 0x80, // Row Order, 0=Top-Bottom and 1=Bottom-Top

            MADCTL_BGR = 0x08 // Blue-Green-Red pixel order
        }
    }
}
