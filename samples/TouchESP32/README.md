# 🌶️ - Touch pad ESP32 sample

Shows how to use Touch Pad for ESP32 and ESP32-S2. A Touch Pad is a pin that is sensitive to touch. It can be just a wire, a surface made out of various materials.

The sample is [located here](./Program.cs).

## Hardware requirements

Either an ESP32 or an ESP32-S2. Other series do not support Touch Pad.

## Samples provided

The sample contains different functions that you can call to understand how things work. The code is commented.

> Important: some elements are ESP32 specific and some other S2 specific. Be careful when using those.

## Build the sample

1. Start Microsoft Visual Studio 2019/2022 and select `File > Open > Project/Solution`.
1. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
1. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

You need to select the sample you want to run. If you want to use wifi, you must include a reference to the `System.Device.Wifi` package, define the HAS_WIFI variable and set your wifi parameters in the code.

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
