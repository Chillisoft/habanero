using Chillisoft.Bo.v2;
using Chillisoft.Util.v2;
using NUnit.Framework;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// The property definition of the key being related to in a
    /// relationship between objects
    /// </summary>
    /// TODO ERIC - review
    public class RelPropDef
    {
        protected PropDef mOwnerPropDef;
        protected string mRelatedClassPropName;

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
            mOwnerPropDef = ownerClassPropDef;
            mRelatedClassPropName = relatedObjectPropName;
        }

        /// <summary>
        /// The property definition name of the owner object
        /// </summary>
        public string OwnerPropertyName
        {
            get { return mOwnerPropDef.PropertyName; }
        }

        /// <summary>
        /// The property name of the related class object
        /// </summary>
        /// TODO ERIC - may need clarification
        public string RelatedClassPropName
        {
            get { return mRelatedClassPropName; }
        }

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
            PropDef propDef = new PropDef("Prop", typeof (string), cbsPropReadWriteRule.ReadManyWriteMany, null);
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