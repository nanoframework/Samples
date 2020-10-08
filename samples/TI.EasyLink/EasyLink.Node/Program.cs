//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TI.EasyLink;
using System;
using System.Diagnostics;
using System.Threading;

namespace EasyLink.Node
{
    public class Program
    {
        static byte s_concentratorAddress = 0x00;
        static byte s_nodeAddress = 0x33;

        public static void Main()
        {
            EasyLinkController controller = new EasyLinkController(PhyType._5kbpsSlLr);

            // need to initialize the EasyLink layer on the target before any operation is allowed
            var initResult = controller.Initialize();

            if (initResult == Status.Success)
            {
                var destinationAddress = new byte[] { s_concentratorAddress };

                byte counter = 0;

                while (true)
                {
                    var packet = new TransmitPacket(
                                destinationAddress,
                                new byte[] { counter++ }
                            );

                    var txResult = controller.Transmit(packet);

                    if (txResult == Status.Success)
                    {
                        Debug.WriteLine($"Tx packet: {packet.Payload[0]}");
                    }
                    else
                    {
                        Debug.WriteLine($"Error when Tx'ing: {txResult}");
                    }

                    Thread.Sleep(3000);
                }
            }
            else
            {
                Debug.WriteLine($"Failed to initialize SimpleLink. Error: {initResult}");
            }

            Thread.Sleep(Timeout.Infinite);

        }
    }
}
