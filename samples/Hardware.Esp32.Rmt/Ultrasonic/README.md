# 🌶️🌶️ - Ultrasonic HC-SR04 sensor with RMT

Shows how to use ESP32 RMT with an Ultrasonic HC-SR04 sensor

This sample initializes the SR04 device and sends 10us pulses and then measures the width of the received pulse to calculate the distance in meters.
The sample contains a SR04 class which can be used directly in your own applications.

The SR04 has 4 pins.

## Connections

- VCC -  5V or 3.3V depending on version of SR04
- GND -  Ground signal
- TRIG - Any available GPIO pin capable of output
- ECHO - Any available GPIO pin

> Note: When connecting ECHO to ESP32 pin be aware that if using a 5v version of SR04 a level shifter should be used.
OK for short tests but for long term use connect a resistor between SR04 output and ESP32 pin(10K) to limit current.

## Hardware requirements

Any ESP32 hardware device running a nanoFramework image with HC-SR04P 3.3V version ( HC-SR04 5v version )

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
