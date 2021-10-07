# Graphics samples

These samples show how utilizing graphics on displays can be achieved using nanoFramework.

**THEY ARE STILL WORK IN PROGRESS so may not work as expected (or at all).
Currently the touch implementation is incomplete.**

Current targets are for the samples are ESP32 and the STM32F769I_DISCOVERY. But should work for other platforms that use supported graphic controllers.

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
