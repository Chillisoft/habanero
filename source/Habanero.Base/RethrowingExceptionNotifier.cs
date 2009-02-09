using System;

namespace Habanero.Base
{
    ///<summary>
    /// An Exception Notifier that just rethrows the error.
    /// Used for testing.
    ///</summary>
    public class RethrowingExceptionNotifier : IExceptionNotifier
    {
        #region Implementation of IExceptionNotifier

        /// <summary>
        /// Notifies the user of an exception that has occurred
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Any further error messages</param>
        /// <param name="title">The title</param>
        public void Notify(Exception ex, string furtherMessage, string title)
        {
            throw new Exception(furtherMessage, ex);
        }

        ///<summary>
        /// The last exception logged by the exception notifier
        ///</summary>
        public string ExceptionMessage
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}