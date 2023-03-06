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

        public int AddressNumber { private set; get; } = -1;
        public int CommandNumber { private set; get; } = -1;

        public string RawAddress { private set; get; }
        public string RawCommand { private set; get; }
        public Protocol Protocol { private get; set; } = Protocol.Unknown;
        public string Payload { private get; set; }
    }
}
