//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.Logging;
using nanoFramework.Logging;
using System;

namespace Logging
{
    internal class MyTestComponent
    {
        private readonly ILogger _logger;

        public MyTestComponent()
        {
            _logger = this.GetCurrentClassLogger();
        }

        public void DoSomeLogging()
        {
            _logger.LogInformation("An informative message");
            _logger.LogError("An error situation");
            _logger.LogWarning(new Exception("Something is not supported"), "With exception context");
        }
    }
}