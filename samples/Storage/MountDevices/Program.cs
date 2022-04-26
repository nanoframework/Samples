//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using nanoFramework.System.IO.FileSystem;


namespace MountDevices
{
    public class Program
    {
        public static void Main()
        {
            SDCard sdCard = null;

            //  Windows.Storage.Devices.SDCard is a static class used to mount the SDcard.
            //  Currently implemented for ESP32 as the SDCard is not automatically mounted when the card is inserted.
            //  Also to enable the mount parameters to be passed depending on the running ESP32 module.
            //  Currently only 1 SDCard can be mounted, this may change later.
            try
            {
                // Mount a MMC sdcard using 4 bit data ( e.g Wrover boards )
                sdCard = new SDCard(new SDCard.SDCardMmcParameters() { dataWidth = SDCard.SDDataWidth._4_bit });

                // Mount a MMC sdcard using 1 bit data ( e.g Olimex EVB boards )
                // var sdCard = new SDCard(new SDCard.SDCardMmcParameters() { dataWidth = SDCard.SDDataWidth._1_bit });

                // Mount a SPI connected SDCard passing the SPI bus and the Chip select pin
                //    SDCard.MountSpi("SPI1", 26);
                //var sdCard = new SDCard(new SDCard.SDCardSpiParameters() { chipSelectPin = 26 });

                sdCard.Mount();
            }
            catch ( Exception ex)
            {
                Debug.WriteLine($"Failed to mount SDCard :-{ex.Message}");
            }

            // SDCard is mounted ?
            if (sdCard.IsMounted)
            {
                // Get the logical root folder for all removable storage devices
                // in nanoFramework the drive letters are fixed, being:
                // D: SD Card

                // get folders on 1st removable device
                var foldersInDevice = Directory.GetDirectories("D:");

                // List all directories
                foreach (var dir in foldersInDevice)
                {
                    Debug.WriteLine($"Folder ->{dir}");
                }


                // get files on the root of the 1st removable device
                var filesInDevice = Directory.GetFiles("D:");
                
                // List all files
                foreach (var file in filesInDevice)
                {
                    Debug.WriteLine($"file ->{file}");
                }

            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
