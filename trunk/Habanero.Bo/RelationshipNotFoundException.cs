using System;
using System.Runtime.Serialization;


namespace Habanero.Bo
{
    /// <summary>
    /// An exception to throw if a specified relationship was not found
    /// </summary>
    [Serializable]
    public class RelationshipNotFoundException : Exception
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public RelationshipNotFoundException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public RelationshipNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public RelationshipNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected RelationshipNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}