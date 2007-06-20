

using System;

namespace Habanero.Generic
{
    /// <summary>
    /// An interface to model a tool that notifies the user of an exception
    /// </summary>
    public interface IExceptionNotifier
    {
        /// <summary>
        /// Notifies the user of an exception that has occurred
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Any further error messages</param>
        /// <param name="title">The title</param>
        void Notify(Exception ex, string furtherMessage, string title);
    }
}