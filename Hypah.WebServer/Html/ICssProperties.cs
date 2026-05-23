
namespace Hypah.WebServer.Html
{
    public interface ICssProperties
    {
        Dictionary<string, string> Properties { get; }
        ICssProperties Set(string property, string value);
    }
}
