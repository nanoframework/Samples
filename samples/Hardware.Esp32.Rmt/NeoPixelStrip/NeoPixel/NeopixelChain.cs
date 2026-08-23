//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.

using nanoFramework.Hardware.Esp32.Rmt;

namespace NeoPixel
{
    public class NeopixelChain
    {
        protected byte[] ColorBytes;
        private readonly int _gpioPin;
        private int _repeat;
        private LedTransmitChannel _ledChannel;

        /// <summary>
        /// Create NeopixelChain object
        /// </summary>
        /// <param name="gpioPin">GPIO pin of led string</param>
        /// <param name="size">Number of pixels in led string</param>
		/// <param name="repeat">
		/// Repeat data on send on to duplicate patterns. When repeating the size should be size of pattern.
		/// Defaults to 1 repeat.
		/// </param>
        public NeopixelChain(int gpioPin, LedType ledType, uint size, int repeat = 1)
        {
            _gpioPin = gpioPin;

            // 3 bytes per pixel
            // All bytes will start as 0(black) at creation
            ColorBytes = new byte[size * 3];

            _repeat = repeat;

            _ledChannel = new LedTransmitChannel(gpioPin, ledType);
        }

        public void Update()
        {
            _ledChannel.SendLedData(ColorBytes, _repeat, true);
        }

        /// <summary>
        /// Get or Set color bytes in chain
        /// </summary>
        /// <param name="index">Index to pixels in chain</param>
        public Color this[uint index]
        {
            get
            {
                uint i = index * 3;
                return new Color() { G = ColorBytes[i], R = ColorBytes[i + 1], B = ColorBytes[i + 2] };
            }
            set
            {
                uint i = index * 3;
                ColorBytes[i] = value.G;
                ColorBytes[i + 1] = value.R;
                ColorBytes[i + 2] = value.B;
            }
        }
    }
}
