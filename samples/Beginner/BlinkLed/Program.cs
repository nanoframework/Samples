// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

Debug.WriteLine("Hello from nanoFramework!");

// Setup the GPIO pin to 2 as it is the embedded led in the ESP32
// We create a controller and open the pin in output mode
// If your board has another pin, change here. If you are using an external led, change here as well.
GpioPin led = new GpioController().OpenPin(2, PinMode.Output);

while (true)
{
    // Turn on the LED
    led.Write(PinValue.High);
    Thread.Sleep(500);
    // Turn off the LED
    led.Write(PinValue.Low);
    Thread.Sleep(500);
}

Thread.Sleep(Timeout.Infinite);
