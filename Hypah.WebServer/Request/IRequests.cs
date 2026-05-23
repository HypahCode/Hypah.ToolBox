using Hypah.WebServer.Html;

namespace Hypah.WebServer.Request
{
    public interface IRequests
    {
        void Register(string fileExt, Func<WebUrl, IRequestHandler> request);
        void RegisterPage(string path, IWebPage page);
        void RegisterDynamicFileHandler(Func<WebUrl, (byte[]? Content, string Mime, int StatusCode)> handler);
    }
}
