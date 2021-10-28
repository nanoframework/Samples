// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Primitives.SimplePrimitives
{
    public static class Extensions
    {
        public static int Next(this Random rand, int min, int max)
        {
            if (max - min == 0)
                return min;
            return min + rand.Next(max-min);
        }
    }
}
