// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
//using System.Linq;
using Habanero.Base.Exceptions;
using Habanero.Base.Util;
using Habanero.Util;

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
            if (!HasExceptions) return;
            ExceptionDetail exceptionDetail = Exceptions[Exceptions.Count -1];

            throw new RecordedExceptionsException(string.Format(
                    "An Exception that was recorded by the RecordingExceptionNotifier and has been rethrown." + 
                    "{0}Title: {1}{0}Further Message: {2}",
                    EnvironmentCF.NewLine, exceptionDetail.Title, this.ExceptionMessage),
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
        /// All the Exception messages for all logged Exceptions Concatenated.
        ///</summary>
        public string ExceptionMessage
        {
            get
            {
                string message = "";
                foreach (var exceptionDetail in Exceptions)
                {
                    message = StringUtilities.AppendMessage(message, exceptionDetail.ToString(), EnvironmentCF.NewLine);
                }
                return message;
            }
        }
        /// <summary>
        /// Returns true if there are any Exceptons Recorded
        /// </summary>
        public bool HasExceptions
        {
            get { return _exceptions.Count > 0; }
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
            /// <summary>
            /// The Exception beign wrapped message plus any FurtherMessage.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.Exception.Message + " - " + FurtherMessage;
            }
        }
    }
}