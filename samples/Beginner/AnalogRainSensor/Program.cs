// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Device.Adc;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Hardware.Esp32;

Debug.WriteLine("Hello from nanoFramework!");

AdcController adc1 = new AdcController();

// Get the reference minimul and maximum values
int max1 = adc1.MaxValue;
int min1 = adc1.MinValue;

Debug.WriteLine("min1=" + min1.ToString() + " max1=" + max1.ToString());

// We will use the pin 34 which is already setup as ADC1_CH6
// If you want to change to use another pin, this function needs to be used to setup the pin
// Note, that not all configurations are possible. You'll have to refer to the ESP32 Technical Reference Manual
// or the board you're using to see which pins can be used as ADC.
// Configuration.SetPinFunction(34, DeviceFunction.ADC1_CH6);

AdcChannel sensor = adc1.OpenChannel(6);

while (true)
{
    // Read the raw value
    int rawValue = sensor.ReadValue();
    // Calculate the voltage
    // The ESP32 is using 3.3 V as the reference voltage
    // The ADC has a 12-bit resolution, so the maximum value is 4095
    // Still, using the AdcController.MaxValue and MinValue is a good practice
    double voltage = ((double)(rawValue - min1) / (max1 - min1)) * 3.3;
    Debug.WriteLine($"Raw Value: {rawValue}, Voltage: {voltage}");

    // Here, it's more empirical, you can adjust to your own needs
    // You can also use directly the raw value to compare
    if (voltage < 1.5)
    {
        Debug.WriteLine("It's wet!");
    }
    else
    {
        Debug.WriteLine("It's dry!");
    }

    Thread.Sleep(1000);
}

Thread.Sleep(Timeout.Infinite);
