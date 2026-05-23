using Hypah.WebServer.Html.HtmlElements.CodeHighlight.Hypah.DebugServer.Web.Builders;

namespace Hypah.WebServer.Html.HtmlElements.CodeHighlight
{
    internal static class CodeLightBuilder
    {
        public static string Light(string code, string syntax)
        {
            switch (syntax.ToLower())
            {
                case "shader":
                case "hlsl":
                    return CodeLightShader.ToHtml(code);

                case "c#":
                case "cs":
                case "csharp":
                case "c++":
                    return CodeLightCSharp.ToHtml(code);
            }
            return CodeLightDefault.ToHtml(code);
        }
    }
}
