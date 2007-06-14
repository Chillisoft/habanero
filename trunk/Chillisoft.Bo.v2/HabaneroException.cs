using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides an exception to throw when a business object is not found
    /// </summary>
    [Serializable()]
    public class BusinessObjectNotFoundException : BaseApplicationException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusinessObjectNotFoundException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusinessObjectNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusinessObjectNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusinessObjectNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }



    /// <summary>
    /// Provides an exception to throw when a property value is invalid
    /// </summary>
    [Serializable()]
    public class PropertyValueInvalidException : BaseApplicationException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public PropertyValueInvalidException() : base()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public PropertyValueInvalidException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public PropertyValueInvalidException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected PropertyValueInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }



    /// <summary>
    /// Provides an exception to throw when a business object is in an
    /// invalid state
    /// </summary>
    [Serializable()]
    public class BusObjectInAnInvalidStateException : BaseApplicationException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusObjectInAnInvalidStateException() : base()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusObjectInAnInvalidStateException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusObjectInAnInvalidStateException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusObjectInAnInvalidStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }



    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control
    /// </summary>
    [Serializable()]
    public class BusObjectConcurrencyControlException : BaseApplicationException, ISerializable
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

        protected object mobj; //TODO make this be a business object only

        /// <summary>
        /// Constructor to initialise the exception with the specified message
        /// to display and the object in question
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="obj">The object involved in the exception</param>
        public BusObjectConcurrencyControlException(string message, object obj) : base(message)
        {
            mobj = obj;
        }

        /// <summary>
        /// Returns the object involved in the exception
        /// </summary>
        public object getObject
        {
            get { return mobj; }
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
            // Implement type-specific serialization logic and call base serializer.
            info.AddValue("businessObject", mobj);
            base.GetObjectData(info, context);
        }
    }



    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control, where another user has
    /// subsequently edited the record being saved
    /// </summary>
    [Serializable()]
    public class BusObjOptimisticConcurrencyControlException : BusObjectConcurrencyControlException, ISerializable
    {
        private string mUserNameEdited;
        private string mMachineNameEdited;
        private DateTime mDateUpdated;
        private string mObjectID;
        private string mClassName;

        /// <summary>
        /// Constructor to initialise the exception with a set of concurrency
        /// details
        /// </summary>
        /// <param name="className">The class name</param>
        /// <param name="userName">The user name that edited the record</param>
        /// <param name="machineName">The machine name that edited the record</param>
        /// <param name="dateUpdated">The date that the record was edited</param>
        /// <param name="objectID">The object ID</param>
        /// <param name="obj">The object whose record was edited</param>
        public BusObjOptimisticConcurrencyControlException(string className,
                                                           string userName,
                                                           string machineName,
                                                           DateTime dateUpdated,
                                                           string objectID,
                                                           BusinessObject obj) :
                                                               base("You cannot save the changes to the " +
                                                                    className +
                                                                    ", as another user has edited the this record.\n" +
                                                                    "UserName : " +
                                                                    (userName.Length > 0 ? userName : "[Unknown]") +
                                                                    "\nMachineName : " +
                                                                    (machineName.Length > 0 ? machineName : "[Unknown]") +
                                                                    "\nDateUpdated : " +
                                                                    dateUpdated.ToString("dd MMM yyyy HH:mm:ss:fff") +
                                                                    "\nObjectID : " + objectID, obj)
        {
            mUserNameEdited = (userName.Length > 0 ? userName : "[Unknown]");
            mMachineNameEdited = (machineName.Length > 0 ? machineName : "[Unknown]");
            mDateUpdated = dateUpdated;
            mObjectID = objectID;
            mobj = obj;
            mClassName = className;
        }

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusObjOptimisticConcurrencyControlException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusObjOptimisticConcurrencyControlException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusObjOptimisticConcurrencyControlException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Returns the business object in question
        /// </summary>
        public BusinessObject BusinessObject
        {
            get { return (BusinessObject) mobj; }
        }

        /// <summary>
        /// Returns the machine name that edited the record
        /// </summary>
        public string MachineNameEdited
        {
            get { return mMachineNameEdited; }
        }

        /// <summary>
        /// Returns the user name that edited the record
        /// </summary>
        public string UserNameEdited
        {
            get { return mUserNameEdited; }
        }

        /// <summary>
        /// Returns the object's ID
        /// </summary>
        public string ObjectID
        {
            get { return mObjectID; }
        }

        /// <summary>
        /// Returns the date that the record was edited
        /// </summary>
        public DateTime DateTimeEdited
        {
            get { return mDateUpdated; }
        }

        /// <summary>
        /// Returns the class name
        /// </summary>
        public string ClassName
        {
            get { return mClassName; }
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusObjOptimisticConcurrencyControlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            mUserNameEdited = (string) info.GetValue("UserNameEdited", typeof (string));
            mMachineNameEdited = (string) info.GetValue("MachineNameEdited", typeof (string));
            mDateUpdated = (DateTime) info.GetValue("DateUpdated", typeof (DateTime));
            mObjectID = (string) info.GetValue("ObjectID", typeof (string));
            mClassName = (string) info.GetValue("_className", typeof (string));
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
            info.AddValue("UserNameEdited", mUserNameEdited);
            info.AddValue("MachineNameEdited", mMachineNameEdited);
            info.AddValue("DateUpdated", mDateUpdated);
            info.AddValue("ObjectID", mObjectID);
            info.AddValue("_className", mClassName);
            base.GetObjectData(info, context);
        }
    }



    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control, where another user has deleted
    /// the record in question
    /// </summary>
    [Serializable()]
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
                                                           base("You cannot save the changes to the " +
                                                                className +
                                                                ", as another user has deleted the record.\n" +
                                                                "\nObjectID : " + objectID, obj)
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



    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control at the point of beginning an
    /// object edit.  Typically occurs if another user/process has edited the
    /// object in the database since it was last loaded by the object manager
    /// </summary>
    [Serializable()]
    public class BusObjBeginEditConcurrencyControlException : BusObjOptimisticConcurrencyControlException
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
                                                          BusinessObject obj) :
                                                              base(
                                                              className, userName, machineName, dateUpdated, objectID,
                                                              obj)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception using details held in a
        /// BusObjOptimisticConcurrencyControlException provided
        /// </summary>
        /// <param name="ex">The BusObjOptimisticConcurrencyControlException</param>
        public BusObjBeginEditConcurrencyControlException(BusObjOptimisticConcurrencyControlException ex) :
            base(ex.ClassName, ex.UserNameEdited, ex.MachineNameEdited,
                 ex.DateTimeEdited, ex.ObjectID, ex.BusinessObject)
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



    /// <summary>
    /// Provides an exception to throw when a new edit state is being set
    /// while the object is already in edit mode
    /// </summary>
    [Serializable()]
    public class EditingException : BaseApplicationException, ISerializable
    {
        protected object mobj; //TODO move this such that it has type BusObj

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public EditingException() : base()
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



    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control, where a save is being attempted
    /// when another user has duplicated the record in question
    /// </summary>
    [Serializable()]
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
                                                              base("You cannot save " +
                                                                   className +
                                                                   ", as another user has created a duplicate record.\n" +
                                                                   "UserName : " +
                                                                   (userName.Length > 0 ? userName : "[Unknown]") +
                                                                   "\nMachineName : " +
                                                                   (machineName.Length > 0 ? machineName : "[Unknown]") +
                                                                   "\nDateUpdated : " +
                                                                   dateDuplicated.ToString("dd MMM yyyy HH:mm:ss:fff") +
                                                                   "\nDuplicateObject : " + duplicateWhereClause, obj)
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