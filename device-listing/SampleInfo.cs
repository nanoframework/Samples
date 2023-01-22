// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Iot.Tools.DeviceListing
{
    class SampleInfo : IComparable<SampleInfo>
    {
        public string Title { get; private set; }
        public string Name { get; private set; }
        public string ReadmePath { get; private set; }
        public HashSet<string> Categories { get; private set; } = new HashSet<string>();
        public string CategoriesFilePath { get; private set; }
        public bool CategoriesFileExists { get; private set; }
        public string Language { get; private set; }

        public SampleInfo(string readmePath, string categoriesFilePath)
        {
            ReadmePath = readmePath;
            Name = new DirectoryInfo(readmePath).Parent?.Name;
            Title = GetTitle(readmePath) ?? "Error";
            CategoriesFilePath = categoriesFilePath;
            CategoriesFileExists = File.Exists(categoriesFilePath);
            Language = FindLanguage(readmePath);

            ImportCategories();
        }

        public int CompareTo(SampleInfo? other)
        {
            return Title.CompareTo(other?.Title);
        }

        public string FindLanguage(string filepath)
        {
            // Pattern is like path\name.zn-cn.ext or path\name.ext
            var separator = Path.DirectorySeparatorChar;
            var file = filepath.Substring(filepath.LastIndexOf(separator) + 1);
            var filewithoutext = file.Substring(0, file.LastIndexOf('.'));
            var posdot = filewithoutext.LastIndexOf('.');
            // We do have another dot, so by convention, it does contain our langage
            if (posdot > 0)
            {
                return filewithoutext.Substring(posdot + 1);
            }

            // Neutral language by convention
            return string.Empty;
        }

        private void ImportCategories()
        {
            if (!CategoriesFileExists)
            {
                Console.WriteLine($"Warning: Category file is missing. [{CategoriesFilePath}]");
                return;
            }

            foreach (string line in File.ReadAllLines(CategoriesFilePath))
            {
                if (line is not { Length: > 0 })
                {
                    continue;
                }

                if (!Categories.Add(line))
                {
                    Console.WriteLine($"Warning: Category `{line}` is duplicated in `{CategoriesFilePath}`");
                }
            }
        }

        private static string? GetTitle(string readmePath)
        {
            string[] lines = File.ReadAllLines(readmePath);
            int inc = 0;
            do
            {
                if (lines[inc].StartsWith("# "))
                {
                    return lines[inc].Substring(2);
                }

                inc++;
            } while (inc < lines.Length);

            return null;
        }
    }
}
