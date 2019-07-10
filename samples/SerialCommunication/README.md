# Serial Communication sample

Shows how to use the [Windows.Devices.SerialCommunication](http://docs.nanoframework.net/api/Windows.Devices.SerialCommunication.html) API to send/receive data using an UART (COM port).

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

This sample allows the user to configure and communicate with a Serial device over an UART (COM port). You can choose one of four scenarios:

- Configure the Serial device
- Send data over the Output stream
- Receive data from the Input stream
- Register for Events on the Serial device

## Scenarios

### Configure the Serial device

This scenario demonstrates the how to instantiate a Serial Device and use of various Get/Set property APIs in order to query for/alter Serial device properties such as Baud Rate, Stop Bits etc.

### Send data over the output stream

This scenario demonstrates the use of Output stream on the SerialDevice object in order to send data to a Serial device.

### Receive data from the Input stream

This scenario demonstrates the use of Input stream on the SerialDevice object in order to read data from a Serial device.

### Register for Events on the Serial device

This scenario demonstrates the use of event notification APIs provided by Windows.Devices.SerialCommunication for **Data Received** event type, an subsequently read the available data from the Input stream.

## Hardware requirements

Any hardware device running a nanoFramework image built with serial communication enabled.
This sample is coded to use the STM32F769IDiscovery target board, but can be easily changed to any other target that features a serial port.

## Related topics

### Reference

- [Windows.Devices.SerialCommunication](http://docs.nanoframework.net/api/Windows.Devices.SerialCommunication.html)

## Build the sample

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build. 
2. Start Microsoft Visual Studio 2017 and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
4. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select Build > Deploy Solution.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging.
