# 🌶️ to 🌶️🌶️ - System.IO.Ports serial Communication sample

Shows how to use the [System.IO.Ports](http://docs.nanoframework.net/api/System.IO.Ports.html) API to send/receive data using an UART (COM port).

This sample allows the user to configure and communicate with a Serial device over an UART (COM port). You can choose one of four scenarios:

- Configure the Serial device
- Send data over the Output stream
- Receive data from the Input stream
- Register for Events on the Serial device

The sample is [located here](./).

## Scenarios

### Configure the Serial device

This scenario demonstrates the how to instantiate a Serial Device and use of various Get/Set property APIs in order to query for/alter Serial device properties such as Baud Rate, Stop Bits etc.

When working with ESP32 edit the nfproj file and add `BUIID_FOR_ESP32` to the DefineConstants, like this:

```text
<DefineConstants>$(DefineConstants);BUIID_FOR_ESP32;</DefineConstants>
```

### Send data over the output stream

This scenario demonstrates the use of Output stream on the SerialDevice object in order to send data to a Serial device.

### Receive data from the Input stream

This scenario demonstrates the use of Input stream on the SerialDevice object in order to read data from a Serial device.

### Register for Events on the Serial device

This scenario demonstrates the use of event notification APIs provided by System.IO.Ports for **Data Received** event type, an subsequently read the available data from the Input stream.

## Hardware requirements

Any hardware device running a nanoFramework image built with serial communication enabled.
This sample is coded to use the STM32F769IDiscovery target board, but can be easily changed to any other target that features a serial port.

## Related topics

### Reference

- [System.IO.Ports](http://docs.nanoframework.net/api/System.IO.Ports.html)

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
