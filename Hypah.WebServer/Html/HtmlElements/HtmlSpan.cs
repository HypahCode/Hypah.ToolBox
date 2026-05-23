
namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlSpan : HtmlBuilder
    {

        protected override string HtmlTag => "span";
        public HtmlSpan()
        {
        }

        public HtmlSpan(string styleClass)
        {
            Class = styleClass;
        }

    }
}
