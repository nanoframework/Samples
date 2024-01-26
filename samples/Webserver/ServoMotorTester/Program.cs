using System;
using System.Device.Pwm;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Networking;
using nanoFramework.WebServer;

namespace ServoMotorTester
{
    public class Program
    {
        // Adjust the SSID and the password
        private const string Ssid = "yourSSID";
        private const string Password = "yourPassword";

        // Adjust the pin used for the servo motor
        private static int _pinPwm = 7;

        private static WebServer _server;
        private static PwmChannel _pwm;
        private static ServoInformation _servo = new ServoInformation();

        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework ServoMotor tester!");
            Console.WriteLine($"Connected with wifi credentials. IP Address: {GetCurrentIPAddress()}");

            var res = WifiNetworkHelper.ConnectDhcp(Ssid, Password, token: new CancellationTokenSource(60_000).Token);
            if (!res)
            {
                Console.WriteLine("Can't connect to the WiFi, check your credentials.");
                return;
            }

            // Setting up the web server
            _server = new WebServer(80, HttpProtocol.Http);
            _server.CommandReceived += ServerCommandReceived;
            _server.Start();

            // setting up the PWM
            Configuration.SetPinFunction(_pinPwm, DeviceFunction.PWM1);
            _pwm = PwmChannel.CreateFromPin(_pinPwm, frequency: (int)_servo.Frequency);
            _pwm.Start();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void ServerCommandReceived(object obj, WebServerEventArgs e)
        {
            const string PageProcess = "req";
            const string ParamMinPulse = "mi";
            const string ParamMaxPulse = "ma";
            const string ParamFrequency = "fe";
            const string ParamPosition = "po";

            var url = e.Context.Request.RawUrl;
            if (url.IndexOf($"/{PageProcess}") == 0)
            {
                string resp = string.Empty;
                var parameters = WebServer.DecodeParam(url);
                foreach (UrlParameter param in parameters)
                {
                    if (param.Name == ParamMinPulse)
                    {
                        if (uint.TryParse(param.Value, out var value))
                        {
                            _servo.MinPulse = value;
                            resp += $"{nameof(_servo.MinPulse)}: {_servo.MinPulse} ";
                        }

                        continue;
                    }

                    if (param.Name == ParamMaxPulse)
                    {
                        if (uint.TryParse(param.Value, out var value))
                        {
                            _servo.MaxPulse = value;
                            resp += $"{nameof(_servo.MaxPulse)}: {_servo.MaxPulse} ";
                        }

                        continue;
                    }

                    if (param.Name == ParamFrequency)
                    {
                        if (uint.TryParse(param.Value, out var value))
                        {
                            _servo.Frequency = value;
                            _pwm.Frequency = (int)_servo.Frequency;
                            resp += $"{nameof(_servo.Frequency)}: {_servo.Frequency} ";
                        }

                        continue;
                    }

                    if (param.Name == ParamPosition)
                    {
                        if (uint.TryParse(param.Value, out var value))
                        {
                            _servo.Position = value;
                            if (_servo.Position > 100)
                            {
                                _servo.Position = 100;
                            }

                            resp += $"{nameof(_servo.Position)}: {_servo.Position} ";
                        }

                        continue;
                    }
                }

                uint duration = (uint)(_servo.MinPulse + (_servo.MaxPulse - _servo.MinPulse) / 100.0f * _servo.Position);
                _pwm.DutyCycle = (double)duration / 1_000_000 * _pwm.Frequency;
                resp += $"{nameof(_pwm.DutyCycle)} {_pwm.DutyCycle} ";

                WebServer.OutPutStream(e.Context.Response, resp);
                return;
            }

            string strResp = string.Empty;
            strResp += "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
            strResp += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Servo Motor Discover</title>";
            strResp += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/></head><body>";
            strResp += "<meta http-equiv=\"Cache-control\" content=\"no-cache\"/>";
            //create the script part
            strResp += "<script language=\"JavaScript\">var xhr = new XMLHttpRequest();function btnclicked(boxMSG, cmdSend) {";
            strResp += "document.getElementById('status').innerHTML=\"waiting\";";
            strResp += "xhr.open('GET', cmdSend + boxMSG.value);";
            strResp += "xhr.send(null); xhr.onreadystatechange = function() {if (xhr.readyState == 4) {document.getElementById('status').innerHTML=xhr.responseText;}};}";
            strResp += "</script>";
            //body
            strResp += "</head><body><table >";
            strResp += $"<tr><td>Min pulse</td><td><input id=\"MinPulse\" type=\"text\" value=\"{_servo.MinPulse}\" /></td><td><input id=\"MinPulseBtn\" type=\"button\" value=\"Update\" onclick=\"btnclicked(document.getElementById ('MinPulse'),'{PageProcess}?{ParamMinPulse}=')\"  /></td></tr>";
            strResp += $"<tr><td>Max pulse</td><td><input id=\"MaxPulse\" type=\"text\" value=\"{_servo.MaxPulse}\" /></td><td><input id=\"MaxPulseBtn\" type=\"button\" value=\"Update\" onclick=\"btnclicked(document.getElementById('MaxPulse'),'{PageProcess}?{ParamMaxPulse}=')\" /></td></tr>";
            strResp += $"<tr><td>Frequency</td><td><input id=\"Frequency\" type=\"text\" value=\"{_servo.Frequency}\" /></td><td><input id=\"FrequencyBtn\" type=\"button\" value=\"Update\" onclick=\"btnclicked(document.getElementById('Frequency'),'{PageProcess}?{ParamFrequency}=')\" /></td></tr>";
            strResp += $"<tr><td>Position %</td><td><input id=\"Position\" type=\"text\" value=\"{_servo.Position}\" /></td><td><input id=\"PositionBtn\" type=\"button\" value=\"Update\" onclick=\"btnclicked(document.getElementById('Position'),'{PageProcess}?{ParamPosition}=')\" /></td></tr>";
            strResp += "</table><div id=\"status\"></div></body></html>";
            WebServer.OutPutStream(e.Context.Response, strResp);
        }

        private static string GetCurrentIPAddress()
        {
            NetworkInterface ni = NetworkInterface.GetAllNetworkInterfaces()[0];

            // get first NI ( Wifi on ESP32 )
            return ni.IPv4Address.ToString();
        }
    }
}
