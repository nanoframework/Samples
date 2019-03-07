//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;
using Windows.Storage.Streams;

namespace FileAccess
{
    public class Scenario4_WriteAndReadBytesInAFile
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
                var myFile = removableDevices[0].CreateFile("data-file-with-content.bin", CreationCollisionOption.ReplaceExisting);

                Console.WriteLine($"OK: Successfully created file: {myFile.Path}");

                string userContent = "this is a string to be saved as binary data";

                IBuffer writeBuffer = GetBufferFromString(userContent);
                FileIO.WriteBuffer(myFile, writeBuffer);

                Console.WriteLine($"The following { writeBuffer.Length } bytes of text were written to '{myFile.Name}':\r\n{ userContent }");

                IBuffer readBuffer = FileIO.ReadBuffer(myFile);
                using (DataReader dataReader = DataReader.FromBuffer(readBuffer))
                {
                    string fileContent = dataReader.ReadString(readBuffer.Length);

                    Console.WriteLine($"The following {readBuffer.Length} bytes of text were read from '{myFile.Name}':\r\n{ fileContent }");
                }
            }
            else
            {
                // there is no removable device present
                Console.WriteLine($"ERROR: Can't create file. There is no removable device present.");
            }
        }

        private static IBuffer GetBufferFromString(String str)
        {
            using (InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream())
            {
                using (DataWriter dataWriter = new DataWriter(memoryStream))
                {
                    dataWriter.WriteString(str);

                    return dataWriter.DetachBuffer();
                }
            }
        }
    }
}
