# 🌶️🌶️🌶️ - Complete Azure MQTT sample using BMP280 sensor **with** Azure lib and deep sleep

Shows how to use the Azure IoT SDK library measuring a BMP280 sensor. This leverages the [nanoFramework.IoT.Device](https://github.com/nanoframework/nanoFramework.IoT.Device) repository and [Azure SDK](https://github.com/nanoframework/nanoFramework.Azure.Devices).

The sample is [located here](./).

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image. 
This code has been tested with ESP32 boards. This can be replaced by any other board with networking capabilities.

The sample uses wifi, this part can be as well replaced with an ethernet connection. It will work the exact same way.

> [!Important]
>
> In this sample, if the connection or something is not setup properly you won't be able to debug properly. Please make sure you are using another sample to understand how Azure Client with a sample and deep sleep is working before using this one.

### Reference

- [nanoFramework.IoT.Device Bmxx80 devices](https://github.com/nanoframework/nanoFramework.IoT.Device/tree/develop/devices/Bmxx80).
- [Azure IoT documentation for MQTT](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-mqtt-support).
- [nanoFramework Network helpers](https://github.com/nanoframework/System.Device.Wifi).
- [nanoFramework Azure IoT SDK](https://github.com/nanoframework/nanoFramework.Azure.Devices).

## Build the sample

1. Simply adjust the device IoT Hub elements and your network.
2. Start Microsoft Visual Studio 2019 by opening the solution.
3. Make sure you have your ESP32 showing up in the Device Explorer window.
4. Press `Ctrl+Shift+B`, or select **Build** \> **Build Solution**.

## Run the sample

> [!Important]
>
> You can debug this sample only for one cycle as the device will very quickly go to sleep. If you want to debug, comment the part where it goes to sleep and replace with and infinite thread sleep timeout or a loop to once of the previous part of the code.
>
> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
>
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
