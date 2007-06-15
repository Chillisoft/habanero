using System;
using System.Collections;
using Chillisoft.Bo.CriteriaManager.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;
using log4net;
using NUnit.Framework;
using StringComparer=Chillisoft.Util.v2.StringComparer;

namespace Chillisoft.Bo.ClassDefinition.v2
{

    /// <summary>
    /// An enumeration used to specify different file access modes.
    /// </summary>
    public enum PropReadWriteRule
    {
        /// <summary>Full access</summary>
        ReadManyWriteMany,
        /// <summary>Read but not write/edit</summary>
        ReadOnly,
        /// <summary>Write/edit but not read from</summary>
        WriteOnly,
        /// <summary>Can only be edited when the object is new</summary>
        ReadManyWriteNew,
        /// <summary>Can only be edited it if was never edited before 
        /// (regardless of whether the object is new or not)</summary>
        ReadManyWriteOnce
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
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.ClassDefinition.v2.PropDef");
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
        private ILookupListSource _lookupListSource = new NullLookupListSource();

        
        #region "Constuctor and destructors"

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
							this(propName, propType, null, null, propRWStatus, databaseFieldName, defaultValue, null)
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
			:this(propName, propType,null,null, propRWStatus, null, defaultValue, null)
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
		public PropDef(string propName,
					string assemblyName, string typeName,
					PropReadWriteRule propRWStatus,
					string databaseFieldName,
					string defaultValue) 
			:this(propName, null, assemblyName, typeName, propRWStatus, databaseFieldName, null, defaultValue)
		{
		}

		private PropDef(string propName,
					   Type propType, string assemblyName, string typeName,
					   PropReadWriteRule propRWStatus,
					   string databaseFieldName,
					   object defaultValue, string defaultValueString)
		{
			if (propName.IndexOfAny(new char[] { '.', '-', '|' }) != -1)
			{
				throw new ArgumentException(
					"A property name cannot contain any of the following characters: [.-|]  Invalid property name " +
					propName);
			}
			ArgumentValidationHelper.CheckStringArgumentNotEmpty(propName, "propName","This field is compulsary for the PropDef class.");
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
		}

		#endregion


		#region "properties"

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
		protected string PropertyTypeAssemblyName
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
		protected string PropertyTypeName
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
			protected set { MyPropertyType = value; }
        }

        /// <summary>
        /// The property rule relevant to this definition
        /// </summary>
        public PropRuleBase PropRule
        {
            get { return _propRule; }
			protected set{ _propRule = value;}
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
		
		#endregion

        
        #region "Rules"

        /// <summary>
        /// Assigns the relevant property rule to this Property Definition.
        /// </summary>
        /// <param name="lPropRule">A rule of type PropRuleBase</param>
        public void assignPropRule(PropRuleBase lPropRule)
        {
            _propRule = lPropRule;
        }

        /// <summary>
        /// Tests whether a specified property value is valid against the current
        /// property rule.  A boolean is returned and an error message,
        /// where appropriate, is stored in a referenced parameter.
        /// </summary>
        /// <param name="propValue">The property value to be tested</param>
        /// <param name="errorMessage">A string which may be amended to reflect
        /// an error message if the value is not valid</param>
        /// <returns>Returns true if valid, false if not</returns>
        protected internal bool isValueValid(Object propValue,
                                             ref string errorMessage)
        {
            errorMessage = "";
            if (!(_propRule == null))
            {
                return _propRule.isPropValueValid(propValue, ref errorMessage);
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
        /// TODO ERIC: what is this?
        protected internal Type PropType
        {
            get { return MyPropertyType; }
			protected set { MyPropertyType = value; }
        }

        #endregion

        
        #region IParameterSQLInfo Members

        /// <summary>
        /// Returns the parameter type (typically either DateTime or String)
        /// </summary>
        /// TODO ERIC: can elaborate on this and parameter name
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

        /// <summary>
        /// Provides access to read and write the ILookupListSource object
        /// in this definition
        /// </summary>
        public ILookupListSource LookupListSource
        {
            get { return _lookupListSource; }
            set { _lookupListSource = value; }
        }

        /// <summary>
        /// Returns the rule for how the property can be accessed. 
        /// See the PropReadWriteRule enumeration for more detail.
        /// </summary>
        public PropReadWriteRule ReadWriteRule
        {
            get { return _propRWStatus; }
			protected set{ _propRWStatus = value;}
        }

        /// <summary>
        /// Indicates whether this object has a LookupList object set
        /// </summary>
        /// <returns>Returns true if so, or false if the local
        /// LookupListSource equates to NullLookupListSource</returns>
        public bool HasLookupList()
        {
            return (!(_lookupListSource is NullLookupListSource));
        }

        #endregion


        # region PropertyComparer

        /// <summary>
        /// Returns an appropriate IComparer object depending on the
        /// property type.  Can be used, for example, to provide to the
        /// ArrayList.Sort() function in order to determine how to compare
        /// items.  Caters for the following types: String, Int, Guid,
        /// DateTime, Single and TimeSpan.
        /// </summary>
        /// <returns>Returns an IComparer object, or null if the property
        /// type is not one of those mentioned above</returns>
        public IComparer GetPropertyComparer()
        {
            if (this.PropertyType.Equals(typeof (string)))
            {
                return new StringComparer(this.PropertyName);
            }
            else if (this.PropertyType.Equals(typeof (int)))
            {
                return new IntComparer(this.PropertyName);
            }
            else if (this.PropertyType.Equals(typeof (Guid)))
            {
                return new GuidComparer(this.PropertyName);
            }
            else if (this.PropertyType.Equals(typeof (DateTime)))
            {
                return new DateTimeComparer(this.PropertyName);
            }
            else if (this.PropertyType.Equals(typeof (Single)))
            {
                return new SingleComparer(this.PropertyName);
            }
            else if (this.PropertyType.Equals(typeof (TimeSpan)))
            {
                return new TimeSpanComparer(this.PropertyName);
            }
            return null;
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


    #region Tests

    [TestFixture]
    public class PropDefTester
    {
        private PropDef _propDef;

        [SetUp]
        public void Init()
        {
            _propDef = new PropDef("PropName", typeof (string), PropReadWriteRule.ReadOnly, null);
        }

        [Test]
        public void TestCreatePropDef()
        {
            Assert.AreEqual("PropName", _propDef.PropertyName);
            Assert.AreEqual("PropName", _propDef.DatabaseFieldName);
            Assert.AreEqual(typeof (string), _propDef.PropType);
            PropDef lPropDef = new PropDef("prop", typeof (int), PropReadWriteRule.ReadManyWriteMany, 1);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreatePropDefInvalidDefault()
        {
            PropDef lPropDef = new PropDef("prop", typeof (int), PropReadWriteRule.ReadManyWriteMany, "");
        }

		[Test]
		public void TestCreateLatePropDefInvalidTypeNotAccessed()
		{
			PropDef lPropDef = new PropDef("prop", "NonExistentAssembly", "NonExistentType", PropReadWriteRule.ReadManyWriteMany, null, "");
		}

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreatePropDefInvalidDefault2()
        {
            PropDef lPropDef = new PropDef("prop", typeof (string), PropReadWriteRule.ReadManyWriteMany, 1);
        }

		[Test]
		[ExpectedException(typeof(UnknownTypeNameException))]
		public void TestCreateLatePropDefInvalidType()
		{
			PropDef propDef = new PropDef("prop", "NonExistentAssembly", "NonExistentType", PropReadWriteRule.ReadManyWriteMany, null, "");
			Type propType = propDef.PropertyType;
			Assert.Fail("This line should not be reached because the previous line should have failed.");
		}

		[Test]
		public void TestCreateLatePropDefInvalidDefaultNotAccessed()
		{
			PropDef lPropDef = new PropDef("prop", "System", "Int32", PropReadWriteRule.ReadManyWriteMany, null, "");
			//No error
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void TestCreateLatePropDefInvalidDefault()
		{
			PropDef lPropDef = new PropDef("prop", "System", "Int32", PropReadWriteRule.ReadManyWriteMany, null, "");
			object defaultValue = lPropDef.DefaultValue;
			Assert.Fail("This line should not be reached because the previous line should have failed.");
		}

		public void TestCreateBOProp()
        {
            BOProp prop = _propDef.CreateBOProp(false);
            Assert.AreEqual("PropName", prop.PropertyName);
            Assert.AreEqual("PropName", prop.DatabaseFieldName);
        }
    }

    #endregion //Tests

}
