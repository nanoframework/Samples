//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.Spi;
using System.Diagnostics;
using System.Threading;

namespace SpiExamples
{
    public class Program
    {
        public static void Main()
        {
            SpiDevice spiDevice;
            SpiConnectionSettings connectionSettings;
            Debug.WriteLine("Hello from sample for System.Device.Spi!");
            // You can get the values of SpiBus
            SpiBusInfo spiBusInfo = SpiDevice.GetBusInfo(1);
            Debug.WriteLine($"{nameof(spiBusInfo.MaxClockFrequency)}: {spiBusInfo.MaxClockFrequency}");
            Debug.WriteLine($"{nameof(spiBusInfo.MinClockFrequency)}: {spiBusInfo.MinClockFrequency}");

            // Note: the ChipSelect pin should be adjusted to your device, here 12
            connectionSettings = new SpiConnectionSettings(1, 12);
            // You can adjust other settings as well in the connection
            connectionSettings.ClockFrequency = 1_000_000;
            connectionSettings.DataBitLength = 8;
            connectionSettings.DataFlow = DataFlow.LsbFirst;
            connectionSettings.Mode = SpiMode.Mode2;

            // Then you create your SPI device by passing your settings
            spiDevice = SpiDevice.Create(connectionSettings);

            // You can write a SpanByte
            SpanByte writeBufferSpanByte = new byte[2] { 42, 84 };
            spiDevice.Write(writeBufferSpanByte);
            // Or a ushort buffer
            ushort[] writeBufferushort = new ushort[2] { 4200, 8432 };
            spiDevice.Write(writeBufferushort);
            // Or simply a byte
            spiDevice.WriteByte(42);

            // The read operations are similar
            SpanByte readBufferSpanByte = new byte[2];
            // This will read 2 bytes
            spiDevice.Read(readBufferSpanByte);
            ushort[] readUshort = new ushort[4];
            // This will read 4 ushort
            spiDevice.Read(readUshort);
            // read 1 byte
            byte readMe = spiDevice.ReadByte();
            Debug.WriteLine($"I just read a byte {readMe}");

            // And you can operate full transferts as well
            SpanByte writeBuffer = new byte[4] { 0xAA, 0xBB, 0xCC, 0x42 };
            SpanByte readBuffer = new byte[4];
            spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
            // Same for ushirt arrays:
            ushort[] writeBufferus = new ushort[4] { 0xAABC, 0x00BB, 0xCC00, 0x4242 };
            ushort[] readBufferus = new ushort[4];
            spiDevice.TransferFullDuplex(writeBufferus, readBufferus);

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
