# GPIOChangeCounter sample

Shows how to use the [Windows.Devices.Gpio](http://docs.nanoframework.net/api/Windows.Devices.Gpio.html) [GpioChangeCounter API](http://docs.nanoframework.net/api/Windows.Devices.Gpio.GpioChangeCounter.html) to count pulses from a device.

Shows how to use the [Windows.Devices.Gpio](http://docs.nanoframework.net/api/Windows.Devices.Gpio.html) [GpioChangeCounter API](http://docs.nanoframework.net/api/Windows.Devices.Gpio.GpioChangeCounter.html) to:

- Count rising, failing and both edges of a pulse on a GPIO pins 
- Read the count and the relative time of count
- Reset the count
- Start / Stop count

## Hardware requirements

Any hardware device running a nanoFramework image built with GPIO support enabled.
The sample uses a PWM signal to generate the pulses so requires the PWM pin to be connected to the Counter pin.

## Related topics

### Reference

[Windows.Devices.Gpio.GpioChangeCounter](http://docs.nanoframework.net/api/Windows.Devices.Gpio.GpioChangeCounter.html)

## System requirements

**Client:** Windows 10

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
