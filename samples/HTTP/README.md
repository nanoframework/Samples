# 🌶️🌶️ - HTTP sample pack

Shows how to use the HTTP related APIs in [System.Net](http://docs.nanoframework.net/api/System.Net.html).

Please note that samples are offered in "pairs": one for wired connected targets and another one for Wi-Fi connected targets.

## Scenarios

### HTTP listener

[🌶️🌶️ - Http Listener](./HttpListener) on how to implement a HTTP listener to reply to incoming HTTP requests.
[🌶️🌶️ - Http Listener with wifi](./HttpListener_Wifi) on how to implement a HTTP listener to reply to incoming HTTP requests for Wi-Fi connected targets..

> **Note:** this is **NOT** an HTTP server, just a sample to illustrate how to use the HTTP Listener class.  

### HTTP Web Request

[🌶️🌶️ - Http Web Request](./HttpWebRequest) on how to perform a HTTP Web requests. Optionally to secured (TLS) servers.
[🌶️🌶️ - Http Web Request with wifi](./HttpWebRequest_Wifi) on how to perform a HTTP Web requests for Wi-Fi connected targets. Optionally to secured (TLS) servers.

### HTTP Azure Get

Illustrates how to connect to [🌶️🌶️🌶️ - Azure and perform GET requests](./AzureGET).
Illustrates how to connect to [🌶️🌶️🌶️ - Azure and perform GET requests with wifi](./AzureGET_Wifi) for Wi-Fi connected targets.

### HTTP Azure POST

Illustrates how to connect to [🌶️🌶️🌶️ - Azure and perform POST requests](./AzurePOST).
Illustrates how to connect to [🌶️🌶️🌶️ - Azure and perform POST requests with wifi](./AzurePOST) for Wi-Fi connected targets.

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image.
This sample is coded to use the STM32F769IDiscovery target board, but can be easily changed to any other target that has networking capabilities.

> NOTE: When working with a target with Wi-Fi capabilities, make sure to add `HAS_WIFI` into the DefineConstants, like this:

```text
<DefineConstants>$(DefineConstants);HAS_WIFI;</DefineConstants>
```

## Related topics

### Reference

- [System.Net.HttpWebRequest](http://docs.nanoframework.net/api/System.Net.HttpWebRequest.html)

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
