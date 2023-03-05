//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;
using nanoFramework.Hardware.Esp32.Rmt;

namespace InfraredRemote
{
    public abstract class Decoder
    {
        /// <summary>
        /// Defines constructor of <see cref="Decoder"/>.</typeparam>
        /// </summary>
        /// <param name="signalLengthTolerance">Tolerance of signal length represented as fraction value. </param>
        protected Decoder(double signalLengthTolerance)
        {
            SignalLengthTolerance = signalLengthTolerance;
        }
        protected SignalData lastData = null;
        protected abstract int PulseLengthInMicroseconds { get; }

        protected abstract int HeaderMark { get; }
        protected abstract int HeaderSpace { get; }
        protected abstract int RepeatSpace { get; }

        protected abstract int ZeroSpace { get; }

        protected abstract int OneSpace { get; }

        protected abstract Protocol Protocol { get; }
        protected double SignalLengthTolerance { get; private set; } = 0.2;

        protected abstract int SignalLength { get; }

        protected abstract bool UseLessSignificantBitFirst { get; }

        protected abstract int AddressBits { get; }
        protected abstract int CommandBits { get; }

        /// <summary>
        /// Decodes Rmt signal into SignalData.
        /// </summary>
        /// <param name="receivedSignal">Array representing decoded signal.</param>
        /// <returns>SignalData object.</returns>
        public virtual SignalData Decode(RmtCommand[] receivedSignal)
        {
            SignalData signalData = new SignalData();

            var firstPulse = receivedSignal[0];
            var lastPulse = receivedSignal[receivedSignal.Length - 1];
            bool isHeaderMark = Match(firstPulse.Duration0, HeaderMark);
            bool isSpaceMark = Match(firstPulse.Duration1, HeaderSpace);
            bool isEndOfTransmission = Match(lastPulse.Duration0, PulseLengthInMicroseconds) && !lastPulse.Level0 && lastPulse.Duration1 == 0 &&
                                       lastPulse.Level1;
            bool isRepeat = receivedSignal.Length == 2 && Match(firstPulse.Duration0, HeaderMark) && !firstPulse.Level0 && Match(firstPulse.Duration1, RepeatSpace) && firstPulse.Level1;

            var message = ExtractRawPayload(receivedSignal);

            if (isRepeat)
            {
                return lastData;
            }

            if (isHeaderMark && isSpaceMark && (receivedSignal.Length == SignalLength) && isEndOfTransmission)
            {
                signalData.Protocol = Protocol;
                var address = ExtractAddress(message);
                var command = ExtractCommand(message);
                signalData.RawAddress = address;
                signalData.RawCommand = command;
                signalData.Payload = message;
                if (UseLessSignificantBitFirst)
                {
                    signalData.AddressNumber = Convert.ToInt32(Reverse(address), 2);
                    signalData.CommandNumber = ToInt32(Reverse(command));
                    var temp = Convert.ToInt32(Reverse(command), 2);
                }
                else
                {
                    signalData.AddressNumber = ToInt32(address);
                    signalData.CommandNumber = ToInt32(command);
                }

                lastData = signalData;
            }

            return signalData;
        }

        protected virtual string ExtractCommand(string message)
        {
            return message.Substring(AddressBits, CommandBits);
        }

        protected virtual string ExtractAddress(string message)
        {
            return message.Substring(0, AddressBits);
        }

        private string ExtractRawPayload(RmtCommand[] response)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < response.Length - 1; i++)
            {
                var rmtCommand = response[i];
                if (Match(rmtCommand.Duration1, OneSpace))
                {
                    sb.Append("1");
                }
                else
                {
                    sb.Append("0");
                }
            }

            return sb.ToString();
        }

        protected bool Match(int tick, int expectedTick)
        {
            var ticksLow = expectedTick * (1 - SignalLengthTolerance);
            var ticksHigh = expectedTick * (1 + SignalLengthTolerance);
            return (tick >= ticksLow &&
                    tick <= ticksHigh);
        }

        private string Reverse(string valueToBeReversed)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = valueToBeReversed.Length - 1; i >= 0; i--)
            {
                sb.Append(valueToBeReversed[i]);
            }

            return sb.ToString();
        }

        private int ToInt32(string binaryValue)
        {
            int dec_value = 0;

            int base1 = 1;

            for (int i = binaryValue.Length - 1; i >= 0; i--)
            {
                if (binaryValue[i] == '1')
                    dec_value += base1;
                base1 = base1 * 2;
            }

            return dec_value;
        }
    }
}
