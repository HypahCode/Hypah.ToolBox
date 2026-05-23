using Hypah.WebServer.Html;
using Hypah.WebServer.Request.Requests;
using Hypah.WebServer.Server;

namespace Hypah.WebServer.Request
{
    internal sealed class RequestRegister : IRequests, IPageManager
    {
        private readonly Dictionary<string, Func<WebUrl, IRequestHandler>> _requestHandlers = new Dictionary<string, Func<WebUrl, IRequestHandler>>();
        internal Func<WebUrl, (byte[]? Content, string Mime, int StatusCode)>? DynamicFileHandler { get; private set; }

        public Dictionary<string, IWebPage> Pages { get; } = new Dictionary<string, IWebPage>();
        private readonly IHttpServer _server;
        private readonly IWebServer _webServer;

        public RequestRegister(IHttpServer server, IWebServer webServer)
        {
            _server = server;
            _webServer = webServer;
        }

        public IRequestHandler? GetDynamicPage(WebUrl url)
        {
            var path = url.GetFile().ToLower();
            if (string.IsNullOrEmpty(path))
            {
                path = "index";
            }

            if (Pages.ContainsKey(path))
            {
                var webpage = Pages[path];
                var page = _webServer.PageBuilder.CreateNewPage(url, this);
                return new HtmlRequest(page, webpage);
            }

            return null;
        }

        public IRequestHandler? GetRequestHandler(WebUrl url)
        {
            var file = url.GetFile();
            var ext = Path.GetExtension(file).ToLower();

            if (_requestHandlers.ContainsKey(ext))
            {
                return new FileRequest(url);
            }

            return null;
        }

        public void Register(string fileExt, Func<WebUrl, IRequestHandler> request)
        {
            _requestHandlers[fileExt.ToLower()] = request;
        }

        public void RegisterPage(string path, IWebPage page)
        {
            Pages[path.ToLower()] = page;
        }

        public void RegisterDynamicFileHandler(Func<WebUrl, (byte[]? Content, string Mime, int StatusCode)> handler)
        {
            DynamicFileHandler = handler;
        }
    }
}
