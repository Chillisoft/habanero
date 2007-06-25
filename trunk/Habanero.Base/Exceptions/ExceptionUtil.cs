using System;

namespace Habanero.Base.Exceptions
{
    /// <summary>
    /// Provides extra utilities for dealing with exceptions, such as
    /// a method to format an exception string
    /// </summary>
    public class ExceptionUtil
    {
        /// <summary>
        /// This is deprecated - rather use GetExceptionString(Exception,int,bool).
        /// Returns an exception string formatted to display the exception
        /// message, stack trace and inner exception, each on different lines,
        /// at a specific indentation
        /// </summary>
        /// <param name="ex">The exception to display</param>
        /// <param name="indent">The indentation as a number of spaces</param>
        /// <returns>Returns a string</returns>
        /// TODO ERIC - consider removing
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
        /// This is deprecated - rather use GetExceptionString(Exception,int,bool).
        ///  </summary>
        /// <param name="ex">The exception to display</param>
        /// <param name="indent">The indentation as a number of spaces</param>
        /// <returns>Returns a string</returns>
        /// TODO ERIC - remove
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
                    str + "INNER EXCEPTION:" + Environment.NewLine + GetCategorizedExceptionString(ex.InnerException, indent + 8);
            }
            return str;
        }

        /// <summary>
        /// This is deprecated - rather use GetExceptionString(Exception,int,bool).
        /// </summary>
        /// <param name="ex">The exception to display</param>
        /// <param name="indent">The indentation as a number of spaces</param>
        /// <returns>Returns a string</returns>
        /// TODO ERIC - remove
        public static string GetSummarisedExceptionString(Exception ex, int indent)
        {
            string str = "";
            str += GetIndent(indent);
            str += "FINAL EXCEPTION:";
            str += Environment.NewLine + GetIndent(indent);
            str += ex.GetType() + ": " + ex.Message;
            str += Environment.NewLine + Environment.NewLine + GetIndent(indent);
            str += "INNER EXCEPTION:";
            str += Environment.NewLine + GetIndent(indent);
            str += ex.InnerException.GetType() + ": " + ex.InnerException.Message;
            if (ex.InnerException != null)
            {
                str += Environment.NewLine + Environment.NewLine;
                str += GetIndent(indent + 8);
                str += "BASE EXCEPTION:";
                str += Environment.NewLine + GetIndent(indent + 8);
                str += ex.GetBaseException().GetType() + ": " + ex.GetBaseException().Message;
                //return
                //    str + "INNER EXCEPTION:" + Environment.NewLine + GetSummarisedExceptionString(ex.InnerException, indent + 8);
            }
            return str;
        }

        /// <summary>
        /// Arranges the exception into a readable string to be shown to the
        /// final user
        /// </summary>
        /// <param name="ex">The exception to display</param>
        /// <param name="indent">The amount of indentation, used by the
        /// application to indent inner exceptions, so you can set this to zero</param>
        /// <param name="showStackTrace">False will display just the error
        /// messages and true will add the stack trace</param>
        /// <returns>Returns a string</returns>
        public static string GetExceptionString(Exception ex, int indent, bool showStackTrace)
        {
            string str = "";
            str += GetIndent(indent);
            str += ex.GetType() + ": " + ex.Message;
            if (showStackTrace)
            {
                str += Environment.NewLine + GetIndent(indent);
                str += ex.StackTrace;
            }
            if (ex.InnerException != null)
            {
                str += Environment.NewLine + Environment.NewLine;
                str += GetIndent(indent + 8);
                return
                    str + "INNER EXCEPTION:" + Environment.NewLine + 
                    GetExceptionString(ex.InnerException, indent + 8, showStackTrace);
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