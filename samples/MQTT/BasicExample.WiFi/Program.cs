// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;
using System.Device.Wifi;

namespace BasicExample.Wifi
{
    public class Program
    {
        public static void Main()
        {
            // STEP 1: setup network
            // You need to set Wifi connection credentials in the configuration first!
            // Go to Device Explorer -> Edit network configuration -> Wifi proiles and set SSID and password there.
            SetupAndConnectNetwork();

            // STEP 2: connect to MQTT broker
            // Warning: test.mosquitto.org is very slow and congested, and is only suitable for very basic validation testing.
            // Change it to your local broker as soon as possible.
            var client = new MqttClient("test.mosquitto.org");
            var clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // STEP 3: subscribe to topics you want
            client.Subscribe(new[] { "nf-mqtt/basic-demo" }, new[] { MqttQoSLevel.AtLeastOnce });
            client.MqttMsgPublishReceived += HandleIncomingMessage;

            // STEP 4: publish something and watch it coming back
            for (int i = 0; i < 5; i++)
            {
                client.Publish("nf-mqtt/basic-demo", Encoding.UTF8.GetBytes("===== Hello MQTT! ====="), null, null, MqttQoSLevel.AtLeastOnce, false);
                Thread.Sleep(5000);
            }

            // STEP 5: disconnecting
            client.Disconnect();

            // App must not return.
            Thread.Sleep(Timeout.Infinite);
        }

        private static void HandleIncomingMessage(object sender, MqttMsgPublishEventArgs e)
        {
            Debug.WriteLine($"Message received: {Encoding.UTF8.GetString(e.Message, 0, e.Message.Length)}");
        }


        /// <summary>
        /// This is a helper function to pick up first available network interface and use it for communication.
        /// </summary>
        private static void SetupAndConnectNetwork()
        {
            // Get the first WiFI Adapter
            var wifiAdapter = WifiAdapter.FindAllAdapters()[0];

            // Begin network scan.
            wifiAdapter.ScanAsync();

            // While networks are being scan, continue on configuration. If networks were set previously, 
            // board may already be auto-connected, so reconnection is not even needed.
            var wiFiConfiguration = Wireless80211Configuration.GetAllWireless80211Configurations()[0];
            var ipAddress = NetworkInterface.GetAllNetworkInterfaces()[0].IPv4Address;
            var needToConnect = string.IsNullOrEmpty(ipAddress) || (ipAddress == "0.0.0.0");
            while (needToConnect)
            {
                foreach (var network in wifiAdapter.NetworkReport.AvailableNetworks)
                {
                    // Show all networks found
                    Debug.WriteLine($"Net SSID :{network.Ssid},  BSSID : {network.Bsid},  rssi : {network.NetworkRssiInDecibelMilliwatts},  signal : {network.SignalBars}");

                    // If its our Network then try to connect
                    if (network.Ssid == wiFiConfiguration.Ssid)
                    {

                        var result = wifiAdapter.Connect(network, WifiReconnectionKind.Automatic, wiFiConfiguration.Password);

                        if (result.ConnectionStatus == WifiConnectionStatus.Success)
                        {
                            Debug.WriteLine($"Connected to Wifi network {network.Ssid}.");
                            needToConnect = false;
                        }
                        else
                        {
                            Debug.WriteLine($"Error {result.ConnectionStatus} connecting to Wifi network {network.Ssid}.");
                        }
                    }
                }

                Thread.Sleep(10000);
            }

            ipAddress = NetworkInterface.GetAllNetworkInterfaces()[0].IPv4Address;
            Debug.WriteLine($"Connected to Wifi network with IP address {ipAddress}");
        }
    }
}
