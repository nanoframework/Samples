// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;

namespace BasicExample.Ethernet
{
    public class Program
    {
        public static void Main()
        {
            // STEP 1: setup network
            // Different nanoFramework target have different networks. For example, ESP32 usually use WiFi,
            // while STM32F769 has Ethernet. Need to set it up and wait for a valid IP address.
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
                client.Publish("nf-mqtt/basic-demo", Encoding.UTF8.GetBytes("===== Hello MQTT! ====="), MqttQoSLevel.AtLeastOnce, false);
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
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            if (nis.Length > 0)
            {
                // Get the first interface (usually there's just one anyway).
                NetworkInterface ni = nis[0];

                // DHCP is nabled by default, but enabling it anyway.
                ni.EnableDhcp();

                // Wait for DHCP to complete.
                Debug.WriteLine("Waiting for IP address...");
                while (true)
                {
                    if (ni.IPv4Address != null && ni.IPv4Address.Length > 0)
                    {
                        if (ni.IPv4Address[0] != '0')
                        {
                            Debug.WriteLine($"We have an IP address: {ni.IPv4Address}");
                            break;
                        }
                        else
                        {
                            Debug.WriteLine("Still waiting for IP address...");
                        }
                    }

                    Thread.Sleep(500);
                }
            }
            else
            {
                throw new NotSupportedException("ERROR: there is no network interface configured.\r\nOpen the 'Edit Network Configuration' in Device Explorer and configure one.");
            }
        }
    }
}
