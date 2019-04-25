//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;


namespace FileAccess
{
    class Scenario8_DeleteFIlesAndFolders
    {
        public static void Execute(StorageFolder device)
        {
            Console.WriteLine($"== Scenario8_DeleteFIlesAndFolders ==");

            // This sample demonstrates how to delete all folders and files at a location( whole folder tree )
            // The deletes the folders & files from Scenario5 & Scenario6

            try
            {
                StorageFolder folder11 = device.GetFolder("Folder11");

                // Delete contents of Folder11
                DeleteFolderTreeHelper(folder11);

                // Delete Folder11
                folder11.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: deleting folders & files : {ex.Message}");
            }
        }

        /// <summary>
        /// Delete all folders and files starting from starting folder
        /// </summary>
        /// <param name="startingFolder">Start folder to delete from.</param>
        /// <remarks>
        /// This method recusively works down the tree,  deleting files and folders on the tree leafs first. 
        /// If a drive is passed as start then all folders and files will be deleted.
        /// </remarks>
        static void DeleteFolderTreeHelper(StorageFolder startingFolder)
        {
            Console.WriteLine($"Enter DeleteFolderTreeHelper {startingFolder.Path}");

            StorageFolder[] folders = startingFolder.GetFolders();
            foreach (StorageFolder folder in folders)
            {
                DeleteFolderTreeHelper(folder);

                Console.WriteLine($"Delete folder {folder.Path}");
                try
                {
                    folder.Delete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception Deleting folder {folder.Path} : {ex.Message}");
                }
            }

            StorageFile[] files = startingFolder.GetFiles();
            foreach (StorageFile file in files)
            {
                Console.WriteLine($"Delete tree file {file.Path}");
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception Deleting file {file.Path} : {ex.Message}");
                }
            }
        }


    }
}



