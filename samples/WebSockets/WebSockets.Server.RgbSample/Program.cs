using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.WebSockets;
using System.Net.WebSockets.Server;
using System.Net.WebSockets.WebSocketFrame;
using System.Threading;
using nanoFramework.AtomLite;
using nanoFramework.Networking;
using nanoFramework.WebServer;

namespace NFWebSockets.Server.RgbExample
{
    public class Program
    {
        private static WebSocketServer _wsServer;

        public static void Main()
        {
            //connect to wifi
            const string Ssid = "REPLACE-WITH-YOUR-SSID";
            const string Password = "REPLACE-WITH-YOUR-WIFI-KEY";

            // Give 60 seconds to the wifi join to happen
            CancellationTokenSource cs = new(60000);
            var success = WiFiNetworkHelper.ScanAndConnectDhcp(Ssid, Password, token: cs.Token);
            if (!success)
            {
                //Red Light indicates no Wifi connection
                AtomLite.NeoPixel.SetColor(Color.Red);
                Thread.Sleep(Timeout.Infinite);
            }

            //Initialize WebsocketServer with Webserver intergration
            _wsServer = new WebSocketServer(new WebSocketServerOptions()
            {
                MaxClients = 10,
                IsStandAlone = false
            });

            _wsServer.MessageReceived += WsServer_MessageReceived;
            _wsServer.Start();

            //WebServer
            WebServer webServer = new WebServer(80, HttpProtocol.Http);
            webServer.CommandReceived += WebServer_CommandReceived;
            webServer.Start();

            //The webapp url
            Debug.WriteLine($"http://{IPAddress.GetDefaultLocalAddress()}/");

            //set Led to green to indicate that everything is running OK. 
            AtomLite.NeoPixel.SetColor(Color.Green);

            Thread.Sleep(Timeout.Infinite);
        }


        //webserver receive message
        private static void WebServer_CommandReceived(object obj, WebServerEventArgs e)
        {
            //check the path of the request
            if (e.Context.Request.RawUrl == "/")
            {
                //check if this is a websocket request or a page request 
                if (e.Context.Request.Headers["Upgrade"] == "websocket")
                {
                    //Upgrade to a websocket
                    _wsServer.AddWebSocket(e.Context);
                }
                else
                {
                    //Return the WebApp
                    e.Context.Response.ContentType = "text/html";
                    e.Context.Response.ContentLength64 = html.Length;
                    WebServer.OutPutStream(e.Context.Response, html);
                }
            }
            else
            {
                //Send Page not Found
                e.Context.Response.StatusCode = 404;
                WebServer.OutPutStream(e.Context.Response, "Page not Found!");
            }
        }

        //Websocket Server Receive message
        private static void WsServer_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var wsServer = (WebSocketServer)sender;
            if (e.Frame.MessageType == WebSocketMessageType.Binary && e.Frame.MessageLength == 3)
            {
                AtomLite.NeoPixel.SetColor(Color.FromArgb(e.Frame.Buffer[0], e.Frame.Buffer[1], e.Frame.Buffer[2]));
                wsServer.BroadCast(e.Frame.Buffer);
            }
        }



        //WebApp
        private static string html = @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <title>WebSocket Client</title>
</head>
<body>
    <h1> Hello .NET nanoFramework WebsocketServer!</h1>
    Received: <br />
    <div id=""color"">
        color
    </div>
    <div id=""mycolorpicker"" class=""cp-default""></div>
    
    <script>
        var count = 0;
        var lastSendRGB
        var lastRGB
        var sendNext = true;
        var ws = new WebSocket(""ws://"" + location.hostname + "":80"");
        ws.binaryType = ""arraybuffer"";
        ws.onopen = function () {
            //ws.send(""Hello"");
        };
        ws.onmessage = function (evt) {
            if(evt.data instanceof ArrayBuffer)
			{
                var buf = new Uint8Array(evt.data).buffer;
                var dv = new DataView(buf);
                hexString = toHexString(new Uint8Array(evt.data));
                document.getElementById(""color"").innerHTML = hexString;
                document.body.style.backgroundColor = hexString;
                if (!sendNext) {
                    if (dv.getUint8(0) === lastSendRGB.getUint8(0) && dv.getUint8(1) === lastSendRGB.getUint8(1) && lastSendRGB.getUint8(2) === dv.getUint8(2)) {
                        if (lastSendRGB.getUint8(0) === lastRGB.getUint8(0) && lastSendRGB.getUint8(1) === lastRGB.getUint8(1) && lastSendRGB.getUint8(2) === lastRGB.getUint8(2)) {
                            sendNext = true;
                        } else {
                            lastSendRGB = lastRGB;
                            //document.getElementById(""number"").innerHTML = Number(lastSendRGB.getUint8(0));
                            ws.send(lastSendRGB); 
                        }
                    }
                }
            }else{
				console.log(evt.data);
			}
        };
        (function (s, t, u) { var v = (s.SVGAngle || t.implementation.hasFeature(""http://www.w3.org/TR/SVG11/feature#BasicStructure"", ""1.1"") ? ""SVG"" : ""VML""), picker, slide, hueOffset = 15, svgNS = 'http://www.w3.org/2000/svg'; var w = ['<div class=""picker-wrapper"">', '<div class=""picker""></div>', '<div class=""picker-indicator""></div>', '</div>', '<div class=""slide-wrapper"">', '<div class=""slide""></div>', '<div class=""slide-indicator""></div>', '</div>'].join(''); function mousePosition(a) { if (s.event && s.event.contentOverflow !== u) { return { x: s.event.offsetX, y: s.event.offsetY } } if (a.offsetX !== u && a.offsetY !== u) { return { x: a.offsetX, y: a.offsetY } } var b = a.target.parentNode.parentNode; return { x: a.layerX - b.offsetLeft, y: a.layerY - b.offsetTop } } function $(a, b, c) { a = t.createElementNS(svgNS, a); for (var d in b) a.setAttribute(d, b[d]); if (Object.prototype.toString.call(c) != '[object Array]') c = [c]; var i = 0, len = (c[0] && c.length) || 0; for (; i < len; i++)a.appendChild(c[i]); return a } if (v == 'SVG') { slide = $('svg', { xmlns: 'http://www.w3.org/2000/svg', version: '1.1', width: '100%', height: '100%' }, [$('defs', {}, $('linearGradient', { id: 'gradient-hsv', x1: '0%', y1: '100%', x2: '0%', y2: '0%' }, [$('stop', { offset: '0%', 'stop-color': '#FF0000', 'stop-opacity': '1' }), $('stop', { offset: '13%', 'stop-color': '#FF00FF', 'stop-opacity': '1' }), $('stop', { offset: '25%', 'stop-color': '#8000FF', 'stop-opacity': '1' }), $('stop', { offset: '38%', 'stop-color': '#0040FF', 'stop-opacity': '1' }), $('stop', { offset: '50%', 'stop-color': '#00FFFF', 'stop-opacity': '1' }), $('stop', { offset: '63%', 'stop-color': '#00FF40', 'stop-opacity': '1' }), $('stop', { offset: '75%', 'stop-color': '#0BED00', 'stop-opacity': '1' }), $('stop', { offset: '88%', 'stop-color': '#FFFF00', 'stop-opacity': '1' }), $('stop', { offset: '100%', 'stop-color': '#FF0000', 'stop-opacity': '1' })])), $('rect', { x: '0', y: '0', width: '100%', height: '100%', fill: 'url(#gradient-hsv)' })]); picker = $('svg', { xmlns: 'http://www.w3.org/2000/svg', version: '1.1', width: '100%', height: '100%' }, [$('defs', {}, [$('linearGradient', { id: 'gradient-black', x1: '0%', y1: '100%', x2: '0%', y2: '0%' }, [$('stop', { offset: '0%', 'stop-color': '#000000', 'stop-opacity': '1' }), $('stop', { offset: '100%', 'stop-color': '#CC9A81', 'stop-opacity': '0' })]), $('linearGradient', { id: 'gradient-white', x1: '0%', y1: '100%', x2: '100%', y2: '100%' }, [$('stop', { offset: '0%', 'stop-color': '#FFFFFF', 'stop-opacity': '1' }), $('stop', { offset: '100%', 'stop-color': '#CC9A81', 'stop-opacity': '0' })])]), $('rect', { x: '0', y: '0', width: '100%', height: '100%', fill: 'url(#gradient-white)' }), $('rect', { x: '0', y: '0', width: '100%', height: '100%', fill: 'url(#gradient-black)' })]) } else if (v == 'VML') { slide = ['<DIV style=""position: relative; width: 100%; height: 100%"">', '<v:rect style=""position: absolute; top: 0; left: 0; width: 100%; height: 100%"" stroked=""f"" filled=""t"">', '<v:fill type=""gradient"" method=""none"" angle=""0"" color=""red"" color2=""red"" colors=""8519f fuchsia;.25 #8000ff;24903f #0040ff;.5 aqua;41287f #00ff40;.75 #0bed00;57671f yellow""></v:fill>', '</v:rect>', '</DIV>'].join(''); picker = ['<DIV style=""position: relative; width: 100%; height: 100%"">', '<v:rect style=""position: absolute; left: -1px; top: -1px; width: 101%; height: 101%"" stroked=""f"" filled=""t"">', '<v:fill type=""gradient"" method=""none"" angle=""270"" color=""#FFFFFF"" opacity=""100%"" color2=""#CC9A81"" o:opacity2=""0%""></v:fill>', '</v:rect>', '<v:rect style=""position: absolute; left: 0px; top: 0px; width: 100%; height: 101%"" stroked=""f"" filled=""t"">', '<v:fill type=""gradient"" method=""none"" angle=""0"" color=""#000000"" opacity=""100%"" color2=""#CC9A81"" o:opacity2=""0%""></v:fill>', '</v:rect>', '</DIV>'].join(''); if (!t.namespaces['v']) t.namespaces.add('v', 'urn:schemas-microsoft-com:vml', '#default#VML') } function hsv2rgb(a) { var R, G, B, X, C; var h = (a.h % 360) / 60; C = a.v * a.s; X = C * (1 - Math.abs(h % 2 - 1)); R = G = B = a.v - C; h = ~~h; R += [C, X, 0, 0, X, C][h]; G += [X, C, C, X, 0, 0][h]; B += [0, 0, X, C, C, X][h]; var r = Math.floor(R * 255); var g = Math.floor(G * 255); var b = Math.floor(B * 255); return { r: r, g: g, b: b, hex: ""#"" + (16777216 | b | (g << 8) | (r << 16)).toString(16).slice(1) } } function rgb2hsv(a) { var r = a.r; var g = a.g; var b = a.b; if (a.r > 1 || a.g > 1 || a.b > 1) { r /= 255; g /= 255; b /= 255 } var H, S, V, C; V = Math.max(r, g, b); C = V - Math.min(r, g, b); H = (C == 0 ? null : V == r ? (g - b) / C + (g < b ? 6 : 0) : V == g ? (b - r) / C + 2 : (r - g) / C + 4); H = (H % 6) * 60; S = C == 0 ? 0 : C / V; return { h: H, s: S, v: V } } function slideListener(d, e, f) { return function (a) { a = a || s.event; var b = mousePosition(a); d.h = b.y / e.offsetHeight * 360 + hueOffset; d.s = d.v = 1; var c = hsv2rgb({ h: d.h, s: 1, v: 1 }); f.style.backgroundColor = c.hex; d.callback && d.callback(c.hex, { h: d.h - hueOffset, s: d.s, v: d.v }, { r: c.r, g: c.g, b: c.b }, u, b) } }; function pickerListener(d, e) { return function (a) { a = a || s.event; var b = mousePosition(a), width = e.offsetWidth, height = e.offsetHeight; d.s = b.x / width; d.v = (height - b.y) / height; var c = hsv2rgb(d); d.callback && d.callback(c.hex, { h: d.h - hueOffset, s: d.s, v: d.v }, { r: c.r, g: c.g, b: c.b }, b) } }; var x = 0; function ColorPicker(f, g, h) { if (!(this instanceof ColorPicker)) return new ColorPicker(f, g, h); this.h = 0; this.s = 1; this.v = 1; if (!h) { var i = f; i.innerHTML = w; this.slideElement = i.getElementsByClassName('slide')[0]; this.pickerElement = i.getElementsByClassName('picker')[0]; var j = i.getElementsByClassName('slide-indicator')[0]; var k = i.getElementsByClassName('picker-indicator')[0]; ColorPicker.fixIndicators(j, k); this.callback = function (a, b, c, d, e) { ColorPicker.positionIndicators(j, k, e, d); g(a, b, c) } } else { this.callback = h; this.pickerElement = g; this.slideElement = f } if (v == 'SVG') { var l = slide.cloneNode(true); var m = picker.cloneNode(true); var n = l.getElementsByTagName('linearGradient')[0]; var o = l.getElementsByTagName('rect')[0]; n.id = 'gradient-hsv-' + x; o.setAttribute('fill', 'url(#' + n.id + ')'); var p = [m.getElementsByTagName('linearGradient')[0], m.getElementsByTagName('linearGradient')[1]]; var q = m.getElementsByTagName('rect'); p[0].id = 'gradient-black-' + x; p[1].id = 'gradient-white-' + x; q[0].setAttribute('fill', 'url(#' + p[1].id + ')'); q[1].setAttribute('fill', 'url(#' + p[0].id + ')'); this.slideElement.appendChild(l); this.pickerElement.appendChild(m); x++ } else { this.slideElement.innerHTML = slide; this.pickerElement.innerHTML = picker } addEventListener(this.slideElement, 'click', slideListener(this, this.slideElement, this.pickerElement)); addEventListener(this.pickerElement, 'click', pickerListener(this, this.pickerElement)); enableDragging(this, this.slideElement, slideListener(this, this.slideElement, this.pickerElement)); enableDragging(this, this.pickerElement, pickerListener(this, this.pickerElement)) }; function addEventListener(a, b, c) { if (a.attachEvent) { a.attachEvent('on' + b, c) } else if (a.addEventListener) { a.addEventListener(b, c, false) } } function enableDragging(b, c, d) { var e = false; addEventListener(c, 'mousedown', function (a) { e = true }); addEventListener(c, 'mouseup', function (a) { e = false }); addEventListener(c, 'mouseout', function (a) { e = false }); addEventListener(c, 'mousemove', function (a) { if (e) { d(a) } }) } ColorPicker.hsv2rgb = function (a) { var b = hsv2rgb(a); delete b.hex; return b }; ColorPicker.hsv2hex = function (a) { return hsv2rgb(a).hex }; ColorPicker.rgb2hsv = rgb2hsv; ColorPicker.rgb2hex = function (a) { return hsv2rgb(rgb2hsv(a)).hex }; ColorPicker.hex2hsv = function (a) { return rgb2hsv(ColorPicker.hex2rgb(a)) }; ColorPicker.hex2rgb = function (a) { return { r: parseInt(a.substr(1, 2), 16), g: parseInt(a.substr(3, 2), 16), b: parseInt(a.substr(5, 2), 16) } }; function setColor(a, b, d, e) { a.h = b.h % 360; a.s = b.s; a.v = b.v; var c = hsv2rgb(a); var f = { y: (a.h * a.slideElement.offsetHeight) / 360, x: 0 }; var g = a.pickerElement.offsetHeight; var h = { x: a.s * a.pickerElement.offsetWidth, y: g - a.v * g }; a.pickerElement.style.backgroundColor = hsv2rgb({ h: a.h, s: 1, v: 1 }).hex; a.callback && a.callback(e || c.hex, { h: a.h, s: a.s, v: a.v }, d || { r: c.r, g: c.g, b: c.b }, h, f); return a }; ColorPicker.prototype.setHsv = function (a) { return setColor(this, a) }; ColorPicker.prototype.setRgb = function (a) { return setColor(this, rgb2hsv(a), a) }; ColorPicker.prototype.setHex = function (a) { return setColor(this, ColorPicker.hex2hsv(a), u, a) }; ColorPicker.positionIndicators = function (a, b, c, d) { if (c) { b.style.left = 'auto'; b.style.right = '0px'; b.style.top = '0px'; a.style.top = (c.y - a.offsetHeight / 2) + 'px' } if (d) { b.style.top = (d.y - b.offsetHeight / 2) + 'px'; b.style.left = (d.x - b.offsetWidth / 2) + 'px' } }; ColorPicker.fixIndicators = function (a, b) { b.style.pointerEvents = 'none'; a.style.pointerEvents = 'none' }; s.ColorPicker = ColorPicker })(window, window.document);
        ColorPicker(document.getElementById('mycolorpicker'), function (hex, hsv, rgb) {
            //document.body.style.backgroundColor = hex;
            
            var buffer = new ArrayBuffer(3);
            var dataView = new DataView(buffer);
            var int8View = new Int8Array(buffer);
            dataView.setUint8(0, rgb.r);
            dataView.setUint8(1, rgb.g);
            dataView.setUint8(2, rgb.b);
            if (sendNext) {
                lastRGB = dataView;
                ws.send(dataView);
                //document.getElementById(""number"").innerHTML = Number(dataView.getUint8(0));
                lastSendRGB = dataView;
                sendNext = false;
            }else{ 
                lastRGB = dataView;
            }
            
        });
        function toHexString(byteArray) 
        {
            return ""#"" + Array.from(byteArray, function(byte) {
                return ('0' + (byte & 0xFF).toString(16)).slice(-2);
            }).join('')
        }
    </script>
</body>
</html>
";
    }
}
