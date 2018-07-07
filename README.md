![nanoFramework logo](https://github.com/nanoframework/Home/blob/master/resources/logo/nanoFramework-repo-logo.png)

-----

### Welcome to the **nanoFramework** team code samples repository!


This repo contains code samples used by the team when testing, working on proof of concepts for new and improved features and other explorational endeavours.
Feel free to browse, take what you like and contribute back if you want.

> Note: sometimes it's convenient to reference the source code instead of the NuGet packages in projects that require debugging by following the execution flow into other projects or even class libraries. For that we recommend using a very handy Visual Studio extension that allows NuGet assembly references to project references switching. That's [NuGet Reference Switcher](https://github.com/rsuter/NuGetReferenceSwitcher).

### Sample list

* [ADC](ADC) it's a sample solution to test reading voltages from the board ADC hardware.
* [Blinky](Blinky) it's a basic "Hello world" app that blinks an LED.
* [DebugGC.Test](DebugGC.Test) it's a sample solution for testing GC messages output.
* [GCStressTest](GCStressTest) it's a test application that highly stresses the GC with random object creation.
* [Gpio+Events.Test](Gpio+Events.Test) it's a sample project that constantly blinks an LED and turns another LED on/off reacting to the state of a button.
* [I2C](I2C) it's a sample project demoing the I2C API by connecting to a touchscreen controller and reacting to touch events.
* [Interop demonstration](Interop) it's a demonstration on how to create an Interop library and how to use it in another C# project.
* [PWM](PWM) it's a sample solution to test outputting a PWM signal to drive an LED increasing and decreasing its light intensity periodically.
* [RTC Sample](RTC) it's a sample solution for testing RTC related stuff.
* [SerialCommunication sample](/SerialCommunication) it's a sample solution for testing sending/receiving data using an UART (COM port)
* [SPI](SPI) it's a sample project demoing the SPI API by connecting to a MEMS gyroscope and reading the acceleration output.
* [System.Random](System.Random) it's a basic sample that shows how to use the various APIs to generate random numbers.
* [Timer sample](/Timer) it's a sample solution for testing timers and their callbacks.
* [ToString Test](ToStringTest) it's a sample solution for testing `ToString()` with integers, floats and doubles.
* [Hardware Esp32 Test](HardwareEsp32) it's a sample to show the features of the Hardware.Esp32 assembly.

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Slack community [here](https://join.slack.com/t/nanoframework/shared_invite/enQtMzI3OTg4MTk0NTgwLWQ0ODQ3ZWIwZjgxZWFmNjU3MDIwN2E2YzM2OTdhMWRiY2Q3M2NlOTk2N2IwNTM3MmRlMmQ2NTRlNjZlYzJlMmY).


## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/master/CONTRIBUTORS.md).


## License

The nanoFramework Interpreter is licensed under the [Apache 2.0 license](http://www.apache.org/licenses/LICENSE-2.0).


## Code of Conduct
This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community.
