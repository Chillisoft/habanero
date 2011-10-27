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
using System.Collections.Generic;

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
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
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
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
            IClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo1 = new BOWithIntID {TestField = "PropValue", IntID = 55};
            bo1.Save();
            IPrimaryKey id = BOPrimaryKey.CreateWithValue((ClassDef) autoIncClassDef, bo1.IntID);
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
            IClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
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
            IClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
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
            IClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
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
		public void TestGetBusinessObjectWhenNotExists_CriteriaObject()//NotLoadedViaKey()
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
		public void TestGetBusinessObjectWhenNotExists_CriteriaObject_Untyped()//NotLoadedViaKey_Untyped()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
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

        protected static SelectQuery CreateSelectQuery(ContactPersonTestBO cp)
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
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                (ContactPersonTestBO) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, cp.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
        }

    	[Test]
        public void TestGetBusinessObject_CriteriaObject_Typed()
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
		public void TestGetBusinessObject_CriteriaObject_Typed_ThroughRelationship()
		//Modified from TestBusinessObjectLoader_GetBusinessObjectCollection.Test_GetBusinessObjectCollection_CriteriaString_ThroughRelationship_TwoLevels
		{
			//---------------Set up test pack-------------------
			Engine.LoadClassDef_IncludingCarAndOwner();
			//            DateTime now = DateTime.Now;
			string surname;
			string regno;
			string engineNo;
			Engine engine = Engine.CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
			Engine.CreateEngineWithCarWithContact();
			//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			Criteria criteria = new Criteria("Car.CarRegNo", Criteria.ComparisonOp.Equals, regno);

			//---------------Execute Test ----------------------
			Engine returned =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Engine>(criteria);

			//---------------Test Result -----------------------
			Assert.AreSame(engine, returned);
		}

		[Test]
		public void TestGetBusinessObject_CriteriaObject_Untyped_ThroughRelationship()
		//Modified from TestBusinessObjectLoader_GetBusinessObjectCollection.Test_GetBusinessObjectCollection_CriteriaString_ThroughRelationship_TwoLevels
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = Engine.LoadClassDef_IncludingCarAndOwner();
			//            DateTime now = DateTime.Now;
			string surname;
			string regno;
			string engineNo;
			Engine engine = Engine.CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
			Engine.CreateEngineWithCarWithContact();
			//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			Criteria criteria = new Criteria("Car.CarRegNo", Criteria.ComparisonOp.Equals, regno);

			//---------------Execute Test ----------------------
			Engine returned =
				(Engine)BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);

			//---------------Test Result -----------------------
			Assert.AreSame(engine, returned);
		}

		[Test]
		public virtual void TestGetBusinessObject_CriteriaObject_Typed_ThroughRelationship_TwoLevels()
		//Modified from TestBusinessObjectLoader_GetBusinessObjectCollection.Test_GetBusinessObjectCollection_CriteriaString_ThroughRelationship_TwoLevels
		{
			//---------------Set up test pack-------------------
			Engine.LoadClassDef_IncludingCarAndOwner();
			//            DateTime now = DateTime.Now;
			string surname;
			string regno;
			string engineNo;
			Engine engine = Engine.CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
			Engine.CreateEngineWithCarWithContact();
			//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			Criteria criteria = new Criteria("Car.Owner.Surname", Criteria.ComparisonOp.Equals, surname);

			//---------------Execute Test ----------------------
			Engine returned =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Engine>(criteria);

			//---------------Test Result -----------------------
			Assert.AreSame(engine, returned);
		}

		[Test]
		public virtual void TestGetBusinessObject_CriteriaObject_Untyped_ThroughRelationship_TwoLevels()
		//Modified from TestBusinessObjectLoader_GetBusinessObjectCollection.Test_GetBusinessObjectCollection_CriteriaString_ThroughRelationship_TwoLevels
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = Engine.LoadClassDef_IncludingCarAndOwner();
			//            DateTime now = DateTime.Now;
			string surname;
			string regno;
			string engineNo;
			Engine engine = Engine.CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
			Engine.CreateEngineWithCarWithContact();
			//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			Criteria criteria = new Criteria("Car.Owner.Surname", Criteria.ComparisonOp.Equals, surname);

			//---------------Execute Test ----------------------
			Engine returned =
				(Engine)BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);

			//---------------Test Result -----------------------
			Assert.AreSame(engine, returned);
		}

    	[Test]
    	public void TestGetBusinessObject_CriteriaObject_ShouldReturnSameAsByID()
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
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                (ContactPersonTestBO) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
        }
        
        [Test]
        public void TestGetBusinessObject_CriteriaString_Untyped()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                (ContactPersonTestBO) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, "Surname='"+cp.Surname+"'");

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
        }

        [Test]
        public void TestTryLoadDeletedObject_RaiseError()
        {
            try
            {

                //---------------Set up test pack-------------------
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO personToDelete = ContactPersonTestBO.CreateSavedContactPerson();
                personToDelete.MarkForDelete();
                personToDelete.Save();

                //Ensure that a fresh object is loaded from DB
                BORegistry.BusinessObjectManager.ClearLoadedObjects();

                //--------Execute------------------------------------------------------
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(personToDelete.ID);
                Assert.Fail("Expected to throw an BusObjDeleteConcurrencyControlException");
            }
            //---------------Test Result -----------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                StringAssert.Contains("There are no records in the database for the Class: ContactPersonTestBO", ex.Message);
            }
        }

        [Test]
        public void TestTryLoadDeletedObject_RaiseError_Untyped()
        {
            try
            {
                //---------------Set up test pack-------------------
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO personToDelete = ContactPersonTestBO.CreateSavedContactPerson();
                personToDelete.MarkForDelete();
                personToDelete.Save();

                //Ensure that a fresh object is loaded from DB
                BORegistry.BusinessObjectManager.ClearLoadedObjects();

                //--------Execute------------------------------------------------------
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(personToDelete.ClassDef, personToDelete.ID);
                Assert.Fail("Expected to throw an BusObjDeleteConcurrencyControlException");
            }
            //---------------Test Result -----------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                StringAssert.Contains("There are no records in the database for the Class: ContactPersonTestBO", ex.Message);
            }
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
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();

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
		public void TestGetBusinessObject_CriteriaString()
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
        
        [Test]
		public void TestGetBusinessObject_CriteriaString_Typed()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO.CreateSavedContactPerson(surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

            //---------------Execute Test ----------------------
            ContactPersonTestBO cp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>("Surname='"+surname+"'");

            //---------------Test Result -----------------------
            Assert.AreEqual(surname, cp.Surname);
            Assert.AreEqual(firstName, cp.FirstName);
        }

		[Test]
		public void TestGetBusinessObject_CriteriaString_Typed_ThroughRelationship()
		//Modified from TestBusinessObjectLoader_GetBusinessObjectCollection.Test_GetBusinessObjectCollection_CriteriaString_ThroughRelationship_TwoLevels
		{
			//---------------Set up test pack-------------------
			Engine.LoadClassDef_IncludingCarAndOwner();
			//            DateTime now = DateTime.Now;
			string surname;
			string regno;
			string engineNo;
			Engine engine = Engine.CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
			Engine.CreateEngineWithCarWithContact();
			//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			string criteria = string.Format("Car.CarRegNo = '{0}'", regno);

			//---------------Execute Test ----------------------
			Engine returned =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Engine>(criteria);

			//---------------Test Result -----------------------
			Assert.AreSame(engine, returned);
		}

		[Test]
		public void TestGetBusinessObject_CriteriaString_Untyped_ThroughRelationship()
		//Modified from TestBusinessObjectLoader_GetBusinessObjectCollection.Test_GetBusinessObjectCollection_CriteriaString_ThroughRelationship_TwoLevels
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = Engine.LoadClassDef_IncludingCarAndOwner();
			//            DateTime now = DateTime.Now;
			string surname;
			string regno;
			string engineNo;
			Engine engine = Engine.CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
			Engine.CreateEngineWithCarWithContact();
			//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			string criteria = string.Format("Car.CarRegNo = '{0}'", regno);

			//---------------Execute Test ----------------------
			Engine returned =
				(Engine) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);

			//---------------Test Result -----------------------
			Assert.AreSame(engine, returned);
		}

		[Test]
		public virtual void TestGetBusinessObject_CriteriaString_Typed_ThroughRelationship_TwoLevels()
		//Modified from TestBusinessObjectLoader_GetBusinessObjectCollection.Test_GetBusinessObjectCollection_CriteriaString_ThroughRelationship_TwoLevels
		{
			//---------------Set up test pack-------------------
			Engine.LoadClassDef_IncludingCarAndOwner();
			//            DateTime now = DateTime.Now;
			string surname;
			string regno;
			string engineNo;
			Engine engine = Engine.CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
			Engine.CreateEngineWithCarWithContact();
			//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			string criteria = string.Format("Car.Owner.Surname = '{0}'", surname);

			//---------------Execute Test ----------------------
			Engine returned =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Engine>(criteria);

			//---------------Test Result -----------------------
			Assert.AreSame(engine, returned);
		}

		[Test]
		public virtual void TestGetBusinessObject_CriteriaString_Untyped_ThroughRelationship_TwoLevels()
		//Modified from TestBusinessObjectLoader_GetBusinessObjectCollection.Test_GetBusinessObjectCollection_CriteriaString_ThroughRelationship_TwoLevels
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = Engine.LoadClassDef_IncludingCarAndOwner();
			//            DateTime now = DateTime.Now;
			string surname;
			string regno;
			string engineNo;
			Engine engine = Engine.CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
			Engine.CreateEngineWithCarWithContact();
			//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			string criteria = string.Format("Car.Owner.Surname = '{0}'", surname);

			//---------------Execute Test ----------------------Engine returned =
			Engine returned =
				(Engine) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);

			//---------------Test Result -----------------------
			Assert.AreSame(engine, returned);
		}

        //private static ContactPersonTestBO CreateSavedContactPerson(string surname, string firstName)
        //{
        //    ContactPersonTestBO contact = new ContactPersonTestBO();
        //    contact.Surname = surname;
        //    contact.FirstName = firstName;
        //    contact.Save();
        //    return contact;
        //}

        protected static void WaitForGC()
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
            //Assert.AreEqual(relationshipCriteria, addresses.SelectQuery.Criteria);
            StringAssert.Contains(relationshipCriteria.ToString(), addresses.SelectQuery.Criteria.ToString() );
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

        [Test]
        public virtual void TestGetBusinessObject_PropNameNotCorrect()
        {

            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            string propName = "NonExistantProperty";
            //---------------Execute Test ----------------------
            try
            {
                Criteria criteria = new Criteria(propName, Criteria.ComparisonOp.Equals, "1");

                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
                Assert.Fail("Expected to throw an InvalidPropertyNameException");
            }
            //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                StringAssert.Contains(propName, ex.Message);
            }
        }

        [Test]
        public void TestBoLoader_NotRefreshBusinessObjectIfCurrentlyBeingEdited()
        {
            //-------------Setup Test Pack------------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cpTemp = ContactPersonTestBO.CreateSavedContactPerson();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();

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
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
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
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
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
       
    }

}