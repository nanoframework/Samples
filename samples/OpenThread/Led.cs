using System;
using System.Drawing;
using System.Threading;
using nanoFramework.Runtime.Native;
using CCSWE.nanoFramework.NeoPixel;
using CCSWE.nanoFramework.NeoPixel.Drivers;
using nanoFramework.Networking.Thread;

namespace Samples
{
    public class Led
    {
        private NeoPixelStrip _led;
        private ThreadDeviceRole _role;

        /// <summary>
        /// Open _led for inbuilt Neopixel
        /// </summary>
        public Led()
        {
            if (SystemInfo.TargetName.Contains("ESP32_H2") || SystemInfo.TargetName.Contains("ESP32_C6"))
            {
                var driver = new Ws2812B(CCSWE.nanoFramework.NeoPixel.ColorOrder.GRB);
                _led = new NeoPixelStrip(8, 1, driver);
            }
            else
            {
                _led = null;
            }
        }

        public void SetRxTX()
        {
            Set(_role, 0.2d);
            Thread.Sleep(50);
            Set(_role, 0.1d);
        }

        public void Set(ThreadDeviceRole role)
        {
            Set(role, 0.1d);
        }

        private void Set(ThreadDeviceRole role, double brightness)
        {
            Color col;

            if (_led == null)
            {
                return;
            }

            // Save it for RXTX
            _role = role;

            switch (role)
            {
                case ThreadDeviceRole.Detached:
                    col = Color.White;
                    break;

                case ThreadDeviceRole.Child:
                    col = Color.Green;
                    break;

                case ThreadDeviceRole.Router:
                    col = Color.Blue;
                    break;

                case ThreadDeviceRole.Leader:
                    col = Color.Red;
                    break;

                case ThreadDeviceRole.Disabled:
                default:
                    col = Color.Black;
                    brightness = 0;
                    break;
            }

            _led.SetLed(0, col, brightness);
            _led.Update();
        }
    }
}
