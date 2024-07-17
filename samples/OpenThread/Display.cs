//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Networking.Thread;
using System;
using System.Text;

namespace Samples
{
    internal class Display
    {
        public static string LH
        {
            get { return DateTime.UtcNow.ToString("HH:mm:ss") + "-"; }
        }

        public static void Log(string str)
        {
            Console.WriteLine($"{LH} {str}");
        }

        public static void Log(string[] strings)
        {
            foreach (string line in strings)
            {
                Log(line);
            }
        }

        public static void Role(ThreadDeviceRole role)
        {
            switch (role)
            {
                case ThreadDeviceRole.Child: Log("Role = Child"); break;
                case ThreadDeviceRole.Router: Log("Role = Router"); break;
                case ThreadDeviceRole.Leader: Log("Role = Leader"); break;
                case ThreadDeviceRole.Detached: Log("Role = Detached"); break;
                case ThreadDeviceRole.Disabled: Log("Role = Disabled"); break;
                default:
                    Log($"Role is {role}");
                    break;
            }
        }

        public static void LogMemoryStats(string info)
        {
            uint manMem = nanoFramework.Runtime.Native.GC.Run(true);

            uint total;
            uint free;
            uint largest;

            nanoFramework.Hardware.Esp32.NativeMemory.GetMemoryInfo(nanoFramework.Hardware.Esp32.NativeMemory.MemoryType.All, out total, out free, out largest);
            Console.WriteLine($"{LH} Memory All ({info}) Managed:{manMem} Native total:{total}/Free:{free}/Largest:{largest}");
        }
    }
}
