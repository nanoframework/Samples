
# HTTP.HttpAzureGET Sample

This sample shows how to use the [NanoFramework.System.Net](https://docs.nanoframework.net/api/System.Net.html) API to GET a message from the Azure IoT Hub. This can also be used as a starting point to understand a HTTP GET.

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/main.zip), but be
> sure to unzip everything to access any shared dependencies.

## Hardware requirements

Any hardware device running a nanoFramework image that supports WiFi.

## Related topics

### References

- [Sample.AMQP](https://github.com/nanoframework/Samples/tree/main/samples/AMQP)
- [Wikipedia X.509 Certificate .pem format](https://en.wikipedia.org/wiki/X.509#Certificate_filename_extensions)

## Using the sample

1. Create an Azure account
2. Create an IoT Hub using your Azure Account
3. Create a Device in your IoT Hub
4. Generate a SAS Token by creating a symmetric key for your IoT Device you created utilizing the primary key of the device. There are many ways to do this. One example using the Azure CLI can be found [here](https://docs.microsoft.com/en-us/cli/azure/ext/azure-iot/iot/hub?view=azure-cli-latest#ext-azure-iot-az-iot-hub-generate-sas-token). A simple way to create this is using the Azure IoT explorer program. 
5. Start Microsoft Visual Studio 2019 and select **File** \> **Open** \> **Project/Solution**.
6. Enter in your SAS Token into Program.cs using the following format "SharedAccessSignature sr=(YourIoTHubName).azure-devices.net%2Fdevices%2F(YourdeviceName)&sig=(YourDeviceSymmetricKey)"
```csharp
 string sas = "<Enter SAS Token Here See Read Me for example>";
```
7. Enter your Wifi Settings in the program.cs file and create a #define BUILD_FOR_ESP32
```csharp
private const string MySsid = "<replace-with-valid-ssid";
private const string MyPassword = "<replace-with-valid-password>";
```
8. Send a message to your device using a service such as Azure Iot Explorer under the Cloud-To-Device tab. Or you can create an Azure Function to send a message
8. Connect a button from the GPIO pin in Program.cs (Pin 0) to ground. When you press the button it will create a connection from Pin 0 to ground and send a message to the cloud.
```charp
// setup user button
_userButton = GpioController.GetDefault().OpenPin(0);
_userButton.SetDriveMode(GpioPinDriveMode.Input);
_userButton.ValueChanged += UserButton_ValueChanged;
```

## Build the sample

1. Starting in the folder where you unzipped the samples, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
2. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.
3. Make sure to click on device explorer and select your device.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select Build > Deploy Solution.**
**
### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging.

## Additional Info

- Since this is an embedded device with the NanoFramework installed certificates are not prebuilt into the device to make a HTTPS connection.

- Therefore if you want to make a HTTPS POST or GET you have to use your own certificates or use ones that the website already has. That is why this example uses the .pem file from azure.

- For another example of using a certificate file look at the HTTPWebRequest sample.
