using System;
using System.Runtime.Serialization;
using Habanero.Base;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control at the point of beginning an
    /// object edit.  Typically occurs if another user/process has edited the
    /// object in the database since it was last loaded by the object manager
    /// </summary>
    [Serializable]
    public class BusObjBeginEditConcurrencyControlException : BusObjectConcurrencyControlException
    {
        /// <summary>
        /// Constructor to initialise the exception with a set of details
        /// regarding the editing of the object
        /// </summary>
        /// <param name="className">The class name</param>
        /// <param name="userName">The user name editing the record</param>
        /// <param name="machineName">The machine name editing the record</param>
        /// <param name="dateUpdated">The date that the editing took place</param>
        /// <param name="objectID">The object's ID</param>
        /// <param name="obj">The object in question</param>
        public BusObjBeginEditConcurrencyControlException(string className,
            string userName,
            string machineName,
            DateTime dateUpdated,
            string objectID,
            IBusinessObject obj)
            :
                base("You cannot Edit '" + className +
                     "', as another user has edited this record. \n" +
                     "UserName: " +
                     (userName.Length > 0 ? userName : "[Unknown]") +
                     " \nMachineName: " +
                     (machineName.Length > 0 ? machineName : "[Unknown]") +
                     " \nDateUpdated: " +
                     dateUpdated.ToString("dd MMM yyyy HH:mm:ss:fff") +
                     " \nObjectID: " + objectID, obj)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusObjBeginEditConcurrencyControlException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusObjBeginEditConcurrencyControlException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusObjBeginEditConcurrencyControlException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusObjBeginEditConcurrencyControlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}