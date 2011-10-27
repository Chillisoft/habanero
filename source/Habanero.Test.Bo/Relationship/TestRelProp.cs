// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{

    [TestFixture]
    public class TestRelProp
    {
        private RelPropDef mRelPropDef;
        private PropDefCol mPropDefCol;

        [SetUp]
        public void SetUp()
        {
            PropDef propDef = new PropDef("Prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            mRelPropDef = new RelPropDef(propDef, "PropName");
            mPropDefCol = new PropDefCol();
            mPropDefCol.Add(propDef);
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [Test]
        public void TestCreateRelProp()
        {
            IBOPropCol propCol = mPropDefCol.CreateBOPropertyCol(true);
            IRelProp relProp = mRelPropDef.CreateRelProp(propCol);

            Assert.AreEqual("Prop", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName", relProp.RelatedClassPropName);

            Assert.IsTrue(relProp.IsNull);
        }

        [Test]
        public void TestCreateRelPropNotNull()
        {
            PropDef propDef = new PropDef("Prop1", typeof(string), PropReadWriteRule.ReadWrite, "1");
            RelPropDef relPropDef = new RelPropDef(propDef, "PropName1");
            PropDefCol propDefCol = new PropDefCol();

            propDefCol.Add(propDef);
            IBOPropCol propCol = propDefCol.CreateBOPropertyCol(true);
            IRelProp relProp = relPropDef.CreateRelProp(propCol);

            Assert.AreEqual(relPropDef.OwnerPropertyName, relProp.OwnerPropertyName);
            Assert.AreEqual(relPropDef.RelatedClassPropName, relProp.RelatedClassPropName);

            Assert.IsFalse(relProp.IsNull);
        }

        [Test]
        public void Test_UpdateRelatedObject_ShouldRaiseEvent()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            ISingleRelationship relationship = (ISingleRelationship)contactPersonTestBO.Relationships["Organisation"];
            bool eventCalled = false;

            IRelKey key = relationship.RelKey;
            IRelProp relProp = key[0];
            relProp.PropValueUpdated += delegate { eventCalled = true; };
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation").OwningBOHasForeignKey);
            Assert.IsFalse(eventCalled);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = organisationTestBO;
            //---------------Test Result -----------------------
            Assert.IsTrue(eventCalled);
        }
        [Test]
        public void Test_UpdatePropValue_ShouldRaiseEvent()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            ISingleRelationship relationship = (ISingleRelationship)contactPersonTestBO.Relationships["Organisation"];
            bool eventCalled = false;

            IRelKey key = relationship.RelKey;
            IRelProp relProp = key[0];
            IBOProp prop = relProp.BOProp;
            relProp.PropValueUpdated += delegate { eventCalled = true; };
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation").OwningBOHasForeignKey);
            Assert.IsFalse(eventCalled);
            //---------------Execute Test ----------------------
            prop.Value = Guid.NewGuid();
            //---------------Test Result -----------------------
            Assert.IsTrue(eventCalled);
        }
    }
}
