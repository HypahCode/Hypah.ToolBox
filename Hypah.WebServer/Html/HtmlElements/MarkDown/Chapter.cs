namespace Hypah.WebServer.Html.HtmlElements.MarkDown
{
    public class Chapter
    {
        public string Anchor { get; }

        public string Name { get; }
        public int Size { get; }

        public Chapter(string anchor, string name, int size)
        {
            Anchor = anchor;
            Name = name;
            Size = size;
        }
    }
}
