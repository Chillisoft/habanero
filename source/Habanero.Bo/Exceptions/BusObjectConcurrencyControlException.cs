using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control
    /// </summary>
    [Serializable]
    public class BusObjectConcurrencyControlException : BusinessObjectException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusObjectConcurrencyControlException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusObjectConcurrencyControlException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusObjectConcurrencyControlException(string message, Exception inner) : base(message, inner)
        {
        }
        /// <summary>
        /// The Business Object the concurrency control exception is for
        /// </summary>
        protected object _obj; //TODO make this be a business object only

        /// <summary>
        /// Constructor to initialise the exception with the specified message
        /// to display and the object in question
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="obj">The object involved in the exception</param>
        public BusObjectConcurrencyControlException(string message, object obj) : base(message)
        {
            _obj = obj;
        }

        /// <summary>
        /// Returns the object involved in the exception
        /// </summary>
        public object getObject
        {
            get { return _obj; }
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusObjectConcurrencyControlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _obj = info.GetValue("businessObject", typeof (object));
        }

        /// <summary>
        /// Gets object data using the specified serialisation info and
        /// streaming context
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Implement type-specific serialization logic and call base serializer.
            info.AddValue("businessObject", _obj);
            base.GetObjectData(info, context);
        }
    }
}