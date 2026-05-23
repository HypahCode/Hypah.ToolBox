
namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlImg : HtmlBuilder
    {

        protected override string HtmlTag => "img";
        public HtmlImg(string src)
        {
            Properties["src"] = src;
            Properties["alt"] = src;
        }
    }
}
