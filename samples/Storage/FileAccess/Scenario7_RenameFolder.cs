﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using Windows.Storage;

namespace FileAccess
{
    class Scenario7_RenameFolder
    {
        public static void Execute(StorageFolder device)
        {
            Debug.WriteLine("== Scenario7_RenameFolder ==");

            // In Scenario5 we created a folder structure.
            // D:\\Folder11\Folder22\Folder31
            // And added files to Folder31
            //
            // This sample we will rename D:\\Folder11\Folder22 -> D:\\Folder11\Folder23

            try
            {
                // Build up the path for folders by using GetFolder, If the folder doesn't exist you will get an exception
                StorageFolder Folder22 = device.GetFolder("Folder11").GetFolder("Folder22");

                try
                {
                    // Rename folder from "Folder22" -> "Folder23"
                    Folder22.Rename("Folder23");
                    Debug.WriteLine($"OK: Successfully renamed folder to: {Folder22.Path}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ERROR: Renaming folder: {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: Getting folder : {ex.Message}");
            }

        }
    }
}
