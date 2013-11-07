using System;
using System.Runtime.Serialization;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when a there is an issue writing to a property on 
    /// the businessobject due to the ReadWriteRule that has been set up for the property.
    /// </summary>
    [Serializable]
    public class BOPropReadException : BusinessObjectException
    {

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BOPropReadException( string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BOPropReadException( string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BOPropReadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}