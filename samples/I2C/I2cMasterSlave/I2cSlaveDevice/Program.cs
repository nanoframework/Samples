//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.I2c;
using System.Diagnostics;
using nanoFramework.Hardware.Esp32;

namespace Samples.I2c.SlaveDevice
{
    public class Program
    {
        public static void Main()
        {
            Configuration.SetPinFunction(Gpio.IO18, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(Gpio.IO19, DeviceFunction.I2C1_CLOCK);

            // create an I2C slave device on bus 1 with address 0x10
            var device = new I2cSlaveDevice(1, 0x10);

            while (true)
            {
                // wait "forever" for a single byte
                try
                {
                    if (device.ReadByte(out byte registerAddress, 500))
                    {
                        switch (registerAddress)
                        {
                            case 0x22:
                                // reply back with 2 dummy bytes
                                device.Write(new byte[] { 0xBE, 0xEF });

                                Debug.WriteLine($"Received message: {registerAddress:X2}");

                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Timeout I2C device");
                }
            }
        }
    }
}
