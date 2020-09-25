//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using STM32 = nanoFramework.Hardware.Stm32;

namespace Stm32.ReadDeviceIDs
{
    public class Program
    {
        public static void Main()
        {
            string uniqueDeviceId = "";

            foreach(byte b in STM32.Utilities.UniqueDeviceId)
            {
                uniqueDeviceId += b.ToString("X2");
            }

            Console.WriteLine($"Unique device ID: {uniqueDeviceId}");

            Console.WriteLine($"Device identifier: { STM32.Utilities.DeviceId.ToString("X4")}");

            Console.WriteLine($"Device revision identifier: { STM32.Utilities.DeviceRevisionId.ToString("X4")}");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
