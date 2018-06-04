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
    public class Program
    {
        static SerialDevice _serialDevice;

        public static void Main()
        {
            // get available ports
            var serialPorts = SerialDevice.GetDeviceSelector();

            Console.WriteLine("available serial ports: " + serialPorts);

            // COM6 in STM32F769IDiscovery board (Tx, Rx pins exposed in Arduino header CN13: TX->D1, RX->D0)

            // open COM
            _serialDevice = SerialDevice.FromId("COM6");

            // set parameters
            _serialDevice.BaudRate = 38400;
            _serialDevice.Parity = SerialParity.None;
            _serialDevice.StopBits = SerialStopBitCount.One;
            _serialDevice.Handshake = SerialHandshake.None;
            _serialDevice.DataBits = 8;

            // setup read timeout
            // because we are reading from the UART it's recommended to set a read timeout
            // otherwise the reading operation doesn't return until the requested number of bytes has been read
            _serialDevice.ReadTimeout = new TimeSpan(0, 0, 4);

            // setup write timeout
            // because we are writing to the UART it's recommended to set a write timeout
            // otherwise the write operation doesn't return until the requested number of bytes has been written
            _serialDevice.WriteTimeout = new TimeSpan(0, 0, 5);

            // setup data writer for Serial Device output stream
            DataWriter outputDataWriter = new DataWriter(_serialDevice.OutputStream);

            // setup data read for Serial Device input stream
            DataReader inputDataReader = new DataReader(_serialDevice.InputStream);
            inputDataReader.InputStreamOptions = InputStreamOptions.Partial;

            for ( ; ; )
            {
                Thread.Sleep(2000);


                // write string to Serial Device output stream using data writer
                // (this doesn't send any data, just writes to the stream)
                var bytesWritten = outputDataWriter.WriteString(DateTime.UtcNow + " hello from nanoFramework!\r\n");
                Console.WriteLine("Wrote " + outputDataWriter.UnstoredBufferLength + " bytes to output stream.");

                // calling the 'Store' method on the data writer actually sends the data
                var bw1 = outputDataWriter.Store();
                Console.WriteLine("Sent " + bw1 + " bytes over " + _serialDevice.PortName + ".");


                // another dummy string, just to output something when the above is still Txing
                bytesWritten = outputDataWriter.WriteString(DateTime.UtcNow + "...\r\n");
                Console.WriteLine("Wrote " + outputDataWriter.UnstoredBufferLength + " bytes to output stream.");

                // calling the 'Store' method on the data writer actually sends the data
                bw1 = outputDataWriter.Store();
                Console.WriteLine("Sent " + bw1 + " bytes over " + _serialDevice.PortName + ".");


                // attempt to read 5 bytes from the Serial Device input stream
                var bytesRead = inputDataReader.Load(5);

                Console.WriteLine("Read completed: " + bytesRead + " bytes were read from " + _serialDevice.PortName + ".");

                if (bytesRead > 0)
                {
                    String temp = inputDataReader.ReadString(bytesRead);
                    Console.WriteLine("String: >>" + temp + "<< ");
                }
            }
        }
    }
}
