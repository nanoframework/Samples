# 1-Wire sample

Shows how to use the [nanoFramework.Devices.OneWire](http://docs.nanoframework.net/api/nanoFramework.Devices.OneWire.html) API to read and write from/to a 1-Wire device.

## Hardware requirements

- An [STM32F407 DISCOVERY](https://www.st.com/en/evaluation-tools/stm32f4discovery.html) board.
- An 1-Wire device connected to GPIO PC10 and ground.

The code sample is demonstrative of the use of the 1-Wire API.

If you have another type of device supporting 1-Wire, you will have to adjust the pin.

## Related topics

### Reference

- [nanoFramework.Devices.OneWire](http://docs.nanoframework.net/api/nanoFramework.Devices.OneWire.html)
- [1-Wire Protocol](https://en.wikipedia.org/wiki/1-Wire)

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
