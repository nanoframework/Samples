//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;

namespace FileAccess
{
    public class Scenario3_WriteAndReadTextInAFile
    {
        public static void Execute()
        {
            string textFromFile = null;

            // Get the logical root folder for all removable storage devices
            // in nanoFramework the drive letters are fixed, being:
            // D: SD Card
            // E: USB Mass Storage Device
            StorageFolder externalDevices = Windows.Storage.KnownFolders.RemovableDevices;

            // list all removable storage devices
            var removableDevices = externalDevices.GetFolders();

            if (removableDevices.Length > 0)
            {
                // create a file
                var myFile = removableDevices[0].CreateFile("text-file-with-something.txt", CreationCollisionOption.ReplaceExisting);

                Console.WriteLine($"OK: Successfully created file: {myFile.Path}");

                string textContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

                try
                {
                    // write text to the file
                    FileIO.WriteText(myFile, textContent);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"ERROR: write operation on file failed.");
                }

                try
                {
                    // read text from the file
                    textFromFile = FileIO.ReadText(myFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: read operation on file failed.");
                }

                // compare
                if(textContent.GetHashCode() == textFromFile.GetHashCode())
                {
                    Console.WriteLine($"OK: read text matches written text.");
                }
                else
                {
                    Console.WriteLine($"ERROR: read text does not match written text.");
                }
            }
            else
            {
                // there is no removable device present
                Console.WriteLine($"ERROR: Can't create file. There is no removable device present.");
            }
        }
    }
}
