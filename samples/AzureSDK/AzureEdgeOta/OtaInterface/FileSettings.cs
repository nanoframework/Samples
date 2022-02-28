// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.

namespace AzureEdgeOTAEngine
{
    /// <summary>
    /// File settings for a PE file
    /// </summary>
    internal class FileSettings
    {
        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signature
        /// </summary>
        public string Signature { get; set; } = string.Empty;
    }
}
