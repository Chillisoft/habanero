using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Bo
{


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

            mRelationshipDef = new SingleRelationshipDef("Relation1", typeof(MockBO), mRelKeyDef, false);
            //DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetConnectionString();
        }

        [Test]
        public void TestCreateRelationshipDef()
        {
            Assert.AreEqual("Relation1", mRelationshipDef.RelationshipName);
            Assert.AreEqual(typeof(MockBO), mRelationshipDef.RelatedObjectClassType);
            Assert.AreEqual(mRelKeyDef, mRelationshipDef.RelKeyDef);
        }

        [Test]
        [ExpectedException(typeof(HabaneroArgumentException))]
        public void TestCreateRelationshipWithNonBOType()
        {
            RelationshipDef relDef = new SingleRelationshipDef("Relation1", typeof(String), mRelKeyDef, false);
        }

        [Test]
        public void TestCreateRelationship()
        {
            SingleRelationship rel = (SingleRelationship)mRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelationship(), "Should be false since props are not defaulted in Mock bo");
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            mMockBo.ApplyEdit();
            Assert.IsTrue(rel.HasRelationship(), "Should be true since prop MockBOProp1 has been set");

            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), mMockBo.GetPropertyValue("MockBOID"));
            MockBO ltempBO = (MockBO)rel.GetRelatedObject(DatabaseConnection.CurrentConnection);
            Assert.IsFalse(ltempBO == null);
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOID"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValueString("MockBOProp1"), ltempBO.GetPropertyValueString("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
        }
    }

    internal class MockBO : BusinessObject
    {
        public MockBO()
            : base()
        {
        }

        public MockBO(ClassDef def)
            : base(def)
        {
        }

        public static MockBO Create()
        {
            return (MockBO)ClassDef.ClassDefs[typeof(MockBO)].CreateNewBusinessObject();
        }


        protected static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof(MockBO)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof(MockBO)];
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
            ClassDef lClassDef = new ClassDef(typeof(MockBO), primaryKey, lPropDefCol, keysCol, null);
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("MockBOProp1", typeof(Guid), PropReadWriteRule.ReadWrite, "MockBOProp1", null);
            lPropDefCol.Add(propDef);

            lPropDefCol.Add("MockBOProp2", typeof(string), PropReadWriteRule.WriteOnce, "MockBOProp2", null);

            propDef =
                lPropDefCol.Add("MockBOID", typeof(Guid), PropReadWriteRule.WriteOnce, "MockBOID", null);
            return lPropDefCol;
        }

        #region forTesting

        internal PropDefCol PropDefCol
        {
            get { return _classDef.PropDefcol; }
        }

        internal BOPropCol PropCol
        {
            get { return _boPropCol; }
        }

        #endregion //For Testing
    }

        #endregion
}
