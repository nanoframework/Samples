using nanoFramework.Hardware.Esp32;
using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Pwm;

namespace m5stack.screen
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            // Screen init 
            int chipSelect;
            int dataCommand;
            int reset;
            int backLightPin;

            const bool wroover = false;

            // Text to display and location on screen
            const int screenWidth = 320;
            const int screenHeight = 240;
            const int screenBufferSize = 30 * 1024;

            const int textPosX = 10;
            const int textPosY = 0;
            const string text = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry. 
                                    Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. 
                                    It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. 
                                    It was popularised in the 1960s with the release of Letraset sheets containing 
                                    Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

            if (wroover)
            {
                backLightPin = 5;
                chipSelect = 22;
                dataCommand = 21;
                reset = 18;
            }
            else
            {
                backLightPin = 32;
                chipSelect = 14;
                dataCommand = 27;
                reset = 33;
                Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);
                Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
                Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);
            }

            Configuration.SetPinFunction(backLightPin, DeviceFunction.PWM1);
            DisplayControl.Initialize(new SpiConfiguration(1, chipSelect, dataCommand, reset, backLightPin), new ScreenConfiguration(0, 0, screenWidth, screenHeight), screenBufferSize);
            Debug.WriteLine("Screen initialized");

            PwmController pwm = PwmController.GetDefault();
            pwm.SetDesiredFrequency(44100);
            PwmPin pwmPin = pwm.OpenPin(backLightPin);
            pwmPin.SetActiveDutyCyclePercentage(0.1);
            pwmPin.Start();

            Font DisplayFont = Resource.GetFont(Resource.FontResources.segoeuiregular12);
            Bitmap charBitmap = new Bitmap(DisplayFont.MaxWidth + 1, DisplayFont.Height);

            DisplayControl.Write(text, textPosX, textPosY, screenWidth, screenHeight, DisplayFont, Color.DarkBlue, Color.White);
            
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
