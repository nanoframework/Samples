//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using Windows.Storage;

namespace FileAccess
{
    public class Scenario1_CreateAFolderInStorage
    {
        public static void Execute(StorageFolder device)
        {
            Debug.WriteLine($"== Scenario1_CreateAFolderInStorage ==");
            try
            {
                // create a folder (failing if there is already one with this name)
                var folderNew = device.CreateFolder("folder1", CreationCollisionOption.FailIfExists);

                Debug.WriteLine($"OK: Successfully created folder: {folderNew.Path}");
            }
            catch(Exception)
            {
                Debug.WriteLine($"ERROR: can't create the folder as it already exists.");
            }
        }
    }
}
