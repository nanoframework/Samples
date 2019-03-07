//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Threading;

namespace FileAccess
{
    public class Program
    {
        public static void Main()
        {
            // need to wait to allow time for the drives to be enumerated
            Thread.Sleep(3000);

            // create a folder
            Scenario1_CreateAFolderInRemovableStorage.Execute();

            // create a file
            Scenario2_CreateAFileInRemovableStorage.Execute();

            // write text and read to/from a file
            Scenario3_WriteAndReadTextInAFile.Execute();

            // write bytes and read to/from a file
            Scenario4_WriteAndReadBytesInAFile.Execute();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
