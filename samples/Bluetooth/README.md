# 🌶️ to 🌶️🌶️🌶️ - Bluetooth sample pack

These samples are using [nanoFramework.Device.Bluetooth](https://github.com/nanoframework/nanoFramework.Device.Bluetooth) to work with the nimble BLE implementation on the ESP32 device. For more information about the library you can check [nanoFrameworkDevice.Bluetooth Library repository](https://github.com/nanoframework/nanoFramework.Device.Bluetooth) and the API documentation.

## Hardware requirements

Currently only support on ESP32 devices running either the ESP32_BLE_REV0, ESP32_PICO or ESP32_BLE_REV3 firmware.
On other firmware versions a not supported exception will be returned.

## Server Samples

* [🌶️🌶️🌶️ -  Bluetooth Low energy Improv sample](ImprovWifi)
Provision device directly from a web page using *Improv* standard.
See sample readme for more information.

* [🌶️ -  Bluetooth Low energy serial](BluetoothLESerial)
Shows how to use the built-in SSP(Serial Service Profile) which simulates a serial link over Bluetooth. Use a phone app. 
such as "Serial Bluetooth Terminal" to connect to device and send and receive messages.

* [🌶️🌶️ -  Bluetooth Low energy sample 1](BluetoothLESample1)
This shows how to create a simple bare bones custom service.

* [🌶️🌶️ -  Bluetooth Low energy sample 2](BluetoothLESample2)
This sample adds security to the Characteristic access. This will force the Server/Client to pair which is 
used to generate key pairs for communications. See sample for details. 

* [🌶️ -  Bluetooth Low energy sample 3](BluetoothLESample3)
This show cases the use of adding extra services or replacing an existing service 
like the default "Device Information Service". 

## Central/Client samples

* [🌶️ -  Simple Watcher sample](Central1)

This a simple sample showing how to scan for Bluetooth LE devices.

* [🌶️🌶️ -  Central Data Collect sample](Central2) This Sample will scan for devices with the service UUID used in the BluetoothLESample3 sample and connect to all found devices.
It will then read and set-up notifications for changes in Environmental Service temperatures. The [🌶️ -  Bluetooth Low energy sample 3](BluetoothLESample3) was changed to provide notifications of temperture changes.

* [🌶️🌶️🌶️ -  Demonstrates pairing and authentication in Client program.](Central3) Central/Client sample to show how to watch for advertisments and connect to a device which requires encryption and 
authentication to access the Characteristics values.

## Building the samples

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
