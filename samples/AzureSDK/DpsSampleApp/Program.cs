using nanoFramework.Azure.Devices.Client;
using nanoFramework.Azure.Devices.Provisioning.Client;
using nanoFramework.Azure.Devices.Shared;
using nanoFramework.Networking;
using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

Debug.WriteLine("Hello DPS device for .NET nanoFramework!");

const string Ssid = "yourSSID";
const string Password = "yourWifiPassword";

// One minute unit
int sleepTimeMinutes = 60000;

if (!ConnectToWifi()) return;

// IMPORTANT: please refer to the DPS documentation to understand how to create
// each provisioning. Uncomment the provisioning type you want to use.
// https://docs.microsoft.com/en-us/azure/iot-dps/

const string DpsAddress = "global.azure-devices-provisioning.net"; // You can use as well your own address like yourdps.azure-devices-provisioning.net
const string IdScope = "0neXXXXXXXX"; // Replace the OneXX by the ID scope you'll find in your DPS

// Individual registration, uncomment to use this
/*
const string RegistrationID = "nanoDPSDevice01";
const string SasKey = "averyveryverylongsasbase64token";
var provisioning = ProvisioningDeviceClient.Create(DpsAddress, IdScope, RegistrationID, SasKey, azureCA);
// end of comments for individual
*/

// Group registration, uncomment to use this
/*
// const string RegistrationID = "nano-7C-9E-BD-F6-05-8C";
// const string SasKey = "ashortderivedsastokenbase64";
var provisioning = ProvisioningDeviceClient.Create(DpsAddress, IdScope, RegistrationID, SasKey, azureCA);
// end of comments for group
*/

// Individual certificate registration, uncomment to use this
const string RegistrationID = "nanoCertTest";

// Those certificates are self signed and provided as example
// You will have to provide your own certificates
const string cert = @"-----BEGIN CERTIFICATE-----
MIIDNjCCAh6gAwIBAgIQNw39CRZLRI9C2MRpqKEOBTANBgkqhkiG9w0BAQsFADAz
MQswCQYDVQQGEwJVUzENMAsGA1UECgwEVEVTVDEVMBMGA1UEAwwMbmFub0NlcnRU
ZXN0MB4XDTIxMDcyNzEyNDY0MVoXDTIyMDcyNzEyNTY0MFowMzELMAkGA1UEBhMC
VVMxDTALBgNVBAoMBFRFU1QxFTATBgNVBAMMDG5hbm9DZXJ0VGVzdDCCASIwDQYJ
KoZIhvcNAQEBBQADggEPADCCAQoCggEBAK6HVATbEfcWPKIJ0pYEz7VRL1lZSlg+
+/ozHMssKP31DVuxBmnWCVDtBGzfu2DxxZCmGq++R90PsolFpEdctbOeQrcF9q66
N8+donCIVTrLXBs5uJFHgCpK8E1m990zsJ+y77qYwxBOTf6LcFiBLk17JESD9JCY
5y8Y0M8rEhoWLvd0+N8WiRwoIJfzWQC9PSphkdfLKQqaQEXQv8T734OQYPLbFk8f
iq971ipvclyvUWCPXOjPF7NS2rpdQ+ZFikVYUOmDWr+wm0Hro8djjDTeOm+ShoyL
w48q9qZWMkcQ4KHuOJh65tphQDs6qIPQ/8MAjCR2PO+inSZAfGSaTTUCAwEAAaNG
MEQwDgYDVR0PAQH/BAQDAgWgMBMGA1UdJQQMMAoGCCsGAQUFBwMCMB0GA1UdDgQW
BBQMfNre9wMs5szAu43XekDAWa0YGDANBgkqhkiG9w0BAQsFAAOCAQEADzgibKMV
jO+3bdPyNi21W3EnoAoayPqisG0WFas9knmKs1vSk1sOjQr+HHkYU7GmsFnvWcBN
C3EC9y1sgPI302Ai09JFUbVb1S4JUMpdo9DyQbKI0AI6iLq7Qn2i2Ko6cVcPaKpF
+bFoaJ9UiC3FuJe5Vc5rFq3buZM+BPpDnSUeV1TCVDH3RnTdvOinuN7OueUn/4Dl
3X16GFRTab3oQA9DKQMTVLsrng8w50mAYPqC3CkgRdXtuykCfgPCAal6wTXtACeH
xTUVziM5N0DG2Yk2oYQJB3fiVFLtfTCAmqs1PD21nqDHgi9o/mKGWQ0YCp3nHcoD
z0xYA5aXpTkH3Q==
-----END CERTIFICATE-----
";

const string privateKey = @"-----BEGIN ENCRYPTED PRIVATE KEY-----
MIIFHDBOBgkqhkiG9w0BBQ0wQTApBgkqhkiG9w0BBQwwHAQIeKspDP5XntMCAggA
MAwGCCqGSIb3DQIJBQAwFAYIKoZIhvcNAwcECBBUGFl4iJesBIIEyENikTjnMF6r
NSWD2Y3SXrxJsSXk/zBGHZaAIr80D59lTxYg0tsbct9zsMt/CDIdEwCcl/FYOrbl
j820cNu1+SCkN4ml/qKLBiKByDaNQWiQT9jjhOb12CC4ATY8DoRHq9LlZHHaIqTZ
IVl1QGtrZGHn+FMg/Z9RKvw3kpQTw4nSzVQw9DD8a71aIfcMgQOqep1eBEWvQqo0
jT4ln+n2EJOrvWG/JvoQBCHCeU+pMSsz5nRuMNOQyjJ1m2qtkCU/ms3M54S+RZys
uJ2325S6W9yZrOOBD5tSDa7oUB5pPJJr9mLuFp04zkBXob8G6l2Lf2d1dB4mWLyc
FepT7nzqJMv44H+DK3dFJw0oeChZ2ZKrO56gSq9YuKA8sJH5uwyqFXws+Hat7QxW
f9qf/I29gQYChT8mc3bqVrP79bGNQPzMt5N7yQY7CWf0KYVNXw+edEBSLnGgXxrz
WrheeIiGTLI6NQYPK0rK8fU+aXhzFskSqXoJIIXNKu/ac1QyooMM/D/j5Yp2inBm
PIWg5C59jAWwUVB+MByrBA2op6cm8gwrBQYkNYh03U1HLXlOgHFAxByt20kViBHf
ak1EEC2kHnW2lug+smxpUuWulA0wPI3cII9caiiGjr7ipQviejFhcumH5RyhD0Mj
BFCfrA3FOB8k5ZzXbF4HVSmL2tFIQgvV18tcyb96wN28Wn/REQjp6xkOv2k4nfIy
eSVQnm4x2Qj2BYiydlwzk13nqrpfXNKo4ERjHVVEPyJbmsFdnbYIRLW0lSszGFgT
FWrEZImzMs9srGghhbVCC+N5U4dWhQpT2UjC79c+ZLZrzm8po5FXxrwV/LlHJGYU
u62tb+7D71DgPZDgdbhontlRZiUJywQVJrdt1jlIrnhO06W2h5l019lH5zXQJ07K
XPKpA8SN8rPOFgIdMrYcGwG5K+ziqtYZWxeH1AEfe3/xYkUiiR7OTWz2JCo+bA2M
SemtWY4ACKFaWvOkYBzYatmyWepH2H7vHwGy1r/9cQju1k+bcvHg8kT+p5dPF97i
hrm5JPm1ScyrtSt2q9KZ5X6BM2huTBzNy+kMICOcAU3LvxwrVQ+wG7ef09sFDf7f
r+G1/QJl8zgAQu0rW+nmmc68JOX1T4V7cfrpp8Dl9nb3hYCcIwL1yZADeV5qAjVK
MvP6GxFRQ5t2kSUfoVB+Cnr+EzHoTFYxM+pvugTZYsCM8FyLJoPpyjdYbc3y0/iy
p8qWdU3vzlj+oz25V9oj+aef3e8TD1aLnQq9cI8m5a71WZjEyUlhBVI9rVOBgUvd
RlkyiWAsVSnLfZxPA/kpH5AlLIjn1nH8eBHbK2ftc504GQolkUHwVLR2lApnuiLy
0irqerDYJnm2gmRkHhnuXY3+HReVlWaX0J+1VYhwHOwLt9yOM2gSSCpxVMiPnMT/
UjOZLA6X9YT9j5v8cLAbP7HmQnN1/GqGFN5K7ysHwaFumpFnLzu3578WvTCHGchU
GMxnjaRdZBbZqoCx92XgjYAz2dPLyRf0U6tSZUsLyzuwOlNxFr0d4QHdJ0nX4j2q
WNTHW61gJz2stFOefG9jItbpoZUOyEWc1Dx3+aV887dPoEh5b00eeghYxo51//He
lOSLtZJ072NZegNiDXIVZQ==
-----END ENCRYPTED PRIVATE KEY-----
";

X509Certificate azureCA = new X509Certificate(DpsSampleApp.Resources.GetBytes(DpsSampleApp.Resources.BinaryResources.BaltimoreRootCA_crt));
X509Certificate2 deviceCert = new X509Certificate2(cert, privateKey, "1234");
var provisioning = ProvisioningDeviceClient.Create(DpsAddress, IdScope, RegistrationID, deviceCert, azureCA);
// end of comments for certification

var myDevice = provisioning.Register(null, new CancellationTokenSource(30000).Token);

if(myDevice.Status != ProvisioningRegistrationStatusType.Assigned)
{
    Debug.WriteLine($"Registration is not assigned: {myDevice.Status}, error message: {myDevice.ErrorMessage}");
    return;
}

Debug.WriteLine($"Device successfully assigned:");
Debug.WriteLine($"  Assigned Hub: {myDevice.AssignedHub}");
Debug.WriteLine($"  Created time: {myDevice.CreatedDateTimeUtc}");
Debug.WriteLine($"  Device ID: {myDevice.DeviceId}");
Debug.WriteLine($"  Error code: {myDevice.ErrorCode}");
Debug.WriteLine($"  Error message: {myDevice.ErrorMessage}");
Debug.WriteLine($"  ETAG: {myDevice.Etag}");
Debug.WriteLine($"  Generation ID: {myDevice.GenerationId}");
Debug.WriteLine($"  Last update: {myDevice.LastUpdatedDateTimeUtc}");
Debug.WriteLine($"  Status: {myDevice.Status}");
Debug.WriteLine($"  Sub Status: {myDevice.Substatus}");

// Uncomment the following for Individual or group SAS based DPS:
//var device = new DeviceClient(myDevice.AssignedHub, myDevice.DeviceId, SasKey, nanoFramework.M2Mqtt.Messages.MqttQoSLevel.AtMostOnce, azureCA);
// Uncomment the following for certificate based DPS:
var device = new DeviceClient(myDevice.AssignedHub, myDevice.DeviceId, deviceCert, nanoFramework.M2Mqtt.Messages.MqttQoSLevel.AtMostOnce, azureCA);
// Keep only oner of the previous line commented depending on if you're certificate based or SAS based registration.

var res = device.Open();
if(!res)
{
    Debug.WriteLine($"can't open the device");
    return;
}

var twin = device.GetTwin(new CancellationTokenSource(15000).Token);

if(twin != null)
{
    Debug.WriteLine($"Got twins");
    Debug.WriteLine($"  {twin.Properties.Desired.ToJson()}");
}

Thread.Sleep(Timeout.Infinite);

bool ConnectToWifi()
{
    Debug.WriteLine("Program Started, connecting to Wifi.");

    // As we are using TLS, we need a valid date & time
    // We will wait maximum 1 minute to get connected and have a valid date
    var success = WifiNetworkHelper.ConnectDhcp(Ssid, Password, requiresDateTime: true, token: new CancellationTokenSource(sleepTimeMinutes).Token);
    if (!success)
    {
        Debug.WriteLine($"Can't connect to wifi: {WifiNetworkHelper.Status}");
        if (WifiNetworkHelper.HelperException != null)
        {
            Debug.WriteLine($"WifiNetworkHelper.HelperException");
        }
    }

    Debug.WriteLine($"Date and time is now {DateTime.UtcNow}");
    return success;
}
