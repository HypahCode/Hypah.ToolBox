
namespace Hypah.WebServer.Html.DefaultPageBuilder
{
    public static class DefaultColors
    {
        public static string AccentColor { get; private set; } = "#bf432a";

        public static string Background { get; set; } = "#000000";
        public static string ContentBackground { get; set; } = "#222222";
        public static string Text { get; set; } = "#FFFFFF";
        public static string Header { get; set; } = "#EEEEEE";
        public static string H1 => "#EEEEEE";
        public static string H2 => H1;
        public static string H3 => H1;
        public static string H4 => H1;
        
        public static string Link => AccentColor;
        public static string LinkVisited => AccentColor;
        public static string LinkHoover => Text;
        public static string LinkActive => Text;

        // Menu
        public static string MenuHoverColor => AccentColor;

        // Table
        public static string TableHeaderColor => AccentColor;
        public static string TableEvenBackground { get; set; } = "#333333";
        public static string TableBorderColor { get; set; } = "#555555";
        public static string TableHoverColor { get; set; } = "#555555";

        // Block qoute
        public static string BlockqouteBackground { get; set; } = "#333333";
        public static string BlockqouteBorder { get; set; } = "#555555";
        public static void SetAccentColor(string color)
        {
            AccentColor = color;
        }

        public static string Lighten(string hexColor, double amount)
        {
            // amount = 0.0 to 1.0 (e.g. 0.2 = 20% lighter)

            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(1);

            int r = int.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            r = (int)(r + (255 - r) * amount);
            g = (int)(g + (255 - g) * amount);
            b = (int)(b + (255 - b) * amount);

            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
