
namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlButton : HtmlBuilder
    {
        protected override string HtmlTag => "button";
        public HtmlButton(string Text, string type = "submit")
        {
            Add(new HtmlText(Text));
        }
    }
}
