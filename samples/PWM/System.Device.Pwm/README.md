# System.Device.Pwm

Shows how to use the [System.Devices.Pwm](https://github.com/nanoframework/System.Device.Pwm) API to drive a GPIO pin with a PWM signal. This pin is connected to an LED thus increasing and decreasing its light intensity periodically.

PWM are traditionally used to drive servo motors, motor speed, led intensity and used for clocks as well.

## ESP32 case

You will have first to setup the pin function. Install the `nanoFramework.Hardware.Esp32` nuget:

```csharp
Configuration.SetPinFunction(18, DeviceFunction.PWM1);
PwmChannel pwmPin = PwmChannel.CreateFromPin(18, 40000, 0);
```

Once the pin setup, you can create from the pin a PWM Channel. PWM0 to PWM7 are lox resolution PWM, 8 to 15 are high resolution. PWM comes by pair and share the same frequency. So if you need PWM with different frequencies, you need to setup pins with at least 2 of difference in the number.

Note: `pwmPin` will be null in case the pin hasn't been reserved.

## STM32 case

For STM32, pins which can support PWM are marked in the documentation with TIM. Make sure the pin you want to use is TIM enable and make sure it is not reserved for another usage.

```csharp
PwmChannel pwmPin = PwmChannel.CreateFromPin(18, 40000, 0);
```

This will create a PWM channel if the pin is TIM enabled an not reserved for something else.

Note: `pwmPin` will be null in case the PWM can't be created.
