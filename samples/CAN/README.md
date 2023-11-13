# 🌶️ - CAN sample

Shows how to use the [nanoFramework.Device.Can](http://docs.nanoframework.net/api/nanoFramework.Device.Can.html) API to read and write from/to a Can device.

The sample is [located here](./Can.TestApp/Program.cs).

## Hardware requirements

- An [STM32F407 DISCOVERY](https://www.st.com/en/evaluation-tools/stm32f4discovery.html) board.
- A CAN bus wiring

The code sample is demonstrative of the use of the CAN API.

## Related topics

### Reference

- [nanoFramework.Device.Can](http://docs.nanoframework.net/api/nanoFramework.Device.Can.html)
- [CAN bus explained](https://www.csselectronics.com/screen/page/simple-intro-to-can-bus/language/en) by CSS Electronics.

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
