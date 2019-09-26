using nanoFramework.TI.EasyLink;
using System;
using System.Threading;

namespace EasyLink.Concentrator
{
    public class Program
    {
        static byte s_concentratorAddress = 0xAA;

        public static void Main()
        {
            EasyLinkController controller = new EasyLinkController();

            controller.AddAddressToFilter(new byte[] { s_concentratorAddress });

            var initResult = controller.Initialize();

            if (initResult == Status.Success)
            {
                while (true)
                {
                    ReceivedPacket packet;

                    Console.WriteLine($"Waiting for packet...");

                    var rxResult = controller.Receive(out packet);

                    if (rxResult == Status.Success)
                    {
                        Console.WriteLine($"Rx packet: {packet.Payload[0]}, RSSI: { packet.Rssi }dB");
                    }
                    else
                    {
                        Console.WriteLine($"Error receiving packet: {rxResult}");
                    }

                }
            }
            else
            {
                Console.WriteLine($"Failed to initialize SimpleLink. Error: {initResult}");
            }
        }
    }
}
