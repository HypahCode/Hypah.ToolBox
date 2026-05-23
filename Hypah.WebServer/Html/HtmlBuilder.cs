namespace Hypah.WebServer.Html
{
    public class HtmlBuilder
    {
        private readonly string _pageName;

        public HtmlBuilder(string pageName)
        {
            _pageName = pageName;
        }

        public string BuildHtml()
        {
            return _pageName;
        }
    }
}
