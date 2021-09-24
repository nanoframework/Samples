# GPIO and events sample (.NET IoT style)

Shows how to use the [System.Device.Gpio](http://docs.nanoframework.net/api/System.Device.Gpio.html) API allowing your to set and read the state of GPIO pins.
This sample is using the .NET IoT API style.


> **Note:** This sample is part of a large collection of nanoFramework feature samples. 
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a 
> [ZIP file](https://github.com/nanoframework/Samples/archive/main.zip), but be 
> sure to unzip everything to access any shared dependencies. 
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->


Shows how to use the [System.Device.Gpio]() API allowing your to:
* set the state of output GPIO pins 
* read state of input GPIO pins
* setup handlers to react to GPIO state change events on external stimulus


## Hardware requirements

Any hardware device running a nanoFramework image built with GPIO support (.NET IoT) enabled.

### Reference

[System.Device.Gpio](http://docs.nanoframework.net/api/System.Device.Gpio.html)

<!-- [nanoFramework app samples]() -->

## System requirements

**Client:** Windows 10

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

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 
