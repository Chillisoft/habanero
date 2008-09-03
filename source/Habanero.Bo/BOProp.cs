//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using log4net;

namespace Habanero.BO
{
    ///<summary>
    /// Stores the values (current Value, DatabaseValue etc) and 
    ///  state (dirty, valid) of a property of a <see cref="IBusinessObject"/>.
    /// Has a reference to the Property Definition <see cref="PropDef"/> that was used to create it.
    /// The Property definition includes property rules and validation functionality.
    /// The property of a business object may represent a property such as FirstName, Surname.
    /// Typically a <see cref="IBusinessObject"/> will have a collection of Properties.
    ///</summary>
    public class BOProp : IBOProp
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.BOProp");
        protected object _currentValue;
        protected bool _isDirty;
        protected bool _isValid = true;

        protected PropDef _propDef;

        protected string _invalidReason = "";
        protected object _persistedValue;
        protected bool _origValueIsValid = true;
        protected string _origInvalidReason = "";
        protected bool _isObjectNew;
        protected object _valueBeforeLastEdit;
        private IBOPropAuthorisation _boPropAuthorisation = null;
        /// <summary>
        /// Indicates that the value held by the property has been
        /// changed. This is fired any time that the current value of the property is set to a new value.
        /// </summary>
        public event EventHandler<BOPropEventArgs> Updated;

        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="propDef">The property definition</param>
        public BOProp(IPropDef propDef)
        {
            _propDef = (PropDef) propDef;
        }

        /// <summary>
        /// Constructor to initialise a new property with a specific value
        /// </summary>
        /// <param name="propDef">The property definition</param>
        /// <param name="propValue">The initial value</param>
        internal BOProp(IPropDef propDef, object propValue) : this(propDef)
        {
            InitialiseProp(propValue, true);
        }
        
        ///<summary>
        /// The property definition of the property that this BOProp represents.
        ///</summary>
        public IPropDef PropDef
        {
            get { return _propDef; }
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        public string PropertyName
        {
            get { return _propDef.PropertyName; }
        }

        /// <summary>
        /// Returns the database field name
        /// </summary>
        public string DatabaseFieldName
        {
            get { return _propDef.DatabaseFieldName; }
        }

        /// <summary>
        /// Initialises the property with the specified value
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        public void InitialiseProp(object propValue)
        {
            InitialiseProp(propValue, false);
        }

        /// <summary>
        /// Initialises the property with the specified value, and indicates
        /// whether the object is new or not
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        /// <param name="isObjectNew">Whether the object is new or not</param>
        public void InitialiseProp(object propValue, bool isObjectNew)
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
                    else if (this.PropertyType == typeof(string))
                    {
                        if (propValue is Guid)
                        {
                            propValue = ((Guid) propValue).ToString("B");
                        }
                        else
                        {
                            propValue = propValue.ToString();
                        }
                    }
                    else
                    {
                        try
                        {
                            propValue = Convert.ChangeType(propValue, this.PropertyType);
                        }
                        catch (InvalidCastException)
                        {
                            log.Error(
                                string.Format("Problem in InitialiseProp(): Can't convert value of type {0} to {1}",
                                              propValue.GetType().FullName, this.PropertyType.FullName));
                            log.Error(string.Format("Value: {0}, Property: {1}, Field: {2}, Table: {3}", propValue, this._propDef.PropertyName, this._propDef.DatabaseFieldName, this._propDef.ClassDef.GetTableName(_propDef)));
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
            _isValid = _propDef.IsValueValid(propValue, ref _invalidReason);

            _currentValue = propValue;
            _isObjectNew = isObjectNew;
            //Set up origional properties s.t. property can be backed up and restored.
            BackupPropValue();
        }

        /// <summary>
        /// Restores the property's original value as defined in PersistedValue
        /// </summary>
        public void RestorePropValue()
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
        public void BackupPropValue()
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
                if ((_currentValue != null) && (_currentValue.Equals(value))) return;
                object newValue = null;
                if (value != null)
                {
                    newValue = ((PropDef)this.PropDef).GetNewValue(value);
                }

                if (!Equals(_persistedValue, newValue))
                {
                    string message;
                    if (!IsEditable(out message))
                    {
                        throw new BusinessObjectReadWriteRuleException(_propDef);
                    }
                }

                _invalidReason = "";
                _isValid = _propDef.IsValueValid(newValue, ref _invalidReason);
                _valueBeforeLastEdit = _currentValue;
                _currentValue = newValue;
                FireBOPropValueUpdated();
                _isDirty = true;
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
                    if ((!(_isObjectNew)) && _persistedValue != null && !Equals(_persistedValue,newValue))
                    {
                        throw new BusinessObjectReadWriteRuleException(_propDef);
                    }
                    break;
                case PropReadWriteRule.WriteNotNew:
                    if (_isObjectNew && !Equals(_persistedValue,newValue))
                    {
                        throw new BusinessObjectReadWriteRuleException(_propDef);
                    }
                    break;
                case PropReadWriteRule.WriteNew:
                    if (!_isObjectNew && !Equals(_persistedValue, newValue))
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
        public string PersistedPropertyValueString
        {
            get
            {
                if (PersistedPropertyValue == null)
                {
                    return "";
                }
                if (_propDef.PropType == typeof (DateTime))
                {
                    return (Value == DBNull.Value) 
                        ? PersistedPropertyValue.ToString() 
                        : ((DateTime) PersistedPropertyValue).ToString("dd MMM yyyy HH:mm:ss:fff");
                    //Sql return ((DateTime)Value).ToString("dd MMM yyyy HH:mm:ss:fff");
                }
                if (_propDef.PropType == typeof (Guid))
                {
                    return ((Guid) PersistedPropertyValue).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                }
                if ((_propDef.PropType == typeof (String)) && (PersistedPropertyValue is Guid))
                {
                    return ((Guid) PersistedPropertyValue).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                }
                return PersistedPropertyValue.ToString();
            }
        }

        /// <summary>
        /// Returns the property value as a string
        /// </summary>
        public string PropertyValueString
        {
            get
            {
                try
                {
                    if (_currentValue == null)
                    {
                        return "";
                    }
                    if (_propDef.PropType == typeof (DateTime))
                    {
                        return (Value == DBNull.Value) 
                            ? Value.ToString() 
                            : ((DateTime) Value).ToString("dd MMM yyyy HH:mm:ss:fff");
                    }
                    if (_propDef.PropType == typeof (Guid))
                    {
                        if (_currentValue is Guid)
                        {
                            return ((Guid) Value).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                        }
                        return
                            (new Guid(Value.ToString())).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                    }
                    if ((_propDef.PropType == typeof (String)) && (_currentValue is Guid))
                    {
                        return ((Guid) _currentValue).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                    }
                    return _currentValue.ToString();
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
        public bool IsValid
        {
            get { return _isValid; }
        }

        /// <summary>
        /// Returns a string which indicates why the property value may
        /// be invalid
        /// </summary>
        public string InvalidReason
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
        public bool IsObjectNew
        {
            get { return _isObjectNew; }
            set { _isObjectNew = value; }
        }

        /// <summary>
        /// This property returns the 
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
                return SqlFormattingHelper.FormatFieldName(DatabaseFieldName, sql.Connection) + " is NULL ";
            }
            if (sql == null)
            {
                return DatabaseFieldName + " = '" + PersistedPropertyValueString + "'";
            }
            string paramName = sql.ParameterNameGenerator.GetNextParameterName();
            sql.AddParameter(paramName, PersistedPropertyValue);
            return SqlFormattingHelper.FormatFieldName(DatabaseFieldName, sql.Connection) + " = " + paramName;
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
                if (sql == null)
                {
                    return DatabaseFieldName + " = '" + PropertyValueString + "'";
                }
                return SqlFormattingHelper.FormatFieldName(DatabaseFieldName, sql.Connection) + " is NULL ";
            }
            if (sql == null)
            {
                return DatabaseFieldName + " = '" + PropertyValueString + "'";
            }
            String paramName = sql.ParameterNameGenerator.GetNextParameterName();
            sql.AddParameter(paramName, Value);
            return SqlFormattingHelper.FormatFieldName(DatabaseFieldName, sql.Connection) + " = " + paramName;
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
        public string DirtyXml
        {
            get
            {
                return "<" + PropertyName + "><PreviousValue>" + PersistedPropertyValueString +
                       "</PreviousValue><NewValue>" + PropertyValueString + "</NewValue></" +
                       PropertyName + ">";
            }
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
            get { return _propDef.DisplayName;}
        }

        /// <summary>
        /// Set the authorisation rule strategy to be used 
        /// </summary>
        /// <param name="boPropAuthorisation"></param>
        internal protected void SetAuthorisationRules(IBOPropAuthorisation boPropAuthorisation)
        {
            _boPropAuthorisation = boPropAuthorisation;
        }

        ///<summary>
        /// Returns whether the BOProperty is Editable or not. The BOProp may not be editable
        ///  based on a number of factors. 
        ///  1) If its ReadWrite Rules are set to ReadOnly.
        /// 
        ///</summary>
        ///<param name="message"></param>
        ///<returns></returns>
        public virtual bool IsEditable(out string message)
        {
            message = "";
            if (!AreReadWriteRulesEditable(out message)) return false;
            if (_boPropAuthorisation == null) return true;
            if (!_boPropAuthorisation.IsAuthorised(BOPropActions.CanUpdate))
            {
                message = string.Format("The logged on user {0} is not authorised to update the {1} ", 
                        Thread.CurrentPrincipal.Identity.Name, this.PropertyName);
                return false;
            }

            return true;
        }

        private bool AreReadWriteRulesEditable(out string message)
        {
            switch (_propDef.ReadWriteRule)
            {
                case PropReadWriteRule.ReadWrite:
                    break;
                case PropReadWriteRule.ReadOnly:
                    message = "The property '" + this.DisplayName + "' is not editable since it is set up as ReadOnly";
                    return false;
                case PropReadWriteRule.WriteOnce:
                    if (_isObjectNew || _persistedValue == null) break;
                    message = "The property '" + this.DisplayName + "' is not editable since it is set up as WriteOnce and the value has already been set";
                    return false;
                case PropReadWriteRule.WriteNotNew:
                    if (_isObjectNew)
                    {
                        message = "The property '" + this.DisplayName +
                                  "' is not editable since it is set up as WriteNew and the object is new";
                        return false;
                    }
                    break;
                case PropReadWriteRule.WriteNew:
                    if (!_isObjectNew)
                    {
                        message = "The property '" + this.DisplayName + 
                                  "' is not editable since it is set up as WriteNew and the object is not new";
                        return false;
                    }
                    break;
                default:
                    break;
            }
            message = "";
            return true;
        }
    }

}