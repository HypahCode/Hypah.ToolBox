using Hypah.WebServer.Html.DefaultPageBuilder;

namespace Hypah.WebServer.Html.HtmlElements.Cards
{
    public class HtmlRoundedRectangleCard : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => throw new NotImplementedException();

        public IHtmlBuildable Title { get; set; }
        public List<IHtmlBuildable> Values { get; } = new List<IHtmlBuildable>();

        public string? BorderColor { get; set; }
        public string? BackgroundColor { get; set; }

        public HtmlRoundedRectangleCard(IHtmlBuildable title, params IHtmlBuildable[] values)
        {
            Title = title;
            Values.AddRange(values);
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            cssBuilder.Set(".rounded-card")
                .Set("padding", "15px 20px")
                .Set("border-radius", "8px")
                .Set("min-width", "200px")
                .Set("color", "#eee")
                .Set("box-shadow", "0 0 8px rgba(0,0,0,0.3)");
            if (!string.IsNullOrEmpty(BorderColor))
            {
                cssBuilder.Set(".rounded-card").Set("border", $"2px solid {BorderColor}");
            }
            if (!string.IsNullOrEmpty(BackgroundColor))
            {
                cssBuilder.Set(".rounded-card").Set("background", BackgroundColor);
            }
            
            cssBuilder.Set(".rounded-title")
                .Set("font-size", "16px")
                .Set("font-weight", "bold")
                .Set("margin-bottom", "8px");

            cssBuilder.Set(".rounded-value")
                .Set("font-size", "14px")
                .Set("margin-bottom", "4px");


            var memCard = new HtmlDiv() { Class = "rounded-card" };
            memCard.Add(new HtmlDiv() { Class = "rounded-title" }).Add(Title);
            foreach (var value in Values)
            {
                memCard.Add(new HtmlDiv() { Class = "rounded-value" }).Add(value);
            }

            return memCard.BuildHtml(cssBuilder);
        }
    }
}
