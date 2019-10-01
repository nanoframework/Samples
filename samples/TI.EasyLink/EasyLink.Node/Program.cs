using nanoFramework.TI.EasyLink;
using System;
using System.Threading;

namespace EasyLink.Node
{
    public class Program
    {
        static byte s_concentratorAddress = 0xAA;
        static byte s_nodeAddress = 0x33;

        public static void Main()
        {
            EasyLinkController controller = new EasyLinkController();

            controller.AddAddressToFilter(new byte[] { s_nodeAddress });

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

                    var txResult = controller.Transmit(packet, 5*1000);

                    if (txResult == Status.Success)
                    {
                        Console.WriteLine($"Tx packet: {packet.Payload[0]}");
                    }
                    else
                    {
                        Console.WriteLine($"Error when Tx'ing: {txResult}");
                    }

                    Thread.Sleep(5000);
                }
            }
            else
            {
                Console.WriteLine($"Failed to initialize SimpleLink. Error: {initResult}");
            }

            Thread.Sleep(Timeout.Infinite);

        }
    }
}
