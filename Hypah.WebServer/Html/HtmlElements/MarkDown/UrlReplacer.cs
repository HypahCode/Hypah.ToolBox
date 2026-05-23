using System.Text.RegularExpressions;

namespace Hypah.WebServer.Html.HtmlElements.MarkDown
{
    internal static class UrlReplacer
    {
        private static readonly Dictionary<string, (string Url, string? Title)> ReferenceDefinitions = new();
        private static readonly Regex InlineLinkRegex = new(@"\[([^\]]+)\]\(([^)\s]+)(?:\s+""([^""]*)"")?\)", RegexOptions.Compiled);
        private static readonly Regex ReferenceDefinitionRegex = new(@"^\s*\[([^\]]+)\]\s*:\s*(\S+)(?:\s+""([^""]*)"")?\s*$", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex ReferenceLinkRegex = new(@"\[([^\]]+)\]\[([^\]]+)\]", RegexOptions.Compiled);
        private static readonly Regex RawUrlRegex = new(@"(https?://[^\s<]+)", RegexOptions.Compiled);
        private static readonly Regex InternalLinkRegex = new(@"\[\[([^\]]+)\]\]", RegexOptions.Compiled);
        private static readonly Regex ImageRegex = new(@"!\[([^\]]*)\]\(([^)\s]+)(?:\s+""([^""]*)"")?\)", RegexOptions.Compiled);

        public static string ReplaceUrl(string text, Func<string, string>? linkCreator = null)
        {
            ReferenceDefinitions.Clear();

            text = ParseReferenceDefinitions(text);
            text = ReplaceImages(text, linkCreator);
            text = ReplaceInternalLinks(text, linkCreator);
            text = ReplaceInlineLinks(text);
            text = ReplaceReferenceLinks(text);
            text = ReplaceRawUrls(text);

            return text;
        }

        private static string ReplaceImages(string text, Func<string, string>? linkCreator)
        {
            return ImageRegex.Replace(text, match =>
            {
                var altText = match.Groups[1].Value;
                var url = match.Groups[2].Value;
                var title = match.Groups[3].Success ? match.Groups[3].Value : null;

                if (linkCreator != null)
                {
                    var imgUrl = linkCreator(url);
                    if (!string.IsNullOrEmpty(title))
                    {
                        // TODO create this VIA link creator!!
                        return $"<img src=\"{imgUrl}\" alt=\"{altText}\" title=\"{title}\">";
                    }
                    return $"<img src=\"{imgUrl}\" alt=\"{altText}\">";
                }
                return text;
            });
        }

        private static string ReplaceInternalLinks(string text, Func<string, string>? linkCreator)
        {
            return InternalLinkRegex.Replace(text, match =>
            {
                var pageName = match.Groups[1].Value;

                if (linkCreator != null)
                {
                    return linkCreator(pageName);
                }

                return match.Value;
            });
        }

        private static string ParseReferenceDefinitions(string text)
        {
            var matches = ReferenceDefinitionRegex.Matches(text);
            foreach (Match match in matches)
            {
                var id = match.Groups[1].Value;
                var url = match.Groups[2].Value;
                var title = match.Groups[3].Success ? match.Groups[3].Value : null;

                if (!ReferenceDefinitions.ContainsKey(id))
                {
                    ReferenceDefinitions[id] = (url, title);
                }
            }

            text = ReferenceDefinitionRegex.Replace(text, "");
            return text;
        }

        private static string ReplaceInlineLinks(string text)
        {
            return InlineLinkRegex.Replace(text, match =>
            {
                var linkText = match.Groups[1].Value;
                var url = match.Groups[2].Value;
                var title = match.Groups[3].Success ? match.Groups[3].Value : null;

                return CreateAnchorTag(url, linkText, title);
            });
        }

        private static string ReplaceReferenceLinks(string text)
        {
            return ReferenceLinkRegex.Replace(text, match =>
            {
                var linkText = match.Groups[1].Value;
                var refId = match.Groups[2].Value;

                if (ReferenceDefinitions.TryGetValue(refId, out var def))
                {
                    return CreateAnchorTag(def.Url, linkText, def.Title);
                }

                return match.Value;
            });
        }

        private static string ReplaceRawUrls(string text)
        {
            return RawUrlRegex.Replace(text, match =>
            {
                var url = match.Value;
                return CreateAnchorTag(url, url, null);
            });
        }

        private static string CreateAnchorTag(string url, string text, string? title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                return $"<a href=\"{url}\" title=\"{title}\">{text}</a>";
            }
            return $"<a href=\"{url}\">{text}</a>";
        }
    }
}
