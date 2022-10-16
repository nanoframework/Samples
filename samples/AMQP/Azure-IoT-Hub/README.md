# 🌶️🌶️ -Azure AMQP sample

Shows how to use AMQP.Net Lite library with Azure.

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image. This code has been tested with a SMT32F769IDISCOVERY board.

## TLS connection to Azure IoT Hub

To use a secured (TLS) connection to Azure IoT Hub the following is required:

1. Upload the root CA certificate for Azure. That's the "Baltimore CyberTrust Root". You can download it from [here](https://docs.microsoft.com/en-us/azure/security/fundamentals/tls-certificate-changes#what-is-changing). It should be uploaded to the device using the Network Configuration dialog from Device Explorer.

1. Set the `Connection.DisableServerCertValidation` to `false`.

1. Generate an SAS Token for your device. [Azure IoT Explorer](https://github.com/Azure/azure-iot-explorer) it's a convenient tool to do this for you.
After setting up the access to you IoT Hub, navigate to the device, enter the desired parameters and generate the SAS Token (see the print screen below).
![](azure-iot-explorer-sas.png) 

### Reference

- [AMQP API](http://azure.github.io/amqpnetlite/api/Amqp.html)

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
