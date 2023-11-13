# 🌶️ to 🌶️🌶️ - Hardware STM32 sample pack

Shows how to use various APIs specific to STM32 targets.

- [🌶️ - Alarm test](Stm32.TestAlarms/)
- [🌶️🌶️ - Backup memory](Stm32.BackupMemory/)
- [🌶️ - Read device IDs](Stm32.ReadDeviceIDs/)
- [🌶️🌶️ -Power down/off test](Stm32.PowerMode/)

## Hardware requirements

An STM32 hardware device running a nanoFramework image.

## Related topics

### Reference

- [nanoFramework.Hardware.Stm32.BackupMemory](http://docs.nanoframework.net/api/nanoFramework.Hardware.Stm32.BackupMemory.html)
- [nanoFramework.Hardware.Stm32.Power](http://docs.nanoframework.net/api/nanoFramework.Hardware.Stm32.Power.html)
- [nanoFramework.Hardware.Stm32.RTC](http://docs.nanoframework.net/api/nanoFramework.Hardware.Stm32.RTC.html)
- [nanoFramework.Hardware.Stm32.Utilities](http://docs.nanoframework.net/api/nanoFramework.Hardware.Stm32.Utilities.html)

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
