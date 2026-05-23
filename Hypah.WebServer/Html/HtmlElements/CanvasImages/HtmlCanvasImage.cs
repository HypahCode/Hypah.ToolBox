namespace Hypah.WebServer.Html.HtmlElements.CanvasImages
{
    public class HtmlCanvasImage : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => throw new NotImplementedException();

        public int Width { get; }
        public int Height { get; }
        public string Name { get; set; }

        public readonly PixelValue[,] _values;

        public HtmlCanvasImage(string name, PixelValue[,] values)
        {
            Width = values.GetLength(0);
            Height = values.GetLength(1);
            Name = name;
            _values = values;
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            var canvas = new HtmlCanvas(Name, Width, Height);
            var script = new HtmlScript();
            script.Add(GetScript());
            return canvas.BuildHtml(cssBuilder) + script.BuildHtml(cssBuilder);
        }

        private string GetScript()
        {
            var script = CanvasImageJs.script;

            var jsonData = new List<string>();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    jsonData.Add(_values[x, y].ToString());
                }
            }
            var jsonArrayData = string.Join("\n, ", jsonData);

            script = script.Replace("GRAPH_CANVAS", Name);
            script = script.Replace("[[ARRAYDATA]]", jsonArrayData);
            return script;
        }
    }
}
