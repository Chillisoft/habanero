using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    ///<summary>
    /// An Exception Notifier that records all exceptions it has been notified of.
    ///</summary>
    public class RecordingExceptionNotifier : IExceptionNotifier
    {
        private readonly List<ExceptionDetail> _exceptions = new List<ExceptionDetail>();

        ///<summary>
        /// The details of the Exceptions that this <see cref="RecordingExceptionNotifier"/> has been notified of.
        ///</summary>
        public IList<ExceptionDetail> Exceptions
        {
            get { return _exceptions; }
        }

        ///<summary>
        /// Rethrows the first recorded exception on the recorded exception stack if it exists.
        ///</summary>
        public void RethrowRecordedException()
        {
            if (Exceptions.Count == 0) return;
            ExceptionDetail exceptionDetail = Exceptions[0];
            throw new Exception(string.Format(
                    "An Exception that was recorded by the RecordingExceptionNotifier and has been rethrown." + 
                    "{0}Title: {1}{0}Further Message: {2}",
                    Environment.NewLine, exceptionDetail.Title, exceptionDetail.FurtherMessage),
                    exceptionDetail.Exception);
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
            _exceptions.Add(new ExceptionDetail(ex, furtherMessage, title));
        }

        ///<summary>
        /// The last exception logged by the exception notifier
        ///</summary>
        public string ExceptionMessage
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        ///<summary>
        /// The details of an Exception that this <see cref="RecordingExceptionNotifier"/> has been notified of.
        ///</summary>
        public class ExceptionDetail
        {
            ///<summary>
            /// Creates an <see cref="ExceptionDetail"/> with the specified information.
            ///</summary>
            /// <param name="exception">The exception</param>
            /// <param name="furtherMessage">Any further error messages</param>
            /// <param name="title">The title</param>
            public ExceptionDetail(Exception exception, string furtherMessage, string title)
            {
                Exception = exception;
                FurtherMessage = furtherMessage;
                Title = title;
            }

            ///<summary>
            /// The Exception
            ///</summary>
            public Exception Exception { get; private set; }

            ///<summary>
            /// Any further messages for the exception
            ///</summary>
            public string FurtherMessage { get; private set; }

            ///<summary>
            /// The title for the notification
            ///</summary>
            public string Title { get; private set; }
        }
    }
}