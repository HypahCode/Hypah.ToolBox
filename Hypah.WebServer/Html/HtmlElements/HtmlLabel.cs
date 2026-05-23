
namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlLabel : HtmlBuilder
    {
        protected override string HtmlTag => "label";
        public HtmlLabel(string Text, string? @for = null)
        {
            if (@for != null)
            {
                Properties["for"] = @for;
            }
            Add(new HtmlText(Text));
        }
    }
}
