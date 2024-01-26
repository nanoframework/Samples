

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
