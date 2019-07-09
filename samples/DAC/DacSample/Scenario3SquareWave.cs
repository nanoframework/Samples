//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Devices.Dac;
using System.Threading;

namespace DacSample
{
    internal static class Scenario3SquareWave
    {
        public static void Execute(ref DacChannel channel)
        {
            int value = 0;
            int upperValue;
            int periodCounter = 0;
            int halfPeriod;

            // get upper value from DAC resolution
            upperValue = (int)Math.Pow(2, Scenario1ConfigureDac.dacResolution);

            // figure out an expedite way to get a more or less square wave from the DAC and time resolution
            halfPeriod = ( upperValue / (Scenario1ConfigureDac.s_timeResolution * 10) ) / 2;

            for (;;)
            {
                if (periodCounter == halfPeriod)
                {
                    // tweak the value so it doesn't overflow the DAC
                    value = upperValue - 1;
                }
                else if (periodCounter == halfPeriod * 2)
                {
                    value = 0;

                    periodCounter = 0;
                }

                channel.WriteValue((ushort)value);

                Thread.Sleep(Scenario1ConfigureDac.s_timeResolution);

                periodCounter++;
            }
        }
    }
}
