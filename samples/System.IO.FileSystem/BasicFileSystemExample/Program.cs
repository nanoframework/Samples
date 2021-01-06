using System;
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

            string sampeFilePath = "D:\\sampleFile.txt";
            string sampleFileUsbPath = "E:\\sampleFileUSB.txt";
            string sampleText = "Lorem Ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. ";

            #endregion


            #region Init

            // Init
            Thread.Sleep(3000);     // Wait until the Storage Devices are mounted (SD Card & USB). This usally takes some seconds after startup.

            // D: is SD Card
            // E:: is USB drive

            #endregion

            Debug.WriteLine("+++++ System.IO.FileSystem examples +++++");

            Debug.WriteLine("+++++ Creating a sample file +++++");
            File.Create(sampeFilePath);

            byte[] sampleBuffer = Encoding.UTF8.GetBytes(sampleText);

            FileStream fs = new FileStream(sampeFilePath, FileMode.Open, FileAccess.ReadWrite);
            fs.Write(sampleBuffer, 0, sampleBuffer.Length);

            if(fs.CanSeek)
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
            File.Copy(sampeFilePath, sampleFileUsbPath);


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
