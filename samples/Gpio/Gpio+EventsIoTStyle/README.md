# 🌶️ - GPIO and events sample (.NET IoT style)

Shows how to use the [System.Device.Gpio](http://docs.nanoframework.net/api/System.Device.Gpio.html) API allowing your to set and read the state of GPIO pins.
This sample is using the .NET IoT API style.

- set the state of output GPIO pins
- read state of input GPIO pins
- setup handlers to react to GPIO state change events on external stimulus

The sample is [located here](./Program.cs).

## Hardware requirements

Any hardware device running a nanoFramework image built with GPIO support (.NET IoT) enabled.

### Reference

[System.Device.Gpio](http://docs.nanoframework.net/api/System.Device.Gpio.html)

## System requirements

**Client:** Windows 10

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
