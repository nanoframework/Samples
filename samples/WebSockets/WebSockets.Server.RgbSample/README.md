# WebSocket Server Sample

Shows how to use the WebSockets related APIs in [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html). Documentation on the Websocket library can be found in the [WebSockets Repo](https://github.com/nanoframework/System.Net.WebSockets).

## Sample

This sample shows howto use Websocket Server with a Webserver hosting a WebApp that controlls a rgb led.

## Hardware requirements

This Sample was written for the [M5Stack ATOM Lite](https://shop.m5stack.com/products/atom-lite-esp32-development-kit) device, but can be easily changed to another board connected to a rgb led.

## Related topics

### Reference

- [nanoFramework.Net.WebSockets](https://github.com/nanoframework/System.Net.WebSockets/blob/develop/README.md)
- [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html)
- [getting started guide](https://www.feiko.io/posts/2022-01-03-getting-started-with-net-nanoframework)

## Building the samples

1. Start Microsoft Visual Studio 2022 (VS 2019 and VS 2017 should be OK too) and select `File > Open > Project/Solution`.
2. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
3. Change the WiFi Ssid and Password on line 22 and 23.
4. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.
 
## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.
- The debug output will show the url of the WebApp. Make sure you're connected to the same network as the device and open this url in your browser.
- Set the mood by changing the color of the rgb led using the WebApp. 

> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
