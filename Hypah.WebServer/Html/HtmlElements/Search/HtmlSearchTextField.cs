namespace Hypah.WebServer.Html.HtmlElements.Search
{
    public class HtmlSearchTextField : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => throw new NotImplementedException();

        public string Action { get; set; }
        public string Background { get; set; } = "rgba(0, 0, 0, 0.25)";
        public string BackgroundFocus { get; set; } = "rgba(0,0,0,0.35)";
        public string PlaceholderTextColor { get; set; } = "rgba(255, 255, 255, 0.50)";
        public string SearchIcon { get; set; } = "<svg viewBox='0 0 24 24' width='16' height='16'><path fill='currentColor' d='M15.5 14h-.79l-.28-.27A6.471 6.471 0 0016 9.5 6.5 6.5 0 109.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z'/></svg>";

        public HtmlSearchTextField(string action)
        {
            Action = action;
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            var form = new HtmlForm(Action) {  Class = "searchbar" };
            var input = form.Add(new HtmlInput("search", "Query", ""));
            input.Properties.Add("placeholder", "search...");
            form.Add(new HtmlButton(SearchIcon, "submit"));
            AddCss(cssBuilder);
            return form.BuildHtml(cssBuilder);
        }

        private void AddCss(ICssBuilder cssBuilder)
        {
            cssBuilder.Set(".searchbar")
                .Set("position", "relative")
                .Set("display", "inline-flex")
                .Set("align-items", "center");

            cssBuilder.Set(".searchbar input[type=\"search\"]")
                .Set("padding", "6px 34px 6px 12px")
                .Set("border-radius", "6px")
                .Set("border", "none")
                .Set("outline", "none")
                .Set("background", Background)
                .Set("color", "white")
                .Set("font-size", "14px")
                .Set("width", "150px")
                .Set("transition", "width 0.25s ease, background 0.25s ease");

            cssBuilder.Set(".searchbar input[type=\"search\"]::placeholder")
                .Set("color", PlaceholderTextColor);

            cssBuilder.Set(".searchbar input[type=\"search\"]:focus")
                .Set("width", "220px")
                .Set("background", BackgroundFocus);

            cssBuilder.Set(".searchbar button")
                .Set("position", "absolute")
                .Set("right", "6px")
                .Set("border", "none")
                .Set("background", "none")
                .Set("color", "white")
                .Set("cursor", "pointer")
                .Set("font-size", "16px")
                .Set("opacity", "0.8")
                .Set("padding", "0");

            cssBuilder.Set(".searchbar button:hover")
                .Set("opacity", "1");
        }
    }
}
