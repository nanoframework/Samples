# 🌶️ - 1-Wire sample

Shows how to use the [nanoFramework.Device.OneWire](http://docs.nanoframework.net/api/nanoFramework.Device.OneWire.html) API to read and write from/to a 1-Wire device.

The sample is [located here](./).

## Hardware requirements

- A .NET nanoFramework target with 1-Wire capability enabled in the firmware for example a STM32F407_DISCO or any of the ESP32 targets.

**Important**: make sure you properly setup the UART2 pins for ESP32 before creating the `OneWireHost`. For that you have add a reference to `nanoFramework.Hardware.ESP32` NuGet.

```csharp
///////////////////////////////////////////////////////////////////////////
// when connecting to an ESP32 device, need to configure the GPIOs for
// the COM port being used for 1-Wire.
// In .NET nanoFramework official images that's COM3.
//Configuration.SetPinFunction(21, DeviceFunction.COM3_RX);
//Configuration.SetPinFunction(22, DeviceFunction.COM3_TX);
```

In .NET nanoFramework official firmware builds COM3 it's used for 1-Wire host.
For other devices like STM32, please make sure you're using the pre-set pins for One Wire. For STM32 devices the default it's at port C pin 10.

The code sample is demonstrative of the use of the 1-Wire API.

If you have another type of device supporting 1-Wire, you will have to adjust the pin.

## Related topics

### Reference

- [nanoFramework.Device.OneWire](http://docs.nanoframework.net/api/nanoFramework.Device.OneWire.html)
- [1-Wire Protocol](https://en.wikipedia.org/wiki/1-Wire)

## Build the sample

1. Start Microsoft Visual Studio 2022 (VS 2019 should be OK too) and select `File > Open > Project/Solution`.
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
