using System.Text.RegularExpressions;

namespace Hypah.Utility.Text
{
    public static class StringExtensions
    {
        public static string Between(this string str, string start, string end)
        {
            return str.Between(start, end, StringComparison.Ordinal);
        }

        public static string Between(this string str, string start, string end, StringComparison comparison)
        {
            int startIndex = str.IndexOf(start, comparison) + start.Length;
            int endIndex = str.IndexOf(end, startIndex, comparison);
            if (startIndex < 0 || endIndex < 0)
                return string.Empty;
            return str.Substring(startIndex, endIndex - startIndex);
        }

        public static string Between(this string str, string start, string end, int startOffset)
        {
            int startIndex = str.IndexOf(start, startOffset, StringComparison.Ordinal) + start.Length;
            int endIndex = str.IndexOf(end, startIndex, StringComparison.Ordinal);
            if (startIndex < 0 || endIndex < 0)
                return string.Empty;
            return str.Substring(startIndex, endIndex - startIndex);
        }

        public static int IndexOfReverse(this string haystack, string needle, int startIndex)
        {
            var revHaystack = new string(haystack.Reverse().ToArray());
            var revNeedle = new string(needle.Reverse().ToArray());
            var revIndex = haystack.Length - startIndex;
            var idx = revHaystack.IndexOf(revNeedle, revIndex);
            if (idx == -1)
            {
                return -1;
            }
            return haystack.Length - idx - needle.Length;
        }

        public static string ToTitleCase(this string title)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(title);
        }

        /// <summary>
        /// Puts first character into upper case
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Capitalize(this string input)
        {
            if (input.Length == 0) return string.Empty;
            if (input.Length == 1) return input.ToUpper();

            return input.Substring(0, 1).ToUpper() + input.Substring(1);
        }

        public static bool IsCapitalized(this string input)
        {
            if (input.Length == 0) return false;
            return string.CompareOrdinal(input.Substring(0, 1), input.Substring(0, 1).ToUpper()) == 0;
        }


        public static bool IsLowerCase(this string input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                if (string.CompareOrdinal(input.Substring(i, 1), input.Substring(i, 1).ToLower()) != 0)
                    return false;
            }
            return true;
        }

        public static bool IsEmpty(this string str) => string.IsNullOrEmpty(str);
        public static bool IsWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static string Left(this string str, int numberOfCharacters)
        {
            if (str.IsEmpty())
                return string.Empty;
            if (str.Length > numberOfCharacters)
                str = str.Substring(0, numberOfCharacters);
            return str;
        }

        public static string Right(this string str, int numberOfCharacters)
        {
            if (str.IsEmpty())
                return string.Empty;
            if (str.Length > numberOfCharacters)
                str = str.Substring(str.Length - numberOfCharacters, numberOfCharacters);
            return str;
        }

        public static string Repeat(this string str, int count)
        {
            var result = "";
            for (int i = 0; i < count; i++)
            {
                result += str;
            }
            return result;
        }

        public static string OnlyDigits(this string value)
        {
            return new string(value?.Where(char.IsDigit).ToArray());
        }

        public static string TitleCaseToSpaced(this string str)
        {
            var regex = new Regex(@"(?:([a-z])([A-Z]))");
            return regex.Replace(str, @"$1 $2");
        }

        public static IEnumerable<string> SplitTrim(this string str, string separator, params char[] trimChars)
        {
            return str
                .Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(sub => sub.Trim(trimChars));
        }
    }
}
