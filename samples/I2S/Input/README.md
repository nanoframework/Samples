# 🌶️🌶️ - I2S Microphone sample

Shows how to use the [System.Device.I2s](http://docs.nanoframework.net/api/System.Device.I2s.html) API to read from an I2S device.
In this sample we'll be using a [PDM Microphone](https://www.adafruit.com/product/3492) breakout board to record 1s of audio data to a SD card.

The sample is [located here](./Program.cs).

## Hardware requirements

- A [PDM Microphone](https://www.adafruit.com/product/3492) breakout board,
- a [microSD card](https://www.adafruit.com/product/254) breakout board and 
- a microSD card to store the recorded audio to.

The code sample is demonstrative of the use of the I2S API.

## Related topics

### Reference

- [System.Device.I2s](http://docs.nanoframework.net/api/System.Device.I2s.html)

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
