// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Am2320;
using nanoFramework.Hardware.Esp32;
using System.Device.I2c;
using System.Diagnostics;
using System.Threading;

Debug.WriteLine("Hello from AM2320!");

//////////////////////////////////////////////////////////////////////
// when connecting to an ESP32 device, need to configure the I2C GPIOs
// used for the bus
Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);
Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);

// If you are using another I2C temperature and humidity sensor, you will have to adjust the name of the class.
// You will also have to adjust the I2C address if it is different.
using Am2320 am2330 = new(new I2cDevice(new I2cConnectionSettings(1, Am2320.DefaultI2cAddress, I2cBusSpeed.StandardMode)));

while (true)
{
    // Most temperature and humidity sensors have a temperature and humidity property.
    var temp = am2330.Temperature;
    var hum = am2330.Humidity;

    // In most of the cases, you will have to check if the last read was successful.
    // It happens that a read fails because of a timing issue for example.
    if (am2330.IsLastReadSuccessful)
    {
        Debug.WriteLine($"Temp = {temp.DegreesCelsius} C, Hum = {hum.Percent} %");
    }
    else
    {
        Debug.WriteLine("Not sucessfull read");
    }

    // And it's important to wait a little bit before reading again.
    // All sensors have a limit, it's important to check the datasheet.
    // In our case, we usually have a property like this one.
    Thread.Sleep(Am2320.MinimumReadPeriod);
}
