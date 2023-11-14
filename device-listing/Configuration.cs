// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using DdeviceListing;

namespace DeviceListing
{
    /// <summary>
    /// The configuration.
    /// </summary>
    internal class Configuration
    {
        /// <summary>
        /// Gets or sets the list of categories.
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the list of ignored folders.
        /// </summary>
        public string[] Ignored { get; set; }

        /// <summary>
        /// Gets or sets the list of languages to generate.
        /// </summary>
        public string[] Languages {get; set;}

        /// <summary>
        /// Gets or sets the samples path.
        /// </summary>
        public string SamplesPath { get; set;}

        /// <summary>
        /// Gets or sets whether to add or not README.md in the path to folder/link.
        /// </summary>
        public bool AddReadme { get; set; }
    }
}
