//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting;

using Microsoft.Extensions.Logging;

namespace nanoFramework.Logging
{
    public class LoggingService : IHostedService
    {
        private ILogger Logger { get; set; }
        private ILogger ServiceLogger { get; set; }

        public LoggingService(ILogger logger, ILoggerFactory loggerFactory)
        {
            Logger = logger;
            ServiceLogger = loggerFactory.CreateLogger(nameof(LoggingService));
        }

        public void Start()
        {
            Logger.Log(LogLevel.Information, new EventId(10, "Start"), "Logging started", null);

            // show global log level
            Logger.LogTrace("Trace");
            Logger.LogDebug("Debug");
            Logger.LogInformation("Information");
            Logger.LogWarning("Warning");
            Logger.LogError("Error");
            Logger.LogCritical("Critical");

            // show current class log level
            ServiceLogger.LogTrace("Trace");
            ServiceLogger.LogDebug("Debug");
            ServiceLogger.LogInformation("Information");
            ServiceLogger.LogWarning("Warning");
            ServiceLogger.LogError("Error");
            ServiceLogger.LogCritical("Critical");
        }

        public void Stop()
        {
            Logger.Log(LogLevel.Information, new EventId(11, "Stop"), "Logging stopped", null);
        }
    }
}
