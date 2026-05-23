using Hypah.WebServer.Html;
using Hypah.WebServer.Request;
using Hypah.WebServer.Server;

namespace Hypah.WebServer
{
    public interface IWebServer
    {
        /// <summary>
        /// Set your custom web page builder
        /// </summary>
        IPageBuilderFactory PageBuilder { get; set; }

        IEnumerable<Endpoint> Start();
        void Stop();

        void RegisterPage(IWebPage page);
        void Register(string fileExt, Func<WebUrl, IRequestHandler> request);
        void RegisterDynamicFileHandler(Func<WebUrl, (byte[]? Content, string Mime, int StatusCode)> handler);
    }
}
