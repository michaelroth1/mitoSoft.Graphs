using System.Text.RegularExpressions;

namespace mitoSoft.Graphs.GraphVizInterop.Extensions
{
    internal static class StringExtensions
    {
        public static string Between(this string text, string start, string end)
        {
            if (text.IndexOf(start) == -1)
            {
                return string.Empty;
            }
            else
            {
                int p1 = text.IndexOf(start) + start.Length;
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

        public static string RemoveIllegalDotCharacters(this string text)
        {
            text = Regex.Replace(text, "[^a-zA-Z0-9]+", "_");

            text = text.Replace("___", "__");
            text = text.Replace("__", "_");
            text = text.Trim('_').Trim();

            return text;
        }
    }
}