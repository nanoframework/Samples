//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;

namespace FileAccess
{
    public class Scenario2_CreateAFileInRemovableStorage
    {
        public static void Execute()
        {
            // Get the logical root folder for all removable storage devices
            // in nanoFramework the drive letters are fixed, being:
            // D: SD Card
            // E: USB Mass Storage Device
            StorageFolder externalDevices = Windows.Storage.KnownFolders.RemovableDevices;

            // list all removable storage devices
            var removableDevices = externalDevices.GetFolders();

            if (removableDevices.Length > 0)
            {
                // create a file (replace if there is already one with this name)
                var fileNew = removableDevices[0].CreateFile("i-am-a-file.txt", CreationCollisionOption.ReplaceExisting);

                Console.WriteLine($"OK: Successfully created file: {fileNew.Path}");
            }
            else
            {
                // there is no removable device present
                Console.WriteLine($"ERROR: Can't create file. There is no removable device present.");
            }
        }
    }
}
