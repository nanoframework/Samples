namespace InfraredRemote
{
    public class SignalData
    {
        public int AddressNumber { set; get; } = -1;
        public int CommandNumber { set; get; } = -1;

        public string RawAddress { set; get; }
        public string RawCommand { set; get; }
        public Protocol Protocol { get; set; } = Protocol.Unknown;
        public string Payload { get; set; }
    }
}
