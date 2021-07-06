//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace SerialCommunication
{
    public class Scenario3_Read
    {
        public static void Execute(ref SerialPort serialDevice)
        {
            // setup read timeout
            // because we are reading from the UART it's recommended to set a read timeout
            // otherwise the reading operation doesn't return until the requested number of bytes has been read
            serialDevice.ReadTimeout = 4000;


            // if the WatchChar is set the inputDataReader.Load bellow will return as soon as this character is received in the incoming stream, 
            // no matter if the request amount of bytes has been read or not
            // serialDevice.WatchChar = '\r';
            
            byte[] buffer = new byte[5];

            for (;;)
            {
                // attempt to read 5 bytes from the SerialPort
                if (serialDevice.BytesToRead > buffer.Length)
                {
                    var bytesRead = serialDevice.Read(buffer, 0, buffer.Length);

                    Debug.WriteLine("Read completed: " + bytesRead + " bytes were read from " + serialDevice.PortName + ".");

                    if (bytesRead > 0)
                    {
                        String temp = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Debug.WriteLine("String: >>" + temp + "<< ");
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
