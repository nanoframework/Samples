# GPIOChangeCounter sample

Shows how to use the [Windows.Devices.Gpio]((http://docs.nanoframework.net/api/Windows.Devices.Gpio.html) GpioChangeCounter API to count pulses from a device.


> **Note:** This sample is part of a large collection of nanoFramework feature samples. 
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a 
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be 
> sure to unzip everything to access any shared dependencies. 
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->


Shows how to use the [Windows.Devices.Gpio]() GpioChangeCounter API to:
* Count rising, failing and both edges of a pulse on a GPIO pins 
* Read the count and the relative time of count
* Reset the count
* Start / Stop count


## Hardware requirements

Any hardware device running a nanoFramework image built with GPIO support enabled.
The sample uses a PWM signal to generate the pulses so requires the PWM pin to be connected to the Counter pin.


## Related topics

### Samples

[GpioChangeCounter](/Gpio/GpioChangeCounter)

### Reference

[Windows.Devices.Gpio](http://docs.nanoframework.net/api/Windows.Devices.Gpio.html)

<!-- [nanoFramework app samples]() -->

## System requirements

**Client:** Windows 10

## Build the sample

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build. 
2. Start Microsoft Visual Studio 2017 and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
4. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select Build > Deploy Solution. 

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 
