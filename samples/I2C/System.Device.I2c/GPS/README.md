# 🌶️🌶️ - I2C GPS sample

Shows how to use the [System.Device.I2c](http://docs.nanoframework.net/api/System.Device.I2c.html) API to read and write from/to an I2C device.
In this sample we'll be using a [IES-SHIELD-GPS](https://i-groupuk.com/downloads/global-positioning-system-gps-shield-for-arduino/) GPS controller mounted on a [STM32F769I DISCOVERY](http://www.st.com/en/evaluation-tools/32f769idiscovery.html) board.

The sample project includes a full driver for the IES-SHIELD-GPS and IES-SHIELD-GPAM allowing you to retrieve the current date, time, longitude, latitude, heading, speed, (GPAM only: pitch and roll) from the device.

The sample is [located here](./nanoframework.Samples.GPS/Program.cs).

## Hardware requirements

An [STM32F769II DISCOVERY](http://www.st.com/en/evaluation-tools/32f769idiscovery.html) board.
The code sample is demonstrative of the use of the I2C API.

## Related topics

### Reference

- [System.Device.I2c](http://docs.nanoframework.net/api/System.Device.I2c.html)

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
