using System;
using System.Device.Pwm;
using System.Diagnostics;
using System.Threading;

Debug.WriteLine("Hello from Pwm!");

bool goingUp = true;
float dutyCycle = .00f;

// Please adjust the chip ID, formally TIM and the channel, in this case the pin number
PwmChannel pwmPin = new(1, 2, 40000, 0.5);
pwmPin.Start();

for (; ; )
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
    pwmPin.DutyCycle = dutyCycle;

    Thread.Sleep(50);
}

Thread.Sleep(Timeout.Infinite);
