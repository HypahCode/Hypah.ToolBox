
namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlBr : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => throw new NotSupportedException();

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            return "<br>";
        }
    }
}
