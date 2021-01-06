//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using Windows.Storage;


namespace FileAccess
{
    class Scenario9_RenameFile
    {
        public static void Execute(StorageFolder device)
        {
            Debug.WriteLine($"== Scenario9_RenameFile ==");

            try
            {
                // Rename file created in Scenario2_CreateAFileInStorage from "file1.txt" to "newname.txt"

                // first make sure target name doesn't exist otherwise the code will fail( i.e. 2nd time run )
                // get files from storage folder
                var files = device.GetFiles();

                foreach(var cursor in files)
                {
                    if(cursor.Name == "newname.txt")
                    {
                        cursor.Delete();

                        break;
                    }
                }

                // To reference file you need a StorageFIle reference to file
                // If in root directory this can be done using GetFileFromPath, the file must exist otherwise you will get an exception.
                var file = StorageFile.GetFileFromPath(device.Path + "file1.txt");
                
                // For files in a folder you can add folder to path
                // var file = StorageFile.GetFileFromPath(device.Path + "folder\file1.txt");

                file.Rename("newname.txt");
                Debug.WriteLine($"OK: Successfully renamed file to : {file.Path}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: Can't rename file :{ex.Message}");
            }

        }
    }
}
