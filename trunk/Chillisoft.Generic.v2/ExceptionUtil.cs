using System;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Provides extra utilities for dealing with exceptions, such as
    /// a method to format an exception string
    /// </summary>
    public class ExceptionUtil
    {
        /// <summary>
        /// Returns an exception string formatted to display the exception
        /// message, stack trace and inner exception, each on different lines,
        /// at a specific indentation
        /// </summary>
        /// <param name="ex">The exception to display</param>
        /// <param name="indent">The indentation as a number of spaces</param>
        /// <returns>Returns a string</returns>
        public static string GetExceptionString(Exception ex, int indent)
        {
            string str = "";
            str += GetIndent(indent);
            str += "Exception: " + ex.ToString() + Environment.NewLine;
            str += GetIndent(indent);
            str += "Stacktrace: " + ex.StackTrace;
            if (ex.InnerException != null)
            {
                str += Environment.NewLine;
                str += GetIndent(indent+8);
                return
                    str + "Inner Exception:" + Environment.NewLine + GetExceptionString(ex.InnerException, indent + 8);
            }
            return str;
        }

        /// <summary>
        /// Temporary solution to above's ilegibility
        /// </summary>
        /// <param name="ex">The exception to display</param>
        /// <param name="indent">The indentation as a number of spaces</param>
        /// <returns>Returns a string</returns>
        public static string GetCategorizedExceptionString(Exception ex, int indent)
        {
            string str = "";
            str += GetIndent(indent);
            str += "FINAL EXCEPTION:";
            str += Environment.NewLine + GetIndent(indent);
            str += ex.GetType() + ": " + ex.Message;
            str += Environment.NewLine + Environment.NewLine + GetIndent(indent);
            str += "BASE EXCEPTION:";
            str += Environment.NewLine + GetIndent(indent);
            str += ex.GetBaseException();
            if (ex.InnerException != null)
            {
                str += Environment.NewLine + Environment.NewLine;
                str += GetIndent(indent+8);
                return
                    str + "INNER EXCEPTION:" + Environment.NewLine + GetExceptionString(ex.InnerException, indent + 8);
            }
            return str;
        }

        /// <summary>
        /// Creates a string of spaces to serve as an indentation for the text
        /// </summary>
        /// <param name="numberOfSpaces">The number of spaces to indent by</param>
        /// <returns>Returns a string</returns>
        private static string GetIndent(int numberOfSpaces)
        {
            string indentString = "";
            for (int i = 0; i < numberOfSpaces; i++)
            {
                indentString += " ";
            }
            return indentString;
        }
    }
}