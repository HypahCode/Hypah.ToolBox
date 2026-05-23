using System.Text;

namespace Hypah.WebServer.Html.HtmlElements.Graph
{
    public class HtmlLineGraph : IHtmlBuildable
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public List<IHtmlBuildable> Builders => new List<IHtmlBuildable>();

        public List<GraphLine> GraphLines { get; } = new List<GraphLine>();
        public string Name { get; }

        public HtmlLineGraph(string name, params GraphLine[] lines)
            : this(name, 100, 100, lines)
        {
        }

        public HtmlLineGraph(string name, int width, int height, params GraphLine[] lines)
        {
            Name = name;
            Width = width;
            Height = height;
            GraphLines.AddRange(lines);
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            var sb = new StringBuilder();
            foreach (var line in GraphLines)
            {
                sb.AppendLine("array.push(\r\n");
                sb.AppendLine(CreateArrayData(Name, 0, 1, line));
                sb.AppendLine(");");
                sb.AppendLine("");
            }
            return CreateGraphCanvas(Name, sb.ToString());
        }

        private string CreateGraphCanvas(string name, string data)
        {
            string canvasName = "canvasJS_" + name.Replace(' ', '_');

            string html = "<canvas id=\"" + canvasName + "\" width=\"" + Width + "\" height=\"" + Height + "\">no canvas</canvas>";
            html += "<script>" + LineGraph.CreateScript("#000000") + "</script>";
            html = html.Replace("GRAPH_CANVAS", canvasName);
            html = html.Replace("[[ARRAYDATA]]", data);
            return html;
        }

        private static string GetLineColors(IEnumerable<GraphLine> graphLines)
        {
            return string.Join(", ", graphLines.Select(x => $"\"{x.Color}\""));
        }

        private static string CreateArrayData(string name, float min, float max, GraphLine line)
        {
            var json = "{" + Environment.NewLine;
            json += "\"name\": \"" + name + "\"," + Environment.NewLine;
            json += "\"color\": \"" + line.Color + "\"," + Environment.NewLine;
            json += "\"data\": [" + Environment.NewLine + CreateJSONArrayData(line.Line) + "]}";
            return json;
        }

        private static string CreateJSONArrayData(List<GraphLinePoint> data)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                sb.Append("{ \"x\": " + data[i].X.ToString() + ", \"y\": " + data[i].Y.ToString() + " }");
                if (i != data.Count - 1)
                    sb.Append("," + Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
