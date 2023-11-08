//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.Logging.Debug;
using Microsoft.Extensions.Logging;

namespace nanoFramework.Logging
{
    /// <summary>
    /// Extension methods for setting up logging services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection"/> 
        /// that is enabled for <see cref="LogLevel"/> Information or higher.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddLogging(this IServiceCollection services)
        {
            return AddLogging(services, LogLevel.Information);
        }

        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="level">The <see cref="LogLevel"/> to set as the minimum.</param></param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddLogging(this IServiceCollection services, LogLevel level)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            var loggerFactory = new DebugLoggerFactory();
            LogDispatcher.LoggerFactory = loggerFactory;
            LoggerExtensions.MessageFormatter = typeof(LoggerFormat).GetType().GetMethod("MessageFormatter");

            var logger = (DebugLogger)loggerFactory.GetCurrentClassLogger();
            logger.MinLogLevel = level;

            // using TryAdd prevents duplicate logging objects if AddLogging() is added more then once
            services.TryAdd(new ServiceDescriptor(typeof(ILogger), logger));
            services.TryAdd(new ServiceDescriptor(typeof(ILoggerFactory), loggerFactory));

            return services;
        }
    }

    public class LoggerFormat
    {
        public string MessageFormatter(string className, LogLevel logLevel, EventId eventId, string state, Exception exception)
        {
            string logstr = string.Empty;
            switch (logLevel)
            {
                case LogLevel.Trace:
                    logstr = "TRACE: ";
                    break;
                case LogLevel.Debug:
                    logstr = "DEBUG: ";
                    break;
                case LogLevel.Warning:
                    logstr = "WARNING: ";
                    break;
                case LogLevel.Error:
                    logstr = "ERROR: ";
                    break;
                case LogLevel.Critical:
                    logstr = "CRITICAL:";
                    break;
                case LogLevel.Information:
                    logstr = "INFO: ";
                    break;
                case LogLevel.None:
                default:
                    break;
            }

            string eventstr = eventId.Id != 0 ? $"EVENT ID: {eventId}, " : string.Empty;
            string msg = $"[{className}] {eventstr}{logstr} {state}";
            if (exception != null)
            {
                msg += $" {exception}";
            }

            return msg;
        }
    }
}
