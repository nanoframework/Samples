// Copyright (c) 2024 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.

using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using Iot.Device.Button;

Debug.WriteLine("Hello from nanoFramework!");

// Create a GpioController, then create a GpioPin for the led and the button
var gpio = new GpioController();

// Setup the GPIO pin to 2 as it is the embedded led in the ESP32
// Open the pin in output mode
// If your board has another pin, change here. If you are using an external led, change here as well.
GpioPin led = gpio.OpenPin(2, PinMode.Output);

// Initialize a new button with the corresponding button pin
// You can adjust the pin number based on the pin you are using
// As for the simple button sample, it is using pull up by default
GpioButton button = new GpioButton(buttonPin: 25);

Debug.WriteLine("Button is initialized, starting to read state");

// Enable or disable holding or doublepress events
button.IsDoublePressEnabled = true;
button.IsHoldingEnabled = true;

// Write to debug if the button is down
button.ButtonDown += (sender, e) =>
{
    Debug.WriteLine($"buttondown IsPressed={button.IsPressed}");
    led.Write(PinValue.High);
};

// Write to debug if the button is up
button.ButtonUp += (sender, e) =>
{
    Debug.WriteLine($"buttonup IsPressed={button.IsPressed}");
    led.Write(PinValue.Low);
};

// Write to debug if the button is pressed
button.Press += (sender, e) =>
{
    Debug.WriteLine($"Press");
};

// Write to debug if the button is double pressed
button.DoublePress += (sender, e) =>
{
    Debug.WriteLine($"Double press");
};

// Write to debug if the button is held and released
button.Holding += (sender, e) =>
{
    switch (e.HoldingState)
    {
        case ButtonHoldingState.Started:
            Debug.WriteLine($"Holding Started");
            break;
        case ButtonHoldingState.Completed:
            Debug.WriteLine($"Holding Completed");
            break;
    }
};

Thread.Sleep(Timeout.Infinite);
