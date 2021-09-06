# MQTT sample pack

The M2Mqtt library provides a main class `MqttClient` that represents the MQTT client to connect to a broker. You can connect to the broker providing its IP address or host name and optionally some parameters related to MQTT protocol.

After connecting to the broker you can use `Publish()` method to publish a message to a topic and `Subscribe()` method to subscribe to a topic and receive message published on it. The `MqttClient` class is events based so that you receive an event when a message is published to a topic you subscribed to. You can receive event when a message publishing is complete, you have subscribed or unsubscribed to a topic.

For more details, see the comments inside the samples.


| Sample | Description | 
|---|---|
|[BasicExample.Ethernet](./BasicExample.Ethernet)|The most basic usage, without authentication or certificates. Ready to run on Ethernet-based boards.|
|[BasicExample.WiFi](./BasicExample.WiFi)|The most basic usage, without authentication or certificates. Ready to run on WiFi-based boards.|
|[AdvancedExample.Certificates](./AdvancedExample.Certificate)|Basic usage, but uses secure connection and certificate-based authorization.|
|[AdvancedExample.Aws](./AdvancedExample.Aws)|Very advanced sample of how to use MQTT in AWS. Requires having an AWS account.|
|[AdvancedExample.Azure](./AdvancedExample.Azure)|Very advanced sample of how to use MQTT in Azure. Requires having an Azure account.|


> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image.
These samples have been tested with SMT32F769IDISCOVERY, ESP-WROVER-KIT and ESP32 Pico boards.

## Related topics

### Reference

## Build the sample

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build. 
2. Start Microsoft Visual Studio 2019 (VS 2017 should be OK too) and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
4. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select Build > Deploy Solution.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging.
