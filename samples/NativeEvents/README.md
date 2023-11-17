# 🌶️🌶️ - Native events sample

Shows how to use the [CustomEvent](http://docs.nanoframework.net/api/nanoFramework.Runtime.Events.CustomEvent.html) class that allows to post events from the native code to the managed application.

The sample is [located here](./Program.cs).

## Hardware requirements

Any hardware device running a nanoFramework image built with GPIO support enabled.

## Other requirements

A nanoFramework image with code that includes posting the event.

## Related topics

### Blog post

Check [this blog post](https://jsimoesblog.wordpress.com/2019/08/23/posting-native-events-in-nanoframework/) with a more detailed explanation on how to have this working in your code.

### Reference

[CustomEvent](http://docs.nanoframework.net/api/nanoFramework.Runtime.Events.CustomEvent.html)

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
