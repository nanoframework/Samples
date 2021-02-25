# Texas Instruments EasyLink sample pack

Shows how to use TI EasyLink API to send/receive packets over a radio link.

- [Simple Node](EasyLink.Node/)
- [Concentrator](EasyLink.Concentrator/)

It will be hard to debug both Solutions simultaneously in the same machine because that requires two instances of Visual Studio open simultaneously.
You can try that, but that's not recommended. The suggestion goes toward loading the Node solution first, build and deploy. Because the application runs without a debugger attached you'll have a standalone board transmitting whatever you've programmed it to. Next load the Concentrator Solutions, build and start a debug session to receive radio packets from the other board.
In case you change the addresses of the node and/or concentrator make sure to use the same on both projects.

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Hardware requirements

A TI CC13xx or CC26xx hardware device running a nanoFramework image.

## Related topics

### Reference

- [nanoFramework.TI.EasyLink](http://docs.nanoframework.net/api/nanoFramework.TI.EasyLink.EasyLinkController.html)

## Build the sample

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build.
2. Start Microsoft Visual Studio 2019 (VS 2017 should be OK too) and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
4. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select Build > Deploy Solution.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging.
