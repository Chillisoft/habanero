using System;

namespace Habanero.Base
{
    ///<summary>
    /// An Exception Notifier that executes the provided delegate when notified of an exception.
    ///</summary>
    public class DelegatedExceptionNotifier : IExceptionNotifier
    {
        private readonly NotifyDelegate _notifyDelegate;

        ///<summary>
        /// The delegate type for the Notification of an exception
        ///</summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Any further error messages</param>
        /// <param name="title">The title</param>
        public delegate void NotifyDelegate(Exception ex, string furtherMessage, string title);

        ///<summary>
        /// Create a <see cref="DelegatedExceptionNotifier"/> with the specified exception notification delegate.
        ///</summary>
        ///<param name="notifyDelegate">The <see cref="NotifyDelegate"/> to execute when an exception is recieved by the <see cref="DelegatedExceptionNotifier"/>.</param>
        public DelegatedExceptionNotifier(NotifyDelegate notifyDelegate)
        {
            if (notifyDelegate == null) throw new ArgumentNullException("notifyDelegate");
            _notifyDelegate = notifyDelegate;
        }

        #region Implementation of IExceptionNotifier

        /// <summary>
        /// Notifies the user of an exception that has occurred
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Any further error messages</param>
        /// <param name="title">The title</param>
        public void Notify(Exception ex, string furtherMessage, string title)
        {
            _notifyDelegate(ex, furtherMessage, title);
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