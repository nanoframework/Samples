# 🌶️ - WebSocket Client Sample

Shows how to use the WebSockets related APIs in [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html). Documentation on the Websocket library can be found in the [WebSockets Repo](https://github.com/nanoframework/System.Net.WebSockets).

## Sample

This sample shows howto use Websocket Client. 

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image.

## Related topics

### Reference

- [nanoFramework.Net.WebSockets](https://github.com/nanoframework/System.Net.WebSockets/blob/develop/README.md)
- [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html)
- [getting started guide](https://www.feiko.io/posts/2022-01-03-getting-started-with-net-nanoframework)

## Building the samples

1. Start Microsoft Visual Studio 2022 (VS 2019 and VS 2017 should be OK too) and select `File > Open > Project/Solution`.
2. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
3. Change the Wifi Ssid and Password on line 14 and 15, also change the url for the websocket server on line 41.  
4. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
