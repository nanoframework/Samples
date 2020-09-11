//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;
using TIUtilities = nanoFramework.Hardware.TI.Utilities;

namespace Hardware.TI.Utilities
{
    public class Program
    {
        public static void Main()
        {
            string devideAddress = "";
            byte[] ieeeAddress = TIUtilities.GetIeeeAddress();

            foreach (byte b in ieeeAddress)
            {
                devideAddress += b.ToString("X2");
            }

            Debug.WriteLine($"Device IEEE address: {devideAddress}");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
