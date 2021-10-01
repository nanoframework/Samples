# Windows.Devices.Pwm sample

Please prefer to use [System.Device.Pwm](https://github.com/nanoframework/System.Device.Pwm). This is going o be deprecated. Sample available [here](../System.Device.Pwm).

Shows how to use the [Windows.Devices.Pwm](http://docs.nanoframework.net/api/Windows.Devices.Pwm.html) API to drive a GPIO pin with a PWM signal. This pin is connected to an LED thus increasing and decreasing its light intensity periodically.

## Hardware requirements

Any hardware device running a nanoFramework image. This example was coded to use an STM32F769I-DISCOVERY board.

## Related topics

### Reference

- [Windows.Devices.Pwm](http://docs.nanoframework.net/api/Windows.Devices.Pwm.html)

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
