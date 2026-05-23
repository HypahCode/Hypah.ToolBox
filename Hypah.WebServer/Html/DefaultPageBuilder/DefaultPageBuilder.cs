using Hypah.WebServer.Attributes;
using Hypah.WebServer.Html.HtmlElements;
using Hypah.WebServer.Request;
using System.Reflection;

namespace Hypah.WebServer.Html.DefaultPageBuilder
{
    internal class DefaultPageBuilder : IPageBuilder
    {
        private readonly WebUrl _url;
        private readonly IPageManager _pageManager;

        private string HeaderHeight = "52px";
        private string MenuWidth = "175px";

        public DefaultPageBuilder(WebUrl url, IPageManager pageManager)
        {
            _url = url;
            _pageManager = pageManager;
        }

        public string Build(IWebPage webpage)
        {
            var cssBuilder = new CssBuilder();
            var content = webpage.BuildContent(_url, cssBuilder);
            var pageContent = BuildPageFrame(_url, content, cssBuilder);
            return DefaultHtml.BuildHtmlPage($"{DefaultContentText.ApplicationName} - {webpage.Name}", cssBuilder.ToCssString(), pageContent);
        }

        private string BuildPageFrame(WebUrl url, IHtmlBuildable content, ICssBuilder cssBuilder)
        {
            cssBuilder.Set("body").Set("font-family", "\"Lato\", sans-serif");
            cssBuilder.Set("html, body").Set("margin", "0");
            cssBuilder.Set("html, body").Set("padding", "0");

            cssBuilder.Set("#contentWrapper").Set("margin", "0 auto");

            cssBuilder.Set("#menumainbar").Set("background", DefaultColors.ContentBackground);
            cssBuilder.Set("#menumainbar").Set("top", "0");
            cssBuilder.Set("#menumainbar").Set("width", "100%");
            cssBuilder.Set("#menumainbar").Set("min-height", "600px");

            var contentWrapper = new HtmlDiv() { Id = "contentWrapper" };
            var mainBar = new HtmlDiv() { Id = "menumainbar" };

            contentWrapper.Add(CreateHeader(url, cssBuilder));
            contentWrapper.Add(mainBar);
            contentWrapper.Add(CreateFooter(cssBuilder));
            mainBar.Add(CreateMenu(cssBuilder));

            // Body
            var mainContent = new HtmlDiv() { Id = "main-content", Class = "main-content" };
            mainContent.Add(CreateMainContent(cssBuilder, content));
            mainBar.Add(mainContent);


            return contentWrapper.BuildHtml(cssBuilder);
        }

        private IHtmlBuildable CreateHeader(WebUrl url, ICssBuilder cssBuilder)
        {
            var headerOffset = new HtmlDiv() { Id = "headerOffset" };
            cssBuilder.Set("#headerOffset").Set("height", HeaderHeight)
                .Set("width", "100%");

            var headerbar = new HtmlDiv() { Id = "headerbar" };
            cssBuilder.Set("#headerbar").Set("height", HeaderHeight)
                .Set("position", "fixed")
                .Set("left", "0")
                .Set("top", "0")
                .Set("padding-left", "16px")
                .Set("padding-right", "16px")
                .Set("width", "100%");
            var baseColor = DefaultColors.MenuHoverColor;
            var lighter = DefaultColors.Lighten(baseColor, 0.2);
            var darker = DefaultColors.Lighten(baseColor, -0.2);
            cssBuilder.Set("#headerbar").Set("background", $"linear-gradient(to bottom, {lighter}, {darker})")
                .Set("box-shadow", "0 2px 10px rgba(0,0,0,0.6)")
                .Set("display", "flex")
                .Set("align-items", "center")
                .Set("flex-wrap", "nowrap");

            cssBuilder.Set("#headercontent").Set("display", "flex")
                .Set("align-items", "center");

            cssBuilder.Set("#headercontent h2").Set("margin", "0")
                .Set("font-size", "20px")
                .Set("font-weight", "700")
                .Set("letter-spacing", "0.5px")
                .Set("text-shadow", "0 1px 2px rgba(0,0,0,0.4)");

            var headerContent = headerbar.Add(new HtmlDiv() { Id = "headercontent" });

            if (!string.IsNullOrEmpty(DefaultContentText.ApplicationIcon))
            {
                cssBuilder.Set("#headercontent img").Set("margin-right", "12px");
                headerContent.Add(new HtmlImg(DefaultContentText.ApplicationIcon));
            }
            
            headerContent.Add(new HtmlH(H.H2)).Add(new HtmlText(DefaultContentText.ApplicationName));

            if (DefaultHeaderRightSideContainer.RightSideContainer != null)
            {
                headerbar.Add(DefaultHeaderRightSideContainer.RightSideContainer.BuildContent(url, cssBuilder));
            }

            headerOffset.Add(headerbar);
            return headerOffset;
        }

        private IHtmlBuildable CreateFooter(ICssBuilder cssBuilder)
        {

            cssBuilder.Set("#footerbar").Set("text-align", "center");
            cssBuilder.Set("#footerbar").Set("padding", "16px 0");
            cssBuilder.Set("#footerbar").Set("background", "#111");
            cssBuilder.Set("#footerbar").Set("border-top", "1px solid #333");
            cssBuilder.Set("#footerbar").Set("color", "#888");
            cssBuilder.Set("#footerbar").Set("font-size", "14px");



            var footer = new HtmlDiv() { Id = "footerbar" };
            footer.Add(new HtmlBr());
            footer.Add(new HtmlText(DefaultContentText.FooterText));
            footer.Add(new HtmlBr());
            footer.Add(new HtmlBr());
            return footer;
        }

        private IHtmlBuildable CreateMenu(ICssBuilder cssBuilder)
        {
            cssBuilder.Set(".vertical-menu").Set("width", MenuWidth);
            cssBuilder.Set(".vertical-menu").Set("position", "fixed");
            cssBuilder.Set(".vertical-menu a").Set("color", DefaultColors.MenuHoverColor);
            cssBuilder.Set(".vertical-menu a").Set("display", "block");
            cssBuilder.Set(".vertical-menu a").Set("padding", "12px");
            cssBuilder.Set(".vertical-menu a").Set("text-decoration", "none");
            cssBuilder.Set(".vertical-menu a").Set("font-weight", "bold");
            cssBuilder.Set(".vertical-menu a:hover").Set("background-color", DefaultColors.MenuHoverColor);
            cssBuilder.Set(".vertical-menu a:hover").Set("color", "white");

            var verticalMenu = new HtmlDiv() { Class = "vertical-menu" };
            verticalMenu.Add(new HtmlBr());
            verticalMenu.Add(new HtmlBr());
            foreach (var page in _pageManager.Pages)
            {
                var webPageAttribute = page.Value.GetType().GetCustomAttribute<WebPageAttribute>();
                if (webPageAttribute?.IsVisibleInMenu ?? true)
                {
                    verticalMenu.Add(new HtmlA(page.Key)).Add(new HtmlText(page.Value.Name));
                }
            }

            return verticalMenu;
        }

        private IHtmlBuildable CreateMainContent(ICssBuilder cssBuilder, IHtmlBuildable content)
        {
            cssBuilder.Set("#main-content").Set("padding", "12px");
            cssBuilder.Set("#main-content").Set("margin-left", MenuWidth);

            var div = new HtmlDiv() { Class = "main-content" };
            div.Add(content);
            return div;
        }
    }
}