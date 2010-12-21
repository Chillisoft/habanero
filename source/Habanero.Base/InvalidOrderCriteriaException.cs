using System;

//TODO andrew 21 Dec 2010: Removed Serialization code
//using System.Runtime.Serialization;
namespace Habanero.Base.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when a property value is invalid
    /// </summary>
    [Serializable]
    public class InvalidOrderCriteriaException : HabaneroApplicationException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public InvalidOrderCriteriaException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public InvalidOrderCriteriaException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public InvalidOrderCriteriaException(string message, Exception inner)
            : base(message, inner)
        {
        }

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected InvalidOrderCriteriaException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }
}