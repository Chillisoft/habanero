using System;
using System.Runtime.Serialization;
using Habanero.Base;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control, where another user has
    /// subsequently edited the record being saved
    /// </summary>
    [Serializable]
    public class BusObjPessimisticConcurrencyControlException : BusObjUserConcurrencyControlExceptionBase
    {
        /// <summary>
        /// Constructor to initialise the exception with a set of concurrency
        /// details
        /// </summary>
        /// <param name="className">The class name</param>
        /// <param name="userName">The user name that locked the record</param>
        /// <param name="machineName">The machine name that locked the record</param>
        /// <param name="dateUpdated">The date that the record was locked</param>
        /// <param name="objectID">The object ID</param>
        /// <param name="obj">The object whose record was locked</param>
        public BusObjPessimisticConcurrencyControlException(string className,
            string userName,
            string machineName,
            DateTime dateUpdated,
            string objectID,
            IBusinessObject obj)
            :
                base("You cannot begin edits on the '" + className +
                     "', as another user has started edits and therefore locked to this record. \n" +
                     "UserName: " +
                     (userName.Length > 0 ? userName : "[Unknown]") +
                     " \nMachineName: " +
                     (machineName.Length > 0 ? machineName : "[Unknown]") +
                     " \nDateUpdated: " +
                     dateUpdated.ToString("dd MMM yyyy HH:mm:ss:fff") +
                     " \nObjectID: " + objectID, obj)
        {
            _userNameEdited = (userName.Length > 0 ? userName : "[Unknown]");
            _machineNameEdited = (machineName.Length > 0 ? machineName : "[Unknown]");
            _dateUpdated = dateUpdated;
            _objectID = objectID;
            _obj = obj;
            _className = className;
        }

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusObjPessimisticConcurrencyControlException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusObjPessimisticConcurrencyControlException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusObjPessimisticConcurrencyControlException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusObjPessimisticConcurrencyControlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}