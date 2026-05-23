namespace Hypah.WebServer.Html.HtmlElements.CodeHighlight
{
    public class HtmlCodeHighlight : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => throw new NotImplementedException();

        private readonly string _content;
        private readonly string _syntax;

        public HtmlCodeHighlight(string content, string syntax)
        {
            _content = content;
            _syntax = syntax;
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            return CodeLightBuilder.Light(_content, _syntax);
        }
    }
}
