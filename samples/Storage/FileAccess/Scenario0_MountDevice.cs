//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;
using Windows.Storage.Devices;

namespace FileAccess
{
    class Scenario0_mountDevice
    {
        public static void Execute()
        {
            Console.WriteLine($"== Scenario0_mountDevice ==");

            //  With Esp32 we need to mount the SDCard, STM32 CHibios devices are mounted automatically

            //  Windows.Storage.Devices.SDCard is a static class used to mount the SDcard.
            //  Currently implemented for ESP32 as the SDCard is not automatically mounted when the card is inserted.
            //  Also this allows the mount parameters to be passed depending on configuration of the running ESP32 module.
            //  Currently only 1 SDCard can be mounted which becomes drive "D:\", this may change later.
            try
            {
                // Uncomment the mount statement for the type of connected SDCard

                // Mount a MMC sdcard using 4 bit data ( e.g Wrover boards )
                //SDCard.MountMMC(false);

                // Mount a MMC sdcard using 1 bit data ( e.g Olimex EVB boards )
                //SDCard.MountMMC(true);

                // Mount a SPI connected SDCard passing the SPI bus and the Chip select pin
                // Default pins for "SPI1" : Mosi=23  Miso=25  Clk=19
                // Or use different pins by calling the Hardware.ESp32.Configuration.SetPinFunction()
                // Use Chip select on another pin like gpio 26 

                SDCard.MountSpi("SPI1", 26);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to mount SDCard :-{ex.Message}");
                // No point continuing
                throw new Exception("Mount failed");
            }
        }
    }
}
