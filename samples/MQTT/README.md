# 🌶️🌶️ to 🌶️🌶️🌶️ - MQTT sample pack

The M2Mqtt library provides a main class `MqttClient` that represents the MQTT client to connect to a broker. You can connect to the broker providing its IP address or host name and optionally some parameters related to MQTT protocol.

After connecting to the broker you can use `Publish()` method to publish a message to a topic and `Subscribe()` method to subscribe to a topic and receive message published on it. The `MqttClient` class is events based so that you receive an event when a message is published to a topic you subscribed to. You can receive event when a message publishing is complete, you have subscribed or unsubscribed to a topic.

For more details, see the comments inside the samples.

| Sample | Description |
|---|---|
|[🌶️🌶️ - BasicExample.Ethernet](./BasicExample.Ethernet)|The most basic usage, without authentication or certificates. Ready to run on Ethernet-based boards.|
|[🌶️🌶️ -BasicExample.Wifi](./BasicExample.WiFi)|The most basic usage, without authentication or certificates. Ready to run on Wifi-based boards.|
|[🌶️🌶️🌶️ - AdvancedExample.Certificates](./AdvancedExample.Certificates)|Basic usage, but uses secure connection and certificate-based authorization.|
|[🌶️🌶️🌶️ - AdvancedExample.Aws](./AdvancedExample.Aws)|Very advanced sample of how to use MQTT in AWS. Requires having an AWS account.|
|[🌶️🌶️🌶️ - AdvancedExample.Azure](./AdvancedExample.Azure)|Very advanced sample of how to use MQTT in Azure. Requires having an Azure account.|

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image.
These samples have been tested with SMT32F769IDISCOVERY, ESP-WROVER-KIT and ESP32 Pico boards.

## Related topics

### Reference

- See API documentation [here](https://docs.nanoframework.net/api/nanoFramework.M2Mqtt.html).

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
