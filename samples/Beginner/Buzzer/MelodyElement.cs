// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Buzzer.Samples
{
    /// <summary>
    /// A base class for melody sequence elements.
    /// </summary>
    public abstract class MelodyElement
    {
        /// <summary>
        /// Create a Melody Element.
        /// </summary>
        /// <param name="duration">A duration.</param>
        public MelodyElement(Duration duration) => Duration = duration;

        /// <summary>
        /// Duration which defines how long should element take on melody sequence timeline.
        /// </summary>
        public Duration Duration { get; set; }

    }
}
