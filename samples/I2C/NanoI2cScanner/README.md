# 🌶️🌶️ - I2C Scanner sample

Shows how to use the [System.Device.I2c](http://docs.nanoframework.net/api/System.Device.I2c.html) API to scan the I2C bus.

The sample is [located here](./Program.cs).

This sample uses the specificity of `Write` and `Read` function returning an `I2cTransferResult` structure.

When running the code, you'll get output in debug that will look like this:

```text
0x0E Write: 4, transferred: 0, Read: 2, transferred: 0
0x0E - Absent
0x0F Write: 4, transferred: 0, Read: 2, transferred: 0
0x0F - Absent
0x10 Write: 1, transferred: 1, Read: 1, transferred: 1
0x10 - Present
0x11 Write: 4, transferred: 0, Read: 2, transferred: 0
0x11 - Absent
```

The first part is the I2C device address and then you'll get the status of a write and a read. A successful read or write has the status result as `I2cTransferStatus.FullTransfer`. Any other result means that there is an issue. The number of bytes transferred should be 1 in both cases.

> Note: you can adjust the code to validate only read or write, some devices, may require specific write before being able to read them. In general, writing only is sufficient in the bus.

## Hardware requirements

Any device with I2C in the firmware. The sample is built specifically for ESP32 devices. If you are using a STM32 or anything else, comment the 2 Configuration lines and remove the Hardware.Esp32 nuget.

## Related topics

### Reference

- [System.Device.I2c](http://docs.nanoframework.net/api/System.Device.I2c.html)

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
