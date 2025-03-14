//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hosting
{
    public class Program
    {
        public static void Main()
        {
            IHostBuilder hostBuilder = CreateHostBuilder();
            IHost host = hostBuilder.Build();

            // starts application and blocks the main calling thread 
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(IHardwareService), typeof(HardwareService));
                    services.AddHostedService(typeof(LedService));
                });
    }
}
