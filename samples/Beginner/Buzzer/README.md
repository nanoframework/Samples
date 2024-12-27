# 🌶️ - PWM and playing music with a buzzer

Shows how to use the [System.Device.Pwm](https://docs.nanoframework.net/api/System.Device.Pwm.html) API to use Pulse Width Modulation pins.

We will use the embedded LED present on the board for this case. You can also use an external LED with the 100-ohm resistor. You can follow the following [schema](../BlinkLed/README.md).

And if you want to know more about PWM, how this works, you can read the [All what you've always wanted to know about PWM](https://docs.nanoframework.net/content/getting-started-guides/pwm-explained.html) content!

We will use the pin 25 for this example. Just connect your buzzer on the ground on one pin and the pin 25. You can adjust the pin if you want, you'll have to adjust the code as well:

![schema buzzer](https://docs.nanoframework.net/devicesimages/Buzzer/Buzzer.Samples.wiring.png)

## Running the sample

Ensure you have all the [software requirements](../README.md#software-requirements).

To build the sample, follow the section [here](../README.md#build-the-sample). And to run it, [here](../README.md#run-the-sample).

The sample is [located here](./Program.cs). The code is using PWM to generate the frequencies for the soung. This is typically how the sound was produced in the early 80's on PC!

This sample uses the Buzzer nuget and the detailed [explanations are available here](https://github.com/nanoframework/nanoFramework.IoT.Device/tree/develop/devices/Buzzer).

And as a result, you will [hear like in this video](https://www.bing.com/videos/riverview/relatedvideo?q=nanoframework+buzzer&mid=225B785C50D46C4B31FC225B785C50D46C4B31FC&FORM=VIRE)!

If you want to debug, foillow the instructions [explained in the led sample](../BlinkLed//README.md#debugging).
