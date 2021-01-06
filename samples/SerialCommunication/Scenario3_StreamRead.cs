//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace SerialCommunication
{
    public class Scenario3_StreamRead
    {
        public static void Execute(ref SerialDevice serialDevice)
        {
            // setup read timeout
            // because we are reading from the UART it's recommended to set a read timeout
            // otherwise the reading operation doesn't return until the requested number of bytes has been read
            serialDevice.ReadTimeout = new TimeSpan(0, 0, 4);

            // setup data read for Serial Device input stream
            DataReader inputDataReader = new DataReader(serialDevice.InputStream)
            {
                InputStreamOptions = InputStreamOptions.Partial
            };

            // if the WatchChar is set the inputDataReader.Load bellow will return as soon as this character is received in the incoming stream, 
            // no matter if the request amount of bytes has been read or not
            // serialDevice.WatchChar = '\r';

            for (;;)
            {
                // attempt to read 5 bytes from the Serial Device input stream
                var bytesRead = inputDataReader.Load(5);

                Debug.WriteLine("Read completed: " + bytesRead + " bytes were read from " + serialDevice.PortName + ".");

                if (bytesRead > 0)
                {
                    String temp = inputDataReader.ReadString(bytesRead);
                    Debug.WriteLine("String: >>" + temp + "<< ");
                }

                Thread.Sleep(1000);
            }
        }
    }
}
