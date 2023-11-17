# 🌶️🌶️ - Networking sample pack

Shows how to use network Sockets API.

## Samples

- [🌶️🌶️ - Socket Client](Socket.Client/)
- [🌶️🌶️ - Socket Client (Wi-Fi connection)](Socket.Client_WiFi/)

Shows how to use various APIs related with networking.

> NOTE: if you're editing the project files, when working with a target with Wi-Fi capabilities, make sure to add `HAS_WIFI` into the DefineConstants, like this:

```text
<DefineConstants>$(DefineConstants);HAS_WIFI;</DefineConstants>
```

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/main.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Hardware requirements

An hardware device running a nanoFramework image with networking capabilities enabled.

## Related topics

### Reference

- [System.Net.Sockets](http://docs.nanoframework.net/api/System.Net.Sockets.html)

## Build the sample

1. Start Microsoft Visual Studio 2022 or Visual Studio 2019 (Visual Studio 2017 should be OK too) and select `File > Open > Project/Solution`.
1. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
1. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> [!NOTE]
>
> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
>
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
