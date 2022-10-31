//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;

namespace AudioPlayer
{
    /// <summary>
    /// This class can be used to parse / create a WAV file header.
    /// </summary>
    /// <remarks>
    /// For WAV header specification see <see href="https://docs.fileformat.com/audio/wav/" />.
    /// </remarks>
    public class WavFileHeader
    {
        private readonly byte[] _header;

        /// <summary>
        /// Creates a new instance of <see cref="WavFileHeader" /> based on the provided header bytes.
        /// </summary>
        /// <param name="header">The header bytes to use as backing for all supported header properties.</param>
        /// <exception cref="ArgumentException">Throws if the header array does not contain exactly 44 bytes.</exception>
        public WavFileHeader(byte[] header)
        {
            if (header == null || header.Length != 44)
            {
                throw new ArgumentException("Only WAV file headers with 44 bytes are supported.");
            }

            _header = header;
        }

        public WavFileHeader()
        {
            _header = new byte[44];
            RiffChunkId = "RIFF";
            WaveFormat = "WAVE";
            FormatChunkId = "fmt ";
            DataChunkId = "data";
        }

        /// <summary>
        /// Marks the file as a riff file.
        /// Characters are each 1 byte long.
        /// </summary>
        public string RiffChunkId
        {
            get => Encoding.UTF8.GetString(_header, 0, 4);
            set
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                if (bytes.Length != 4)
                {
                    throw new ArgumentException("RiffChunkId must be 4 bytes long");
                }

                Array.Copy(bytes, 0, _header, 0, 4);
            }
        }

        /// <summary>
        /// Size of the overall file - 8 bytes, in bytes (32-bit integer).
        /// Typically, you’d fill this in after creation.
        /// </summary>
        public int FileSize
        {
            get => BitConverter.ToInt32(_header, 4);
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, _header, 4, 4);
            }
        }

        /// <summary>
        /// File Type Header.
        /// For our purposes, it must always equal to “WAVE”.
        /// </summary>
        public string WaveFormat
        {
            get => Encoding.UTF8.GetString(_header, 8, 4);
            set
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                if (bytes.Length != 4)
                {
                    throw new ArgumentException("WaveFormat must be 4 bytes long");
                }

                Array.Copy(bytes, 0, _header, 8, 4);
            }
        }

        /// <summary>
        /// Format chunk marker (4 characters).
        /// Includes trailing <see cref="string.Empty" />.
        /// </summary>
        public string FormatChunkId
        {
            get => Encoding.UTF8.GetString(_header, 12, 4);
            set
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                if (bytes.Length != 4)
                {
                    throw new ArgumentException("FormatChunkId must be 4 bytes long");
                }

                Array.Copy(bytes, 0, _header, 12, 4);
            }
        }

        /// <summary>
        /// Length of format data.
        /// </summary>
        public int FormatChunkSize
        {
            get => BitConverter.ToInt32(_header, 16);
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, _header, 16, 4);
            }
        }

        /// <summary>
        /// AudioFormat: Indicates how the sample data for the wave file is stored.
        /// The most common format is integer PCM, which has a code of 1.
        /// <para>
        /// Other formats include:
        /// <list type="bullet">
        /// <item><term>2</term><description>ADPCM</description></item>
        /// <item><term>3</term><description>floating point PCM</description></item>
        /// <item><term>6</term><description>A-law</description></item>
        /// <item><term>7</term><description>μ-law</description></item>
        /// <item><term>65534</term><description>WaveFormatExtensible</description></item>
        /// </list>
        /// </para>
        /// </summary>
        public short AudioFormat
        {
            get => BitConverter.ToInt16(_header, 20);
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, _header, 20, 2);
            }
        }

        /// <summary>
        /// Number of channels: Typically a file will have 1 channel (mono) or 2 channels (stereo).
        /// A 5.1 surround sound file will have 6 channels.
        /// </summary>
        public short NumberOfChannels
        {
            get => BitConverter.ToInt16(_header, 22);
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, _header, 22, 2);
            }
        }

        /// <summary>
        /// Sample Rate: The number of sample frames that occur each second.
        /// A typical value would be 44100, which is the same as an audio CD.
        /// </summary>
        public int SampleRate
        {
            get => BitConverter.ToInt32(_header, 24);
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, _header, 24, 4);
            }
        }

        /// <summary>
        /// Bytes per second
        /// <para>
        /// The spec calls this byte rate, which means the number of bytes required for one second of audio data. This is
        /// equal to the bytes per sample frame times the sample rate.
        /// </para>
        /// <para>
        /// So with a bytes per sample frame of 4, and a sample
        /// rate of 44100, this should equal 176400.
        /// </para>
        /// </summary>
        public int BytesPerSecond
        {
            get => BitConverter.ToInt32(_header, 28);
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, _header, 28, 4);
            }
        }

        /// <summary>
        /// Bytes per sample frame
        /// <para>
        /// Called block align by the spec, this is the number of
        /// bytes required to store a single sample frame,
        /// i.e. a single sample for each channel.
        /// </para>
        /// </summary>
        public short BytesPerSampleFrame
        {
            get => BitConverter.ToInt16(_header, 32);
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, _header, 32, 2);
            }
        }

        /// <summary>
        /// Bits per sample
        /// <para>
        /// For integer PCM data, typical values will be 8, 16, or 32.
        /// </para>
        /// </summary>
        public short BitsPerSample
        {
            get => BitConverter.ToInt16(_header, 34);
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, _header, 32, 2);
            }
        }

        /// <summary>
        /// Marks the start of the data chunk.
        /// <para>
        /// Should always be "data" for PCM WAV files.
        /// </para>
        /// </summary>
        public string DataChunkId
        {
            get => Encoding.UTF8.GetString(_header, 36, 4);
            set
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                if (bytes.Length != 4)
                {
                    throw new ArgumentException("DataChunkId must be 4 bytes long");
                }

                Array.Copy(bytes, 0, _header, 36, 4);
            }
        }

        /// <summary>
        /// Data chunk size
        /// <para>
        /// The size of the chunk data.
        /// </para>
        /// </summary>
        public int DataChunkSize
        {
            get => BitConverter.ToInt32(_header, 40);
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, _header, 40, 4);
            }
        }

        /// <summary>
        /// Gets the WAV file header as byte[].
        /// </summary>
        /// <returns>The header byte[].</returns>
        public byte[] GetHeaderData()
        {
            return _header;
        }
    }
}
