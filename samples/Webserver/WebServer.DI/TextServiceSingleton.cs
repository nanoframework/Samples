//
// Copyright (c) 2020 Laurent Ellerbach and the project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;

namespace nanoFramework.WebServer.Sample
{
    internal class TextServiceSingleton : ITextServiceSingleton
    {
        public Guid guid { get; }

        public TextServiceSingleton()
        {
            guid = Guid.NewGuid();
        }

        public string GetText()
        {
            return guid.ToString();
        }
    }
}
