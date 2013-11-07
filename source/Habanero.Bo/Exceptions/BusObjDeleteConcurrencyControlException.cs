using System;
using System.Runtime.Serialization;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control, where another user has deleted
    /// the record in question
    /// </summary>
    [Serializable]
    public class BusObjDeleteConcurrencyControlException : BusObjectConcurrencyControlException
    {
        /// <summary>
        /// Constructor to initialise the exception with details regarding the
        /// object whose record was deleted
        /// </summary>
        /// <param name="className">The class name</param>
        /// <param name="objectID">The object's ID</param>
        /// <param name="obj">The object in question</param>
        public BusObjDeleteConcurrencyControlException(string className,
            string objectID,
            object obj) :
                base("You cannot save the changes to '" +
                     className +
                     "', as another user has deleted the record. \n" +
                     "\nObjectID: " + objectID, obj)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusObjDeleteConcurrencyControlException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusObjDeleteConcurrencyControlException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusObjDeleteConcurrencyControlException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusObjDeleteConcurrencyControlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}