//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Threading;
using Windows.Storage;

namespace AccessRemovableDevices
{
    public class Program
    {
        public static void Main()
        {
            // need to wait to allow time for the drives to be enumerated
            Thread.Sleep(3000);


            // Get the logical root folder for all removable storage devices
            // in nanoFramework the drive letters are fixed, being:
            // D: SD Card
            // E: USB Mass Storage Device
            StorageFolder externalDevices = KnownFolders.RemovableDevices;

            // list all removable storage devices
            var removableDevices = externalDevices.GetFolders();

            if (removableDevices.Length > 0)
            {
                // get folders on 1st removable device
                var foldersInDevice = removableDevices[0].GetFolders();

                // get files on the root of the 1st removable device
                var filesInDevice = removableDevices[0].GetFiles();
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
