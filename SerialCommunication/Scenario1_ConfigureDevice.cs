//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using Windows.Devices.SerialCommunication;

namespace SerialCommunication
{
    public class Scenario1_ConfigureDevice
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


            // uncomment the scenario to test (!!note that none of these returns!!)

            // uncomment the following call to transmit data
            Scenario2_StreamWrite.Execute(ref _serialDevice);

            // uncomment the following call to read data using a stream reader
            //Scenario3_StreamRead.Execute(ref _serialDevice);

            // uncomment the following call to wait for a data received event
            //Scenario4_DataReceivedEvent.Execute(ref _serialDevice);


            Thread.Sleep(Timeout.Infinite);
        }
    }
}
