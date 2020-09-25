//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;

namespace FileAccess
{
    class Scenario5_CreateMultiLevelFolders
    {
        public static void Execute(StorageFolder device)
        {
            Console.WriteLine($"== Scenario5_CreateMultiLevelFolders ==");


            // Create multi level folders on device
            // D:\\Folder11\Folder21
            //             \Folder22\Folder31

            // Create folder on 1st Removalable Drive (D:\)
            StorageFolder Folder11 = CreateFolderHelper(device, "Folder11", CreationCollisionOption.ReplaceExisting);

            // Create 1st folder within Folder11
            StorageFolder Folder21 = CreateFolderHelper(Folder11, "Folder21", CreationCollisionOption.ReplaceExisting);
            // Create 2nd folder within Folder11 
            StorageFolder Folder22 = CreateFolderHelper(Folder11, "Folder22", CreationCollisionOption.ReplaceExisting);

            // Create folder within Folder22 
            StorageFolder Folder31 = CreateFolderHelper(Folder22, "Folder31", CreationCollisionOption.ReplaceExisting);

            Console.WriteLine($"OK: Successfully created all multi level folders");

        }

        /// <summary>
        /// Helper method to create a Folder at a specific location
        /// </summary>
        /// <param name="location">Location to create folder</param>
        /// <param name="folderName">Folder name to create</param>
        /// <returns></returns>
        static StorageFolder CreateFolderHelper(StorageFolder location, String folderName, CreationCollisionOption option)
        {
            StorageFolder folderNew = null;
            try
            {
                // create a folder (failing if there is already one with this name)
                folderNew = location.CreateFolder(folderName, option);

                Console.WriteLine($"OK: Successfully created folder: {folderNew.Path}");
            }
            catch (Exception )
            {
                Console.WriteLine($"ERROR: Can't create the folder as it already exists.");
            }

            return folderNew;
        }
    }
}
