//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using nanoFramework.System.IO.FileSystem;

namespace FileAccess
{
    class Scenario0_mountDevice
    {
        public static void Execute()
        {
            Debug.WriteLine($"== Scenario0_mountDevice ==");

            //  With Esp32 we need to mount the SDCard, STM32 CHibios devices are mounted automatically

            SDCard sdCard = null;

            //  Currently implemented for ESP32 as the SDCard is not automatically mounted when the card is inserted.
            //  Also this allows the mount parameters to be passed depending on configuration of the running ESP32 module.
            //  Currently only 1 SDCard can be mounted which becomes drive "D:\", this may change later.
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to mount SDCard :-{ex.Message}");

                // No point continuing
                throw new Exception("Mount failed");
            }
        }
    }
}
