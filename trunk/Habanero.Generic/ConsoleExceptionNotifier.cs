using System;


namespace Habanero.Base
{
    /// <summary>
    /// Displays exception message output to the console
    /// </summary>
    public class ConsoleExceptionNotifier : IExceptionNotifier
    {
        /// <summary>
        /// Constructor to initialise a new notifier
        /// </summary>
        public ConsoleExceptionNotifier()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Notifies the user of an exception that has occurred, by adding
        /// the error text to the console
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Any further error messages</param>
        /// <param name="title">The title</param>
        public void Notify(Exception ex, string furtherMessage, string title)
        {
            Console.Out.WriteLine("Error: " + furtherMessage);
            Console.Out.WriteLine("Further details: " + ExceptionUtil.GetExceptionString(ex, 0));
        }
    }
}