# WiFi Soft AP sample

Shows how to use various APIs related with WiFi Soft AP.

### Starts a Soft AP (Hot spot) and runs a simple web server to configure the Wireless connection parameters.

The sample uses GPIO pin 5 to switch back into the SoftAP configuration mode. When pulled to ground on boot will switch into Soft AP mode.  
You can connect to the web server via the url http://192.168.4.1/

Once you save the wireless configuration the next boot the device will automatically connect to that Access Point
and the Soft AP will be disabled.

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/main.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Hardware requirements

An hardware device with WiFi networking capabilities running a nanoFramework image.
Currently only the Esp32.

## Related topics

### Reference

- [SYstem.Net.NetworkInformation](http://docs.nanoframework.net/api/System.Net.NetworkInformation.html)

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
