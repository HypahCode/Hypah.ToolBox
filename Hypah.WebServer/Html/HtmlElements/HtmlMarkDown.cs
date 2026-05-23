using Hypah.WebServer.Html.DefaultPageBuilder;
using Hypah.WebServer.Html.HtmlElements.MarkDown;

namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlMarkDown : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => new List<IHtmlBuildable>();

        public string MarkDown { get; }
        private readonly Func<string, string>? _linkCreator;

        public List<Chapter> Chapters { get; } = new List<Chapter>();

        public HtmlMarkDown(string markDown, Func<string, string>? linkCreator = null)
        {
            MarkDown = markDown;
            _linkCreator = linkCreator;
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            cssBuilder.Set("blockqoute").Set("background", DefaultColors.BlockqouteBackground);
            cssBuilder.Set("blockqoute").Set("border-left", "8px solid " + DefaultColors.BlockqouteBorder);
            cssBuilder.Set("blockqoute").Set("display", "block");
            cssBuilder.Set("blockqoute").Set("margin-bottom", "40px");
            cssBuilder.Set("blockqoute").Set("margin-top", "1em");
            cssBuilder.Set("blockqoute").Set("margin-left", "40px");
            cssBuilder.Set("blockqoute").Set("margin-right", "40px");
            cssBuilder.Set("blockqoute").Set("padding", "0.5em 10px");

            cssBuilder.Set("code").Set("background", DefaultColors.BlockqouteBorder);
            cssBuilder.Set("code").Set("padding-top", "0px");
            cssBuilder.Set("code").Set("padding-bottom", "0px");
            cssBuilder.Set("code").Set("padding-left", "6px");
            cssBuilder.Set("code").Set("padding-right", "6px");


            cssBuilder.Set("#markdowntable").Set("border-collapse", "collapse");
            cssBuilder.Set("#markdowntable").Set("width", "100%");

            //cssBuilder.Set("#defaulmarkdowntablettable td, #defaulttable th").Set("border", $"1px solid {DefaultColors.TableBorderColor}");
            cssBuilder.Set("#markdowntable td, #defaulttable th").Set("padding", "4px");

            cssBuilder.Set("#markdowntable tr:nth-child(even)").Set("background-color", DefaultColors.TableEvenBackground);

            cssBuilder.Set("#markdowntable tr:hover").Set("background-color", DefaultColors.TableHoverColor);

            cssBuilder.Set("#markdowntable th").Set("padding-top", "8px");
            cssBuilder.Set("#markdowntable th").Set("padding-bottom", "8px");
            cssBuilder.Set("#markdowntable th").Set("text-align", "left");
            cssBuilder.Set("#markdowntable th").Set("background-color", DefaultColors.TableHeaderColor);
            cssBuilder.Set("#markdowntable th").Set("color", "white");

            var html = MarkDownParser.Parse(MarkDown, _linkCreator, out var chapters);
            Chapters.AddRange(chapters);
            return html;
        }
    }
}
