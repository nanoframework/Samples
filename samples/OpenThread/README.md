# 🌶️🌶️ - OpenTHread Networking sample pack

Shows how to use OpenThread Networking API.

## Samples

- [🌶️🌶️🌶️ - UPD OpenThread Client using sockets](UdpThreadClient/)
- [🌶️🌶️🌶️ - UPD OpenThread Server using sockets](UdpThreadServer/)

Shows how to use various APIs related to OpenThread.

## Hardware requirements

These project are for the ESP32_C6 and ESP32_H2 Espressif devkit boards with a Ws2812B Neopixel on pin 8. 
This can be easily disabled or Ws2812B added to pin 8 for other boards.

## Sample description

### Upd socket samples

These 2 sample work together to create a client / server communications over OpenThread.
They use UPD sockets over the IPV6 networking of the OpenThread stack.

The neopixel shows the current role of the node.

- White -> Detached from network
- Green -> Child
- Blue  -> Router
- Red   -> Leader
 
 The led will flash when message is transmitted or received.

 The samples use the CCSWE.nanoFramework.Neopixel for driving the neopixels.
 
 There is currently a problem driving the RMT on the ESP32_H2 devices as the core frequency for RMT is
 32Mhz instead of 80Mhz. This will be fixed shortly.

#### UdpThreadClient

This sample will send a broadcast message every 5 seconds using the built-in mesh broadcast address "ff03::1" and port 12324.
Any UdpThreadServer running on the same mesh network will receive message and respond back to sender. 
Any received messages are logged on console.

#### UdpThreadServer

Sample opens sockets and waits for any messages on port 1234. If any message is received to is echoed back to sending address.

## Related topics

### Reference

- [nanoFramework.Networking.Thread](http://docs.nanoframework.net/api/nanoFramework.Networking.Thread)

## Build the sample

1. Start Microsoft Visual Studio 2022 or Visual Studio 2019 (Visual Studio 2017 should be OK too) and select `File > Open > Project/Solution`.
1. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
1. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> [!NOTE]
>
> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
>
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
