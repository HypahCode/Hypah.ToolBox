using Hypah.WebServer.Html;

namespace Hypah.WebServer.Request
{
    public interface IPageManager
    {
        public Dictionary<string, IWebPage> Pages { get; }
    }
}
