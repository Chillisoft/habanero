using System;
using System.Runtime.Serialization;
using Habanero.Base.Exceptions;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when a business object is not found
    /// </summary>
    [Serializable]
    public abstract class BusinessObjectException : HabaneroApplicationException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        protected BusinessObjectException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        protected BusinessObjectException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        protected BusinessObjectException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusinessObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}