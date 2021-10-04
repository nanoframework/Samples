# I2C sample

Shows how to use the [Windows.Devices.I2c](http://docs.nanoframework.net/api/Windows.Devices.I2c.html) API to read and write from/to an I2C device.
In this sample we'll be using a [STMPE811](https://www.digikey.com/en/product-highlight/s/stmicroelectronics/stmpe811) resistive touchscreen controller mounted in a [STM32F429I DISCOVERY](http://www.st.com/en/evaluation-tools/32f429idiscovery.html) board.

The sample project includes a minimal driver for the STMPE811.
It configures the touchscreen controller and the GPIO pin where the INT signal of the controller is connected allowing the code to react to a touch event rather then wasting CPU by constantly pooling the touch detection.

Following a touch event (and as long as the screen is pressed) the console outputs the coordinates of the touch point.

## Hardware requirements

An [STM32F429I DISCOVERY](http://www.st.com/en/evaluation-tools/32f429idiscovery.html) board.
The code sample is demonstrative of the use of the I2C API.

## Related topics

### Reference

- [Windows.Devices.I2c](http://docs.nanoframework.net/api/Windows.Devices.I2c.html)
- [Windows.Devices.Gpio](http://docs.nanoframework.net/api/Windows.Devices.Gpio.html)

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
