# Hosting sample pack

The .NET nanoFramework Generic Host provides convenience methods for creating [dependency injection (DI)](https://github.com/nanoframework/nanoFramework.DependencyInjection/tree/main) application containers with preconfigured defaults.

## Samples

### Generic Host application container sample 

[Simple](./Simple) shows how to create a very simple Generic Host object.

###  Hardware sample 

[SlowBlink](./SlowBlink) shows how to create a Generic Host application container including gpio hardware.  Project sample will require a working led connected to a GPIO port.

###  Sensor Queue sample 

[Sensor Queue](./SensorQueue) shows how to create a background queue to process sensor data using the Generic Host object.

###  Logging sample 

[Logging](./Logging) shows how to use extention methods to add logging to a Generic Host object.

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
