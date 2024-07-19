//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.I2c;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Hardware.Esp32;

namespace Samples.I2c.MasterDevice
{
    public class Program
    {
        public static void Main()
        {
            // Configure I2C GPIO pins
            Configuration.SetPinFunction(Gpio.IO18, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(Gpio.IO19, DeviceFunction.I2C1_CLOCK);

            // create I2C device
            var myI2cDevice = I2cDevice.Create(new I2cConnectionSettings(
                1,
                0x10,
                I2cBusSpeed.FastMode));

            // setup read buffer
            var buffer = new byte[2];

            while (true)
            {
                try
                {
                    // set address to read from
                    if (myI2cDevice.Write(new byte[] { 0x22 }).BytesTransferred != 1)
                    {
                        Debug.WriteLine("Error writting to I2C device to set register address to read from");
                    }
                    else
                    {
                        if (myI2cDevice.Read(buffer).BytesTransferred != 2)
                        {
                            Debug.WriteLine("Error reading from I2C device");
                        }
                        else
                        {

                            // expected buffer content is: 0xBE, 0xEF
                            Debug.WriteLine($"Register content: {buffer[0]:X2} {buffer[1]:X2}");
                        }
                    }

                    // pause before a new read
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error reading from I2C device: {ex.Message}");
                }
            }
        }
    }
}
