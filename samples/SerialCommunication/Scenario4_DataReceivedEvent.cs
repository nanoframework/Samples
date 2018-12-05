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
    public class Scenario4_DataReceivedEvent
    {
        public static void Execute(ref SerialDevice serialDevice)
        {
            // setup read timeout
            // because we are reading from the UART it's recommended to set a read timeout
            // otherwise the reading operation doesn't return until the requested number of bytes has been read
            serialDevice.ReadTimeout = new TimeSpan(0, 0, 4);

            // setup data read for Serial Device input stream
            DataReader inputDataReader = new DataReader(serialDevice.InputStream);

            // setup an event handler that will fire when a char is received in the serial device input stream
            serialDevice.DataReceived += SerialDevice_DataReceived;

            // set a watch char to be notified when it's available in the input stream
            serialDevice.WatchChar = '\r';

            Thread.Sleep(Timeout.Infinite);
        }

        private static void SerialDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if(e.EventType == SerialData.Chars)
            {
                Console.WriteLine("rx chars");
            }
            else if (e.EventType == SerialData.WatchChar)
            {
                Console.WriteLine("rx watch char");


                SerialDevice serialDevice = (SerialDevice)sender;

                using (DataReader inputDataReader = new DataReader(serialDevice.InputStream))
                {
                    inputDataReader.InputStreamOptions = InputStreamOptions.Partial;

                    // read all available bytes from the Serial Device input stream
                    var bytesRead = inputDataReader.Load(serialDevice.BytesToRead);

                    Console.WriteLine("Read completed: " + bytesRead + " bytes were read from " + serialDevice.PortName + ".");

                    if (bytesRead > 0)
                    {
                        String temp = inputDataReader.ReadString(bytesRead);
                        Console.WriteLine("String: >>" + temp + "<< ");
                    }
                }
            }
        }
    }
}
