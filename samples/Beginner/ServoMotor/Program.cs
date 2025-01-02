// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Device.Pwm;
using System.Diagnostics;
using System.Threading;
using Iot.Device.ServoMotor;
using nanoFramework.Hardware.Esp32;

Debug.WriteLine("Hello from nanoFramework!");

// We are using pin 21, you can adjust the pin number based on the pin you are using
int pinServo = 21;

// When using an ESP32, you have to setup the pin function then create the PWM channel
Configuration.SetPinFunction(pinServo, DeviceFunction.PWM1);

// Each servo motor has specific pulse width limits, check the datasheet of the servo motor for the values
using PwmChannel pwmChannel = PwmChannel.CreateFromPin(pinServo, 50);
ServoMotor servoMotor = new ServoMotor(
    pwmChannel,
    180,
    900,
    2100);

servoMotor.Start();  // Enable control signal.

for (int i = 0; i < 10; i++)
{
    // Move position.
    // ~0.9ms; Approximately 0 degrees.
    servoMotor.WriteAngle(0);
    Thread.Sleep(1000);
    // ~1.5ms; Approximately 90 degrees.
    servoMotor.WriteAngle(90);
    Thread.Sleep(1000);
    // ~2.1ms; Approximately 180 degrees.
    servoMotor.WriteAngle(180);
    Thread.Sleep(1000);
}

servoMotor.Stop(); // Disable control signal.

Thread.Sleep(Timeout.Infinite);
