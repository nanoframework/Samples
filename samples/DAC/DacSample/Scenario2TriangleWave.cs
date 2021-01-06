//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.Dac;
using System.Threading;

namespace DacSample
{
    internal static class Scenario2TriangleWave
    {
        public static void Execute(ref DacChannel channel)
        {
            UInt32 value = 0;
            uint increment = 0;
            bool goingUp = false;
            int upperValue;

            // get upper value from DAC resolution
            upperValue = (int)Math.Pow(2, Scenario1ConfigureDac.dacResolution);

            // compute a reasonable increment value from the the DAC resolution
            increment = (uint)Math.Pow(2, (Scenario1ConfigureDac.dacResolution / 2));


            for (; ; )
            {
                if (value == upperValue)
                {
                    // tweak the value so it doesn't overflow the DAC
                    value--;

                    channel.WriteValue((ushort)value);

                    value++;

                    // invert to go down
                    goingUp = false;
                }
                else if (value == 0)
                {
                    channel.WriteValue((ushort)value);

                    // invert to go up
                    goingUp = true;
                }
                else
                {
                    channel.WriteValue((ushort)value);
                }

                if (goingUp)
                {
                    value += increment;
                }
                else
                {
                    value -= increment;
                }

                Thread.Sleep(Scenario1ConfigureDac.s_timeResolution);
            }
        }
    }
}
