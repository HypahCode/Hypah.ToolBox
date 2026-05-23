
namespace Hypah.WebServer.Attributes
{
    public class WebPageAttribute : Attribute
    {
        public WebPageAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; }
        public bool IsVisibleInMenu { get; set; } = true;
    }
}
