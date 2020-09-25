//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace Reflection.CustomAttributes
{
    public class AuthorAttribute : Attribute
    {
        private readonly string _author;

        public string Author => _author;

        public AuthorAttribute(string author)
        {
            _author = author;
        }
    }
}
