# WebSocket sample pack

Shows how to use the WebSockets related APIs in [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html).

## Samples


### WebSockets Server Sample 

[Server.RgbSample](./WebSockets.Server.RgbSample) shows howto use Websocket Server with a Webserver hosting a WebApp that controlls a rgb led.

### WebSockets Client Sample 

[Client.Sample](./Websockets.Client.Sample) shows how to use the Websocket Client.

### WebSockets Server and Client sample 

[ServerClient.Sample](./Websockets.ServerClient.Sample) shows how to configure and start a WebSocket Server and (ssl) Client.

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image.
The Websocket Server Sample requires a M5Stack ATOM Lite board, but can be easily changed to another board connected to a rgb led.

## Related topics

### Reference

- [nanoFramework.Net.WebSockets](https://github.com/nanoframework/System.Net.WebSockets/blob/develop/README.md)
- [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html)

## Building the samples

1. Start Microsoft Visual Studio 2022 (VS 2019 and VS 2017 should be OK too) and select `File > Open > Project/Solution`.
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
