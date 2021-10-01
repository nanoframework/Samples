# Hardware ESP32 Deep sleep sample

Shows how to use the [nanoFramework.Hardware.Esp32](http://docs.nanoframework.net/api/nanoFramework.Hardware.Esp32.html) API to access the ESP32 specific functions.

This example shows how to use change default pins for devices and to use the Sleep methods in the nanoFramework.Hardware.Esp32 nuget package.
Putting the ESP32 into Deep sleep mode and waking it up after a set period plus other examples.

## Hardware requirements

Any ESP32 hardware device running a nanoFramework image.

## Related topics

### Reference

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
