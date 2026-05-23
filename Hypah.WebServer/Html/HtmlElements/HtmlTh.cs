
namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlTh : HtmlBuilder
    {
        protected override string HtmlTag => "th";

        public Alignment Alignment = Alignment.Left;

        public HtmlTh(string text, Alignment alignment = Alignment.Left)
        {
            Alignment = alignment;
            Add(new HtmlText(text));
        }

        public override string BuildHtml(ICssBuilder cssBuilder)
        {
            Properties.Add("align", Alignment.ToString());
            return base.BuildHtml(cssBuilder);
        }
    }
}
