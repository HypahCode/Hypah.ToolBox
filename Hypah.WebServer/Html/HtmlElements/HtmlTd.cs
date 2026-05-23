
namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlTd : HtmlBuilder
    {
        protected override string HtmlTag => "td";

        public Alignment Alignment = Alignment.Left;

        public HtmlTd() { }

        public HtmlTd(Alignment alignment)
        {
            Alignment = alignment;
        }

        public override string BuildHtml(ICssBuilder cssBuilder)
        {
            Properties.Add("align", Alignment.ToString());
            return base.BuildHtml(cssBuilder);
        }
    }
}
