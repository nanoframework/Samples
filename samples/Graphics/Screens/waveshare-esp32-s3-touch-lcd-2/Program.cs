//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Device.Gpio;
using System.Drawing;
using System.Threading;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Presentation.Media;
using nanoFramework.UI;

namespace WaveshareEsp32S3TouchLcd2
{
    public class Program
    {
        private const int LcdMosi = 38;
        private const int LcdMiso = 40;
        private const int LcdClock = 39;
        private const int LcdChipSelect = 45;
        private const int LcdDataCommand = 42;
        private const int LcdBacklight = 1;
        private const int ScreenWidth = 240;
        private const int ScreenHeight = 320;
        private const int StripHeight = 8;
        private const int SquareSize = 16;

        private static GpioController s_gpio;

        private static readonly ushort[] s_strip = new ushort[ScreenWidth * StripHeight];

        public static void Main()
        {
            Debug.WriteLine("Waveshare ESP32-S3-Touch-LCD-2: managed ST7789 screen test");

            Configuration.SetPinFunction(LcdMosi, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(LcdClock, DeviceFunction.SPI1_CLOCK);
            // The LCD is write-only; GPIO40 is the board's shared SPI MISO line.
            Configuration.SetPinFunction(LcdMiso, DeviceFunction.SPI1_MISO);
            // The generic SPI driver does not switch the backlight on.
            s_gpio = new GpioController();
            s_gpio.OpenPin(LcdBacklight, PinMode.Output);
            s_gpio.Write(LcdBacklight, PinValue.High);
            Debug.WriteLine("Backlight enabled (GPIO1 HIGH)");

            // Reset is not GPIO-controlled; the backlight is managed above, not by graphics.
            var spiConfig = new SpiConfiguration(1, LcdChipSelect, LcdDataCommand, -1, -1);
            // Generic SPI graphics takes the longer side first, then applies the orientation.
            var screenConfig = new ScreenConfiguration(
                0, 0, ScreenHeight, ScreenWidth, St7789Driver.GetDriver());

            uint bufferSize = DisplayControl.Initialize(spiConfig, screenConfig, 30 * 1024);
            Debug.WriteLine($"Display initialized: {DisplayControl.ScreenWidth}x{DisplayControl.ScreenHeight}, buffer {bufferSize} bytes");
            if (DisplayControl.ScreenWidth != ScreenWidth || DisplayControl.ScreenHeight != ScreenHeight)
            {
                throw new InvalidOperationException("Expected a 240x320 portrait display. Check the Generic SPI graphics firmware.");
            }

            Color[] colors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.White, Color.Black };
            string[] names = new string[] { "red", "green", "blue", "white", "black" };
            for (int i = 0; i < colors.Length; i++)
            {
                Debug.WriteLine($"Full-screen test: {names[i]}");
                FillScreen(colors[i]);
                Thread.Sleep(1000);
            }

            DrawTextTest();
            Debug.WriteLine("Text test displayed for 10 seconds");
            Thread.Sleep(10000);

            DrawTestPattern();
            Debug.WriteLine("Pattern: RGB/YCM bars, gray ramp, white border; corners red/green/blue/yellow.");
            Debug.WriteLine("Animating a white square in the bottom band. Touch is not part of this sample.");
            AnimateSquare();
        }

        private static void FillScreen(Color color)
        {
            ushort pixel = color.ToBgr565();
            for (int i = 0; i < s_strip.Length; i++)
            {
                s_strip[i] = pixel;
            }

            for (int y = 0; y < ScreenHeight; y += StripHeight)
            {
                DisplayControl.Write(0, (ushort)y, ScreenWidth, StripHeight, s_strip);
            }
        }

        private static void DrawTextTest()
        {
            Font font = Resource.GetFont(Resource.FontResources.NinaB);
            if (font == null)
            {
                throw new InvalidOperationException("The embedded NinaB font could not be loaded.");
            }

            string[] lines = new string[]
            {
                "Text rendering test",
                "",
                "The quick brown fox",
                "jumps over the lazy dog.",
                "Pack my box with five",
                "dozen liquor jugs.",
                "",
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                "abcdefghijklmnopqrstuvwxyz",
                "0123456789",
                ".,!? :; '\" () [] {}",
                "+-*/= @#$%& _<>"
            };

            const int margin = 8;
            const int textWidth = ScreenWidth - 2 * margin;
            int lineHeight = font.Height + 4;
            if (lines.Length * lineHeight > ScreenHeight - 2 * margin)
            {
                throw new InvalidOperationException("The text test does not fit the display height.");
            }

            FillScreen(Color.DarkBlue);
            ushort[] pixels = new ushort[textWidth * lineHeight];
            // Render off-screen: the generic SPI driver does not support native direct-text writes.
            using (Bitmap bitmap = new Bitmap(textWidth, lineHeight))
            {
                for (int line = 0; line < lines.Length; line++)
                {
                    font.ComputeExtent(lines[line], out int width, out int height);
                    if (width > textWidth || height > lineHeight - 2)
                    {
                        throw new InvalidOperationException($"Text does not fit: {lines[line]}");
                    }

                    bitmap.FillRectangle(0, 0, textWidth, lineHeight, Color.DarkBlue);
                    bitmap.DrawText(lines[line], font, line == 0 ? Color.Yellow : Color.White, 0, 2);
                    for (int y = 0; y < lineHeight; y++)
                    {
                        for (int x = 0; x < textWidth; x++)
                        {
                            pixels[y * textWidth + x] = bitmap.GetPixel(x, y).ToBgr565();
                        }
                    }

                    DisplayControl.Write(margin, (ushort)(margin + line * lineHeight),
                        textWidth, (ushort)lineHeight, pixels);
                    Debug.WriteLine($"Text: {lines[line]}");
                }
            }
        }
        private static void DrawTestPattern()
        {
            Color[] bars = new Color[]
            {
                Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Cyan, Color.Magenta
            };

            for (int top = 0; top < ScreenHeight; top += StripHeight)
            {
                for (int row = 0; row < StripHeight; row++)
                {
                    int y = top + row;
                    for (int x = 0; x < ScreenWidth; x++)
                    {
                        Color color = Color.Black;
                        if (y < 160)
                        {
                            color = bars[x / (ScreenWidth / bars.Length)];
                        }
                        else if (y < 240)
                        {
                            byte gray = (byte)(x * 255 / (ScreenWidth - 1));
                            color = Color.FromArgb(gray, gray, gray);
                        }

                        if (x < SquareSize && y < SquareSize)
                        {
                            color = Color.Red;
                        }
                        else if (x >= ScreenWidth - SquareSize && y < SquareSize)
                        {
                            color = Color.Green;
                        }
                        else if (x < SquareSize && y >= ScreenHeight - SquareSize)
                        {
                            color = Color.Blue;
                        }
                        else if (x >= ScreenWidth - SquareSize && y >= ScreenHeight - SquareSize)
                        {
                            color = Color.Yellow;
                        }

                        if (x == 0 || x == ScreenWidth - 1 || y == 0 || y == ScreenHeight - 1)
                        {
                            color = Color.White;
                        }

                        s_strip[row * ScreenWidth + x] = color.ToBgr565();
                    }
                }

                DisplayControl.Write(0, (ushort)top, ScreenWidth, StripHeight, s_strip);
            }
        }

        private static void AnimateSquare()
        {
            ushort[] square = new ushort[SquareSize * SquareSize];
            ushort[] erase = new ushort[SquareSize * SquareSize];
            for (int i = 0; i < square.Length; i++)
            {
                square[i] = Color.White.ToBgr565();
            }

            int x = 24;
            int step = 4;
            while (true)
            {
                DisplayControl.Write((ushort)x, 272, SquareSize, SquareSize, square);
                Thread.Sleep(50);
                DisplayControl.Write((ushort)x, 272, SquareSize, SquareSize, erase);

                x += step;
                if (x == 24 || x == ScreenWidth - 24 - SquareSize)
                {
                    step = -step;
                    Debug.WriteLine("Screen test running");
                }
            }
        }
    }
}
