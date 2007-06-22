using Habanero.Bo;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// The property definition of the key being related to in a
    /// relationship between objects
    /// </summary>
    /// TODO ERIC - review
    public class RelPropDef
    {
        private PropDef _ownerPropDef;
		private string _relatedClassPropName;

		#region Constructors

		/// <summary>
        /// Constructor to create new RelPropDef object
        /// </summary>
        /// <param name="ownerClassPropDef">The property definition of the 
        /// owner object</param>
        /// <param name="relatedObjectPropName">The property name of the 
        /// related object</param>
        public RelPropDef(PropDef ownerClassPropDef,
                          string relatedObjectPropName)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(ownerClassPropDef, "ownerClassPropDef");
            _ownerPropDef = ownerClassPropDef;
            _relatedClassPropName = relatedObjectPropName;
		}

		#endregion Constructors

		#region Properties

    	///<summary>
    	/// Gets or sets the Owner Property Def
    	///</summary>
    	protected PropDef OwnerProperty
    	{
			get { return _ownerPropDef; }
			set
			{
				ArgumentValidationHelper.CheckArgumentNotNull(value, "value");
				_ownerPropDef = value;
			}
    	}

		/// <summary>
        /// The property definition name of the owner object
        /// </summary>
        public string OwnerPropertyName
        {
            get { return _ownerPropDef.PropertyName; }
        }

        /// <summary>
        /// The property name of the related class object
        /// </summary>
        /// TODO ERIC - may need clarification
        public string RelatedClassPropName
        {
			get { return _relatedClassPropName; }
			protected set { _relatedClassPropName = value; }
		}

		#endregion Properties
		
		/// <summary>
        /// Creates a new RelProp object based on this property definition
        /// </summary>
        /// <param name="lBoPropCol">The collection of properties</param>
        /// <returns>The newly created RelProp object</returns>
        protected internal RelProp CreateRelProp(BOPropCol lBoPropCol)
        {
            BOProp lBoProp = lBoPropCol[OwnerPropertyName];
            return new RelProp(this, lBoProp);
        }
    }


    #region Tests

    [TestFixture]
    public class RelPropDefTester
    {
        private RelPropDef mRelPropDef;
        private PropDefCol mPropDefCol;

        [SetUp]
        public void init()
        {
            PropDef propDef = new PropDef("Prop", typeof (string), PropReadWriteRule.ReadManyWriteMany, null);
            mRelPropDef = new RelPropDef(propDef, "PropName");
            mPropDefCol = new PropDefCol();
            mPropDefCol.Add(propDef);
        }

        [Test]
        public void TestCreateRelPropDef()
        {
            Assert.AreEqual("Prop", mRelPropDef.OwnerPropertyName);
            Assert.AreEqual("PropName", mRelPropDef.RelatedClassPropName);
        }

        [Test]
        public void TestCreateRelProp()
        {
            BOPropCol propCol = mPropDefCol.CreateBOPropertyCol(true);
            RelProp relProp = mRelPropDef.CreateRelProp(propCol);

            Assert.AreEqual("Prop", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName", relProp.RelatedClassPropName);
        }
    }

    #endregion //Test

}