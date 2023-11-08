//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.I2s;
using System.Diagnostics;
using System.IO;
using nanoFramework.Hardware.Esp32;

namespace AudioPlayer
{
    /// <summary>
    /// This class is intended to be used for a simple I2S WAV file player on ESP32.
    /// You have to provide pin configuration for I2S communication and a full path to the
    /// WAV file you want to play.
    /// </summary>
    public class I2sWavPlayer : IDisposable
    {
        public enum Bus
        {
            One = 1,
            Two = 2
        }

        private readonly I2sDevice _i2S;
        private readonly FileStream _stream;

        /// <summary>
        /// Creating a new instance of <see cref="I2sWavPlayer" />.
        /// </summary>
        /// <param name="bus">The I2S bus ID on ESP32 platforms.</param>
        /// <param name="audioFile">Full path to WAV file.</param>
        /// <param name="bckPin">The Pin ID of the BCK pin. (32 for <see cref="Bus.One" />).</param>
        /// <param name="dataPin">The Pin ID of the Data Out pin. (33 for <see cref="Bus.One" />).</param>
        /// <param name="wsPin">The Pin ID of the WS pin. (25 for <see cref="Bus.One" />).</param>
        /// <exception cref="IOException">Throws an IOException if the WAV file provided does not have at least 44 bytes (header).</exception>
        public I2sWavPlayer(Bus bus, string audioFile, int bckPin = 32, int dataPin = 33, int wsPin = 25)
        {
            switch (bus)
            {
                case Bus.One:
                    // I2S Audio device:
                    Configuration.SetPinFunction(bckPin, DeviceFunction.I2S1_BCK);
                    Configuration.SetPinFunction(dataPin, DeviceFunction.I2S1_DATA_OUT);
                    Configuration.SetPinFunction(wsPin, DeviceFunction.I2S1_WS);
                    break;
                case Bus.Two:
                    // I2S Audio device:
                    Configuration.SetPinFunction(bckPin, DeviceFunction.I2S2_BCK);
                    Configuration.SetPinFunction(dataPin, DeviceFunction.I2S2_DATA_OUT);
                    Configuration.SetPinFunction(wsPin, DeviceFunction.I2S2_WS);
                    break;
            }

            _stream = new FileStream(audioFile, FileMode.Open, FileAccess.Read);

            var header = new byte[44];
            var len = _stream.Read(header, 0, header.Length);
            if (len != 44)
            {
                throw new IOException("Not enough bytes in the wav file header.");
            }

            var headerParser = new WavFileHeader(header);

            _i2S = new I2sDevice(new I2sConnectionSettings((int) bus)
            {
                Mode = I2sMode.Master | I2sMode.Tx,
                //Mode = I2sMode.Master | I2sMode.Tx | I2sMode.Pdm, // Try this if output contains lots of static and poor audio quality
                CommunicationFormat = I2sCommunicationFormat.I2S,

                SampleRate = headerParser.SampleRate,
                BitsPerSample = ToBitsPerSample(headerParser.BitsPerSample),
                ChannelFormat = ToChannelFormat(headerParser.NumberOfChannels),
                BufferSize = 40000
            });
        }

        public void Dispose()
        {
            _i2S.Dispose();

            _stream.Close();
            _stream.Dispose();
        }

        public void Play()
        {
            _stream.Seek(44, SeekOrigin.Begin);

            var buffer = new byte[10000]; 
            var spanBytes = new SpanByte(buffer);
            SpanByte writeSpanByte;
            int length;

            try
            {
                while (true)
                {
                    writeSpanByte = spanBytes;
                    length = _stream.Read(buffer, 0, buffer.Length);

                    if (length == 0)
                    {
                        // end of file, quit:
                        break;
                    }

                    if (length != buffer.Length)
                    {
                        writeSpanByte = spanBytes.Slice(0, length);
                    }

                    _i2S.Write(writeSpanByte);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception happened, but why? {e.Message}");
            }
        }

        private static I2sChannelFormat ToChannelFormat(short channels)
        {
            return channels switch
            {
                1 => I2sChannelFormat.OnlyLeft,
                2 => I2sChannelFormat.RightLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(channels), "Only supports either Mono or Stereo WAV files.")
            };
        }

        private static I2sBitsPerSample ToBitsPerSample(short bitsPerSample)
        {
            return bitsPerSample switch
            {
                8 => I2sBitsPerSample.Bit8,
                16 => I2sBitsPerSample.Bit16,
                24 => I2sBitsPerSample.Bit24,
                32 => I2sBitsPerSample.Bit32,
                _ => throw new ArgumentOutOfRangeException(nameof(bitsPerSample), "Only 8, 16, 24 or 32 bits per sample are supported.")
            };
        }
    }
}
