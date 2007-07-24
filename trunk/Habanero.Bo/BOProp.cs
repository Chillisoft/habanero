using System;
using System.Drawing;
using System.Globalization;
using Habanero.Base.Exceptions;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.CriteriaManager;
using Habanero.DB;
using Habanero.Base;
using Habanero.Util;
using log4net;
using NUnit.Framework;

namespace Habanero.Bo
{
 
    /// <summary>
    /// Stores the object's property value at any given point in time
    /// </summary>
    public class BOProp : IParameterSqlInfo
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.Bo.BOProp");
        protected object _currentValue = null;
        protected PropDef _propDef;
        protected bool _isValid = true;
        protected string _invalidReason = "";
        protected object _persistedValue;
        protected bool _origValueIsValid = true;
        protected string _origInvalidReason = "";
        protected bool _isObjectNew = false;
        protected bool _isDirty = false;

        public event EventHandler<BOPropEventArgs> Updated;

        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="propDef">The property definition</param>
        internal BOProp(PropDef propDef)
        {
            _propDef = propDef;
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
                        try
                        {
                            propValue = new Guid(propValue.ToString());
                        }
                        catch (FormatException)
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
                    else {
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
            _isValid = _propDef.isValueValid(propValue, ref _invalidReason);

            _currentValue = propValue;
            _isObjectNew = isObjectNew;
            //Set up origional properties s.t. property can be backed up and restored.
            _origInvalidReason = _invalidReason;
            _origValueIsValid = _isValid;
            _persistedValue = propValue;
        }

        /// <summary>
        /// Restores the property's original value as defined in PersistedValue
        /// </summary>
        protected internal void RestorePropValue()
        {
            _isValid = _origValueIsValid;
            _invalidReason = _origInvalidReason;
            _currentValue = _persistedValue;
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
                    }
                    _isValid = _propDef.isValueValid(newValue, ref _invalidReason);
                    _currentValue = newValue;
                    FireBOPropValueUpdated();
                    _isDirty = true;
//					if (!_isValid) {
//						throw new InvalidPropertyValueException(_invalidReason);
//					}
                }
            }
        }

        /// <summary>
        /// Calls the Updated() method
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
        internal bool isValid
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
                return this.DatabaseFieldName + " is NULL ";
            }
            else
            {
                if (sql == null)
                {
                    return this.DatabaseFieldName + " = '" + this.PersistedPropertyValueString + "'";
                }
                else
                {
                    string paramName = sql.ParameterNameGenerator.GetNextParameterName();
                    sql.AddParameter(paramName, PersistedPropertyValue);
                    return this.DatabaseFieldName + " = " + paramName;
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
                return this.DatabaseFieldName + " is NULL ";
            }
            else
            {
                if (sql == null)
                {
                    return this.DatabaseFieldName + " = '" + this.PropertyValueString + "'";
                }
                else
                {
                    String paramName = sql.ParameterNameGenerator.GetNextParameterName();
                    sql.AddParameter(paramName, this.Value);
                    return this.DatabaseFieldName + " = " + paramName;
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
    }

}