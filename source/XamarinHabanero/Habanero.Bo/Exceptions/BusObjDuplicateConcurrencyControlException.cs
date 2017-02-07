using System;
using System.Runtime.Serialization;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control, where a save is being attempted
    /// when another user has duplicated the record in question
    /// </summary>
    [Serializable]
    public class BusObjDuplicateConcurrencyControlException : BusObjectConcurrencyControlException
    {
        /// <summary>
        /// Constructor to initialise the exception with a set of details
        /// regarding the object that was duplicated
        /// </summary>
        /// <param name="className">The class name</param>
        /// <param name="userName">The user name that duplicated the record</param>
        /// <param name="machineName">The machine name that duplicated the
        /// record</param>
        /// <param name="dateDuplicated">The date when the record was duplicated</param>
        /// <param name="duplicateWhereClause">The duplicate "where" clause</param>
        /// <param name="obj">The object in question</param>
        public BusObjDuplicateConcurrencyControlException(string className,
            string userName,
            string machineName,
            DateTime dateDuplicated,
            string duplicateWhereClause,
            object obj) :
                base("You cannot save '" + className +
                     "', as another user has created a duplicate record. \n" +
                     "UserName: " +
                     (userName.Length > 0 ? userName : "[Unknown]") +
                     " \nMachineName: " +
                     (machineName.Length > 0 ? machineName : "[Unknown]") +
                     " \nDateUpdated: " +
                     dateDuplicated.ToString("dd MMM yyyy HH:mm:ss:fff") +
                     " \nDuplicateObject: " + duplicateWhereClause, obj)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusObjDuplicateConcurrencyControlException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusObjDuplicateConcurrencyControlException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusObjDuplicateConcurrencyControlException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusObjDuplicateConcurrencyControlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}