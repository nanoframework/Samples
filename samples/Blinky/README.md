# 🌶️ - Blink your first led

Shows how to use the [System.Device.Gpio](http://docs.nanoframework.net/api/System.Device.Gpio.html) API to set GPIO pins.

The sample is [located here](./Blinky/Program.cs).

## Hardware requirements

Any hardware device running a nanoFramework image.

Some boards like most of the ESP32 dev kits and most of the STM32 boards have an embedded led. You can use it to test this code. You just need to understand the GPIO number and use if in this sample.

## Related topics

### Reference

- [System.Device.Gpio](http://docs.nanoframework.net/api/System.Device.Gpio.html)

## Build the sample

1. Start Microsoft Visual Studio 2022 or Visual Studio 2019 (Visual Studio 2017 should be OK too) and select `File > Open > Project/Solution`.
1. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
1. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> [!NOTE]
>
> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
>
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
