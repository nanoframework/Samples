# 🌶️🌶️ - ESP32-S3-BOX-Lite screen sample and ADC buttons

This sample shows how to drive the LCD of the Espressif **ESP32-S3-BOX-Lite** with
a **managed graphic driver** for the `ST7789(V)` display controller.

The sample was written against the pinout published in the official Espressif
Board Support Package ([esp-bsp / esp-box-lite](https://github.com/espressif/esp-bsp/tree/master/bsp/esp-box-lite)).

## The display

The ESP32-S3-BOX-Lite ships with a 2.4" **320 x 240** LCD driven by a
`ST7789(V)` controller over SPI, using 16-bit (RGB565) color. The panel needs
color inversion turned on.

| Signal        | GPIO   |
|---------------|--------|
| MOSI (DATA0)  | GPIO6  |
| CLK (PCLK)    | GPIO7  |
| CS            | GPIO5  |
| DC            | GPIO4  |
| RST           | GPIO48 |
| Backlight     | GPIO45 |

The managed driver (register set + initialization sequence) is defined in
[St7789BoxLiteDriver.cs](./St7789BoxLiteDriver.cs). Its initialization sequence is
based on the `ST7789` driver published in the
[`nanoFramework.Graphics.St7789`](https://www.nuget.org/packages/nanoFramework.Graphics.St7789)
NuGet package, but it is **not identical**: the orientation is customized for the
ESP32-S3-BOX-Lite panel (`MADCTL_MV | MADCTL_MY | MADCTL_BGR`, i.e. `swap_xy` +
`mirror_y`), whereas the stock driver uses `MADCTL_ML | MADCTL_BGR`. If you prefer
to reference the NuGet and use `St7789.GraphicDriver`, you will need to override
its `OrientationLandscape` to the value above — otherwise the image is sheared
("off by a rotation").

> [!IMPORTANT]
>
> To use a managed (generic) graphic driver you **MUST** run a .NET nanoFramework
> firmware image that was built with the Generic Graphic Driver. On an image that
> was built with a specific native driver, the managed driver is ignored; on an
> image without any graphic library it will throw.

## Is there a specific sensor?

The only onboard "sensors" are the **dual digital microphones**, fed through the
`ES7243E` audio ADC and paired with an `ES8156` DAC. Audio codecs are controlled
over I2C (`SCL = GPIO18`, `SDA = GPIO8`).This is not implement (yet) on
[nanoFramework.IoT.Device](https://github.com/nanoFramework/nanoFramework.IoT.Device),
so it won't be used in this sample.

The board also exposes three
resistive-ladder navigation buttons read through an ADC (see next section).

Because there is no dedicated motion/environmental sensor to demonstrate, this
sample focuses on the screen driver and ADC buttons.

## Buttons

The three front navigation buttons (**Previous / Enter / Next**) are **not** on
separate GPIOs. They are a resistor ladder on a single analog input —
**ADC1 channel 0, which is GPIO1** — and are distinguished by voltage
(from the `esp-box-lite` BSP):

- Previous ≈ 2410 mV
- Enter ≈ 1980 mV
- Next ≈ 820 mV

Because of that, a per-pin `GpioButton` cannot be used. Instead
[BoxLiteButtons.cs](./BoxLiteButtons.cs) samples the ADC and drives one
`AdcButton` per voltage window. `AdcButton` derives from `ButtonBase` in the
[`nanoFramework.Iot.Device.Button`](https://www.nuget.org/packages/nanoFramework.Iot.Device.Button)
NuGet, so each button raises the usual `Press`, `DoublePress` and `Holding`
events:

```csharp
BoxLiteButtons buttons = new BoxLiteButtons();
buttons.Enter.Press += (sender, e) => Debug.WriteLine("Enter: press");
```

When a button is pressed its name is also written on the screen. The text is
rendered into an off-screen `Bitmap` and pushed with `DisplayControl.Write`
(the direct pixel path). This is required because the generic SPI driver does
not support the native text-drawing path of `DisplayControl.Write(text, ...)`.

> [!NOTE]
> The voltage windows and the ADC full-scale reference are approximate and may
> need tuning for your board/attenuation.

## Configuration

```csharp
// ESP32-S3 requires the SPI pins to be routed explicitly.
Configuration.SetPinFunction(6, DeviceFunction.SPI1_MOSI);
Configuration.SetPinFunction(7, DeviceFunction.SPI1_CLOCK);
// MISO is unused by the panel but a valid pin must still be set.
Configuration.SetPinFunction(39, DeviceFunction.SPI1_MISO);
Configuration.SetPinFunction(45, DeviceFunction.PWM1);

var spiConfig = new SpiConfiguration(1, 5, 4, 48, 45);
var screenConfig = new ScreenConfiguration(0, 0, 320, 240, St7789BoxLiteDriver.GetDriver());
DisplayControl.Initialize(spiConfig, screenConfig, 30 * 1024);
```

## Hardware requirements

An [ESP32-S3-BOX-Lite](https://github.com/espressif/esp-box) board.

## Related samples

Check the other [screen samples](../) and the
[generic driver sample](../../GenericDriver/) for more details on managed
graphic drivers.

Check out also [ADC samples](../../../ADC/)!
