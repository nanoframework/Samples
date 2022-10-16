﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using Windows.Storage;

namespace FileAccess
{
    public class Scenario2_CreateAFileInStorage
    {
        public static void Execute(StorageFolder device)
        {
            Debug.WriteLine($"== Scenario2_CreateAFileInStorage ==");

            try
            {
                // create a file (replace if there is already one with this name)
                var fileNew = device.CreateFile("file1.txt", CreationCollisionOption.ReplaceExisting);

                Debug.WriteLine($"OK: Successfully created file: {fileNew.Path}");
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error: Unable to create file: {ex.Message}");
            }
        }
    }
}
