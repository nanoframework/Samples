# I2C GPS sample

Shows how to use the [Windows.Devices.I2c](http://docs.nanoframework.net/api/Windows.Devices.I2c.html) API to read and write from/to an I2C device.
In this sample we'll be using a [IES-SHIELD-GPS](https://i-groupuk.com/downloads/global-positioning-system-gps-shield-for-arduino/) GPS controller mounted on a [STM32F769I DISCOVERY](http://www.st.com/en/evaluation-tools/32f769idiscovery.html) board.

The sample project includes a full driver for the IES-SHIELD-GPS and IES-SHIELD-GPAM allowing you to retrieve the current date, time, longitude, latitude, heading, speed, (GPAM only: pitch and roll) from the device.

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Hardware requirements

An [STM32F769II DISCOVERY](http://www.st.com/en/evaluation-tools/32f769idiscovery.html) board.
The code sample is demonstrative of the use of the I2C API.

## Related topics

### Reference

- [Windows.Devices.I2c](http://docs.nanoframework.net/api/Windows.Devices.I2c.html)

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
