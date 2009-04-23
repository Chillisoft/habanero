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
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB4O;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    /// <summary>
    ///Test Business object loading individual business objects. 
    /// </summary>
    public abstract class TestBusinessObjectLoader
    {
        protected abstract void SetupDataAccessor();

        protected abstract void DeleteEnginesAndCars();

        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            SetupDataAccessor();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
            new Address();
        }

        [TearDown]
        public virtual void TearDownTest()
        {
        }

        [Test]
        public void Test_GetBusinessObject_ManualCreatePrimaryKey()
        {
            BOWithIntID.DeleteAllBOWithIntID();
            ClassDef.ClassDefs.Clear();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo1 = new BOWithIntID {TestField = "PropValue", IntID = 55};
            bo1.Save();
            IPrimaryKey id = BOPrimaryKey.CreateWithValue(autoIncClassDef, bo1.IntID);
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo1.Status.IsNew);
            Assert.IsNotNull(bo1.IntID);
            //---------------Execute Test ----------------------
            BOWithIntID returnedBO =
                (BOWithIntID) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(autoIncClassDef, id);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnedBO);
        }

        [Test]
        public void Test_GetBusinessObjectByValue()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID {TestField = "PropValue", IntID = 55};
            object expectedID = bo.IntID;
            bo.Save();
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsNotNull(bo.IntID);
            //---------------Execute Test ----------------------
            IBusinessObject returnedBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue
                (autoIncClassDef, expectedID);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnedBO);
        }

        [Test]
        public void Test_GetBusinessObjectByValue_DoesNotExist()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID {TestField = "PropValue", IntID = 55};
            bo.Save();
            const int idDoesNotExist = 5425;
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsNotNull(bo.IntID);
            //---------------Execute Test ----------------------
            try
            {
#pragma warning disable 168
                IBusinessObject returnedBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue
                    (autoIncClassDef, idDoesNotExist);
#pragma warning restore 168
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                StringAssert.Contains
                    ("A Error has occured since the object you are trying to refresh has been deleted by another user",
                     ex.Message);
            }
        }

        [Test]
        public void Test_GetBusinessObject_ManualCreatePrimaryKey_ByType()
        {
            BOWithIntID.DeleteAllBOWithIntID();
            ClassDef.ClassDefs.Clear();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo1 = new BOWithIntID {TestField = "PropValue", IntID = 55};
            bo1.Save();
            IPrimaryKey id = BOPrimaryKey.CreateWithValue(typeof (BOWithIntID), bo1.IntID);
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo1.Status.IsNew);
            Assert.IsNotNull(bo1.IntID);
            //---------------Execute Test ----------------------
            BOWithIntID returnedBO =
                (BOWithIntID) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(autoIncClassDef, id);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnedBO);
        }

        [Test]
        public void Test_GetBusinessObjectByValue_ByType()
        {
            ClassDef.ClassDefs.Clear();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID {TestField = "PropValue", IntID = 55};
            object expectedID = bo.IntID;
            bo.Save();
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsNotNull(bo.IntID);
            //---------------Execute Test ----------------------
            IBusinessObject returnedBO =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue(typeof (BOWithIntID), expectedID);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnedBO);
        }
#pragma warning disable 168
        [Test]
        public void Test_GetBusinessObjectByValue_ByType_DoesNotExist()
        {
            ClassDef.ClassDefs.Clear();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID {TestField = "PropValue", IntID = 55};
            bo.Save();
            const int idDoesNotExist = 5425;
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsNotNull(bo.IntID);
            //---------------Execute Test ----------------------
            try
            {
                IBusinessObject returnedBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue
                    (typeof (BOWithIntID), idDoesNotExist);

                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                StringAssert.Contains
                    ("A Error has occured since the object you are trying to refresh has been deleted by another user. There are no records in the database for the Class: BOWithIntID identified by",
                     ex.Message);
            }
        }
        [Test]
        public void Test_GetBusinessObjectByValue_Generic()
        {
            ClassDef.ClassDefs.Clear();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID { TestField = "PropValue", IntID = 55 };
            object expectedID = bo.IntID;
            bo.Save();
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsNotNull(bo.IntID);
            //---------------Execute Test ----------------------
            IBusinessObject returnedBO =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue<BOWithIntID>( expectedID);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnedBO);
        }

        [Test]
        public void Test_GetBusinessObjectByValue_Generic_DoesNotExist()
        {
            ClassDef.ClassDefs.Clear();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID { TestField = "PropValue", IntID = 55 };
            bo.Save();
            const int idDoesNotExist = 5425;
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsNotNull(bo.IntID);
            //---------------Execute Test ----------------------
            try
            {
                IBusinessObject returnedBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue
                    <BOWithIntID>(idDoesNotExist);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                StringAssert.Contains
                    ("A Error has occured since the object you are trying to refresh has been deleted by another user. There are no records in the database for the Class: BOWithIntID identified by",
                     ex.Message);
            }
        }
#pragma warning restore 168

        [Test]
        public void TestGetBusinessObjectWhenNotExists_NotLoadedViaKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria
                ("ContactPersonID", Criteria.ComparisonOp.Equals, Guid.NewGuid().ToString("N"));

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
            Criteria criteria = new Criteria
                ("ContactPersonID", Criteria.ComparisonOp.Equals, Guid.NewGuid().ToString("N"));

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
            const string surname = "Surname";
            const string surnameField = "Surname_field";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Equals, cp.Surname);
            criteria.Field.Source = new Source(cp.ClassDef.ClassName);
            criteria.Field.FieldName = surnameField;
            SelectQuery query = new SelectQuery(criteria);

            query.Fields.Add(surname, new QueryField(surname, surnameField, null));
            query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
            query.Source = new Source(cp.ClassDef.ClassName);
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
                (ContactPersonTestBO) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(cp.ClassDef, query);

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
            ContactPersonTestBO loadedCP =
                (ContactPersonTestBO) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, cp.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
        }

        [Test]
        public void TestGetBusinessObject_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
        }

        [Test]
        public void TestMethod()
        {
            //---------------Assert Precondition----------------
        }

        [Test]
        public void TestGetBusinessObjectByIDGuid()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO cp1 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
            ContactPersonTestBO cp2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp1.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(cp1, cp2);
            Assert.AreSame(cp, cp2);
        }

        [Test]
        public void TestGetBusinessObject_CriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                (ContactPersonTestBO) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);

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
            personToDelete.MarkForDelete();
            personToDelete.Save();

            //Ensure that a fresh object is loaded from DB
            BusinessObjectManager.Instance.ClearLoadedObjects();

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
            personToDelete.MarkForDelete();
            personToDelete.Save();

            //Ensure that a fresh object is loaded from DB
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //--------Execute------------------------------------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(personToDelete.ClassDef, personToDelete.ID);
        }

        [Test]
        public void TestGetBusinessObjectDuplicatesException_IfTryLoadObjectWithCriteriaThatMatchMoreThanOne_Generic()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO.CreateSavedContactPerson(surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aa");

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("FirstName", Criteria.ComparisonOp.Equals, firstName);

            try
            {
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("There was an error with loading the class", ex.Message);
                StringAssert.Contains("returned more than one record when only one was expected", ex.DeveloperMessage);
            }
        }

        [Test]
        public void TestGetBusinessObjectDuplicatesException_IfTryLoadObjectWithCriteriaThatMatchMoreThanOne()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();

            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO.CreateSavedContactPerson(surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aa");

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("FirstName", Criteria.ComparisonOp.Equals, firstName);

            try
            {
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("There was an error with loading the class", ex.Message);
                StringAssert.Contains("returned more than one record when only one was expected", ex.DeveloperMessage);
            }
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
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);
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

        [Test]
        public void TestGetRelatedBusinessObject()
        {
            //---------------Set up test pack-------------------
            Car car = Car.CreateSavedCar("5");
            Engine engine = Engine.CreateSavedEngine(car, "20");

            //---------------Execute Test ----------------------
            Car loadedCar = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject
                (engine.Relationships.GetSingle<Car>("Car"));

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
            Car loadedCar =
                (Car)
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject
                    ((ISingleRelationship) engine.Relationships["Car"]);

            //---------------Test Result -----------------------
            Assert.AreSame(car, loadedCar);
        }

        [Test]
        public void TestGetRelatedBusinessObjectCollection_Generic()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
            ContactPersonTestBO cp = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);
            address.ContactPersonID = cp.ContactPersonID;
            address.Save();

            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<AddressTestBO> addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<AddressTestBO>
                    ((IMultipleRelationship) cp.Relationships["Addresses"]);

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
            AddressTestBO address = new AddressTestBO();
            address.ContactPersonID = cp.ContactPersonID;
            address.Save();

            //---------------Assert PreConditions---------------    

            //---------------Execute Test ----------------------

            IBusinessObjectCollection addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection
                    (typeof (AddressTestBO), cp.Relationships.GetMultiple("Addresses"));

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
            AddressTestBO address1 = AddressTestBO.CreateSavedAddress(cp.ContactPersonID, "ffff");
            AddressTestBO address2 = AddressTestBO.CreateSavedAddress(cp.ContactPersonID, "bbbb");

            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<AddressTestBO> addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<AddressTestBO>
                    (cp.Relationships.GetMultiple("Addresses"));

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

            IBusinessObjectCollection addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection
                    (typeof (AddressTestBO), cp.Relationships.GetMultiple("Addresses"));
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
            IBusinessObjectCollection addresses = cp.Relationships.GetMultiple("Addresses").BusinessObjectCollection;
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
        public void TestLoadThroughRelationship_Multiple()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteDoNothing();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            AddressTestBO address = AddressTestBO.CreateSavedAddress(cp.ContactPersonID);

            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<AddressTestBO> addresses = cp.Addresses;

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


        public void Test3LayersLoadRelated_LoadsObjectFromObjectManager()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            OrganisationTestBO.LoadDefaultClassDef();

            OrganisationTestBO org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = org.Relationships.GetMultiple("ContactPeople").BusinessObjectCollection;
            ContactPersonTestBO loadedContactPerson = (ContactPersonTestBO) col[0];
            IBusinessObjectCollection colAddresses =
                loadedContactPerson.Relationships.GetMultiple("Addresses").BusinessObjectCollection;
            IBusinessObject loadedAddressTestBO = colAddresses[0];

            //---------------Test Result -----------------------
            Assert.AreSame(loadedContactPerson, contactPersonTestBO);
            Assert.AreSame(loadedAddressTestBO, address);
        }

        [Test, ExpectedException(typeof (InvalidPropertyNameException))]
        public virtual void TestGetBusinessObject_PropNameNotCorrect()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.CreateSavedContactPersonNoAddresses();

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("NonExistantProperty", Criteria.ComparisonOp.Equals, "1");
            BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
        }

        [Test]
        public void TestBoLoader_NotRefreshBusinessObjectIfCurrentlyBeingEdited()
        {
            //-------------Setup Test Pack------------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cpTemp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            ContactPersonTestBO cpLoaded =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cpTemp.ID);
            cpLoaded.FirstName = TestUtil.GetRandomString();

            //-------------Assert Preconditon ---------------
            Assert.IsTrue(cpLoaded.Status.IsEditing);

            //-------------Execute Test ---------------------
            try
            {
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpLoaded);
                Assert.Fail();
            }
                //-------------Test Result ---------------------
            catch (HabaneroDeveloperException ex)
            {
                Assert.IsTrue
                    (ex.Message.Contains("A Error has occured since the object being refreshed is being edited."));
                Assert.IsTrue
                    (ex.DeveloperMessage.Contains
                         ("A Error has occured since the object being refreshed is being edited."));
                Assert.IsTrue(ex.DeveloperMessage.Contains(cpLoaded.ID.AsString_CurrentValue()));
                Assert.IsTrue(ex.DeveloperMessage.Contains(cpLoaded.ClassDef.ClassName));
            }
        }

        [Test]
        public void TestRefresh_DoesNotRefreshNewBo()
        {
            //-------------Setup Test Pack ------------------
            new Engine();
            new Car();
            ContactPerson contactPerson = new ContactPerson();

            //-------------Execute test ---------------------
            IBusinessObject businessObject = BORegistry.DataAccessor.BusinessObjectLoader.Refresh(contactPerson);

            //-------------Test Result ----------------------
            Assert.AreSame(contactPerson, businessObject);
            Assert.IsTrue(businessObject.Status.IsNew);
        }

        //TODO: refresh dirty object if correct method called need a strategy for this?.
        // should restore just cancel edits or restore from the DB strategy for this too.
        [Test]
        public void TestGetDirtyObject_DoesNotReloadFromDatabase()
        {
            //---------------Set up test pack-------------------
            ContactPerson cp = ContactPerson.CreateSavedContactPerson();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPerson cpLoaded = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                (cp.ID);
            string newSurname = TestUtil.GetRandomString();
            cpLoaded.Surname = newSurname;

            //---------------Assert Precondition----------------
            Assert.IsTrue(cpLoaded.Status.IsEditing);
            Assert.AreEqual(newSurname, cpLoaded.Surname);

            //---------------Execute Test ----------------------
            ContactPerson cpLoaded2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                (cp.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(cpLoaded2, cpLoaded);
            Assert.AreEqual(newSurname, cpLoaded2.Surname);
            Assert.IsTrue(cpLoaded.Status.IsEditing);
        }

        [Test]
        public void Test_GetCount()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");
            ContactPersonTestBO.CreateSavedContactPerson("bbbb", "bbb");
            ContactPersonTestBO.CreateSavedContactPerson("cccc", "ccc");
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            int count = BORegistry.DataAccessor.BusinessObjectLoader.GetCount(classDef, null);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, count);
        }

        #region Nested type: TestBusinessObjectLoaderDB

        [TestFixture]
        public class TestBusinessObjectLoaderDB : TestBusinessObjectLoader
        {
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
                BOWithIntID.DeleteAllBOWithIntID();
            }

            [Test]
            public void TestGetBusinessObjectByIDInt()
            {
                //---------------Set up test pack-------------------
                ClassDef.ClassDefs.Clear();
                TestAutoInc.LoadClassDefWithAutoIncrementingID();

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("testautoincid", Criteria.ComparisonOp.Equals, "1");
                TestAutoInc tai1 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TestAutoInc>(criteria);
                TestAutoInc tai2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TestAutoInc>(tai1.ID);

                //---------------Test Result -----------------------
                Assert.AreSame(tai1, tai2);
                Assert.AreEqual("testing", tai2.TestField);
            }

            [Test]
            public void TestGetBusinessObjectByIDInt_CriteriaString()
            {
                //---------------Set up test pack-------------------
                ClassDef.ClassDefs.Clear();
                TestAutoInc.LoadClassDefWithAutoIncrementingID();

                //---------------Execute Test ----------------------
                TestAutoInc tai1 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TestAutoInc>
                    ("testautoincid = 1");
                TestAutoInc tai2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TestAutoInc>(tai1.ID);

                //---------------Test Result -----------------------
                Assert.IsNotNull(tai1);
                Assert.AreSame(tai1, tai2);
                Assert.AreEqual("testing", tai2.TestField);
            }

            [Test]
            public void TestGetBusinessObjectByIDInt_CriteriaString_Untyped()
            {
                //---------------Set up test pack-------------------
                ClassDef.ClassDefs.Clear();
                ClassDef classDef = TestAutoInc.LoadClassDefWithAutoIncrementingID();

                //---------------Execute Test ----------------------
                TestAutoInc tai1 =
                    (TestAutoInc)
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, "testautoincid = 1");
                TestAutoInc tai2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TestAutoInc>(tai1.ID);

                //---------------Test Result -----------------------
                Assert.IsNotNull(tai1);
                Assert.AreSame(tai1, tai2);
                Assert.AreEqual("testing", tai2.TestField);
            }

            [Test]
            public void TestGetBusinessObjectByIDIntSavenewAutoIncNumber()
            {
                //---------------Set up test pack-------------------
                ClassDef.ClassDefs.Clear();
                TestAutoInc.LoadClassDefWithAutoIncrementingID();
                TestAutoInc autoInc = new TestAutoInc();
                autoInc.TestAutoIncID = int.MaxValue;
                autoInc.Save();

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("testautoincid", Criteria.ComparisonOp.Equals, autoInc.TestAutoIncID);
                TestAutoInc tai1 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TestAutoInc>(criteria);
                TestAutoInc tai2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TestAutoInc>(tai1.ID);

                ////---------------Test Result -----------------------
                Assert.AreSame(tai1, tai2);
                Assert.AreEqual("testing", tai2.TestField);
            }

            //[Test]
            //public void Test_BusinessObjectClearsItselfFromObjectManager()
            //{
            //    //---------------Set up test pack-------------------
            //    BusinessObjectManager.Instance.ClearLoadedObjects();
            //    WaitForGC();
            //    ClassDef.ClassDefs.Clear();
            //    ContactPersonTestBO.LoadDefaultClassDef();
            //    const string surname = "abc";
            //    const string firstName = "aa";
            //    ContactPersonTestBO savedContactPerson = CreateSavedContactPerson(surname, firstName);

            //    //---------------Assert Precondition----------------
            //    Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            //    Assert.IsNotNull(savedContactPerson);

            //    //---------------Execute Test ----------------------
            //    savedContactPerson = null;
            //    WaitForGC();

            //    //---------------Test Result -----------------------
            //    Assert.IsNull(savedContactPerson);
            //    Assert.AreEqual(0, BusinessObjectManager.Instance.Count);
            //}

            [Test]
            public void Test_ReturnSameObjectFromBusinessObjectLoader()
            {
                //---------------Set up test pack-------------------
                //------------------------------Setup Test
                new Engine();
                new Car();
                ContactPerson originalContactPerson = new ContactPerson();
                originalContactPerson.Surname = "FirstSurname";
                originalContactPerson.Save();

                BusinessObjectManager.Instance.ClearLoadedObjects();

                //load second object from DB to ensure that it is now in the object manager
                ContactPerson myContact2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                        (originalContactPerson.ID);

                //---------------Assert Precondition----------------
                Assert.AreNotSame(originalContactPerson, myContact2);

                //---------------Execute Test ----------------------
                ContactPerson myContact3 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                        (originalContactPerson.ID);

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
                BusinessObjectManager.Instance.ClearLoadedObjects();
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
                BusinessObjectManager.Instance.ClearLoadedObjects();
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


            /// <summary>
            /// Tests to ensure that if the object has been edited by another user
            ///  and the default strategy to reload has been replaced then one we do not get back is always the latest.
            /// Note: This behaviour must be made configurable using a strategy TestGetTheFreshestObject_Strategy test 
            /// </summary>
            [Test, Ignore("Need to implement via a strategy")]
            public void TestDontGetTheFreshestObject_Strategy()
            {
                //------------------------------Setup Test
                ClassDef.ClassDefs.Clear();
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
                originalContactPerson.Surname = "FirstSurname";
                originalContactPerson.Save();
                IPrimaryKey origCPID = originalContactPerson.ID;

                BusinessObjectManager.Instance.ClearLoadedObjects();

                //load second object from DB to ensure that it is now in the object manager
                ContactPersonTestBO myContact2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(origCPID);

                //-----------------------------Execute Test-------------------------
                //Edit first object and save
                originalContactPerson.Surname = "SecondSurname";
                originalContactPerson.Save();

                ContactPersonTestBO myContact3 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(origCPID);

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
                BusinessObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();

                const string surname = "abc";
                const string firstName = "aa";
                ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
                ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson
                    (surname, firstName);
                ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

                //---------------Assert Precondition----------------
                //Object loaded in object manager
                Assert.AreEqual(3, BusinessObjectManager.Instance.Count);
                Assert.IsTrue(BusinessObjectManager.Instance.Contains(savedContactPerson.ID));

                //---------------Execute Test ----------------------
                Criteria criteria1 = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);
                Criteria criteria2 = new Criteria("FirstName", Criteria.ComparisonOp.Equals, firstName);
                Criteria criteria = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);
                ContactPersonTestBO cp =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

                //---------------Test Result -----------------------
                Assert.AreSame(savedContactPerson, cp);

                Assert.AreEqual(surname, cp.Surname);
                Assert.AreEqual(firstName, cp.FirstName);
                Assert.IsFalse(cp.Status.IsNew);
                Assert.IsFalse(cp.Status.IsDirty);
                Assert.IsFalse(cp.Status.IsDeleted);
            }

            [Test]
            public void TestGetBusinessObject_MultipleCriteria_CriteriaString()
            {
                //---------------Set up test pack-------------------
                ClassDef.ClassDefs.Clear();
                ContactPersonTestBO.LoadDefaultClassDef();
                BusinessObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();

                const string surname = "abc";
                const string firstName = "aa";
                ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
                ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson
                    (surname, firstName);
                ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

                //---------------Assert Precondition----------------
                //Object loaded in object manager
                Assert.AreEqual(3, BusinessObjectManager.Instance.Count);
                Assert.IsTrue(BusinessObjectManager.Instance.Contains(savedContactPerson.ID));

                //---------------Execute Test ----------------------
                ContactPersonTestBO cp =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                        ("Surname = " + surname + " AND FirstName = " + firstName);

                //---------------Test Result -----------------------
                Assert.AreSame(savedContactPerson, cp);

                Assert.AreEqual(surname, cp.Surname);
                Assert.AreEqual(firstName, cp.FirstName);
                Assert.IsFalse(cp.Status.IsNew);
                Assert.IsFalse(cp.Status.IsDirty);
                Assert.IsFalse(cp.Status.IsDeleted);
            }

            [Test]
            public void TestGetBusinessObject_MultipleCriteria_CriteriaString_Untyped()
            {
                //---------------Set up test pack-------------------
                ClassDef.ClassDefs.Clear();
                ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
                BusinessObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();

                const string surname = "abc";
                const string firstName = "aa";
                ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
                ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson
                    (surname, firstName);
                ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

                //---------------Assert Precondition----------------
                //Object loaded in object manager
                Assert.AreEqual(3, BusinessObjectManager.Instance.Count);
                Assert.IsTrue(BusinessObjectManager.Instance.Contains(savedContactPerson.ID));

                //---------------Execute Test ----------------------
                ContactPersonTestBO cp =
                    (ContactPersonTestBO)
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject
                        (classDef, "Surname = " + surname + " AND FirstName = " + firstName);

                //---------------Test Result -----------------------
                Assert.AreSame(savedContactPerson, cp);

                Assert.AreEqual(surname, cp.Surname);
                Assert.AreEqual(firstName, cp.FirstName);
                Assert.IsFalse(cp.Status.IsNew);
                Assert.IsFalse(cp.Status.IsDirty);
                Assert.IsFalse(cp.Status.IsDeleted);
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

                BusinessObjectManager.Instance.ClearLoadedObjects();
                WaitForGC();

                SelectQuery query = CreateSelectQuery(cp);
//                new SelectQuery(new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname)));
//                query.Fields.Add("Surname", new QueryField("Surname", "Surname_field", null));
//                query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
//                query.Source = new Source(cp.ClassDef.TableName);

                //---------------Assert Precondition ---------------
                //Object not loaded in all loaded business objects
                Assert.AreEqual(0, BusinessObjectManager.Instance.Count);

                //---------------Execute Test ----------------------
                ContactPersonTestBO loadedCp =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(query);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
                Assert.AreNotSame(loadedCp, cp);
                Assert.AreEqual(cp.ContactPersonID, loadedCp.ContactPersonID);
                Assert.AreEqual(cp.Surname, loadedCp.Surname);
                Assert.IsTrue(String.IsNullOrEmpty(loadedCp.FirstName), "Firstname is not being loaded");
                // not being loaded
                Assert.IsFalse(loadedCp.Status.IsNew);
                Assert.IsFalse(loadedCp.Status.IsDeleted);
                Assert.IsFalse(loadedCp.Status.IsDirty);
                Assert.IsTrue(loadedCp.Status.IsValid());
            }

            [Test]
            public void TestLoadFromDatabaseAlwaysLoadsSameObject()
            {
                //---------------Set up test pack-------------------
                new Engine();
                new Car();
                ContactPerson originalContactPerson = new ContactPerson();
                const string firstSurname = "FirstSurname";
                originalContactPerson.Surname = firstSurname;
                originalContactPerson.Save();
                IPrimaryKey origConactPersonID = originalContactPerson.ID;
                originalContactPerson = null;
                BusinessObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();

                //load second object from DB to ensure that it is now in the object manager
                ContactPerson loadedContactPerson1 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(origConactPersonID);

                //---------------Assert Precondition----------------
                Assert.IsNull(originalContactPerson);
                Assert.AreEqual(firstSurname, loadedContactPerson1.Surname);
                Assert.AreNotSame(originalContactPerson, loadedContactPerson1);
                Assert.AreEqual(1, BusinessObjectManager.Instance.Count);

                //---------------Execute Test ----------------------

                ContactPerson loadedContactPerson2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(origConactPersonID);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
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
            [Test]
            public void TestGetTheFreshestObject_Strategy()
            {
                //------------------------------Setup Test----------------------------
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
                originalContactPerson.Surname = "FirstSurname";
                originalContactPerson.Save();

                BusinessObjectManager.Instance.ClearLoadedObjects();

                //load second object from DB to ensure that it is now in the object manager
                ContactPersonTestBO myContact2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                        (originalContactPerson.ID);

                //-----------------------------Assert Precondition -----------------
                Assert.AreNotSame(originalContactPerson, myContact2);
                IPrimaryKey id = myContact2.ID;
                Assert.IsTrue(BusinessObjectManager.Instance.Contains(id));
                IBusinessObject boFromAllLoadedObjects = BusinessObjectManager.Instance[id];
                Assert.AreSame(boFromAllLoadedObjects, myContact2);
                Assert.IsFalse(myContact2.Status.IsEditing);

                //-----------------------------Execute Test-------------------------
                //Edit first object and save
                originalContactPerson.Surname = "SecondSurname";
                originalContactPerson.Save();

                ContactPersonTestBO myContact3 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                        (originalContactPerson.ID);

                //-----------------------------Assert Result-----------------------
                Assert.IsFalse(myContact3.Status.IsEditing);
                Assert.AreNotSame(originalContactPerson, myContact3);
                Assert.IsTrue(BusinessObjectManager.Instance.Contains(myContact3));
                Assert.AreSame(myContact3, myContact2);
                //The two surnames should be equal since the myContact3 was refreshed
                // when it was loaded.
                Assert.AreEqual(originalContactPerson.Surname, myContact3.Surname);
                //Just to check the myContact2 should also match since it is physically the 
                // same object as myContact3
                Assert.AreEqual(originalContactPerson.Surname, myContact2.Surname);
            }

            [Test]
            public void TestLoadedObjectIsAddedToObjectManager()
            {
                //---------------Set up test pack-------------------
                ContactPerson.DeleteAllContactPeople();
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO contactPerson1 = ContactPersonTestBO.CreateSavedContactPerson
                    (Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
                BusinessObjectManager.Instance.ClearLoadedObjects();

                //---------------Assert Precondition----------------
                Assert.AreEqual(0, BusinessObjectManager.Instance.Count);

                //---------------Execute Test ----------------------
                ContactPersonTestBO contactPerson =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                        (contactPerson1.ID);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
                Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPerson));
            }

            [Test]
            public void Test_RefreshCallsAfterLoadNotCalledIfObjectNotReloaded()
            {
                //---------------Set up test pack---------------------
                ClassDef.ClassDefs.Clear();
                ContactPersonTestBO.LoadDefaultClassDef();

                ContactPersonTestBO cp = new ContactPersonTestBO();
                cp.Surname = Guid.NewGuid().ToString();
                cp.Save();

                //---------------Assert Precondition------------------
                Assert.IsTrue(BusinessObjectManager.Instance.Contains(cp));
                Assert.IsFalse(cp.AfterLoadCalled);

                //---------------Execute Test ------------------------
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cp);

                //---------------Test Result -------------------------
                Assert.IsTrue(cp.AfterLoadCalled);
            }

            [Test]
            public void Test_RefreshCallsAfterLoadCalledIfObjectLoaded()
            {
                //---------------Set up test pack---------------------
                ClassDef.ClassDefs.Clear();
                ContactPersonTestBO.LoadDefaultClassDef();

                ContactPersonTestBO cp = new ContactPersonTestBO();
                cp.Surname = Guid.NewGuid().ToString();
                cp.Save();
                BusinessObjectManager.Instance.ClearLoadedObjects();

                ContactPersonTestBO cpLoaded =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.ID);
                string newFirstname = cp.FirstName = TestUtil.GetRandomString();
                cp.Save();
                cpLoaded.AfterLoadCalled = false;

                //---------------Assert Precondition------------------
                Assert.IsFalse(cpLoaded.AfterLoadCalled);

                //---------------Execute Test ------------------------
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpLoaded);

                //---------------Test Result -------------------------
                Assert.IsTrue(cpLoaded.AfterLoadCalled);
                Assert.AreNotSame(cp, cpLoaded);

                Assert.AreEqual(cp.FirstName, cpLoaded.FirstName);
                Assert.AreEqual(newFirstname, cpLoaded.FirstName);
            }

            [Test]
            public void TestBoLoader_RefreshBusinessObjectDeletedByAnotherUser()
            {
                //-------------Setup Test Pack------------------
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cpTemp = ContactPersonTestBO.CreateSavedContactPerson();
                BusinessObjectManager.Instance.ClearLoadedObjects();

                ContactPersonTestBO cpLoaded =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cpTemp.ID);
                cpTemp.MarkForDelete();
                cpTemp.Save();

                //-------------Execute Test ---------------------
                try
                {
                    BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpLoaded);
                    Assert.Fail();
                }
                    //-------------Test Result ---------------------
                catch (BusObjDeleteConcurrencyControlException ex)
                {
                    StringAssert.Contains
                        ("A Error has occured since the object you are trying to refresh has been deleted by another user.",
                         ex.Message);
                    StringAssert.Contains("There are no records in the database for the Class", ex.Message);
                    StringAssert.Contains("ContactPersonTestBO", ex.Message);
                    StringAssert.Contains(cpLoaded.ID.ToString(), ex.Message);
                }
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
                new Engine();
                new Car();
                ContactPerson originalContactPerson = new ContactPerson();
                originalContactPerson.Surname = "FirstSurname";
                originalContactPerson.Save();

                BusinessObjectManager.Instance.ClearLoadedObjects();

                //load second object from DB to ensure that it is now in the object manager
                ContactPerson myContact2 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                        (originalContactPerson.ID);

                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                ContactPerson myContact3 =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                        (originalContactPerson.ID);

                //---------------Test Result -----------------------
//                Assert.AreNotSame(originalContactPerson, myContact3);
                Assert.AreSame(myContact2, myContact3);
            }

            [Test]
            public void TestRefreshLoadedCollection_RemovedItem()
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
                Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
                BusinessObjectCollection<ContactPersonTestBO> col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                        (criteria);

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


   

    //Test persistable properties.

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
}