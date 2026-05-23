
namespace Hypah.WebServer.Html.HtmlElements.FormAttributes
{
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }
        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
