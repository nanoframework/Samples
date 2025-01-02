# 🌶️ - PWM to drive a servo motor

Shows how to use the [System.Device.Pwm](https://docs.nanoframework.net/api/System.Device.Pwm.html) API to use Pulse Width Modulation pins.

We will use a servo motor for this purpose. The schema is the following and pin 21 will be used. You can change the pin and adjust accordingly the pin:

![schema servo](https://github.com/nanoframework/nanoFramework.IoT.Device/raw/develop/devices/ServoMotor/ServoMotor.png)

And if you want to know more about PWM, how this works, you can read the [All what you've always wanted to know about PWM](https://docs.nanoframework.net/content/getting-started-guides/pwm-explained.html) content!

## Running the sample

Ensure you have all the [software requirements](../README.md#software-requirements).

To build the sample, follow the section [here](../README.md#build-the-sample). And to run it, [here](../README.md#run-the-sample).

The sample is [located here](./Program.cs). The code is very straightforward with the explanations:

```csharp
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
```

And as a result, you will see:

![pwm led](../Docs/servo.gif)

If you want to debug, follow the instructions [explained in the led sample](../BlinkLed//README.md#debugging).
