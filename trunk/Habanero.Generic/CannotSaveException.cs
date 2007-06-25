using System;
using System.Runtime.Serialization;


namespace Habanero.Base
{
    /// <summary>
    /// Provides an exception to throw when the application was unable
    /// to save data
    /// </summary>
    public class CannotSaveException : Exception
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public CannotSaveException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public CannotSaveException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public CannotSaveException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected CannotSaveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}