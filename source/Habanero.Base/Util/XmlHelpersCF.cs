using System.Text;
using System.Xml;

namespace Habanero.Base.Util
{
    public static class XmlHelpersCF
    {
        public static string GetAttributeOrDefault(XmlReader reader, string attributeName, string defaultValue)
        {
            var attribute = reader.GetAttribute(attributeName) ?? defaultValue;
            return attribute;
        }

        private static readonly char[] EscapeChars = new [] { '<', '>', '"', '\'', '&' };
        private static readonly string[] EscapeStringPairs = new [] { "<", "&lt;", ">", "&gt;", "\"", "&quot;", "'", "&apos;", "&", "&amp;" };

        /// <summary>
        /// Escapes the specified text.
        /// </summary>
        /// <param name="str">The text to escape.</param>
        /// <returns>An escaped string.</returns>
        public static string Escape(string str)
        {
            if (str == null)
            {
                return null;
            }
            StringBuilder builder = null;
            int length = str.Length;
            int startIndex = 0;
            while (true)
            {
                int num2 = str.IndexOfAny(EscapeChars, startIndex);
                if (num2 == -1)
                {
                    if (builder == null)
                    {
                        return str;
                    }
                    builder.Append(str, startIndex, length - startIndex);
                    return builder.ToString();
                }
                if (builder == null)
                {
                    builder = new StringBuilder();
                }
                builder.Append(str, startIndex, num2 - startIndex);
                builder.Append(GetEscapeSequence(str[num2]));
                startIndex = num2 + 1;
            }
        }

        private static string GetEscapeSequence(char c)
        {
            int length = EscapeStringPairs.Length;
            for (int i = 0; i < length; i += 2)
            {
                string str = EscapeStringPairs[i];
                string str2 = EscapeStringPairs[i + 1];
                if (str[0] == c)
                {
                    return str2;
                }
            }
            return c.ToString();
        }
    }
}
