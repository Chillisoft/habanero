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
using System.Drawing;
using System.Globalization;
using System.Threading;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.BO.SqlGeneration;
using Habanero.DB;
using Habanero.Base;
using Habanero.Util;
using log4net;

namespace Habanero.BO
{
 
    /// <summary>
    /// Stores the object's property value at any given point in time
    /// </summary>
    public class BOProp : IParameterSqlInfo
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.BOProp");
        protected object _currentValue = null;
        protected PropDef _propDef;
        protected bool _isValid = true;
        protected string _invalidReason = "";
        protected object _persistedValue;
        protected bool _origValueIsValid = true;
        protected string _origInvalidReason = "";
        protected bool _isObjectNew = false;
        protected bool _isDirty = false;
        protected string _displayName = "";
        protected object _valueBeforeLastEdit;

        /// <summary>
        /// Indicates that the value held by the property has been
        /// changed
        /// </summary>
        public event EventHandler<BOPropEventArgs> Updated;

        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="propDef">The property definition</param>
        internal BOProp(PropDef propDef)
        {
            _propDef = propDef;
            _displayName = propDef.DisplayName;
        }

        /// <summary>
        /// Constructor to initialise a new property with a specific value
        /// </summary>
        /// <param name="propDef">The property definition</param>
        /// <param name="propValue">The initial value</param>
        internal BOProp(PropDef propDef, object propValue) : this(propDef)
        {
            InitialiseProp(propValue, true);
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        internal string PropertyName
        {
            get { return _propDef.PropertyName; }
        }

        /// <summary>
        /// Returns the database field name
        /// </summary>
        internal string DatabaseFieldName
        {
            get { return _propDef.DatabaseFieldName; }
        }


        /// <summary>
        /// Initialises the property with the specified value
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        protected internal void InitialiseProp(object propValue)
        {
            InitialiseProp(propValue, false);
        }

        /// <summary>
        /// Initialises the property with the specified value, and indicates
        /// whether the object is new or not
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        /// <param name="isObjectNew">Whether the object is new or not</param>
        private void InitialiseProp(object propValue, bool isObjectNew)
        {
            if (propValue == DBNull.Value)
            {
                propValue = null;
            }
            
            try
            {
                if (propValue != null)
                {
                    if (propValue.GetType().Name == "MySqlDateTime")
                    {
                        if (propValue.ToString().Trim().Length > 0)
                        {
                            propValue = DateTime.Parse(propValue.ToString());
                        }
                        else
                        {
                            propValue = null;
                        }
                    }
                    else if (this.PropertyType == typeof (Guid))
                    {
                    	Guid guidValue;
						if (StringUtilities.GuidTryParse(propValue.ToString(), out guidValue))
						{
							propValue = guidValue;
						} else
						{
							propValue = null;
						}
                    }
                    else if (this.PropertyType == typeof (Image))
                    {
                        propValue = SerialisationUtilities.ByteArrayToObject((byte[]) propValue);
                    }
                    else if (this.PropertyType.IsSubclassOf(typeof (CustomProperty)))
                    {
                        propValue = Activator.CreateInstance(this.PropertyType, new object[] {propValue, true});
                    }
                    else if (this.PropertyType == typeof (Object))
                    {
                        //propValue = propValue;
                    }
                    else if (this.PropertyType == typeof(TimeSpan) && propValue.GetType() == typeof(DateTime))
                    {
                        propValue = ((DateTime)propValue).TimeOfDay;
                    }
                    else if (this.PropertyType.IsEnum && propValue is string)
                    {
                        propValue = Enum.Parse(this.PropertyType, (string)propValue);
                    }
                    else
                    {
                        try {
                            propValue = Convert.ChangeType(propValue, this.PropertyType);
                        }
                        catch (InvalidCastException) {
                            log.Error(
                                string.Format("Problem in InitialiseProp(): Can't convert value of type {0} to {1}",
                                              propValue.GetType().FullName, this.PropertyType.FullName));
                            log.Error(string.Format("Value: {0}, Property: {1}, Field: {2}, Table: {3}", propValue, this._propDef.PropertyName, this._propDef.DatabaseFieldName, _propDef.TableName));
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPropertyValueException(String.Format(
                    "An error occurred while attempting to convert " +
                    "the loaded property value of '{0}' to its specified " +
                    "type of '{1}'. The property value is '{2}'. See log for details",
                    PropertyName, PropertyType, propValue), ex);
            }

            _invalidReason = "";
            _isValid = _propDef.isValueValid(DisplayName, propValue, ref _invalidReason);

            _currentValue = propValue;
            _isObjectNew = isObjectNew;
            //Set up origional properties s.t. property can be backed up and restored.
            BackupPropValue();
            //_origInvalidReason = _invalidReason;
            //_origValueIsValid = _isValid;
            //_persistedValue = propValue;
            //_valueBeforeLastEdit = propValue;
        }

        /// <summary>
        /// Restores the property's original value as defined in PersistedValue
        /// </summary>
        protected internal void RestorePropValue()
        {
            _isValid = _origValueIsValid;
            _invalidReason = _origInvalidReason;
            _currentValue = _persistedValue;
            _valueBeforeLastEdit = _persistedValue;
            _isDirty = false;
            FireBOPropValueUpdated();
        }

        /// <summary>
        /// Copies the current property value to PersistedValue.
        /// This is usually called when the object is persisted
        /// to the database.
        /// </summary>
        protected internal void BackupPropValue()
        {
            _persistedValue = _currentValue;
            _valueBeforeLastEdit = _currentValue;
            _origInvalidReason = _invalidReason;
            _origValueIsValid = _isValid;
            _isDirty = false;
        }

        /// <summary>
        /// Gets and sets the value for this property
        /// </summary>
        public object Value
        {
            get { return _currentValue; }
            set
            {
                if (value is Guid && Guid.Empty.Equals(value))
                {
                    value = null;
                }
                if ((_currentValue == null) || (!_currentValue.Equals(value)))
                {
                    object newValue = null;
                    if (value != null)
                    {
                        try
                        {
                            newValue = Convert.ChangeType(value, this.PropertyType);
                        }
                        catch (InvalidCastException)
                        {
                            newValue = value;
						}
						catch (FormatException)
						{
							if (value is string && String.IsNullOrEmpty((string)value))
							{
								newValue = null;
							} else
							{
								throw;
							}
						}
                    }
                    CheckReadWriteRule(newValue);
                    _isValid = _propDef.isValueValid(DisplayName, newValue, ref _invalidReason);
                    _valueBeforeLastEdit = _currentValue;
                    _currentValue = newValue;
                    FireBOPropValueUpdated();
                    _isDirty = true;
//					if (!_IsValid) {
//						throw new InvalidPropertyValueException(_invalidReason);
//					}
                }
            }
        }

        private void CheckReadWriteRule(object newValue)
        {
            switch (_propDef.ReadWriteRule)
            {                            
                case PropReadWriteRule.ReadWrite:
                    break;
                case PropReadWriteRule.ReadOnly:
                    if (_persistedValue != newValue)
                    {
                        throw new BusinessObjectReadWriteRuleException(_propDef);
                    }
                    break;
                case PropReadWriteRule.WriteOnce:
                    if ((!(_isObjectNew)) && _persistedValue != null && !object.Equals(_persistedValue,newValue))
                    {
                        throw new BusinessObjectReadWriteRuleException(_propDef);
                    }
                    break;
                case PropReadWriteRule.WriteNotNew:
                    if (_isObjectNew && !object.Equals(_persistedValue,newValue))
                    {
                        throw new BusinessObjectReadWriteRuleException(_propDef);
                    }
                    break;
                case PropReadWriteRule.WriteNew:
                    if (!_isObjectNew && !object.Equals(_persistedValue, newValue))
                    {
                        throw new BusinessObjectReadWriteRuleException(_propDef);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets the value held before the value was last updated.
        /// If the object has just been created, this v
        /// </summary>
        public object ValueBeforeLastEdit
        {
            get { return _valueBeforeLastEdit; }
        }

        /// <summary>
        /// Fires an Updated event
        /// </summary>
        protected void FireBOPropValueUpdated()
        {
            if (this.Updated != null)
            {
                Updated(this, new BOPropEventArgs(this));
            }
        }

        /// <summary>
        /// Returns the persisted property value as a string (the value 
        /// assigned at the last backup or database committal)
        /// </summary>
        internal string PersistedPropertyValueString
        {
            get
            {
                if (PersistedPropertyValue == null)
                {
                    return "";
                }
                if (_propDef.PropType == typeof (DateTime))
                {
                    if (!(Value == DBNull.Value))
                    {
                        return ((DateTime) PersistedPropertyValue).ToString("dd MMM yyyy HH:mm:ss:fff");
                    }
                        //Sql return ((DateTime)Value).ToString("dd MMM yyyy HH:mm:ss:fff");
                    else
                    {
                        return PersistedPropertyValue.ToString();
                    }
                }
                else if (_propDef.PropType == typeof (Guid))
                {
                    return ((Guid) PersistedPropertyValue).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                }
                else if ((_propDef.PropType == typeof (String)) && (PersistedPropertyValue is Guid))
                {
                    return ((Guid) PersistedPropertyValue).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                }
                else
                {
                    return PersistedPropertyValue.ToString();
                }
            }
        }

        /// <summary>
        /// Returns the property value as a string
        /// </summary>
        internal string PropertyValueString
        {
            get
            {
                try
                {
                    if (_currentValue == null)
                    {
                        return "";
                    }
                    else if (_propDef.PropType == typeof (DateTime))
                    {
                        if (!(Value == DBNull.Value))
                        {
                            return ((DateTime) Value).ToString("dd MMM yyyy HH:mm:ss:fff");
                        }
                            //Sql return ((DateTime)Value).ToString("dd MMM yyyy HH:mm:ss:fff");
                        else
                        {
                            return Value.ToString();
                        }
                    }
                    else if (_propDef.PropType == typeof (Guid))
                    {
                        if (_currentValue is Guid)
                        {
                            return ((Guid) Value).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            return
                                (new Guid(Value.ToString())).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                        }
                    }
                    else if ((_propDef.PropType == typeof (String)) && (_currentValue is Guid))
                    {
                        return ((Guid) _currentValue).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return _currentValue.ToString();
                    }
                }
                catch (Exception exc)
                {
                    throw new HabaneroApplicationException(
                        exc.Message + "/nError occured for Property " + _propDef.PropertyName,
                        exc);
                }
            }
        }

        /// <summary>
        /// Returns the persisted property value in its object form
        /// </summary>
        public object PersistedPropertyValue
        {
            get { return _persistedValue; }
        }

        /// <summary>
        /// Indicates whether the property value is valid
        /// </summary>
        internal bool IsValid
        {
            get { return _isValid; }
        }

        /// <summary>
        /// Returns a string which indicates why the property value may
        /// be invalid
        /// </summary>
        internal string InvalidReason
        {
            get { return _invalidReason; }
        }

        /// <summary>
        /// Indicates whether the property's value has been changed since
        /// it was last backed up or committed to the database
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirty; }
        }

        /// <summary>
        /// Indicates whether the object is new
        /// </summary>
        internal bool IsObjectNew
        {
            get { return _isObjectNew; }
            set { _isObjectNew = value; }
        }

        /// <summary>
        /// Returns a string containing the database field name and the 
        /// persisted value, in the format of:<br/>
        /// "[fieldname] = '[value]'" (eg. "children = '2'")<br/>
        /// If a sql statement is provided, then the arguments are added
        /// in parameterised form.
        /// </summary>
        /// <param name="sql">A sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        internal string PersistedDatabaseNameFieldNameValuePair(SqlStatement sql)
        {
            if (PersistedPropertyValue == null)
            {
                return SqlGenerationHelper.FormatFieldName(DatabaseFieldName, sql.Connection) + " is NULL ";
            }
            else
            {
                if (sql == null)
                {
                    return DatabaseFieldName + " = '" + PersistedPropertyValueString + "'";
                }
                else
                {
                    string paramName = sql.ParameterNameGenerator.GetNextParameterName();
                    sql.AddParameter(paramName, PersistedPropertyValue);
                    return SqlGenerationHelper.FormatFieldName(DatabaseFieldName, sql.Connection) + " = " + paramName;
                }
            }
        }

        /// <summary>
        /// Returns a string containing the database field name and the 
        /// property value, in the format of:<br/>
        /// "[fieldname] = '[value]'" (eg. "children = '2'")<br/>
        /// If a sql statement is provided, then the arguments are added
        /// in parameterised form.
        /// </summary>
        /// <param name="sql">A sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        internal string DatabaseNameFieldNameValuePair(SqlStatement sql)
        {
            if (_currentValue == null)
            {
                return SqlGenerationHelper.FormatFieldName(DatabaseFieldName, sql.Connection) + " is NULL ";
            }
            else
            {
                if (sql == null)
                {
                    return DatabaseFieldName + " = '" + PropertyValueString + "'";
                }
                else
                {
                    String paramName = sql.ParameterNameGenerator.GetNextParameterName();
                    sql.AddParameter(paramName, Value);
                    return SqlGenerationHelper.FormatFieldName(DatabaseFieldName, sql.Connection) + " = " + paramName;
                }
            }
        }

        /// <summary>
        /// Returns the property type
        /// </summary>
        public Type PropertyType
        {
            get { return _propDef.PropType; }
        }

        /// <summary>
        /// Returns an XML string to describe changes between the property
        /// value and the persisted value.  It consists of an element with the 
        /// property name, containing "PreviousValue" and "NewValue" elements
        /// </summary>
        internal string DirtyXml
        {
            get
            {
                return "<" + PropertyName + "><PreviousValue>" + PersistedPropertyValueString +
                       "</PreviousValue><NewValue>" + PropertyValueString + "</NewValue></" +
                       PropertyName + ">";
            }
        }

        /// <summary>
        /// The name in the expression tree to be updated
        /// </summary>
        public string ParameterName
        {
            get { return PropertyName; }
        }

        /// <summary>
        /// The table name to be added to the parameter
        /// </summary>
        public string TableName
        {
            get { return _propDef.TableName; }
        }

        /// <summary>
        /// The field name to be added to the parameter
        /// </summary>
        public string FieldName
        {
            get { return DatabaseFieldName; }
        }

        /// <summary>
        /// The parameter type to be added to the parameter
        /// </summary>
        public ParameterType ParameterType
        {
            get { return _propDef.ParameterType; }
        }

        /// <summary>
        /// The field name as given to the user in the user interface
        /// (eg. "Computer Part" rather than "ComputerPartID").  This
        /// property is used to improve error messaging, so that the
        /// user recognises the property name as displayed to them,
        /// rather than as it is represented in the code.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (HasDisplayName())
                {
                    return _displayName;
                }
                else
                {
                    return StringUtilities.DelimitPascalCase(PropertyName, " ");
                }
            }
            set
            {
                string newDisplayName = value ?? "";
                if (newDisplayName.EndsWith(":") || newDisplayName.EndsWith("?"))
                {
                    newDisplayName = newDisplayName.Substring(0, newDisplayName.Length - 1);
                }
                if (_displayName != newDisplayName)
                {
                    if (_invalidReason.Contains(String.Format("'{0}'", DisplayName)))
                    {
                        _invalidReason = _invalidReason.Replace(
                            String.Format("'{0}'", DisplayName),
                            String.Format("'{0}'", newDisplayName));
                    }
                    _displayName = newDisplayName;
                }
            }
        }

        ///<summary>
        /// Does the business object property have a specified display name or not.
        ///</summary>
        ///<returns>True if a display name has been set for this property, otherwise false.</returns>
        public bool HasDisplayName()
        {
            return !String.IsNullOrEmpty(_displayName);
        }

    }

}