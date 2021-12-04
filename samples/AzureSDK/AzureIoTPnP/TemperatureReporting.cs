// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace AzureIoTPnP
{
    internal class TemperatureReporting
    {
        public double maxTemp { get; set; }
        public double minTemp { get; set; }
        public double avgTemp { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
