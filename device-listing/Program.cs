// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Iot.Tools.DeviceListing;

string[] categoriesToDisplay = new string[]
{
    "beginner",
    "device",
    "networking",
    "mqtt",
    "azure",
    "aws",
    "rtc",
    "graphics",
    "can",
    "amqp",
    "esp32",
    "stm32",
    "ti",
    "system",
    "tools",
    "interop",
    "json",
    "file",
    "wifi",
    "iot-device",
    "ble"
};

Dictionary<string, string?> categoriesDescriptions = new()
{
    { "beginner", "Special beginner" },
    { "device", "Gpio, I2C, Spi, Pwm, Adc, Dac, 1-Wire, Serial" },

    { "networking", "Networking including HTTP, SSL" },
    { "mqtt", "MQTT" },
    { "amqp", "AMQP" },
    { "azure", "Azure specific" },
    { "aws", "AWS specific" },
    { "wifi", "Wifi" },

    { "rtc", "Real Time Clock" },
    { "can", "CAN" },
    { "graphics", "Graphics for screens" },
    { "iot-device", "IoT.Device" },

    { "esp32", "ESP32 specific" },
    { "stm32", "STM32 Specific" },
    { "ti", "Texas Instrument specific" },
    { "system", "System related" },
    { "tools", "Tools and utilities" },
    { "interop", "Interop" },
    { "json", "Json" },
    { "file", "File and storage access" },
    { "ble", "Bluetooth"}
};

HashSet<string> ignoredDeviceDirectories = new()
{
    "Archive",
};

string? repoRoot = FindRepoRoot(Environment.CurrentDirectory);

if (repoRoot is null)
{
    Console.WriteLine("Error: not in a git repository");
    return;
}

string amplesPath = Path.Combine(repoRoot, "samples");

List<SampleInfo> samples = new();

GetAllDirectoriesAndPopulate(Directory.EnumerateDirectories(amplesPath));

samples.Sort();

var allCategories = new HashSet<string>();

foreach (SampleInfo sample in samples)
{
    bool beingDisplayed = false;
    foreach (string category in sample.Categories)
    {
        if (allCategories.Add(category))
        {
            if (!categoriesDescriptions.ContainsKey(category))
            {
                Console.WriteLine($"Warning: Category `{category}` is missing description (`{sample.Title}`). [{sample.ReadmePath}]");
            }
        }

        beingDisplayed |= !beingDisplayed && categoriesToDisplay.Contains(category);
    }

    if (!beingDisplayed && sample.CategoriesFileExists)
    {
        // We do not want to show the warning when file doesn't exist as you will get separate warning that category.txt is missing in that case.
        Console.WriteLine($"Warning: Sample `{sample.Title}` is not being displayed under any category. [{sample.CategoriesFilePath}]");
    }
}

string alphabeticalDevicesIndex = Path.Combine(repoRoot, "README.md");
string categorizedDeviceListing = GetCategorizedDeviceListing(amplesPath, samples);
ReplacePlaceholder(alphabeticalDevicesIndex, "devices", categorizedDeviceListing);
alphabeticalDevicesIndex = Path.Combine(repoRoot, "README.zh-cn.md");
ReplacePlaceholder(alphabeticalDevicesIndex, "devices", categorizedDeviceListing);

string GetDeviceListing(string devicesPath, IEnumerable<SampleInfo> samples)
{
    var deviceListing = new StringBuilder();
    foreach (SampleInfo sample in samples)
    {
        deviceListing.AppendLine($"* [{sample.Title}]({CreateMarkdownLinkFromPath(sample.ReadmePath, repoRoot)})");
    }

    return deviceListing.ToString();
}

string GetCategorizedDeviceListing(string devicesPath, IEnumerable<SampleInfo> devices)
{
    var sampleListing = new StringBuilder();
    foreach (string categoryToDisplay in categoriesToDisplay)
    {
        if (categoriesDescriptions.TryGetValue(categoryToDisplay, out string? categoryDescription))
        {
            string listingInCurrentCategory = GetDeviceListing(devicesPath, devices.Where((d) => d.Categories.Contains(categoryToDisplay)));
            if (!string.IsNullOrEmpty(listingInCurrentCategory))
            {
                sampleListing.AppendLine($"## {categoryDescription}");
                sampleListing.AppendLine();
                sampleListing.AppendLine(listingInCurrentCategory);
            }
        }
        else
        {
            Console.WriteLine($"Warning: Category `{categoryToDisplay}` should be displayed but is missing description.");
        }
    }

    return sampleListing.ToString();
}

string? FindRepoRoot(string dir)
{
    if (dir is { Length: > 0 })
    {
        if (Directory.Exists(Path.Combine(dir, ".git")))
        {
            return dir;
        }
        else
        {
            DirectoryInfo? parentDir = new DirectoryInfo(dir).Parent;
            return parentDir?.FullName == null ? null : FindRepoRoot(parentDir.FullName);
        }
    }

    return null;
}

string CreateMarkdownLinkFromPath(string path, string parentPath)
{
    if (path.StartsWith(parentPath))
    {
        if (!path.Contains(parentPath))
        {
            throw new Exception($"No common path between `{path}` and `{parentPath}`");
        }

        var relativePath = path.Substring(parentPath.Length + 1).Replace("\\README.md", "");
        UriBuilder uriBuilder = new UriBuilder() { Path = relativePath };

        return uriBuilder.Path;
    }

    throw new Exception($"No common path between `{path}` and `{parentPath}`");
}

bool IsIgnoredDevice(string path)
{
    string dirName = new DirectoryInfo(path).Name;
    return ignoredDeviceDirectories.Contains(dirName);
}

void ReplacePlaceholder(string filePath, string placeholderName, string newContent)
{
    string fileContent = File.ReadAllText(filePath);

    string startTag = $"<{placeholderName}>";
    string endTag = $"</{placeholderName}>";

    int startIdx = fileContent.IndexOf(startTag);
    int endIdx = fileContent.IndexOf(endTag);

    if (startIdx == -1 || endIdx == -1)
    {
        throw new Exception($"`{startTag}` not found in `{filePath}`");
    }

    startIdx += startTag.Length;

    File.WriteAllText(
        filePath,
        fileContent.Substring(0, startIdx) +
        Environment.NewLine +
        // Extra empty line is needed so that github does not break bullet points
        Environment.NewLine +
        newContent +
        fileContent.Substring(endIdx));
}

void GetAllDirectoriesAndPopulate(IEnumerable<string> path)
{
    foreach (string directory in path)
    {
        if (IsIgnoredDevice(directory))
        {
            continue;
        }

        string readme = Path.Combine(directory, "README.md");
        string categories = Path.Combine(directory, "category.txt");

        if (File.Exists(readme))
        {
            var device = new SampleInfo(readme, categories);

            if (device.Title == null)
            {
                Console.WriteLine($"Warning: Device directory contains readme file without title on the first line. [{directory}]");
                continue;
            }

            samples.Add(device);
        }
        else
        {
            if (File.Exists(categories))
            {
                Console.WriteLine($"Warning: Device directory does not have a README.md file. [{directory}]");
            }
        }

        GetAllDirectoriesAndPopulate(Directory.EnumerateDirectories(directory));
    }
}
