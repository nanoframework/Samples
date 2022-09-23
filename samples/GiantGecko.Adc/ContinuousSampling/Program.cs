////
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
////

using System;
using System.Diagnostics;
using System.Threading;
using nanoFramework.GiantGecko.Adc;

namespace ContinuousSampling
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("ADC sample: continuous channel scanning a list of ADC channels");
            Console.WriteLine();

            // this is instantiating the ADC controller with the default configuration
            // there is an overloaded constructor that accepts the configuration as a parameter 
            AdcController adc1 = new AdcController();

            // setup configuration to perform conversion
            var adcChannel0Config = new AdcChannelConfiguration()
            {
                ReferenceVoltage = ReferenceVoltage.InternalDifferencial_5V,
                SampleResolution = SampleResolution._12bits
            };

            // calling this will have the ADC start scanning continuously the specified channels (one and three) using the configuration requested
            adc1.StartContinuousSampling(new int[] {1, 3 }, adcChannel0Config);

            // allow some time for the ADC to start the scanning 
            Thread.Sleep(100);

            int[] lastSamples;

            // print the last sample from the channels being scanned (these are the raw values read from the ADC channels)
            for (int i = 0; i < 10; i++)
            {
                lastSamples = adc1.LastContinuousSamples;

                Console.WriteLine("Last sample");

                Console.WriteLine($"channel 1: {lastSamples[0]}");
                Console.WriteLine($"channel 3: {lastSamples[1]}");

                Console.WriteLine();
            }

            // stop continuous conversion
            adc1.StopContinuousSampling();

            // there is also the possibility to have the library take N samples and return the average value of those
            adc1.StartAveragedContinuousSampling(new int[] { 2, 4 }, 10);

            // print the averaged 10 last samples from the channels being scanned (these are the raw values read from the ADC channels)
            for (int i = 0; i < 10; i++)
            {
                lastSamples = adc1.LastContinuousSamples;

                Console.WriteLine("Average for the last 10 samples");

                Console.WriteLine($"channel 2: {lastSamples[0]}");
                Console.WriteLine($"channel 4: {lastSamples[1]}");

                Console.WriteLine();
            }

            // stop continuous conversion
            adc1.StopContinuousSampling();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
