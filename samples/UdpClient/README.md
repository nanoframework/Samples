# UdpClient sample pack

Shows how to use UdpClient API in various situations.

## Hardware requirements

An hardware device running a nanoFramework image with networking capabilities enabled.

## Samples provided

Three samples are provided:
- **QOTDClient** : show how to use `UdpClient` as an udp client. It connect to a public `Quote of the day` server and display the result.
- **UdpEchoServer** : show how to use `UdpClient` as an udp server. It implements a simple Udp server that listen to messages and echo them back to the sender. You need a companion sender application to test it. The `Sender.ipynb` notebook in the folder can be used with .net interactive in vscode to send messages to the servers.
- **DumpSSDPRequests** : show how to join a multicast group and get multicast messages. This sample display the SSDP messages that are sent by the different devices on the local network.

## Build the sample

1. Start Microsoft Visual Studio 2019/2022 and select `File > Open > Project/Solution`.
1. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
1. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

You need to select the sample you want to run. If you want to use wifi, define the HAS_WIFI variable and set your wifi parameters in the code.
The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.

> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
