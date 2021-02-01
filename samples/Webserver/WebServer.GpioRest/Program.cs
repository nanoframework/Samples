using System;
using System.Threading;
using System.Diagnostics;
using nanoFramework.Networking;
using nanoFramework.WebServer;

#if HAS_WIFI
using Windows.Devices.WiFi;
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

#if HAS_WIFI
                // Get the first WiFI Adapter
                WiFiAdapter wifi = WiFiAdapter.FindAllAdapters()[0];
                Debug.WriteLine("Getting wifi adaptor");

                wifi.AvailableNetworksChanged += WifiAvailableNetworksChanged;

            rescan:
                wifi.ScanAsync();
                Debug.WriteLine("Scanning...");

                var timeout = DateTime.UtcNow.AddSeconds(10);
                while (!_isConnected)
                {
                    Thread.Sleep(100);
                    if (DateTime.UtcNow > timeout)
                    {
                        goto rescan;
                    }
                }
#endif

                NetworkHelpers.SetupAndConnectNetwork(false);

                Debug.WriteLine("Waiting for network up and IP address...");

#if HAS_WIFI
                while (!NetworkHelpers.CheckIP())
                {
                    Thread.Sleep(500);
                    connectRetry++;
                    if (connectRetry == 5)
                    {
                        connectRetry = 0;
                        goto rescan;
                    }
                }
#endif

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

#if HAS_WIFI
        private static void WifiAvailableNetworksChanged(WiFiAdapter sender, object e)
        {
            var wifiNetworks = sender.NetworkReport.AvailableNetworks;
            foreach (var net in wifiNetworks)
            {
                Debug.WriteLine($"SSID: {net.Ssid}, strength: {net.SignalBars}");
                if (net.Ssid == MySsid)
                {
                    if (_isConnected)
                    {
                        sender.Disconnect();
                        _isConnected = false;
                        Thread.Sleep(3000);
                    }
                    // Connect to network
                    WiFiConnectionResult result = sender.Connect(net, WiFiReconnectionKind.Automatic, MyPassword);

                    // Display status
                    if (result.ConnectionStatus == WiFiConnectionStatus.Success)
                    {
                        Debug.WriteLine($"Connected to Wifi network {net.Ssid}");
                        _isConnected = true;
                        break;
                    }
                    else
                    {
                        Debug.WriteLine($"Error {result.ConnectionStatus} connecting to Wifi network");
                    }
                }

            }
        }
#endif

    }
}
