//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace InfraredRemote
{
    public class SignalData
    {
        public SignalData(int addressNumber, int commandNumber, string rawAddress, string rawCommand, Protocol protocol, string payload)
        {
            AddressNumber = addressNumber;
            CommandNumber = commandNumber;
            RawAddress = rawAddress;
            RawCommand = rawCommand;
            Protocol = protocol;
            Payload = payload;
        }

        public int AddressNumber { get; private set; } = -1;
        public int CommandNumber { get; private set; } = -1;

        public string RawAddress { get; private set; }
        public string RawCommand { get; private set; }
        public Protocol Protocol { get; private set; } = Protocol.Unknown;
        public string Payload { get; private set; }
    }
}
