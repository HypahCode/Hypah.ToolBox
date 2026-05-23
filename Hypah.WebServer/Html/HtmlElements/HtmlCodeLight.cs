using Hypah.WebServer.Html.HtmlElements.CodeHighlight.Hypah.DebugServer.Web.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hypah.WebServer.Html.HtmlElements
{
    internal class HtmlCodeLight : IHtmlBuildable
    {
        public List<IHtmlBuildable> Builders => new List<IHtmlBuildable>();

        public string Code { get; }

        public HtmlCodeLight(string code)
        {
            Code = code;
        }

        public string BuildHtml(ICssBuilder cssBuilder)
        {
            return CodeLightShader.ToHtml(Code);
        }
    }
}
