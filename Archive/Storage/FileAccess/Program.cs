//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;
using Windows.Storage;
using nanoFramework.Runtime.Native;

namespace FileAccess
{
    public class Program
    {
        public static void Main()
        {
            bool mountRequired = DeviceNeedsMounting();

            if (mountRequired == false)
            {
                // need to wait to allow time for the drives to be enumerated
                Thread.Sleep(3000);
            }
            else
            {
                // mount device if on ESP32
                Scenario0_mountDevice.Execute();
            }


            Debug.WriteLine($"== GetRemovableDevices ==");

            // Get the logical root folder for all removable storage devices
            // in nanoFramework the drive letters are fixed, being:
            // D: SD Card
            // E: USB Mass Storage Device
            var externalDevices = Windows.Storage.KnownFolders.RemovableDevices;

            // list all removable storage devices
            var removableDevices = externalDevices.GetFolders();
            if (removableDevices.Length > 0)
            {
                // Use the first storage device
                StorageFolder device = removableDevices[0];

                // create a folder
                Scenario1_CreateAFolderInStorage.Execute(device);

                // create a file
                Scenario2_CreateAFileInStorage.Execute(device);

                // write text and read to/from a file
                Scenario3_WriteAndReadTextInAFile.Execute(device);

                // write bytes and read to/from a file
                // NOTE: this scenario is not supported anymore and won't be updated.
                // The plan is to deprecate Windows.Storage entirely and use System.IO.FileSystem going forward

                // Create multi level folders
                Scenario5_CreateMultiLevelFolders.Execute(device);

                // Create files in Folder
                Scenario6_CreateFilesInFolder.Execute(device);

                // Rename a folder
                Scenario7_RenameFolder.Execute(device);

                // Delete the created files and folders
                Scenario8_DeleteFIlesAndFolders.Execute(device);

                // Rename files
                Scenario9_RenameFile.Execute(device);


                if (mountRequired == true)
                {
                    // Unmount device if it was mounted
                    ScenarioA_UnmountDevice.Execute();
                }
            }
            else
            {
                // there is no removable device present
                Debug.WriteLine($"ERROR: Can't do anything. There is no removable device present.");
            }



            Debug.WriteLine($"== GetInternalDevices ==");

            // Get the logical root folder for all internal storage devices
            // in nanoFramework the drive letters are fixed, being:
            // I: Internal SPIFFS flash first (and default) partition
            // J: Internal SPIFFS flash second partition

            ////////////////////////////////////////////////
            // Note: SPIFFS devices do no support folders //
            ////////////////////////////////////////////////

            var InternalDevices = Windows.Storage.KnownFolders.InternalDevices;

            // Get a list all of Internal storage devices 
            var flashDevices = InternalDevices.GetFolders();
            foreach (var device in flashDevices)
            {
                // Note we are unable to create folders otherwise we will get an Unsupported error
                
                // create a file
                Scenario2_CreateAFileInStorage.Execute(device);

                // write text and read to/from a file
                Scenario3_WriteAndReadTextInAFile.Execute(device);

                // write bytes and read to/from a file
                // NOTE: this scenario is not supported anymore and won't be updated.
                // The plan is to deprecate Windows.Storage entirely and use System.IO.FileSystem going forward

                // Rename file
                Scenario9_RenameFile.Execute(device);

            }
            
            if(flashDevices.Length == 0)
            {
                // there is no removable device present
                Debug.WriteLine($"ERROR: Can't do anything. There is no internal devices present.");
            }



            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        /// Check if device needs mounting
        /// </summary>
        /// <returns>True if running on device that needss mounting</returns>
        /// <remarks>
        /// Currently only ESP32 devices need mounting
        /// </remarks>
        static bool DeviceNeedsMounting()
        {
            return (SystemInfo.Platform.IndexOf("ESP32") >= 0);
        }
    }
}
