using System;

namespace Tetris
{
    public class ExtendedWeakReference
    {
        public enum PriorityLevel : int
        {
            OkayToThrowAway = 1000,
            NiceToHave = 500000,
            Important = 750000,
            Critical = 1000000,
            System = 10000000,
        }
        public static uint c_SurvivePowerdown {get;set;}
        public uint Id { get; set; }
        public uint Flags { get; set; }
        public Type Selector { get; set; }
        public int Priority { get; set; }

        public object Target { get; set; }
        public static ExtendedWeakReference RecoverOrCreate(Type selector, uint id, uint flags)
        {
            ExtendedWeakReference wr = Recover(selector, id);
            return wr;
        }

        public static ExtendedWeakReference Recover(Type selector, uint id)
        {
            return new ExtendedWeakReference();
        }
    }
}