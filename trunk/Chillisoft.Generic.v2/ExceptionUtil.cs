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
            for (int i = 0; i < indent; i++)
            {
                str += " ";
            }
            str += "Exception: " + ex.ToString() + Environment.NewLine;
            for (int i = 0; i < indent; i++)
            {
                str += " ";
            }
            str += "Stacktrace: " + ex.StackTrace;
            if (ex.InnerException != null)
            {
                str += Environment.NewLine;
                for (int i = 0; i < indent + 8; i++)
                {
                    str += " ";
                }
                return
                    str + "Inner Exception:" + Environment.NewLine + GetExceptionString(ex.InnerException, indent + 8);
            }
            return str;
        }
    }
}