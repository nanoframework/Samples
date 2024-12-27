// Copyright (c) 2024 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.

using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

Debug.WriteLine("Hello from nanoFramework!");

// Create a GpioController, then create a GpioPin for the led and the button
var gpio = new GpioController();

// Setup the GPIO pin to 2 as it is the embedded led in the ESP32
// Open the pin in output mode
// If your board has another pin, change here. If you are using an external led, change here as well.
GpioPin led = gpio.OpenPin(2, PinMode.Output);

// Create the button with input pull up mode
// Adjust the pin number if needed
GpioPin button = gpio.OpenPin(25, PinMode.InputPullUp);

// We want to set an event on the pin when the button is pressed or released
button.ValueChanged += (s, e) =>
{
    // When we press the button, the as we have the pin connected to a pull up, the initial value is high,
    // and when we press it, the value goes to the ground. So, the even is falling when we press.
    Debug.WriteLine($"Button is {(e.ChangeType == PinEventTypes.Falling ? "pressed" : "released")}");
    if (e.ChangeType == PinEventTypes.Falling)
    {
        // light the led
        led.Write(PinValue.High);
    }
    else
    {
        // switch off the led
        led.Write(PinValue.Low);
    }
};

Thread.Sleep(Timeout.Infinite);
