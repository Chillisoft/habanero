//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.Comparer;
using Habanero.Util;

//using log4net;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// A PropDef contains a Business Object property definition, with
    /// the property name and information such as the 
    /// access rules for the property (i.e. write-once, read-many or 
    /// read-many-write-many).
    /// Stores the <see cref="IPropRule"/>s for the Property Definition.
    /// Contains functionality to determine whether a value is valid based on the 
    /// Property rules.
    /// </summary>
    /// <futureEnhancements>
    /// TODO_Future:
    /// <ul>
    /// <li>Add ability to store calculated properties.</li>
    /// <li>Lazy initialisation of properties.</li>
    /// <li>Property definitions that reference properties from related Business objects 
    ///     e.g. An Invoice Line has a property definition that references InvoiceDate through its
    ///     relationship Invoice.</li>
    /// </ul>
    /// </futureEnhancements>
    public class PropDef : IPropDef
    {
//        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.ClassDefinition.PropDef");
        private string _propertyName;
        private string _description;
        private Type _propType;
        private PropReadWriteRule _propRWStatus;

        private string _propTypeAssemblyName;
        private string _propTypeName;
        private object _defaultValue;
        private string _defaultValueString;
        private bool _hasDefaultValueBeenValidated;
        private bool _compulsory;
        private readonly List<IPropRule> _propRules = new List<IPropRule>();


        private string _databaseFieldName; //This allows you to have a 
        //database field name different from your property name. 
        //We have customers whose standard for naming database 
        //fields is DATABASE_FIELD_NAME. 
        //This is also powerful for migrating systems 
        //where the database has already been set up.

        private ILookupList _lookupList = new NullLookupList();

        private bool _autoIncrementing;
        private int _length;
        private string _displayName;
        private bool _keepValuePrivate;
        private bool _persistable = true;
        private ClassDef _classDef;
        private string _unitOfMeasure = "";
        private BOPropDataMapper _propDataMapper;

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
        public PropDef
            (string propertyName, Type propType, PropReadWriteRule propRWStatus, string databaseFieldName,
             object defaultValue)
            : this(propertyName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null)
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
        public PropDef(string propertyName, Type propType, PropReadWriteRule propRWStatus, object defaultValue)
            : this(propertyName, propType, null, null, propRWStatus, null, defaultValue, null)
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
        public PropDef
            (string propertyName, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing)
            : this(
                propertyName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValueString,
                compulsory, autoIncrementing)
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
        internal PropDef
            (string propertyName, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing, int length)
            : this(
                propertyName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValueString,
                compulsory, autoIncrementing, length)
        {
        }

//        /// <summary>
//        /// This constructor is used to create a propdef using property type assembly and class name and other information. 
//        /// The default value and the property type are loaded when they are needed.
//        /// </summary>
//        /// <param name="propertyName">The name of the property (e.g. "surname")</param>
//        /// <param name="assemblyName">The assembly name of the property type</param>
//        /// <param name="typeName">The type name of the property type (e.g. "string")</param>
//        /// <param name="propRWStatus">Rules for how a property can be accessed.
//        /// See PropReadWriteRule enumeration for more detail.</param>
//        /// <param name="databaseFieldName">The database field name - this
//        /// allows you to have a database field name that is different to the
//        /// property name, which is useful for migrating systems where
//        /// the database has already been set up.</param>
//        /// <param name="defaultValueString">The default value that a property 
//        /// of a new object will be set to</param>
//        /// <param name="compulsory">Whether this property is a required field or not.</param>
//        /// <param name="autoIncrementing">Whether this is an auto-incrementing field in the database</param>
//        /// <param name="length">The maximum length for a string</param>
//        /// <param name="displayName">The display name for the property</param>
//        /// <param name="description">The description of the property</param>
//        public PropDef
//            (string propertyName, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
//             string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing, int length,
//             string displayName, string description)
//            : this(
//                propertyName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValueString,
//                compulsory, autoIncrementing, length, displayName, description)
//        {
//        }




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
        public PropDef
            (string propertyName, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing, int length,
             string displayName, string description, bool keepValuePrivate)
            : this(
                propertyName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValueString,
                compulsory, autoIncrementing, length, displayName, description, keepValuePrivate)
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
        public PropDef
            (string propertyName, Type propType, PropReadWriteRule propRWStatus, string databaseFieldName,
             object defaultValue, bool compulsory, bool autoIncrementing, int length, string displayName,
             string description)
            : this(
                propertyName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null, compulsory,
                autoIncrementing, length, displayName, description)
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
        public PropDef
            (string propertyName, Type propType, PropReadWriteRule propRWStatus, string databaseFieldName,
             object defaultValue, bool compulsory, bool autoIncrementing)
            : this(
                propertyName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null, compulsory,
                autoIncrementing)
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
        internal PropDef
            (string propertyName, Type propType, PropReadWriteRule propRWStatus, string databaseFieldName,
             object defaultValue, bool compulsory, bool autoIncrementing, int length, string displayName,
             string description, bool keepValuePrivate)
            : this(
                propertyName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null, compulsory,
                autoIncrementing, length, displayName, description, keepValuePrivate)
        {
        }

        #endregion //Progressive Public Constructors

        #region Progressive Private Constructors

        private PropDef
            (string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, object defaultValue, string defaultValueString)
            : this(
                propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue,
                defaultValueString, false)
        {
        }

        private PropDef
            (string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, object defaultValue, string defaultValueString, bool compulsory)
            : this(
                propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue,
                defaultValueString, compulsory, false)
        {
        }

        private PropDef
            (string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, object defaultValue, string defaultValueString, bool compulsory,
             bool autoIncrementing)
            : this(
                propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue,
                defaultValueString, compulsory, autoIncrementing, int.MaxValue)
        {
        }

        private PropDef
            (string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, object defaultValue, string defaultValueString, bool compulsory,
             bool autoIncrementing, int length)
            : this(
                propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue,
                defaultValueString, compulsory, autoIncrementing, length, "")
        {
        }

        private PropDef
            (string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, object defaultValue, string defaultValueString, bool compulsory,
             bool autoIncrementing, int length, string displayName)
            : this(
                propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue,
                defaultValueString, compulsory, autoIncrementing, length, displayName, "")
        {
        }

        private PropDef
            (string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, object defaultValue, string defaultValueString, bool compulsory,
             bool autoIncrementing, int length, string displayName, string description, bool keepValuePrivate)
        {
            SetupPropDef
                (propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue,
                 defaultValueString, compulsory, autoIncrementing, length, displayName, description, keepValuePrivate);
        }

        private PropDef
            (string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, object defaultValue, string defaultValueString, bool compulsory,
             bool autoIncrementing, int length, string displayName, string description)
            : this(
                propertyName, propType, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValue,
                defaultValueString, compulsory, autoIncrementing, length, displayName, description, false)
        {
        }

        #endregion //Progressive Private Constructors

        private void SetupPropDef
            (string propertyName, Type propType, string assemblyName, string typeName, PropReadWriteRule propRWStatus,
             string databaseFieldName, object defaultValue, string defaultValueString, bool compulsory,
             bool autoIncrementing, int length, string displayName, string description, bool keepValuePrivate)
        {
            ArgumentValidationHelper.CheckStringArgumentNotEmpty
                (propertyName, "propertyName", "This field is compulsary for the PropDef class.");
            if (propertyName.IndexOfAny(new[] {'.', '-', '|'}) != -1)
            {
                throw new ArgumentException
                    ("A property name cannot contain any of the following characters: [.-|]  Invalid property name "
                     + propertyName);
            }
            _propertyName = propertyName;
            if (propType != null)
            {
                MyPropertyType = propType;
            }
            else
            {
                _propTypeAssemblyName = assemblyName;
                _propTypeName = typeName;
            }
            _propRWStatus = propRWStatus;
            _databaseFieldName = databaseFieldName ?? propertyName;
            if (defaultValue != null)
            {
                MyDefaultValue = defaultValue;
            }
            else
            {
                _defaultValueString = defaultValueString;
            }
            _compulsory = compulsory;
            _autoIncrementing = autoIncrementing;
            _length = length;
            _displayName = displayName;
            if (string.IsNullOrEmpty(_displayName))
            {
                _displayName = StringUtilities.DelimitPascalCase(_propertyName, " ");
            }
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
            set { _propertyName = value; }
        }

        ///<summary>
        /// The display name for the property.
        ///</summary>
        public string DisplayName
        {
            get { return _displayName; } //            set { _displayName = value; }
        }

        ///<summary>
        /// The description of the property.
        ///</summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// The name of the property type assembly
        /// </summary>
        public string PropertyTypeAssemblyName
        {
            get { return _propTypeAssemblyName; }
            set
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
            set
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
            set { MyPropertyType = value; }
        }

        /// <summary>
        /// Returns a List of PropRules <see cref="IPropRule"/> for the Property Definition.
        /// </summary>
        public List<IPropRule> PropRules
        {
            get { return _propRules; }
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
            set { _databaseFieldName = value; }
        }

        /// <summary>
        /// The default value that a property of a new object will be set to
        /// </summary>
        public object DefaultValue
        {
            get { return MyDefaultValue; }
            set { MyDefaultValue = value; }
        }

        /// <summary>
        /// The default value that a property of a new object will be set to
        /// </summary>
        public string DefaultValueString
        {
            get { return _defaultValueString; }
            set
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
            set { _compulsory = value; }
        }

        /// <summary>
        /// Provides access to read and write the ILookupList object
        /// in this definition
        /// </summary>
        public virtual ILookupList LookupList
        {
            get { return _lookupList; }
            set
            {
                _lookupList = value;
                _lookupList.PropDef = this;
            }
        }

        /// <summary>
        /// Returns the rule for how the property can be accessed. 
        /// See the PropReadWriteRule enumeration for more detail.
        /// </summary>
        public PropReadWriteRule ReadWriteRule
        {
            get { return _propRWStatus; }
            set { _propRWStatus = value; }
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
            set { _autoIncrementing = value; }
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


        ///<summary>
        /// Gets and sets the class def that this propDef is part of.
        ///</summary>
        public IClassDef ClassDef
        {
            get { return _classDef; }
            internal set { _classDef = (ClassDef) value; }
           
        }

        #endregion

        #region "Rules"

        /// <summary>
        /// Tests whether a specified property value is valid against the current
        /// property rule.  A boolean is returned and an error message,
        /// where appropriate, is stored in a referenced parameter.
        /// </summary>
        /// <param name="propValue">The property value to be tested in the user interface, clarifies error messaging</param>
        /// <param name="errorMessage">A string which may be amended to reflect
        /// an error message if the value is not valid</param>
        /// <returns>Returns true if valid, false if not</returns>
        public bool IsValueValid(object propValue, ref string errorMessage)
        {
            string tmpErrMsg = "";
            errorMessage = "";
            string displayNameFull = this.ClassDef == null ? DisplayName : this.ClassDef.ClassName + "." + DisplayName;
            if (_compulsory)
            {
                if (propValue == null || propValue == DBNull.Value
                    || (propValue is string && (string) propValue == String.Empty))
                {
                    errorMessage = String.Format("'{0}' is a compulsory field and has no value.", displayNameFull);
                    return false;
                }
            }
            //Validate Type
            if (!IsValueValidType(propValue, ref errorMessage))
            {
                return false;
            }
            //Valid Item in list
            if (!IsLookupListItemValid(propValue, ref errorMessage))
            {
                return false;
            }
            //Validate string lengths are less than the maximum length allowable for this propdef.
            //TODO Brett 09 Jan 2009: I think this should be removed since it is one of the rules. I agree with the capability
            //   in firestarter to be able to easily set up a property rule that has a maximum length but it should not be a seperate
            //   property on prop def.
            if (propValue is string && _length != Int32.MaxValue)
            {
                if (((string) propValue).Length > _length)
                {
                    errorMessage = String.Format("'{0}' cannot be longer than {1} characters.", displayNameFull, _length);
                    return false;
                }
            }
            
            bool valid = true;
            foreach (IPropRule propRule in _propRules)
            {

                bool tmpValid = (propRule == null
                                 || propRule.IsPropValueValid(displayNameFull, GetNewValue(propValue), ref tmpErrMsg));
                valid = valid & tmpValid;
                errorMessage = StringUtilities.AppendMessage(errorMessage, tmpErrMsg);
            }
            return valid;
        }
        /// <summary>
        /// Is lookup list item with a value of propValue valid if not outs Error message
        /// </summary>
        /// <param name="propValue"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected bool IsLookupListItemValid(object propValue, ref string errorMessage)
        {
            errorMessage = "";
            if (!this.HasLookupList()) return true;
            if (!this.LookupList.LimitToList) return true;

            if (propValue == null || string.IsNullOrEmpty(Convert.ToString(propValue))) return true;

            if (this.LookupList is BusinessObjectLookupList)
            {
                BusinessObjectLookupList list = ((BusinessObjectLookupList)this.LookupList);
               // if (list.Criteria == null) return true;//If there are no criteria then the business object will
                // definitely be in the list since the business object is the right type.

                //if (BusinessObjectManager.Instance.Contains(propValue.ToString()) )
                IBusinessObject businessObject = GetlookupBusinessObjectFromObjectManager(propValue);
                if (businessObject != null)
                {
                    return CheckBusinessObjectMeetsLookupListCriteria(propValue, businessObject, list, ref errorMessage);
                }
            }

            Dictionary<string, string> idValueLookupList = this.LookupList.GetIDValueLookupList();
            if (!idValueLookupList.ContainsKey(ConvertValueToString((propValue))))
            {
                string classNameFull =  this.ClassDef == null? "": this.ClassDef.ClassNameFull;
                errorMessage += String.Format("'{0} - {1}' invalid since '{2}' is not in the lookup list of available values.", classNameFull,  DisplayName, propValue);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checks whether the particular buisness object set to a property for this PropDef
        /// meets the criteria for the <see cref="BusinessObjectLookupList"/>.
        /// </summary>
        /// <param name="propValue"></param>
        /// <param name="businessObject">The Business object that is being checked to see if it matches the criteria</param>
        /// <param name="list">The <see cref="BusinessObjectLookupList"/> that the bo is being compared to</param>
        /// <param name="errorMessage">An error message if the businessObject does not match the criteria</param>
        /// <returns></returns>
        protected bool CheckBusinessObjectMeetsLookupListCriteria(object propValue
                , IBusinessObject businessObject
                , BusinessObjectLookupList list, ref string errorMessage)
        {
            Type type = businessObject.GetType();
            if (type == list.BoType || type.IsSubclassOf(list.BoType))
            {
                if (list.Criteria == null) return true;
                if (list.Criteria.IsMatch(businessObject)) return true;

                string classNameFull = ClassDef == null ? "" : ClassDef.ClassNameFull;
                errorMessage += String.Format("'{0} - {1}' invalid since '{2}' is not in the lookup list of available values.",
                                              classNameFull, DisplayName, propValue);
                return false;
            }
            errorMessage = String.Format("'{0}' for property '{1}' is not valid. ", propValue, DisplayName);
            errorMessage += "The Business object '" + type + "' returned for this ID is not a type of '" + list.BoType + "'.";
            return false;
        }

        internal IBusinessObject GetlookupBusinessObjectFromObjectManager(object propValue)
        {
            if (!this.HasLookupList()) return null;
            if (!(this.LookupList is BusinessObjectLookupList)) return null;
            IBusinessObject businessObject = null;
            BusinessObjectLookupList list = ((BusinessObjectLookupList)this.LookupList);
            if (propValue is Guid && list.LookupBoClassDef.PrimaryKeyDef.IsGuidObjectID)
            {
                lock (BusinessObjectManager.Instance)
                {
                    if (BusinessObjectManager.Instance.Contains((Guid)propValue))
                    {
                        return BusinessObjectManager.Instance[(Guid) propValue];
                    }
                }
            }
            BOPrimaryKey boPrimaryKey = BOPrimaryKey.CreateWithValue((ClassDef) list.LookupBoClassDef, propValue);

            if (boPrimaryKey != null)
            {
//                IBusinessObjectCollection find = BusinessObjectManager.Instance.Find(boPrimaryKey.GetKeyCriteria(), list.LookupBoClassDef.ClassType);
//                if (find.Count > 0) businessObject = find[0];
                IBusinessObject found = BusinessObjectManager.Instance.FindFirst(boPrimaryKey.GetKeyCriteria(), list.LookupBoClassDef.ClassType);
                businessObject = found;
            }
            return businessObject;
        }

        internal object GetNewValue(object value)
        {
            object newValue;
            try
            {
                newValue = Convert.ChangeType(value, this.PropertyType);
            }
            catch (InvalidCastException)
            {
                newValue = GetNewValueOnError(value);
            }
            catch (FormatException)
            {
                newValue = GetNewValueOnError(value);
            }
            return newValue;
        }

        private static object GetNewValueOnError(object value)
        {
            object newValue;
            if (value is string && String.IsNullOrEmpty((string) value))
            {
                newValue = null;
            }
            else
            {
                newValue = value;
            }
            return newValue;
        }

        private bool IsValueValidType(object propValue, ref string errorMessage)
        {
            errorMessage = "";
            if (propValue == null) return true;
            if (propValue is string && string.IsNullOrEmpty((string) propValue)) return true;
            if (this.HasLookupList()) return true;
            Type propertyType = this.PropertyType;
            if (propValue.GetType().IsSubclassOf(propertyType)) return true;
            try
            {
                Convert.ChangeType(propValue, propertyType);
            }
            catch (InvalidCastException)
            {
                if (!(propValue is Guid && propertyType == typeof (string)))
                {
                    errorMessage = GetErrorMessage(propValue,this.DisplayName);
                }
            }
            catch (FormatException)
            {
                errorMessage = GetErrorMessage(propValue, this.DisplayName);
            }
            return string.IsNullOrEmpty(errorMessage);
        }

        private string GetErrorMessage(object propValue, string displayName)
        {
            string errorMessage = String.Format("'{0}' for property '{1}' is not valid. ", propValue, displayName);
            errorMessage += "It is not a type of " + this.PropertyTypeName + ".";
            return errorMessage;
        }

        #endregion

        #region "BOProps"

        /// <summary>
        /// Creates a new Business Object property (BOProp)
        /// </summary>
        /// <param name="assignDefaultValue">Whether the Business Object this
        /// property is being created for is a new object or is being 
        /// loaded from the DB. If it is new, then the property is
        /// initialised with the default value.
        /// </param>
        /// <returns>The newly created BO property</returns>
        public IBOProp CreateBOProp(bool assignDefaultValue)
        {
            if (assignDefaultValue)
            {
                object defaultValue = MyDefaultValue;
                return HasLookupList() ? new BOPropLookupList(this, defaultValue) : new BOProp(this, defaultValue);
            }
              return HasLookupList() ? new BOPropLookupList(this) : new BOProp(this);
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
        public IPropertyComparer<T> GetPropertyComparer<T>() where T : IBusinessObject
        {
            Type comparerType = typeof (PropertyComparer<,>);
            comparerType = comparerType.MakeGenericType(typeof (T), PropertyType);
            IPropertyComparer<T> comparer =
                (IPropertyComparer<T>) Activator.CreateInstance(comparerType, this.PropertyName);
            return comparer;
        }

        #endregion //PropertyComparer

        #region Type and Default Value Initialisation

        private Type MyPropertyType
        {
            get
            {
                if (_propType != null) return _propType;

                Type tempPropType = null;
                TypeLoader.LoadClassType
                    (ref tempPropType, _propTypeAssemblyName, _propTypeName, "property", "property definition");
                MyPropertyType = tempPropType;
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
                //TODO Brett: Remove all this code move to mapper.
                object defaultValue;
                if (MyPropertyType == typeof (DateTime) && _defaultValueString != null)
                {
                    return _defaultValueString;
                    //switch (_defaultValueString.ToUpper())
                    //{
                    //    case "TODAY":
                    //        return DateTime.Today;
                    //    case "NOW":
                    //        return DateTime.Now;
                    //}
                }
                if (_defaultValue == null && _defaultValueString != null)
                {
                    _hasDefaultValueBeenValidated = false;
                    if (MyPropertyType == typeof (Guid))
                    {
                        defaultValue = new Guid(_defaultValueString);
                    }
                    else if (MyPropertyType.IsEnum)
                    {
                        defaultValue = Enum.Parse(MyPropertyType, _defaultValueString);
                    }
                    else
                    {
                        try
                        {
                            defaultValue = Convert.ChangeType(_defaultValueString, MyPropertyType);
                        }
                        catch (InvalidCastException ex)
                        {
                            throw new InvalidCastException
                                (String.Format
                                     ("The default value '{0}' cannot be cast to " + "the property type ({1}).",
                                      _defaultValueString, _propTypeName), ex);
                        }
                        catch (FormatException ex)
                        {
                            throw new FormatException
                                (String.Format
                                     ("The default value '{0}' cannot be converted to " + "the property type ({1}).",
                                      _defaultValueString, _propTypeName), ex);
                        }
                    }
                }
                else
                {
                    defaultValue = _defaultValue;
                }
                ValidateDefaultValue(defaultValue);
                return _defaultValue;
            }
            set
            {
                _hasDefaultValueBeenValidated = false;
                ValidateDefaultValue(value);
                _defaultValueString = _defaultValue != null ? _defaultValue.ToString() : null;
            }
        }


        ///<summary>
        /// Is this property persistable or not. This is used for special properties e.g. Dynamically inserted properties
        /// as for Asset Management System (See Intermap Asset Management) or for any reflective/calculated field that 
        /// you would like to store propdef information for e.g. rules, Units of measure etc.
        /// This will prevent the property from being persisted in the usual manner.
        ///</summary>
        public bool Persistable
        {
            get { return _persistable; }
            set { _persistable = value; }
        }

        ///<summary>
        /// The unit of measure that this property is recorded in. e.g. Weight might be recorded in Kg. Capacity in Litre, m^3 etc
        ///</summary>
        public string UnitOfMeasure
        {
            get { return _unitOfMeasure; }
            set { _unitOfMeasure = value; }
        }

        ///<summary>
        /// Returns the full display name for a property definition.
        /// If there is a unit of measure then it is appended to the display name in brackets e.g. DisplayName (UOM).
        /// If there is no display name then it will return the PascalCase Delimited property Name i.e. Display Name.
        ///</summary>
        public string DisplayNameFull
        {
            get
            {
                string displayName = this.DisplayName;
                if (string.IsNullOrEmpty(displayName))
                    displayName = StringUtilities.DelimitPascalCase(_propertyName, " ");
                if (!string.IsNullOrEmpty(this.UnitOfMeasure))
                {
                    return displayName + " (" + this.UnitOfMeasure + ")";
                }
                return displayName;
            }
        }

        private void ValidateDefaultValue(object defaultValue)
        {
            if (_hasDefaultValueBeenValidated) return;

            if ((defaultValue == null) || MyPropertyType.IsInstanceOfType(defaultValue))
            {
                _defaultValue = defaultValue;
                _hasDefaultValueBeenValidated = true;
            }
            else
            {
                throw new ArgumentException
                    (string.Format
                         ("Default value {0} is invalid since it is " + "not of type {1}.", defaultValue,
                          _propTypeName), "defaultValue");
            }
        }

        #endregion

        ///<summary>
        /// Adds an <see cref="IPropRule"/> to the <see cref="PropRules"/> for the 
        /// Property Definiton.
        ///</summary>
        ///<param name="rule">The new rules to be added for the Property Definition.</param>
        public void AddPropRule(IPropRule rule)
        {
            if (rule == null)
                throw new HabaneroApplicationException("You cannot add a null property rule to a property def");
            _propRules.Add(rule);
        }

        ///<summary>
        /// Makes a shallow clone of this property definition (i.e. the clone includes a list of all the
        ///  property rules but the property rules have not been cloned
        ///</summary>
        ///<returns></returns>
        public IPropDef Clone()
        {
            PropDef propDef = new PropDef
                (PropertyName, PropertyTypeAssemblyName, PropertyTypeName, ReadWriteRule,
                 DatabaseFieldName, DefaultValueString, Compulsory, AutoIncrementing, Length,
                 DisplayName, Description, KeepValuePrivate)
                {
                    LookupList = LookupList,
                    Persistable = Persistable,
                    DefaultValue = DefaultValue,
                    PropertyTypeAssemblyName = PropertyTypeAssemblyName,
                    PropType = PropType
                };
            foreach (IPropRule rule in _propRules)
            {
                propDef.AddPropRule(rule);
            }
            return propDef;
        }

        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        ///<exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (PropDef)) return false;
            return Equals((PropDef) obj);
        }

        ///<summary>
        /// Returns true if the PropDef obj is equal to this propDef
        ///</summary>
        ///<param name="obj">The PropDef to be compared to</param>
        ///<returns></returns>
        public bool Equals(PropDef obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj._propertyName, _propertyName) && Equals(obj._description, _description)
                   && Equals(obj._propType, _propType) && Equals(obj._propRWStatus, _propRWStatus)
                   && Equals(obj._propTypeAssemblyName, _propTypeAssemblyName)
                   && Equals(obj._defaultValue, _defaultValue) && Equals(obj._defaultValueString, _defaultValueString)
                   && obj._compulsory.Equals(_compulsory) && Equals(obj._databaseFieldName, _databaseFieldName)
                   && Equals(obj._lookupList, _lookupList) && obj._autoIncrementing.Equals(_autoIncrementing)
                   && obj._length == _length && Equals(obj._displayName, _displayName)
                   && obj._keepValuePrivate.Equals(_keepValuePrivate) && obj._persistable.Equals(_persistable)
                   && Equals(obj._classDef, _classDef) && Equals(obj._unitOfMeasure, _unitOfMeasure);
        }

        ///<summary>
        ///Serves as a hash function for a particular type. 
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object" />.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = (_propertyName != null ? _propertyName.GetHashCode() : 0);
                result = (result * 397) ^ (_description != null ? _description.GetHashCode() : 0);
                result = (result * 397) ^ (_propType != null ? _propType.GetHashCode() : 0);
                result = (result * 397) ^ _propRWStatus.GetHashCode();
                result = (result * 397) ^ (_propTypeAssemblyName != null ? _propTypeAssemblyName.GetHashCode() : 0);
                result = (result * 397) ^ (_defaultValue != null ? _defaultValue.GetHashCode() : 0);
                result = (result * 397) ^ (_defaultValueString != null ? _defaultValueString.GetHashCode() : 0);
                result = (result * 397) ^ _compulsory.GetHashCode();
                result = (result * 397) ^ (_databaseFieldName != null ? _databaseFieldName.GetHashCode() : 0);
                result = (result * 397) ^ (_lookupList != null ? _lookupList.GetHashCode() : 0);
                result = (result * 397) ^ _autoIncrementing.GetHashCode();
                result = (result * 397) ^ _length;
                result = (result * 397) ^ (_displayName != null ? _displayName.GetHashCode() : 0);
                result = (result * 397) ^ _keepValuePrivate.GetHashCode();
                result = (result * 397) ^ _persistable.GetHashCode();
                result = (result * 397) ^ (_classDef != null ? _classDef.GetHashCode() : 0);
                result = (result * 397) ^ (_unitOfMeasure != null ? _unitOfMeasure.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// 
        /// This method provides a the functionality to convert any object to the appropriate
        ///   type for the particular BOProp Type. e.g it will convert a valid guid string to 
        ///   a valid Guid Object.
        /// </summary>
        /// <param name="valueToParse">The value to be converted</param>
        /// <param name="returnValue"></param>
        /// <returns>An object of the correct type.</returns>
        public bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            _propDataMapper = CreateDataMapper();
            return _propDataMapper.TryParsePropValue(valueToParse, out returnValue);
        }

        /// <summary>
        /// Converts the value of a valid type for this property definition to a string relevant.
        /// A null value will be oonverted to a zero length string.
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <returns>The converted string.</returns>
        public string ConvertValueToString(object value)
        {
            CreateDataMapper();

            return _propDataMapper == null ? value.ToString() : _propDataMapper.ConvertValueToString(value);
        }

        private BOPropDataMapper CreateDataMapper()
        {
            if (_propDataMapper != null) return _propDataMapper;

            if (this.PropertyType == typeof(Guid))
            {
                _propDataMapper = new BOPropGuidDataMapper();
            }
            else if (this.PropertyType == typeof(DateTime))
            {
                _propDataMapper = new BOPropDateTimeDataMapper();
            }
            else if (this.PropertyType == typeof(string))
            {
                _propDataMapper = new BOPropStringDataMapper();
            }
            else if (this.PropertyType == typeof(bool))
            {
                _propDataMapper = new BOPropBoolDataMapper();
            } else if (this.PropertyType == typeof(int))
            {
                _propDataMapper = new BOPropIntDataMapper();
            }
            if (_propDataMapper == null) _propDataMapper = new BOPropGeneralDataMapper(this);
            return _propDataMapper;
        }

    }


}
