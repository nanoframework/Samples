[![Discord](https://img.shields.io/discord/478725473862549535.svg)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://github.com/nanoframework/Home/blob/master/resources/logo/nanoFramework-repo-logo.png)

-----
Document Language: [English](README.md) | [简体中文](README.zh-cn.md)

## Welcome to the **nanoFramework** team code samples repository!

This repo contains code samples used by the team when testing, working on proof of concepts for new and improved features and other explorational endeavours.
Feel free to browse, take what you like and contribute back if you want.

> Note: sometimes it's convenient to reference the source code instead of the NuGet packages in projects that require debugging by following the execution flow into other projects or even class libraries. For that we recommend using a very handy Visual Studio extension that allows NuGet assembly references to project references switching. That's [NuGet Reference Switcher](https://github.com/rsuter/NuGetReferenceSwitcher).

## Sample by category

### Communication

<table>
 <tr>
  <td><a href="samples/SerialCommunication">SerialCommunication sample pack</a></td>
  <td><a href="samples/CAN">CAN sample</a></td>
  <td><a href="samples/AMQP">AMQP sample pack</a></td>
 </tr>
 <tr>
  <td><a href="samples/MQTT/TestMqtt">MQTT sample pack</a></td>
  <td><a href="samples/MQTT/AzureMQTT">MQTT sample with Azure IoT Hub</a></td>
  <td><a href="samples/MQTT/AwsMQTT">MQTT sample with Amazon Web Services (AWS) IoT</a></td>
 </tr>
</table>

### Devices

<table>
 <tr>
  <td><a href="samples/ADC">ADC</a></td>
  <td><a href="samples/1-Wire">1-Wire</a></td>
  <td><a href="samples/Gpio">Gpio</a></td>
 </tr>
<tr>
  <td><a href="samples/I2C">I2C</a></td>
  <td><a href="samples/PWM">PWM</a></td>
  <td><a href="samples/RTC">RTC Sample</a></td>
 </tr>
  <td><a href="samples/SPI">SPI</a></td>
  <td><a href="DAC">DAC</a></td>
  <td><!--<a href="RTXC">RTC Sample</a>--></td>
 </tr>
</table>

### Graphics
<table>
 <tr>
  <td><a href="samples/GraphicsWpf/Primitives">Primitives</a></td>
  <td><a href="samples/GraphicsWpf/SimpleWpf">SimpleWpf</a></td>
  <td><a href="samples/GraphicsWpf/Tetris">Tetris</a></td>
  <td><!--<a href="Utility/util3">Utility Three</a>--></td>
 </tr>
</table>

### Networking

<table>
 <tr>
  <td><a href="samples/Networking">Sockets sample pack</a></td>
  <td><a href="samples/SSL">SSL sample pack</a></td>
  <td><a href="samples/HTTP">HTTP</a></td>
  <td><a href="samples/Webserver">Webserver sample pack</a></td>
 </tr>
</table>

### ESP32
<table>
 <tr>
  <td><a href="samples/Hardware.Esp32">Hardware Esp32 Test</a></td>
  <td><a href="samples/Hardware.Esp32.Rmt">RMT interface</a></td>
  <td><a href="samples/Wifi">WiFi</a></td>
  <td><a href="samples/WiFiAP">WiFi Soft AP</a></td>
  <td><!--<a href="Utility/util3">Utility Three</a>--></td>
 </tr>
</table>

### STM32

<table>
 <tr>
  <td><a href="samples/Hardware.Stm32">Hardware STM32</a></td>
  <td><!--<a href="Utility/util2">Utility Two</a>--></td>
  <td><!--<a href="Utility/util3">Utility Three</a>--></td>
 </tr>
</table>

### TI CC13xx

<table>
 <tr>
  <td><a href="samples/TI.EasyLink">TI.EasyLink</a></td>
  <td><a href="samples/Hardware.TI">Hardware TI SimpleLink</a></td>
  <td><!--<a href="Utility/util3">Utility Three</a>--></td>
 </tr>
</table>

### System

<table>
 <tr>
  <td><a href="samples/Converter.Base64">Converter.Base64</a></td>
  <td><a href="samples/DebugGC.Test">Runtime GC Test</a></td>
  <td><a href="samples/ExecutionConstraint">Execution Constraint demo</a></td>
 </tr>
<tr>
  <td><a href="samples/ManagedResources">Resource Manager</a></td>
  <td><a href="samples/System.Random">System Random sample</a></td>
  <td><a href="samples/ToStringTest">ToString Test</a></td>
 </tr>
 <tr>
  <td><a href="samples/NumberParser">Number Parser Test</a></td>
  <td><a href="samples/Threading">Threading</a></td>
  <td><!--<a href="Utility/util3">Utility Three</a>--></td>
 </tr>
</table>

### Tools and utilities

<table>
 <tr>
  <td><a href="samples/UnitTest">Unit Test Framework</a></td>
  <td><!--<a href="samples/DebugGC.Test">Runtime GC Test</a>--></td>
  <td><!--<a href="samples/ExecutionConstraint">Execution Constraint demo</a>--></td>
 </tr>
<tr>
  <td><!--<a href="samples/ManagedResources">Resource Manager</a>--></td>
  <td><!--<a href="samples/System.Random">System Random sample</a>--></td>
  <td><!--<a href="samples/ToStringTest">ToString Test</a>--></td>
 </tr>
 <tr>
  <td><!--<a href="samples/NumberParser">Number Parser Test</a>--></td>
  <td><!--<a href="samples/Threading">Threading</a>--></td>
  <td><!--<a href="Utility/util3">Utility Three</a>--></td>
 </tr>
</table>

### Miscellaneous

<table>
 <tr>
  <td><a href="samples/Blinky">Blinky</a></td>
  <td><a href="samples/GCStressTest">GCStress test</a></td>
  <td><a href="samples/Interop">Interop demonstration</a></td>
 </tr>
 <tr>
  <td><a href="samples/Reflection">Reflection</a></td>
  <td><a href="samples/Timer">Timer sample</a></td>
  <td><a href="samples/Storage">Storage sample pack</a></td>
 </tr>
 <tr>
  <td><a href="samples/Json nanoFramework">nanoFramework Json</a></td>
  <td><a href="samples/NativeEvents">Native Events</a></td>
  <td><a href="samples/System.IO.FileSystem">File System sample</a></td>
 </tr>
</table>

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/master/CONTRIBUTORS.md).

## License

The **nanoFramework** samples are licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
