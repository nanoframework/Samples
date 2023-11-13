//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.DependencyInjection;
using nanoFramework.Hosting;

namespace Hosting
{
    public class Program
    {
        public static void Main()
        { 
            var host = CreateHostBuilder().Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(BackgroundQueue));
                    services.AddHostedService(typeof(SensorService));
                    services.AddHostedService(typeof(DisplayService));
                    services.AddHostedService(typeof(MonitorService));
                });
    }
}
