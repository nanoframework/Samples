# Ultrasonic HC-SR04 sensor with RMT

Shows how to use ESP32 RMT with an Ultrasonic HC-SR04 sensor

This sample intializes the SR04 device and sends 10us pulses and then measures the width of the received pulse to calculate the distance in meters.
The sample contains a SR04 class which can be used directly in your own applications.

The SR04 has 4 pins.

Connections
VCC -  5V or 3.3V depending on version of SR04
GND -  Ground signal
TRIG - Any available GPIO pin capable of output
ECHO - Any available GPIO pin 

Note:
When connecting ECHO to ESP32 pin be aware that if using a 5v version of SR04 a level shifter should be used.
OK for short tests but for long term use connect a resistor between SR04 output and ESP32 pin(10K) to limit current.


> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Hardware requirements

Any ESP32 hardware device running a nanoFramework image with HC-SR04P 3.3V version ( HC-SR04 5v version )

## Related topics

### Reference

## Build the sample

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build. 
2. Start Microsoft Visual Studio 2019 (VS 2017 should be OK too)/2019 and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the sub folder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
4. Add the connection and certificate information to the program.
5. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select Build > Deploy Solution.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging.
