using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.CriteriaManager.v2;
using NUnit.Framework;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Represents a property in a relationship
    /// </summary>
    /// TODO ERIC - review
    public class RelProp
    {
        private BOProp mBoProp;
        private RelPropDef mRelPropDef;

        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="mRelPropDef">The relationship property definition</param>
        /// <param name="lBoProp">The property</param>
        internal RelProp(RelPropDef mRelPropDef, BOProp lBoProp)
        {
            this.mRelPropDef = mRelPropDef;
            mBoProp = lBoProp;
        }

        /// <summary>
        /// Returns the property name of the relationship owner
        /// </summary>
        internal string OwnerPropertyName
        {
            get { return mRelPropDef.OwnerPropertyName; }
        }

        /// <summary>
        /// Returns the property name of the related object
        /// </summary>
        internal string RelatedClassPropName
        {
            get { return mRelPropDef.RelatedClassPropName; }
        }

        /// <summary>
        /// Indicates if the property is null
        /// </summary>
        internal bool IsNull
        {
            get { return mBoProp == null || mBoProp.PropertyValue == null; }
        }

        /// <summary>
        /// Returns the related property's expression
        /// </summary>
        /// <returns>Returns an IExpression object</returns>
        internal IExpression RelatedPropExpression()
        {
            if (mBoProp.PropertyValue == null)
            {
                return new Parameter(mRelPropDef.RelatedClassPropName, "IS", "NULL");
            }
            return new Parameter(mRelPropDef.RelatedClassPropName, "=", mBoProp.PropertyValueString);
        }
    }

    #region Tests

    [TestFixture]
    public class RelPropTester
    {
        private RelPropDef mRelPropDef;
        private PropDefCol mPropDefCol;

        [SetUp]
        public void init()
        {
            PropDef propDef = new PropDef("Prop", typeof (string), cbsPropReadWriteRule.ReadManyWriteMany, null);
            mRelPropDef = new RelPropDef(propDef, "PropName");
            mPropDefCol = new PropDefCol();
            mPropDefCol.Add(propDef);
        }

        [Test]
        public void TestCreateRelProp()
        {
            BOPropCol propCol = mPropDefCol.CreateBOPropertyCol(true);
            RelProp relProp = mRelPropDef.CreateRelProp(propCol);

            Assert.AreEqual("Prop", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName", relProp.RelatedClassPropName);

            Assert.IsTrue(relProp.IsNull);
        }

        [Test]
        public void TestCreateRelPropNotNull()
        {
            PropDef propDef = new PropDef("Prop1", typeof (string), cbsPropReadWriteRule.ReadManyWriteMany, "1");
            RelPropDef relPropDef = new RelPropDef(propDef, "PropName1");
            PropDefCol propDefCol = new PropDefCol();

            propDefCol.Add(propDef);
            BOPropCol propCol = propDefCol.CreateBOPropertyCol(true);
            RelProp relProp = relPropDef.CreateRelProp(propCol);

            Assert.AreEqual(relPropDef.OwnerPropertyName, relProp.OwnerPropertyName);
            Assert.AreEqual(relPropDef.RelatedClassPropName, relProp.RelatedClassPropName);

            Assert.IsFalse(relProp.IsNull);
        }
    }

    #endregion //Tests
}