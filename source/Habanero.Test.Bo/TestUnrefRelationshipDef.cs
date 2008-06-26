//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{

    [TestFixture]
    public class TestUnrefRelationshipDef : TestUsingDatabase
    {
        private RelationshipDef _multipleRelationshipDef;
        private RelKeyDef _RelKeyDef;
        private IPropDefCol _propDefCol;
        private MockBO _mockBo;
        private SingleRelationshipDef _singleRelationshipDef;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [SetUp]
        public void init()
        {
            _mockBo = new MockBO();
            _propDefCol = _mockBo.PropDefCol;

            _RelKeyDef = new RelKeyDef();
            IPropDef propDef = _propDefCol["MockBOID"];

            RelPropDef relPropDef = new RelPropDef(propDef, "MockBOProp1");
            _RelKeyDef.Add(relPropDef);

            _multipleRelationshipDef = new MultipleRelationshipDef("Relation1", typeof(MockBO),
                                                       _RelKeyDef, false, "",
                                                       DeleteParentAction.DeleteRelated);


            _singleRelationshipDef = new SingleRelationshipDef("Single", typeof(MockBO),
                                                       _RelKeyDef, false,
                                                       DeleteParentAction.DeleteRelated);
            DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetConnectionString();
        }

        [Test]
        public void TestCreateRelationshipDef()
        {
            Assert.AreEqual("Relation1", _multipleRelationshipDef.RelationshipName);
            Assert.AreEqual(typeof(MockBO), _multipleRelationshipDef.RelatedObjectClassType);
            Assert.AreEqual(_RelKeyDef, _multipleRelationshipDef.RelKeyDef);
        }

        [Test]
        [ExpectedException(typeof(HabaneroArgumentException))]
        public void TestCreateRelationshipWithNonBOType()
        {
            new MultipleRelationshipDef("Relation1", typeof(String), _RelKeyDef, false, "",
                                                                 DeleteParentAction.DeleteRelated);
        }

        [Test]
        public void TestCreateRelationship()
        {
            MultipleRelationship rel =
                (MultipleRelationship)_multipleRelationshipDef.CreateRelationship(_mockBo, _mockBo.PropCol);
            Assert.AreEqual(_multipleRelationshipDef.RelationshipName, rel.RelationshipName);

            Assert.IsTrue(_mockBo.GetPropertyValue("MockBOProp1") == null);

            Assert.IsTrue(rel.GetRelatedBusinessObjectCol().Count == 0);

            //			_mockBo.SetPropertyValue("MockBOProp1",_mockBo.GetPropertyValue("MockBOID"));
            //			_mockBo.Save();
            //			Assert.IsTrue (rel.HasRelationship(), "Should be true since prop MockBOProp1 has been set");
            //
            //			Assert.AreEqual(_mockBo.GetPropertyValue("MockBOProp1") ,_mockBo.GetPropertyValue("MockBOID"));
            //TODO:
            //			MockBO ltempBO = (MockBO) rel.GetRelatedObject();
            //			Assert.IsFalse(ltempBO == null);
            //			Assert.AreEqual(_mockBo.GetPropertyValue("MockBOID") ,ltempBO.GetPropertyValue("MockBOID"), "The object returned should be the one with the ID = MockBOID");
            //			Assert.AreEqual(_mockBo.GetPropertyValueString("MockBOProp1") ,ltempBO.GetPropertyValueString("MockBOID"), "The object returned should be the one with the ID = MockBOID");
            //			Assert.AreEqual(_mockBo.GetPropertyValue("MockBOProp1") ,ltempBO.GetPropertyValue("MockBOID"), "The object returned should be the one with the ID = MockBOID");
        }

		[Test]
		public void TestCreateRelationshipUsingGenerics()
		{
			MultipleRelationship rel =
				(MultipleRelationship)_multipleRelationshipDef.CreateRelationship(_mockBo, _mockBo.PropCol);
			Assert.AreEqual(_multipleRelationshipDef.RelationshipName, rel.RelationshipName);

			Assert.IsTrue(_mockBo.GetPropertyValue("MockBOProp1") == null);

			BusinessObjectCollection<MockBO> relatedObjects;
			relatedObjects = rel.GetRelatedBusinessObjectCol<MockBO>();
			Assert.IsTrue(relatedObjects.Count == 0);

			//			_mockBo.SetPropertyValue("MockBOProp1",_mockBo.GetPropertyValue("MockBOID"));
			//			_mockBo.Save();
			//			Assert.IsTrue (rel.HasRelationship(), "Should be true since prop MockBOProp1 has been set");
			//
			//			Assert.AreEqual(_mockBo.GetPropertyValue("MockBOProp1") ,_mockBo.GetPropertyValue("MockBOID"));
			//TODO:
			//			MockBO ltempBO = (MockBO) rel.GetRelatedObject();
			//			Assert.IsFalse(ltempBO == null);
			//			Assert.AreEqual(_mockBo.GetPropertyValue("MockBOID") ,ltempBO.GetPropertyValue("MockBOID"), "The object returned should be the one with the ID = MockBOID");
			//			Assert.AreEqual(_mockBo.GetPropertyValueString("MockBOProp1") ,ltempBO.GetPropertyValueString("MockBOID"), "The object returned should be the one with the ID = MockBOID");
			//			Assert.AreEqual(_mockBo.GetPropertyValue("MockBOProp1") ,ltempBO.GetPropertyValue("MockBOID"), "The object returned should be the one with the ID = MockBOID");
		}
        [Test]
        public void TestCreateSingleRelationship()
        {
            SingleRelationship rel =
                (SingleRelationship)_singleRelationshipDef.CreateRelationship(_mockBo, _mockBo.Props);

            //-------------Execute Test ------------------------
            IBusinessObjectCollection relatedObjects = rel.GetRelatedBusinessObjectCol();
            //-------------Test Result -------------------------
            Assert.IsTrue(relatedObjects.Count == 0);
        }
        [Test]
        public void TestCreateSingleRelationshipUsingGenerics()
        {

            //---------------Set up test pack-------------------

            SingleRelationship rel =
                (SingleRelationship)_singleRelationshipDef.CreateRelationship(_mockBo, _mockBo.Props);
            //-------------Execute Test ------------------------
            BusinessObjectCollection<MockBO> relatedObjects = rel.GetRelatedBusinessObjectCol<MockBO>();
            //-------------Test Result -------------------------
            Assert.IsTrue(relatedObjects.Count == 0);
        }
    }
}
