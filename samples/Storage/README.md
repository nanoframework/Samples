# Storage sample pack

Shows how to use the [Windows.Storage](http://docs.nanoframework.net/api/Windows.Storage.html) API to access storage, create and list folders and files, read and write from/to files using text or binary data.

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/master.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

This sample shows how a user can create a folder and files in removable storage and files in internal flash storage.
Internal flash storage doesn't support folders.

## Scenarios

### Access Removable Devices

Lists the removable storage devices present in the target and runs the following folder and file base scenarios on the 1st drive.

### Access Internal Devices

Lists the internal storage devices present in the target and runs the following file base scenarios on the 1st drive.

### Mount a device (ESP32 Only)

Shows how to mount a SDCard device and list the folders and files.

### Create a folder

This scenario demonstrates how to create a folder in a storage device, not replacing it if it already exists.

### Create a file

This scenario demonstrates how to create a file in a folder inside a storage device.

### Write and read text to/from a file

This scenario demonstrates how to write a string to a text file and then read it back.

### Write and read binary data to/from a file

This scenario demonstrates how to write a string converted to binary formate to a binary file and then read it back.

### Create multi level folders

Shows how to create folders in multi levels.

### Create files in a folder

This scenario goes further than the Create a file scenario to create multiple files in a low level folder.

### Rename a folder

This scenario demonstrates how to rename a folder.

### Delete FIles and Folders

This scenario demonstrates how delete files and folder with a recursive method to delete a whole folder tree.

### Rename files

This scenario demonstrates how to rename a file.

### Un-mount SDCard

This scenario demonstrates how to un-mount a mounted drive (ESP32)

### Subscribe to Removable Devices event handlers

This scenario demonstrates how to subscribe event handlers for Removable Devices events to be notified when a storage device is inserted and removed.

## Hardware requirements

Any hardware device running a nanoFramework image built with support for Windows.Storage device (SD card or USB mass storage device).

## Related topics

### Reference

- [Windows.Storage](http://docs.nanoframework.net/api/Windows.Storage.html)

## Build the sample

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build. 
2. Start Microsoft Visual Studio 2017 and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
4. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select Build > Deploy Solution.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging.
