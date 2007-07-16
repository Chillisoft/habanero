namespace Habanero.Util
{
    /// <summary>
    /// Provides a collection of utilities for strings
    /// </summary>
    public class StringUtilities
    {
        /// <summary>
        /// Replaces single quotes with double quotes in the given string
        /// </summary>
        /// <param name="value">The string to amend</param>
        /// <returns>Returns the reformatted string</returns>
        public static string ReplaceSingleQuotesWithTwo(string value)
        {
            return value.Replace("'", "''");
        }

        /// <summary>
        /// Breaks up a Pascal-cased string into sections that are divided
        /// by the given delimiter.  For instance, an input string of
        /// "PascalCase" and a delimiter of " " will give "Pascal Case"
        /// </summary>
        /// <param name="inputString">The string to delimit</param>
        /// <param name="delimiter">The delimiter to insert between
        /// sections, such as a space or comma</param>
        /// <returns>Returns the delimited string</returns>
        public static string DelimitPascalCase(string inputString, string delimiter)
        {
            string formatted = "";
            int counter = 0;
            foreach (char c in inputString)
            {
                string str = c.ToString();
                if (str == str.ToUpper() && counter > 0
                    && str != " "
                    && inputString.Substring(counter-1, 1) != " ")
                {
                    formatted += delimiter + str;
                }
                else
                {
                    formatted += str;
                }
                counter++;
            }
            return formatted;
        }
    }
}