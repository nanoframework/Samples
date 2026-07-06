//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Drawing;
using System.Diagnostics;
using System.Threading;
using Iot.Device.Button;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Presentation.Media;
using nanoFramework.UI;

namespace Esp32S3BoxLite
{
    /// <summary>
    /// Sample showing how to drive the screen of the Espressif ESP32-S3-BOX-Lite
    /// using a managed graphic driver (generic SPI driver).
    ///
    /// The ESP32-S3-BOX-Lite ships with a 2.4" 320x240 LCD driven by a ST7789(V)
    /// controller over SPI. The pin mapping below comes from the official
    /// Espressif Board Support Package (esp-bsp / esp-box-lite):
    ///
    ///   LCD MOSI (DATA0)  -> GPIO6
    ///   LCD CLK  (PCLK)   -> GPIO7
    ///   LCD CS            -> GPIO5
    ///   LCD DC            -> GPIO4
    ///   LCD RST           -> GPIO48
    ///   LCD Backlight     -> GPIO45
    ///
    /// Sensor note: unlike the full ESP32-S3-BOX (which carries an ICM-42607
    /// IMU), the ESP32-S3-BOX-Lite has NO motion/environmental sensor and NO
    /// touch controller (BSP_CAPS_IMU = 0, BSP_CAPS_TOUCH = 0). Its only onboard
    /// "sensors" are the dual digital microphones fed through the ES7243E audio
    /// ADC, controlled over I2C (SCL = GPIO18, SDA = GPIO8). This sample therefore
    /// focuses on the screen driver. See the README for details.
    ///
    /// IMPORTANT: You **MUST** run a .NET nanoFramework image that is built with
    /// the Generic Graphic Driver for a managed driver to have any effect.
    /// </summary>
    public class Program
    {
        // ESP32-S3-BOX-Lite display pins (from the Espressif esp-box-lite BSP).
        private const int LcdMosi = 6;
        private const int LcdClock = 7;
        private const int LcdChipSelect = 5;
        private const int LcdDataCommand = 4;
        private const int LcdReset = 48;
        private const int LcdBacklight = 45;

        // The ST7789 on the BOX-Lite is a 320x240 panel used in landscape.
        private const int ScreenWidth = 320;
        private const int ScreenHeight = 240;

        // Area (in the free bottom-center band) where the pressed button name is shown.
        private const int StatusX = 70;
        private const int StatusY = 205;
        private const int StatusWidth = 180;
        private const int StatusHeight = 26;

        private static Font _font;
        private static Bitmap _statusBitmap;

        public static void Main()
        {
            Debug.WriteLine("Starting ESP32-S3-BOX-Lite screen sample (ST7789)");

            // On the ESP32-S3 the SPI pins must be routed explicitly.
            // MISO is not connected to the panel but a valid pin must still be set.
            Configuration.SetPinFunction(LcdMosi, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(LcdClock, DeviceFunction.SPI1_CLOCK);
            Configuration.SetPinFunction(39, DeviceFunction.SPI1_MISO);

            // Drive the backlight through PWM so brightness can be controlled.
            Configuration.SetPinFunction(LcdBacklight, DeviceFunction.PWM1);

            var spiConfig = new SpiConfiguration(
                1,
                LcdChipSelect,
                LcdDataCommand,
                LcdReset,
                LcdBacklight);

            // The panel matches the driver size, so it starts at the (0,0) origin.
            var screenConfig = new ScreenConfiguration(
                0,
                0,
                ScreenWidth,
                ScreenHeight,
                St7789BoxLiteDriver.GetDriver());

            uint bufferSize = DisplayControl.Initialize(spiConfig, screenConfig, 30 * 1024);
            Debug.WriteLine($"Screen initialized, maximum buffer size: {bufferSize} bytes");

            // Paint the screen to prove the driver is working.
            FillScreen(Color.DarkBlue);
            DrawCornerMarkers();

            Debug.WriteLine("Done drawing on the ESP32-S3-BOX-Lite screen");

            // Load the font used to write text on screen.
            _font = Resource.GetFont(Resource.FontResources.segoeuiregular12);
            Debug.WriteLine(_font == null ? "Font FAILED to load" : "Font loaded");

            // Draw a startup message on the main thread to verify text rendering
            // works independently of the button events.
            ShowText("Press a button");

            // The three navigation buttons (Previous / Enter / Next) are an ADC
            // resistor-ladder on GPIO1, exposed here as Button objects.
            SetupButtons();

            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        /// Wires up the three navigation buttons and logs their events.
        /// </summary>
        private static void SetupButtons()
        {
            BoxLiteButtons buttons = new BoxLiteButtons();

            RegisterButton(buttons.Previous, "Previous");
            RegisterButton(buttons.Enter, "Enter");
            RegisterButton(buttons.Next, "Next");

            Debug.WriteLine("Buttons ready (Previous / Enter / Next)");
        }

        private static void RegisterButton(AdcButton button, string name)
        {
            button.IsHoldingEnabled = true;
            button.IsDoublePressEnabled = true;

            button.Press += (sender, e) =>
            {
                Debug.WriteLine($"{name}: press");
                ShowText(name);
            };
            button.DoublePress += (sender, e) =>
            {
                Debug.WriteLine($"{name}: double press");
                ShowText($"{name} x2");
            };
            button.Holding += (sender, e) =>
            {
                if (e.HoldingState == ButtonHoldingState.Started)
                {
                    Debug.WriteLine($"{name}: holding");
                    ShowText($"{name} (hold)");
                }
            };
        }

        /// <summary>
        /// Writes text in the status area. The text is rendered into an off-screen
        /// bitmap (pure managed memory, so it is independent of the display driver)
        /// and the resulting pixels are pushed with the direct write path, which is
        /// the one supported by the generic SPI driver.
        /// </summary>
        private static void ShowText(string text)
        {
            if (_font == null)
            {
                Debug.WriteLine("Cannot draw text: font not loaded");
                return;
            }

            if (_statusBitmap == null)
            {
                _statusBitmap = new Bitmap(StatusWidth, StatusHeight);
            }

            // Render onto the off-screen bitmap.
            _statusBitmap.FillRectangle(0, 0, StatusWidth, StatusHeight, Color.DarkBlue);
            _statusBitmap.DrawText(text, _font, Color.White, 2, 4);

            // Copy the rendered pixels into a BGR565 buffer and push them.
            ushort[] buffer = new ushort[StatusWidth * StatusHeight];
            int i = 0;
            for (int y = 0; y < StatusHeight; y++)
            {
                for (int x = 0; x < StatusWidth; x++)
                {
                    buffer[i++] = _statusBitmap.GetPixel(x, y).ToBgr565();
                }
            }

            DisplayControl.Write(StatusX, StatusY, StatusWidth, StatusHeight, buffer);
        }

        /// <summary>
        /// Fills the whole screen with a solid color writing horizontal strips
        /// so the working buffer stays small.
        /// </summary>
        private static void FillScreen(Color color)
        {
            const int stripHeight = 8;
            ushort encoded = color.ToBgr565();

            ushort[] strip = new ushort[ScreenWidth * stripHeight];
            for (int i = 0; i < strip.Length; i++)
            {
                strip[i] = encoded;
            }

            for (int y = 0; y < ScreenHeight; y += stripHeight)
            {
                DisplayControl.Write(0, (ushort)y, ScreenWidth, stripHeight, strip);
            }
        }

        /// <summary>
        /// Draws a colored square in each corner and one in the center.
        /// </summary>
        private static void DrawCornerMarkers()
        {
            const int size = 60;

            DrawRectangle(0, 0, size, size, Color.Blue);
            DrawRectangle(ScreenWidth - size, 0, size, size, Color.Red);
            DrawRectangle(0, ScreenHeight - size, size, size, Color.Green);
            DrawRectangle(ScreenWidth - size, ScreenHeight - size, size, size, Color.White);
            DrawRectangle((ScreenWidth - size) / 2, (ScreenHeight - size) / 2, size, size, Color.Yellow);
        }

        private static void DrawRectangle(int x, int y, int width, int height, Color color)
        {
            ushort encoded = color.ToBgr565();

            ushort[] buffer = new ushort[width * height];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = encoded;
            }

            DisplayControl.Write((ushort)x, (ushort)y, (ushort)width, (ushort)height, buffer);
        }
    }
}
