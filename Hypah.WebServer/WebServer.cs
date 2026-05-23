using Hypah.WebServer.Attributes;
using Hypah.WebServer.Html;
using Hypah.WebServer.Html.DefaultPageBuilder;
using Hypah.WebServer.Request;
using Hypah.WebServer.Server;
using System.Reflection;

namespace Hypah.WebServer
{
    public sealed class WebServer : IWebServer, IDisposable
    {
        private readonly IServerLogger? _logger;
        private readonly HttpServerRunner _runner;
        public IPageBuilderFactory PageBuilder { get; set; } = new DefaultPageBuilderFactory();

        public WebServer(int port, IServerLogger? logger)
        {
            _logger = logger;
            _runner = new HttpServerRunner(port, this, _logger);
        }

        public IEnumerable<Endpoint> Start()
        {
            return _runner.Start();
        }

        public void Stop()
        {
            _runner.Stop();
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        public void RegisterPage(IWebPage page)
        {
            var pageAttrib = page.GetType().GetCustomAttribute<WebPageAttribute>();
            var path = pageAttrib.Path.ToLower();
            if (string.IsNullOrEmpty(path)) path = "index";
            _runner.RequestRegister.RegisterPage(path, page);
        }

        public void Register(string fileExt, Func<WebUrl, IRequestHandler> request)
        {
            _runner.RequestRegister.Register(fileExt, request);
        }

        public void RegisterPages(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var attrib = type.GetCustomAttribute<WebPageAttribute>();
                if (attrib != null && typeof(IWebPage).IsAssignableFrom(type))
                {
                    try
                    {
                        var webPage = Activator.CreateInstance(type) as IWebPage;
                        if (webPage != null)
                        {
                            RegisterPage(webPage);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger?.Log(LogLevel.Warn, nameof(WebServer), $"Could not create instance of web page for {type.FullName}");
                    }
                }
            }
        }

        public void RegisterDynamicFileHandler(Func<WebUrl, (byte[]? Content, string Mime, int StatusCode)> handler)
        {
            _runner.RequestRegister.RegisterDynamicFileHandler(handler);
        }
    }
}
