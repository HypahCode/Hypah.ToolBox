namespace Hypah.WebServer.Html.HtmlElements.CodeHighlight
{
    internal class CodeLightDefault : CodeLightBase
    {
        private string _code;

        private CodeLightDefault(string code, IEnumerable<int>? highlightedLines = null)
        {
            _code = code;
        }

        public static string ToHtml(string code, IEnumerable<int>? highlightedLines = null)
        {
            CodeLightDefault builder = new(code, highlightedLines);
            code = builder.ReplaceSpecialCharsWithHtmlChars(code);
            return "<pre>" + code + "</pre>";
        }
    }
}
