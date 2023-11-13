# 🌶️🌶️🌶️ - Interop sample

This project illustrates the use of the 'Interop' feature which allow a C# manage API to call native code.

There are two Solutions in this sample:

- In the [*awesome-library*](awesome-library/) folder you'll find the solution with the Interop library: `NF.AwesomeLib.sln`.
- In the [*test-application*](test-application/) folder you'll find the test application that is referencing the Interop library: `Test.Interop.sln`.

:warning: This is an advanced topic. :warning:

Please read this [blog post](https://jsimoesblog.wordpress.com/2018/06/19/interop-in-net-nanoframework/) with a detailed explanation on how to create, build and use an Interop library.

## Hardware requirements

Any hardware device running a .NET nanoFramework image.
You'll be replacing the existing image with one that includes support for the Interop library.

## Related topics

### Reference

## Build the sample

1. Start Microsoft Visual Studio 2022 (VS 2019 and 2017 should be OK too) and select `File > Open > Project/Solution`.
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
