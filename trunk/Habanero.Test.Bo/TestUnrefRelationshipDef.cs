using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.DB;
using Habanero.Test.General;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Bo
{

    [TestFixture]
    public class TestUnrefRelationshipDef : TestUsingDatabase
    {
        private RelationshipDef mRelationshipDef;
        private RelKeyDef mRelKeyDef;
        private PropDefCol mPropDefCol;
        private MockBO mMockBo;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [SetUp]
        public void init()
        {
            mMockBo = new MockBO();
            mPropDefCol = mMockBo.PropDefCol;

            mRelKeyDef = new RelKeyDef();
            PropDef propDef = mPropDefCol["MockBOID"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "MockBOProp1");
            mRelKeyDef.Add(lRelPropDef);

            mRelationshipDef = new MultipleRelationshipDef("Relation1", typeof(MockBO),
                                                           mRelKeyDef, false, "",
                                                           DeleteParentAction.DeleteRelated);
            DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetConnectionString();
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
            RelationshipDef relDef = new MultipleRelationshipDef("Relation1", typeof(String), mRelKeyDef, false, "",
                                                                 DeleteParentAction.DeleteRelated);
        }

        [Test]
        public void TestCreateRelationship()
        {
            MultipleRelationship rel =
                (MultipleRelationship)mRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);

            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);

            Assert.IsTrue(rel.GetRelatedBusinessObjectCol().Count == 0);

            //			mMockBo.SetPropertyValue("MockBOProp1",mMockBo.GetPropertyValue("MockBOID"));
            //			mMockBo.ApplyEdit();
            //			Assert.IsTrue (rel.HasRelationship(), "Should be true since prop MockBOProp1 has been set");
            //
            //			Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1") ,mMockBo.GetPropertyValue("MockBOID"));
            //TODO:
            //			MockBO ltempBO = (MockBO) rel.GetRelatedObject();
            //			Assert.IsFalse(ltempBO == null);
            //			Assert.AreEqual(mMockBo.GetPropertyValue("MockBOID") ,ltempBO.GetPropertyValue("MockBOID"), "The object returned should be the one with the ID = MockBOID");
            //			Assert.AreEqual(mMockBo.GetPropertyValueString("MockBOProp1") ,ltempBO.GetPropertyValueString("MockBOID"), "The object returned should be the one with the ID = MockBOID");
            //			Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1") ,ltempBO.GetPropertyValue("MockBOID"), "The object returned should be the one with the ID = MockBOID");
        }
    }
}
