//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace SerialCommunication
{
    public class Scenario2_StreamWrite
    {
        public static void Execute(ref SerialDevice serialDevice)
        {
            // setup write timeout
            // because we are writing to the UART it's recommended to set a write timeout
            // otherwise the write operation doesn't return until the requested number of bytes has been written
            serialDevice.WriteTimeout = new TimeSpan(0, 0, 5);

            // setup data writer for Serial Device output stream
            DataWriter outputDataWriter = new DataWriter(serialDevice.OutputStream);

            for (; ; )
            {
                // write string to Serial Device output stream using data writer
                // (this doesn't send any data, just writes to the stream)
                var bytesWritten = outputDataWriter.WriteString(DateTime.UtcNow + " hello from nanoFramework!\r\n");
                Console.WriteLine("Wrote " + outputDataWriter.UnstoredBufferLength + " bytes to output stream.");

                // calling the 'Store' method on the data writer actually sends the data
                var bw1 = outputDataWriter.Store();
                Console.WriteLine("Sent " + bw1 + " bytes over " + serialDevice.PortName + ".");

                // another dummy string, just to output something when the above is still Txing
                bytesWritten = outputDataWriter.WriteString(DateTime.UtcNow + "...\r\n");
                Console.WriteLine("Wrote " + outputDataWriter.UnstoredBufferLength + " bytes to output stream.");

                // calling the 'Store' method on the data writer actually sends the data
                bw1 = outputDataWriter.Store();
                Console.WriteLine("Sent " + bw1 + " bytes over " + serialDevice.PortName + ".");

                Thread.Sleep(2000);
            }
        }
    }
}
