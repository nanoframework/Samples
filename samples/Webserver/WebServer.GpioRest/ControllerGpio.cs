using System;
using System.Device.Gpio;
using System.Net;

namespace nanoFramework.WebServer.GpioRest
{
    class ControllerGpio
    {
        private static GpioController _controller = new GpioController();

        /// <summary>
        /// Open a pin, ex /open/2/output 
        /// </summary>
        /// <param name="e">The server context</param>
        [Route("open")]
        public void Open(WebServerEventArgs e)
        {
            try
            {
                var rawUrl = e.Context.Request.RawUrl.TrimStart('/');
                var args = rawUrl.Split('/');
                if (args.Length < 3)
                {
                    WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                    return;
                }

                var pinNumber = Convert.ToInt32(args[1]);

                if (!_controller.IsPinOpen(pinNumber))
                {
                    _controller.OpenPin(pinNumber);
                }

                if (args[2].ToLower() == "output")
                {
                    _controller.SetPinMode(pinNumber, PinMode.Output);
                }
                else if (args[2].ToLower() == "input")
                {
                    _controller.SetPinMode(pinNumber, PinMode.Input);
                }
                else
                {
                    WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                    return;
                }

                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Close an open pin, ex /close/2
        /// </summary>
        /// <param name="e">The server context</param>
        [Route("close")]
        public void Close(WebServerEventArgs e)
        {
            try
            {
                var rawUrl = e.Context.Request.RawUrl.TrimStart('/');
                var args = rawUrl.Split('/');
                if (args.Length < 2)
                {
                    WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                    return;
                }

                var pinNumber = Convert.ToInt32(args[1]);

                if (_controller.IsPinOpen(pinNumber))
                {
                    _controller.ClosePin(pinNumber);
                }

                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Write a value in an open pin, ex: /write/2/high or /write/2/1
        /// You can use either high or 1 / low or 0
        /// </summary>
        /// <param name="e">The server context</param>
        [Route("write")]
        public void Write(WebServerEventArgs e)
        {
            try
            {
                var rawUrl = e.Context.Request.RawUrl.TrimStart('/');
                var args = rawUrl.Split('/');
                if (args.Length < 3)
                {
                    WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                    return;
                }

                var pinNumber = Convert.ToInt32(args[1]);
                if (!_controller.IsPinOpen(pinNumber))
                {
                    WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                    return;
                }

                if ((args[2].ToLower() == "high") || (args[2] == "1"))
                {
                    _controller.Write(pinNumber, PinValue.High);
                }
                else if ((args[2].ToLower() == "low") || (args[2] == "0"))
                {
                    _controller.Write(pinNumber, PinValue.Low);
                }

                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Read the value, the raw value "low" or "high" is returned as a text.
        /// </summary>
        /// <param name="e">The server context</param>
        [Route("read")]
        public void Read(WebServerEventArgs e)
        {
            try
            {
                var rawUrl = e.Context.Request.RawUrl.TrimStart('/');
                var args = rawUrl.Split('/');
                if (args.Length < 2)
                {
                    WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                    return;
                }

                var pinNumber = Convert.ToInt32(args[1]);
                if (!_controller.IsPinOpen(pinNumber))
                {
                    WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                    return;
                }

                var result = _controller.Read(pinNumber);
                e.Context.Response.ContentType = "text/plain";
                WebServer.OutPutStream(e.Context.Response, result.ToString());
            }
            catch (Exception)
            {
                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
            }
        }
    }
}
