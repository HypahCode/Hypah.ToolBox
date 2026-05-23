
namespace Hypah.WebServer.Html.HtmlElements
{
    public abstract class HtmlBuilder : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders { get; } = new List<IHtmlBuildable>();

        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();


        public string? Id { get; init; }
        public string? Class { get; set; }

        protected abstract string HtmlTag { get; }

        public virtual string BuildHtml(ICssBuilder cssBuilder) 
        {
            FillProperties();
            return BuildWithTag(cssBuilder);
        }

        protected virtual void FillProperties()
        {
            if (!string.IsNullOrEmpty(Id)) Properties["id"] = Id;
            if (!string.IsNullOrEmpty(Class)) Properties["class"] = Class;
        }

        public HtmlBuilder Add(string text)
        {
            Builders.Add(new HtmlText(text));
            return this;
        }


        public HtmlBuilder Add(HtmlBuilder builder)
        {
            Builders.Add(builder);
            return builder;
        }

        public void Add(IHtmlBuildable builder)
        {
            Builders.Add(builder);
        }

        public string BuildPropertiesString()
        {
            return string.Join(" ", Properties.Select(x => $"{x.Key}=\"{x.Value}\""));
        }

        protected string BuildWithTag(ICssBuilder cssBuilder)
        {
            var html = $@"<{HtmlTag}";
            var propertiesString = BuildPropertiesString();
            if (!string.IsNullOrEmpty(propertiesString)) html += " " + propertiesString;
            html += ">";
            html += BuildParentBuilders(cssBuilder);
            html += $"</{HtmlTag}>";
            return html;
        }

        protected string BuildParentBuilders(ICssBuilder cssBuilder)
        {
            var html = "";
            foreach (var item in Builders)
            {
                html += item.BuildHtml(cssBuilder);
            }
            return html;
        }
    }
}
