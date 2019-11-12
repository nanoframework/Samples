# HTTP sample pack

Shows how to use the HTTP related APIs in [System.Net](http://docs.nanoframework.net/api/System.Net.html).

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Scenarios

### HTTP listener

Example on how to implement a HTTP listener to reply to incoming HTTP requests.

> **Note:** this is **NOT** an HTTP server, just a sample to illustrate how to use the HTTP Listener class.  

### HTTP Web Request

Example on how to perform a HTTP Web requests. Optionally to secured (TLS) servers.

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image.
This sample is coded to use the STM32F769IDiscovery target board, but can be easily changed to any other target that has networking capabilities.

## Related topics

### Reference

- [System.Net.HttpWebRequest](http://docs.nanoframework.net/api/System.Net.HttpWebRequest.html)

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
