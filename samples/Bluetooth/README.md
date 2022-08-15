# Bluetooth samples

These samples are using [nanoFramework.Device.Bluetooth](https://github.com/nanoframework/nanoFramework.Device.Bluetooth) to work with the nimble BLE implementation on the ESP32 device. For more information about the library you can check [nanoFrameworkDevice.Bluetooth Library repository](https://github.com/nanoframework/nanoFramework.Device.Bluetooth) and the API documentation.

## Hardware requirements

Currently only support on ESP32 devices running either the ESP32_BLE_REV0, ESP32_PICO or ESP32_BLE_REV3 firmware.
On other firmware versions a not supported exception will be returned.

## Samples
* [Bluetooth Low energy Improv sample](ImprovWifi)
Provision device directly from a web page using *Improv* standard.
See sample readme for more information.

* [Bluetooth Low energy serial](BluetoothLESerial)
Shows how to use the built-in SSP(Serial Service Profile) which simulates a serial link over Bluetooth. Use a phone app. 
such as "Serial Bluetooth Terminal" to connect to device and send and receive messages.

* [Bluetooth Low energy sample 1](BluetoothLESample1)
This shows how to create a custom service which shows the use of:

| Custom service | Behaviour |
| --- | --- |
| Read static value (value that doesn't change) | Value text |
| Read a dynamic value using an event handler. | value 3 bytes (Hour/Minute/Seconds) |
| Notifying clients of a changed value | Notify time every 60 seconds or when date updated. |
| Read and Write a value | Read/Write 3 bytes RGB |

You will be able to connect to the service and read values or subscribe to be Notified ever 60 seconds.
Suitable Phone apps: "LightBlue" or "nRF Connect"

* [Bluetooth Low energy sample 2](BluetoothLESample2)
This sample adds security to the Characteristic access. This will force the Server/Client to bond/pair which is 
used to generate key pairs for communications. All access is now encrypted. 

| Custom service | Behaviour |
| --- | --- |
| Read and Write (requires encryption) a value | Read/Write Int32 |
| Read (requires encryption) and Write a value | Read/Write Int32 (same value) |

The 1st Characteristic allows the read but the write requires it to be paired.
The 2nd Characteristic allows writes but requires to be paired for the read.

* [Bluetooth Low energy sample 3](BluetoothLESample3)
This show cases the use of adding extra services to main service or replacing an existing service 
like the default "Device Information Service". 

This sample also includes some standard Bluetooth services as separate classes which may be useful 
for any Bluetooth LE project.

### Device Information Service 

Provides device information like Manufacturer, model, software version etc.

### Battery Level Service

Publishes the current battery level as a percentage.

### Current Time Service

Publishes the current date/time of device and optionally allows the date/time to be set on device.

### Environmental Sensor service

This allows multiple environmental sensors to be published such as Temperature, Humidity, Pressure, Rainfall.
This sample class includes these 4 but other types can easily added to class. Multiple sensor of same type can be added.
The sample shows 3 Temperatures (Instantaneous, Maximum, Minimum) added to service and a humidity sensor.

* [Simple Watcher sample](Central1)

This a simple sample showing how to scan for Bluetooth LE devices.

* [Central Data Collect sample](Central2)

Sample will scan for devices with a name starting with "Sample" and connect to all found devices.
It will then read and set-up notifications for changes in Environmental Service temperatures.

The [Bluetooth Low energy sample 2](BluetoothLESample2) was changed for this purpose so that the temperatures are
always changing up and down.

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
