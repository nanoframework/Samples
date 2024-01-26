// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.

namespace ServoMotorTester
{
    internal class ServoInformation
    {
        //servo info
        public uint MinPulse { get; set; } = 800;
        public uint MaxPulse { get; set; } = 2200;
        public uint Frequency { get; set; } = 50;
        public uint Position { get; set; } = 0;
    }
}
