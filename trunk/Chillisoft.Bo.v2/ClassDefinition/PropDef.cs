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
    public enum cbsPropReadWriteRule
    {
        /// <summary>Full access</summary>
        ReadManyWriteMany,
        /// <summary>Read but not write/edit</summary>
        OnlyRead,
        /// <summary>Write/edit but not read from</summary>
        OnlyWrite,
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
    /// <li>TODO ERIC: Rename OnlyRead/Write to Read/WriteOnly?</li>
    /// </ul>
    /// </futureEnhancements>
    public class PropDef : IParameterSqlInfo
    {
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.ClassDefinition.v2.PropDef");
        protected string mPropName;
        protected Type mPropType;
        protected cbsPropReadWriteRule mpropRWStatus;
        protected string mDatabaseFieldName; //This allows you to have a 
            //database field name different from your property name. 
            //We have customers whose standard for naming database 
            //fields is DATABASE_FIELD_NAME. 
            //This is also powerful for migrating systems 
            //where the database has already been set up.
        protected readonly object mDefaultValue = null;
        private PropRuleBase mPropRule;
        private ILookupListSource itsLookupListSource = new NullLookupListSource();

        
        #region "Constuctor and destructors"

        /// <summary>
        /// This constructor is used when you do not want to set the 
        /// property without any rules or property controls. This should seldom be 
        /// used, except in the case of calculated fields that are not 
        /// updated to the database (i.e. isPersistable = false)
        /// </summary>
        /// <param name="propName">The name of the property (e.g. surname)</param>
        /// <param name="propType">The type of the property (e.g. string)</param>
        /// <param name="propRWStatus">Rules for how a property can be
        /// accessed. See cbsPropReadWriteRule enumeration for more detail.
        /// </param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up</param>
        /// <param name="defaultValue">The default value that a property 
        /// of a new object will be set to</param>
        public PropDef(string propName,
                       Type propType,
                       cbsPropReadWriteRule propRWStatus,
                       string databaseFieldName,
                       object defaultValue)
        {
            if (propName.IndexOfAny(new char[] {'.', '-', '|'}) != -1)
            {
                throw new ArgumentException(
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name " +
                    propName);
            }
            mPropName = propName;
            mPropType = propType;
            mpropRWStatus = propRWStatus;
            mDatabaseFieldName = databaseFieldName;
            if ((defaultValue == null) || mPropType.IsInstanceOfType(defaultValue))
            {
                mDefaultValue = defaultValue;
            }
            else
            {
                throw new ArgumentException("default value " + defaultValue +
                                            " is invalid since it is not of type " +
                                            mPropType.ToString(), "defaultValue");
            }
        }

        /// <summary>
        /// A second constructor similar to the other, but assumes that the 
        /// databaseFieldName will be the same as the propName
        /// </summary>
        public PropDef(string propName,
                       Type propType,
                       cbsPropReadWriteRule propRWStatus,
                       object defaultValue) :
                           this(propName, propType, propRWStatus, propName, defaultValue)
        {
        }

        #endregion

        
        #region "properties"

        /// <summary>
        /// The name of the property, e.g. surname
        /// </summary>
        public string PropertyName
        {
            get { return mPropName; }
        }

        /// <summary>
        /// The type of the property, e.g. string
        /// </summary>
        public Type PropertyType
        {
            get { return mPropType; }
        }

        /// <summary>
        /// The property rule relevant to this definition
        /// </summary>
        public PropRuleBase PropRule
        {
            get { return mPropRule; }
        }

        /// <summary>
        /// The database field name - this allows you to have a 
        /// database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up
        /// </summary>
        protected internal string DataBaseFieldName
        {
            get { return mDatabaseFieldName; }
        }

        /// <summary>
        /// The default value that a property of a new object will be set to
        /// </summary>
        public object DefaultValue
        {
            get { return mDefaultValue; }
        }

        #endregion

        
        #region "Rules"

        /// <summary>
        /// Assigns the relevant property rule to this Property Definition.
        /// </summary>
        /// <param name="lPropRule">A rule of type PropRuleBase</param>
        public void assignPropRule(PropRuleBase lPropRule)
        {
            mPropRule = lPropRule;
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
            if (!(mPropRule == null))
            {
                return mPropRule.isPropValueValid(propValue, ref errorMessage);
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
                if (mDefaultValue != null)
                {
                    //log.Debug("Creating BoProp with default value " + mDefaultValue );
                }
                return new BOProp(this, mDefaultValue);
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
            get { return mPropType; }
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
                if (this.mPropType == typeof (DateTime))
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
            get { return mDatabaseFieldName; }
        }

        /// <summary>
        /// Returns the parameter name
        /// </summary>
        public string ParameterName
        {
            get { return mPropName; }
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
            get { return itsLookupListSource; }
            set { itsLookupListSource = value; }
        }

        /// <summary>
        /// Returns the rule for how the property can be accessed. 
        /// See the cbsPropReadWriteRule enumeration for more detail.
        /// </summary>
        public cbsPropReadWriteRule ReadWriteRule
        {
            get { return mpropRWStatus; }
        }

        /// <summary>
        /// Indicates whether this object has a LookupList object set
        /// </summary>
        /// <returns>Returns true if so, or false if the local
        /// LookupListSource equates to NullLookupListSource</returns>
        public bool HasLookupList()
        {
            return (!(this.itsLookupListSource is NullLookupListSource));
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

    }


    #region Tests

    [TestFixture]
    public class PropDefTester
    {
        private PropDef mpropDef;

        [SetUp]
        public void Init()
        {
            mpropDef = new PropDef("PropName", typeof (string), cbsPropReadWriteRule.OnlyRead, null);
        }

        [Test]
        public void TestCreatePropDef()
        {
            Assert.AreEqual("PropName", mpropDef.PropertyName);
            Assert.AreEqual("PropName", mpropDef.DataBaseFieldName);
            Assert.AreEqual(typeof (string), mpropDef.PropType);
            PropDef lPropDef = new PropDef("prop", typeof (int), cbsPropReadWriteRule.ReadManyWriteMany, 1);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreatePropDefInvalidDefault()
        {
            PropDef lPropDef = new PropDef("prop", typeof (int), cbsPropReadWriteRule.ReadManyWriteMany, "");
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreatePropDefInvalidDefault2()
        {
            PropDef lPropDef = new PropDef("prop", typeof (string), cbsPropReadWriteRule.ReadManyWriteMany, 1);
        }

        public void TestCreateBOProp()
        {
            BOProp prop = mpropDef.CreateBOProp(false);
            Assert.AreEqual("PropName", prop.PropertyName);
            Assert.AreEqual("PropName", prop.DataBaseFieldName);
        }
    }

    #endregion //Tests

}