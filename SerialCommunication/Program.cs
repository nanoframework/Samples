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
            // nasty hack to allow setting breakpoints
            Thread.Sleep(5000);

            // get available ports
            var serialPorts = SerialDevice.GetDeviceSelector();

            Console.WriteLine("available serial ports: " + serialPorts);

            // COM6 in STM32F769IDiscovery board (Tx, Rx pins exposed in Arduino header CN13)

            // open COM
            _serialDevice = SerialDevice.FromId("COM6");

            // set parameters
            _serialDevice.BaudRate = 38400;
            _serialDevice.Parity = SerialParity.None;
            _serialDevice.StopBits = SerialStopBitCount.One;
            _serialDevice.Handshake = SerialHandshake.None;
            _serialDevice.DataBits = 8;

            // setup data writer for Serial Device output stream
            DataWriter outputDataWriter = new DataWriter(_serialDevice.OutputStream);

            for ( ; ; )
            {
                Thread.Sleep(2000);

                // write string to Serial Device output stream using data writer
                // (this doesn't send any data, just writes to the stream)
                var bytesWritten = outputDataWriter.WriteString(DateTime.UtcNow + " hello from nanoFramework!\r\n");

                // calling the 'Store' methods on the data writer actually sends the data
                var bw1 = outputDataWriter.Store();

                Console.WriteLine("Sent " + bw1 + " bytes over " + _serialDevice.PortName + ".");
            }
        }
    }
}
