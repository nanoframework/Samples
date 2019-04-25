//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;
using Windows.Storage.Devices;


namespace FileAccess
{
    class ScenarioA_UnmountDevice
    {
        public static void Execute()
        {
            Console.WriteLine($"== ScenarioA_UnmountDevice ==");

            //
            //  Unmount device if its been previously mounted.
            //  This is done to allow the card to be removed
            //
            //  Currently the mount card class only allows for 1 device to be mounted
            //
            if (SDCard.IsMounted)
            {
                SDCard.Unmount();
            }
        }
    }
}
