//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace Reflection.CustomAttributes
{
    public class MaxAttribute : Attribute
    {
        private readonly uint _max;

        public uint Max  => _max;

        public MaxAttribute(uint m)
        {
            _max = m;
        }
    }
}
