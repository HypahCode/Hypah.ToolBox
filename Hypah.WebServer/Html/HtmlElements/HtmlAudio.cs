namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlAudio : IHtmlBuildable
    {
        public string AudioSource { get; set; }

        public List<IHtmlBuildable> Builders => throw new NotImplementedException();

        public HtmlAudio(string audioSource)
        {
            AudioSource = audioSource;
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            return "<audio controls src=\"{AudioSource}\"></audio>";
        }
    }
}
