# **D**igital **A**nalog **C**onverter sample pack

Shows how to use the [System.Device.Dac](http://docs.nanoframework.net/api/System.Device.Dac.html) API to output values to a DAC.

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/main.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Board Pin Reference

### STM32F769I_DISCO pin assignments

DAC Channel 0 = GPIOA_PIN4 (CN14_A1)

### ESP32 pin assignments

ESP32 pin assignments are variable, and must be assigned before use.

## Scenarios

This sample allows the developer to output several output sequences to the **D**igital **A**nalog **C**onverter. You can choose one of three scenarios:

### Instantiate and configure DAC peripheral

This scenario demonstrates the how to instantiate the device default DAC controller and use it's property APIs in order to grab information about the DAC resolution in order to adjust the samples output.

### Output a triangle wave

![](images/triangle-wave.jpg)

This scenario demonstrates the use the DAC to output a triangle wave.

### Output a square wave

![](images/square-wave.jpg)

This scenario demonstrates the use the DAC to output a square wave.

### Output a sine wave

![](images/sine-wave.jpg)

This scenario demonstrates the use the DAC to output a sine wave.

## Hardware requirements

Any hardware device running a nanoFramework image built with DAC support enabled.

## Related topics

### Reference

- [System.Device.Dac](http://docs.nanoframework.net/api/System.Device.Dac.html)

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
