using System;
using System.Runtime.Serialization;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Provides an exception to throw when an invalid object ID is
    /// encountered
    /// </summary>
    [Serializable()]
    public class InvalidObjectIdException : BaseApplicationException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public InvalidObjectIdException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public InvalidObjectIdException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public InvalidObjectIdException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected InvalidObjectIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}