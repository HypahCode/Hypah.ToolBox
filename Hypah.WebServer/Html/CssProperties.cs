
namespace Hypah.WebServer.Html
{
    public class CssProperties : ICssProperties
    {
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public ICssProperties Set(string property, string value)
        {
            Properties[property] = value;
            return this;
        }
    }
}
