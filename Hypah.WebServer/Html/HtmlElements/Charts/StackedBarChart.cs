namespace Hypah.WebServer.Html.HtmlElements.Charts
{
    public class StackedBarChart : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => throw new NotImplementedException();

        public List<Segment> Bars { get; } = new List<Segment>();
        public string Width { get; set; } = "300px";
        public string Height { get; set; } = " 24px";
       
        public StackedBarChart(params Segment[] bars)
        {
            Bars.AddRange(bars);
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            var div = new HtmlDiv() { Class = "stacked-bar" };
            var total = Bars.Sum(x => x.Value);
            if (total <= 0) total = 1.0f;
            foreach (var bar in Bars)
            {
                var segment = new HtmlDiv() { Class= "stacked-bar-segment" };
                segment.Properties.Add("style", $"width: {(bar.Value / total) * 100f}%; background:{bar.Color};");
                if (!string.IsNullOrEmpty(bar.Text))
                {
                    segment.Add(new HtmlText(bar.Text));
                }
                div.Add(segment);
            }

            cssBuilder.Set(".stacked-bar")
                .Set("display", "flex")
                .Set("width", Width)
                .Set("height", Height)
                .Set("border-radius", "4px")
                .Set("overflow", " hidden")
                .Set("background", "#222")
                .Set("font-size", "11px")
                .Set("color", "#fff");

            cssBuilder.Set(".stacked-bar-segment")
                .Set("display", "flex")
                .Set("align-items", "center")
                .Set("justify-content", "center")
                .Set("white-space", "nowrap");

            return div.BuildHtml(cssBuilder);
        }

        public class Segment
        {
            public float Value { get; set; }
            public string Color { get; set; }
            public string? Text { get; set; }

            public Segment(float value, string color, string? text = null)
            {
                Value = value;
                Color = color;
                Text = text;
            }
        }
    }
}
