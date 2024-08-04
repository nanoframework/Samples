# 🌶️ to 🌶️🌶️ - .NET **nanoFramework** Webserver sample pack

Shows how to use .NET **nanoFramework** Webserver library in common usage scenarios.

## Samples

- [🌶️ - REST and GPIO](./WebServer.GpioRest/)
- [🌶️🌶️ - HTTP server](./WebServer.Sample/WebServer.Sample/)
- [🌶️🌶️ - HTTP server with file system](./WebServer.Sample/WebServer.Sample.FileSystem/)
- [🌶️🌶️ - Dependency Injection](./WebServer.DI/)

## Usage

You just need to specify a port and a timeout for the queries and add an event handler when a request is incoming. With this first way, you will have an event raised every time you'll receive a request.

```csharp
using (WebServer server = new WebServer(80, HttpProtocol.Http)
{
    // Add a handler for commands that are received by the server.
    server.CommandReceived += ServerCommandReceived;

    // Start the server.
    server.Start();

    Thread.Sleep(Timeout.Infinite);
}
```

You can as well pass a controller where you can use decoration for the routes and method supported.

```csharp
using (WebServer server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(ControllerPerson), typeof(ControllerTest) }))
{
    // Start the server.
    server.Start();

    Thread.Sleep(Timeout.Infinite);
}
```

In this case, you're passing 2 classes where you have public methods decorated which will be called every time the route is found.

With the previous example, a very simple and straight forward Test controller will look like that:

```csharp
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
}
```

In this example, the `RoutePostTest` will be called every time the called url will be `test` or `Test2` or `tEst42` or `TEST`, the url can be with parameters and the method GET. Be aware that `Test` won't call the function, neither `test/`.

The `RouteAnyTest`is called whenever the url is `test/any` whatever the method is.

There is a more advance example with simple REST API to get a list of Person and add a Person. Check it in the [sample](./WebServer.Sample/WebServer.Sample/ControllerPerson.cs).

> ![Important]
>
> - By default the routes are not case sensitive and the attribute **must** be lowercase
> - If you want to use case sensitive routes like in the previous example, use the attribute `CaseSensitive`. As in the previous example, you **must** write the route as you want it to be responded to.

## A simple GPIO controller REST API

You will find in simple [GPIO controller sample](https://github.com/nanoframework/Samples/tree/main/samples/Webserver/WebServer.GpioRest) REST API. The controller not case sensitive and is working like this:

- To open the pin 2 as output: <http://yoururl/open/2/output>
- To open pin 4 as input: <http://yoururl/open/4/input>
- To write the value high to pin 2: <http://yoururl/write/2/high>
  - You can use high or 1, it has the same effect and will place the pin in high value
  - You can use low of 0, it has the same effect and will place the pin in low value
- To read the pin 4: <http://yoururl/read/4>, you will get as a raw text `high`or `low`depending on the state

## DI integration with controllers

Check [WebServer.DI sample](./WebServer.DI) if you want to use controllers with automatically injecting services from [nanoFramework.DependencyInjection](https://github.com/nanoframework/nanoFramework.DependencyInjection).

- Type you credentials in Program.cs files
- Navigate to http://yoururl/test
- You should get text with two GUIDs, one should stay the same across all requests (singleton service)

## Authentication on controllers

Controllers support authentication. 3 types of authentications are currently implemented on controllers only:

- Basic: the classic user and password following the HTTP standard. Usage:
  - `[Authentication("Basic")]` will use the default credential of the webserver
  - `[Authentication("Basic:myuser mypassword")]` will use myuser as a user and my password as a password. Note: the user cannot contains spaces.
- APiKey in header: add ApiKey in headers with the API key. Usage:
  - `[Authentication("ApiKey")]` will use the default credential of the webserver
  - `[Authentication("ApiKeyc:akey")]` will use akey as ApiKey.
- None: no authentication required. Usage:
  - `[Authentication("None")]` will use the default credential of the webserver

The Authentication attribute applies to both public Classes an public Methods.

As for the rest of the controller, you can add attributes to define them, override them. The following example gives an idea of what can be done:

```csharp
[Authentication("Basic")]
class ControllerAuth
{
    [Route("authbasic")]
    public void Basic(WebServerEventArgs e)
    {
        WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
    }

    [Route("authbasicspecial")]
    [Authentication("Basic:user2 password")]
    public void Special(WebServerEventArgs e)
    {
        WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
    }

    [Authentication("ApiKey:superKey1234")]
    [Route("authapi")]
    public void Key(WebServerEventArgs e)
    {
        WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
    }

    [Route("authnone")]
    [Authentication("None")]
    public void None(WebServerEventArgs e)
    {
        WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
    }

    [Authentication("ApiKey")]
    [Route("authdefaultapi")]
    public void DefaultApi(WebServerEventArgs e)
    {
        WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
    }
}
```

And you can pass default credentials to the server:

```csharp
using (WebServer server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(ControllerPerson), typeof(ControllerTest), typeof(ControllerAuth) }))
{
    // To test authentication with various scenarios
    server.ApiKey = "ATopSecretAPIKey1234";
    server.Credential = new NetworkCredential("topuser", "topPassword");

    // Start the server.
    server.Start();

    Thread.Sleep(Timeout.Infinite);
}
```

With the previous example the following happens:

- All the controller by default, even when nothing is specified will use the controller credentials. In our case, the Basic authentication with the default user (topuser) and password (topPassword) will be used.
  - When calling <http://yoururl/authbasic> from a browser, you will be prompted for the user and password, use the default one topuser and topPassword to get access
  - When calling <http://yoururl/authnone>, you won't be prompted because the authentication has been overridden for no authentication
  - When calling <http://yoururl/authbasicspecial>, the user and password are different from the defautl ones, user2 and password is the right couple here
- If you would have define in the controller a specific user and password like `[Authentication("Basic:myuser mypassword")]`, then the default one for all the controller would have been myuser and mypassword
- When calling <http://yoururl/authapi>, you must pass the header `ApiKey` (case sensitive) with the value `superKey1234` to get authorized, this is overridden the default Basic authentication
- When calling <http://yoururl/authdefaultapi>, the default key `ATopSecretAPIKey1234` will be used so you have to pass it in the headers of the request

All up, this is an example to show how to use authentication, it's been defined to allow flexibility.

## Managing incoming queries thru events

Very basic usage is the following:

```csharp
private static void ServerCommandReceived(object source, WebServerEventArgs e)
{
    var url = e.Context.Request.RawUrl;
    Debug.WriteLine($"Command received: {url}, Method: {e.Context.Request.HttpMethod}");

    if (url.ToLower() == "/sayhello")
    {
        // This is simple raw text returned
        WebServer.OutPutStream(e.Context.Response, "It's working, url is empty, this is just raw text, /sayhello is just returning a raw text");
    }
    else
    {
        WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.NotFound);
    }
}
```

You can do more advance scenario like returning a full HTML page:

```csharp
WebServer.OutPutStream(e.Context.Response, "<html><head>" +
    "<title>Hi from nanoFramework Server</title></head><body>You want me to say hello in a real HTML page!<br/><a href='/useinternal'>Generate an internal text.txt file</a><br />" +
    "<a href='/Text.txt'>Download the Text.txt file</a><br>" +
    "Try this url with parameters: <a href='/param.htm?param1=42&second=24&NAme=Ellerbach'>/param.htm?param1=42&second=24&NAme=Ellerbach</a></body></html>");
```

And can get parameters from a URL a an example from the previous link on the param.html page:

```csharp
if (url.ToLower().IndexOf("/param.htm") == 0)
{
    // Test with parameters
    var parameters = WebServer.decryptParam(url);
    string toOutput = "<html><head>" +
        "<title>Hi from nanoFramework Server</title></head><body>Here are the parameters of this URL: <br />";
    foreach (var par in parameters)
    {
        toOutput += $"Parameter name: {par.Name}, Value: {par.Value}<br />";
    }
    toOutput += "</body></html>";
    WebServer.OutPutStream(e.Context.Response, toOutput);
}
```

To serve static files, you have to use the `nanoFramework.WebServer.FileSystem` nuget. This is only supported on devices having the `System.IO.FileSystem` capability.

```csharp
// E = USB storage
// D = SD Card
// I = Internal storage
// Adjust this based on your configuration
const string DirectoryPath = "I:\\";
string[] _listFiles;

// Gets the list of all files in a specific directory
// See the MountExample for more details if you need to mount an SD card and adjust here
// https://github.com/nanoframework/Samples/blob/main/samples/System.IO.FileSystem/MountExample/Program.cs
_listFiles = Directory.GetFiles(DirectoryPath);
// Remove the root directory
for (int i = 0; i < _listFiles.Length; i++)
{
    _listFiles[i] = _listFiles[i].Substring(DirectoryPath.Length);
}

var fileName = url.Substring(1);
// Note that the file name is case sensitive
// Very simple example serving a static file on an SD card                   
foreach (var file in _listFiles)
{
    if (file == fileName)
    {
        WebServer.SendFileOverHTTP(e.Context.Response, DirectoryPath + file);
        return;
    }
}

WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.NotFound);
```

> [!Important]
>
> Serving files requires the `nanoFramework.WebServer.FileSystem` nuget **AND** that the device supports storage so `System.IO.FileSystem`.

And also **REST API** is supported, here is a comprehensive example:

```csharp
if (url.ToLower().IndexOf("/api/") == 0)
{
    string ret = $"Your request type is: {e.Context.Request.HttpMethod}\r\n";
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
```

This API example is basic but as you get the method, you can choose what to do.

As you get the url, you can check for a specific controller called. And you have the parameters and the content payload!

Example of a result with call:

![result](./doc/POSTcapture.jpg)

And more! Check the complete example for more about this WebServer!

## Using HTTPS

You will need to generate a certificate and keys:

```csharp
X509Certificate _myWebServerCertificate509 = new X509Certificate2(_myWebServerCrt, _myWebServerPrivateKey, "1234");

// X509 RSA key PEM format 2048 bytes
        // generate with openssl:
        // > openssl req -newkey rsa:2048 -nodes -keyout selfcert.key -x509 -days 365 -out selfcert.crt
        // and paste selfcert.crt content below:
        private const string _myWebServerCrt =
@"-----BEGIN CERTIFICATE-----
MORETEXT
-----END CERTIFICATE-----";

        // this one is generated with the command below. We need a password.
        // > openssl rsa -des3 -in selfcert.key -out selfcertenc.key
        // the one below was encoded with '1234' as the password.
        private const string _myWebServerPrivateKey =
@"-----BEGIN RSA PRIVATE KEY-----
MORETEXTANDENCRYPTED
-----END RSA PRIVATE KEY-----";

using (WebServer server = new WebServer(443, HttpProtocol.Https))
{
    // Add a handler for commands that are received by the server.
    server.CommandReceived += ServerCommandReceived;
    server.HttpsCert = _myWebServerCertificate509;

    server.SslProtocols = System.Net.Security.SslProtocols.Tls | System.Net.Security.SslProtocols.Tls11 | System.Net.Security.SslProtocols.Tls12;
    // Start the server.
    server.Start();

    Thread.Sleep(Timeout.Infinite);
}
```

> [!IMPORTANT]
> Because the certificate above is not issued from a Certificate Authority it won't be recognized as a valid certificate. If you want to access the nanoFramework device with your browser, for example, you'll have to add the [CRT file](./WebServer.Sample/WebServer.Sample/webserver-cert.crt) as a trusted one. On Windows, you just have to double click on the CRT file and then click "Install Certificate...".

You can of course use the routes as defined earlier. Both will work, event or route with the notion of controller.

## Build the sample

1. Start Microsoft Visual Studio 2022 or Visual Studio 2019 and select `File > Open > Project/Solution`.
1. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
1. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> [!NOTE]
>
> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
>
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
