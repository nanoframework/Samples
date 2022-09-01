//
// Copyright (c) 2020 Laurent Ellerbach and the project contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.WebServer.Sample
{
    internal class TextService : ITextService, ITextServiceSingleton
    {
        public Guid guid { get; }

        public TextService()
        {
            guid = Guid.NewGuid();
        }

        public string GetText()
        {
            return guid.ToString();
        }
    }
}
