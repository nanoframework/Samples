# Bluetooth samples

These samples are using [nanoFramework.Device.Bluetooth](https://github.com/nanoframework/nanoFramework.Device.Bluetooth) to work with the nimble BLE implementation on the ESP32 device. For more information about the library you can check [nanoFrameworkDevice.Bluetooth Library repository](https://github.com/nanoframework/nanoFramework.Device.Bluetooth) and the API documentation.

## Hardware requirements

Currently only support on ESP32 devices running either the ESP32_BLE_REV0, ESP32_PICO or ESP32_BLE_REV3 firmware.
On other firmware versions a not supported exception will be returned.

## Samples

* [Bluetooth Low energy serial](BluetoothLESerial)
* [ ]()


## Build the sample

1. Start Microsoft Visual Studio 2019 (VS 2017 or VS 2022 should be OK too) and select `File > Open > Project/Solution`.
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
