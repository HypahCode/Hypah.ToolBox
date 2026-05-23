namespace Hypah.WebServer.Html.HtmlElements.Cards
{
    public class HtmlDivGrid : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders { get; } = new List<IHtmlBuildable>();
        public string? CardBorderColor { get; set; }
        public string? CardBackgroundColor { get; set; }

        public void Add(IHtmlBuildable card)
        {
            Builders.Add(card);
        }

        public void Add(HtmlRoundedRectangleCard card)
        {
            if (!string.IsNullOrEmpty(CardBorderColor) && string.IsNullOrEmpty(card.BorderColor))
                card.BorderColor = CardBorderColor;
            if (!string.IsNullOrEmpty(CardBackgroundColor) && string.IsNullOrEmpty(card.BackgroundColor))
                card.BackgroundColor = CardBackgroundColor;
            Builders.Add(card);
        }        

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            cssBuilder.Set(".div-grid")
                .Set("display", "flex")
                .Set("gap", "20px")
                .Set("flex-wrap", "wrap")
                .Set("margin-top", "20px");

            var grid = new HtmlDiv() { Class = "div-grid" };

            foreach (var card in Builders)
            {
                grid.Add(card.BuildHtml(cssBuilder));
            }

            return grid.BuildHtml(cssBuilder);
        }
    }
}
