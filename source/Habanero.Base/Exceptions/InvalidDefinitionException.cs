using System;
using System.Runtime.Serialization;

namespace Habanero.Base.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when the xml being
    /// examined is invalid or not well-formed
    /// </summary>
    [Serializable]
    public class InvalidDefinitionException : HabaneroDeveloperException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public InvalidDefinitionException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public InvalidDefinitionException(string message) : base(message, message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public InvalidDefinitionException(string message, Exception inner) : base(message, message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected InvalidDefinitionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}