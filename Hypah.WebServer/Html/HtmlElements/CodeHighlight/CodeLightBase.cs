namespace Hypah.WebServer.Html.HtmlElements.CodeHighlight
{
    internal class CodeLightBase
    {
        protected readonly string _operatorColor = "#f5cb9f";
        protected readonly string _keywordColor = "#6695ed";
        protected readonly string _typeColor = "#f55bbc";
        protected readonly string _hlslFuncColor = "#00ccff";
        protected readonly string _commentColor = "#00e68e";
        protected readonly string _includeColor = "#ff6666";
        protected readonly string _attributeColor = "#e0ed66";
        protected readonly char[] _seperatorWordChars = [' ', '\t', '.', '(', ')', '=', '+', '-', '*', '&', '|', '{', '}', ';', '<', '>'];


        protected string ReplaceSpecialCharsWithHtmlChars(string l)
        {
            l = l.Replace("&", "&amp;");
            l = l.Replace("<", "&lt;");
            l = l.Replace(">", "&gt;");
            return l;
        }

        protected string HighlightReplace(string line, string keywordColor, params string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                line = line.Replace(keyword, $"<font color={keywordColor}>{keyword}</font>");
            }
            return line;
        }

        protected string Highlight(string line, string keywordColor, params string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                var index = 0;
                while (index != -1)
                {
                    index = line.IndexOf(keyword, index);
                    if (index == -1)
                    {
                        break;
                    }

                    if (index > 0)
                    {
                        if (!_seperatorWordChars.Contains(line[index - 1]))
                        {
                            index += keyword.Length;
                            continue;
                        }
                    }
                    if (index + keyword.Length < line.Length)
                    {
                        if (!_seperatorWordChars.Contains(line[index + keyword.Length]))
                        {
                            index += keyword.Length;
                            continue;
                        }
                    }

                    var l1 = line.Substring(0, index);
                    var l2 = line.Substring(index + keyword.Length);
                    var replacement = $"<font color={keywordColor}>{keyword}</font>";
                    line = l1 + replacement + l2;

                    index += replacement.Length;
                }
            }
            return line;
        }

    }
}
