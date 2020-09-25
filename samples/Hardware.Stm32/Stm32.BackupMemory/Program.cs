//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using nanoFramework.Hardware.Stm32;

namespace Stm32BackupMemory.TestApp
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine($"The backup memory has {BackupMemory.Size} bytes.");

            // write a byte array
            uint testBufferPosition = 5;

            byte[] testBuffer = new byte[] { 0xFA, 0xCE, 0xBE, 0xEF, 0xFA, 0xCE, 0xBE, 0xEF };
            BackupMemory.WriteBytes(testBufferPosition, testBuffer);

            // read back the byte array
            byte[] readBackBuffer = new byte[testBuffer.Length];
            BackupMemory.ReadBytes(testBufferPosition, readBackBuffer);

            if(readBackBuffer.GetHashCode() != testBuffer.GetHashCode())
            {
                Console.WriteLine("Array read from backup memory is different than what was written.");
            }
            else
            {
                Console.WriteLine("Buffer comparison check!");
            }

            // write an Int64
            uint testInt64Position = 15;

            Int64 testInt64 = 9876543210;
            BackupMemory.WriteInt64(testInt64Position, testInt64);

            if (BackupMemory.ReadInt64(testInt64Position) != testInt64)
            {
                Console.WriteLine("Int64 read from backup memory is different than what was written.");
            }
            else
            {
                Console.WriteLine("Int64 comparison check!");
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
