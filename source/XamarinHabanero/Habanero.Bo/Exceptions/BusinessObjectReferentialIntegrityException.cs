using System;
using System.Runtime.Serialization;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when a the referential integrity constraints of 
    /// a business object are being violated
    /// </summary>
    [Serializable]
    public class BusinessObjectReferentialIntegrityException : BusinessObjectException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>

        public BusinessObjectReferentialIntegrityException()

        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusinessObjectReferentialIntegrityException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusinessObjectReferentialIntegrityException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusinessObjectReferentialIntegrityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}