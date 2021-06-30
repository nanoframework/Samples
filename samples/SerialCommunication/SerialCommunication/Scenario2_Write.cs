//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;
using System.IO.Ports;

namespace SerialCommunication
{
    public class Scenario2_Write
    {
        public static void Execute(ref SerialPort serialDevice)
        {
            // setup write timeout
            // because we are writing to the UART it's recommended to set a write timeout
            // otherwise the write operation doesn't return until the requested number of bytes has been written
            serialDevice.WriteTimeout = 500;

            for (; ; )
            {
                // write string followed by new line to Serial Device 
                serialDevice.WriteLine(DateTime.UtcNow + " hello from nanoFramework!");

                //Debug.WriteLine("Wrote string over " + serialDevice.PortName + ".");

                serialDevice.Write(DateTime.UtcNow + ">>>|");

                Thread.Sleep(750);

                serialDevice.Write(">>>>>>|");

                Thread.Sleep(750);

                serialDevice.WriteLine(">>>>>>>>>>|");

                Thread.Sleep(750);

                serialDevice.WriteLine(DateTime.UtcNow.ToString());

                //Debug.WriteLine("Wrote partial strings over " + serialDevice.PortName + ".");

                Thread.Sleep(2000);
            }
        }
    }
}
