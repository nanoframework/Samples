using System;
using System.Net.WebSockets;
using System.Net.WebSockets.WebSocketFrame;
using System.Text;
using System.Threading;
using nanoFramework.Networking;

namespace NFWebSockets.Client.Example
{
    public class Program
    {
        public static void Main()
        {
            //connect to wifi
            const string Ssid = "REPLACE-WITH-YOUR-SSID";
            const string Password = "REPLACE-WITH-YOUR-WIFI-KEY";

            // Give 60 seconds to the wifi join to happen
            CancellationTokenSource cs = new(60000);
            var success = WifiNetworkHelper.ScanAndConnectDhcp(Ssid, Password, token: cs.Token);
            if (!success)
            {
                //Red Light indicates no Wifi connection
                throw new Exception("Couldn't connect to the Wifi network");
            }

            //setup WebSocketClient
            ClientWebSocket websocketClient = new ClientWebSocket(new ClientWebSocketOptions()
            {
                //Change the heart beat to a 30 second interval
                KeepAliveInterval = TimeSpan.FromSeconds(30)
            });

            //Handler for receiving websocket messages. 
            websocketClient.MessageReceived += WebsocketClient_MessageReceived;
            //Setup custom header
            var headers = new ClientWebSocketHeaders();
            headers["userId"] = "nano";

            //Connect the client to the websocket server with custom headers
            websocketClient.Connect("ws://REPLACE-WITH-YOUR-WEBSOCKET-SERVER-ADDRESS", headers);

            //Send a message very 5 seconds
            while (websocketClient.State == WebSocketState.Open)
            {
                websocketClient.SendString("Hello nanoFramework Websocket!");
                Thread.Sleep(5000);
            }
        }

        private static void WebsocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var client = (ClientWebSocket)sender;

            //If message is of type Text, echo message back to client
            if (e.Frame.MessageType == WebSocketMessageType.Text)
            {
                //check if message is not fragmented
                if (!e.Frame.IsFragmented)
                {
                    string message = Encoding.UTF8.GetString(e.Frame.Buffer, 0, e.Frame.MessageLength);
                    client.SendString(message);
                }
                //close connection because fragmented messages are not allowed
                else
                {
                    client.Close(WebSocketCloseStatus.PolicyViolation, "Fragmented messages are not allowed"!);
                }
            }
        }
    }
}
