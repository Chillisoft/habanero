using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when a new edit state is being set
    /// while the object is already in edit mode
    /// </summary>
    [Serializable]
    public class EditingException : BusinessObjectException
    {
        /// <summary>
        /// The business object that had the editing exception.
        /// </summary>
        protected object mobj;

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public EditingException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public EditingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public EditingException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a set of details
        /// regarding the object
        /// </summary>
        /// <param name="className">The class name</param>
        /// <param name="objectID">The object's ID</param>
        /// <param name="obj">The object in question</param>
        public EditingException(string className, string objectID, object obj) :
            base("You cannot start editing " + className + " as the object " +
                 objectID + " is already in edit mode")
        {
            mobj = obj;
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected EditingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            mobj = info.GetValue("businessObject", typeof (object));
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
            info.AddValue("businessObject", mobj);
            base.GetObjectData(info, context);
        }
    }
}