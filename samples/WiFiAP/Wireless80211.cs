using System.Net.NetworkInformation;
using System.Threading;

namespace WiFiAP
{
    class Wireless80211
    {
        public static bool IsEnabled()
        {
            Wireless80211Configuration wconf = GetConfiguration();
            return ((wconf.Options & Wireless80211Configuration.ConfigurationOptions.Enable) == Wireless80211Configuration.ConfigurationOptions.Enable);
        }

        /// <summary>
        /// Disable the Wireless station interface.
        /// </summary>
        public static void Disable()
        {
            Wireless80211Configuration wconf = GetConfiguration();
            wconf.Options = Wireless80211Configuration.ConfigurationOptions.None;
            wconf.SaveConfiguration();
        }

        /// <summary>
        /// Configure and enable the Wireless station interface
        /// </summary>
        /// <param name="ssid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Configure(string ssid, string password)
        {
            Wireless80211Configuration wconf = GetConfiguration();


            // Set Options for Network Interface
            //
            // Enable      - Enable the Wireless Station ( Disable to reduce power )
            // AutoConnect - Automatically try to connect on boot.
            //
            wconf.Options = Wireless80211Configuration.ConfigurationOptions.AutoConnect |
                            Wireless80211Configuration.ConfigurationOptions.Enable;

            wconf.Ssid = ssid;
            wconf.Password = password;
            if (wconf.Password.Length==0)
                wconf.Authentication = System.Net.NetworkInformation.AuthenticationType.Open;
            else
                wconf.Authentication = System.Net.NetworkInformation.AuthenticationType.WPA2;

            // Save the configuration so on restart it will be running.
            wconf.SaveConfiguration();

            return false;
        }

        /// <summary>
        /// Get the Wireless station configuration.
        /// </summary>
        /// <returns>Wireless80211Configuration object</returns>
        public static Wireless80211Configuration GetConfiguration()
        {
             NetworkInterface ni = GetInterface();
            return Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];
        }

        public static NetworkInterface GetInterface()
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Find WirelessAP interface
            foreach (NetworkInterface ni in Interfaces)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    return ni;
                }
            }
            return null;
        }


        public static string WaitIP()
        {
            while (true)
            {
                NetworkInterface ni = GetInterface();
                if (ni.IPv4Address != null && ni.IPv4Address.Length > 0)
                {
                    if (ni.IPv4Address[0] != '0')
                    {
                        return ni.IPv4Address;
                    }
                }
                Thread.Sleep(500);
            }
        }
    }
}
