
namespace Hypah.WebServer.Html
{
    public interface ICssBuilder
    {
        Dictionary<string, ICssProperties> Selectors { get; } 
        ICssProperties Set(string selector);
        string ToCssString();
    }
}
