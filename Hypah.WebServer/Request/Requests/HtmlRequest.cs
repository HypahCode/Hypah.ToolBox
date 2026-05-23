using Hypah.WebServer.Html;

namespace Hypah.WebServer.Request.Requests
{
    internal class HtmlRequest : IRequestHandler
    {
        private readonly IPageBuilder _pageBulder;
        private readonly IWebPage _webPage;

        public HtmlRequest(IPageBuilder pageBulder, IWebPage webPage)
        {
            _pageBulder = pageBulder;
            _webPage = webPage;
        }

        public bool Handle(IRequestServer server)
        {
            string html = "";
            try
            {
                html = _pageBulder.Build(_webPage);
            }
            catch (Exception ex)
            {
                html = ex.ToString();
            }
            server.SendToBrowser(MimeType.TextHtml, StatusCode.StatusOk, html);
            return true;
        }
    }
}
