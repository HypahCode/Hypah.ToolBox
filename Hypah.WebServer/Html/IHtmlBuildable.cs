
namespace Hypah.WebServer.Html
{
    public interface IHtmlBuildable
    {
        List<IHtmlBuildable> Builders { get; }
        string BuildHtml(ICssBuilder cssBuilder);
    }
}
