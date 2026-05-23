namespace Hypah.WebServer.Html.HtmlElements.CanvasImages
{
    public struct PixelValue
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public PixelValue(byte r, byte g, byte b, byte a = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public PixelValue(byte grayScale)
            : this(grayScale, grayScale, grayScale, 255)
        { }

        public override string ToString()
        {
            return $"{{\"r\": {r}, \"g\": {g}, \"b\": {b}, \"a\": {a}}}";
        }
    }
}
