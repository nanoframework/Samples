//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;


namespace FileAccess
{
    class Scenario9_RenameFile
    {
        public static void Execute(StorageFolder device)
        {
            Console.WriteLine($"== Scenario9_RenameFile ==");

            try
            {
                // Rename file created in Scenario2_CreateAFileInStorage from "file1.txt" to "newname.txt"

                // first make sure target name doesn't exist otherwise the code will fail( i.e. 2nd time run )
                // ignore any errors
                try
                {
                    StorageFile.GetFileFromPath(device.Path + "newname.txt").Delete();
                }
                catch (Exception) { };


                // To reference file you need a StorageFIle reference to file
                // If in root directory this can be done using GetFileFromPath, the file must exist otherwise you will get an exception.
                var file = StorageFile.GetFileFromPath(device.Path + "file1.txt");

                // For files in a folder you can add folder to path
                // var file = StorageFile.GetFileFromPath(device.Path + "folder\file1.txt");

                file.Rename("newname.txt");
                Console.WriteLine($"OK: Successfully renamed file to : {file.Path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Can't rename file :{ex.Message}");
            }

        }
    }
}
