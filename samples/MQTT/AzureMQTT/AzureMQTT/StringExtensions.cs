//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace System
{
    public static class StringExtensions
    {
        public static bool StartsWith(this string s, string value)
        {
            return s.IndexOf(value) == 0;
        }

        public static bool Contains(this string s, string value)
        {
            return s.IndexOf(value) > 0;
        }
    }
}
