# 🌶️🌶️ - Waveshare ESP32-S3-Touch-LCD-2 screen test

This sample drives the **Waveshare ESP32-S3-Touch-LCD-2** board's 2-inch
**240 x 320 ST7789** LCD in portrait mode using a **managed graphic driver**.
The controller commands are defined in [St7789Driver.cs](./St7789Driver.cs);
the firmware's generic SPI graphics interface handles transport.

This is a display-only test: the CST816D touch controller, QMI8658 IMU, camera,
and microSD card are not initialized. No external wiring is needed; the NinaB
font is embedded in the application. This sample is not for the similarly named LCD-2.1 or LCD-2.8 boards.

## Firmware: octal, not quad

Flash **`ESP32_S3_OCTAL`**. Waveshare specifies an **ESP32-S3R8**, with **8 MB
octal PSRAM** and **16 MB external flash**. The nanoFramework OCTAL/QUAD choice
here refers to the **PSRAM interface**, not the display's SPI bus or the flash
capacity. Do not choose `ESP32_S3_QUAD` for this board.

The firmware must include **nanoFramework.Graphics with the Generic SPI graphic
driver**. The published `ESP32_S3_OCTAL` target currently includes it. A firmware
with no graphics support cannot run this sample; one with a fixed native LCD
driver will not use the managed initialization sequence.

Connect with a USB data cable. Close any serial terminal/debug session before
flashing and replace `COMx` with the board's port:

```powershell
# nanoFramework.Tools.NanoFirmwareFlasher 2.x
nanoff --target ESP32_S3_OCTAL --serialport COMx --update
```

For NanoFirmwareFlasher 3.x, the equivalent command is:

```powershell
nanoff flash target ESP32_S3_OCTAL serialport COMx
```

If necessary, hold **BOOT**, press and release **RESET**, then release BOOT to
enter download mode. The USB COM port can change in download mode. Reset after
flashing, select the running nanoFramework device in your IDE, and deploy the
application. Flashing replaces the manufacturer's demo; back it up first if
you need to preserve it.

## Display wiring

The pinout comes from Waveshare's downloadable Arduino and ESP-IDF demos:

| LCD signal | GPIO |
| ---------- | ---- |
| MOSI | 38 |
| Clock | 39 |
| CS | 45 |
| DC | 42 |
| Backlight (active high) | 1 |
| Reset | Not connected to an ESP32 GPIO; use `-1` |

GPIO40 is the board's shared SPI MISO line. The LCD itself is write-only, but
the sample routes MISO too, as required by the ESP32 graphics SPI setup.
Backlight GPIO1 is explicitly opened as a GPIO output and driven HIGH before
graphics initialization. The Generic SPI driver configures a backlight output
but does not switch it on; merely mapping PWM1 is not sufficient. The sample
passes -1 for the graphics backlight parameter so it leaves GPIO1 alone.
The driver uses software reset, RGB565 pixels,
color inversion for the IPS panel, and no display RAM offset.

The portrait MADCTL value is `0x08`: no mirroring or axis swap, with BGR order
to match `Color.ToBgr565()` and `DisplayControl.Write`. Only portrait orientation
is defined by this sample.

The Generic SPI backend stores the `ScreenConfiguration` dimensions as longer
side then shorter side, so the sample passes **320, 240**, then selects portrait
in the managed driver to obtain a **240 x 320** drawing area. It checks the
reported dimensions before drawing.

## Build and run

Open [WaveshareEsp32S3TouchLcd2.sln](./WaveshareEsp32S3TouchLcd2.sln) in an IDE
with the .NET nanoFramework extension installed. Restore NuGet packages, build,
select your device, and deploy/start the application.

From a Developer PowerShell with NuGet and nanoFramework MSBuild components
available, run these commands from this sample directory:

```powershell
nuget restore .\WaveshareEsp32S3TouchLcd2.sln
msbuild .\WaveshareEsp32S3TouchLcd2.sln /p:platform="Any CPU" /p:Configuration=Release /verbosity:minimal
```

### Expected result

1. The backlight turns on; debug output reports `Backlight enabled (GPIO1 HIGH)`.
2. The whole display turns **red, green, blue, white, then black**, each for one
   second. Debug output names each color, making swapped color channels obvious.
3. A text page appears for **10 seconds**, with a yellow heading and white text
   on a dark blue background. It includes two pangrams (each covers A-Z):
   - "The quick brown fox jumps over the lazy dog."
   - "Pack my box with five dozen liquor jugs."
   - Separate uppercase/lowercase alphabets, digits, and punctuation.
4. A persistent pattern appears:
   - Six vertical bars in the upper half: **red, green, blue, yellow, cyan, magenta**.
   - A left-to-right **black-to-white gray ramp** below the bars.
   - A **one-pixel white border** around the entire 240 x 320 screen.
   - Corner markers: **red top-left, green top-right, blue bottom-left,
     yellow bottom-right**, inside the border.
5. A **white square moves left and right** in the bottom black band. This tests
   repeated partial-window writes, not just initial full-screen drawing.
6. Debug output periodically reports `Screen test running`.

Reset the board to repeat the color and text tests. Pixel data is sent through
`DisplayControl.Write` using a reusable eight-row buffer (3,840 bytes), rather
than allocating a full-screen managed bitmap. There is no allocation per animation
frame and no dependency on the native direct-text drawing path.

### Text rendering

The sample reuses the NinaB tinyfont from the graphics primitives sample. Unlike
some of the other bundled fonts, it includes all the punctuation used in this
text test as well as both alphabets. The font is loaded through the embedded
[Resource.resx](./Resource.resx) and [Resource.Designer.cs](./Resource.Designer.cs).

The Generic SPI driver does not support native direct-text writes. Instead,
`DrawTextTest` renders each line into one reusable off-screen `Bitmap`, converts
its pixels to BGR565, and sends them with `DisplayControl.Write`. The bitmap is
only one text line high and is disposed after the page is drawn. Each line is
measured before drawing, so a font/layout mismatch throws instead of silently
clipping text. Debug output also logs every line.

### Troubleshooting

- **Deployment reports `E4005: Failed to open specified COM port`:** stop the
  current IDE debug session and close serial monitors or other tools using the
  port, then deploy again. A firmware reflash is not required for application
  changes.
- **No debug startup message:** check deployment, selected device, USB cable,
  and reset/boot state before investigating the LCD.
- **Panel stays completely dark:** look for `Backlight enabled (GPIO1 HIGH)`.
  If it is absent, capture the startup exception/output. If it is present, check
  the board model and backlight circuit/power. No PWM configuration is needed.
- **Graphics initialization throws:** check the firmware's generic SPI graphics
  support and that its native assembly versions match the NuGet packages.
- **Backlight on but no pattern:** verify this is the exact LCD-2 board and the
  pin mapping above. Do not substitute a reset GPIO from another board.
- **Red and blue swapped:** check that the managed driver is actually being used,
  with MADCTL `0x08` and BGR565 pixel buffers.
- **Missing edges or wrapped/sheared image:** check the 240 x 320 portrait
  dimensions, zero offsets, and that no landscape axis swap has been added.

Physical display behavior must be verified on the board; a successful build
alone cannot confirm wiring, colors, orientation, or panel operation.

## References

- [Waveshare product specifications](https://www.waveshare.com/esp32-s3-touch-lcd-2.htm)
- [Waveshare wiki, demo download, and schematic](https://www.waveshare.com/wiki/ESP32-S3-Touch-LCD-2)
- [ESP32_S3_OCTAL firmware configuration (includes Generic SPI graphics)](https://github.com/nanoframework/nf-interpreter/blob/main/targets/ESP32/defconfig/ESP32_S3_OCTAL_defconfig)
- [Octal PSRAM firmware configuration](https://github.com/nanoframework/nf-interpreter/blob/main/targets/ESP32/_IDF/sdkconfig.default_octal_ble.esp32s3)
- [Other screen samples](../README.md)
