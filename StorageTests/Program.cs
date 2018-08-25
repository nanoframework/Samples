using System;
using System.Threading;
using Windows.Storage;

namespace StorageTests
{
    public class Program
    {
        public static void Main()
        {
            var filename = "sample.txt";

            // Get the logical root folder for all external storage devices.
            StorageFolder externalDevices = Windows.Storage.KnownFolders.RemovableDevices;

            // Get the first child folder, which represents the SD card.
            StorageFolder storageFolder = (await externalDevices.GetFoldersAsync()).FirstOrDefault();

            if (storageFolder != null)
            {
                //Find the ID of the SD card / USB storage device
                //var allProperties = storageFolder.Properties;
                //IEnumerable<string> propertiesToRetrieve = new List<string> { "ExternalStorageId" };

                //var storageIdProperties = await allProperties.RetrievePropertiesAsync(propertiesToRetrieve);

                //string cardId = (string)storageIdProperties["ExternalStorageId"];
                //Console.WriteLine(String.Format("The Cards ID is '{0}'.", cardId));


                //List all folders and files already on the device
                //GetFoldersAsync(CommonFolderQuery.DefaultQuery);

                //GetFilesAsync(CommonFileQuery.DefaultQuery);

                //Create a file
                StorageFile sampleFile;
                try
                {
                    sampleFile = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    Console.WriteLine(String.Format("The file '{0}' was created.", sampleFile.Name));
                }
                catch (Exception ex)
                {
                    // I/O errors are reported as exceptions.
                    Console.WriteLine(String.Format("Error creating file '{0}': {1}", filename, ex.Message));
                }

                //Get the file
                StorageFile file = await storageFolder.TryGetItemAsync(filename) as StorageFile;
                if (file != null)
                {
                    Console.WriteLine(String.Format("Operation result: {0}", file.Name));
                }
                else
                {
                    Console.WriteLine("Operation result: null");
                }


                //Copy the file
                StorageFile file = sampleFile;
                if (file != null)
                {
                    try
                    {
                        StorageFile fileCopy = await file.CopyAsync(storageFolder, "sample - Copy.txt", NameCollisionOption.ReplaceExisting);
                        Console.WriteLine(String.Format("The file '{0}' was copied and the new file was named '{1}'.", file.Name, fileCopy.Name));
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("File not found.");
                    }
                    catch (Exception ex)
                    {
                        // I/O errors are reported as exceptions.
                        Console.WriteLine(String.Format("Error copying file '{0}': {1}", file.Name, ex.Message));
                    }
                }
                else
                {
                    Console.WriteLine("File not found.");
                }


                //Compare the file
                if (file.IsEqual(sampleFile))
                {
                    Console.WriteLine("Files compared and are the same");
                }
                else
                {
                    Console.WriteLine("Files compared and are different.");
                }

                //Delete the file
                if (file != null)
                {
                    try
                    {
                        string filename = file.Name;
                        await file.DeleteAsync();
                        file = null;
                        Console.WriteLine(String.Format("The file '{0}' was deleted", filename));
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("File not found");

                    }
                    catch (Exception ex)
                    {
                        // I/O errors are reported as exceptions.
                        Console.WriteLine(String.Format("Error deleting file '{0}': {1}", file.Name, ex.Message));

                    }
                }
                else
                {

                    Console.WriteLine("File not found");

                }


                Console.WriteLine("Finished!");

                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
