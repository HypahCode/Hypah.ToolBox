using Hypah.WebServer.Attributes;
using Hypah.WebServer.Html.DefaultPageBuilder;
using System.Reflection;

namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlA : HtmlBuilder
    {
        public string Href { get; }

        public HtmlA(string href)
        {
            Href = href;
        }

        public HtmlA(string href, string text)
        {
            Href = href;
            Add(new HtmlText(text));
        }

        public HtmlA(string text, Type webPage, object urlQuery = null)
        {
            Href = webPage.GetCustomAttribute<WebPageAttribute>()!.Path;
            if (urlQuery != null)
            {
                var query = EncodeObjectIntoUrl(urlQuery);
                Href += "?" + query;
            }
            Add(new HtmlText(text));
        }

        public HtmlA(string text, IWebPage page, object urlQuery = null)
            : this(text, page.GetType(), urlQuery)
        { }

        protected override string HtmlTag => "a";
        protected override void FillProperties()
        {
            base.FillProperties();
            Properties["href"] = Href;
        }

        public override string BuildHtml(ICssBuilder cssBuilder)
        {
            cssBuilder.Set("a:link").Set("color", DefaultColors.Link);
            cssBuilder.Set("a:visited").Set("color", DefaultColors.LinkVisited);
            cssBuilder.Set("a:hover").Set("color", DefaultColors.LinkHoover);
            cssBuilder.Set("a:active").Set("color", DefaultColors.LinkActive);

            return base.BuildHtml(cssBuilder);
        }

        private string EncodeObjectIntoUrl(object urlQuery)
        {
            var keyValues = new List<string>();
            foreach (var prop in urlQuery.GetType().GetProperties())
            {
                var value = prop.GetValue(urlQuery);
                if (value != null)
                {
                    keyValues.Add($"{prop.Name}={value}");
                }
            }
            return string.Join("&", keyValues);
        }
    }
}
