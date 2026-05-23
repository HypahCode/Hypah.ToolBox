using System.Text;

namespace Hypah.WebServer.Html
{
    public class CssBuilder : ICssBuilder
    {
        public Dictionary<string, ICssProperties> Selectors { get; } = new Dictionary<string, ICssProperties>();

        public ICssProperties Set(string selector)
        {
            if (Selectors.TryGetValue(selector, out var properties))
            {
                return properties;
            }
            properties = new CssProperties();
            Selectors[selector] = properties;
            return properties;
        }

        public string ToCssString()
        {
            var builder = new StringBuilder();
            foreach (var selector in Selectors)
            {
                builder.Append(selector.Key);
                builder.AppendLine(" {");
                var properties = selector.Value.Properties.Select(x => $"  {x.Key} : {x.Value}");
                builder.Append(string.Join(";\n", properties)).AppendLine(";");
                builder.AppendLine("}");
            }

            return builder.ToString();
        }
    }
}
