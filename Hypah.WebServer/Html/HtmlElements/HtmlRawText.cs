namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlRawText : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => throw new NotSupportedException();

        public string Text { get; }

        public HtmlRawText(string text)
        {
            Text = text;
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            return Text;
        }
    }
}
