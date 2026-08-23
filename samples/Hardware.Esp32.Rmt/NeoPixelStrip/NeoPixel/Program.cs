//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.

using nanoFramework.Hardware.Esp32.Rmt;
using System.Threading;

namespace NeoPixel
{
    public class Program
    {
        // GPIO pin for Led strip data connection
        private const int GpioPin = 18;
        // Number of LEDs in strip
        private const int SizeLedStrip = 256;
        // Size of led pattern
        private const int patternSize  = 16;
        // Number of times pattern is repeated in led strip. We output first pattern and this is repeated by nanoFramework driver.
        private const int repeat = SizeLedStrip / patternSize;

        // Colours for pattern
        private static readonly Color RedColor = new Color { R = 255 };
        private static readonly Color GeenColor = new Color { G = 255 };
        private static readonly Color BlueColor = new Color { B = 255 };
        private static readonly Color BlackColor = new Color();

        public static void Main()
        {
            var chain = new NeopixelChain(GpioPin, LedType.WS2812, patternSize, repeat);

            while (true)
            {
                foreach (Color NextColor in new Color[] { RedColor, GeenColor, BlueColor })
                {
                    for (uint i = 0; i < patternSize; i++)
                    {
                        chain[i] = NextColor;
                        chain.Update();

                        // slow it down
                        Thread.Sleep(10);

                        chain[i] = BlackColor;
                    }
                }
            }
        }
    }
}
