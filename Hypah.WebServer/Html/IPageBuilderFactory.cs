using Hypah.WebServer.Request;

namespace Hypah.WebServer.Html
{
    public interface IPageBuilderFactory
    {
        public IPageBuilder CreateNewPage(WebUrl url, IPageManager pageManager);
    }
}
