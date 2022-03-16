//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace Reflection.CustomAttributes
{
    public class ComplexAttribute : Attribute
    {
        private readonly uint _max;
        private readonly string _s;
        private readonly bool _b;

        public uint Max  => _max;

        public string S => _s;
        public bool B => _b;

        public ComplexAttribute(uint m, string s, bool b)
        {
            _max = m;
            _s = s;
            _b = b;
        }
    }
}
