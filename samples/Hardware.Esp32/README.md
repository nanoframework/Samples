# Hardware ESP32 Deep sleep sample

Shows how to use the [nanoFramework.Hardware.Esp32](http://docs.nanoframework.net/api/nanoFramework.Hardware.Esp32.html) API to access the ESP32 specific functions.

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

This example shows how to use change default pins for devices and to use the Sleep methods in the nanoFramework.Hardware.Esp32 nuget package.
Putting the ESP32 into Deep sleep mode and waking it up after a set period plus other examples.

## Hardware requirements

Any ESP32 hardware device running a nanoFramework image.

## Related topics

### Reference

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

> **Note:** As this puts the device to Sleep you can not debug past that point.
