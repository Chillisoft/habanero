using System;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;
using Chillisoft.Test;
using Chillisoft.Util.v2;
using NUnit.Framework;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// Defines a relationship where the owner relates to only one other object.
    /// </summary>
    public class SingleRelationshipDef : RelationshipDef
    {
        /// <summary>
        /// Constructor to create a new single relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectClassType">The class type of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object</param>
        /// TODO ERIC - review last param
        public SingleRelationshipDef(string relationshipName, Type relatedObjectClassType, RelKeyDef relKeyDef,
                                     bool keepReferenceToRelatedObject)
            : base(relationshipName, relatedObjectClassType, relKeyDef, keepReferenceToRelatedObject)
        {
        }

        /// <summary>
        /// Overrides abstract method of RelationshipDef to create a new
        /// relationship
        /// </summary>
        /// <param name="owningBo">The business object that will manage
        /// this relationship</param>
        /// <param name="lBOPropCol">The collection of properties</param>
        /// <returns></returns>
        internal override Relationship CreateRelationship(BusinessObjectBase owningBo, BOPropCol lBOPropCol)
        {
            return new SingleRelationship(owningBo, this, lBOPropCol);
        }
    }

    #region testing

    [TestFixture]
    public class RelationshipDefTester : TestUsingDatabase
    {
        #region MockBO For Testing

        private RelationshipDef mRelationshipDef;
        private RelKeyDef mRelKeyDef;
        private PropDefCol mPropDefCol;
        private MockBO mMockBo;


        [TestFixtureSetUp]
        public void init()
        {
            this.SetupDBConnection();
            mMockBo = new MockBO();
            mPropDefCol = mMockBo.PropDefCol;

            mRelKeyDef = new RelKeyDef();
            PropDef propDef = mPropDefCol["MockBOProp1"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "MockBOID");
            mRelKeyDef.Add(lRelPropDef);

            mRelationshipDef = new SingleRelationshipDef("Relation1", typeof (MockBO), mRelKeyDef, false);
            //DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetConnectionString();
        }

        [Test]
        public void TestCreateRelationshipDef()
        {
            Assert.AreEqual("Relation1", mRelationshipDef.RelationshipName);
            Assert.AreEqual(typeof (MockBO), mRelationshipDef.RelatedObjectClassType);
            Assert.AreEqual(mRelKeyDef, mRelationshipDef.RelKeyDef);
        }

        [Test]
        [ExpectedException(typeof (HabaneroArgumentException))]
        public void TestCreateRelationshipWithNonBOType()
        {
            RelationshipDef relDef = new SingleRelationshipDef("Relation1", typeof (String), mRelKeyDef, false);
        }

        [Test]
        public void TestCreateRelationship()
        {
            SingleRelationship rel = (SingleRelationship) mRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelationship(), "Should be false since props are not defaulted in Mock bo");
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            mMockBo.ApplyEdit();
            Assert.IsTrue(rel.HasRelationship(), "Should be true since prop MockBOProp1 has been set");

            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), mMockBo.GetPropertyValue("MockBOID"));
            MockBO ltempBO = (MockBO) rel.GetRelatedObject(DatabaseConnection.CurrentConnection);
            Assert.IsFalse(ltempBO == null);
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOID"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValueString("MockBOProp1"), ltempBO.GetPropertyValueString("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
        }
    }

    internal class MockBO : BusinessObjectBase
    {
        public MockBO() : base()
        {
        }

        public MockBO(ClassDef def) : base(def)
        {
        }

        public static MockBO Create()
        {
            return (MockBO) ClassDef.GetClassDefCol[typeof (MockBO)].CreateNewBusinessObject();
        }


        protected static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (MockBO)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.GetClassDefCol[typeof (MockBO)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["MockBOID"]);
            ClassDef lClassDef = new ClassDef(typeof (MockBO), primaryKey, lPropDefCol, keysCol, null);
            return lClassDef;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("MockBOProp1", typeof (Guid), cbsPropReadWriteRule.ReadManyWriteMany, "MockBOProp1", null);
            lPropDefCol.Add(propDef);

            lPropDefCol.Add("MockBOProp2", typeof (string), cbsPropReadWriteRule.ReadManyWriteOnce, "MockBOProp2", null);

            propDef =
                lPropDefCol.Add("MockBOID", typeof (Guid), cbsPropReadWriteRule.ReadManyWriteOnce, "MockBOID", null);
            return lPropDefCol;
        }

        #region forTesting

        internal PropDefCol PropDefCol
        {
            get { return mClassDef.PropDefcol; }
        }

        internal BOPropCol PropCol
        {
            get { return mBOPropCol; }
        }

        #endregion //For Testing
    }

    #endregion

    #endregion //Testing
}