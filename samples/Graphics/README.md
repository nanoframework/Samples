# Graphics samples

These samples show how utilizing graphics on displays can be achieved using nanoFramework.

**THEY ARE STILL WORK IN PROGRESS so may not work as expected (or at all).
Currently the touch implementation is incomplete.**

Current targets are for the samples are ESP32 and the STM32F769I_DISCOVERY. But should work for other platforms that use supported graphic controllers.

> **CRITICAL**: all display **must** be initialized before being able to be used.

This initialization can be different depending on your device. ESP32 devices must be properly initialize to work, the GPIO must match the functions. You also need to know the exact size of the screen. See the [M5Stack](../Screens/README.md) and M5Stick examples for more configurations.

```csharp
int backLightPin = 32;
int chipSelect = 14;
int dataCommand = 27;
int reset = 33;
// Add the nanoFramework.Hardware.Esp32 to the solution
Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);
Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);
// Adjust as well the size of your screen and the position of the screen on the driver
DisplayControl.Initialize(new SpiConfiguration(1, chipSelect, dataCommand, reset, backLightPin), new ScreenConfiguration(0, 0, 320, 240));
// Depending on you ESP32, you may also have to use either PWM either GPIO to set the backlight pin mode on
// GpioController.OpenPin(backLightPin, PinMode.Output);
// GpioController.Write(backLightPin, PinValue.High);
```

> **IMPORTANT**: If your ESP32 does not have SPRAM, you won't be able to get a full frame buffer. In this case, you can only use the primitives to write text, draw (small rectangles) and points. You can adjust the amount of memory you are requesting.

For STM32 devices the pins setup are by default in most cases, you can directly use:

```csharp
//WARNING: Invalid pin mappings will never be returned, and may need you to reflash the device!
DisplayControl.Initialize(new SpiConfiguration(), new ScreenConfiguration());
```

> **In case the screen is wrongly initialize, the device will hang and you may have to reflash it.**

## Primitives

This demonstrates the low level graphic functions that are available and the primitive functions used by the WPF

These can also be used with smaller screens on more memory constrained devices.

- ESP32 without PSRAM

## Screens

This demonstrates the low level text display function that is available. Very useful for memory constrained devices with smaller screens.

## SimpleWPF

This contains an animated graphical menu with allows the selection of pages that demonstrates features of the limited WPF that's available in nanoFramework.

- Vertical Stack Panel
- Horizontal Stack panel
- Canvas panel
- Scrollable text Panel
- Free drawing panel

Requires the GPIO pins numbers to be defined for the Left, Right, Up, Down & Select keys, see Program.cs

## Tetris

This is a nanoFramework version of the Tetris game with high scores.

Requires the GPIO pins numbers to be defined for the Left, Right, Up, Down & Select keys.

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
