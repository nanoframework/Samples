# 🌶️ - Dependency injection sample pack

A Dependency Injection (DI) Container provides functionality and automates many of the tasks involved in Object Composition, Interception, and Lifetime Management. These API mirrors as close as possible the official .NET 
[Dependency Injection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection). Exceptions are mainly derived from the lack of generics support in .NET nanoFramework.

## Samples

### Dependency injection application container sample 

[🌶️ -  SlowBlink](./SlowBlink) shows how to create a dependency injection application container including gpio and logging.  Project sample will require a working led connected to a GPIO port.

### Dependency injection usage sample

[🌶️ -  Simple](./Simple) shows how to create a very simple dependency injection object.

## Building the samples

1. Start Microsoft Visual Studio 2022 (VS 2019 and VS 2017 should be OK too) and select `File > Open > Project/Solution`.
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
