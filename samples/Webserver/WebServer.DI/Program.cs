//
// Copyright (c) 2020 Laurent Ellerbach and the project contributors
// See LICENSE file in the project root for full license information.
//

using System.Threading;
using nanoFramework.Networking;
using nanoFramework.DependencyInjection;
using System;

namespace nanoFramework.WebServer.Sample
{
    public class Program
    {
        private static string MySsid = "myssid";
        private static string MyPassword = "mypassword";

        public static void Main()
        {
            Console.WriteLine("Hello from a webserver!");
            Console.WriteLine("Waiting for network up and IP address...");
            bool success;
            CancellationTokenSource cs = new(60000);
            success = WifiNetworkHelper.ConnectDhcp(
                MySsid, 
                MyPassword, 
                requiresDateTime: true, 
                token: cs.Token);

            if (!success)
            {
                Console.WriteLine($"Can't get a proper IP address and DateTime, error: {WifiNetworkHelper.Status}.");
                if (WifiNetworkHelper.HelperException != null)
                {
                    Console.WriteLine($"Exception: {WifiNetworkHelper.HelperException}");
                }
                return;
            }

            Console.WriteLine($"Connected to network {MySsid}");

            var serviceProvider = ConfigureServices();

            using (var webServer = new WebServerDi(80, HttpProtocol.Http, new Type[] { typeof(ControllerTest) }, serviceProvider))
            {
                webServer.Start();
                Thread.Sleep(Timeout.Infinite);
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddTransient(typeof(ITextService), typeof(TextService))
                .AddSingleton(typeof(ITextServiceSingleton), typeof(TextService))
                .BuildServiceProvider();
        }
    }
}
