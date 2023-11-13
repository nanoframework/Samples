# Azure IoT Device Provisioning Service (DPS) example

You will find a sample to show how to connect to Azure IoT Device Provisioning Service (DPS). DPS allows you to provision devices individually, by group with authentication with SAS token or X.509 certificates. Those 4 scenarios are fully supported and described below.

**IMPORTANT:** please refer to the DPS documentation to understand how to create each provisioning. Uncomment the provisioning type you want to use.

The documentation can be found [here](https://docs.microsoft.com/en-us/azure/iot-dps/).

Refer to the main [.NET nanoFramework SDK](https://github.com/nanoframework/nanoFramework.Azure.Devices) to understand the usage.

The sample is [located here](./).

## Usage

The device **must** be connected to the Internet and have a valid date and time.

### Provisioning using symmetric key

For symmetric key provisioning you need the following elements:

- A registration ID
- The ID Scope
- The device name
- The key or the derived key for group provisioning

The code is then straight forward:

```csharp
const string RegistrationID = "nanoDPStTest";
const string DpsAddress = "global.azure-devices-provisioning.net";
const string IdScope = "0ne01234567";
const string SasKey = "alongkeyencodedbase64";

// See the previous sections in the SDK help, you either need to have the Azure certificate embedded
// Either passing it in the constructor
X509Certificate azureCA = new X509Certificate(DpsSampleApp.Resources.GetBytes(DpsSampleApp.Resources.BinaryResources.BaltimoreRootCA_crt));
var provisioning = ProvisioningDeviceClient.Create(DpsAddress, IdScope, RegistrationID, SasKey, azureCA);
var myDevice = provisioning.Register(new CancellationTokenSource(60000).Token);

if(myDevice.Status != ProvisioningRegistrationStatusType.Assigned)
{
    Debug.WriteLine($"Registration is not assigned: {myDevice.Status}, error message: {myDevice.ErrorMessage}");
    return;
}

// You can then create the device
var device = new DeviceClient(myDevice.AssignedHub, myDevice.DeviceId, SasKey, nanoFramework.M2Mqtt.Messages.MqttQoSLevel.AtMostOnce, azureCA);

// Open it and continue like for the previous sections
var res = device.Open();
if(!res)
{
    Debug.WriteLine($"can't open the device");
    return;
}
```

Note: like for the `DeviceClient` you need to make sure you are connected to a network and that the system has a valid date and time.

### Provisioning using X.509 certificates

For provisioning using a X.509 certificate you need the following elements:

- A registration ID
- The ID Scope
- The device name
- A X.509 device certificate
- Make sure that your IoT Hub is as well aware of the root/intermediate certificate you are using otherwise you won't be able to connect to your IoT Hub once your device is provisioned

The code is then straight forward:

```csharp
const string RegistrationID = "nanoCertTest";
const string DpsAddress = "global.azure-devices-provisioning.net";
const string IdScope = "0ne0034F11A";

const string cert = @"
-----BEGIN CERTIFICATE-----
Your certificate
-----END CERTIFICATE-----
";

const string privateKey = @"
-----BEGIN ENCRYPTED PRIVATE KEY-----
the encrypted private key
-----END ENCRYPTED PRIVATE KEY-----
";

// See the previous sections in the SDK help, you either need to have the Azure certificate embedded
// Either passing it in the constructor
X509Certificate azureCA = new X509Certificate(DpsSampleApp.Resources.GetBytes(DpsSampleApp.Resources.BinaryResources.BaltimoreRootCA_crt));

// Note: if the private key is not encrypted with a password, use an empty string for the password parameter
// You can as well store your certificate directly in the device certificate store
// And you can store it as a resource as well if needed
X509Certificate2 deviceCert = new X509Certificate2(cert, privateKey, "1234");

var provisioning = ProvisioningDeviceClient.Create(DpsAddress, IdScope, RegistrationID, deviceCert, azureCA);
var myDevice = provisioning.Register(new CancellationTokenSource(60000).Token);

if(myDevice.Status != ProvisioningRegistrationStatusType.Assigned)
{
    Debug.WriteLine($"Registration is not assigned: {myDevice.Status}, error message: {myDevice.ErrorMessage}");
    return;
}

// You can then create the device
var device = new DeviceClient(myDevice.AssignedHub, myDevice.DeviceId, deviceCert, nanoFramework.M2Mqtt.Messages.MqttQoSLevel.AtMostOnce, azureCA);
// Open it and continue like for the previous sections
var res = device.Open();
if(!res)
{
    Debug.WriteLine($"can't open the device");
    return;
}
```

### Additional payload

Additional payload is supported as well. You can set it up as as json string in the `ProvisioningRegistrationAdditionalData` class when calling the `Register` function. When the device has been provisioned, you may have as well additional payload provided.
