# 🌶️🌶️ to 🌶️🌶️🌶️ - Azure SDK sample pack

These samples are using [Azure IoT Hub SDK](https://github.com/nanoframework/nanoFramework.Azure.Devices) to connect to Azure IoT Hub. For more information about the library you can check [nanoFramework Azure.Devices.Client Library repository](https://github.com/nanoframework/nanoFramework.Azure.Devices)

## Samples

* [🌶️🌶️ -  Azure IoT Hub SDK with MQTT protocol](AzureSDK)
* [🌶️🌶️🌶️ -  Complete Azure MQTT sample using BMP280 sensor](AzureSDKSleepBMP280)
* [🌶️🌶️ -  Azure IoT Device Provisioning Service (DPS) example](DpsSampleApp)
* [🌶️🌶️🌶️ -  Azure SDK with X.509 CA Signed certificate sample using BMP280 Sensor](AzureSDKSensorCertificate)

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
