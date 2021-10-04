# HTTP Listener sample

Shows how to use the HTTP related APIs in [System.Net](http://docs.nanoframework.net/api/System.Net.html). Shows how to implement a HTTP listener to reply to incoming HTTP requests.

> **Note:** this is **NOT** an HTTP server, just a sample to illustrate how to use the HTTP Listener class.  

> NOTE: when working with ESP32 edit the nfproj files and add `BUIID_FOR_ESP32` to the DefineConstants, like this:
```text
<DefineConstants>$(DefineConstants);BUIID_FOR_ESP32;</DefineConstants>
```

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image.
This sample is coded to use the STM32F769IDiscovery target board, but can be easily changed to any other target that has networking capabilities.

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
