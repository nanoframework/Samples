using nanoFramework.Networking;
using nanoFramework.Runtime.Native;
using System;
using System.Net;
using System.Threading;

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

            //Console.WriteLine("Waiting for valid Date & Time...");

            //NetworkHelpers.DateTimeAvailable.WaitOne();

            // Create a listener.
            HttpListener listener = new HttpListener("http");

            listener.Start();
            Console.WriteLine("Listening...");

            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            
            // Construct a response.
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            
            // You must close the output stream.
            output.Close();
            listener.Stop();


            Thread.Sleep(Timeout.Infinite);
        }
    }
}
