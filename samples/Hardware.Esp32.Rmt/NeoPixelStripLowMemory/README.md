# NeoPixel Strip WS2812 with RMT

Shows how to use ESP32 RMT with an NeoPixel Strip WS2812

This sample send signal to strip with pin D5.

**Connections**  
GIN -  Any available GPIO pin (D5 in the sample)
GND -  Ground signal  
+5V - External Power Supply 5V min 2A  

**Notes**:  
- Put resistor 400 Ohm between GIN and GPIO pin  
- Join the ground of the external power supply and the GND of ESP32 
- This example require a preview firmware dont use *-stable* parameter on *nanoff*

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Hardware requirements

- Any ESP32 hardware device running a nanoFramework image
- Led Strip NeoPixel WS2812
- External power supply 5V
- Resistor 400 Ohm
- Capacitor 1000 µF across + and - terminals external power suply

## Related topics

### Reference

## Build the sample

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build. 
2. Start Microsoft Visual Studio 2017/2019 and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the sub folder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
4. Add the connection and certificate information to the program.
5. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select Build > Deploy Solution.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging.
