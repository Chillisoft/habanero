using System;
using System.Runtime.Serialization;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Provides an exception to throw when a method receives the name of a class
    /// as a string to instantiate, but cannot find the type for that class name
    /// </summary>
    [Serializable()]
    public class UnknownTypeNameException : BaseApplicationException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public UnknownTypeNameException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public UnknownTypeNameException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public UnknownTypeNameException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected UnknownTypeNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}