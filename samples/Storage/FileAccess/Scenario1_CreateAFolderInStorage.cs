//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Storage;

namespace FileAccess
{
    public class Scenario1_CreateAFolderInStorage
    {
        public static void Execute(StorageFolder device)
        {
            Console.WriteLine($"== Scenario1_CreateAFolderInStorage ==");
            try
            {
                // create a folder (failing if there is already one with this name)
                var folderNew = device.CreateFolder("folder1", CreationCollisionOption.FailIfExists);

                Console.WriteLine($"OK: Successfully created folder: {folderNew.Path}");
            }
            catch(Exception)
            {
                Console.WriteLine($"ERROR: can't create the folder as it already exists.");
            }
        }
    }
}
