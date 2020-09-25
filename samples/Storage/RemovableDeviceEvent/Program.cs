//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using Windows.Storage;

namespace RemovableDeviceEvent
{
    public class Program
    {
        public static void Main()
        {
            // add event handlers for Removable Device insertion and removal
            StorageEventManager.RemovableDeviceInserted += StorageEventManager_RemovableDeviceInserted;
            StorageEventManager.RemovableDeviceRemoved += StorageEventManager_RemovableDeviceRemoved;

            Thread.Sleep(Timeout.Infinite);
        }

        private static void StorageEventManager_RemovableDeviceRemoved(object sender, RemovableDeviceEventArgs e)
        {
            Console.WriteLine($"Removable Device @ \"{e.Path}\" removed.");
        }

        private static void StorageEventManager_RemovableDeviceInserted(object sender, RemovableDeviceEventArgs e)
        {
            Console.WriteLine($"Removable Device @ \"{e.Path}\" inserted.");
        }
    }
}
