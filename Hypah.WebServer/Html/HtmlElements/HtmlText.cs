
namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlText : IHtmlBuildable
    {
        public string Text { get; }

        public List<IHtmlBuildable> Builders => throw new NotSupportedException();

        public HtmlText(string text)
        {
            Text = text;
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            return Text;
        }
    }
}
