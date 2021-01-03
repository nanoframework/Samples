These samples show how utilising graphics on displays can be achieved using nanoFramework.

Current targets are for the samples are ESP32 and the STM32F769I_DISCOVERY. But should work for other platforms that use supported graphic controllers.

# Primitives

This demonstrates the low level graphic functions that are available and the primitive functions used by the WPF

These can also be used with smaller screens on more memory constrained devices.

- ESP32 without PSRAM

# SimpleWPF

This contains an animated graphical menu with allows the selection of pages that demonstrates features of the limited WPF that's available in nanoFramework.

- Vertical Stack Panel
- Horizontal Stack panel
- Canvas panel
- Scrollable text Panel
- Free drawing panel

Requires the GPIO pins numbers to be defined for the Left, Right, Up, Down & Select keys, see Program.cs

# Tetris

This is a nanoFramwork version of the Tetris game with high scores.

Requires the GPIO pins numbers to be defined for the Left, Right, Up, Down & Select keys.



THEY ARE STILL WORK IN PROGRESS so may not work as expected (or at all).
Currently the touch implementation is incomplete.

