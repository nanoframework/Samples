[![Discord](https://img.shields.io/discord/478725473862549535.svg)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://github.com/nanoframework/Home/blob/main/resources/logo/nanoFramework-repo-logo.png)

-----
文档语言: [English](README.md) | [简体中文](README.zh-cn.md)

# 欢迎使用 **nanoFramework** 例程库！

本库包括了团队在测试时使用的例程代码，用于验证新增功能以及其它实验室探索。
随意浏览，随心所欲，尽享回报。

> 注意：有时候调试项目时，引用项目比引用Nuget包更方便，可以直接跟踪进入内部逻辑。因此，建议安装VS扩展，用于把Nuget引用切换为项目引用。 [NuGet Reference Switcher](https://github.com/rsuter/NuGetReferenceSwitcher).

## 例程分类

### 通信

<devices>

### Special beginner

* [🌶️ - Blink your first led](samples/Blinky)
* [🌶️ - Blink your first LED](samples/Beginner/BlinkLed)
* [🌶️ - Press a button and light a LED](samples/Beginner/Button)
* [🌶️ - Press a button and light a LED with a Button nuget](samples/Beginner/ButtonNuget)
* [🌶️ - Pulse Width Modulation (PWM) and changing the light of a LED](samples/Beginner/PwmLed)
* [🌶️ - Read a soil moisture sensor with Analog to Digital Converter (ADC)](samples/Beginner/AnalogRainSensor)
* [🌶️ - Reading an AM2320 I2C Temerature and Humidity sensor](samples/Beginner/TempHumI2c)
* [🌶️ - Special IoT beginner content with .NET nanoFramework](samples/Beginner)
* [🌶️ - System.Device.Pwm](samples/PWM/System.Device.Pwm)
* [🌶️ - ulse Width Modulation (PWM) to drive a servo motor](samples/Beginner/ServoMotor)

### Gpio, I2C, I2S, Spi, Pwm, Adc, Dac, 1-Wire, Serial

* [🌶️ - 1-Wire sample](samples/1-Wire)
* [🌶️ - Analogic/Digital converter](samples/ADC)
* [🌶️ - Analogic/Digital converter](samples/GiantGecko.Adc)
* [🌶️ - Blink your first led](samples/Blinky)
* [🌶️ - Blink your first LED](samples/Beginner/BlinkLed)
* [🌶️ - Digital Analog Converter sample](samples/DAC)
* [🌶️ - ESP32 Pulse Counter sample](samples/Gpio/Esp32PulseCounter)
* [🌶️ - GPIO and events sample](samples/Gpio/Gpio+Events)
* [🌶️ - GPIO and events sample (.NET IoT style)](samples/Gpio/Gpio+EventsIoTStyle)
* [🌶️ - GPIO sample pack](samples/Gpio)
* [🌶️ - Press a button and light a LED](samples/Beginner/Button)
* [🌶️ - Press a button and light a LED with a Button nuget](samples/Beginner/ButtonNuget)
* [🌶️ - Pulse Width Modulation (PWM) and changing the light of a LED](samples/Beginner/PwmLed)
* [🌶️ - Read a soil moisture sensor with Analog to Digital Converter (ADC)](samples/Beginner/AnalogRainSensor)
* [🌶️ - Reading an AM2320 I2C Temerature and Humidity sensor](samples/Beginner/TempHumI2c)
* [🌶️ - System.Device.Pwm](samples/PWM/System.Device.Pwm)
* [🌶️ - System.Device.PWM sample](samples/PWM)
* [🌶️ - System.Device.Spi sample](samples/SPI)
* [🌶️ - ulse Width Modulation (PWM) to drive a servo motor](samples/Beginner/ServoMotor)
* [🌶️ to 🌶️🌶️ - System.IO.Ports serial Communication sample](samples/SerialCommunication)
* [🌶️🌶️ - ESP32-S3-BOX-Lite screen sample and ADC buttons](samples/Graphics/Screens/esp32-s3-box-lite)
* [🌶️🌶️ - I2C sample sample pack](samples/I2C)
* [🌶️🌶️ - I2C Scanner sample](samples/I2C/NanoI2cScanner)
* [🌶️🌶️ - I2S Microphone sample](samples/I2S/Input)
* [🌶️🌶️ - I2S sample pack](samples/I2S)
* [🌶️🌶️ - I2S Speaker sample](samples/I2S/Output)
* [🌶️🌶️ - Waveshare ESP32-S3-Touch-LCD-2 screen test](samples/Graphics/Screens/waveshare-esp32-s3-touch-lcd-2)
* [🌶️🌶️ - WebServer to test pour Servo Motors](samples/Webserver/ServoMotorTester)
* [🌶️🌶️🌶️ - Using Azure SDK with BMP280 on M5Stack with .NET nanoFramework](samples/AzureSDK/AzureSDKSensorCertificate/Readme.md)

### AMQP

* [🌶️🌶️ - AMQP sample pack](samples/AMQP)
* [🌶️🌶️ - Azure Service Bus AMQP sample](samples/AMQP/Azure-ServiceBus-Sender)
* [🌶️🌶️ -Azure AMQP sample](samples/AMQP/Azure-IoT-Hub)

### Azure specific

* [🌶️🌶️ - AMQP sample pack](samples/AMQP)
* [🌶️🌶️ - Azure IoT Hub SDK with MQTT protocol](samples/AzureSDK/AzureSDK)
* [🌶️🌶️ - Azure IoT Plug & Play with MQTT protocol](samples/AzureSDK/AzureIoTPnP)
* [🌶️🌶️ - Azure Service Bus AMQP sample](samples/AMQP/Azure-ServiceBus-Sender)
* [🌶️🌶️ - Simple sample **with** Azure lib and retry pattern for connection](samples/AzureSDK/AzureSDKBasic)
* [🌶️🌶️ - Using a Modem like SIM7080, simple sample **with** Azure lib and retry pattern for connection](samples/AzureSDK/AzureSDKBasicFullyManaged)
* [🌶️🌶️ -Azure AMQP sample](samples/AMQP/Azure-IoT-Hub)
* [🌶️🌶️ to 🌶️🌶️🌶️ - Azure SDK sample pack](samples/AzureSDK)
* [🌶️🌶️🌶️ - Azure Edge OTA example](samples/AzureSDK/AzureEdgeOta)
* [🌶️🌶️🌶️ - Complete Azure MQTT sample using BMP280 sensor **with** Azure lib and deep sleep](samples/AzureSDK/AzureSDKSleepBMP280)
* [🌶️🌶️🌶️ - Complete Azure MQTT sample using BMP280 sensor **without Azure lib** and with deep sleep](samples/AzureMQTTTwinsBMP280Sleep)
* [🌶️🌶️🌶️ - HTTP.HttpAzureGET Sample](samples/HTTP/HttpAzureGET)
* [🌶️🌶️🌶️ - HTTP.HttpAzurePOST Sample](samples/HTTP/HttpAzurePOST)
* [🌶️🌶️🌶️ - Using Azure SDK with BMP280 on M5Stack with .NET nanoFramework](samples/AzureSDK/AzureSDKSensorCertificate/Readme.md)
* [Azure IoT Device Provisioning Service (DPS) example](samples/AzureSDK/DpsSampleApp)

### Bluetooth

* [🌶️ - Bluetooth Low Energy Serial profile sample](samples/Bluetooth/BluetoothLESerial)
* [🌶️ - Bluetooth Low energy: adding, replacing services to the main service](samples/Bluetooth/BluetoothLESample3)
* [🌶️ to 🌶️🌶️🌶️ - Bluetooth sample pack](samples/Bluetooth)
* [🌶️🌶️ -  Demonstrates how to use the watcher filter classes](samples/Bluetooth/WatcherFilters)
* [🌶️🌶️ - Bluetooth Low energy: Broadcast current values in a Bluetooth advertisement](samples/Bluetooth/BroadcastValues)
* [🌶️🌶️ - Bluetooth Low energy: Environmental Sensor data collection](samples/Bluetooth/Central2)
* [🌶️🌶️ - Bluetooth Low energy: read static and dynamic values, notification, read/write value](samples/Bluetooth/BluetoothLESample1)
* [🌶️🌶️ - Bluetooth Low energy: read/write with encryption a value](samples/Bluetooth/BluetoothLESample2)
* [🌶️🌶️ - Bluetooth Low energy: Watch for Bluetooth Advertisements](samples/Bluetooth/Central1)
* [🌶️🌶️🌶️ -  Create an IBeacon](samples/Bluetooth/BluetoothBeacon)
* [🌶️🌶️🌶️ - Bluetooth Low energy: Demonstrates pairing and authentication in Client program](samples/Bluetooth/Central3)
* [🌶️🌶️🌶️ - Improv Wifi provisioning](samples/Bluetooth/ImprovWifi)

### CAN

* [🌶️ - CAN sample](samples/CAN)

### ESP32 specific

* [🌶️ - ESP32 Pulse Counter sample](samples/Gpio/Esp32PulseCounter)
* [🌶️ - Hardware ESP32 Deep sleep sample](samples/Hardware.Esp32)
* [🌶️ - Touch pad ESP32 sample](samples/TouchESP32)
* [🌶️ to 🌶️🌶️🌶️ - Bluetooth sample pack](samples/Bluetooth)
* [🌶️🌶️ - Hardware ESP32 RMT sample pack](samples/Hardware.Esp32.Rmt)
* [🌶️🌶️ - Infrared remote receiver based on VS1838 with RMT](samples/Hardware.Esp32.Rmt/InfraredRemoteReceiver)
* [🌶️🌶️ - NeoPixel Strip WS2812 with RMT](samples/Hardware.Esp32.Rmt/NeoPixelStrip)
* [🌶️🌶️ - Simple sample **with** Azure lib and retry pattern for connection](samples/AzureSDK/AzureSDKBasic)
* [🌶️🌶️ - Ultrasonic HC-SR04 sensor with RMT](samples/Hardware.Esp32.Rmt/Ultrasonic)
* [🌶️🌶️ - Using a Modem like SIM7080, simple sample **with** Azure lib and retry pattern for connection](samples/AzureSDK/AzureSDKBasicFullyManaged)
* [🌶️🌶️ - Waveshare ESP32-S3-Touch-LCD-2 screen test](samples/Graphics/Screens/waveshare-esp32-s3-touch-lcd-2)
* [🌶️🌶️🌶️ -  Create an IBeacon](samples/Bluetooth/BluetoothBeacon)
* [🌶️🌶️🌶️ - Complete Azure MQTT sample using BMP280 sensor **with** Azure lib and deep sleep](samples/AzureSDK/AzureSDKSleepBMP280)
* [🌶️🌶️🌶️ - Complete Azure MQTT sample using BMP280 sensor **without Azure lib** and with deep sleep](samples/AzureMQTTTwinsBMP280Sleep)
* [🌶️🌶️🌶️ - Improv Wifi provisioning](samples/Bluetooth/ImprovWifi)

### File and storage access

* [🌶️ - System.IO.FileSystem samples](samples/System.IO.FileSystem)

### Giant Gecko specific

* [🌶️ to 🌶️🌶️ - Giant Gecko hardware sample pack](samples/Hardware.GiantGecko)
* [🌶️🌶️ - Giant Gecko Power Mode](samples/Hardware.GiantGecko/GiantGecko.PowerMode)

### Graphics for screens

* [🌶️🌶️ - ESP32-S3-BOX-Lite screen sample and ADC buttons](samples/Graphics/Screens/esp32-s3-box-lite)
* [🌶️🌶️ - Graphics Primitives](samples/Graphics/Primitives)
* [🌶️🌶️ - Screen samples](samples/Graphics/Screens)
* [🌶️🌶️ - Using an existing generic graphic driver](samples/Graphics/UsingGenericDriver)
* [🌶️🌶️ - Waveshare ESP32-S3-Touch-LCD-2 screen test](samples/Graphics/Screens/waveshare-esp32-s3-touch-lcd-2)
* [🌶️🌶️ -Simple WPF](samples/Graphics/SimpleWpf)
* [🌶️🌶️ to 🌶️🌶️🌶️ -Graphics samples](samples/Graphics)
* [🌶️🌶️🌶️ - Creating your own generic graphic driver](samples/Graphics/GenericDriver)
* [🌶️🌶️🌶️ - Tetris Demo Game for nanoFramework](samples/Graphics/Tetris)

### IoT.Device

* [🌶️ - Press a button and light a LED with a Button nuget](samples/Beginner/ButtonNuget)
* [🌶️🌶️🌶️ - Complete Azure MQTT sample using BMP280 sensor **with** Azure lib and deep sleep](samples/AzureSDK/AzureSDKSleepBMP280)
* [🌶️🌶️🌶️ - Complete Azure MQTT sample using BMP280 sensor **without Azure lib** and with deep sleep](samples/AzureMQTTTwinsBMP280Sleep)
* [🌶️🌶️🌶️ - Using Azure SDK with BMP280 on M5Stack with .NET nanoFramework](samples/AzureSDK/AzureSDKSensorCertificate/Readme.md)

### Interop

* [🌶️🌶️ - Native events sample](samples/NativeEvents)
* [🌶️🌶️🌶️ - Interop sample](samples/Interop)

### Json

* [🌶️🌶️ - nanoFramework Json sample](samples/Json)

### Model Context Protocol (MCP)

* [🌶️🌶️ - Model Context Protocol (MCP) sample](samples/McpAi)

### MQTT

* [🌶️🌶️ to 🌶️🌶️🌶️ - MQTT sample pack](samples/MQTT)
* [🌶️🌶️🌶️ - Complete Azure MQTT sample using BMP280 sensor **without Azure lib** and with deep sleep](samples/AzureMQTTTwinsBMP280Sleep)

### Networking including HTTP, SSL

* [🌶️ - UdpClient sample pack](samples/UdpClient)
* [🌶️ - WebSocket 客户端示例](samples/WebSockets/WebSockets.Client.Sample/README.zh-cn.md)
* [🌶️ - WiFI 示例](samples/Wifi/README.zh-cn.md)
* [🌶️ to 🌶️🌶️ - .NET **nanoFramework** Webserver sample pack](samples/Webserver)
* [🌶️ to 🌶️🌶️ - WebSocket示例包](samples/WebSockets/README.zh-cn.md)
* [🌶️🌶️ - HTTP Listener sample](samples/HTTP/HttpListener)
* [🌶️🌶️ - HTTP sample pack](samples/HTTP)
* [🌶️🌶️ - HTTP WebRequest sample](samples/HTTP/HttpWebRequest)
* [🌶️🌶️ - Model Context Protocol (MCP) sample](samples/McpAi)
* [🌶️🌶️ - Networking sample pack](samples/Networking)
* [🌶️🌶️ - OpenTHread Networking sample pack](samples/OpenThread)
* [🌶️🌶️ - WebServer to test pour Servo Motors](samples/Webserver/ServoMotorTester)
* [🌶️🌶️ - WebSocket ServerClient Sample](samples/WebSockets/Websockets.ServerClient.Sample/README.zh-cn.md)
* [🌶️🌶️ - WebSocket Server示例与RGB Led](samples/WebSockets/WebSockets.Server.RgbSample/README.zh-cn.md)
* [🌶️🌶️ - Wifi Soft AP sample](samples/WiFiAP)
* [🌶️🌶️ to 🌶️🌶️🌶️ - MQTT sample pack](samples/MQTT)
* [🌶️🌶️ to 🌶️🌶️🌶️ - TLS sample pack](samples/SSL)
* [🌶️🌶️🌶️ - HTTP.HttpAzureGET Sample](samples/HTTP/HttpAzureGET)
* [🌶️🌶️🌶️ - HTTP.HttpAzurePOST Sample](samples/HTTP/HttpAzurePOST)

### Real Time Clock

* [🌶️ - RTC sample](samples/RTC)

### STM32 Specific

* [🌶️ - Giant Gecko Read Device IDs](samples/Hardware.GiantGecko/GiantGecko.ReadDeviceIDs)
* [🌶️ - STM32 Alarm](samples/Hardware.Stm32/Stm32.TestAlarms)
* [🌶️ - STM32 Read Device ID](samples/Hardware.Stm32/Stm32.ReadDeviceIDs)
* [🌶️ to 🌶️🌶️ - Hardware STM32 sample pack](samples/Hardware.Stm32)
* [🌶️🌶️ - STM32 Backup Memory](samples/Hardware.Stm32/Stm32.BackupMemory)
* [🌶️🌶️ - STM32 Power Mode](samples/Hardware.Stm32/Stm32.PowerMode)

### Texas Instrument specific

* [🌶️ - Texas Instruments EasyLink sample pack](samples/TI.EasyLink)
* [🌶️ - TI utilities read IEEE address](samples/Hardware.TI/TI.Utilities)
* [🌶️ to 🌶️🌶️ - Hardware TI SimpleLink sample pack](samples/Hardware.TI)
* [🌶️🌶️ - TI Power Mode](samples/Hardware.TI/TI.PowerMode)

### Tools and utilities

* [🌶️ - Dependency injection sample pack](samples/DependencyInjection)
* [🌶️ - Logging samples](samples/Logging)
* [🌶️ - Unit Test framework sample pack](samples/UnitTest)
* [🌶️🌶️ - Hosting sample pack](samples/Hosting)

### System related

* [🌶️ - Collections sample](samples/Collections)
* [🌶️ - Convert Base64 sample pack](samples/Converter.Base64)
* [🌶️ - Debug Garbage Collector Test](samples/DebugGC.Test)
* [🌶️ - GPIO and events sample](samples/Gpio/Gpio+Events)
* [🌶️ - GPIO and events sample (.NET IoT style)](samples/Gpio/Gpio+EventsIoTStyle)
* [🌶️ - Managed resources sample](samples/ManagedResources)
* [🌶️ - Number Parsing sample pack](samples/NumberParser)
* [🌶️ - RTC sample](samples/Timer)
* [🌶️ - System.Random sample](samples/System.Random)
* [🌶️ - ToString samples](samples/ToStringTest)
* [🌶️ to 🌶️🌶️ - Threading sample pack](samples/Threading)
* [🌶️🌶️ - Execution Constraint demo](samples/ExecutionConstraint)
* [🌶️🌶️ - GC stress test](samples/GCStressTest)
* [🌶️🌶️ - Native events sample](samples/NativeEvents)
* [🌶️🌶️ - Reflection sample pack](samples/Reflection)
* [🌶️🌶️🌶️ - Interop sample](samples/Interop)

### USB Client related

* [🌶️🌶️ - System.Device.UsbClient sample pack](samples/UsbClient)

### Wifi

* [🌶️ - WiFI 示例](samples/Wifi/README.zh-cn.md)
* [🌶️🌶️ - Wifi Soft AP sample](samples/WiFiAP)
* [🌶️🌶️🌶️ - Improv Wifi provisioning](samples/Bluetooth/ImprovWifi)

</devices>

## 文档反馈

有关文档、提供反馈、问题以及如何做出贡献的信息，请参阅 [Home repo](https://github.com/nanoframework/Home).

加入我们的讨论社区 [here](https://discord.gg/gCyBu8T).

## 信用

本项目贡献者可在 [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md) 中找到

## 授权

**nanoFramework** 例程基于 [MIT license](https://opensource.org/licenses/MIT) 授权。

## 行为准则

本项目采用了 [Contributor Covenant](http://contributor-covenant.org/) 规范来阐明社区预期行为。
