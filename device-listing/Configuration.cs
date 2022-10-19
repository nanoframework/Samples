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
    }
}
