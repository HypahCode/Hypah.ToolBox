namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlCanvas : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => throw new NotSupportedException();

        public string Id { get; }
        public int Width { get; }
        public int Height { get; }

        public HtmlCanvas(string id, int width, int height)
        {
            Id = id;
            Width = width;
            Height = height;
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            return $"<canvas id=\"{Id}\" width=\"{Width}\" height=\"{Height}\">no canvas</canvas>";
        }
    }
}
