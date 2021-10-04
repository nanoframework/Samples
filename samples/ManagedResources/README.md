# Managed resources sample

Shows how to use the [System.Resources.ResourceManager](http://docs.nanoframework.net/api/System.Resources.ResourceManager.html) API to access the application managed resources.

This sample illustrates how to access from the code the application managed resources.

> **warning**: To access the application managed resources the project has to have a reference to `nanoFramework.Runtime.Native`.

## Hardware requirements

Any hardware device running a nanoFramework image.

## Related topics

### Reference

- [System.Resources.ResourceManager](http://docs.nanoframework.net/api/System.Resources.ResourceManager.html)

## Build the sample

1. Start Microsoft Visual Studio 2019 (VS 2017 should be OK too) and select `File > Open > Project/Solution`.
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
