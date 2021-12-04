# Bluetooth Low Energy Serial profile sample

Shows how to use the Serial Protocol Profile APIs included in the [NanoFramework.Device.Bluetooth](http://docs.nanoframework.net/api/NanoFramework.Device.Bluetooth.html).   

The sample allows a Bluetooth Terminal program to connect and send/receive text messages. There are a number of applications on both Android and IOS
that will work.

The device should be seen as "nanoFrameworkSerial" when scanning for the device.

Send device 'help' to get information on what messages it will respond to.

This sample is a example of what you could do to provision your device with required data like wifi credentials, name etc. 

## Hardware requirements

A ESP32 device running a nanoFramework image with the Bluetooth enabled.

Currently there are 2 firmware images that support Bluetooth:-

- ESP32_BLE_REV0
- ESP32_BLE_REV3

## Related topics

### Reference

- [NanoFramework.Device.Bluetooth](http://docs.nanoframework.net/api/NanoFramework.Device.Bluetooth.html)

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
