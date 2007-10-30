//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO.Comparer;
using Habanero.BO.CriteriaManager;
using Habanero.BO;
using Habanero.Base;
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
        WriteOnce
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
    public class PropDef : IParameterSqlInfo
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.ClassDefinition.PropDef");
        private string _propName;
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

        #region Constuctor and destructors

        /// <summary>
        /// This constructor is used to create a propdef using it's property type and other information. 
        /// </summary>
        /// <param name="propName">The name of the property (e.g. "surname")</param>
        /// <param name="propType">The type of the property (e.g. string)</param>
        /// <param name="propRWStatus">Rules for how a property can be accessed.
		/// See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up.</param>
        /// <param name="defaultValue">The default value that a property 
        /// of a new object will be set to</param>
        public PropDef(string propName,
                       Type propType,
                       PropReadWriteRule propRWStatus,
                       string databaseFieldName,
                       object defaultValue) :
							this(propName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null, false, false)
        {
        }

		/// <summary>
		/// This constructor is used to create a propdef using it's property type and other information. 
		/// The database field name is presumed to be the same as the property name.
		/// </summary>
		/// <param name="propName">The name of the property (e.g. "surname")</param>
		/// <param name="propType">The type of the property (e.g. string)</param>
		/// <param name="propRWStatus">Rules for how a property can be accessed.
		/// See PropReadWriteRule enumeration for more detail.</param>
		/// <param name="defaultValue">The default value that a property 
		/// of a new object will be set to</param>
        public PropDef(string propName,
                       Type propType,
                       PropReadWriteRule propRWStatus,
                       object defaultValue) 
			:this(propName, propType,null,null, propRWStatus, null, defaultValue, null, false, false)
        {
        }

		/// <summary>
		/// This constructor is used to create a propdef using property type assembly and class name and other information. 
		/// The default value and the property type are loaded when they are needed.
		/// </summary>
		/// <param name="propName">The name of the property (e.g. "surname")</param>
		/// <param name="assemblyName">The assembly name of the property type</param>
		/// <param name="typeName">The type name of the property type (e.g. "string")</param>
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
		public PropDef(string propName,
					string assemblyName, string typeName,
					PropReadWriteRule propRWStatus,
					string databaseFieldName,
					string defaultValue, 
                    bool compulsory, bool autoIncrementing)
            : this(propName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValue, compulsory, autoIncrementing)
		{
		}

		private PropDef(string propName,
					   Type propType, string assemblyName, string typeName,
					   PropReadWriteRule propRWStatus,
					   string databaseFieldName,
					   object defaultValue, string defaultValueString, bool compulsory, bool autoIncrementing)
		{			
			ArgumentValidationHelper.CheckStringArgumentNotEmpty(propName, "propName","This field is compulsary for the PropDef class.");
			if (propName.IndexOfAny(new char[] { '.', '-', '|' }) != -1)
			{
				throw new ArgumentException(
					"A property name cannot contain any of the following characters: [.-|]  Invalid property name " +
					propName);
			}
			_propName = propName;
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
				_databaseFieldName = propName;
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
		}

		#endregion


		#region Properties

		/// <summary>
        /// The name of the property, e.g. surname
        /// </summary>
        public string PropertyName
        {
            get { return _propName; }
			protected set{ _propName = value;}
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
        protected internal string DatabaseFieldName
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
        /// <returns>Returns true if valid, false if not</returns>
        protected internal bool isValueValid(Object propValue, ref string errorMessage)
        {
            if (_compulsory)
            {
                if (propValue == null
                    || propValue == DBNull.Value
                    || (propValue is string && (string) propValue == String.Empty))
                {
                    errorMessage = String.Format("'{0}' is a compulsory field and has no value.", PropertyName);
                    return false;
                }
            }

            errorMessage = "";
            if (_propRule != null)
            {
                return _propRule.isPropValueValid(PropertyName, propValue, ref errorMessage);
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
                if (MyDefaultValue != null)
                {
                    //log.Debug("Creating BoProp with default value " + _defaultValue );
                }
				return new BOProp(this, MyDefaultValue);
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

        
        #region IParameterSqlInfo Members

        /// <summary>
        /// Returns the parameter type (typically either DateTime or String)
        /// </summary>
        public ParameterType ParameterType
        {
            get
            {
                if (MyPropertyType == typeof (DateTime))
                {
                    return ParameterType.Date;
                }
                else
                {
                    return ParameterType.String;
                }
            }
        }

        /// <summary>
        /// Returns the database field name
        /// </summary>
        public string FieldName
        {
            get { return _databaseFieldName; }
        }

        /// <summary>
        /// Returns the parameter name
        /// </summary>
        public string ParameterName
        {
            get { return _propName; }
        }

        /// <summary>
        /// Returns an empty string
        /// </summary>
        public string TableName
        {
            get { return ""; }
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
        public IComparer<T> GetPropertyComparer<T>() where T:BusinessObject
        {
        	Type comparerType = typeof(PropertyComparer<,>);
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
				if (_defaultValue == null && _defaultValueString != null)
				{
					_hasDefaultValueBeenValidated = false;
					if (MyPropertyType == typeof(Guid))
					{
						defaultValue = new Guid(_defaultValueString);
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
