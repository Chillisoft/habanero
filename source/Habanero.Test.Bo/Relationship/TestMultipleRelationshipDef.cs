//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{

    [TestFixture]
    public class TestMultipleRelationshipDef 
    {
        private RelationshipDef _multipleRelationshipDef;
        private RelKeyDef _RelKeyDef;
        private IPropDefCol _propDefCol;
        private MockBO _mockBo;
        private SingleRelationshipDef _singleRelationshipDef;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
        }

        [SetUp]
        public void init()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
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
            IMultipleRelationship rel =
                (IMultipleRelationship)_multipleRelationshipDef.CreateRelationship(_mockBo, _mockBo.PropCol);
            Assert.AreEqual(_multipleRelationshipDef.RelationshipName, rel.RelationshipName);

            Assert.IsTrue(_mockBo.GetPropertyValue("MockBOProp1") == null);

            Assert.AreEqual(0, rel.BusinessObjectCollection.Count );
        }

        [Test]
        public void TestCreateSingleRelationship()
        {
            ISingleRelationship rel =
                (ISingleRelationship)_singleRelationshipDef.CreateRelationship(_mockBo, _mockBo.Props);

            //-------------Execute Test ------------------------
            bool hasRelatedObject = rel.HasRelatedObject();
            //-------------Test Result -------------------------
            Assert.IsTrue(hasRelatedObject);
        }

    }
}
