# Texas Instruments EasyLink sample pack

Shows how to use TI EasyLink API to send/receive packets over a radio link.

- [Simple Node](EasyLink.Node/)
- [Concentrator](EasyLink.Concentrator/)

It will be hard to debug both Solutions simultaneously in the same machine because that requires two instances of Visual Studio open simultaneously.
You can try that, but that's not recommended. The suggestion goes toward loading the Node solution first, build and deploy. Because the application runs without a debugger attached you'll have a standalone board transmitting whatever you've programmed it to. Next load the Concentrator Solutions, build and start a debug session to receive radio packets from the other board.
In case you change the addresses of the node and/or concentrator make sure to use the same on both projects.

## Hardware requirements

A TI CC13xx or CC26xx hardware device running a nanoFramework image.

## Related topics

### Reference

- [nanoFramework.TI.EasyLink](http://docs.nanoframework.net/api/nanoFramework.TI.EasyLink.EasyLinkController.html)


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
