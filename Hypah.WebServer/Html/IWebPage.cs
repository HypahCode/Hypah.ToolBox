
using Hypah.WebServer.Request;

namespace Hypah.WebServer.Html
{
    public interface IWebPage
    {
        string Name { get; }

        IHtmlBuildable BuildContent(WebUrl url, ICssBuilder cssBuilder);
    }
}
