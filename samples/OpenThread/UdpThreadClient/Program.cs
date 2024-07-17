//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using System.Net;
using nanoFramework.Networking.Thread;

namespace Samples
{
    public class Program
    {
        private const int UDP_PORT = 1234;

        private static OpenThread _ot;
        private static AutoResetEvent _waitNetAttached = new AutoResetEvent(false);

        public static Led _led = new Led();

        public static void Main()
        {
            Console.WriteLine();
            Display.Log("Sample UDP thread UDP client");

            try
            {
                // Target is mesh broadcast address
                // this will be received by all mesh devices except sleepy devices.
                // If you use a specific mesh local address here it will only received by 1 target
                string remoteAdress = "ff03::1";

                _led.Set(ThreadDeviceRole.Disabled);

                // Initialize OpenThread stack
                InitThread();

                Display.Log("Wait for OpenThread to be attached...");
                _waitNetAttached.WaitOne();

                IPAddress meshLocal = _ot.MeshLocalAddress;
                Display.Log($"Own mesh local IPV6 address {meshLocal}");

                Display.Log("Display current active dataset");
                CommandAndResult("dataset active");

                Display.Log("Display interface IP addresses");
                CommandAndResult("ipaddr");

                Display.Log("Open UDP socket for communication");
                NetUtils.OpenUdpSocket("", UDP_PORT, _ot.MeshLocalAddress);

                Display.Log("Start a receive thread for UDP message responses");
                Thread ReceiveUdpthread = new Thread(() => NetUtils.ReceiveUdpMessages());
                ReceiveUdpthread.Start();

                while (true)
                {
                    Display.Log($"Send (broadcast) messages on port:{UDP_PORT}");
                    NetUtils.SendMessageSocketTo(UDP_PORT, remoteAdress, $"Test message via socket @ {DateTime.UtcNow}");
                    _led.SetRxTX();

                    Thread.Sleep(5000);
                }
            }
            catch (Exception e)
            {
                Display.Log($"Exception : {e.Message}");
                Display.Log($"Stack : {e.StackTrace}");
            }

            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        /// Initialize the OpenThread
        /// </summary>
        static void InitThread()
        {
            OpenThreadDataset data = new OpenThreadDataset()
            {
                // Minimum data required to set up/connect to Thread network
                NetworkName = "nanoFramework",
                // 000102030405060708090A0B0C0D0E0F
                NetworkKey = new byte[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
                PanId = 0x1234,
                Channel = 15
            };

            Display.Log("---- Thread Dataset ------");
            Display.Log($"Network name {data.NetworkName}");
            Display.Log($"NetworkKey   {BitConverter.ToString(data.NetworkKey)}");
            Display.Log($"Channel      {data.Channel}");
            Display.Log("---- Thread Dataset end ------");

            // Use local radio, ESP32_C6 or ESP32_H2
            _ot = OpenThread.CreateThreadWithNativeRadio(ThreadDeviceType.Router);

            // Set up event handlers
            _ot.OnStatusChanged += Ot_OnStatusChanged;
            _ot.OnRoleChanged += Ot_OnRoleChanged;
            _ot.OnConsoleOutputAvailable += Ot_OnConsoleOutputAvailable;

            _ot.Dataset = data;

            Display.Log($"Starting OpenThread stack");
            _ot.Start();

            Display.Log($"Current Role");
            Display.Role(_ot.Role);
        }

        static void CommandAndResult(string cmd)
        {
            Console.WriteLine($"{Display.LH} command>{cmd}");
            string[] results = _ot.CommandLineInputAndWaitResponse(cmd);
            Display.Log(results);
        }

        #region OpenThread events handlers

        private static void Ot_OnConsoleOutputAvailable(OpenThread sender, OpenThreadConsoleOutputAvailableArgs args)
        {
            Display.Log(args.consoleLines);
        }

        private static void Ot_OnRoleChanged(OpenThread sender, OpenThreadRoleChangeEventArgs args)
        {
            Display.Role(args.currentRole);
            _led.Set(args.currentRole);
        }

        private static void Ot_OnStatusChanged(OpenThread sender, OpenThreadStateChangeEventArgs args)
        {
            switch ((ThreadDeviceState)args.currentState)
            {
                case ThreadDeviceState.Detached:
                    Display.Log("Status - Detached");
                    break;

                case ThreadDeviceState.Attached:
                    Display.Log("Status - Attached");
                    _waitNetAttached.Set();
                    break;

                case ThreadDeviceState.GotIpv6:
                    Display.Log("Status - Got IPV6 address");
                    break;

                case ThreadDeviceState.Start:
                    Display.Log("Status - Started");
                    break;

                case ThreadDeviceState.Stop:
                    Display.Log("Status - Stopped");
                    break;

                case ThreadDeviceState.InterfaceUp:
                    Display.Log("Status - Interface UP");
                    break;

                case ThreadDeviceState.InterfaceDown:
                    Display.Log("Status - Interface DOWN");
                    break;

                default:
                    Display.Log($"Status - changed to {args.currentState}");
                    break;
            }
        }

        #endregion
    }
}
