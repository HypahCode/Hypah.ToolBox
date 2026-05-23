using Hypah.WebServer.Html.HtmlElements.CodeHighlight;
using System.Text;

namespace Hypah.WebServer.Html.HtmlElements.MarkDown
{
    internal class MarkDownParser
    {
        private const char Space = ' ';
        private const string FourSpaces = "    ";

        private List<string> MarkDown { get; } = new List<string>();

        private int CurrentIndex = 0;
        private StringBuilder Html { get; } = new StringBuilder();
        private Func<string, string>? _linkCreator;

        public List<Chapter> Chapters { get; } = new List<Chapter>();

        internal static string Parse(string markDown, Func<string, string>? linkCreator = null)
        {
            var parser = new MarkDownParser(markDown, linkCreator);
            return parser.Html.ToString();
        }

        internal static string Parse(string markDown, Func<string, string>? linkCreator, out List<Chapter> chapters)
        {
            var parser = new MarkDownParser(markDown, linkCreator);
            chapters = parser.Chapters;
            return parser.Html.ToString();
        }


        internal static string Parse(List<string> markDown)
        {
            var parser = new MarkDownParser(markDown);
            return parser.Html.ToString();
        }

        private MarkDownParser(string markDown, Func<string, string>? linkCreator = null)
        {
            _linkCreator = linkCreator;
            MarkDown = new List<string>(markDown.Replace("\r", "").Replace("\t", "    ").Split('\n'));
            while (CurrentIndex < MarkDown.Count)
            {
                ProcessLine(CurrentIndex);
                CurrentIndex++;
            }
        }

        private MarkDownParser(List<string> markDown)
        {
            MarkDown = new List<string>(markDown);
            while (CurrentIndex < MarkDown.Count)
            {
                ProcessLine(CurrentIndex);
                CurrentIndex++;
            }
        }

        private void ProcessLine(int lineIndex)
        {
            // Block qoutes
            if (IsBlockqoute(out var blockqouteLines))
            {
                var blockqouteHtml = Parse(blockqouteLines);
                AppendHtmlLine("blockqoute", blockqouteHtml!);
                CurrentIndex += blockqouteLines.Count - 1;
                return;
            }
            if (IsList())
            {
                return; // Parsing is done internally
            }
            if (IsHeading(out var hSize, out var headingText))
            {
                var anchor = $"anchor_{Chapters.Count}";
                AppendHtmlLineWithAnchor($"h{hSize}", anchor, FormatTextLine(headingText));
                Chapters.Add(new Chapter(anchor, headingText, hSize));
                return;
            }
            if (IsHorizontalRule())
            {
                Html.AppendLine("<hr>");
                return;
            }
            if (IsCodeBlock(out var code, out var syntax))
            {
                var highlightedCode = CodeLightBuilder.Light(code, syntax);
                AppendHtmlLine("blockqoute", highlightedCode);
                return;
            }
            if (IsTable())
            {
                return;
            }

            var line = GetLine(lineIndex);
            Html.AppendLine(FormatTextLine(line) + "<br>");
        }

        private bool IsTable()
        {
            var line = GetLine(CurrentIndex);
            if (!line.StartsWith("|")) return false;
            
            var tableLines = StringUtils.GetLines(MarkDown, CurrentIndex, x => x.StartsWith("|"));
            if (tableLines.Count < 2) return false;

            Html.AppendLine("<table id=\"markdowntable\">\n");

            var alignmentsStr = tableLines[1].Trim().Split('|', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
            var alignments = new List<string>();
            foreach (var a in alignmentsStr)
            {
                if (a.StartsWith(":") && a.EndsWith(":")) alignments.Add("c");
                else if (a.EndsWith(":")) alignments.Add("r");
                else alignments.Add("l");
            }

            var headers = tableLines[0].Trim().Split('|', StringSplitOptions.RemoveEmptyEntries).Select(x => FormatTextLine(x.Trim())).ToArray();
            Html.AppendLine("<tr>");
            var alIdx = 0;
            foreach (var header in headers)
            {
                Html.AppendLine($"<th {AlignmentStyle(alignments[alIdx++])}>{FormatTextLine(header)}</th>");
            }
            Html.AppendLine("</tr>");

            for (int i = 2; i < tableLines.Count; i++)
            {
                try
                {
                    var columns = tableLines[i].Trim().Split('|', StringSplitOptions.RemoveEmptyEntries).Select(x => FormatTextLine(x.Trim())).ToArray();
                    Html.AppendLine("<tr>");
                    alIdx = 0;
                    foreach (var column in columns)
                    {
                        Html.AppendLine($"<td {AlignmentStyle(alignments[alIdx++])}>{FormatTextLine(column)}</td>");
                    }
                    Html.AppendLine("</tr>");
                }
                catch (Exception)
                {
                }
            }
            Html.AppendLine("</table>");
            
            CurrentIndex += tableLines.Count - 1;
            return true;
        }

        private string AlignmentStyle(string a)
        {
            if (a == "c") return "style=\"text-align: center\"";
            if (a == "r") return "style=\"text-align: right\"";
            return "style=\"text-align: left\"";
        }

        private bool IsCodeBlock(out string code, out string syntax)
        {
            code = "";
            syntax = "";
            var line = GetLine(CurrentIndex);
            if (!line.TrimEnd().StartsWith("```")) return false;
            syntax = line.Substring(3).Trim();
            CurrentIndex++;
            while (CurrentIndex < MarkDown.Count)
            {
                line = GetLine(CurrentIndex);
                if (line.TrimEnd() == "```")
                {
                    return true;
                }
                code += GetLine(CurrentIndex) + "\n";
                CurrentIndex++;
            }
            return true;
        }

        private bool IsList()
        {
            var line = GetLine(CurrentIndex);
            if (IsTaskList(line))
            {
                ParseTaskList();
                return true;
            }
            if (line.StartsWith("* ") || line.EndsWith("- ") || line.EndsWith("+ "))
            {
                var bullets = ParseList(line.Substring(0, 2));
                Html.AppendLine("<ul>");
                foreach (var bullet in bullets)
                {
                    Html.AppendLine($"<li>{bullet}</li>");
                }
                Html.AppendLine("</ul>");
                return true;
            }
            else if (line.StartsWith("1. "))
            {
                var bullets = ParseList("1.");
                Html.AppendLine("<ol>");
                foreach (var bullet in bullets)
                {
                    Html.AppendLine($"<li>{bullet}</li>");
                }
                Html.AppendLine("</ol>");
                return true;
            }
            return false;
        }

        private bool IsTaskList(string line)
        {
            var trimmed = line.TrimStart();
            return trimmed.StartsWith("- [") || trimmed.StartsWith("* [") || trimmed.StartsWith("+ [");
        }

        private void ParseTaskList()
        {
            Html.AppendLine("<ul class=\"task-list\">");
            while (CurrentIndex < MarkDown.Count)
            {
                var line = GetLine(CurrentIndex);
                if (!IsTaskList(line))
                {
                    break;
                }

                var trimmed = line.TrimStart();
                var isChecked = trimmed.StartsWith("- [x]") || trimmed.StartsWith("- [X]") ||
                                trimmed.StartsWith("* [x]") || trimmed.StartsWith("* [X]") ||
                                trimmed.StartsWith("+ [x]") || trimmed.StartsWith("+ [X]");

                var textStart = trimmed.IndexOf(']') + 2;
                var taskText = trimmed.Substring(textStart).Trim();

                var checkbox = isChecked ? " checked" : "";
                Html.AppendLine($"<li><input type=\"checkbox\" disabled{checkbox}> {FormatTextLine(taskText)}</li>");

                CurrentIndex++;
            }
            Html.AppendLine("</ul>");
            CurrentIndex--;
        }

        private List<string> ParseList(string bulletChar)
        {
            var bulletPoints = new List<string>();
            var listItem = "";
            for (; CurrentIndex < MarkDown.Count; CurrentIndex++)
            {
                var line = GetLine(CurrentIndex);
                if (StringUtils.StartsWithNumberAndPoint(line, out var num) && bulletChar == "1.") // Ordered list 
                {
                    if (!string.IsNullOrEmpty(listItem.Trim('\n')))
                    {
                        bulletPoints.Add(Parse(listItem));
                    }
                    listItem = line.Substring(num.Length);
                    continue;
                }
                if (line.StartsWith(bulletChar)) // Unordered list
                {
                    if (!string.IsNullOrEmpty(listItem.Trim('\n')))
                    {
                        bulletPoints.Add(Parse(listItem));
                    }
                    listItem = line.Substring(2);
                    continue;
                }
                if (string.IsNullOrEmpty(line.Trim()))
                {
                    listItem += "\n";
                    continue;
                }
                if (line.StartsWith(FourSpaces))
                {
                    var indentLines = StringUtils.GetLines(MarkDown, CurrentIndex, x => x.StartsWith(FourSpaces) || string.IsNullOrWhiteSpace(x));
                    StringUtils.Trim4Spaces(indentLines);
                    listItem += "\n" + string.Join('\n', indentLines);
                    CurrentIndex += indentLines.Count - 1;
                    continue;
                }

                CurrentIndex--;
                bulletPoints.Add(Parse(listItem));
                return bulletPoints;
            }
            if (!string.IsNullOrEmpty(listItem.Trim('\n')))
            {
                bulletPoints.Add(Parse(listItem));
            }
            return bulletPoints;
        }

        private bool IsBlockqoute(out List<string>? blockqouteLines)
        {
            blockqouteLines = null;
            var line = GetLine(CurrentIndex);
            if (!line.Trim().StartsWith(">")) return false;
            blockqouteLines = StringUtils.GetLines(MarkDown, CurrentIndex, (x) => x.Trim().StartsWith(">"));
            for (int i = 0; i < blockqouteLines.Count; i++)
            {
                blockqouteLines[i] = blockqouteLines[i].Trim().Substring(1);
            }
            return true;
        }

        private string FormatTextLine(string line)
        {
            var text = line;

            text = EscapeCharacters(text);
            text = FormatUrl(text);

            text = Replace2(text,          "***", "***", "<em><strong>", "</strong></em>");
            text = Replace2Connected(text, "_**", "**_", "<em><strong>", "</strong></em>");
            text = Replace2Connected(text, "__*", "*__", "<em><strong>", "</strong></em>");
            text = Replace2Connected(text, "___", "___", "<em><strong>", "</strong></em>");
            text = Replace2(text,          "*__", "__*", "<em><strong>", "</strong></em>");
            text = Replace2(text,          "**_", "_**", "<em><strong>", "</strong></em>");

            text = Replace2(text,          "**", "**", "<strong>", "</strong>");
            text = Replace2Connected(text, "_*", "*_", "<strong>", "</strong>");
            text = Replace2Connected(text, "__", "__", "<strong>", "</strong>");
            text = Replace2(text,          "*_", "_*", "<strong>", "</strong>");

            text = Replace2(text,          "*", "*", "<em>", "</em>");
            text = Replace2Connected(text, "_", "_", "<em>", "</em>");

            text = Replace2(text, "~~", "~~", "<s>", "</s>");
            text = Replace2(text, "~", "~", "<sub>", "</sub>");
            text = Replace2(text, "^", "^", "<sup>", "</sup>");
            text = Replace2(text, "==", "==", "<mark>", "</mark>");

            text = Replace2(text, "``", "``", "<code>", "</code>");

            text = text.Replace("\n", "<br>");
            return text;
        }

        private static readonly Dictionary<char, char> EscapedChars = new()
        {
            { '\\', '\\' },
            { '`', '`' },
            { '*', '*' },
            { '_', '_' },
            { '{', '{' },
            { '}', '}' },
            { '[', '[' },
            { ']', ']' },
            { '(', '(' },
            { ')', ')' },
            { '#', '#' },
            { '+', '+' },
            { '-', '-' },
            { '.', '.' },
            { '!', '!' },
            { '>', '>' },
            { '~', '~' },
            { '^', '^' },
            { '=', '=' },
        };

        private string EscapeCharacters(string text)
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\\' && i + 1 < text.Length && EscapedChars.ContainsKey(text[i + 1]))
                {
                    result.Append(text[i + 1]);
                    i++;
                }
                else
                {
                    result.Append(text[i]);
                }
            }
            return result.ToString();
        }

        private string FormatUrl(string text)
        {
            return UrlReplacer.ReplaceUrl(text, _linkCreator);
        }

        private string Replace2(string line, string start, string end, string startReplacement, string endReplacement)
        {
            var text = line;
            while (!string.IsNullOrEmpty(StringUtils.Between(text, start, end)))
            {
                text = StringUtils.ReplaceFirst(text, start, startReplacement);
                text = StringUtils.ReplaceFirst(text, end, endReplacement);
            }
            return text;
        }

        private string Replace2Connected(string line, string start, string end, string startReplacement, string endReplacement)
        {
            var text = line;
            while (!string.IsNullOrEmpty(StringUtils.BetweenConnected(text, start, end)))
            {
                text = StringUtils.ReplaceFirst(text, start, startReplacement);
                text = StringUtils.ReplaceFirst(text, end, endReplacement);
            }
            return text;
        }

        private bool IsHeading(out int size, out string heading)
        {
            var line = GetLine(CurrentIndex);

            if (line.StartsWith("#"))
            {
                for (int i = 6; i > 0; i--)
                {
                    if (StringUtils.CountSameSequential(line, 0, i, '#') && (StringUtils.CharIs(line, i, Space)))
                    {
                        size = i;
                        heading = line.Substring(i);
                        return true;
                    }
                }
            }

            var nextLine = GetLine(CurrentIndex + 1);
            if (StringUtils.AllCharsAre(nextLine, '-') && line.Length >= 3)
            {
                size = 2;
                heading = line;
                CurrentIndex++;
                return true;
            } 
            if (StringUtils.AllCharsAre(nextLine, '=') && line.Length >= 3)
            {
                size = 1;
                heading = line;
                CurrentIndex++;
                return true;
            } 

            size = -1;
            heading = line;
            return false;
        }

        private bool IsHorizontalRule()
        {
            var line = GetLine(CurrentIndex).Trim();
            if (line.Length >= 3)
            {
                if (line.All(c => c == '-') || line.All(c => c == '*') || line.All(c => c == '_'))
                {
                    return true;
                }
            }
            return false;
        }

        private void AppendHtmlLine(string tag, string text)
        {
            Html.AppendLine($"<{tag}>{text}</{tag}>");
        }

        private void AppendHtmlLineWithAnchor(string tag, string anchor, string text)
        {
            Html.AppendLine($"<{tag} id=\"{anchor}\">{text}</{tag}>");
        }

        private void AppendHtml(string tag, string text)
        {
            Html.Append($"<{tag}>{text}</{tag}>");
        }

        private string GetLine(int index)
        {
            if ((index < 0) || (index > MarkDown.Count - 1)) return "";
            return MarkDown[index];
        }




    }
}