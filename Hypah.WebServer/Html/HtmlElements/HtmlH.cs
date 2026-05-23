
namespace Hypah.WebServer.Html.HtmlElements
{
    public enum H { H1, H2, H3, H4 }

    public class HtmlH : HtmlBuilder
    {
        public HtmlH(H size)
        {
            Size = size;
        }

        public HtmlH(H size, string text)
        {
            Size = size;
            Add(new HtmlText(text));
        }

        public H Size { get; }

        protected override string HtmlTag => Size.ToString();
    }
}
