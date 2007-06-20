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
    }
}