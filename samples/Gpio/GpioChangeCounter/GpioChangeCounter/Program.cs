using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Gpio;
using Windows.Devices.Pwm;


//
//  This sample demostrates the GpioChangeCounter 
//
//  A GpioChangeCounter can be used to count pulses on a GPIO pin from things such as a flow meter, wind speed device or any other device that generates pulses.
// 
//  As the counter is low level it can handle much high frequency signals and avoids handling interupts in managed code. 
//  Also its a 64 bit counter so is not going to overflow. Counting 1Mhz pulses would overflow in 292470 years.
//
//  The counter can be set up to counter the raising, failing or both edges of a pulse.
//  This sample will uses a pwm signal generated on one pin to simulate a signal to count.


// For counter sample to work a wire needs to be connected between the counter input pin and Pwm output pin.

namespace ChangeCounter
{
    public class Program
    {
        // Set up the counter pin used
        const int COUNTER_INPUT_PIN = 5;

        // Set up the PWM outout pin used
        const int PWM_OUTPUT_PIN = 18;

        // Frequency used by PWM controller 
        const int PWM_FREQUENCY = 1234;

        public static void Main()
        {
            Debug.WriteLine("Change Counter test running");

            // Initialise PWM output pin
            PwmController pwmc = PwmController.GetDefault();
            pwmc.SetDesiredFrequency(PWM_FREQUENCY);

            PwmPin pwmTestPin =  pwmc.OpenPin(PWM_OUTPUT_PIN);
            pwmTestPin.SetActiveDutyCyclePercentage(0.5);

            Debug.WriteLine($"Open PWM pin {PWM_OUTPUT_PIN} frequency {pwmc.ActualFrequency}");
            Debug.WriteLine($"This pin must be connected to GpioChangeCounter pin {COUNTER_INPUT_PIN}");


            // Initialise count pin by opening GPIO as input
            GpioPin countPin = GpioController.GetDefault().OpenPin(COUNTER_INPUT_PIN);
            countPin.SetDriveMode(GpioPinDriveMode.InputPullUp);

            // Create a Counter passing in the GPIO pin
            GpioChangeCounter gpcc = new GpioChangeCounter(countPin);
            // Counter both raising and falling edges
            gpcc.Polarity = GpioChangePolarity.Both;

            Debug.WriteLine($"Counter pin {COUNTER_INPUT_PIN} created");

            // Start counter
            gpcc.Start();

            // Read count before we start PWM ( should be 0 )
            // We want to save the start relative time 
            GpioChangeCount count1 = gpcc.Read();

            // Start PWM signal
            pwmTestPin.Start();

            // Wait 1 Sec
            Thread.Sleep(1000);

             // Read current count 
            GpioChangeCount count2 = gpcc.Read();

            // Stop PWM signal & counter
            pwmTestPin.Stop();
            gpcc.Stop();


            // Change polarity of counter so only counting rising edges
            gpcc.Polarity = GpioChangePolarity.Rising;

            gpcc.Start();
            GpioChangeCount count3 = gpcc.Reset();

            pwmTestPin.Start();

            // Wait 1 Sec
            Thread.Sleep(1000);

            pwmTestPin.Stop();
            
            // Read count 
            GpioChangeCount count4 = gpcc.Read();

            gpcc.Stop();

            DisplayResults("Count pulses for 1 second with both edges", count1, count2);
            DisplayResults("Count pulses for 1 second with just rising edge", count3, count4);

            // Next test tries to measure the frequncy of the PWM signal
            pwmTestPin.Start();
            gpcc.Start();

            while (true)
            {
                // Reset Counter to zero
                GpioChangeCount countStart = gpcc.Reset();

                Thread.Sleep(1000);

                // Wait 1 sec and read again
                GpioChangeCount countEnd = gpcc.Read();

                // Sleep is not accurate so calculate actual time in secounds based of relative time differences of the 2 counts
                // Ticks are in 100 nano sec increments
                double periodSecs = (double)(countEnd.RelativeTime.Ticks - countStart.RelativeTime.Ticks)/10000000000.0;
                int frequecy = (int)((double)countEnd.Count / periodSecs);

                Debug.WriteLine($"Period {periodSecs:F6} Sec | Frequency {frequecy} Hz");
            }
        }


        static void DisplayResults(string text, GpioChangeCount start, GpioChangeCount end)
        {
            TimeSpan period = end.RelativeTime - start.RelativeTime;
            Debug.WriteLine($"Count {text} Time:{period} Count:{end.Count}");
        }
    }
}
