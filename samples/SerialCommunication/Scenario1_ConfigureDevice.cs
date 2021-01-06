//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//


//////////////////////////////////////////////////////////////////////
///
/// NOTE: when working with ESP32 this define needs to be uncommented
//#define BUIID_FOR_ESP32
///
//////////////////////////////////////////////////////////////////////


using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.SerialCommunication;
#if BUIID_FOR_ESP32
using nanoFramework.Hardware.Esp32;
#endif

namespace SerialCommunication
{
    public class Scenario1_ConfigureDevice
    {
        static SerialDevice _serialDevice;

        public static void Main()
        {
            // get available ports
            var serialPorts = SerialDevice.GetDeviceSelector();

            Debug.WriteLine("available serial ports: " + serialPorts);


#if BUIID_FOR_ESP32
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // COM2 in ESP32-WROVER-KIT mapped to free GPIO pins
            // mind to NOT USE pins shared with other devices, like serial flash and PSRAM
            // also it's MANDATORY to set pin funcion to the appropriate COM before instanciating it

            // set GPIO functions for COM2 (this is UART1 on ESP32)
            Configuration.SetPinFunction(Gpio.IO04, DeviceFunction.COM2_TX);
            Configuration.SetPinFunction(Gpio.IO05, DeviceFunction.COM2_RX);

            // open COM2
            _serialDevice = SerialDevice.FromId("COM2");
#else
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            // COM6 in STM32F769IDiscovery board (Tx, Rx pins exposed in Arduino header CN13: TX->D1, RX->D0)
            // open COM6
            _serialDevice = SerialDevice.FromId("COM6");
#endif

            // set parameters
            _serialDevice.BaudRate = 9600;
            _serialDevice.Parity = SerialParity.None;
            _serialDevice.StopBits = SerialStopBitCount.One;
            _serialDevice.Handshake = SerialHandshake.None;
            _serialDevice.DataBits = 8;

            // uncomment the scenario to test (!!note that none of these returns!!)

            // uncomment the following call to transmit data
            //Scenario2_StreamWrite.Execute(ref _serialDevice);

            // uncomment the following call to read data using a stream reader
            //Scenario3_StreamRead.Execute(ref _serialDevice);

            // uncomment the following call to wait for a data received event
            //Scenario4_DataReceivedEvent.Execute(ref _serialDevice);


            Thread.Sleep(Timeout.Infinite);
        }
    }
}
