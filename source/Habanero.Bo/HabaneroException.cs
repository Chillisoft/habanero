// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
//TODO andrew 22 Dec 2010: CF : does not support serialization
//using System.Runtime.Serialization;
using System.Security.Permissions;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBeProtected.Global
namespace Habanero.BO
{
    /// <summary>
    /// Provides an exception to throw when a business object is not found
    /// </summary>
    [Serializable]
    public abstract class BusinessObjectException : HabaneroApplicationException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        protected BusinessObjectException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        protected BusinessObjectException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        protected BusinessObjectException(string message, Exception inner)
            : base(message, inner)
        {
        }

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusinessObjectException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }

    /// <summary>
    /// Provides an exception to throw when a there is an issue writing to a property on 
    /// the businessobject due to the ReadWriteRule that has been set up for the property.
    /// </summary>
    [Serializable]
    public class BOPropWriteException : BusinessObjectException
    {
        private readonly IPropDef _propDef;

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        /// <param name="propDef">The property definition for the property that had the ReadWriteRule which threw the error.</param>
        public BOPropWriteException(PropDef propDef): this(propDef, ConstructMessage(propDef))
        {
            _propDef = propDef;
        }

        private static string ConstructMessage(PropDef propDef)
        {
            if (propDef == null) return "";
            string displayName = String.IsNullOrEmpty(propDef.DisplayName) ? propDef.PropertyName : propDef.DisplayName;
            return String.Format("Error writing to property '{0}' because it is configured as a '{1}' property.",
                                 displayName, propDef.ReadWriteRule);
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="propDef">The property definition for the property that had the ReadWriteRule which threw the error.</param>
        /// <param name="message">The error message</param>
        public BOPropWriteException(IPropDef propDef, string message) : base(message)
        {
            _propDef = propDef;
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="propDef">The property definition for the property that had the ReadWriteRule which threw the error.</param>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BOPropWriteException(PropDef propDef, string message, Exception inner) : base(message, inner)
        {
            _propDef = propDef;
        }

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>

        //protected BOPropWriteException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
        
        ///<summary>
        /// The property definition for the property that had the ReadWriteRule which threw the error.
        ///</summary>
        public IPropDef PropDef
        {
            get { return _propDef; }
        }
    }

    /// <summary>
    /// Provides an exception to throw when a there is an issue writing to a property on 
    /// the businessobject due to the ReadWriteRule that has been set up for the property.
    /// </summary>
    [Serializable]
    public class BOPropReadException : BusinessObjectException
    {

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BOPropReadException( string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BOPropReadException( string message, Exception inner)
            : base(message, inner)
        {
        }

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BOPropReadException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }

    /// <summary>
    /// Provides an exception to throw when a business object is not found
    /// </summary>
    [Serializable]
    public class BusinessObjectNotFoundException : BusinessObjectException
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

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusinessObjectNotFoundException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }

	/// <summary>
	/// Provides an exception to throw when a the referential integrity constraints of 
	/// a business object are being violated
	/// </summary>
	[Serializable]
    public class BusinessObjectReferentialIntegrityException : BusinessObjectException
	{
		/// <summary>
		/// Constructor to initialise the exception
		/// </summary>

		public BusinessObjectReferentialIntegrityException()

		{
		}

		/// <summary>
		/// Constructor to initialise the exception with a specific message
		/// to display
		/// </summary>
		/// <param name="message">The error message</param>
		public BusinessObjectReferentialIntegrityException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor to initialise the exception with a specific message
		/// to display, and the inner exception specified
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="inner">The inner exception</param>
		public BusinessObjectReferentialIntegrityException(string message, Exception inner)
			: base(message, inner)
		{
		}

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusinessObjectReferentialIntegrityException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
	}
    /// <summary>
    /// Provides an exception to throw when a the referential integrity constraints of 
    /// a business object are being violated
    /// </summary>
    [Serializable]
    public class BusObjPersistException : BusinessObjectException
    {
		/// <summary>
		/// Constructor to initialise the exception
		/// </summary>
		public BusObjPersistException()
		{
		}

		/// <summary>
		/// Constructor to initialise the exception with a specific message
		/// to display
		/// </summary>
		/// <param name="message">The error message</param>
		public BusObjPersistException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor to initialise the exception with a specific message
		/// to display, and the inner exception specified
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="inner">The inner exception</param>
		public BusObjPersistException(string message, Exception inner)
			: base(message, inner)
		{
		}

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusObjPersistException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }
   

    /// <summary>
    /// Provides an exception to throw when a property value is invalid
    /// </summary>
    [Serializable]
    public class InvalidPropertyValueException : HabaneroApplicationException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public InvalidPropertyValueException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public InvalidPropertyValueException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public InvalidPropertyValueException(string message, Exception inner) : base(message, inner)
        {
        }

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected InvalidPropertyValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //}
    }






    /// <summary>
    /// Provides an exception to throw when a business object is in an
    /// invalid state
    /// </summary>
    [Serializable]
    public class BusObjectInAnInvalidStateException : BusinessObjectException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusObjectInAnInvalidStateException()
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

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusObjectInAnInvalidStateException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }



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

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusObjectConcurrencyControlException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //    _obj = info.GetValue("businessObject", typeof (object));
        //}

        ///// <summary>
        ///// Gets object data using the specified serialisation info and
        ///// streaming context
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    // Implement type-specific serialization logic and call base serializer.
        //    info.AddValue("businessObject", _obj);
        //    base.GetObjectData(info, context);
        //}
    }



    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control, where another user has
    /// subsequently edited the record being saved
    /// </summary>
    [Serializable]
    public class BusObjOptimisticConcurrencyControlException : BusObjectConcurrencyControlException
    {
        private readonly string mUserNameEdited;
        private readonly string mMachineNameEdited;
        private readonly DateTime mDateUpdated;
        private readonly string mObjectID;
        private readonly string mClassName;

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
                                                           IBusinessObject obj) :
                                                               base("You cannot save the changes to '" + className +
                                                                    "', as another user has edited this record. \n" +
                                                                    "UserName: " +
                                                                    (userName.Length > 0 ? userName : "[Unknown]") +
                                                                    " \nMachineName: " +
                                                                    (machineName.Length > 0 ? machineName : "[Unknown]") +
                                                                    " \nDateUpdated: " +
                                                                    dateUpdated.ToString("dd MMM yyyy HH:mm:ss:fff") +
                                                                    " \nObjectID: " + objectID, obj)
        {
            mUserNameEdited = (userName.Length > 0 ? userName : "[Unknown]");
            mMachineNameEdited = (machineName.Length > 0 ? machineName : "[Unknown]");
            mDateUpdated = dateUpdated;
            mObjectID = objectID;
            _obj = obj;
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
        public IBusinessObject BusinessObject
        {
            get { return (BusinessObject) _obj; }
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

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusObjOptimisticConcurrencyControlException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //    mUserNameEdited = (string) info.GetValue("UserNameEdited", typeof (string));
        //    mMachineNameEdited = (string) info.GetValue("MachineNameEdited", typeof (string));
        //    mDateUpdated = (DateTime) info.GetValue("DateUpdated", typeof (DateTime));
        //    mObjectID = (string) info.GetValue("ObjectID", typeof (string));
        //    mClassName = (string) info.GetValue("_className", typeof (string));
        //}

        ///// <summary>
        ///// Gets object data using the specified serialisation info and
        ///// streaming context
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    // Implement type-specific serialization logic and call base serializer.
        //    info.AddValue("UserNameEdited", mUserNameEdited);
        //    info.AddValue("MachineNameEdited", mMachineNameEdited);
        //    info.AddValue("DateUpdated", mDateUpdated);
        //    info.AddValue("ObjectID", mObjectID);
        //    info.AddValue("_className", mClassName);
        //    base.GetObjectData(info, context);
        //}
    }



    /// <summary>
    /// Provides an exception to throw when an error has occurred with the
    /// business object's concurrency control, where another user has
    /// subsequently edited the record being saved
    /// </summary>
    [Serializable]
    public class BusObjPessimisticConcurrencyControlException : BusObjectConcurrencyControlException
    {
        private readonly string mUserNameEdited;
        private readonly string mMachineNameEdited;
        private readonly DateTime? mDateUpdated;
        private readonly string mObjectID;
        private readonly string mClassName;

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
            mUserNameEdited = (userName.Length > 0 ? userName : "[Unknown]");
            mMachineNameEdited = (machineName.Length > 0 ? machineName : "[Unknown]");
            mDateUpdated = dateUpdated;
            mObjectID = objectID;
            _obj = obj;
            mClassName = className;
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
        /// Returns the business object in question
        /// </summary>
        public IBusinessObject BusinessObject
        {
            get { return (BusinessObject)_obj; }
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
        public DateTime? DateTimeEdited
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

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusObjPessimisticConcurrencyControlException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //    mUserNameEdited = (string)info.GetValue("UserNameEdited", typeof(string));
        //    mMachineNameEdited = (string)info.GetValue("MachineNameEdited", typeof(string));
        //    mDateUpdated = (DateTime)info.GetValue("DateUpdated", typeof(DateTime));
        //    mObjectID = (string)info.GetValue("ObjectID", typeof(string));
        //    mClassName = (string)info.GetValue("_className", typeof(string));
        //}

        ///// <summary>
        ///// Gets object data using the specified serialisation info and
        ///// streaming context
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    // Implement type-specific serialization logic and call base serializer.
        //    info.AddValue("UserNameEdited", mUserNameEdited);
        //    info.AddValue("MachineNameEdited", mMachineNameEdited);
        //    info.AddValue("DateUpdated", mDateUpdated);
        //    info.AddValue("ObjectID", mObjectID);
        //    info.AddValue("_className", mClassName);
        //    base.GetObjectData(info, context);
        //}
    }

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

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusObjDeleteConcurrencyControlException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }



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

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusObjBeginEditConcurrencyControlException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }


    /// <summary>
    /// Provides an exception to throw when an object cannot be read.
    ///    Typically due to user permissions.
    /// </summary>
    [Serializable]
    public class BusObjReadException : BusinessObjectException
    {
        /// <summary>
        /// Teh Business Object the Exception is for
        /// </summary>
        protected object mobj;

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public BusObjReadException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public BusObjReadException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BusObjReadException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a set of details
        /// regarding the object
        /// </summary>
        /// <param name="className">The class name</param>
        /// <param name="objectID">The object's ID</param>
        /// <param name="obj">The object in question</param>
        public BusObjReadException(string className, string objectID, object obj)
            :
            base("You cannot start editing " + className + " as the object " +
                 objectID + " is already in edit mode")
        {
            mobj = obj;
        }

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusObjReadException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //    mobj = info.GetValue("businessObject", typeof(object));
        //}

        ///// <summary>
        ///// Gets object data using the specified serialisation info and
        ///// streaming context
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("businessObject", mobj);
        //    base.GetObjectData(info, context);
        //}
    }

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

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected EditingException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //    mobj = info.GetValue("businessObject", typeof (object));
        //}

        ///// <summary>
        ///// Gets object data using the specified serialisation info and
        ///// streaming context
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("businessObject", mobj);
        //    base.GetObjectData(info, context);
        //}
    }



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

        ///// <summary>
        ///// Constructor to initialise the exception with the serialisation info
        ///// and streaming context provided
        ///// </summary>
        ///// <param name="info">The serialisation info</param>
        ///// <param name="context">The streaming context</param>
        //protected BusObjDuplicateConcurrencyControlException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }
    /// <summary>
    /// Provides an exception to throw when the object cannot be deleted due to either the 
    /// custom rules being broken for a deletion or the IsDeletable flag being set to false.
    /// </summary>
    [Serializable]
    public class BusObjDeleteException : BusinessObjectException
    {
        /// <summary>
        /// Constructor to initialise the exception with details regarding the
        /// object whose record was deleted
        /// </summary>
        /// <param name="bo">The business object in question</param>
        /// <param name="message">Additional err message</param>
        public BusObjDeleteException(IBusinessObject bo, string message):
                base(
                    string.Format(
                        "You cannot delete the '{0}', as the IsDeletable is set to false for the object. " +
                        "ObjectID: {1}, also identified as {2} \n " + 
                        "Message: {3}",
                        bo.ClassDef.ClassName, bo.ID, bo,message))
        {
        }
    }
    /// <summary>
    /// Provides an exception to throw when the object cannot be deleted due to either the 
    /// custom rules being broken for a deletion or the IsDeletable flag being set to false.
    /// </summary>
    [Serializable]
    public class BusObjEditableException : BusinessObjectException
    {
        /// <summary>
        /// Constructor to initialise the exception with details regarding the
        /// object whose record was deleted
        /// </summary>
        /// <param name="bo">The business object in question</param>
        /// <param name="message">Additional message</param>
        public BusObjEditableException(BusinessObject bo, string message)
            :
                base(
                    string.Format(
                        "You cannot Edit the '{0}', as the IsEditable is set to false for the object. " +
                        "ObjectID: {1}, also identified as {2} \n " + 
                        "Message: {3}",
                        bo.ClassDef.ClassName, bo.ID, bo, message))
        {
        }
    }
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBePrivate.Global
    // ReSharper restore MemberCanBeProtected.Global
}