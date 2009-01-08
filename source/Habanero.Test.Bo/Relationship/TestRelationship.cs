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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public class TestRelationship 
    {

        [TestFixtureSetUp]
        public void SetupFixture()
        {
        }
        [SetUp]
        public void init()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            new Address();

        }

        [Test]
        public void TestRefreshWithRemovedChild()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO cp = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cp.Addresses.Count);

            //---------------Execute Test ----------------------
            address.MarkForDelete();
            address.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cp.Addresses.Count);
        }
        [Test]
        public void TestRefreshWithRemovedChild_DereferenceChild()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO cp = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.FirstName = Guid.NewGuid().ToString("N");
            cp2.Save();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cp.Addresses.Count);

            //---------------Execute Test ----------------------
            address.ContactPersonID = cp2.ContactPersonID;
            address.Save();
            address.SetDeletable(false);
            RelatedBusinessObjectCollection<AddressTestBO> addresses = cp.Addresses;

            //---------------Test Result -----------------------
            Assert.AreEqual(0, addresses.Count);
        }
        [Test]
        public void TestChangedEnginesForeignKey_Dereference_Single_Saved()
        {
            //---------------Set up test pack-------------------
            Car car = new Car();
            Car car2 = new Car();
            Engine engine = new Engine();
            engine.CarID = car.CarID;
            car.Save();
            car2.Save();
            engine.Save();

            //---------------Assert Precondition----------------
            Assert.AreSame(engine, car.GetEngine());
            Assert.AreSame(car, engine.GetCar());

            //---------------Execute Test ----------------------
            engine.CarID = car2.CarID;
            engine.Save();
            Engine loadedEngine = car.GetEngine();

            //---------------Test Result -----------------------
            Assert.IsNull(loadedEngine);
        }

        [Test]
        public void TestChangedEnginesForeignKey_Dereference_Single()
        {
            //---------------Set up test pack-------------------
            Car car = new Car();
            Car car2 = new Car();
            Engine engine = new Engine();
            engine.CarID = car.CarID;
            car.Save();
            car2.Save();
            engine.Save();

            //---------------Assert Precondition----------------
            Assert.AreSame(engine, car.GetEngine());
            Assert.AreSame(car, engine.GetCar());

            //---------------Execute Test ----------------------
            engine.CarID = car2.CarID;
            Engine loadedEngine = car.GetEngine();

            //---------------Test Result -----------------------
            Assert.IsNull(loadedEngine);
        }

        [Test]
        public void TestGetRelatedBusinessObjectCollection_SortOrder()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_SortOrder_AddressLine1();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            AddressTestBO address1 = new AddressTestBO();
            address1.ContactPersonID = cp.ContactPersonID;
            address1.AddressLine1 = "ffff";
            address1.Save();
            AddressTestBO address2 = new AddressTestBO();
            address2.ContactPersonID = cp.ContactPersonID;
            address2.AddressLine1 = "bbbb";
            address2.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------

            RelatedBusinessObjectCollection<AddressTestBO> addresses = cp.Addresses;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, addresses.Count);
            Assert.AreSame(address1, addresses[1]);
            Assert.AreSame(address2, addresses[0]);
            //---------------Tear Down -------------------------     
        }


        [Test]
        public void TestGetRelatedBusinessObjectCollection_SortOrder_ChangeOrder()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_SortOrder_AddressLine1();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            AddressTestBO address1 = new AddressTestBO();
            address1.ContactPersonID = cp.ContactPersonID;
            address1.AddressLine1 = "ffff";
            address1.Save();
            AddressTestBO address2 = new AddressTestBO();
            address2.ContactPersonID = cp.ContactPersonID;
            address2.AddressLine1 = "bbbb";
            address2.Save();

            //---------------Assert PreConditions---------------     
            RelatedBusinessObjectCollection<AddressTestBO> addresses = cp.Addresses;
            Assert.AreEqual(2, addresses.Count);
            Assert.AreSame(address1, addresses[1]);
            Assert.AreSame(address2, addresses[0]);

            //---------------Execute Test ----------------------
            address2.AddressLine1 = "zzzzz";
            address2.Save();
            RelatedBusinessObjectCollection<AddressTestBO> addressesAfterChangeOrder = cp.Addresses;

            //---------------Test Result -----------------------

            Assert.AreSame(address1, addressesAfterChangeOrder[0]);
            Assert.AreSame(address2, addressesAfterChangeOrder[1]);
        }


        [Test]
        public void TestCreateRelationship()
        {
            RelationshipDef mRelationshipDef;
            RelKeyDef mRelKeyDef;
            MockBO mMockBo = GetMockBO(out mRelationshipDef, out mRelKeyDef);

            ISingleRelationship rel = (ISingleRelationship)mRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelatedObject(), "Should be false since props are not defaulted in Mock bo");
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            mMockBo.Save();
            Assert.IsTrue(rel.HasRelatedObject(), "Should be true since prop MockBOProp1 has been set");

            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), mMockBo.GetPropertyValue("MockBOID"));
            MockBO ltempBO = (MockBO)rel.GetRelatedObject();
            Assert.IsFalse(ltempBO == null);
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOID"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValueString("MockBOProp1"), ltempBO.GetPropertyValueString("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");

            Assert.AreSame(ltempBO, rel.GetRelatedObject());
            BusinessObjectManager.Instance.ClearLoadedObjects();
            Assert.AreSame(ltempBO, rel.GetRelatedObject());
            mMockBo.MarkForDelete();
            mMockBo.Save();
        }

        private MockBO GetMockBO(out RelationshipDef mRelationshipDef, out RelKeyDef mRelKeyDef) {
            MockBO mMockBo = new MockBO();
            IPropDefCol mPropDefCol = mMockBo.PropDefCol;
            mRelKeyDef = new RelKeyDef();
            IPropDef propDef = mPropDefCol["MockBOProp1"];
            RelPropDef lRelPropDef = new RelPropDef(propDef, "MockBOID");
            mRelKeyDef.Add(lRelPropDef);
            mRelationshipDef = new SingleRelationshipDef("Relation1", typeof(MockBO), mRelKeyDef, false, DeleteParentAction.Prevent);
            return mMockBo;
        }

        [Test]
        public void TestCreateRelationshipHoldRelRef()
        {
            RelationshipDef mRelationshipDef;
            RelKeyDef mRelKeyDef;
            MockBO mMockBo = GetMockBO(out mRelationshipDef, out mRelKeyDef);
            RelationshipDef lRelationshipDef = new SingleRelationshipDef("Relation1", typeof(MockBO), mRelKeyDef, true, DeleteParentAction.Prevent);
            ISingleRelationship rel = (ISingleRelationship)lRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(lRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelatedObject(), "Should be false since props are not defaulted in Mock bo");
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            mMockBo.Save();
            Assert.IsTrue(rel.HasRelatedObject(), "Should be true since prop MockBOProp1 has been set");

            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), mMockBo.GetPropertyValue("MockBOID"));
            MockBO ltempBO = (MockBO)rel.GetRelatedObject();
            Assert.IsFalse(ltempBO == null);
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOID"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValueString("MockBOProp1"), ltempBO.GetPropertyValueString("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");

            Assert.IsTrue(ReferenceEquals(ltempBO, rel.GetRelatedObject()));
            BusinessObjectManager.Instance.ClearLoadedObjects();
            Assert.IsTrue(ReferenceEquals(ltempBO, rel.GetRelatedObject()));
            mMockBo.MarkForDelete();
            mMockBo.Save();
        }

        [Test]
        public void TestGetRelatedObject()
        {
            RelationshipDef mRelationshipDef;
            RelKeyDef mRelKeyDef;
            MockBO mMockBo = GetMockBO(out mRelationshipDef, out mRelKeyDef);
            RelationshipDef lRelationshipDef = new SingleRelationshipDef("Relation1", typeof(MockBO), mRelKeyDef, true, DeleteParentAction.Prevent);
            ISingleRelationship rel = (ISingleRelationship)lRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(lRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelatedObject(), "Should be false since props are not defaulted in Mock bo");
            //Set a related object
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            //Save the object, so that the relationship can retrieve the object from the database
            mMockBo.Save();
            Assert.IsTrue(rel.HasRelatedObject(), "Should have a related object since the relating props have values");
            MockBO ltempBO = (MockBO)rel.GetRelatedObject();
            Assert.IsNotNull(ltempBO, "The related object should exist");
            //Clear the related object
            mMockBo.SetPropertyValue("MockBOProp1", null);
            Assert.IsFalse(rel.HasRelatedObject(), "Should not have a related object since the relating props have been set to null");
            ltempBO = (MockBO)rel.GetRelatedObject();
            Assert.IsNull(ltempBO, "The related object should now be null");
            //Set a related object again
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            Assert.IsTrue(rel.HasRelatedObject(), "Should have a related object since the relating props have values again");
            ltempBO = (MockBO)rel.GetRelatedObject();
            Assert.IsNotNull(ltempBO, "The related object should exist again");
            mMockBo.MarkForDelete();
            mMockBo.Save();
        }

        [Test]
        public void TestGetReverseRelationship_Addresses()
        {
            //---------------Set up test pack-------------------
            new Engine();
            new Car();
            ContactPerson person = new ContactPerson();
            Address address = new Address();
            SingleRelationship<ContactPerson> relationship = address.Relationships.GetSingle<ContactPerson>("ContactPerson");
            IRelationship expectedRelationship = person.Relationships["Addresses"];

            //---------------Execute Test ----------------------
            IRelationship reverseRelationship = relationship.GetReverseRelationship(person);

            //---------------Test Result -----------------------
            Assert.AreSame(expectedRelationship, reverseRelationship);
        }

        [Test]
        public void TestGetReverseRelationship_Cars()
        {
            //---------------Set up test pack-------------------
            new Engine();
            new Car();
            ContactPerson person = new ContactPerson();
            Car car = new Car();
            SingleRelationship<ContactPerson> relationship = car.Relationships.GetSingle<ContactPerson>("Owner");
            IRelationship expectedRelationship = person.Relationships["Cars"];

            //---------------Execute Test ----------------------
            IRelationship reverseRelationship = relationship.GetReverseRelationship(person);

            //---------------Test Result -----------------------
            Assert.AreSame(expectedRelationship, reverseRelationship);
        }

    }


}
