// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// !!!----------- SAMPLE - ENSURE YOU CHOOSE THE CORRECT TARGET HERE --------------!!!
//#define STM32F769I_DISCO // Comment this in if for the target!
#define ESP32 // Comment this in if for the target platform!
// !!!-----------------------------------------------------------------------------!!!

using System.Threading;
using nanoFramework.Hardware.Esp32;
using nanoFramework.UI;
using Primitives.SimplePrimitives;

namespace Primitives
{
    public class Program
    {
        public static void Main()
        {
            int delayBetween = 3000;

#if ESP32   // This is an example mapping, work them out for your needs!
            int backLightPin = 5;
            int chipSelect = 22;
            int dataCommand = 21;
            int reset = 18;

            Configuration.SetPinFunction(25, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(19, DeviceFunction.SPI1_CLOCK);

            // Adjust as well the size of your screen and the position of the screen on the driver
            DisplayControl.Initialize(new SpiConfiguration(1, chipSelect, dataCommand, reset, backLightPin), new ScreenConfiguration(0, 0, 320, 240), 2 *1024 * 1024);

            // Depending on you ESP32, you may also have to use either PWM either GPIO to set the backlight pin mode on
            // new GpioController().OpenPin(backLightPin, PinMode.Output);
            // GpioController().Write(backLightPin, PinValue.High);

#elif STM32F769I_DISCO // This is an example (working) button map, work the actual pins out for your need!
            //WARNING: Invalid pin mappings will never be returned, and may need you to reflash the device!
            DisplayControl.Initialize(new SpiConfiguration(), new ScreenConfiguration());
#else
            throw new System.Exception("Unknown display mapping!");
#endif

            //DisplayControl.ChangeOrientation(DisplayOrientation.Portrait180);
            // Get full screen bitmap from displayControl to draw on.
            Bitmap fullScreenBitmap = DisplayControl.FullScreen;  

            fullScreenBitmap.Clear();

            Font DisplayFont = Resource.GetFont(Resource.FontResources.SegoeUIRegular12);

            while (true)
            {
                WritePoint wrtPoint = new WritePoint();
                Thread.Sleep(delayBetween);

                RandomDrawLine rdlt = new RandomDrawLine(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                RotateImage ri = new RotateImage(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                ColourGradient colourGradient = new ColourGradient(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                Colours ColourExample = new Colours(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                PagedText pt = new PagedText(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                BouncingBalls bb = new BouncingBalls(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                TileImage ti = new TileImage(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                StretchImage si = new StretchImage(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                SetPixels sp = new SetPixels(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                FontExamples fe = new FontExamples(fullScreenBitmap);
                Thread.Sleep(delayBetween);

                RandomRectangles rr = new RandomRectangles(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                SliceScaling9 ss = new SliceScaling9(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                RandomClipping rc = new RandomClipping(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                MatrixRain mr = new MatrixRain(fullScreenBitmap);
                Thread.Sleep(delayBetween * 10);
                mr.Stop();
            }
        }
    }
}
