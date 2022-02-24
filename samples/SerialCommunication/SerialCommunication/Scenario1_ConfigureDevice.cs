//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;
using System.IO.Ports;
#if BUIID_FOR_ESP32
using nanoFramework.Hardware.Esp32;
#endif

///////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////
///                                                                             ///
/// NOTE: when working with ESP32 edit the nfproj file and add                  ///
/// BUIID_FOR_ESP32 to the DefineConstants, like this:                          ///
///                                                                             ///
/// <DefineConstants>$(DefineConstants);BUIID_FOR_ESP32;</DefineConstants>      ///
///                                                                             ///
/// You'll also need to add a reference to 'nanoFramework.Hardware.Esp32' NuGet ///
///                                                                             ///
///////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////

namespace SerialCommunication
{
    public class Scenario1_ConfigureDevice
    {
        static SerialPort _serialDevice;

        public static void Main()
        {
            // get available ports
            var ports = SerialPort.GetPortNames();

            Debug.WriteLine("Available ports: ");
            foreach (string port in ports)
            {
                Debug.WriteLine($" {port}");
            }

#if BUIID_FOR_ESP32
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // COM2 in ESP32-WROVER-KIT mapped to free GPIO pins
            // mind to NOT USE pins shared with other devices, like serial flash and PSRAM
            // also it's MANDATORY to set pin function to the appropriate COM before instantiating it

            Configuration.SetPinFunction(32, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(33, DeviceFunction.COM2_TX);

            // open COM2
            _serialDevice = new SerialPort("COM2");
#else
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            // COM6 in STM32F769IDiscovery board (Tx, Rx pins exposed in Arduino header CN13: TX->D1, RX->D0)
            // open COM6
            _serialDevice = new SerialPort("COM6");
#endif
            // set parameters
            _serialDevice.BaudRate = 9600;
            _serialDevice.Parity = Parity.None;
            _serialDevice.StopBits = StopBits.One;
            _serialDevice.Handshake = Handshake.None;
            _serialDevice.DataBits = 8;

            // if dealing with massive data input, increase the buffer size
            _serialDevice.ReadBufferSize = 2048;

            // open the serial port with the above settings
            _serialDevice.Open();

            // uncomment the scenario to test (!!note that none of these returns!!)

            // uncomment the following call to transmit data
            Scenario2_Write.Execute(ref _serialDevice);

            // uncomment the following call to read data
            //Scenario3_Read.Execute(ref _serialDevice);

            // uncomment the following call to wait for a data received event
            //Scenario4_DataReceivedEvent.Execute(ref _serialDevice);

            Thread.Sleep(Timeout.Infinite);
        }

        private static void _serialDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
