//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;
using GiantGecko = nanoFramework.Hardware.GiantGecko;

namespace ReadDeviceIDs
{
    public class Program
    {
        public static void Main()
        {
            string uniqueDeviceId = "";

            foreach(byte b in GiantGecko.Utilities.UniqueDeviceId)
            {
                uniqueDeviceId += b.ToString("X2");
            }

            Debug.WriteLine($"Unique device ID: {uniqueDeviceId}");

            Debug.WriteLine($"Production revision: {GiantGecko.Utilities.ProductionRevision}");

            Debug.WriteLine($"Device family: {GiantGecko.Utilities.DeviceFamily}");

            Debug.WriteLine($"Device number: { GiantGecko.Utilities.DeviceNumber}");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
