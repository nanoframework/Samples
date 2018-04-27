//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Windows.Devices.Gpio;
using System;
using System.Threading;
using Windows.Devices.Pwm;

namespace PwmTest
{
	public class Program
    {
        public static void Main()
        {
            bool goingUp = true;
            float dutyCycle = .00f;

            // there is no PWM output pin connected to an LED in STM32F769I_DISCO
            // the closest one is LD3 connected to PA12 and exposed in Arduino connector, pad D13
            // need to set this as input 
            GpioPin dummyPad = GpioController.GetDefault().OpenPin(PinNumber('A', 12));
            dummyPad.SetDriveMode(GpioPinDriveMode.Input);

            PwmController pwmController;
            PwmPin pwmPin;

            // we'll be using PA11, exposed in Arduino connector, pad D10,
            // as the PWM output pin, this one is TIM11_CH4
            pwmController = PwmController.FromId("TIM1");
            pwmController.SetDesiredFrequency(5000);

            // open the PWM pin
            pwmPin = pwmController.OpenPin(PinNumber('A', 11));
            // set the duty cycle
            pwmPin.SetActiveDutyCyclePercentage(dutyCycle);
            // start the party
            pwmPin.Start();

            for (;;)
            {
                if (goingUp)
                {
                    // slowly increase light intensity
                    dutyCycle += 0.05f;

                    // change direction if reaching maximum duty cycle (100%)
                    if (dutyCycle > .95) goingUp = !goingUp;
                }
                else
                {
                    // slowly decrease light intensity
                    dutyCycle -= 0.05f;

                    // change direction if reaching minimum duty cycle (0%)
                    if (dutyCycle < 0.10) goingUp = !goingUp;
                }

                // update duty cycle
                pwmPin.SetActiveDutyCyclePercentage(dutyCycle);

                Thread.Sleep(50);
            }
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
