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
using Habanero.BO.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestRelationship : TestUsingDatabase
    {
        private RelationshipDef mRelationshipDef;
        private RelKeyDef mRelKeyDef;
        private IPropDefCol mPropDefCol;
        private MockBO mMockBo;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SetupDBConnection();
        }
        [SetUp]
        public void init()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorDB();
            mMockBo = new MockBO();
            mPropDefCol = mMockBo.PropDefCol;

            mRelKeyDef = new RelKeyDef();
            IPropDef propDef = mPropDefCol["MockBOProp1"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "MockBOID");
            mRelKeyDef.Add(lRelPropDef);

            mRelationshipDef = new SingleRelationshipDef("Relation1", typeof(MockBO), mRelKeyDef, false, DeleteParentAction.Prevent);
        }

        [Test]
        public void TestRefreshWithRemovedChild()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO cp = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cp.Addresses.Count);

            //---------------Execute Test ----------------------
            address.Delete();
            address.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cp.Addresses.Count);
        }
        [Test]
        public void TestRefreshWithRemovedChild_DereferenceChild()
        {
            //---------------Set up test pack-------------------
            Address address;
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
            BusinessObjectCollection<Address> addresses = cp.Addresses;

            //---------------Test Result -----------------------
            Assert.AreEqual(0, addresses.Count);
        }
        [Test]
        public void TestRefreshWithRemovedChild_Dereference_Single()
        {
            //---------------Set up test pack-------------------
            Car car = new Car();
            Car car2 = new Car();
            Engine engine = new Engine();
            engine.CarID = car.CarID;
            car.Save();
            car2.Save();
            engine.Save();
            //engine.EngineNo = Guid.
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
        public void TestGetRelatedBusinessObjectCollection_SortOrder()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_SortOrder_AddressLine1();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address1 = new Address();
            address1.ContactPersonID = cp.ContactPersonID;
            address1.AddressLine1 = "ffff";
            address1.Save();
            Address address2 = new Address();
            address2.ContactPersonID = cp.ContactPersonID;
            address2.AddressLine1 = "bbbb";
            address2.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------

            RelatedBusinessObjectCollection<Address> addresses = cp.Addresses;
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
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_SortOrder_AddressLine1();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address1 = new Address();
            address1.ContactPersonID = cp.ContactPersonID;
            address1.AddressLine1 = "ffff";
            address1.Save();
            Address address2 = new Address();
            address2.ContactPersonID = cp.ContactPersonID;
            address2.AddressLine1 = "bbbb";
            address2.Save();

            //---------------Assert PreConditions---------------     
            RelatedBusinessObjectCollection<Address> addresses = cp.Addresses;
            Assert.AreEqual(2, addresses.Count);
            Assert.AreSame(address1, addresses[1]);
            Assert.AreSame(address2, addresses[0]);
            
            //---------------Execute Test ----------------------
            address2.AddressLine1 = "zzzzz";
            address2.Save();
            RelatedBusinessObjectCollection<Address> addressesAfterChangeOrder = cp.Addresses;
            
            //---------------Test Result -----------------------

            Assert.AreSame(address1, addressesAfterChangeOrder[0]);
            Assert.AreSame(address2, addressesAfterChangeOrder[1]);
            //---------------Tear Down -------------------------     
        }






        [Test]
        public void TestCreateRelationship()
        {
            SingleRelationship rel = (SingleRelationship)mRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelationship(), "Should be false since props are not defaulted in Mock bo");
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            mMockBo.Save();
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

            Assert.AreSame(ltempBO, rel.GetRelatedObject(DatabaseConnection.CurrentConnection));
            MockBO.ClearLoadedBusinessObjectBaseCol();
            Assert.AreNotSame(ltempBO, rel.GetRelatedObject(DatabaseConnection.CurrentConnection));
            mMockBo.Delete();
            mMockBo.Save();
        }

        [Test]
        public void TestCreateRelationshipHoldRelRef()
        {
            RelationshipDef lRelationshipDef = new SingleRelationshipDef("Relation1", typeof(MockBO), mRelKeyDef, true, DeleteParentAction.Prevent);
            SingleRelationship rel = (SingleRelationship)lRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(lRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelationship(), "Should be false since props are not defaulted in Mock bo");
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            mMockBo.Save();
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

            Assert.IsTrue(ReferenceEquals(ltempBO, rel.GetRelatedObject(DatabaseConnection.CurrentConnection)));
            MockBO.ClearLoadedBusinessObjectBaseCol();
            Assert.IsTrue(ReferenceEquals(ltempBO, rel.GetRelatedObject(DatabaseConnection.CurrentConnection)));
            mMockBo.Delete();
            mMockBo.Save();
        }

        [Test]
        public void TestGetRelatedObject()
        {
            RelationshipDef lRelationshipDef = new SingleRelationshipDef("Relation1", typeof(MockBO), mRelKeyDef, true, DeleteParentAction.Prevent);
            SingleRelationship rel = (SingleRelationship)lRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(lRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelationship(), "Should be false since props are not defaulted in Mock bo");
            //Set a related object
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            //Save the object, so that the relationship can retrieve the object from the database
            mMockBo.Save();
            Assert.IsTrue(rel.HasRelationship(), "Should have a related object since the relating props have values");
            MockBO ltempBO = (MockBO)rel.GetRelatedObject(DatabaseConnection.CurrentConnection);
            Assert.IsNotNull(ltempBO, "The related object should exist");
            //Clear the related object
            mMockBo.SetPropertyValue("MockBOProp1", null);
            Assert.IsFalse(rel.HasRelationship(), "Should not have a related object since the relating props have been set to null");
            ltempBO = (MockBO)rel.GetRelatedObject(DatabaseConnection.CurrentConnection);
            Assert.IsNull(ltempBO, "The related object should now be null");
            //Set a related object again
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            Assert.IsTrue(rel.HasRelationship(), "Should have a related object since the relating props have values again");
            ltempBO = (MockBO)rel.GetRelatedObject(DatabaseConnection.CurrentConnection);
            Assert.IsNotNull(ltempBO, "The related object should exist again"); 
            mMockBo.Delete();
            mMockBo.Save();
        }
    }

    
}
