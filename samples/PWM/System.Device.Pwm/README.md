# 🌶️ - System.Device.Pwm

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

## Hardware requirements

Any hardware device running a nanoFramework image. This example was coded to use an STM32F769I-DISCOVERY board.

## Related topics

### Reference

- [System.Device.Pwm](http://docs.nanoframework.net/api/System.Device.Pwm.html)

## Build the sample

1. Start Microsoft Visual Studio 2019 (VS 2017 should be OK too) and select `File > Open > Project/Solution`.
1. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
1. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
