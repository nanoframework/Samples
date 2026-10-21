//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.UI;

namespace WaveshareEsp32S3TouchLcd2
{
    public static class St7789Driver
    {
        public static GraphicDriver GetDriver()
        {
            return new GraphicDriver
            {
                MemoryWrite = 0x2C,
                SetColumnAddress = 0x2A,
                SetRowAddress = 0x2B,
                SetWindowType = SetWindowType.X16bitsY16Bit,
                DefaultOrientation = DisplayOrientation.Portrait,
                InitializationSequence = new byte[]
                {
                    // Software reset is required because there is no GPIO reset line.
                    (byte)GraphicDriverCommandType.Command, 1, 0x01,
                    // Generic driver delays are in units of 10 ms.
                    (byte)GraphicDriverCommandType.Sleep, 15,
                    (byte)GraphicDriverCommandType.Command, 1, 0x11,
                    (byte)GraphicDriverCommandType.Sleep, 12,
                    // COLMOD: RGB565; INVON: the board uses an IPS panel.
                    (byte)GraphicDriverCommandType.Command, 2, 0x3A, 0x55,
                    (byte)GraphicDriverCommandType.Command, 1, 0x21,
                    (byte)GraphicDriverCommandType.Command, 1, 0x29,
                    (byte)GraphicDriverCommandType.Sleep, 2,
                },
                OrientationPortrait = new byte[]
                {
                    // No axis swap or mirroring; BGR matches DisplayControl.Write's pixel format.
                    (byte)GraphicDriverCommandType.Command, 2, 0x36, 0x08,
                },
                PowerModeNormal = new byte[]
                {
                    (byte)GraphicDriverCommandType.Command, 1, 0x11,
                    (byte)GraphicDriverCommandType.Sleep, 12,
                    (byte)GraphicDriverCommandType.Command, 1, 0x29,
                    (byte)GraphicDriverCommandType.Sleep, 2,
                },
                PowerModeSleep = new byte[]
                {
                    (byte)GraphicDriverCommandType.Command, 1, 0x28,
                    (byte)GraphicDriverCommandType.Command, 1, 0x10,
                    (byte)GraphicDriverCommandType.Sleep, 12,
                },
            };
        }
    }
}
