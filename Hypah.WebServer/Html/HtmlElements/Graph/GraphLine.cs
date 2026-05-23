
namespace Hypah.WebServer.Html.HtmlElements.Graph
{
    public class GraphLine
    {
        public string Color { get; set; }
        public List<GraphLinePoint> Line { get; } = new List<GraphLinePoint>();

        public GraphLine(string color, List<GraphLinePoint> points)
        {
            Color = color;
            Line.AddRange(points);
        }
    }

    public struct GraphLinePoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public GraphLinePoint(float x, float y)
        {
            X = x;
            Y = y;
        }   
    }
}
