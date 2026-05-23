

namespace Hypah.WebServer.Html.HtmlElements.MarkDown
{
    internal static class StringUtils
    {
        public static bool CharIs(string line, int index, char c)
        {
            if (index < 0 || index > line.Length - 1) return false;
            return line[index] == c;
        }

        public static bool AllCharsAre(string line, char c)
        {
            if (line.Length == 0) return false;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != c) return false;
            }
            return true;
        }

        public static bool IsEmptyString(string line)
        {
            return line.Trim().Length == 0;
        }

        public static bool CountSameSequential(string line, int startIndex, int count, char needle, bool forward = true)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                var index = forward ? i : startIndex - i;
                if (index >= line.Length && index < 0)
                    return false;

                if (line[index] != needle)
                    return false;
            }
            return true;
        }

        public static string? BetweenConnected(string inString, string start, string end)
        {
            int idx1 = inString.IndexOf(start);
            int idx2 = inString.IndexOf(end);
            if ((idx1 != -1) && (idx2 != -1) && (idx1 < idx2))
            {
                if (IsSpaceOrTerminator(inString, idx1 - 1)) return null; 
                if (!IsSpaceOrTerminator(inString, idx1 + start.Length)) return null; 

                if (!IsSpaceOrTerminator(inString, idx2 - 1)) return null;
                if (!IsSpaceOrTerminator(inString, idx2 + end.Length)) return null;

                int startIdx = idx1 + start.Length;
                return inString.Substring(startIdx, idx2 - startIdx);
            }
            return null;
        }

        public static string Between(string inString, string start, string end)
        {
            int idx1 = inString.IndexOf(start);
            if (idx1 + start.Length >= inString.Length) return "";
            int idx2 = inString.IndexOf(end, idx1 + start.Length);
            if ((idx1 != -1) && (idx2 != -1) && (idx1 < idx2))
            {
                int startIdx = idx1 + start.Length;
                return inString.Substring(startIdx, idx2 - startIdx);
            }
            return "";
        }

        public static string Between(string inString, string start, string end, int startIndex)
        {
            int idx1 = inString.IndexOf(start, startIndex);
            if (idx1 + start.Length >= inString.Length) return "";
            int idx2 = inString.IndexOf(end, startIndex + idx1 + start.Length);
            if ((idx1 != -1) && (idx2 != -1) && (idx1 < idx2))
            {
                int startIdx = idx1 + start.Length;
                return inString.Substring(startIdx, idx2 - startIdx);
            }
            return "";
        }

        public static bool IsSpaceOrTerminator(string line, int index)
        {
            if (index < 0) return true;
            if (index >= line.Length) return true;
            return line[index] == ' ' || line[index] == '\n' || line[index] == '.';
        }

        public static string ReplaceFirst(string line, string needle, string replacement)
        {
            var idx = line.IndexOf(needle);
            if (idx == -1) return line;
            return line.Substring(0, idx) + replacement + line.Substring(idx + needle.Length);
        }

        public static List<string> GetLines(List<string> lines, int startIndex, Func<string, bool> filter)
        {
            var result = new List<string>();
            var index = startIndex;
            while (index < lines.Count && filter(lines[index]))
            {
                result.Add(lines[index]);
                index++;
            }
            return result;
        }

        internal static void Trim4Spaces(List<string> strings)
        {
            for (int i = 0; i < strings.Count; i++)
            {
                if (strings[i].StartsWith("    "))
                {
                    strings[i] = strings[i].Substring(4);
                }
            }
        }

        internal static bool StartsWithNumberAndPoint(string line, out string num)
        {
            num = "";
            var idx = line.IndexOf(". ");
            if (idx == -1) return false;

            char[] nums = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
            for (int i = 0; i < idx; i++)
            {
               if (!CharInArray(line[i], nums))
                {
                    return false;
                }
            }
            num = line.Substring(0, idx + 2);
            return true;
        }

        internal static bool CharInArray(char c, char[] array)
        {
            foreach (var a in array)
            {
                if (c == a)
                    return true;
            }
            return false;
        }
    }
}
