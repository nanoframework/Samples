//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using Windows.Storage;


namespace FileAccess
{
    class Scenario6_CreateFilesInFolder
    {
        public static void Execute(StorageFolder device)
        {
            Debug.WriteLine($"== Scenario6_CreateFilesInFolder ==");

            try
            {
                // In Scenario5 we created a folder structure.
                // This sample uses the folder stucture and creates files in those folders
                // D:\\Folder11\Folder21\Folder31


                // Build up the path for folders by using GetFolder, If the folder doesn't exist you will get an exception
                StorageFolder Folder11 = device.GetFolder("Folder11");
                StorageFolder Folder21 = Folder11.GetFolder("Folder22");
                StorageFolder Folder31 = Folder21.GetFolder("Folder31");


                // create a file (replace if there is already one with this name)
                var fileNew1 = Folder31.CreateFile("sample1.txt", CreationCollisionOption.ReplaceExisting);
                Debug.WriteLine($"OK: Successfully created 1st file: {fileNew1.Path}");

                var fileNew2 = Folder31.CreateFile("sample2.txt", CreationCollisionOption.ReplaceExisting);
                Debug.WriteLine($"OK: Successfully created 2nd file: {fileNew2.Path}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception creating files in folders: {ex.Message}");
            }

        }
    }
}
