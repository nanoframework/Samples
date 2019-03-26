//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Networking;
using System;
using System.Net;
using System.Net.NetworkInformation;

namespace HttpSamples.HttpListenerSample
{
    public class Program
    {
        public static void Main()
        {
            var networkHerlpers = new NetworkHelpers();
            networkHerlpers.SetupAndConnectNetwork(true);

            Console.WriteLine("Waiting for network up and IP address...");
            NetworkHelpers.IpAddressAvailable.WaitOne();

            Console.WriteLine("Waiting for valid Date & Time...");
            NetworkHelpers.DateTimeAvailable.WaitOne();

            // setup HTTP response
            string responseString = "<HTML><BODY>Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            // Create a listener.
            HttpListener listener = new HttpListener("http");

            Console.WriteLine($"Listening for HTTP requests @ {NetworkInterface.GetAllNetworkInterfaces()[0].IPv4Address}:80 ...");
            listener.Start();

            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request
                HttpListenerContext context = listener.GetContext();

                // Get the response stream and write the response content to it
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);

                // output stream must be closed
                context.Response.Close();
                // context must be closed
                context.Close();
            }
        }
    }
}
