//
// Copyright (c) 2020 Laurent Ellerbach and the project contributors
// See LICENSE file in the project root for full license information.
//

using System.Net;

namespace nanoFramework.WebServer.Sample
{
    public class ControllerTest
    {
        [Route("test"), Route("Test2"), Route("tEst42"), Route("TEST")]
        [CaseSensitive]
        [Method("GET")]
        public void RoutePostTest(WebServerEventArgs e)
        {
            string route = $"The route asked is {e.Context.Request.RawUrl.TrimStart('/').Split('/')[0]}";
            e.Context.Response.ContentType = "text/plain";
            WebServer.OutPutStream(e.Context.Response, route);
        }

        [Route("test/any")]
        public void RouteAnyTest(WebServerEventArgs e)
        {
            WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
        }

        [Route("urlencode")]
        public void UrlEncode(WebServerEventArgs e)
        {
            var rawUrl = e.Context.Request.RawUrl;
            var paramsUrl = WebServer.DecodeParam(rawUrl);
            string ret = "Parameters | Encoded | Decoded";
            foreach (var param in paramsUrl)
            {
                ret += $"{param.Name} | ";
                ret += $"{param.Value} | ";
                // Need to wait for latest version of System.Net
                // See https://github.com/nanoframework/lib-nanoFramework.System.Net.Http/blob/develop/nanoFramework.System.Net.Http/Http/System.Net.HttpUtility.cs
                ret += $"{System.Web.HttpUtility.UrlDecode(param.Value)}";
                ret += "\r\n";
            }
            WebServer.OutPutStream(e.Context.Response, ret);
        }
    }
}
