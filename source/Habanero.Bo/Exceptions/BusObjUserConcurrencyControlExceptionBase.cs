using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Habanero.Base;

namespace Habanero.BO.Exceptions
{
    public class BusObjUserConcurrencyControlExceptionBase : BusObjectConcurrencyControlException
    {
        protected string _userNameEdited;
        protected string _machineNameEdited;
        protected DateTime _dateUpdated;
        protected string _objectID;
        protected string _className;

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        protected BusObjUserConcurrencyControlExceptionBase()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected BusObjUserConcurrencyControlExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _userNameEdited = (string) info.GetValue("UserNameEdited", typeof (string));
            _machineNameEdited = (string) info.GetValue("MachineNameEdited", typeof (string));
            _dateUpdated = (DateTime) info.GetValue("DateUpdated", typeof (DateTime));
            _objectID = (string) info.GetValue("ObjectID", typeof (string));
            _className = (string) info.GetValue("_className", typeof (string));
        }

        /// <summary>
        /// Constructor to initialise the exception with the specified message
        /// to display and the object in question
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="obj">The object involved in the exception</param>
        protected BusObjUserConcurrencyControlExceptionBase(string message, object obj) : base(message, obj)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        protected BusObjUserConcurrencyControlExceptionBase(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        protected BusObjUserConcurrencyControlExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Returns the machine name that edited the record
        /// </summary>
        public string MachineNameEdited
        {
            get { return _machineNameEdited; }
        }

        /// <summary>
        /// Returns the user name that edited the record
        /// </summary>
        public string UserNameEdited
        {
            get { return _userNameEdited; }
        }

        /// <summary>
        /// Returns the date that the record was edited
        /// </summary>
        public DateTime DateTimeEdited
        {
            get { return _dateUpdated; }
        }

        /// <summary>
        /// Returns the class name
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }

        /// <summary>
        /// Returns the business object in question
        /// </summary>
        public IBusinessObject BusinessObject
        {
            get { return (BusinessObject) _obj; }
        }

        /// <summary>
        /// Returns the object's ID
        /// </summary>
        public string ObjectID
        {
            get { return _objectID; }
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
            info.AddValue("UserNameEdited", _userNameEdited);
            info.AddValue("MachineNameEdited", _machineNameEdited);
            info.AddValue("DateUpdated", _dateUpdated);
            info.AddValue("ObjectID", _objectID);
            info.AddValue("_className", _className);
            base.GetObjectData(info, context);
        }
    }
}