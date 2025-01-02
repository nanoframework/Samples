// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Device.Pwm;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Hardware.Esp32;

Debug.WriteLine("Hello from nanoFramework!");

// Pin 2 is the integrated LED in the ESP32, if you are using another board, change the pin number
int pinLed = 2;

// If you have an ESP32, you should setup the pin first to be used with PWM:
Configuration.SetPinFunction(pinLed, DeviceFunction.PWM1);

// Then you can create the PWM Channel from the pin:
PwmChannel pwmPin = PwmChannel.CreateFromPin(pinLed, 40000, 0);
// Note: even if possible, it is not recommended to adjust the frequency once created.

// Start the PWM
pwmPin.Start();

// Now, we will loop to increase and decrease the duty cycle
while (true)
{
    // We will increase the duty cycle by 1 percent every 20 milliseconds
    for (int i = 0; i <= 100; i++)
    {
        pwmPin.DutyCycle = (double)i / 100;
        Thread.Sleep(20);
    }

    // We will decrease the duty cycle by 1 percent every 20 milliseconds
    for (int i = 100; i >= 0; i--)
    {
        pwmPin.DutyCycle = (double)i / 100;
        Thread.Sleep(20);
    }
}

Thread.Sleep(Timeout.Infinite);

