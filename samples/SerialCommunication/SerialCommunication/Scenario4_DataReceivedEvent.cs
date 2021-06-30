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
    public class Scenario4_DataReceivedEvent
    {
        public static void Execute(ref SerialPort serialDevice)
        {
            // setup read timeout
            // because we are reading from the UART it's recommended to set a read timeout
            // otherwise the reading operation doesn't return until the requested number of bytes has been read
            serialDevice.ReadTimeout = 2000;

            // setup an event handler that will fire when a char is received in the serial device input stream
            serialDevice.DataReceived += SerialDevice_DataReceived;

            // set a watch char to be notified when it's available in the input stream
            serialDevice.WatchChar = '\r';

            Debug.WriteLine("Waiting to receive data in serial port...");


            Thread.Sleep(Timeout.Infinite);
        }

        private static void SerialDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialDevice = (SerialPort)sender;

            if (e.EventType == SerialData.Chars)
            {
                Debug.WriteLine("rx chars");
            }
            else if (e.EventType == SerialData.WatchChar)
            {
                Debug.WriteLine("rx watch char");
            }

            // need to make sure that there is data to be read, because
            // the event could have been queued several times and data read on a previous call
            if (serialDevice.BytesToRead > 0)
            {
                byte[] buffer = new byte[serialDevice.BytesToRead];

                var bytesRead = serialDevice.Read(buffer, 0, buffer.Length);

                Debug.WriteLine("Read completed: " + bytesRead + " bytes were read from " + serialDevice.PortName + ".");

                string temp = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.WriteLine("String: >>" + temp + "<< ");
            }
        }
    }
}
