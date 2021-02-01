//
// Copyright (c) 2020 Laurent Ellerbach and the project contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;
using System.Net;
using System.Text;

namespace nanoFramework.WebServer.Sample
{
    public class Person
    {
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class ControllerPerson
    {
        private static ArrayList _persons = new ArrayList();
        private static object _lock = new object();

        [Route("Person")]
        [Method("GET")]
        public void Get(WebServerEventArgs e)
        {
            var ret = "[";
            lock (_lock)
            {
                foreach (var person in _persons)
                {
                    var per = (Person)person;
                    ret += $"{{\"First\"=\"{per.First}\",\"Last\"=\"{per.Last}\"}},";
                }
            }
            if (ret.Length > 1)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            ret += "]";
            e.Context.Response.ContentType = "text/html";
            e.Context.Response.ContentLength64 = ret.Length;
            WebServer.OutPutStream(e.Context.Response, ret);
        }

        [Route("Person/Add")]
        [Method("POST")]
        public void AddPost(WebServerEventArgs e)
        {
            // Get the param from the body
            byte[] buff = new byte[e.Context.Request.ContentLength64];
            e.Context.Request.InputStream.Read(buff, 0, buff.Length);
            string rawData = new string(Encoding.UTF8.GetChars(buff));
            rawData = $"?{rawData}";
            AddPerson(e.Context.Response, rawData);
        }

        [Route("Person/Add")]
        [Method("GET")]
        public void AddGet(WebServerEventArgs e)
        {
            AddPerson(e.Context.Response, e.Context.Request.RawUrl);
        }

        private void AddPerson(HttpListenerResponse response, string url)
        {
            var parameters = WebServer.DecodeParam(url);
            Person person = new Person();
            foreach (var param in parameters)
            {
                if (param.Name.ToLower() == "first")
                {
                    person.First = param.Value;
                }
                else if (param.Name.ToLower() == "last")
                {
                    person.Last = param.Value;
                }
            }
            if ((person.Last != string.Empty) && (person.First != string.Empty))
            {
                lock (_lock)
                {
                    _persons.Add(person);
                }
                WebServer.OutputHttpCode(response, HttpStatusCode.Accepted);
            }
            else
            {
                WebServer.OutputHttpCode(response, HttpStatusCode.BadRequest);
            }
        }
    }
}
