using System;
using System.Diagnostics;
using System.Threading;
using nanoFramework.System.IO.FileSystem;
using nanoFramework.System.IO;

namespace MountExample
{
    /// <summary>
    /// Example code for Mounting SD card drive using System.IO.FileSystem
    /// </summary>
    public class Program
    {
        static SDCard mycard0;
        //static SDCard mycard1;
        //static SDCard mycard2;

        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework - SD card Mount example");

            // Configure pins for SDMMC1 and/or SDMMC2, this only needs to be done with devices that have configurable SDIO/MMC pins (ESP32_S3 and ESP32_P4).
            // Configure pins to match pins used by board.
            // A reference to nanoFramework.Hardware.Esp32 will also be required.
            //Configuration.SetPinFunction(16, DeviceFunction.SDMMC1_CLOCK);
            //Configuration.SetPinFunction(18, DeviceFunction.SDMMC1_COMMAND);
            //Configuration.SetPinFunction(19, DeviceFunction.SDMMC1_D0);
            //Configuration.SetPinFunction(20, DeviceFunction.SDMMC1_D1);
            //Configuration.SetPinFunction(21 DeviceFunction.SDMMC1_D2);
            //Configuration.SetPinFunction(22, DeviceFunction.SDMMC1_D3);

            // Initialise an instance of SDCard using constructor specific to your board/adapter

            // Boards with SD card interface and parameters built in to firmware 
            // mycard0 = new SDCard();

            // Boards with SDIO/MMC interface with 1 bit data lines ( e.g. ESP32 Olimex EVB , POE )
            //mycard0 = new SDCard(new SDCardMmcParameters { dataWidth = SDCard.SDDataWidth._1_bit });

            // Boards with SDIO/MMC and 4 data lines like Espressif Esp32 Wrover all models, these also support card detect
            //mycard0 = new SDCard(
            //    new SDCardMmcParameters { dataWidth = SDCard.SDDataWidth._4_bit },
            //    new CardDetectParameters { autoMount = true, enableCardDetectPin = true, cardDetectedState = false, cardDetectPin = 21 });

            // SPI constructors, check SPI bus pins and chip select pins are correct for your board 
            // Boards with SDCard on SPI bus, SPI 1 default , no card detect
            mycard0 = new SDCard(new SDCardSpiParameters { spiBus = 1, chipSelectPin = 22 });

            // SPI adapter with Card detect 
            //mycard0 = new SDCard(
            //    new SDCardSpiParameters { spiBus = 1, chipSelectPin = 22 },
            //    new CardDetectParameters { autoMount = true, enableCardDetectPin = true, cardDetectedState = false, cardDetectPin = 21 });

            // It also possible to mount a 2nd or 3rd SD card by specifying the SD card slotIndex which defaults to 0.
            // i.e a SDIO as first card and SPI as 2nd. Some boards support 2 SDIO devices. ESP32, ESP32_S3 & ESP32_P4
            //mycard1 = new SDCard(new SDCardSpiParameters { slotIndex = 1, spiBus = 1, chipSelectPin = 22 });
            //mycard2 = new SDCard(new SDCardSpiParameters { slotIndex = 2, spiBus = 1, chipSelectPin = 22 });

            Debug.WriteLine("SDcard inited");

            // Use storage events to detect when a drive had been mounted or unmounted from system
            StorageEventManager.RemovableDeviceInserted += StorageEventManager_RemovableDeviceInserted;
            StorageEventManager.RemovableDeviceRemoved += StorageEventManager_RemovableDeviceRemoved;


            // Mount option 1 - No card detect 
            // Try to manually mount card
            MountMyCard();

            // Mount Option 2,  use events to mount
            // Enable Card Detect events if you have Card detect on adapter 
            //mycard0.CardDetectChanged += Mycard_CardDetectChanged;


            // Unmount drive
            //UnMountIfMounted(mycard0);

            Thread.Sleep(Timeout.Infinite);
        }

        static void UnMountIfMounted(SDCard mycard)
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
                mycard0.Mount();
                Debug.WriteLine("Card Mounted");

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Card failed to mount : {ex.Message}");
                Debug.WriteLine($"IsMounted {mycard0.IsMounted}");
            }

            return false;
        }

        #region Events 

        // Storage events are fired when a drive is successfully mounted in the system and can be used.
        private static void StorageEventManager_RemovableDeviceRemoved(object sender, RemovableDriveEventArgs e)
        {
            Debug.WriteLine($"Card removed - Event:{e.Event} Path:{e.Drive}");
        }

        private static void StorageEventManager_RemovableDeviceInserted(object sender, RemovableDriveEventArgs e)
        {
            Debug.WriteLine($"Card inserted - Event:{e.Event} Path:{e.Drive}");
        }

        // The CardDetectChanged event can be used to automatically mount SD cards when a card is inserted.
        // This only works for SD card adapter that has a card detect pin tied to GPIO pin. 
        // If no Card Detect pin then this event is not required.
        // This event can be used to automatically mount device if AutoMount is not being used in CardDetectParameters 

        private static void Mycard_CardDetectChanged(object sender, CardDetectChangedEventArgs e)
        {
            if (e.CardState == CardDetectState.Inserted)
            {
                Console.WriteLine($"Card inserted");

                // When using manual mount (uncomment)
                //card.Mount();
            }
            else
            {
                Console.WriteLine($"Card removed");

                SDCard card = sender as SDCard;
                if (card.IsMounted)
                {
                    try
                    {
                        card.Unmount();
                    }
                    catch { }

                }
            }
        }

        #endregion
    }
}
