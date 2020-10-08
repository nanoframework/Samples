//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TI.EasyLink;
using System.Diagnostics;
using System.Threading;

namespace EasyLink.Concentrator
{
    public class Program
    {
        static byte s_concentratorAddress = 0x00;

        public static void Main()
        {
            EasyLinkController controller = new EasyLinkController(PhyType._5kbpsSlLr);

            // need to initialize the EasyLink layer on the target before any operation is allowed
            var initResult = controller.Initialize();

            if (initResult == Status.Success)
            {
                controller.AddAddressToFilter(new byte[] { s_concentratorAddress });

                while (true)
                {
                    Debug.WriteLine($"Waiting for packet...");

                    var rxResult = controller.Receive(out ReceivedPacket packet);

                    if (rxResult == Status.Success)
                    {
                        Debug.WriteLine($"Rx packet: {packet.Payload[0]}, RSSI: { packet.Rssi }dB @ {packet.AbsoluteTime}");
                    }
                    else
                    {
                        Debug.WriteLine($"Error receiving packet: {rxResult}");
                    }
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
