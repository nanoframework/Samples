//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;
using Windows.Storage;
using Windows.Storage.Devices;

namespace MountDevices
{
    public class Program
    {
        public static void Main()
        {
            

            //  Windows.Storage.Devices.SDCard is a static class used to mount the SDcard.
            //  Currently implemented for ESP32 as the SDCard is not automatically mounted when the card is inserted.
            //  Also to enable the mount parameters to be passed depending on the running ESP32 module.
            //  Currently only 1 SDCard can be mounted, this may change later.
            try
            {
                // Mount a MMC sdcard using 4 bit data ( e.g Wrover boards )
                SDCard.MountMMC(false);

                // Mount a MMC sdcard using 1 bit data ( e.g Olimex EVB boards )
            //    SDCard.MountMMC(true);

                // Mount a SPI connected SDCard passing the SPI bus and the Chip select pin
            //    SDCard.MountSpi("SPI1", 26);

            }
            catch ( Exception ex)
            {
                Debug.WriteLine($"Failed to mount SDCard :-{ex.Message}");
            }

            // SDCard is mounted ?
            if (SDCard.IsMounted)
            {
                // Get the logical root folder for all removable storage devices
                // in nanoFramework the drive letters are fixed, being:
                // D: SD Card
                StorageFolder externalDevices = Windows.Storage.KnownFolders.RemovableDevices;

                // list all removable storage devices
                var removableDevices = externalDevices.GetFolders();

                if (removableDevices.Length > 0)
                {
                    // get folders on 1st removable device
                    var foldersInDevice = removableDevices[0].GetFolders();
                    // List all folders
                    foreach(StorageFolder folder in foldersInDevice)
                    {
                        Debug.WriteLine($"Folder ->{folder.Path}");
                    }


                    // get files on the root of the 1st removable device
                    var filesInDevice = removableDevices[0].GetFiles();
                    // List all files
                    foreach (StorageFile file in filesInDevice)
                    {
                        Debug.WriteLine($"file ->{file.Path}");
                    }

                }
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
