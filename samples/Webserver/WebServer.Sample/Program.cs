//
// Copyright (c) 2020 Laurent Ellerbach and the project contributors
// See LICENSE file in the project root for full license information.
//

//////////////////////////////////////////
// uncomment the line bellow to use TLS //
#define USE_TLS
//////////////////////////////////////////

using System;
using System.Threading;
using System.Diagnostics;
using nanoFramework.Networking;
using System.Device.Gpio;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using WebServer.Sample;

#if HAS_WIFI
using System.Device.WiFi;
#endif

#if HAS_STORAGE
using Windows.Storage;
#endif

namespace nanoFramework.WebServer.Sample
{
    public class Program
    {

#if HAS_WIFI
        private static string MySsid = "ssid";
        private static string MyPassword = "password";
        private static bool _isConnected = false;
#endif

#if HAS_STORAGE
        private static StorageFolder _storage;
#endif

        private static GpioController _controller;
        private static string _securityKey = "MySecurityKey42";

        public static void Main()
        {
            Debug.WriteLine("Hello from a webserver!");

            try
            {
                Debug.WriteLine("Waiting for network up and IP address...");
                bool success;
                CancellationTokenSource cs = new(60000);
#if HAS_WIFI
                success = WiFiNetworkHelper.ConnectDhcp(MySsid, MyPassword, requiresDateTime: true, token: cs.Token);
#else
                success = NetworkHelper.SetupAndConnectNetwork(cs.Token, true);
#endif
                if(!success)
                {
                    Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {WiFiNetworkHelper.Status}.");
                    if(WiFiNetworkHelper.HelperException !=null)
                    {
                        Debug.WriteLine($"Exception: {WiFiNetworkHelper.HelperException}");
                    }
                    return;
                }

#if HAS_STORAGE
                _storage = KnownFolders.RemovableDevices.GetFolders()[0];
#endif

                _controller = new GpioController();

#if USE_TLS
                X509Certificate _myWebServerCertificate509 = new X509Certificate2(_myWebServerCrt, _myWebServerPrivateKey, "1234");

                // Instantiate a new web server on port 443.
                using (WebServer server = new WebServer(443, HttpProtocol.Https, new Type[] { typeof(ControllerPerson), typeof(ControllerTest), typeof(ControllerAuth) }))
#else
                // Instantiate a new web server on port 80.
                using (WebServer server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(ControllerPerson), typeof(ControllerTest), typeof(ControllerAuth) }))
#endif                
                {
                    // To test authentication with various scenarios
                    server.ApiKey = _securityKey;
                    server.Credential = new NetworkCredential("user", "password");
                    // Add a handler for commands that are received by the server.
                    server.CommandReceived += ServerCommandReceived;

#if USE_TLS
                    server.HttpsCert = _myWebServerCertificate509;

                    server.SslProtocols = System.Net.Security.SslProtocols.Tls11 | System.Net.Security.SslProtocols.Tls12;
#endif

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

        private static void ServerCommandReceived(object source, WebServerEventArgs e)
        {
            try
            {
                var url = e.Context.Request.RawUrl;
                Debug.WriteLine($"Command received: {url}, Method: {e.Context.Request.HttpMethod}");

                if (url.ToLower() == "/sayhello")
                {
                    // This is simple raw text returned
                    WebServer.OutPutStream(e.Context.Response, "It's working, url is empty, this is just raw text, /sayhello is just returning a raw text");
                }
                else if (url.Length <= 1)
                {
                    // Here you can return a real html page for example

                    WebServer.OutPutStream(e.Context.Response, "<html><head>" +
                        "<title>Hi from nanoFramework Server</title></head><body>You want me to say hello in a real HTML page!<br/><a href='/useinternal'>Generate an internal text.txt file</a><br />" +
                        "<a href='/Text.txt'>Download the Text.txt file</a><br>" +
                        "Try this url with parameters: <a href='/param.htm?param1=42&second=24&NAme=Ellerbach'>/param.htm?param1=42&second=24&NAme=Ellerbach</a></body></html>");
                }
#if HAS_STORAGE
                else if (url.ToLower() == "/useinternal")
                {
                    // This tells the web server to use the internal storage and create a simple text file
                    _storage = KnownFolders.InternalDevices.GetFolders()[0];
                    var testFile = _storage.CreateFile("text.txt", CreationCollisionOption.ReplaceExisting);
                    FileIO.WriteText(testFile, "This is an example of file\r\nAnd this is the second line");
                    WebServer.OutPutStream(e.Context.Response, "Created a test file text.txt on internal storage");
                }
#endif
                else if (url.ToLower().IndexOf("/param.htm") == 0)
                {
                    ParamHtml(e);
                }
                else if (url.ToLower().IndexOf("/api/") == 0)
                {
                    // Check the routes and dispatch
                    var routes = url.TrimStart('/').Split('/');
                    if (routes.Length > 3)
                    {
                        // Check the security key
                        if (!CheckAPiKey(e.Context.Request.Headers))
                        {
                            WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.Forbidden);
                            return;
                        }

                        var pinNumber = Convert.ToInt16(routes[2]);

                        // Do we have gpio ?
                        if (routes[1].ToLower() == "gpio")
                        {
                            if ((routes[3].ToLower() == "high") || (routes[3].ToLower() == "1"))
                            {
                                _controller.Write(pinNumber, PinValue.High);
                            }
                            else if ((routes[3].ToLower() == "low") || (routes[3].ToLower() == "0"))
                            {
                                _controller.Write(pinNumber, PinValue.Low);
                            }
                            else
                            {
                                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                                return;
                            }

                            WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
                            return;
                        }
                        else if (routes[1].ToLower() == "open")
                        {
                            if (routes[3].ToLower() == "input")
                            {
                                if (!_controller.IsPinOpen(pinNumber))
                                {
                                    _controller.OpenPin(pinNumber);
                                }

                                _controller.SetPinMode(pinNumber, PinMode.Input);
                            }
                            else if (routes[3].ToLower() == "output")
                            {
                                if (!_controller.IsPinOpen(pinNumber))
                                {
                                    _controller.OpenPin(pinNumber);
                                }

                                _controller.SetPinMode(pinNumber, PinMode.Output);
                            }
                            else
                            {
                                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                                return;
                            }
                        }
                        else if (routes[1].ToLower() == "close")
                        {
                            if (_controller.IsPinOpen(pinNumber))
                            {
                                _controller.ClosePin(pinNumber);
                            }
                        }
                        else
                        {
                            WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                            return;
                        }

                        WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
                        return;
                    }
                    else if (routes.Length == 2)
                    {
                        if (routes[1].ToLower() == "apikey")
                        {
                            // Check the security key
                            if (!CheckAPiKey(e.Context.Request.Headers))
                            {
                                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.Forbidden);
                                return;
                            }

                            if (e.Context.Request.HttpMethod != "POST")
                            {
                                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                                return;
                            }

                            // Get the param from the body
                            if (e.Context.Request.ContentLength64 == 0)
                            {
                                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                                return;
                            }

                            byte[] buff = new byte[e.Context.Request.ContentLength64];
                            e.Context.Request.InputStream.Read(buff, 0, buff.Length);
                            string rawData = new string(Encoding.UTF8.GetChars(buff));
                            var parameters = rawData.Split('=');
                            if (parameters.Length < 2)
                            {
                                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                                return;
                            }

                            if (parameters[0].ToLower() == "newkey")
                            {
                                _securityKey = parameters[1];
                                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
                                return;
                            }

                            WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                            return;
                        }
                    }
                    else
                    {
                        ApiDefault(e);
                    }
                }
                else if (url.ToLower().IndexOf("/favicon.ico") == 0)
                {
                    WebServer.SendFileOverHTTP(e.Context.Response, "favicon.ico", Resources.GetBytes(Resources.BinaryResources.favicon));
                }
#if HAS_STORAGE
                else
                {
                    // Very simple example serving a static file on an SD card
                    var files = _storage.GetFiles();
                    foreach (var file in files)
                    {
                        if (file.Name == url)
                        {
                            WebServer.SendFileOverHTTP(e.Context.Response, file);
                            return;
                        }
                    }

                    WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.NotFound);
                }
#endif

            }
            catch (Exception)
            {
                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.InternalServerError);
            }
        }

        private static bool CheckAPiKey(WebHeaderCollection headers)
        {
            var sec = headers.GetValues("ApiKey");
            if (sec != null)
            {
                if (sec.Length > 0)
                {
                    if (sec[0] == _securityKey)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void ParamHtml(WebServerEventArgs e)
        {
            var url = e.Context.Request.RawUrl;
            // Test with parameters
            var parameters = WebServer.DecodeParam(url);
            string toOutput = "<html><head>" +
                "<title>Hi from nanoFramework Server</title></head><body>Here are the parameters of this URL: <br />";
            foreach (var par in parameters)
            {
                toOutput += $"Parameter name: {par.Name}, Value: {par.Value}<br />";
            }
            toOutput += "</body></html>";
            WebServer.OutPutStream(e.Context.Response, toOutput);
        }

        private static void ApiDefault(WebServerEventArgs e)
        {
            string ret = $"HTTP/1.1 200 OK\r\nContent-Type: text/plain; charset=UTF-8\r\nCache-Control: no-cache\r\nConnection: close\r\n\r\n";
            ret += $"Your request type is: {e.Context.Request.HttpMethod}\r\n";
            ret += $"The request URL is: {e.Context.Request.RawUrl}\r\n";
            var parameters = WebServer.DecodeParam(e.Context.Request.RawUrl);
            if (parameters != null)
            {
                ret += "List of url parameters:\r\n";
                foreach (var param in parameters)
                {
                    ret += $"  Parameter name: {param.Name}, value: {param.Value}\r\n";
                }
            }

            if (e.Context.Request.Headers != null)
            {
                ret += $"Number of headers: {e.Context.Request.Headers.Count}\r\n";
            }
            else
            {
                ret += "There is no header in this request\r\n";
            }

            foreach (var head in e.Context.Request.Headers?.AllKeys)
            {
                ret += $"  Header name: {head}, Values:";
                var vals = e.Context.Request.Headers.GetValues(head);
                foreach (var val in vals)
                {
                    ret += $"{val} ";
                }

                ret += "\r\n";
            }

            if (e.Context.Request.ContentLength64 > 0)
            {

                ret += $"Size of content: {e.Context.Request.ContentLength64}\r\n";
                byte[] buff = new byte[e.Context.Request.ContentLength64];
                e.Context.Request.InputStream.Read(buff, 0, buff.Length);
                ret += $"Hex string representation:\r\n";
                for (int i = 0; i < buff.Length; i++)
                {
                    ret += buff[i].ToString("X") + " ";
                }

            }

            WebServer.OutPutStream(e.Context.Response, ret);

        }

        #region certificates & keys

        // X509 RSA key PEM format 2048 bytes
        // generate with openssl:
        // > openssl req -newkey rsa:2048 -nodes -keyout selfcert.key -x509 -days 365 -out selfcert.crt
        // and paste selfcert.crt content below:
        private const string _myWebServerCrt =
@"-----BEGIN CERTIFICATE-----
MIIDNzCCAh+gAwIBAgIBAjANBgkqhkiG9w0BAQsFADA7MQswCQYDVQQGEwJOTDER
MA8GA1UECgwIUG9sYXJTU0wxGTAXBgNVBAMMEFBvbGFyU1NMIFRlc3QgQ0EwHhcN
MTkwMjEwMTQ0NDA2WhcNMjkwMjEwMTQ0NDA2WjA0MQswCQYDVQQGEwJOTDERMA8G
A1UECgwIUG9sYXJTU0wxEjAQBgNVBAMMCWxvY2FsaG9zdDCCASIwDQYJKoZIhvcN
AQEBBQADggEPADCCAQoCggEBAMFNo93nzR3RBNdJcriZrA545Do8Ss86ExbQWuTN
owCIp+4ea5anUrSQ7y1yej4kmvy2NKwk9XfgJmSMnLAofaHa6ozmyRyWvP7BBFKz
NtSj+uGxdtiQwWG0ZlI2oiZTqqt0Xgd9GYLbKtgfoNkNHC1JZvdbJXNG6AuKT2kM
tQCQ4dqCEGZ9rlQri2V5kaHiYcPNQEkI7mgM8YuG0ka/0LiqEQMef1aoGh5EGA8P
hYvai0Re4hjGYi/HZo36Xdh98yeJKQHFkA4/J/EwyEoO79bex8cna8cFPXrEAjya
HT4P6DSYW8tzS1KW2BGiLICIaTla0w+w3lkvEcf36hIBMJcCAwEAAaNNMEswCQYD
VR0TBAIwADAdBgNVHQ4EFgQUpQXoZLjc32APUBJNYKhkr02LQ5MwHwYDVR0jBBgw
FoAUtFrkpbPe0lL2udWmlQ/rPrzH/f8wDQYJKoZIhvcNAQELBQADggEBAC465FJh
Pqel7zJngHIHJrqj/wVAxGAFOTF396XKATGAp+HRCqJ81Ry60CNK1jDzk8dv6M6U
HoS7RIFiM/9rXQCbJfiPD5xMTejZp5n5UYHAmxsxDaazfA5FuBhkfokKK6jD4Eq9
1C94xGKb6X4/VkaPF7cqoBBw/bHxawXc0UEPjqayiBpCYU/rJoVZgLqFVP7Px3sv
a1nOrNx8rPPI1hJ+ZOg8maiPTxHZnBVLakSSLQy/sWeWyazO1RnrbxjrbgQtYKz0
e3nwGpu1w13vfckFmUSBhHXH7AAS/HpKC4IH7G2GAk3+n8iSSN71sZzpxonQwVbo
pMZqLmbBm/7WPLc=
-----END CERTIFICATE-----";

        // this one is generated with the command below. We need a password.
        // > openssl rsa -des3 -in selfcert.key -out selfcertenc.key
        // the one below was encoded with '1234' as the password.
        private const string _myWebServerPrivateKey =
@"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAwU2j3efNHdEE10lyuJmsDnjkOjxKzzoTFtBa5M2jAIin7h5r
lqdStJDvLXJ6PiSa/LY0rCT1d+AmZIycsCh9odrqjObJHJa8/sEEUrM21KP64bF2
2JDBYbRmUjaiJlOqq3ReB30Zgtsq2B+g2Q0cLUlm91slc0boC4pPaQy1AJDh2oIQ
Zn2uVCuLZXmRoeJhw81ASQjuaAzxi4bSRr/QuKoRAx5/VqgaHkQYDw+Fi9qLRF7i
GMZiL8dmjfpd2H3zJ4kpAcWQDj8n8TDISg7v1t7HxydrxwU9esQCPJodPg/oNJhb
y3NLUpbYEaIsgIhpOVrTD7DeWS8Rx/fqEgEwlwIDAQABAoIBAQCXR0S8EIHFGORZ
++AtOg6eENxD+xVs0f1IeGz57Tjo3QnXX7VBZNdj+p1ECvhCE/G7XnkgU5hLZX+G
Z0jkz/tqJOI0vRSdLBbipHnWouyBQ4e/A1yIJdlBtqXxJ1KE/ituHRbNc4j4kL8Z
/r6pvwnTI0PSx2Eqs048YdS92LT6qAv4flbNDxMn2uY7s4ycS4Q8w1JXnCeaAnYm
WYI5wxO+bvRELR2Mcz5DmVnL8jRyml6l6582bSv5oufReFIbyPZbQWlXgYnpu6He
GTc7E1zKYQGG/9+DQUl/1vQuCPqQwny0tQoX2w5tdYpdMdVm+zkLtbajzdTviJJa
TWzL6lt5AoGBAN86+SVeJDcmQJcv4Eq6UhtRr4QGMiQMz0Sod6ettYxYzMgxtw28
CIrgpozCc+UaZJLo7UxvC6an85r1b2nKPCLQFaggJ0H4Q0J/sZOhBIXaoBzWxveK
nupceKdVxGsFi8CDy86DBfiyFivfBj+47BbaQzPBj7C4rK7UlLjab2rDAoGBAN2u
AM2gchoFiu4v1HFL8D7lweEpi6ZnMJjnEu/dEgGQJFjwdpLnPbsj4c75odQ4Gz8g
sw9lao9VVzbusoRE/JGI4aTdO0pATXyG7eG1Qu+5Yc1YGXcCrliA2xM9xx+d7f+s
mPzN+WIEg5GJDYZDjAzHG5BNvi/FfM1C9dOtjv2dAoGAF0t5KmwbjWHBhcVqO4Ic
BVvN3BIlc1ue2YRXEDlxY5b0r8N4XceMgKmW18OHApZxfl8uPDauWZLXOgl4uepv
whZC3EuWrSyyICNhLY21Ah7hbIEBPF3L3ZsOwC+UErL+dXWLdB56Jgy3gZaBeW7b
vDrEnocJbqCm7IukhXHOBK8CgYEAwqdHB0hqyNSzIOGY7v9abzB6pUdA3BZiQvEs
3LjHVd4HPJ2x0N8CgrBIWOE0q8+0hSMmeE96WW/7jD3fPWwCR5zlXknxBQsfv0gP
3BC5PR0Qdypz+d+9zfMf625kyit4T/hzwhDveZUzHnk1Cf+IG7Q+TOEnLnWAWBED
ISOWmrUCgYAFEmRxgwAc/u+D6t0syCwAYh6POtscq9Y0i9GyWk89NzgC4NdwwbBH
4AgahOxIxXx2gxJnq3yfkJfIjwf0s2DyP0kY2y6Ua1OeomPeY9mrIS4tCuDQ6LrE
TB6l9VGoxJL4fyHnZb8L5gGvnB1bbD8cL6YPaDiOhcRseC9vBiEuVg==
-----END RSA PRIVATE KEY-----";

        #endregion
    }
}
