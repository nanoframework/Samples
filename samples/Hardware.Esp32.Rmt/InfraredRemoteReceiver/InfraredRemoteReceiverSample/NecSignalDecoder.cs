namespace InfraredRemote
{
    /// <summary>
    /// Nec protocol signal decoder.
    /// </summary>
    public class NecSignalDecoder : Decoder
    {
        /// <summary>
        /// Creates instance of <see cref="NecSignalDecoder"/>.</typeparam>
        /// </summary>
        /// <param name="signalLengthTolerance">Tolerance of signal length represented as percentage value.</param>
        public NecSignalDecoder(double signalLengthTolerance)
        : base(signalLengthTolerance)
        {
        }

        protected override int PulseLengthInMicroseconds => 560;
        protected override int HeaderMark => PulseLengthInMicroseconds * 16;
        protected override int HeaderSpace => PulseLengthInMicroseconds * 8;
        protected override int RepeatSpace => PulseLengthInMicroseconds * 4;
        protected override int ZeroSpace => PulseLengthInMicroseconds;
        protected override int OneSpace => PulseLengthInMicroseconds * 3;
        protected override Protocol Protocol => Protocol.Nec;
        protected override int SignalLength => 34;
        protected override bool UseLessSignificantBitFirst => true;
        protected override int AddressBits => 16;
        protected override int CommandBits => 16;

        protected override string ExtractCommand(string message)
        {
            return message.Substring(16, 8);
        }

        protected override string ExtractAddress(string message)
        {
            return message.Substring(0, 8);
        }
    }
}
