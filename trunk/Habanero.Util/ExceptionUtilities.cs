using System;
using System.Windows.Forms;
using Habanero.Generic;


namespace Habanero.Util
{
    /// <summary>
    /// Provides utilities to display or manipulate exceptions
    /// </summary>
    public class ExceptionUtilities
    {
//		public static string GetExceptionString(Exception ex, int indent) {
//			string str = "";
//			for (int i = 0; i < indent; i++) {
//				str += " ";
//			}
//			str += "Exception: " + ex.ToString() + Environment.NewLine;
//			for (int i = 0; i < indent; i++) {
//				str += " ";
//			}
//			str += "Stacktrace: " + ex.StackTrace;
//			if (ex.InnerException != null) {
//				str += Environment.NewLine;
//				for (int i = 0; i < indent + 8; i++) {
//					str += " ";
//				}
//				return str + "Inner Exception:" + Environment.NewLine + GetExceptionString(ex.InnerException, indent + 8);
//			}
//			return str;
//		}

        /// <summary>
        /// Displays a given exception
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
    }
}