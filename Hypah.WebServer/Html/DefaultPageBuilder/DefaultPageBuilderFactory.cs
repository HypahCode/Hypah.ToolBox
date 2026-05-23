using Hypah.WebServer.Request;

namespace Hypah.WebServer.Html.DefaultPageBuilder
{
    internal class DefaultPageBuilderFactory : IPageBuilderFactory
    {
        public IPageBuilder CreateNewPage(WebUrl url, IPageManager pageManager)
        {
            return new DefaultPageBuilder(url, pageManager);
        }
    }
}
