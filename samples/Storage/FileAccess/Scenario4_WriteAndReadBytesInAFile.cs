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
        public static void Execute(StorageFolder device)
        {
            Console.WriteLine($"== Scenario4_WriteAndReadBytesInAFile ==");

            // create a file
            var myFile = device.CreateFile("file3.bin", CreationCollisionOption.ReplaceExisting);

            Console.WriteLine($"OK: Successfully created file: {myFile.Path}");

            string userContent = "this is a string to be saved as binary data";

            IBuffer writeBuffer = GetBufferFromString(userContent);
            uint bytesInBuffer = writeBuffer.Length;

            FileIO.WriteBuffer(myFile, writeBuffer);

            Console.WriteLine($"Of the {bytesInBuffer} in buffer, { writeBuffer.Length } remains to be written to '{myFile.Name}':\r\n{ userContent }");

            IBuffer readBuffer = FileIO.ReadBuffer(myFile);
            using (DataReader dataReader = DataReader.FromBuffer(readBuffer))
            {
                string fileContent = dataReader.ReadString(readBuffer.Length);

                Console.WriteLine($"The following {readBuffer.Length} bytes of text were read from '{myFile.Name}':\r\n{ fileContent }");
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
