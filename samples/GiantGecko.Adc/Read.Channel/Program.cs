////
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
////

using System;
using System.Diagnostics;
using System.Threading;
using nanoFramework.GiantGecko.Adc;

namespace GiantGecko.AdcSamples.Read.Channel
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("ADC sample: Read from ADC channel");
            Console.WriteLine();

            // this is instantiating the ADC controller with the default configuration
            // there is an overloaded constructor that accepts the configuration as a parameter 
            AdcController adc1 = new AdcController();

            // setup configuration for channel 0

            var adcChannel0Config = new AdcChannelConfiguration()
            {
                ReferenceVoltage = ReferenceVoltage.InternalDifferencial_5V,
                SampleResolution = SampleResolution._12bits
            };

            // when opening an ADC channel a specific configuration can be passed as a parameter
            AdcChannel channel0 = adc1.OpenChannel(0, adcChannel0Config);

            // calling this will have the ADC perform a single conversion on the channel using the configuration requested
            var adcValue = channel0.ReadValue();

            Console.WriteLine($"Sample from channel 0: {adcValue}");

            // there is also the possibility to have the library take N samples and return the average value of those
            adcValue = channel0.ReadValueAveraged(10);

            Console.WriteLine($"Average for the last 10 samples from channel 0: {adcValue}");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
