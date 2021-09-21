//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

// Uncomment if you are using an ESP32 and install the nuget:
// using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Pwm;
using System.Diagnostics;
using System.Threading;

Debug.WriteLine("Hello from Pwm!");

bool goingUp = true;
float dutyCycle = .00f;

// If you have an ESP32, you should setup the pin first, uncomment the next line:
// Configuration.SetPinFunction(18, DeviceFunction.PWM1);
// Note: if you have a STM32 board, you have to make sure your pin can use PWM (TIM)
// Then you can create the PWM Channel from the pin:
PwmChannel pwmPin = PwmChannel.CreateFromPin(18, 40000, 0);
// Note: even if possible, it is not recommended to adjust the frequency once created.

// Advance way of creating a PWM Channel.
// In this case you need to understand the chip/PWM/TIM to use and the channel/pin:
// PwmChannel pwmPin = new(1, 2, 40000, 0.5);

// Start the PWM
pwmPin.Start();

while(true)
{
    if (goingUp)
    {
        // slowly increase light intensity
        dutyCycle += 0.05f;

        // change direction if reaching maximum duty cycle (100%)
        if (dutyCycle > .95) goingUp = !goingUp;
    }
    else
    {
        // slowly decrease light intensity
        dutyCycle -= 0.05f;

        // change direction if reaching minimum duty cycle (0%)
        if (dutyCycle < 0.10) goingUp = !goingUp;
    }

    // update duty cycle
    pwmPin.DutyCycle = dutyCycle;

    Thread.Sleep(50);
}

// Stop the PWM:
pwmPin.Stop();

Thread.Sleep(Timeout.Infinite);
