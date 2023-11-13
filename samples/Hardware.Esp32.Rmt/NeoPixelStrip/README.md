# 🌶️🌶️ - NeoPixel Strip WS2812 with RMT

Shows how to use ESP32 RMT with an NeoPixel Strip WS2812

This sample send signal to strip with pin D5.

The sample is [located here](./NeoPixel/).

## Connections

- GIN -  Any available GPIO pin (D5 in the sample)
- GND -  Ground signal  
- +5V - External Power Supply 5V min 2A  

> Notes:
>
> - Put resistor 400 Ohm between GIN and GPIO pin  
> - Join the ground of the external power supply and the GND of ESP32
> - **This example can only manage the first 50 LEDs** due to the ESP32 memory limitation. There is another example to be able to manage more by simplifying the objects used.

## Hardware requirements

- Any ESP32 hardware device running a nanoFramework image
- Led Strip NeoPixel WS2812
- External power supply 5V
- Resistor 400 Ohm
- Capacitor 1000 µF across + and - terminals external power suply

## Related topics

### Reference

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
