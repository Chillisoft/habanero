using System;
using System.Runtime.Serialization;


namespace Habanero.Bo
{
    /// <summary>
    /// Provides an exception to throw when a relationship is accessed
    /// in an invalid way.  This usually occurs when a multiple relationship
    /// was expected and a single one was specified, or vice versa.
    /// </summary>
    [Serializable]
    public class InvalidRelationshipAccessException : Exception
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public InvalidRelationshipAccessException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public InvalidRelationshipAccessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public InvalidRelationshipAccessException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected InvalidRelationshipAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}