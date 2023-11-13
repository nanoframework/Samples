# 🌶️ - ESP32 Pulse Counter sample

Shows how to use the [ESP32 Pulse Counter](https://github.com/nanoframework/nanoFramework.Hardware.Esp32/blob/main/nanoFramework.Hardware.Esp32/Gpio/Gpio%E2%80%8BPulseCounter.cs) API allowing your count pulses on GPIO pins.

- Create a specific Pulse Count GPIO pins
- Read counter of input GPIO pins

## Hardware requirements

Any hardware device running a nanoFramework image built with GPIO support enabled.

## Related topics

### Samples

Pulse counter allows to count pulses without having to setup a GPIO controller and events. It's a fast way to get count during a specific amount of time. This pulse counter allows as well to use 2 different pins and get a pulse count depending on their respective polarities.

### Pulse Counter with 1 pin

The following code illustrate how to setup a counter for 1 pin:

```csharp
GpioPulseCounter counter = new GpioPulseCounter(26);
counter.Polarity = GpioPulsePolarity.Rising;
counter.FilterPulses = 0;

counter.Start();
int inc = 0;
GpioPulseCount counterCount;
while (inc++ < 100)
{
    counterCount = counter.Read();
    Console.WriteLine($"{counterCount.RelativeTime}: {counterCount.Count}");
    Thread.Sleep(1000);
}

counter.Stop();
counter.Dispose();
```

The counter will always be positive and incremental. You can reset to 0 the count by calling the `Reset` function:

```csharp
GpioPulseCount pulses = counter.Reset();
// pulses.Count contains the actual count, it is then put to 0 once the function is called
```

### Pulse Counter with 2 pins

This is typically a rotary encoder scenario. In this case, you need 2 pins and they'll act like in this graphic:**

![rotary encoder principal](https://github.com/nanoframework/nanoFramework.IoT.Device/blob/develop/devices/RotaryEncoder/encoder.png?raw=true)

You can then use a rotary encoder connected to 2 pins:

![rotary encoder](https://github.com/nanoframework/nanoFramework.IoT.Device/blob/develop/devices/RotaryEncoder/RotaryEncoder.Sample_bb.png?raw=true)

The associated code is the same as for 1 pin except in the constructor:

```csharp
GpioPulseCounter encoder = new GpioPulseCounter(12, 14);
encoder.Start();
int incEncod = 0;
GpioPulseCount counterCountEncode;
while (incEncod++ < 100)
{
    counterCountEncode = encoder.Read();
    Console.WriteLine($"{counterCountEncode.RelativeTime}: {counterCountEncode.Count}");
    Thread.Sleep(1000);
}

encoder.Stop();
encoder.Dispose();
```

As a result, you'll get positives and negative pulses.

### Reference

[nanoFramework.Hardware.Esp32](https://github.com/nanoframework/nanoFramework.Hardware.Esp32)

## System requirements

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
