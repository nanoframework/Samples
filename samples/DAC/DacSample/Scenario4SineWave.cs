//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.Dac;
using System.Diagnostics;
using System.Threading;

namespace DacSample
{
    internal static class Scenario4SineWave
    {
        const float maxRads = (float)(2 * Math.PI);

        public static void Execute(ref DacChannel channel)
        {
            int upperValue, midRange;
            float radian = 0;

            // get upper value from DAC resolution
            upperValue = (int)Math.Pow(2, Scenario1ConfigureDac.dacResolution);

            // compute a reasonable increment value from the resolution
            float increment = maxRads / (Scenario1ConfigureDac.dacResolution * 10);
            midRange = upperValue / 2;

            for (; ; )
            {
                // because the DAC can't output negative values
                // we have to offset the sine wave to half the DAC output range
                uint value = (uint)((Math.Sin(radian) * (midRange - 1)) + midRange);

                //Output the current value to console when in debug.
                Debug.WriteLine($"DAC SineWave output current value: {value}");

                // output to DAC
                channel.WriteValue((ushort)value);

                // increment angle
                radian += increment;

                if (radian >= maxRads)
                {
                    // tweak the value so it doesn't overflow the DAC
                    radian = 0;
                }

                Thread.Sleep(Scenario1ConfigureDac.s_timeResolution);
            }
        }
    }
}
