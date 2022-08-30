//
// Copyright (c) 2020 Laurent Ellerbach and the project contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.WebServer.Sample
{
    public class ControllerTest
    {
        private readonly ITextService _textService;
        private readonly ITextServiceSingleton _textServiceSingleton;

        public ControllerTest(ITextService textService, ITextServiceSingleton textServiceSingleton)
        {
            _textService = textService;
            _textServiceSingleton = textServiceSingleton;
        }

        [Route("test")]
        [Method("GET")]
        public void RoutePostTest(WebServerEventArgs e)
        {
            var content = $"Response from {nameof(ITextService)}: {_textService.GetText()}. Response from {nameof(ITextServiceSingleton)}: {_textServiceSingleton.GetText()}";
            e.Context.Response.ContentType = "text/plain";
            WebServer.OutPutStream(e.Context.Response, content);
        }
    }
}
