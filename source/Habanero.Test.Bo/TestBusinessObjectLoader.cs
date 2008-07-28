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
using Habanero.BO.ObjectManager;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    public abstract class TestBusinessObjectLoader
    {
        protected abstract void SetupDataAccessor();

        protected abstract void DeleteEnginesAndCars();

        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            SetupDataAccessor();
            BusinessObject.AllLoadedBusinessObjects().Clear();
            TestUtil.WaitForGC();
        }

        [TearDown]
        public virtual void TearDownTest()
        {
        }

        [Test]
        public void TestGetBusinessObjectWhenNotExists_NotLoadedViaKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("ContactPersonID", Criteria.Op.Equals, Guid.NewGuid().ToString("N"));

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.IsNull(loadedCP);
        }

        [Test]
        public void TestGetBusinessObjectWhenNotExists_NotLoadedViaKey_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("ContactPersonID", Criteria.Op.Equals, Guid.NewGuid().ToString("N"));

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                (ContactPersonTestBO) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.IsNull(loadedCP);
        }

        [Test]
        public void TestGetBusinessObject_SelectQuery()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            SelectQuery query = CreateSelectQuery(cp);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(query);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, cp);
        }

        private static SelectQuery CreateSelectQuery(ContactPersonTestBO cp)
        {
            SelectQuery query = new SelectQuery(new Criteria("Surname", Criteria.Op.Equals, cp.Surname));
            query.Fields.Add("Surname", new QueryField("Surname", "Surname_field", null));
            query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
            query.Source = new Source(cp.ClassDef.TableName);
            return query;
        }

        [Test]
        public void TestGetBusinessObject_SelectQuery_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            SelectQuery query = CreateSelectQuery(cp);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCp =
                (ContactPersonTestBO)BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(cp.ClassDef, query);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, cp);
        }

        [Test]
        public void TestGetBusinessObject_PrimaryKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);      
        }

        [Test]
        public void TestGetBusinessObject_PrimaryKey_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = (ContactPersonTestBO)
                   BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, cp.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
        }

        [Test]
        public void TestGetBusinessObject_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
        }

        [Test]
        public void TestGetBusinessObject_CriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = (ContactPersonTestBO)
                   BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
        }
        
        [Test]
        [ExpectedException(typeof (BusObjDeleteConcurrencyControlException))]
        public void TestTryLoadDeletedObject_RaiseError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO personToDelete = ContactPersonTestBO.CreateSavedContactPerson();
            personToDelete.Delete();
            personToDelete.Save();

            //Ensure that a fresh object is loaded from DB
            BusObjectManager.Instance.ClearLoadedObjects();

            //--------Execute------------------------------------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(personToDelete.ID);
        }

        [Test]
        [ExpectedException(typeof (BusObjDeleteConcurrencyControlException))]
        public void TestTryLoadDeletedObject_RaiseError_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO personToDelete = ContactPersonTestBO.CreateSavedContactPerson();
            personToDelete.Delete();
            personToDelete.Save();

            //Ensure that a fresh object is loaded from DB
            BusObjectManager.Instance.ClearLoadedObjects();

            //--------Execute------------------------------------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(personToDelete.ClassDef, personToDelete.ID);
        }

        [Test]
        public void TestGetBusinessObject_SingleCriteria()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO.CreateSavedContactPerson(surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, surname);
            ContactPersonTestBO cp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(surname, cp.Surname);
            Assert.AreEqual(firstName, cp.FirstName);
        }

        //private static ContactPersonTestBO CreateSavedContactPerson(string surname, string firstName)
        //{
        //    ContactPersonTestBO contact = new ContactPersonTestBO();
        //    contact.Surname = surname;
        //    contact.FirstName = firstName;
        //    contact.Save();
        //    return contact;
        //}

        private static void WaitForGC()
        {
            TestUtil.WaitForGC();
        }

        #region TestBusinessObjectCollectionLoading

        [Test]
        public void TestGetBusinessObjectCollection_SelectQuery()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now.AddMinutes(1));

            SelectQuery query = CreateManualSelectQueryOrderedByDateOfBirth(now, cp1);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(query);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        private static SelectQuery CreateManualSelectQueryOrderedByDateOfBirth(DateTime now, ContactPersonTestBO cp1)
        {
            SelectQuery query = new SelectQuery(new Criteria("DateOfBirth", Criteria.Op.GreaterThan, now));
            query.Fields.Add("DateOfBirth", new QueryField("DateOfBirth", "DateOfBirth", null));
            query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
            query.Source = new Source(cp1.ClassDef.TableName);
            query.OrderCriteria = new OrderCriteria().Add("DateOfBirth");
            return query;
        }

        [Test]
        public void TestGetBusinessObjectCollection_SelectQuery_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now.AddMinutes(1));
            SelectQuery query = CreateManualSelectQueryOrderedByDateOfBirth(now, cp1);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, query);


            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }
        
        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, criteria);


            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_WithOrder()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");


            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_WithOrder_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, criteria, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_GetsSameObjectAsGetBusinessObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria collectionCriteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            Criteria singleCritieria = new Criteria("ContactPersonID", Criteria.Op.Equals, cp1.ContactPersonID);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            BusinessObject.AllLoadedBusinessObjects().Clear();
            ContactPersonTestBO loadedCp = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(singleCritieria);
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(collectionCriteria, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_GetsSameObjectAsGetBusinessObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria collectionCriteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            Criteria singleCritieria = new Criteria("ContactPersonID", Criteria.Op.Equals, cp1.ContactPersonID);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            BusinessObject.AllLoadedBusinessObjects().Clear();
            ContactPersonTestBO loadedCp = (ContactPersonTestBO)BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(cp1.ClassDef, singleCritieria);
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, collectionCriteria, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, col[1]);
        }
        
        [Test]
        public void TestCriteriaSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void TestCriteriaSetUponLoadingCollection_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void TestRefreshLoadedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
        }

        [Test]
        public void TestRefreshLoadedCollection_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
        }

        [Test]
        public void TestGetRelatedBusinessObject()
        {
            //---------------Set up test pack-------------------
            Car car = Car.CreateSavedCar("5");
            Engine engine = Engine.CreateSavedEngine(car, "20");

            //---------------Execute Test ----------------------
            Car loadedCar = BORegistry.DataAccessor.BusinessObjectLoader.
                GetRelatedBusinessObject<Car>((SingleRelationship) engine.Relationships["Car"]);

            //---------------Test Result -----------------------
            Assert.AreSame(car, loadedCar);
        }

        [Test]
        public void TestGetRelatedBusinessObject_Untyped()
        {
            //---------------Set up test pack-------------------
            Car car = Car.CreateSavedCar("5");
            Engine engine = Engine.CreateSavedEngine(car, "20");

            //---------------Execute Test ----------------------
            Car loadedCar = (Car) 
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject((SingleRelationship) engine.Relationships["Car"]);

            //---------------Test Result -----------------------
            Assert.AreSame(car, loadedCar);
        }

        [Test]
        public void TestGetRelatedBusinessObjectCollection_Generic()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO cp = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);
            address.ContactPersonID = cp.ContactPersonID;
            address.Save();
         
            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<Address> addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<Address>(cp.Relationships["Addresses"]);

            //---------------Test Result -----------------------
            Criteria relationshipCriteria = Criteria.FromRelationship(cp.Relationships["Addresses"]);
            Assert.AreEqual(relationshipCriteria, addresses.SelectQuery.Criteria);
            Assert.AreEqual(1, addresses.Count);
            Assert.Contains(address, addresses);
        }

        [Test]
        public void TestGetRelatedBusinessObjectCollection_NotStronglyTyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteDoNothing();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address = new Address();
            address.ContactPersonID = cp.ContactPersonID;
            address.Save();

            //---------------Assert PreConditions---------------    
        
            //---------------Execute Test ----------------------

            IBusinessObjectCollection addresses =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection(
                    typeof(Address), cp.Relationships["Addresses"]);

            //---------------Test Result -----------------------
            Criteria relationshipCriteria = Criteria.FromRelationship(cp.Relationships["Addresses"]);
            Assert.AreEqual(relationshipCriteria, addresses.SelectQuery.Criteria);
            Assert.AreEqual(1, addresses.Count);
            Assert.Contains(address, addresses);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetRelatedBusinessObjectCollection_SortOrder_Generic()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_SortOrder_AddressLine1();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address1 = Address.CreateSavedAddress(cp.ContactPersonID, "ffff");
            Address address2 = Address.CreateSavedAddress(cp.ContactPersonID, "bbbb");

            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<Address> addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<Address>(cp.Relationships["Addresses"]);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, addresses.Count);
            Assert.AreSame(address1, addresses[1]);
            Assert.AreSame(address2, addresses[0]);
        }

        [Test]
        public void TestGetRelatedBusinessObjectCollection_SortOrder_NotStronglyTyped()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
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

            IBusinessObjectCollection addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection(typeof(Address),
                    cp.Relationships["Addresses"]);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, addresses.Count);
            Assert.AreSame(address1, addresses[1]);
            Assert.AreSame(address2, addresses[0]);
            //---------------Tear Down -------------------------     
        }
        [Test]
        public void TestGetRelatedBusinessObjectCollection_LoadedViaRelationship()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
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
            IBusinessObjectCollection addresses = cp.Relationships["Addresses"].GetRelatedBusinessObjectCol();
            //IBusinessObjectCollection addresses =
            //    BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection(typeof(Address),
            //        cp.Relationships["Addresses"]);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, addresses.Count);
            Assert.AreSame(address1, addresses[1]);
            Assert.AreSame(address2, addresses[0]);
            //---------------Tear Down -------------------------     
        }
        [Test]
        public void TestGetBusinessObjectCollection_SortOrder_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef, "Car.CarRegNo, EngineNo");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<Engine> engines = 
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>(null, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
        }
        
        [Test]
        public void TestGetBusinessObjectCollection_SortOrder_ThroughRelationship_Untyped()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef, "Car.CarRegNo, EngineNo");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(car1engine1.ClassDef, null, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
        }
        
        [Test]
        public void TestGetBusinessObjectCollection_SortOrder_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            ContactPerson contactPerson1 = ContactPerson.CreateSavedContactPerson("zzzz");
            ContactPerson contactPerson2 = ContactPerson.CreateSavedContactPerson("aaaa");
            Car car1 = Car.CreateSavedCar("2", contactPerson1);
            Car car2 = Car.CreateSavedCar("5", contactPerson2);
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");

            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = OrderCriteria.FromString("Car.Owner.Surname, EngineNo");
            BusinessObjectCollection<Engine> engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>(null, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void TestLoadThroughRelationship_Multiple()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteDoNothing();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address = Address.CreateSavedAddress(cp.ContactPersonID);

            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<Address> addresses = cp.Addresses;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, addresses.Count);
            Assert.Contains(address, addresses);
        }

        [Test]
        public void TestLoadThroughRelationship_Single()
        {
            //---------------Set up test pack-------------------
            Car car = Car.CreateSavedCar("5");
            Engine engine = Engine.CreateSavedEngine(car, "20");

            //---------------Execute Test ----------------------
            Car loadedCar = engine.GetCar();

            //---------------Test Result -----------------------
            Assert.AreSame(car, loadedCar);
        }

        [Test]
        public void TestLoadAll_Loader()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            BusObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.LoadAll_Loader();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp, col);
        }

        [Test]
        public void TestLoadWithOrderBy()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("ggggg");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson("bbbbb");

            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria().Add("Surname");
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(null,
                                                                                                              orderCriteria);
            //---------------Test Result -----------------------
            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
        }

        [Test]
        public void TestLoadWithOrderBy_ManualOrderbyFieldName()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("ggggg");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson("bbbbb");
            OrderCriteria orderCriteria = new OrderCriteria();

            //---------------Execute Test ----------------------
            orderCriteria.Add(new OrderCriteria.Field("Surname", "Surname_field", null, OrderCriteria.SortDirection.Ascending));
            BusinessObjectCollection<ContactPersonTestBO> col = 
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(null,
                                                                                                              orderCriteria);
            //---------------Test Result -----------------------
            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
        }

        public void Test3LayersLoadRelated_LoadsObjectFromObjectManager()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            OrganisationTestBO.LoadDefaultClassDef();

            OrganisationTestBO org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = org.Relationships["ContactPeople"].GetRelatedBusinessObjectCol();
            ContactPersonTestBO loadedContactPerson = (ContactPersonTestBO) col[0];
            IBusinessObjectCollection colAddresses = loadedContactPerson.Relationships["Addresses"].GetRelatedBusinessObjectCol();
            IBusinessObject loadedAddress = colAddresses[0];

            //---------------Test Result -----------------------
            Assert.AreSame(loadedContactPerson, contactPersonTestBO);
            Assert.AreSame(loadedAddress, address);
        }

        #endregion //TestRegions

        #region TestLoadInheritance

        [Test]
        public void TestLoad_SingleTableInheritance()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();

            //---------------Execute Test ----------------------
            CircleNoPrimaryKey loadedCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<CircleNoPrimaryKey>(circle.ID);

            //---------------Test Result -----------------------

            Assert.AreSame(loadedCircle, circle);
        }

        [Test]
        public void TestLoad_SingleTableInheritance_Hierarchy()
        {
            //---------------Set up test pack-------------------
            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
            FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();

            //---------------Execute Test ----------------------
            FilledCircleNoPrimaryKey loadedFilledCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircleNoPrimaryKey>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedFilledCircle, filledCircle);
        }

        [Test]
        public void TestLoad_ClassTableInheritance()
        {
            //---------------Set up test pack-------------------
            Circle.GetClassDef();
            Circle circle = Circle.CreateSavedCircle();

            //---------------Execute Test ----------------------
            Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCircle, circle);
        }

        [Test]
        public void TestLoad_ClassTableInheritance_Hierarchy()
        {
            //---------------Set up test pack-------------------
            FilledCircle.GetClassDefWithClassInheritanceHierarchy();
            FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();

            //---------------Execute Test ----------------------
            FilledCircle loadedFilledCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedFilledCircle, filledCircle);
        }

        [Test]
        public void TestLoad_ConcreteTableInheritance()
        {
            //---------------Set up test pack-------------------
            Circle.GetClassDefWithConcreteTableInheritance();
            Circle circle = Circle.CreateSavedCircle();

            //---------------Execute Test ----------------------
            Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCircle, circle);
        }

        [Test]
        public void TestLoad_ConcreteTableInheritance_Hierarchy()
        {
            //---------------Set up test pack-------------------
            FilledCircle.GetClassDefWithConcreteInheritanceHierarchy();
            FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();
            //---------------Execute Test ----------------------
            FilledCircle loadedFilledCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedFilledCircle, filledCircle);
        }

        #endregion //TestLoadInheritance

        //public static void CreateSampleData()
        //{
        //    ClassDef.ClassDefs.Clear();
        //    LoadFullClassDef();

        //    string[] surnames = { "zzz", "abc", "abcd" };
        //    string[] firstNames = { "a", "aa", "aa" };

        //    for (int i = 0; i < surnames.Length; i++)
        //    {
        //        if (BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("surname = " + surnames[i]) == null)
        //        {
        //            ContactPersonTestBO contact = new ContactPersonTestBO();
        //            contact.Surname = surnames[i];
        //            contact.FirstName = firstNames[i];
        //            contact.Save();
        //        }
        //    }
        //    ClassDef.ClassDefs.Clear();
        //}
        //[Test]
        //public void TestGetBusinessObject()
        //{
        //    ClassDef.ClassDefs.Clear();
        //    ContactPersonTestBO.LoadDefaultClassDef();


        //    ContactPersonTestBO cp = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
        //    Assert.AreEqual("abc", cp.Surname);


        //    cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abcde");
        //    Assert.IsNull(cp);

        //    cp = (ContactPersonTestBO)BOLoader.Instance.GetBusinessObject(new ContactPersonTestBO(), new Parameter("Surname = invalid"));
        //    Assert.IsNull(cp);
        //}

        #region Nested type: TestBusinessObjectLoaderDB

        [TestFixture]
        public class TestBusinessObjectLoaderDB : TestBusinessObjectLoader
        {
            //TODO: stop this using the BOLoader

            #region Setup/Teardown

            [SetUp]
            public override void SetupTest()
            {
                base.SetupTest();
                ContactPersonTestBO.DeleteAllContactPeople();
            }

            #endregion

            protected override void DeleteEnginesAndCars()
            {
                Engine.DeleteAllEngines();
                Car.DeleteAllCars();
            }

            public TestBusinessObjectLoaderDB()
            {
                new TestUsingDatabase().SetupDBConnection();
            }

            protected override void SetupDataAccessor()
            {
                BORegistry.DataAccessor = new DataAccessorDB();
            }

            //[Test]
            //public void Test_BusinessObjectClearsItselfFromObjectManager()
            //{
            //    //---------------Set up test pack-------------------
            //    BusObjectManager.Instance.ClearLoadedObjects();
            //    WaitForGC();
            //    ClassDef.ClassDefs.Clear();
            //    ContactPersonTestBO.LoadDefaultClassDef();
            //    const string surname = "abc";
            //    const string firstName = "aa";
            //    ContactPersonTestBO savedContactPerson = CreateSavedContactPerson(surname, firstName);

            //    //---------------Assert Precondition----------------
            //    Assert.AreEqual(1, BusObjectManager.Instance.Count);
            //    Assert.IsNotNull(savedContactPerson);

            //    //---------------Execute Test ----------------------
            //    savedContactPerson = null;
            //    WaitForGC();

            //    //---------------Test Result -----------------------
            //    Assert.IsNull(savedContactPerson);
            //    Assert.AreEqual(0, BusObjectManager.Instance.Count);
            //}

            [Test]
            public void Test_ReturnSameObjectFromBusinessObjectLoader()
            {
                //---------------Set up test pack-------------------
                //------------------------------Setup Test
                ContactPerson originalContactPerson = new ContactPerson();
                originalContactPerson.Surname = "FirstSurname";
                originalContactPerson.Save();

                BusObjectManager.Instance.ClearLoadedObjects();

                //load second object from DB to ensure that it is now in the object manager
                ContactPerson myContact2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        originalContactPerson.ID);

                //---------------Assert Precondition----------------
                Assert.AreNotSame(originalContactPerson, myContact2);

                //---------------Execute Test ----------------------
                ContactPerson myContact3 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        originalContactPerson.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(originalContactPerson, myContact3);
                Assert.AreSame(myContact2, myContact3);
            }

            [Test]
            public void TestAfterLoadCalled_GetBusinessObject()
            {
                //---------------Set up test pack-------------------
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
                BusObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                ContactPersonTestBO loadedCP =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.ID);
                //---------------Test Result -----------------------
                Assert.AreNotSame(cp, loadedCP);
                Assert.IsTrue(loadedCP.AfterLoadCalled);
            }

            [Test]
            public void TestAfterLoadCalled_GetBusinessObject_Untyped()
            {
                //---------------Set up test pack-------------------
                ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
                BusObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                ContactPersonTestBO loadedCP =
                    (ContactPersonTestBO)
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, cp.ID);
                //---------------Test Result -----------------------
                Assert.AreNotSame(cp, loadedCP);
                Assert.IsTrue(loadedCP.AfterLoadCalled);
            }

            [Test]
            public void TestAfterLoadCalled_GetCollection()
            {
                //---------------Set up test pack-------------------
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
                BusObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("ContactPersonID", Criteria.Op.Equals, cp.ContactPersonID.ToString("B"));
                BusinessObjectCollection<ContactPersonTestBO> col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(
                        criteria);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                Assert.AreNotSame(cp, col[0]);
                Assert.IsTrue(col[0].AfterLoadCalled);
            }

            [Test, Ignore("This needs to be part of the refresh object strategy. Currently no strategy is being implemented")]
            public void TestAfterLoadCalled_GetCollection_NotReloaded()
            {
                //---------------Set up test pack-------------------
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();

                //---------------Assert Precondition----------------
                Assert.IsFalse(cp.AfterLoadCalled);

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("ContactPersonID", Criteria.Op.Equals, cp.ContactPersonID.ToString("B"));
                BusinessObjectCollection<ContactPersonTestBO> col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(
                        criteria);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                ContactPersonTestBO loadedBO = col[0];
                Assert.AreSame(cp, loadedBO);
                Assert.IsTrue(loadedBO.AfterLoadCalled);
            }

            [Test]
            public void TestAfterLoadCalled_GetCollection_Untyped()
            {
                //---------------Set up test pack-------------------
                ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
                BusObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("ContactPersonID", Criteria.Op.Equals, cp.ContactPersonID.ToString("B"));
                IBusinessObjectCollection col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                ContactPersonTestBO loadedCP = (ContactPersonTestBO) col[0];
                Assert.AreNotSame(cp, loadedCP);
                Assert.IsTrue(loadedCP.AfterLoadCalled);
            }

            [Test, Ignore("This needs to be part of the refresh object strategy. Currently no strategy is being implemented")]
            public void TestAfterLoadCalled_GetCollection_Untyped_NotReloaded()
            {
                //---------------Set up test pack-------------------
                ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();

                //---------------Assert Precondition----------------
                Assert.IsFalse(cp.AfterLoadCalled);

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("ContactPersonID", Criteria.Op.Equals, cp.ContactPersonID.ToString("B"));
                IBusinessObjectCollection col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                ContactPersonTestBO loadedCP = (ContactPersonTestBO) col[0];
                Assert.AreSame(cp, loadedCP);
                Assert.IsTrue(loadedCP.AfterLoadCalled);
            }

            /// <summary>
            /// Tests to ensure that if the object has been edited by another user
            ///  and the default strategy to reload has been replaced then one we do not get back is always the latest.
            /// Note: This behaviour is configurable using a strategy TestGetTheFreshestObject_Strategy test 
            /// </summary>
            [Test]
            public void TestDontGetTheFreshestObject_Strategy()
            {
                //------------------------------Setup Test
                ContactPerson originalContactPerson = new ContactPerson();
                originalContactPerson.Surname = "FirstSurname";
                originalContactPerson.Save();
                IPrimaryKey origCPID = originalContactPerson.ID;

                BusObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();
                //load second object from DB to ensure that it is now in the object manager
                ContactPerson myContact2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        origCPID);

                //-----------------------------Execute Test-------------------------
                //Edit first object and save
                originalContactPerson.Surname = "SecondSurname";
                originalContactPerson.Save();

                ContactPerson myContact3 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        origCPID);

                //-----------------------------Assert Result-----------------------
                Assert.AreSame(myContact3, myContact2);
                //The two surnames should be equal since the myContact3 was refreshed
                // when it was loaded.
                Assert.AreNotEqual(originalContactPerson.Surname, myContact3.Surname);
                //Just to check the myContact2 should also match since it is physically the 
                // same object as myContact3
                Assert.AreNotEqual(originalContactPerson.Surname, myContact2.Surname);
            }

            [Test]
            public void TestGetBusinessObject_MultipleCriteria()
            {
                //---------------Set up test pack-------------------
                ClassDef.ClassDefs.Clear();
                ContactPersonTestBO.LoadDefaultClassDef();
                BusObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();

                const string surname = "abc";
                const string firstName = "aa";
                ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
                ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson(surname, firstName);
                ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

                //---------------Assert Precondition----------------
                //Object loaded in object manager
                Assert.AreEqual(3, BusObjectManager.Instance.Count);
                Assert.IsTrue(BusObjectManager.Instance.Contains(savedContactPerson.ID));

                //---------------Execute Test ----------------------
                Criteria criteria1 = new Criteria("Surname", Criteria.Op.Equals, surname);
                Criteria criteria2 = new Criteria("FirstName", Criteria.Op.Equals, firstName);
                Criteria criteria = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);
                ContactPersonTestBO cp =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

                //---------------Test Result -----------------------
                Assert.AreSame(savedContactPerson, cp);

                Assert.AreEqual(surname, cp.Surname);
                Assert.AreEqual(firstName, cp.FirstName);
                Assert.IsFalse(cp.State.IsNew);
                Assert.IsFalse(cp.State.IsDirty);
                Assert.IsFalse(cp.State.IsDeleted);
            }

            [Test]
            public void TestGetBusinessObject_ReturnsSubType_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();

                CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
                CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
                BusObjectManager.Instance.ClearLoadedObjects();

                //---------------Execute Test ----------------------
                Shape loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(circle.ID);

                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof (CircleNoPrimaryKey), loadedShape);
            }

            [Test]
            public void TestGetBusinessObject_ReturnsSubType_TwoLevelsDeep_DiscriminatorShared_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();

                FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
                FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
                BusObjectManager.Instance.ClearLoadedObjects();

                //---------------Execute Test ----------------------
                Shape loadedShape =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(filledCircle.ID);
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof (FilledCircleNoPrimaryKey), loadedShape);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestGetBusinessObject_ReturnsSubType_TwoLevelsDeep_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();

                FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchyDifferentDiscriminators();
                FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
                BusObjectManager.Instance.ClearLoadedObjects();

                //---------------Execute Test ----------------------
                Shape loadedShape =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(filledCircle.ID);
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof (FilledCircleNoPrimaryKey), loadedShape);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestGetBusinessObject_SelectQuery_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = new ContactPersonTestBO();
                cp.Surname = Guid.NewGuid().ToString("N");
                cp.FirstName = Guid.NewGuid().ToString("N");
                cp.Save();

                BusObjectManager.Instance.ClearLoadedObjects();
                WaitForGC();

                SelectQuery query = new SelectQuery(new Criteria("Surname", Criteria.Op.Equals, cp.Surname));
                query.Fields.Add("Surname", new QueryField("Surname", "Surname_field", null));
                query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
                query.Source = new Source(cp.ClassDef.TableName);

                //---------------Assert Precondition ---------------
                //Object not loaded in all loaded business objects
                Assert.AreEqual(0, BusObjectManager.Instance.Count);

                //---------------Execute Test ----------------------
                ContactPersonTestBO loadedCp =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(query);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, BusObjectManager.Instance.Count);
                Assert.AreNotSame(loadedCp, cp);
                Assert.AreEqual(cp.ContactPersonID, loadedCp.ContactPersonID);
                Assert.AreEqual(cp.Surname, loadedCp.Surname);
                Assert.IsTrue(String.IsNullOrEmpty(loadedCp.FirstName), "Firstname is not being loaded");
                // not being loaded
                Assert.IsFalse(loadedCp.State.IsNew);
                Assert.IsFalse(loadedCp.State.IsDeleted);
                Assert.IsFalse(loadedCp.State.IsDirty);
                Assert.IsTrue(loadedCp.State.IsValid());
            }

            [Test]
            public void TestLoadFromDatabaseAlwaysLoadsSameObject()
            {
                //---------------Set up test pack-------------------
                ContactPerson originalContactPerson = new ContactPerson();
                const string firstSurname = "FirstSurname";
                originalContactPerson.Surname = firstSurname;
                originalContactPerson.Save();
                IPrimaryKey origConactPersonID = originalContactPerson.ID;
                originalContactPerson = null;
                BusObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();

                //load second object from DB to ensure that it is now in the object manager
                ContactPerson loadedContactPerson1 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        origConactPersonID);

                //---------------Assert Precondition----------------
                Assert.IsNull(originalContactPerson);
                Assert.AreEqual(firstSurname, loadedContactPerson1.Surname);
                Assert.AreNotSame(originalContactPerson, loadedContactPerson1);
                Assert.AreEqual(1, BusObjectManager.Instance.Count);

                //---------------Execute Test ----------------------

                ContactPerson loadedContactPerson2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        origConactPersonID);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, BusObjectManager.Instance.Count);
                Assert.AreEqual(firstSurname, loadedContactPerson2.Surname);
                Assert.AreNotSame(originalContactPerson, loadedContactPerson2);

                Assert.AreSame(loadedContactPerson1, loadedContactPerson2);

            }

            /// <summary>
            /// Tests to ensure that if the object has been edited by
            /// another user and is not currently being edited by this user
            ///  then one we get back is always the latest.
            /// Note: This behaviour is configurable using a strategy TestDontGetTheFreshestObject_Strategy test 
            /// </summary>
            [Test, Ignore("To be implemented via a strategy")]
            public void TestGetTheFreshestObject_Strategy()
            {
                //------------------------------Setup Test
                ContactPerson originalContactPerson = new ContactPerson();
                originalContactPerson.Surname = "FirstSurname";
                originalContactPerson.Save();

                BusObjectManager.Instance.ClearLoadedObjects();

                //load second object from DB to ensure that it is now in the object manager
                ContactPerson myContact2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        originalContactPerson.ID);

                //-----------------------------Assert Precondition -----------------
                Assert.AreEqual(1, BusObjectManager.Instance.Count);
                Assert.AreNotSame(originalContactPerson, myContact2);
                IPrimaryKey id = myContact2.ID;
                Assert.IsTrue(BusObjectManager.Instance.Contains(id));
                IBusinessObject boFromAllLoadedObjects =
                    BusObjectManager.Instance[id.GetObjectId()];
                Assert.AreSame(boFromAllLoadedObjects, myContact2);

                //-----------------------------Execute Test-------------------------
                //Edit first object and save
                originalContactPerson.Surname = "SecondSurname";
                originalContactPerson.Save(); 

                ContactPerson myContact3 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        originalContactPerson.ID);

                //-----------------------------Assert Result-----------------------
                Assert.AreNotSame(originalContactPerson, myContact3);
                Assert.AreEqual(1, BusObjectManager.Instance.Count);
                Assert.AreSame(myContact3, myContact2);
                //The two surnames should be equal since the myContact3 was refreshed
                // when it was loaded.
                Assert.AreEqual(originalContactPerson.Surname, myContact3.Surname);
                //Just to check the myContact2 should also match since it is physically the 
                // same object as myContact3
                Assert.AreEqual(originalContactPerson.Surname, myContact2.Surname);
            }

            [Test]
            public void TestLoad_ClassTableInheritance_Fresh()
            {
                //---------------Set up test pack-------------------
                Circle.GetClassDef();
                Circle circle = Circle.CreateSavedCircle();

                //---------------Execute Test ----------------------
                BusObjectManager.Instance.ClearLoadedObjects();
                Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedCircle, circle);
                Assert.AreEqual(circle.Radius, loadedCircle.Radius);
                Assert.AreEqual(circle.ShapeName, loadedCircle.ShapeName);
            }


            [Test]
            public void TestLoad_ClassTableInheritance_Hierarchy_Fresh()
            {
                //---------------Set up test pack-------------------
                FilledCircle.GetClassDefWithClassInheritanceHierarchy();
                FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();

                //---------------Execute Test ----------------------
                BusObjectManager.Instance.ClearLoadedObjects();
                FilledCircle loadedFilledCircle =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedFilledCircle, filledCircle);
                Assert.AreEqual(filledCircle.Radius, loadedFilledCircle.Radius);
                Assert.AreEqual(filledCircle.ShapeName, loadedFilledCircle.ShapeName);
                Assert.AreEqual(filledCircle.Colour, loadedFilledCircle.Colour);
            }

            [Test]
            public void TestLoad_ConcreteTableInheritance_Fresh()
            {
                //---------------Set up test pack-------------------
                Circle.GetClassDef();
                Circle circle = Circle.CreateSavedCircle();

                //---------------Execute Test ----------------------
                BusObjectManager.Instance.ClearLoadedObjects();
                Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedCircle, circle);
                Assert.AreEqual(circle.Radius, loadedCircle.Radius);
                Assert.AreEqual(circle.ShapeName, loadedCircle.ShapeName);
                Assert.IsFalse(loadedCircle.State.IsNew);
                Assert.IsFalse(loadedCircle.State.IsDeleted);
                Assert.IsFalse(loadedCircle.State.IsEditing);
                Assert.IsFalse(loadedCircle.State.IsDirty);
                Assert.IsTrue(loadedCircle.State.IsValid());
            }

            [Test]
            public void TestLoad_ConcreteTableInheritance_Hierarchy_Fresh()
            {
                //---------------Set up test pack-------------------
                FilledCircle.GetClassDefWithConcreteInheritanceHierarchy();
                FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();

                //---------------Execute Test ----------------------
                BusObjectManager.Instance.ClearLoadedObjects();
                FilledCircle loadedFilledCircle =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedFilledCircle, filledCircle);
                Assert.AreEqual(filledCircle.Radius, loadedFilledCircle.Radius);
                Assert.AreEqual(filledCircle.ShapeName, loadedFilledCircle.ShapeName);
                Assert.AreEqual(filledCircle.Colour, loadedFilledCircle.Colour);
            }

            [Test]
            public void TestLoad_SingleTableInheritance_Fresh()
            {
                //---------------Set up test pack-------------------
                CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
                CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();

                //---------------Execute Test ----------------------
                BusObjectManager.Instance.ClearLoadedObjects();
                CircleNoPrimaryKey loadedCircle =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<CircleNoPrimaryKey>(circle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedCircle, circle);
                Assert.AreEqual(circle.Radius, loadedCircle.Radius);
                Assert.AreEqual(circle.ShapeName, loadedCircle.ShapeName);
            }

            [Test]
            public void TestLoad_SingleTableInheritance_Hierarchy_Fresh()
            {
                //---------------Set up test pack-------------------
                FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
                FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();

                //---------------Execute Test ----------------------
                BusObjectManager.Instance.ClearLoadedObjects();
                FilledCircleNoPrimaryKey loadedFilledCircle =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircleNoPrimaryKey>(
                        filledCircle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedFilledCircle, filledCircle);
                Assert.AreEqual(filledCircle.Radius, loadedFilledCircle.Radius);
                Assert.AreEqual(filledCircle.ShapeName, loadedFilledCircle.ShapeName);
                Assert.AreEqual(filledCircle.Colour, loadedFilledCircle.Colour);
                //---------------Tear Down -------------------------
            }

            [Test]
            public void TestLoadedObjectIsAddedToObjectManager()
            {
                //---------------Set up test pack-------------------
                ContactPerson.DeleteAllContactPeople();
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO contactPerson1 = ContactPersonTestBO.CreateSavedContactPerson(Guid.NewGuid().ToString("N"),
                                                                              Guid.NewGuid().ToString("N"));
                BusObjectManager.Instance.ClearLoadedObjects();

                //---------------Assert Precondition----------------
                Assert.AreEqual(0, BusObjectManager.Instance.Count);

                //---------------Execute Test ----------------------
                ContactPersonTestBO contactPerson =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(
                        contactPerson1.ID);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, BusObjectManager.Instance.Count);
                Assert.IsTrue(BusObjectManager.Instance.Contains(contactPerson));
            }
        }

        #endregion

        #region Nested type: TestBusinessObjectLoaderInMemory

        [TestFixture]
        public class TestBusinessObjectLoaderInMemory : TestBusinessObjectLoader
        {
            private DataStoreInMemory _dataStore;

            protected override void SetupDataAccessor()
            {
                _dataStore = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(_dataStore);
            }

            protected override void DeleteEnginesAndCars()
            {
                // do nothing
            }

            [Test]
            public void Test_ReturnSameObjectFromBusinessObjectLoader()
            {
                //---------------Set up test pack-------------------
                //------------------------------Setup Test
                ContactPerson originalContactPerson = new ContactPerson();
                originalContactPerson.Surname = "FirstSurname";
                originalContactPerson.Save();

                BusObjectManager.Instance.ClearLoadedObjects();

                //load second object from DB to ensure that it is now in the object manager
                ContactPerson myContact2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        originalContactPerson.ID);

                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                ContactPerson myContact3 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(
                        originalContactPerson.ID);

                //---------------Test Result -----------------------
//                Assert.AreNotSame(originalContactPerson, myContact3);
                Assert.AreSame(myContact2, myContact3);
            }

            [Test]
            public void TestRefreshLoadedCollection_DeletedItem()
            {
                //---------------Set up test pack-------------------
                DataStoreInMemory dataStore = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
                ContactPersonTestBO.LoadDefaultClassDef();
                DateTime now = DateTime.Now;
                ContactPersonTestBO cp1 = new ContactPersonTestBO();
                cp1.DateOfBirth = now;
                cp1.Surname = Guid.NewGuid().ToString("N");
                cp1.Save();
                ContactPersonTestBO cp2 = new ContactPersonTestBO();
                cp2.DateOfBirth = now;
                cp2.Surname = Guid.NewGuid().ToString("N");
                cp2.Save();
                Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
                BusinessObjectCollection<ContactPersonTestBO> col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(
                        criteria);

                dataStore.Remove(cp2);
                //---------------Execute Test ----------------------
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                Assert.Contains(cp1, col);
                //---------------Tear Down -------------------------
            }
        }

        #endregion
    }


//        [Test, ExpectedException(typeof(UserException))]
//        public void TestGetBusinessObjectDuplicatesException_IfTryLoadObjectWithCriteriaThatMatchMoreThanOne()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadDefaultClassDef();

//            BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("FirstName = aa");
//        }

//        [Test]
//        public void TestGetBusinessObjectCollection()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadDefaultClassDef();

//            OrderCriteria orderBySurname = OrderCriteria.FromString("Surname");
//            BusinessObjectCollection<ContactPersonTestBO> col = BOLoader.Instance.GetBusinessObjectCol<ContactPersonTestBO>("FirstName = aa", orderBySurname);
//            Assert.AreEqual(2, col.Count);
//            Assert.AreEqual("abc", col[0].Surname);
//            Assert.AreEqual("abcd", col[1].Surname);

//            IBusinessObjectCollection col2 = BOLoader.Instance.GetBusinessObjectCol(typeof(ContactPersonTestBO), "FirstName = aa", orderBySurname);
//            Assert.AreEqual(2, col.Count);
//            Assert.AreEqual("abc", ((ContactPersonTestBO)col2[0]).Surname);
//            Assert.AreEqual("abcd", ((ContactPersonTestBO)col2[1]).Surname);

//            col = BOLoader.Instance.GetBusinessObjectCol<ContactPersonTestBO>("FirstName = aaaa", orderBySurname);
//            Assert.AreEqual(0, col.Count);
//        }

//        // Also need to test this works with Sql Server and other Guid forms
//        [Test]
//        public void TestGetBusinessObjectByIDGuid()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadDefaultClassDef();

//            ContactPersonTestBO cp1 = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
//            ContactPersonTestBO cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cp1.ContactPersonID);
//            Assert.AreEqual(cp1, cp2);
//        }

//        [Test]
//        public void TestGetBusinessObjectByIDInt()
//        {
//            ClassDef.ClassDefs.Clear();
//            TestAutoInc.LoadClassDefWithAutoIncrementingID();

//            TestAutoInc tai1 = BOLoader.Instance.GetBusinessObject<TestAutoInc>("TestAutoIncID = 1");
//            TestAutoInc tai2 = BOLoader.Instance.GetBusinessObjectByID<TestAutoInc>(tai1.TestAutoIncID.Value);
//            Assert.AreEqual(tai1, tai2);
//            Assert.AreEqual("testing", tai2.TestField);
//        }

//        // For this test we pretend for a moment that the Surname column is the primary key
//        //   (can change this when we add a table with a real string primary key)
//        [Test]
//        public void TestGetBusinessObjectByIDString()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadClassDefWithSurnameAsPrimaryKey();

//            ContactPersonTestBO cp1 = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
//            ContactPersonTestBO cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cp1.Surname);
//            Assert.AreEqual(cp1, cp2);

//            cp1.Surname = "a b";
//            cp1.Save();
//            cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>("a b");
//            Assert.AreEqual(cp1, cp2);
//            cp1.Surname = "abc";
//            cp1.Save();
//        }

//        [Test]
//        public void TestGetBusinessObjectByIDPrimaryKey()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();

//            ContactPersonTestBO cp1 = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
//            ContactPersonTestBO cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cp1.PrimaryKey);
//            Assert.AreEqual(cp1, cp2);
//        }

//        [Test, ExpectedException(typeof(InvalidPropertyException))]
//        public void TestGetBusinessObjectByIDWithMultiplePrimaryKeys()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonCompositeKey.LoadClassDefs();

//            BOLoader.Instance.GetBusinessObjectByID<ContactPersonCompositeKey>("someIDvalue");
//        }

//        [Test]
//        public void TestLoadMethod()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadDefaultClassDef();

//            ContactPersonTestBO cp1 = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
//            cp1.Surname = "def";
//            BOLoader.Instance.Load(cp1, new Parameter("Surname = abc"));
//            Assert.AreEqual("abc", cp1.Surname);
//        }

//        [Test, Ignore("This functionality has been removed. Need to determine how to handle BO's loading from diff databases in future")]
//        public void TestSetDatabaseConnection()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadDefaultClassDef();

//            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
////            Assert.IsNotNull(cp.GetDatabaseConnection());
////            BOLoader.Instance.SetDatabaseConnection(cp, null);
////            Assert.IsNull(cp.GetDatabaseConnection());
//        }

//        [Test]
//        public void TestBOLoaderRefreshCallsAfterLoad()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadDefaultClassDef();

//            ContactPersonTestBO cp = new ContactPersonTestBO();
//            cp.Surname = Guid.NewGuid().ToString();
//            cp.Save();
//            Assert.IsFalse(cp.AfterLoadCalled);

//            BOLoader.Instance.Refresh(cp);

//            Assert.IsTrue(cp.AfterLoadCalled);
//        }


//        [Test]
//        public void TestBoLoaderCallsAfterLoad_LoadingCollection()
//        {
//            ClassDef.ClassDefs.Clear();
//            //--------SetUp testpack --------------------------
//            ContactPersonTestBO.LoadDefaultClassDef();
//            OrderCriteria orderBySurname = OrderCriteria.FromString("Surname");
//            //----Execute Test---------------------------------
//            BusinessObjectCollection<ContactPersonTestBO> col = BOLoader.Instance.GetBusinessObjectCol<ContactPersonTestBO>("FirstName = aa", orderBySurname);
//            //----Test Result----------------------------------
//            Assert.AreEqual(2, col.Count);
//            ContactPersonTestBO bo = col[0];
//            Assert.IsTrue(bo.AfterLoadCalled);
//        }

//        [Test]
//        public void TestBoLoaderCallsAfterLoad_ViaSearchCriteria()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadDefaultClassDef();

//            ContactPersonTestBO cp = new ContactPersonTestBO();
//            Assert.IsFalse(cp.AfterLoadCalled);
//            cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc AND FirstName = aa");
//            Assert.IsTrue(cp.AfterLoadCalled);
//        }
//        [Test]
//        public void TestBoLoaderCallsAfterLoad_CompositePrimaryKey()
//        {
//            ClassDef.ClassDefs.Clear();
//            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
//            ContactPersonTestBO cpTemp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
//            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.PrimaryKey);
//            Assert.IsTrue(cp.AfterLoadCalled);
//        }
//        [Test]
//        public void TestBoLoader_LoadBusinessObjectDeletedByAnotherUser()
//        {
//            //-------------Setup Test Pack
//            ContactPersonTestBO cpTemp = GetContactPersonSavedToDBButNotInObjectManager();

//            //-------------Execute test ---------------------
//            cpTemp.Delete();
//            cpTemp.Save();
//            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.PrimaryKey);
//            //-------------Test Result ---------------------
//            Assert.IsNull(cp);
//        }
//        [Test]
//        [ExpectedException(typeof(BusObjDeleteConcurrencyControlException))]
//        public void TestBoLoader_RefreshBusinessObjectDeletedByAnotherUser()
//        {
//            //-------------Setup Test Pack
//            ContactPersonTestBO cpTemp = GetContactPersonSavedToDBButNotInObjectManager();

//            //-------------Execute test ---------------------
//            cpTemp.Delete();
//            cpTemp.Save();
//            //-------------Execute Test ---------------------
//            try
//            {
//                BOLoader.Instance.Refresh(cpTemp);
//                Assert.Fail();
//            }
//            //-------------Test Result ---------------------
//            catch (BusObjDeleteConcurrencyControlException ex)
//            {
//                Assert.IsTrue(ex.Message.Contains("A Error has occured since the object you are trying to refresh has been deleted by another user "));
//                throw;
//            }
//        }
//        [Test]
//        public void TestBOLoader_RefreshObjects_WhenRetrievingFromObjectManager()
//        {
//            //-------------Setup Test Pack
//            //Create and save a person
//            ContactPersonTestBO cpTemp = CreateSavedContactPerson();
//            //Clear the object manager so as to simulate a different user
//            ContactPersonTestBO.ClearLoadedBusinessObjectBaseCol();
//            Assert.AreEqual(0, BusObjectManager.Instance.Count);
//            //Get the person from the object manager so as to ensure that they are loaded 
//            // into the object manager.
//            ContactPersonTestBO cpTemp2 =
//                BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
//            //Assert.AreEqual(1, BusObjectManager.Instance.Count);
//            //-------------Execute test ---------------------
//            cpTemp.Surname = "New Surname";
//            cpTemp.Save();
//            Assert.AreNotEqual(cpTemp2.Surname, cpTemp.Surname);

//            ContactPersonTestBO cpTemp3 =
//                BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
//            //Assert.AreEqual(1, BusObjectManager.Instance.Count);
//            //-------------Test Result ---------------------
//            Assert.AreEqual(cpTemp.Surname, cpTemp3.Surname);
//            Assert.AreSame(cpTemp2.Surname, cpTemp3.Surname);

//        }
//        [Test]
//        public void TestBOLoader_GetObjectFromObjectManager()
//        {
//            //-------------Setup Test Pack
//            //Create and save a person
//            ContactPersonTestBO cpTemp = CreateSavedContactPerson();
//            //Clear the object manager so as to simulate a different user
//            ContactPersonTestBO.ClearLoadedBusinessObjectBaseCol();
//            Assert.AreEqual(0, BusObjectManager.Instance.Count);
//            //Get the person from the object manager so as to ensure that they are loaded 
//            // into the object manager.
//            //-------------Execute test --------------------
//            BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
//            Assert.AreEqual(1, BusObjectManager.Instance.Count);

//        }
//        [Test]
//        public void TestBOLoader_GetObjectFromObjectManager_Twice()
//        {
//            //-------------Setup Test Pack
//            //Create and save a person
//            ContactPersonTestBO cpTemp = CreateSavedContactPerson();
//            //Clear the object manager so as to simulate a different user
//            ContactPersonTestBO.ClearLoadedBusinessObjectBaseCol();
//            Assert.AreEqual(0, BusObjectManager.Instance.Count);
//            //Get the person from the object manager so as to ensure that they are loaded 
//            // into the object manager.
//            //-------------Execute test --------------------
//            BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
//            BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
//            //-------------Test Result ---------------------
//            Assert.AreEqual(1, BusObjectManager.Instance.Count);
//        }

//        [Test]
//        public void TestBOLoader_DoesNotRefreshDirtyObjects_WhenRetrievingFromObjectManager()
//        {
//            //-------------Setup Test Pack ------------------
//            ContactPersonTestBO cpTemp = CreateSavedContactPerson();

//            //-------------Execute test ---------------------
//            string newSurnameValue = "New Surname";
//            cpTemp.Surname = newSurnameValue;
//            cpTemp.AfterLoadCalled = false;
//            Assert.IsFalse(cpTemp.AfterLoadCalled); 
//            ContactPersonTestBO cpTemp2 =
//                BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
//            //-------------Test Result ----------------------
//            Assert.AreSame(cpTemp2, cpTemp2);
//            Assert.AreEqual(newSurnameValue, cpTemp2.Surname);
//            Assert.IsFalse(cpTemp2.AfterLoadCalled, "After load should not be called for a dirty object being loaded from DB"); 
//        }

//        [Test]
//        public void TestGetLoadedBusinessObject()
//        {
//            //-------------Setup Test Pack ------------------
//            ContactPersonTestBO contactPersonTestBO = CreateSavedContactPerson();
//            //-------------Execute test ---------------------
//            IBusinessObject businessObject = BOLoader.Instance.GetLoadedBusinessObject(contactPersonTestBO.PrimaryKey);
//            //-------------Test Result ----------------------
//            Assert.AreSame(contactPersonTestBO, businessObject);
//        }

//        [Test]
//        public void TestGetLoadedBusinessObject_DoesNotRefreshNewBo()
//        {
//            //-------------Setup Test Pack ------------------
//            ContactPerson contactPerson = new ContactPerson();
//            //-------------Execute test ---------------------
//            IBusinessObject businessObject = BOLoader.Instance.GetLoadedBusinessObject(contactPerson.PrimaryKey);
//            //-------------Test Result ----------------------
//            Assert.AreSame(contactPerson, businessObject);
//        }

//        #region HelperMethods

//        private static ContactPersonTestBO GetContactPersonSavedToDBButNotInObjectManager()
//        {
//            ContactPersonTestBO cpTemp = CreateSavedContactPerson();
//            //Clear the loaded busiess object so that the we can simulate a user on another machine deleting this object
//            BusObjectManager.Instance.ClearLoadedObjects();
//            return cpTemp;
//        }

//        private static ContactPersonTestBO CreateSavedContactPerson()
//        {
//            ClassDef.ClassDefs.Clear();
//            BusObjectManager.Instance.ClearLoadedObjects();
//            ContactPersonTestBO.LoadDefaultClassDef();
//            //Create a contact person
//            ContactPersonTestBO cpTemp = new ContactPersonTestBO();
//            cpTemp.Surname = Guid.NewGuid().ToString();
//            cpTemp.Save();
//            return cpTemp;
//        }
}
