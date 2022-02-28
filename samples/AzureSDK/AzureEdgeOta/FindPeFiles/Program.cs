// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.

using Azure;
using Azure.Storage.Blobs;
using AzureEdgeOTAEngine;
using CommandLine;
using FindPeFiles;
using Newtonsoft.Json;
using System.Data;
using System.Security.Cryptography;

Parser.Default.ParseArguments<CommandOptions>(args).WithParsed<CommandOptions>(o =>
{
    Console.WriteLine($"Input directory: {o.AppDirectory}");
    Console.WriteLine($"Input directory: {o.EngineDirectory}");
    var inputFiles = Directory.GetFiles(o.AppDirectory, "*.pe");
    var outputtFiles = Directory.GetFiles(o.EngineDirectory, "*.pe");
    List<FileSettings> fileSettings = new List<FileSettings>();
    List<string> diffFiles = new List<string>();

    foreach (var file in inputFiles)
    {
        var fileName = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        if (!outputtFiles.Where(m => m.Substring(m.LastIndexOf(Path.DirectorySeparatorChar) + 1) == fileName).Any())
        {
            diffFiles.Add(file);
        }
    }

    Console.WriteLine("Connecting to Blob storage");
    // Get a reference to a blob
    var uri = o.Container.Substring(0, o.Container.LastIndexOf('/'));
    var container = o.Container.Substring(o.Container.LastIndexOf('/') + 1);
    BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(uri), new AzureSasCredential(o.Token));
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(container);

    Console.WriteLine("Files to add to blob storage:");
    foreach (var file in diffFiles)
    {
        string fileName = file.Substring(file.LastIndexOf('\\') + 1);
        Console.WriteLine($"  {file}");
        Console.WriteLine($"    File name: {fileName}");
        var sha256 = SHA256.Create();
        var fs = File.OpenRead(file);
        byte[] buff = new byte[fs.Length];
        fs.Read(buff, 0, buff.Length);
        var sah256Computed = sha256.ComputeHash(buff);
        var signature = BitConverter.ToString(sah256Computed);
        Console.WriteLine($"    sha256: {signature }");
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        blobClient.DeleteIfExists();
        var res = blobClient.Upload(file);
        Console.WriteLine($"    File uploaded: {res.Value.ETag}");
        FileSettings fl = new FileSettings() { FileName = $"{o.Container}/{fileName}", Signature = signature };
        fileSettings.Add(fl);
    }

    Console.WriteLine();
    Console.WriteLine("Twin for the device:");
    Console.WriteLine(JsonConvert.SerializeObject(fileSettings));
});