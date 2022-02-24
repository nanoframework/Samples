using System;
using System.Threading;
using System.Diagnostics;
using nanoFramework.Networking;
using nanoFramework.WebServer;

#if HAS_WIFI
using System.Device.WiFi;
#endif

namespace nanoFramework.WebServer.GpioRest
{
    public class Program
    {

#if HAS_WIFI
        private static string MySsid = "ssid";
        private static string MyPassword = "password";
#endif

        private static bool _isConnected = false;

        public static void Main()
        {
            Debug.WriteLine("Hello from a webserver!");

            try
            {

                int connectRetry = 0;

                Debug.WriteLine("Waiting for network up and IP address...");
                bool success;
                CancellationTokenSource cs = new(60000);
#if HAS_WIFI
                success = WiFiNetworkHelper.ConnectDhcp(MySsid, MyPassword, requiresDateTime: true, token: cs.Token);
#else
                success = NetworkHelper.SetupAndConnectNetwork(cs.Token, true);
#endif
                if (!success)
                {
                    Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {WiFiNetworkHelper.Status}.");
                    if (WiFiNetworkHelper.HelperException != null)
                    {
                        Debug.WriteLine($"Exception: {WiFiNetworkHelper.HelperException}");
                    }
                    return;
                }

                // Instantiate a new web server on port 80.
                using (WebServer server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(ControllerGpio) }))
                {
                    // Start the server.
                    server.Start();

                    Thread.Sleep(Timeout.Infinite);
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine($"{ex}");
            }
        }
    }
}
