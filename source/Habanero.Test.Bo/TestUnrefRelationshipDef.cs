//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO
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
            //			mMockBo.Save();
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

		[Test]
		public void TestCreateRelationshipUsingGenerics()
		{
			MultipleRelationship rel =
				(MultipleRelationship)mRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
			Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);

			Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);

			BusinessObjectCollection<MockBO> relatedObjects;
			relatedObjects = rel.GetRelatedBusinessObjectCol<MockBO>();
			Assert.IsTrue(relatedObjects.Count == 0);

			//			mMockBo.SetPropertyValue("MockBOProp1",mMockBo.GetPropertyValue("MockBOID"));
			//			mMockBo.Save();
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
