//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;
using System.Threading;
using nanoFramework.Hardware.Esp32.Rmt;

namespace InfraredRemote
{
    //
    //	Sample program to show using the RMT device to read infrared signals with VS1838 and parse them into useful objects.
    // 
    public class Program
    {
        const int IR_RECEIVER_PIN = 7;

        public static void Main()
        {
            var signalDecoder = new NecSignalDecoder(signalLengthTolerance: 0.25);
            InfraredListener listener = new InfraredListener(IR_RECEIVER_PIN);
            listener.SignalEvent += (sender, signal) =>
            {
                DisplayCurrentReadCommand(signal);
                var data = signalDecoder.Decode(signal);
                if (data != null)
                {
                    Console.WriteLine($"Protocol: {data.Protocol} Address: {data.AddressNumber} Command: {data.CommandNumber} from Payload: {data.Payload}");
                }
            };
            listener.Start();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void DisplayCurrentReadCommand(RmtSymbols response)
        {
            Console.WriteLine($"Length:{response.Count}");

            StringBuilder sb = new StringBuilder();
            foreach (RmtSymbol rmtSymbol in response)
            {
                sb.Append("new RmtSymbol(");
                sb.Append(rmtSymbol.Duration0);
                sb.Append(",");
                sb.Append(rmtSymbol.Level0.ToString().ToLower());
                sb.Append(",");
                sb.Append(rmtSymbol.Duration1);
                sb.Append(",");
                sb.Append(rmtSymbol.Level1.ToString().ToLower());
                sb.Append("),");
                sb.Append("\r\n");
            }

            Console.WriteLine(sb.ToString());
        }
    }
}

