//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace BasicFileSystemExample
{
    public class Program
    {

        public static void Main()
        {
            #region Variables

            string sampleFilePath = "D:\\sampleFile.txt";
            string sampleFileUsbPath = "E:\\sampleFileUSB.txt";
            string sampleFileInternalPath = "I:\\sampleFileInternal.txt";
            string sampleText = "Lorem Ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. ";

            #endregion


            #region Init

            // Init
            // Wait until the Storage Devices are mounted (SD Card & USB). 
            // This usally takes some seconds after startup.
            // Not required for Internal drive or drives mounted from managed code. See mount sample.
            Thread.Sleep(3000);

            // D: is SD Card
            // E: is USB drive
            // I: is Internal flash drive

            #endregion

            Debug.WriteLine("+++++ System.IO.FileSystem examples +++++");

            Debug.WriteLine("+++++ Creating a sample file +++++");
            var fs = File.Create(sampleFilePath);

            byte[] sampleBuffer = Encoding.UTF8.GetBytes(sampleText);

            fs.Write(sampleBuffer, 0, sampleBuffer.Length);

            if (fs.CanSeek)
            {
                Debug.WriteLine("+++++ Modify sample file +++++");
                // Seek to beginning of the file and write something there
                fs.Seek(0, SeekOrigin.Begin);
                string startText = "This is the start./r/n";
                fs.Write(Encoding.UTF8.GetBytes(startText), 0, startText.Length);

                // Go to the end of file and write something there
                fs.Seek(0, SeekOrigin.End);
                string endText = "/n/nThis is the End.";
                fs.Write(Encoding.UTF8.GetBytes(endText), 0, endText.Length);
            }

            fs.Dispose();
            Debug.WriteLine("+++++ Copy a file to USB drive +++++");
            File.Copy(sampleFilePath, sampleFileUsbPath);

            Debug.WriteLine("+++++ Move File Back to SD card +++++");
            File.Move(sampleFileUsbPath, "D:\\sampleFile2.txt");

            Debug.WriteLine("+++++ Change file attributes +++++");
            File.SetAttributes(sampleFileUsbPath, FileAttributes.ReadOnly);

            Debug.WriteLine("+++++ Read text from file +++++");
            FileStream fs2 = new FileStream(sampleFileUsbPath, FileMode.Open, FileAccess.Read);
            byte[] fileContent = new byte[fs2.Length];
            fs2.Read(fileContent, 0, (int)fs2.Length);

            Debug.WriteLine("+++++ System.IO.FileSystem examples end +++++");
        }
    }
}
