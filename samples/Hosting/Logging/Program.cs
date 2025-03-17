//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace nanoFramework.Logging
{
    public class Program
    {
        public static void Main()
        {
            IHost host = CreateHostBuilder().Build();
            
            host.StartAsync();
            host.StopAsync();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddLogging(LogLevel.Debug);
                    services.AddHostedService(typeof(LoggingService));
                });
    }
}
