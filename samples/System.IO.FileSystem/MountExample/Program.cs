using System;
using System.Diagnostics;
using System.Threading;
using nanoFramework.System.IO.FileSystem;

namespace MountExample
{
    /// <summary>
    /// Example code for Mounting SD card drive using System.IO.FileSystem
    /// </summary>
    public class Program
    {
        static SDCard mycard; 

        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework - SD card Mount example");

            // Initialise an instance of SDCard using constructor specific to your board/adapter

            // Boards with SD card interface and parameters built in to firmware 
            // mycard = new SDCard();

            // Boards with SDIO/MMC interface with 1 bit data lines ( e.g. ESP32 Olimex EVB , POE )
            // mycard = new SDCard(new SDCard.SDCardMmcParameters { dataWidth = SDCard.SDDataWidth._1_bit });

            // Boards with SDIO/MMC and 4 data lines like Espressif Esp32 Wrover all models, these also support card detect
            //mycard = new SDCard(new SDCard.SDCardMmcParameters { dataWidth = SDCard.SDDataWidth._4_bit, enableCardDetectPin = true, cardDetectPin = 21 });

            // SPI constructors, check SPI bus pins and chip select pins are correct for your board 
            // Boards with SDCard on SPI bus, SPI 1 default , no card detect
            mycard = new SDCard(new SDCard.SDCardSpiParameters { spiBus = 1, chipSelectPin = 22 });

            // SPI adapter with Card detect 
            // mycard = new SDCard(new SDCard.SDCardSpiParameters { spiBus = 1, chipSelectPin = 22, enableCardDetectPin = true, cardDetectPin = 21 });

            Debug.WriteLine("SDcard inited");

            // Option 1 - No card detect 
            // Try to mount card
            MountMyCard();

            // Option 2 use events to mount
            // if Card detect available, enable events and mount when card inserted
            // Enable Storage events if you have Card detect on adapter 
            StorageEventManager.RemovableDeviceInserted += StorageEventManager_RemovableDeviceInserted;
            StorageEventManager.RemovableDeviceRemoved += StorageEventManager_RemovableDeviceRemoved;

            // Unmount drive
            UnMountIfMounted();

            Thread.Sleep(Timeout.Infinite);
        }

        static void UnMountIfMounted()
        {
            if (mycard.IsMounted)
            {
                mycard.Unmount();
            }
        }

        static bool MountMyCard()
        {
            try
            {
                mycard.Mount();
                Debug.WriteLine("Card Mounted");

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Card failed to mount : {ex.Message}");
                Debug.WriteLine($"IsMounted {mycard.IsMounted}");
            }

            return false;
        }

        #region Storage Events 

        // Storage events can be used to automatically mount SD cards when inserted
        // This only works for SD card adapter that include card detect pin tied to GPIO pin
        // If no Card Detect pin then events not required

        private static void StorageEventManager_RemovableDeviceRemoved(object sender, RemovableDeviceEventArgs e)
        {
            Debug.WriteLine($"Card removed - Event:{e.Event} Path:{e.Path}");
        }

        private static void StorageEventManager_RemovableDeviceInserted(object sender, RemovableDeviceEventArgs e)
        {
            Debug.WriteLine($"Card inserted - Event:{e.Event} Path:{e.Path}");

            // Card just inserted lets try to mount it
            MountMyCard();
        }

        #endregion

    }
}
