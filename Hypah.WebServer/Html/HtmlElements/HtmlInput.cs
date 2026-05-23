namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlInput : HtmlBuilder
    {
        protected override string HtmlTag => "Input";

        public HtmlInput(string type, string id, string value)
        {
            Properties["type"] = type;
            Properties["value"] = value;
            Properties["id"] = id;
            Properties["name"] = id;
        }

        public static HtmlInput SubmitButton(string text)
        {
            return new HtmlInput("submit", "submit", text);
        }

        public static HtmlInput Button(string id, string text)
        {
            return new HtmlInput("button", id, text);
        }

        public static HtmlInput Text(string id, string value)
        {
            return new HtmlInput("text", id, value);
        }

        internal static IHtmlBuildable Hidden(string id, string value)
        {
            return new HtmlInput("hidden", id, value);
        }

        internal static IHtmlBuildable Number(string id, string value, string steps = "any", string min = "", string max = "")
        {
            var number = new HtmlInput("number", id, value);
            number.Properties["step"] = steps;
            if (!string.IsNullOrEmpty(min)) number.Properties["min"] = min;
            if (!string.IsNullOrEmpty(max)) number.Properties["max"] = max;
            return number;
        }

        internal static IHtmlBuildable CheckBox(string id, bool value)
        {
            var checkbox = new HtmlInput("checkbox", id, "true");
            if (value)
            {
                checkbox.Properties["checked"] = "true";
            }
            return checkbox;
        }
    }
}
