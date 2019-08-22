# Interop sample

This project illustrates the use of the 'Interop' feature which allow a C# manage API to call native code.

There are two Solutions in this sample:

- In the [*awesome-library*](awesome-library/) folder you'll find the solution with the Interop library: `NF.AwesomeLib.sln`.
- In the [*test-application*](test-application/) folder you'll find the test application that is referencing the Interop library: `Test.Interop.sln`.

:warning: This is an advanced topic. :warning:

Please read this [blog post](https://jsimoesblog.wordpress.com/2018/06/19/interop-in-net-nanoframework/) with a detailed explanation on how to create, build and use an Interop library.

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

This sample shows how to set the date and time of the RTC on a target device.

## Hardware requirements

Any hardware device running a nanoFramework image.

## Related topics

### Reference

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

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging.
