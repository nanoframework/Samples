// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using DeviceListing;
using Iot.Tools.DeviceListing;

// get path from where it's executing to be able to run it on AZDO pipeline
var fullPath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);
string executingPath = Path.GetDirectoryName(fullPath);

Configuration configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(Path.Combine(executingPath, "Configuration.json")));

string? repoRoot = FindRepoRoot(Environment.CurrentDirectory);

if (repoRoot is null)
{
    Console.WriteLine("Error: not in a git repository");
    return;
}

string samplesPath = Path.Combine(repoRoot, configuration.SamplesPath);
List<SampleInfo> samples;

// We will iterate one per language
foreach (var language in configuration.Languages)
{
    samples = new();

    GetAllDirectoriesAndPopulate(Directory.EnumerateDirectories(samplesPath), language);

    samples.Sort();

    var allCategories = new HashSet<string>();

    foreach (SampleInfo sample in samples)
    {
        bool beingDisplayed = false;
        foreach (string category in sample.Categories)
        {
            if (allCategories.Add(category))
            {
                if (!configuration.Categories.Where(m => m.Name == category).Any())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Warning: Category `{category}` is missing description (`{sample.Title}`). [{sample.ReadmePath}]");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            beingDisplayed |= !beingDisplayed && configuration.Categories.Where(m => m.Name == category).Any();
        }

        if (!beingDisplayed && sample.CategoriesFileExists)
        {
            // We do not want to show the warning when file doesn't exist as you will get separate warning that category.txt is missing in that case.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Warning: Sample `{sample.Title}` is not being displayed under any category. [{sample.CategoriesFilePath}]");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    string alphabeticalDevicesIndex = Path.Combine(repoRoot, $"README{(string.IsNullOrEmpty(language) ? string.Empty : $".{language}")}.md");
    string categorizedDeviceListing = GetCategorizedDeviceListing(samplesPath, samples);
    ReplacePlaceholder(alphabeticalDevicesIndex, "devices", categorizedDeviceListing);
}

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
    foreach (string categoryToDisplay in configuration.Categories.Select(m => m.Name))
    {
        var categoryDescription = configuration.Categories.Where(m => m.Name == categoryToDisplay).FirstOrDefault()?.Description;
        if (!string.IsNullOrEmpty(categoryDescription))
        {
            string listingInCurrentCategory = GetDeviceListing(devicesPath, devices.Where(d => d.Categories.Contains(categoryToDisplay)));
            if (!string.IsNullOrEmpty(listingInCurrentCategory))
            {
                sampleListing.AppendLine($"### {categoryDescription}");
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

        var relativePath = path.Substring(parentPath.Length + 1);
        if (!configuration.AddReadme)
        {
            relativePath  = relativePath.Replace("\\README.md", "");
        }

        UriBuilder uriBuilder = new UriBuilder() { Path = relativePath };

        return uriBuilder.Path;
    }

    throw new Exception($"No common path between `{path}` and `{parentPath}`");
}

bool IsIgnoredDevice(string path)
{
    string dirName = new DirectoryInfo(path).Name;
    return configuration.Ignored.Contains(dirName);
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

void GetAllDirectoriesAndPopulate(IEnumerable<string> path, string language)
{
    foreach (string directory in path)
    {
        if (IsIgnoredDevice(directory))
        {
            continue;
        }

        string categories = Path.Combine(directory, "category.txt");

        // Find the README.xx.md files
        var files = Directory.EnumerateFiles(directory, $"README{(string.IsNullOrEmpty(language) ? string.Empty : $".{language}")}.md");
        // If we don't have any, then fall back to the neutral langauge
        if (!files.Any())
        {
            files = Directory.EnumerateFiles(directory, "README.md");
        }

        foreach (var readme in files)
        {
            // string readme = Path.Combine(directory, "README.md");
            var device = new SampleInfo(readme, categories);

            if (device.Title == null)
            {
                Console.WriteLine($"Warning: Device directory contains readme file without title on the first line. [{directory}]");
                continue;
            }

            samples.Add(device);

            GetAllDirectoriesAndPopulate(Directory.EnumerateDirectories(directory), language);
        }
    }
}
