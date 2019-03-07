//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;

namespace FileAccess
{
    public class Scenario1_CreateAFolderInRemovableStorage
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
                try
                {
                    // create a folder (failing if there is already one with this name)
                    var folderNew = removableDevices[0].CreateFolder("nice-folder", CreationCollisionOption.FailIfExists);

                    Console.WriteLine($"OK: Successfully created folder: {folderNew.Path}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"ERROR: can't create the folder as it already exists.");
                }
            }
            else
            {
                // there is no removable device present
                Console.WriteLine($"ERROR: Can't create folder. There is no removable device present.");
            }
        }
    }
}
