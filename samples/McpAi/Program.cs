using System;
using System.Diagnostics;
using System.Threading;
using McpAi;
using nanoFramework.Networking;
using nanoFramework.WebServer;
using nanoFramework.WebServer.Mcp;


// Connect to WiFi (device-specific code)
var connected = WifiNetworkHelper.ConnectDhcp(WiFi.Ssid, WiFi.Password, requiresDateTime: true, token: new CancellationTokenSource(60_000).Token);
if (!connected)
{
    Debug.WriteLine("Failed to connect to WiFi");
    return;
}

// Step 1: Discover and register MCP tools
McpToolRegistry.DiscoverTools(new Type[] { typeof(Light) });
Debug.WriteLine("MCP Tools discovered and registered.");

// Step 2: Start WebServer with MCP controller
// You can add more types if you also want to use it as a Web Server
// Note: HTTPS and certs are also supported, see the pervious sections
using (var server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(McpServerController) }))
{        // Optional: Customize MCP server information and instructions
         // This will override the default server name "nanoFramework" and version "1.0.0"
    McpServerController.ServerName = "MyIoTDevice";
    McpServerController.ServerVersion = "2.1.0";

    // Optional: Customize instructions sent to AI agents
    // This will override the default instruction about single request limitation
    McpServerController.Instructions = "This is my custom IoT device. Please send requests one at a time and wait for responses. Supports GPIO control and sensor readings.";

    server.Start();
    Thread.Sleep(Timeout.Infinite);
}

