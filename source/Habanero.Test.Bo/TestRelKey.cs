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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestRelKey
    {
        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }
        [Test]
        public void Test_Criteria_OneProp()
        {
            //--------------- Set up test pack ------------------
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO myBO = new MyBO();
            MultipleRelationship<MyRelatedBo> relationship = myBO.Relationships.GetMultiple<MyRelatedBo>("MyMultipleRelationship");
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Criteria relCriteria = relationship.RelKey.Criteria;
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("MyBoID = '" + myBO.MyBoID.Value.ToString("B") + "'", relCriteria.ToString());
        }

        [Test]
        public void Test_Criteria_OneProp_NullValue()
        {
            //--------------- Set up test pack ------------------
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyRelatedBo myRelatedBo = new MyRelatedBo();
            SingleRelationship<MyBO> relationship = myRelatedBo.Relationships.GetSingle<MyBO>("MyRelationship");
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Criteria relCriteria = relationship.RelKey.Criteria;
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("MyBoID IS NULL", relCriteria.ToString());
        }

        [Test]
        public void Test_Criteria_EmptyRelKey()
        {
            //--------------- Set up test pack ------------------
            BOPropCol propCol = new BOPropCol();
            RelKeyDef relKeyDef = new RelKeyDef();
            RelKey relKey = new RelKey(relKeyDef, propCol);
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Criteria relCriteria = relKey.Criteria;
            //--------------- Test Result -----------------------
            Assert.IsNull(relCriteria);
        }

        [Test]
        public void Test_Criteria_CompositeKey()
        {
            //--------------- Set up test pack ------------------
            new Car();
            ContactPersonCompositeKey person = new ContactPersonCompositeKey();
            person.PK1Prop1 = TestUtil.GetRandomString();
            person.PK1Prop2 = TestUtil.GetRandomString();
            MultipleRelationship<Car> relationship = person.Relationships.GetMultiple<Car>("Driver");
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Criteria relCriteria = relationship.RelKey.Criteria;
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(string.Format("(DriverFK1 = '{0}') AND (DriverFK2 = '{1}')", person.PK1Prop1, person.PK1Prop2), relCriteria.ToString());

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

            relationship.RelKey.RelatedPropValueChanged += delegate { eventCalled = true; };
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation").OwningBOHasForeignKey);
            Assert.IsFalse(eventCalled);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = organisationTestBO;
            //---------------Test Result -----------------------
            Assert.IsTrue(eventCalled);
        }
    }
}
