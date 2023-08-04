using DpsSampleApp;
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

// You can use as well your own address like yourdps.azure-devices-provisioning.net
const string DpsAddress = "global.azure-devices-provisioning.net"; 
// Replace the 0neXXXXXXXX by the ID scope you'll find in your DPS
const string IdScope = "0neXXXXXXXX"; 

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

// Individual X.509 certificate registration, uncomment to use this
const string RegistrationID = "nanoCertTest";

// Those certificates are self signed and provided as example
// You will have to provide your own X.509 certificates
const string cert = @"-----BEGIN CERTIFICATE-----
MIIDFzCCAf8CFAFzQLDJ4TBRucbSvzvAI760DH5FMA0GCSqGSIb3DQEBCwUAMEgx
CzAJBgNVBAYTAlVTMRMwEQYDVQQIDApTb21lLVN0YXRlMQ0wCwYDVQQKDARURVNU
MRUwEwYDVQQDDAxuYW5vQ2VydFRlc3QwHhcNMjMwNjI3MjM0MTQxWhcNMzMwNjI0
MjM0MTQxWjBIMQswCQYDVQQGEwJVUzETMBEGA1UECAwKU29tZS1TdGF0ZTENMAsG
A1UECgwEVEVTVDEVMBMGA1UEAwwMbmFub0NlcnRUZXN0MIIBIjANBgkqhkiG9w0B
AQEFAAOCAQ8AMIIBCgKCAQEAzbpKfVuVi2pAbm5Weh4oGJcHh1c0k6UkFLA/9ALE
sjWaWDhPFoBBh5PiUFcoJR3ALg6o/56KUttALujyyOmgrmQwLMA2MEimRpyD6TKV
IRHZfsPAiXegihTUZXR/QdLsRHKYLCNiGT7p5WTZ75v/4Zo6CT5No67EbrDLL2Go
PQizOkpZTxvMO4BHS1/Hik25jVfNUyxIENrfBGA6u6NLiFf/LSpL/Uzx8GOxZbBF
HsCqwwTs1Tu8oaJDMoXt/388FNz/Pj4SJq0BulKHfzapo1Wj2aLbPK3Ms3aawH2G
j45mqmrdR1bPx3MIWrSUnvarDjxKi1CGgJLsoHABserJQwIDAQABMA0GCSqGSIb3
DQEBCwUAA4IBAQBM6FPrTDWhlslqg+62q2xe46yRMj4UrNxJn5FY0eSgTDKdG1t9
PuPjf73pzCuvGNrYWs677hmQ+R+9RHx9CgV23VciEpTiFlhf8EjVPNSUxR3FL8ts
x+eut5rCY36h0QW/K0WNAzuDCuYdaRW3U5Kw7k7ohRB3dy1u24FeyiPJLrT5zav3
Ev1kjZ3AfDt++u3phG+u3hhxC6nuCU/Cr5yp1vaxkM33Z6lMNda/D3z65OM29HbO
vPF27k/2+ci3t52czyjgjkjUuwwTRJP8wy7Mtzb57gnr3hueKix9ENFrFgvNP+M0
b2YiAtstnJw7ubj9OAh23ALdnhvrJKD3Aa9A
-----END CERTIFICATE-----";

const string privateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAzbpKfVuVi2pAbm5Weh4oGJcHh1c0k6UkFLA/9ALEsjWaWDhP
FoBBh5PiUFcoJR3ALg6o/56KUttALujyyOmgrmQwLMA2MEimRpyD6TKVIRHZfsPA
iXegihTUZXR/QdLsRHKYLCNiGT7p5WTZ75v/4Zo6CT5No67EbrDLL2GoPQizOkpZ
TxvMO4BHS1/Hik25jVfNUyxIENrfBGA6u6NLiFf/LSpL/Uzx8GOxZbBFHsCqwwTs
1Tu8oaJDMoXt/388FNz/Pj4SJq0BulKHfzapo1Wj2aLbPK3Ms3aawH2Gj45mqmrd
R1bPx3MIWrSUnvarDjxKi1CGgJLsoHABserJQwIDAQABAoIBAAr4n1ZWFwCLVwpM
mhIDH7JIA5/FF8mz6dusloyFxUWXtDZ2MkmJ53S6fzw3ma15C7GpGHwhUVEf3ili
ROhQBUCnmSYZzgn/JdboK+S4zNkpoYawG8l49rfGaplKgCrbe7wevFzGOkoMX97a
5QBxOlEmr2ekyfErtdANX5iURauTfbKXCRfjeqiwhlotAI8SxHe5dJZwRxsjBfx9
FAmEqYbpVWaj3Op/YNZYeAvgcQjM+5oV7pNNcVeWFXFkE8iOiF1PAeJjx6j/3W8b
iypqvUDvqJAUsxJZ2THanlik2qmqePHvaX0V2oDAxqCMN+5MKuAZYOpHhQpacRLU
xEcYVzkCgYEA8EZLpUXLNtjqJEE4QBXwGDnRwc0LSkmjrEBqO8gw9aHrCSIughzJ
GJWE+9UXg/W4ffPpTk+66UWOZXjoXtcUqFAR9klgPVkDjmq26VwZLFYUwtEeR03s
ni6REupRAOIHexoV9nOSGw0v1NRSR94jVGIElUJE+WZmN0Ksw1XPuX0CgYEA2zEt
F5XVtRbDCNbILikLHSbZKHbnO37Wbel1ezVRcbEvVkrDRibPoruIwt7cztb+VJPP
wngGPD962ZajZ/9NIzLuGz1CeDwUpywVoMbnT3j9FqtmroSNNZKbiTWHCJs0i5Ro
R/1+kKU4VTHw4KzlIAK0TRRGazKtX4AN3/QyCb8CgYAGs69AeOXmLb66LCeJghMk
WdiD81gxRkSOdW2BJWBYOZ/4rT9m2a6yRNlkvNjfEWeH+9myGX85KnuCUREKNC2b
VEBsAjfw/h6fRlK7x5ncJrqFhJe3nXDQKLRbNrXztFpJEL00Fp0orAF9ij6RSpzp
qaI/F44c9sI7IFz5Rdd5cQKBgQCUFIXlICmvXIBIkWnNnZbPi/Y7WxDeZdMRkB35
Lc0m5NAZGQsRcpjl1JIRLKS57A8ILo+2ToXP4AbrxtQAJz8Cn56yslEcj2JYifTW
mGmejBtXXFZTYmNPpQsEyC/AxbHa9lj5Aa2mpKgJDmMwNj3YwVrzk9X7B9KO6SD2
gq/nWwKBgQDGpsBPUjRVmrCFXxpviiCSZru6vDwshmSzSOOsmEvGcqXABjJtT1+j
jrvJUb40I2u1XXl9aIF8kScDYMG3Tnkqb+2KP40HL7QzOhNsWRLl3lJCNg3uH3sV
LRnBgga5RJ3ggaMhV/8FXmXOb4reCcj9dYan9oROHXFGyHPe1wgqwA==
-----END RSA PRIVATE KEY-----";

X509Certificate azureCA = new X509Certificate(Resource.GetString(Resource.StringResources.AzureRootCerts));
X509Certificate2 deviceCert = new X509Certificate2(cert, privateKey, "1234");
var provisioning = ProvisioningDeviceClient.Create(DpsAddress, IdScope, RegistrationID, deviceCert, azureCA);
// end of comments for X.509 certificates

var myDevice = provisioning.Register(null, new CancellationTokenSource(30000).Token);

if(myDevice.Status != ProvisioningRegistrationStatusType.Assigned)
{
    Debug.WriteLine($"Registration is not assigned: {myDevice.Status}, error message: {myDevice.ErrorMessage} [code {myDevice.ErrorCode}]");
    return;
}

Debug.WriteLine($"Device successfully assigned:");
Debug.WriteLine($"  Assigned Hub: {myDevice.AssignedHub}");
Debug.WriteLine($"  Created time: {myDevice.CreatedDateTimeUtc}");
Debug.WriteLine($"  Device ID: {myDevice.DeviceId}");
Debug.WriteLine($"  ETAG: {myDevice.Etag}");
Debug.WriteLine($"  Generation ID: {myDevice.GenerationId}");
Debug.WriteLine($"  Last update: {myDevice.LastUpdatedDateTimeUtc}");
Debug.WriteLine($"  Status: {myDevice.Status}");
Debug.WriteLine($"  Sub Status: {myDevice.Substatus}");

// Uncomment the following for Individual or group SAS based DPS:
//var device = new DeviceClient(myDevice.AssignedHub, myDevice.DeviceId, SasKey, nanoFramework.M2Mqtt.Messages.MqttQoSLevel.AtMostOnce, azureCA);
// Uncomment the following for X.509 certificate based DPS:
var device = new DeviceClient(myDevice.AssignedHub, myDevice.DeviceId, deviceCert, nanoFramework.M2Mqtt.Messages.MqttQoSLevel.AtMostOnce, azureCA);
// Keep only one of the previous lines uncommented depending on if you're using X.509 certificate or SAS based registration.

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
