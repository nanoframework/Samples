// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.

using CommandLine;

namespace FindPeFiles
{
    internal class CommandOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('e', "enginedir", Required = true, HelpText = "Engine directory where the base PE files stands.")]
        public string EngineDirectory { get; set; } = string.Empty;

        [Option('a', "appdir", Required = true, HelpText = "Applicztion directory where the PE files to upload stands.")]
        public string AppDirectory { get; set; } = string.Empty;

        [Option('c', "container", Required = true, HelpText = "URL of the blobcontainer to upload the PE files.")]
        public string Container { get; set; } = string.Empty;

        [Option('t', "token", Required = true, HelpText = "SAS token for the blob container.")]
        public string Token { get; set; } = string.Empty;
    }
}
