//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.Comparer;
using Habanero.Util.File;
using log4net;

namespace Habanero.BO.ClassDefinition
{

    /// <summary>
    /// An enumeration used to specify different file access modes.
    /// </summary>
    public enum PropReadWriteRule
    {
        /// <summary>Full access</summary>
        ReadWrite,
        /// <summary>Read but not write/edit</summary>
        ReadOnly,
        /// <summary>Can only be edited it if was never edited before 
        /// (regardless of whether the object is new or not)</summary>
        WriteOnce,
        /// <summary>Can only be edited if the object is not new. 
        /// I.e. the property can only be updated but never created in a new object that is being inserted</summary>
        WriteNotNew,
        /// <summary>Can only be edited if the object is new. 
        /// I.e. the property can only be inserted and can never be updated after that</summary>
        WriteNew
    }

    
    /// <summary>
    /// A PropDef contains a Business Object property definition, with
    /// the property name and information such as the 
    /// access rules for the property (i.e. write-once, read-many or 
    /// read-many-write-many).
    /// </summary>
    /// <futureEnhancements>
    /// TODO_Future:
    /// <ul>
    /// <li>Add ability to calculated properties.</li>
    /// <li>Lazy initialisation of properties.</li>
    /// </ul>
    /// </futureEnhancements>
    public class PropDef
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.ClassDefinition.PropDef");
        private string _propertyName;
        private string _description;
		private string _propTypeAssemblyName;
    	private string _propTypeName;
		private Type _propType;
		private PropReadWriteRule _propRWStatus;
		private string _databaseFieldName; //This allows you to have a 
            //database field name different from your property name. 
            //We have customers whose standard for naming database 
            //fields is DATABASE_FIELD_NAME. 
            //This is also powerful for migrating systems 
            //where the database has already been set up.
		//TODO: I changed this field from ReadOnly, was there any point to this. Please Review. (-Mark)
		private object _defaultValue = null;
    	private string _defaultValueString;
    	private bool _hasDefaultValueBeenValidated;
        private PropRuleBase _propRule;
        private ILookupList _lookupList = new NullLookupList();
    	private bool _compulsory = false;
        private bool _autoIncrementing = false;
        private int _length;
        private string _displayName;
        private bool _keepValuePrivate = false;
        private bool _persistable = true;

        #region Constuctor and destructors

        #region Progressive Public Constructors

        /// <summary>
        /// This constructor is used to create a propdef using it's property type and other information. 
        /// </summary>
        /// <param name="propertyName" >The name of the property (e.g. "surname")</param>
        /// <param name="propType">The type of the property (e.g. string)</param>
        /// <param name="propRWStatus">Rules for how a property can be accessed.
		/// See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up.</param>
        /// <param name="defaultValue">The default value that a property 
        /// of a new object will be set to</param>
        public PropDef(string propertyName,
                       Type propType,
                       PropReadWriteRule propRWStatus,
                       string databaseFieldName,
                       object defaultValue) :
							this(propertyName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null)
        {
        }

		/// <summary>
		/// This constructor is used to create a propdef using it's property type and other information. 
		/// The database field name is presumed to be the same as the property name.
		/// </summary>
		/// <param name="propertyName">The name of the property (e.g. "surname")</param>
		/// <param name="propType">The type of the property (e.g. string)</param>
		/// <param name="propRWStatus">Rules for how a property can be accessed.
		/// See PropReadWriteRule enumeration for more detail.</param>
		/// <param name="defaultValue">The default value that a property 
		/// of a new object will be set to</param>
        public PropDef(string propertyName,
                       Type propType,
                       PropReadWriteRule propRWStatus,
                       object defaultValue) 
			:this(propertyName, propType,null,null, propRWStatus, null, defaultValue, null)
        {
        }

        /// <summary>
        /// This constructor is used to create a propdef using property type assembly and class name and other information. 
        /// The default value and the property type are loaded when they are needed.
        /// </summary>
        /// <param name="propertyName">The name of the property (e.g. "surname")</param>
        /// <param name="assemblyName">The assembly name of the property type</param>
        /// <param name="typeName">The type name of the property type (e.g. "string")</param>
        /// <param name="propRWStatus">Rules for how a property can be accessed.
        /// See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up.</param>
        /// <param name="defaultValueString">The default value that a property 
        /// of a new object will be set to</param>
        /// <param name="compulsory">Whether this property is a required field or not.</param>
        /// <param name="autoIncrementing">Whether this is an auto-incrementing field in the database</param>
        public PropDef(string propertyName,
                    string assemblyName, string typeName,
                    PropReadWriteRule propRWStatus,
                    string databaseFieldName,
                    string defaultValueString,
                    bool compulsory,
                    bool autoIncrementing)
            : this(propertyName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValueString, compulsory, autoIncrementing)
        {
        }

		/// <summary>
		/// This constructor is used to create a propdef using property type assembly and class name and other information. 
		/// The default value and the property type are loaded when they are needed.
		/// </summary>
		/// <param name="propertyName">The name of the property (e.g. "surname")</param>
		/// <param name="assemblyName">The assembly name of the property type</param>
		/// <param name="typeName">The type name of the property type (e.g. "string")</param>
		/// <param name="propRWStatus">Rules for how a property can be accessed.
		/// See PropReadWriteRule enumeration for more detail.</param>
		/// <param name="databaseFieldName">The database field name - this
		/// allows you to have a database field name that is different to the
		/// property name, which is useful for migrating systems where
		/// the database has already been set up.</param>
		/// <param name="defaultValueString">The default value that a property 
		/// of a new object will be set to</param>
		/// <param name="compulsory">Whether this property is a required field or not.</param>
		/// <param name="autoIncrementing">Whether this is an auto-incrementing field in the database</param>
		/// <param name="length">The maximum length for a string</param>
		public PropDef(string propertyName,
					string assemblyName, string typeName,
					PropReadWriteRule propRWStatus,
					string databaseFieldName,
					string defaultValueString, 
                    bool compulsory,
                    bool autoIncrementing,
                    int length)
            : this(propertyName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValueString, compulsory, autoIncrementing, length)
		{
		}

        /// <summary>
        /// This constructor is used to create a propdef using property type assembly and class name and other information. 
        /// The default value and the property type are loaded when they are needed.
        /// </summary>
        /// <param name="propertyName">The name of the property (e.g. "surname")</param>
        /// <param name="assemblyName">The assembly name of the property type</param>
        /// <param name="typeName">The type name of the property type (e.g. "string")</param>
        /// <param name="propRWStatus">Rules for how a property can be accessed.
        /// See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up.</param>
        /// <param name="defaultValueString">The default value that a property 
        /// of a new object will be set to</param>
        /// <param name="compulsory">Whether this property is a required field or not.</param>
        /// <param name="autoIncrementing">Whether this is an auto-incrementing field in the database</param>
        /// <param name="length">The maximum length for a string</param>
        /// <param name="displayName">The display name for the property</param>
        /// <param name="description">The description of the property</param>
        public PropDef(string propertyName,
                    string assemblyName, string typeName,
                    PropReadWriteRule propRWStatus,
                    string databaseFieldName,
                    string defaultValueString,
                    bool compulsory,
                    bool autoIncrementing,
                    int length,
                    string displayName,
                    string description)
            : this(propertyName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValueString, compulsory, autoIncrementing, length, displayName, description)
        {
        }

        /// <summary>
        /// This constructor is used to create a propdef using property type assembly and class name and other information. 
        /// The default value and the property type are loaded when they are needed.
        /// </summary>
        /// <param name="propertyName">The name of the property (e.g. "surname")</param>
        /// <param name="assemblyName">The assembly name of the property type</param>
        /// <param name="typeName">The type name of the property type (e.g. "string")</param>
        /// <param name="propRWStatus">Rules for how a property can be accessed.
        /// See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up.</param>
        /// <param name="defaultValueString">The default value that a property 
        /// of a new object will be set to</param>
        /// <param name="compulsory">Whether this property is a required field or not.</param>
        /// <param name="autoIncrementing">Whether this is an auto-incrementing field in the database</param>
        /// <param name="length">The maximum length for a string</param>
        /// <param name="displayName">The display name for the property</param>
        /// <param name="description">The description of the property</param>
        /// <param name="keepValuePrivate">Whether this property must keep its value private or not</param>
        public PropDef(string propertyName,
                    string assemblyName, string typeName,
                    PropReadWriteRule propRWStatus,
                    string databaseFieldName,
                    string defaultValueString,
                    bool compulsory,
                    bool autoIncrementing,
                    int length,
                    string displayName,
                    string description,
                    bool keepValuePrivate)
            : this(propertyName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValueString, compulsory, autoIncrementing, length, displayName, description, keepValuePrivate)
        {
        }

        /// <summary>
        /// This constructor is used to create a propdef using property type assembly and class name and other information. 
        /// The default value and the property type are loaded when they are needed.
        /// </summary>
        /// <param name="propertyName">The name of the property (e.g. "surname")</param>
        /// <param name="propType">The type of the property (e.g. string)</param>
        /// <param name="propRWStatus">Rules for how a property can be accessed.
        /// See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up.</param>
        /// <param name="defaultValue">The default value that a property 
        /// of a new object will be set to</param>
        /// <param name="compulsory">Whether this property is a required field or not.</param>
        /// <param name="autoIncrementing">Whether this is an auto-incrementing field in the database</param>
        /// <param name="length">The maximum length for a string</param>
        /// <param name="displayName">The display name for the property</param>
        /// <param name="description">The description of the property</param>
        public PropDef(string propertyName,
                    Type propType,
                    PropReadWriteRule propRWStatus,
                    string databaseFieldName,
                    object defaultValue,
                    bool compulsory,
                    bool autoIncrementing,
                    int length,
                    string displayName,
                    string description)
            : this(propertyName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null, compulsory, autoIncrementing, length, displayName, description)
        {
        }

        /// <summary>
        /// This constructor is used to create a propdef using property type assembly and class name and other information. 
        /// The default value and the property type are loaded when they are needed.
        /// </summary>
        /// <param name="propertyName">The name of the property (e.g. "surname")</param>
        /// <param name="propType">The type of the property (e.g. string)</param>
        /// <param name="propRWStatus">Rules for how a property can be accessed.
        /// See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up.</param>
        /// <param name="defaultValue">The default value that a property 
        /// of a new object will be set to</param>
        /// <param name="compulsory">Whether this property is a required field or not.</param>
        /// <param name="autoIncrementing">Whether this is an auto-incrementing field in the database</param>
        public PropDef(string propertyName,
                    Type propType,
                    PropReadWriteRule propRWStatus,
                    string databaseFieldName,
                    object defaultValue,
                    bool compulsory,
                    bool autoIncrementing)
            : this(propertyName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null, compulsory, autoIncrementing)
        {
        }

        /// <summary>
        /// This constructor is used to create a propdef using property type assembly and class name and other information. 
        /// The default value and the property type are loaded when they are needed.
        /// </summary>
        /// <param name="propertyName">The name of the property (e.g. "surname")</param>
        /// <param name="propType">The type of the property (e.g. string)</param>
        /// <param name="propRWStatus">Rules for how a property can be accessed.
        /// See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up.</param>
        /// <param name="defaultValue">The default value that a property 
        /// of a new object will be set to</param>
        /// <param name="compulsory">Whether this property is a required field or not.</param>
        /// <param name="autoIncrementing">Whether this is an auto-incrementing field in the database</param>
        /// <param name="length">The maximum length for a string</param>
        /// <param name="displayName">The display name for the property</param>
        /// <param name="description">The description of the property</param>
        /// <param name="keepValuePrivate">Whether this property must keep its value private or not</param>
        public PropDef(string propertyName,
                    Type propType,
                    PropReadWriteRule propRWStatus,
                    string databaseFieldName,
                    object defaultValue,
                    bool compulsory,
                    bool autoIncrementing,
                    int length,
                    string displayName,
                    string description,
                    bool keepValuePrivate)
            : this(propertyName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null, compulsory, autoIncrementing, length, displayName, description, keepValuePrivate)
        {
        }

        #endregion //Progressive Public Constructors

        #region Progressive Private Constructors

        private PropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus)
            : this(propertyName, propType, assemblyName, typeName, propRWStatus, null)
        { }

        private PropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
               string databaseFieldName)
            : this(propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, null, null)
        { }

        private PropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
               string databaseFieldName, object defaultValue, string defaultValueString)
            : this(propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue, defaultValueString, false)
        { }

        private PropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
               string databaseFieldName, object defaultValue, string defaultValueString, bool compulsory)
            : this(propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue, defaultValueString, compulsory, false)
        { }

        private PropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
               string databaseFieldName, object defaultValue, string defaultValueString,
               bool compulsory, bool autoIncrementing)
            : this(propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue, defaultValueString, compulsory, autoIncrementing, int.MaxValue)
        { }

        private PropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
               string databaseFieldName, object defaultValue, string defaultValueString,
               bool compulsory, bool autoIncrementing, int length)
            : this(propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue, defaultValueString, compulsory, autoIncrementing, length, "")
        { }

        private PropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
               string databaseFieldName, object defaultValue, string defaultValueString,
               bool compulsory, bool autoIncrementing, int length, string displayName)
            : this(propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue, defaultValueString, compulsory, autoIncrementing, length, displayName, "")
        { }

        private PropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
               string databaseFieldName, object defaultValue, string defaultValueString,
               bool compulsory, bool autoIncrementing, int length, string displayName, string description)
            : this(propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue, defaultValueString, compulsory, autoIncrementing, length, displayName, description, false)
        { }

        #endregion //Progressive Private Constructors

        private PropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
                string databaseFieldName, object defaultValue, string defaultValueString,
                bool compulsory, bool autoIncrementing, int length,
                string displayName, string description, bool keepValuePrivate)
        {
            SetupPropDef(propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue,
                         defaultValueString, compulsory, autoIncrementing, length, displayName, description,
                         keepValuePrivate);
        }

        private void SetupPropDef(string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus, string databaseFieldName, object defaultValue, string defaultValueString, bool compulsory, bool autoIncrementing, int length, string displayName, string description, bool keepValuePrivate)
        {
            ArgumentValidationHelper.CheckStringArgumentNotEmpty(propertyName, "propertyName", "This field is compulsary for the PropDef class.");
            if (propertyName.IndexOfAny(new char[] { '.', '-', '|' }) != -1)
            {
                throw new ArgumentException(
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name " +
                    propertyName);
            }
            _propertyName = propertyName;
            if (propType != null)
            {
                MyPropertyType = propType;
            }else
            {
                _propTypeAssemblyName = assemblyName;
                _propTypeName = typeName;
            }
            _propRWStatus = propRWStatus;
            if (databaseFieldName != null)
            {
                _databaseFieldName = databaseFieldName;
            }else
            {
                _databaseFieldName = propertyName;
            }
            if (defaultValue != null)
            {
                MyDefaultValue = defaultValue;
            }else
            {
                _defaultValueString = defaultValueString;
            }
            _compulsory = compulsory;
            _autoIncrementing = autoIncrementing;
            _length = length;
            _displayName = displayName;
            _description = description;
            _keepValuePrivate = keepValuePrivate;
        }

        #endregion


		#region Properties

		/// <summary>
        /// The name of the property, e.g. surname
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
			protected set{ _propertyName = value;}
        }

        ///<summary>
        /// The display name for the property.
        ///</summary>
        public string DisplayName
        {
            get { return _displayName; }
            protected set { _displayName = value; }
        }

        ///<summary>
        /// The description of the property.
        ///</summary>
        public string Description
        {
            get { return _description; }
            protected set { _description = value; }
        }
		
		/// <summary>
		/// The name of the property type assembly
		/// </summary>
		public string PropertyTypeAssemblyName
		{
			get { return _propTypeAssemblyName; }
			protected set
			{
				if (_propTypeAssemblyName != value)
				{
					_propTypeName = null;
					_propType = null;
				}
				_propTypeAssemblyName = value;
			}
		}

		/// <summary>
		/// The name of the property type
		/// </summary>
		public string PropertyTypeName
		{
			get { return _propTypeName; }
			protected set
			{
				if (_propTypeName != value)
				{
					_propType = null;
				}
				_propTypeName = value;
			}
		}

        /// <summary>
        /// The type of the property, e.g. string
        /// </summary>
        public Type PropertyType
        {
			get { return MyPropertyType; }
			protected set { MyPropertyType = value; }
        }

        /// <summary>
        /// Gets and sets the property rule relevant to this definition
        /// </summary>
        public virtual PropRuleBase PropRule
        {
            get { return _propRule; }
			set { _propRule = value; }
        }

        /// <summary>
        /// The database field name - this allows you to have a 
        /// database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up
        /// </summary>
        public string DatabaseFieldName
        {
            get { return _databaseFieldName; }
			protected set{ _databaseFieldName = value;}
        }

        /// <summary>
        /// The default value that a property of a new object will be set to
        /// </summary>
        public object DefaultValue
        {
            get { return MyDefaultValue; }
			protected set{ MyDefaultValue = value;}
        }

		/// <summary>
		/// The default value that a property of a new object will be set to
		/// </summary>
		public string DefaultValueString
		{
			get { return _defaultValueString; }
			protected set
			{
				if (_defaultValueString != value)
				{
					_defaultValue = null;
				}
				_defaultValueString = value;
			}
		}

		///<summary>
		/// Is this property compulsary or not
		///</summary>
		public bool Compulsory
		{
			get { return _compulsory; }
			protected set { _compulsory = value; }
		}


		/// <summary>
		/// Provides access to read and write the ILookupList object
		/// in this definition
		/// </summary>
		public virtual ILookupList LookupList
		{
			get { return _lookupList; }
			set { _lookupList = value; }
		}

		/// <summary>
		/// Returns the rule for how the property can be accessed. 
		/// See the PropReadWriteRule enumeration for more detail.
		/// </summary>
		public PropReadWriteRule ReadWriteRule
		{
			get { return _propRWStatus; }
			protected set { _propRWStatus = value; }
		}

		/// <summary>
		/// Indicates whether this object has a LookupList object set
		/// </summary>
		/// <returns>Returns true if so, or false if the local
		/// LookupList equates to NullLookupList</returns>
		public bool HasLookupList()
		{
			return (!(_lookupList is NullLookupList));
		}

        /// <summary>
        /// Indicates whether this property is auto-incrementing (from the database)
        /// In this case when the BusinessObject is inserted the field will be filled
        /// from the database field.
        /// </summary>
        public bool AutoIncrementing
        {
            get { return _autoIncrementing; }
        }

        /// <summary>
        /// Returns the maximum length for a string property
        /// </summary>
        public int Length
        {
            get { return _length; }
        }

        ///<summary>
        /// Returns whether this property should keep its value private where possible.
        /// This will usually be set to 'true' for password fields. This will then prevent
        /// the value being revealed in error messages and by default controls the user interface.
        ///</summary>
        public bool KeepValuePrivate
        {
            get { return _keepValuePrivate; }
        }

		#endregion

        
        #region "Rules"

        /// <summary>
        /// Tests whether a specified property value is valid against the current
        /// property rule.  A boolean is returned and an error message,
        /// where appropriate, is stored in a referenced parameter.
        /// </summary>
        /// <param name="propValue">The property value to be tested</param>
        /// <param name="errorMessage">A string which may be amended to reflect
        /// an error message if the value is not valid</param>
        /// <param name="displayName">The name of the property as presented to the user
        /// in the user interface, clarifies error messaging</param>
        /// <returns>Returns true if valid, false if not</returns>
        protected internal bool isValueValid(string displayName, Object propValue, ref string errorMessage)
        {
            if (displayName == null)
            {
                displayName = PropertyName;
            }
            if (_compulsory)
            {
                if (propValue == null
                    || propValue == DBNull.Value
                    || (propValue is string && (string) propValue == String.Empty))
                {
                    errorMessage = String.Format("'{0}' is a compulsory field and has no value.", displayName);
                    return false;
                }
            }

            if (propValue is string && _length != Int32.MaxValue)
            {
                if (((string)propValue).Length > _length)
                {
                    errorMessage = String.Format("'{0}' cannot be longer than {1} characters.", displayName, _length);
                    return false;
                }
            }

            errorMessage = "";
            if (_propRule != null)
            {
                return _propRule.isPropValueValid(displayName, propValue, ref errorMessage);
            }
            else
            {
                return true;
            }
        }

        #endregion

        
        #region "BOProps"

        /// <summary>
        /// Creates a new Business Object property (BOProp)
        /// </summary>
        /// <param name="isNewObject">Whether the Business Object this
        /// property is being created for is a new object or is being 
        /// loaded from the DB. If it is new, then the property is
        /// initialised with the default value.
        /// </param>
        /// <returns>The newly created BO property</returns>
        protected internal BOProp CreateBOProp(bool isNewObject)
        {
            if (isNewObject)
            {
                object defaultValue = MyDefaultValue;
                if (defaultValue != null)
                {
                    //log.Debug("Creating BoProp with default value " + _defaultValue );
                }
                return new BOProp(this, defaultValue);
            }
            else
            {
                return new BOProp(this);
            }
		}

		#endregion //BOProps


		#region "For Testing"

		/// <summary>
        /// Returns the type of the property
        /// </summary>
        protected internal Type PropType
        {
            get { return MyPropertyType; }
			protected set { MyPropertyType = value; }
        }

        #endregion

        
        


        # region PropertyComparer

        /// <summary>
        /// Returns an appropriate IComparer object depending on the
        /// property type.  Can be used, for example, to provide to the
        /// ArrayList.Sort() function in order to determine how to compare
        /// items.  Caters for the following types: String, Int, Guid,
		/// DateTime, Single, Double, TimeSpan 
		/// and anything else that supports IComparable.
        /// </summary>
        /// <returns>Returns an IComparer object, or null if the property
        /// type is not one of those mentioned above</returns>
        public IComparer<T> GetPropertyComparer<T>() where T:IBusinessObject
        {
        	Type comparerType = typeof(PropertyComparer<, >);
        	comparerType = comparerType.MakeGenericType(typeof (T), PropertyType);
			IComparer<T> comparer = (IComparer<T>)Activator.CreateInstance(comparerType, this.PropertyName);
        	return comparer;
			//if (this.PropertyType.Equals(typeof (string)))
			//{
			//    return new StringComparer<T>(this.PropertyName);
			//}
			//else if (this.PropertyType.Equals(typeof (int)))
			//{
			//    return new IntComparer<T>(this.PropertyName);
			//}
			//else if (this.PropertyType.Equals(typeof (Guid)))
			//{
			//    return new GuidComparer<T>(this.PropertyName);
			//}
			//else if (this.PropertyType.Equals(typeof (DateTime)))
			//{
			//    return new DateTimeComparer<T>(this.PropertyName);
			//}
			//else if (this.PropertyType.Equals(typeof (Single)))
			//{
			//    return new SingleComparer<T>(this.PropertyName);
			//}
			//else if (this.PropertyType.Equals(typeof (TimeSpan)))
			//{
			//    return new TimeSpanComparer<T>(this.PropertyName);
			//}
			//return null;
        }

        #endregion //PropertyComparer

		#region Type and Default Value Initialisation

    	private Type MyPropertyType
    	{
			get
			{
				TypeLoader.LoadClassType(ref _propType, _propTypeAssemblyName, _propTypeName,
					"property", "property definition");
				return _propType;
			}
			set
			{
				_propType = value;
				TypeLoader.ClassTypeInfo(_propType, out _propTypeAssemblyName, out _propTypeName);
			}
    	}

		private object MyDefaultValue
		{
			get
			{
				object defaultValue = null;
                if (MyPropertyType == typeof(DateTime) && _defaultValueString != null)
                {
                    switch(_defaultValueString.ToUpper())
                    {
                        case "TODAY":
                            return DateTime.Today;
                            break;
                        case "NOW":
                            return DateTime.Now;
                            break;
                    }
                }
				if (_defaultValue == null && _defaultValueString != null)
				{
					_hasDefaultValueBeenValidated = false;
					if (MyPropertyType == typeof(Guid))
					{
						defaultValue = new Guid(_defaultValueString);
					} else if (MyPropertyType.IsEnum)
					{
					    defaultValue = Enum.Parse(MyPropertyType, _defaultValueString);
					} else 
					{
						try
						{
							defaultValue = Convert.ChangeType(_defaultValueString, MyPropertyType);
						}catch(InvalidCastException ex)
						{
							throw new InvalidCastException(String.Format(
								"The default value '{0}' cannot be cast to " +
								"the property type ({1}).", _defaultValueString, _propTypeName), ex);
						}catch(FormatException ex)
						{
							throw new FormatException( String.Format(
								"The default value '{0}' cannot be converted to " +
								"the property type ({1}).",_defaultValueString,_propTypeName), ex);
						}
					}
				} else
				{
					defaultValue = _defaultValue;
				}
				validateDefaultValue(defaultValue);
				return _defaultValue;
			}
			set
			{
				_hasDefaultValueBeenValidated = false;
				validateDefaultValue(value);
				if (_defaultValue != null)
				{
					_defaultValueString = _defaultValue.ToString();
				} else
				{
					_defaultValueString = null;
				}
			}
		}

        internal string TableName
        {
            get { return ""; }
        }

        ///<summary>
        /// Cdfdasfkl;
        ///</summary>
        public bool Persistable
        {
            get { return _persistable; }
            set { _persistable = value; }
        }


        private void validateDefaultValue(object defaultValue)
    	{
    		if (!_hasDefaultValueBeenValidated)
    		{
    			if ((defaultValue == null) || MyPropertyType.IsInstanceOfType(defaultValue))
    			{
    				_defaultValue = defaultValue;
    				_hasDefaultValueBeenValidated = true;
    			}
    			else
    			{
    				throw new ArgumentException(string.Format(
						"Default value {0} is invalid since it is " +
						"not of type {1}.", defaultValue, _propTypeName), "defaultValue");
    			}
    		}
		}

		#endregion

	}



}
