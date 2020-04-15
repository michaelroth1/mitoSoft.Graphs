using System.Text.RegularExpressions;

namespace mitoSoft.Graphs.GraphVizInterop.Extensions
{
    internal static class StringExtensions
    {
        public static string Between(this string text, int startIndex, params char[] end)
        {
            if (startIndex == -1)
            {
                return string.Empty;
            }
            else
            {
                return text.Substring(startIndex).Split(end)[0];
            }
        }

        public static string Between(this string text, string start, params char[] end)
        {
            int startIndex = text.IndexOf(start);
            if (startIndex == -1)
            {
                return string.Empty;
            }
            else
            {
                return Between(text, startIndex + start.Length, end);
            }
        }

        public static string Between(this string text, string start, string end)
        {
            int startIndex = text.IndexOf(start);
            if (startIndex == -1)
            {
                return string.Empty;
            }
            else
            {
                int p1 = startIndex + start.Length;
                int p2 = text.IndexOf(end, p1);

                if (end == "" || p2 == -1)
                {
                    return (text.Substring(p1));
                }
                else
                {
                    return text.Substring(p1, p2 - p1);
                }
            }
        }

        public static string EscapeDotName(this string text)
        {
            text = Regex.Replace(text, "[^a-zA-Z0-9]+", "_");

            text = text.Replace("___", "__");
            text = text.Replace("__", "_");
            text = text.Trim('_').Trim();

            return text;
        }

        public static string EscapeDotLabel(this string text)
        {
            text = Regex.Replace(text, "\"", "'");

            return text;
        }
    }
}