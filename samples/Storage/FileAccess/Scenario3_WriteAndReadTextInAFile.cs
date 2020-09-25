//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;

namespace FileAccess
{
    public class Scenario3_WriteAndReadTextInAFile
    {
        public static void Execute(StorageFolder device)
        {
            Console.WriteLine($"== Scenario3_WriteAndReadTextInAFile ==");

            string textFromFile = null;

            // create a file
            var myFile = device.CreateFile("file2.txt", CreationCollisionOption.ReplaceExisting);

            Console.WriteLine($"OK: Successfully created file: {myFile.Path}");

            string textContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            try
            {
                // write text to the file
                FileIO.WriteText(myFile, textContent);
            }
            catch(Exception )
            {
                Console.WriteLine($"ERROR: write operation on file failed.");
            }

            try
            {
                // read text from the file
                textFromFile = FileIO.ReadText(myFile);
            }
            catch (Exception )
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
    }
}
