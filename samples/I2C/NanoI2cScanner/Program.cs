// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Hardware.Esp32;
using System;
using System.Device.I2c;
using System.Diagnostics;
using System.Threading;

// Depending on the device you are using, you may have to adjust the pins
// Here for an ESP32 with the Hardware.Esp32 nuget
Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);
Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
// For other devices like STM32, refer to the documentation, also remove the esp32 nuget
// and comment the line above.

Debug.WriteLine("Hello from I2C Scanner!");
SpanByte span = new byte[1];
bool isDevice;
// On a normal bus, not all those ranges are supported but scanning anyway
for (int i = 0; i <= 0xFF; i++)
{
    isDevice = false;
    I2cDevice i2c = new(new I2cConnectionSettings(1, i));
    // What we write is not important
    var res = i2c.WriteByte(0x07);
    // A successfull write will be: 0x10 Write: 1, transferred: 1
    // A non successful one: 0x0F Write: 4, transferred: 0
    Debug.Write($"0x{i:X2} Write: {res.Status}, transferred: {res.BytesTransferred}");
    isDevice = res.Status == I2cTransferStatus.FullTransfer;

    // What we read doesn't matter, reading only 1 element is what's needed
    res = i2c.Read(span);
    // A successfull write will be: Read: 1, transferred: 1
    // A non successfull one: Read: 2, transferred: 0
    Debug.WriteLine($", Read: {res.Status}, transferred: {res.BytesTransferred}");

    // For most devices, success should be when you can write and read
    // Now, this can be adjusted with just read or write depending on the
    // device you are looking for
    isDevice &= res.Status == I2cTransferStatus.FullTransfer;
    Debug.WriteLine($"0x{i:X2} - {(isDevice ? "Present" : "Absent")}");

    // Just force to dispose so we can use the next one
    i2c.Dispose();
}

Thread.Sleep(Timeout.Infinite);
