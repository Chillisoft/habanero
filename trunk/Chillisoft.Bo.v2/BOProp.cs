using System;
using System.Drawing;
using System.Globalization;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.CriteriaManager.v2;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;
using NUnit.Framework;

namespace Chillisoft.Bo.v2
{
    public delegate void BOPropValueUpdatedHandler(Object sender, EventArgs e);

    /// <summary>
    /// Stores the object's property value at any given point in time
    /// </summary>
    public class BOProp : IParameterSqlInfo
    {
        protected object _currentValue = null;
        protected PropDef _propDef;
        protected bool _isValid = true;
        protected string _invalidReason = "";
        protected object _persistedValue;
        protected bool _origValueIsValid = true;
        protected string _origInvalidReason = "";
        protected bool _isObjectNew = false;
        protected bool _isDirty = false;

        public event BOPropValueUpdatedHandler BOPropValueUpdated;

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
        internal string DataBaseFieldName
        {
            get { return _propDef.DataBaseFieldName; }
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
                else if (this.PropertyType == typeof (TimeSpan) && propValue.GetType() == typeof (DateTime))
                {
                    propValue = ((DateTime) propValue).TimeOfDay;
                }
                else
                {
                    propValue = Convert.ChangeType(propValue, this.PropertyType);
                }
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
        public object PropertyValue
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
//						throw new PropertyValueInvalidException(_invalidReason);
//					}
                }
            }
        }

        /// <summary>
        /// Returns the persisted value (the value assigned at the last
        /// backup or database committal)
        /// </summary>
        public object PersistedValue
        {
            get { return _persistedValue; }
        }

        /// <summary>
        /// Calls the BOPropValueUpdated() method
        /// </summary>
        protected void FireBOPropValueUpdated()
        {
            if (this.BOPropValueUpdated != null)
            {
                BOPropValueUpdated(this, new EventArgs());
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
                    if (!(PropertyValue == DBNull.Value))
                    {
                        return ((DateTime) PersistedPropertyValue).ToString("dd MMM yyyy HH:mm:ss:fff");
                    }
                        //SQL return ((DateTime)PropertyValue).ToString("dd MMM yyyy HH:mm:ss:fff");
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
                        if (!(PropertyValue == DBNull.Value))
                        {
                            return ((DateTime) PropertyValue).ToString("dd MMM yyyy HH:mm:ss:fff");
                        }
                            //SQL return ((DateTime)PropertyValue).ToString("dd MMM yyyy HH:mm:ss:fff");
                        else
                        {
                            return PropertyValue.ToString();
                        }
                    }
                    else if (_propDef.PropType == typeof (Guid))
                    {
                        if (_currentValue is Guid)
                        {
                            return ((Guid) PropertyValue).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            return
                                (new Guid(PropertyValue.ToString())).ToString("B").ToUpper(CultureInfo.InvariantCulture);
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
        /// TODO ERIC - this duplicates PersistedValue
        internal object PersistedPropertyValue
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
        /// TODO ERIC - what does this actually mean?
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
                return this.DataBaseFieldName + " is NULL ";
            }
            else
            {
                if (sql == null)
                {
                    return this.DataBaseFieldName + " = '" + this.PersistedPropertyValueString + "'";
                }
                else
                {
                    string paramName = sql.GetParameterNameGenerator().GetNextParameterName();
                    sql.AddParameter(paramName, PersistedPropertyValue);
                    return this.DataBaseFieldName + " = " + paramName;
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
                return this.DataBaseFieldName + " is NULL ";
            }
            else
            {
                if (sql == null)
                {
                    return this.DataBaseFieldName + " = '" + this.PropertyValueString + "'";
                }
                else
                {
                    String paramName = sql.GetParameterNameGenerator().GetNextParameterName();
                    sql.AddParameter(paramName, this.PropertyValue);
                    return this.DataBaseFieldName + " = " + paramName;
                }
            }
        }

        /// <summary>
        /// Returns the property type
        /// </summary>
        internal Type PropertyType
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
            get { return DataBaseFieldName; }
        }

        /// <summary>
        /// The parameter type to be added to the parameter
        /// </summary>
        public ParameterType ParameterType
        {
            get { return _propDef.ParameterType; }
        }
    }

    #region Testing

    [TestFixture]
    public class PropTester
    {
        private PropDef mPropDef;
        private BOProp mProp;

        [SetUp]
        public void init()
        {
            mPropDef = new PropDef("PropName", typeof (string), cbsPropReadWriteRule.OnlyRead, null);
            mProp = mPropDef.CreateBOProp(false);
        }

        [Test]
        public void TestSetBOPropValue()
        {
            mProp.PropertyValue = "Prop Value";
            Assert.AreEqual("Prop Value", mProp.PropertyValue);
        }

        [Test]
        public void TestRestorePropValue()
        {
            mProp.InitialiseProp("OrigionalValue");
            mProp.PropertyValue = "Prop New Value";
            Assert.AreEqual("Prop New Value", mProp.PropertyValue);
            Assert.IsTrue(mProp.IsDirty);
            Assert.IsTrue(mProp.isValid);
            mProp.RestorePropValue();
            Assert.AreEqual("OrigionalValue", mProp.PropertyValue);
            Assert.IsFalse(mProp.IsDirty);
            Assert.IsTrue(mProp.isValid);
        }

        [Test]
        public void TestPropCompulsoryRestore()
        {
            //Test compulsory with no default set
            PropDef lPropDefWithRules = new PropDef("PropNameWithRules", typeof (string),
                                                    cbsPropReadWriteRule.ReadManyWriteMany, null);
            lPropDefWithRules.assignPropRule(new PropRuleString(lPropDefWithRules.PropertyName, true, -1, -1));
            BOProp lBOProp = lPropDefWithRules.CreateBOProp(true);
            Assert.IsFalse(lBOProp.isValid);
            Assert.IsTrue(lBOProp.InvalidReason.Length > 0);
            lBOProp.PropertyValue = "New Value";
            Assert.IsTrue(lBOProp.isValid);
            Assert.IsFalse(lBOProp.InvalidReason.Length > 0);
            lBOProp.RestorePropValue();
            Assert.IsFalse(lBOProp.isValid);
            Assert.IsTrue(lBOProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropBrokenRuleRestore()
        {
            //Test compulsory with no default set
            PropDef lPropDefWithRules = new PropDef("PropNameWithRules", typeof (string),
                                                    cbsPropReadWriteRule.ReadManyWriteMany, null);
            lPropDefWithRules.assignPropRule(new PropRuleString(lPropDefWithRules.PropertyName, false, 50, 51));
            BOProp lBOProp = lPropDefWithRules.CreateBOProp(true);
            Assert.IsTrue(lBOProp.isValid);
            try
            {
                lBOProp.PropertyValue = "New Value";
            }
            catch (PropertyValueInvalidException)
            {
                //do nothing
            }
            Assert.IsFalse(lBOProp.isValid);
            lBOProp.RestorePropValue();
            Assert.IsTrue(lBOProp.isValid);
        }

        [Test]
        public void TestBackupProp()
        {
            mProp.InitialiseProp("OrigionalValue");
            mProp.PropertyValue = "Prop New Value";
            Assert.AreEqual("Prop New Value", mProp.PropertyValue);
            mProp.BackupPropValue();
            Assert.AreEqual("Prop New Value", mProp.PropertyValue);
            Assert.IsFalse(mProp.IsDirty);
            Assert.IsTrue(mProp.isValid);
        }

        [Test]
        //Test that the proprety is not being set to dirty when the 
            // value is set but has not changed.
            public void TestDirtyProp()
        {
            mProp.InitialiseProp("OrigionalValue");
            mProp.PropertyValue = "OrigionalValue";
            Assert.IsFalse(mProp.IsDirty);
            Assert.IsTrue(mProp.isValid);
        }

        [Test]
        //Test persisted property value is returned correctly.
            public void TestPersistedPropValue()
        {
            mProp.InitialiseProp("OrigionalValue");
            mProp.PropertyValue = "New Value";
            Assert.IsTrue(mProp.IsDirty);
            Assert.AreEqual("OrigionalValue", mProp.PersistedPropertyValue);
            Assert.AreEqual("PropName = 'New Value'", mProp.DatabaseNameFieldNameValuePair(null));
            Assert.AreEqual("PropName = 'OrigionalValue'", mProp.PersistedDatabaseNameFieldNameValuePair(null));
        }

        [Test]
        //Test DirtyXML.
            public void TestDirtyXml()
        {
            mProp.InitialiseProp("OrigionalValue");
            mProp.PropertyValue = "New Value";
            Assert.IsTrue(mProp.IsDirty);
            string dirtyXml = "<" + mProp.PropertyName + "><PreviousValue>OrigionalValue" +
                              "</PreviousValue><NewValue>New Value</NewValue></" +
                              mProp.PropertyName + ">";
            Assert.AreEqual(dirtyXml, mProp.DirtyXml);
        }
    }

    #endregion //Testing
}