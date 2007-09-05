using System;
using System.Windows.Forms;
using Habanero.Base;

namespace Habanero.Base.Exceptions
{
    /// <summary>
    /// Provides utilities to display or manipulate exceptions
    /// </summary>
    public class ExceptionUtilities
    {
        /// <summary>
        /// Displays a given exception in either a MessageBox (if it is a
        /// UserException) or the UIExceptionNotifier as specified at the
        /// start of the application
        /// </summary>
        /// <param name="ex">The exception to display</param>
        public static void Display(Exception ex)
        {
            if (ex is UserException)
            {
                MessageBox.Show(ex.Message);
            }
            else
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "Error occurred", "Error");
            }
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