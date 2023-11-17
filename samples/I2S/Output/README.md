# 🌶️🌶️ - I2S Speaker sample

Shows how to use the [System.Device.I2s](http://docs.nanoframework.net/api/System.Device.I2s.html) API to write to an I2s device.
In this sample we'll be using a [MAX98357A](https://www.adafruit.com/product/3006) breakout board and a generic 4 Ohm, 3W speaker.

The sample project includes an AudioPlayer and a WavFileHeader abstraction.

The sample is [located here](./).

## Hardware requirements

- A [MAX98357A](https://www.adafruit.com/product/3006) breakout board,
- a generic 4 Ohm, 3W speaker,
- a [microSD card](https://www.adafruit.com/product/254) breakout board and 
- a microSD card to store the recorded audio to.

The code sample is demonstrative of the use of the I2S API.

## Related topics

### Reference

- [System.Device.I2s](http://docs.nanoframework.net/api/System.Device.I2s.html)

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
