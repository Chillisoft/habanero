//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    /// <summary>
    /// Tests Business Object loader. Loading a collection of business objects.
    /// Default Fixture Setup: Tests loading a collection of business objects from memory.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectLoader_GetBusinessObjectCollection
    {
        #region Setup/Teardown

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

        #endregion

        private DataStoreInMemory _dataStore;

        protected virtual void SetupDataAccessor()
        {
            _dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(_dataStore);
        }

        protected virtual void DeleteEnginesAndCars()
        {
            // do nothing for in-memory
        }

        private static SelectQuery CreateManualSelectQueryOrderedByDateOfBirth(DateTime now, BusinessObject cp1)
        {
            SelectQuery query = new SelectQuery(new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now));
            query.Fields.Add("DateOfBirth", new QueryField("DateOfBirth", "DateOfBirth", null));
            query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
            query.Source = new Source(cp1.ClassDef.TableName);
            query.OrderCriteria = new OrderCriteria().Add("DateOfBirth");
            return query;
        }

        private static ContactPersonTestBO CreateContactPersonInDB_With_SSSSS_InSurname()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = Guid.NewGuid().ToString("N") + "SSSSS";
            contactPersonTestBO.Save();
            return contactPersonTestBO;
        }

        private static void CreateContactPersonInDB()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = Guid.NewGuid().ToString("N");
            contactPersonTestBO.Save();
            return;
        }

        [Test]
        public void Test_CollectionLoad_CriteriaSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
            Assert.AreEqual("ContactPersonTestBO.Surname ASC", col.SelectQuery.OrderCriteria.ToString());
            Assert.AreEqual(-1, col.SelectQuery.Limit);
        }


        [Test]
        public void Test_CollectionLoad_CriteriaStringSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "searchSurname");
            const string stringCriteria = "Surname = searchSurname";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(stringCriteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
            Assert.AreEqual("ContactPersonTestBO.Surname ASC", col.SelectQuery.OrderCriteria.ToString());
            Assert.AreEqual(-1, col.SelectQuery.Limit);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_CriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string surname = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO.CreateSavedContactPerson();
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "Surname = " + surname;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_CriteriaString_WithOrder_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            //            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_NullCriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();
            const Criteria criteria = null;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_NullCriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("", "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_GuidCriteria()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("ContactPersonID = '" + person.ContactPersonID + "'", "");

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_StringCriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            col.Sort("Surname", true, true);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }


        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_StringCriteriaObject_Untyped_DateOfBirth()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            string criteria = "DateOfBirth = '" + now + "'";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_StringCriteriaObject_WithOrder_Untyped_DateOfBirth()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            string criteria = "DateOfBirth = '" + now + "'";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithOrderBy_ManualOrderbyFieldName()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("ggggg");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson("bbbbb");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("", "Surname");

            //---------------Test Result -----------------------
            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
        }


        [Test]
        public void Test_CollectionLoad_LoadedCollectionHasSuppliedClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = (ClassDef) ContactPersonTestBO.LoadDefaultClassDef();
            ClassDef changedClassDef = classDef.Clone();
            changedClassDef.TypeParameter = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (changedClassDef, "");
            //---------------Test Result -----------------------
            Assert.AreEqual(changedClassDef, col.ClassDef);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void Test_CollectionLoad_LoadedCollectionHasSuppliedClassDef_WithOrder()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = (ClassDef) ContactPersonTestBO.LoadDefaultClassDef();
            ClassDef changedClassDef = classDef.Clone();
            changedClassDef.TypeParameter = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (changedClassDef, "", "");
            //---------------Test Result -----------------------
            Assert.AreEqual(changedClassDef, col.ClassDef);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void Test_CollectionLoad_LoadedCollectionHasSuppliedClassDef_WithSelectQuery()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = (ClassDef) ContactPersonTestBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            ClassDef changedClassDef = classDef.Clone();
            changedClassDef.TypeParameter = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (changedClassDef, selectQuery);
            //---------------Test Result -----------------------
            Assert.AreEqual(changedClassDef, col.ClassDef);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void
            Test_CollectionLoad_LoadWithLimit_ThenRefresh_CriteriaObject_NewObjectWithinLimitAndCriteria_Included()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.LoadWithLimit(criteria, "Surname", 2);
            ContactPersonTestBO cpnew = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cpnew, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
            Assert.IsFalse(col.Contains(cp2));
        }

        [Test]
        public void
            Test_GetCollection_Generic_LoadWithLimit_ThenRefresh_CriteriaObject_NewObjectWithinLimitAndCriteria_Included
            ()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            int noRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (criteria, orderCriteria, 0, 2, out noRecords);
//            col.LoadWithLimit(criteria, "Surname", 2);
            ContactPersonTestBO cpnew = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cpnew, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
            Assert.IsFalse(col.Contains(cp2));
        }

        [Test]
        public void
            Test_GetCollection_NonGeneric_LoadWithLimit_ThenRefresh_CriteriaObject_NewObjectWithinLimitAndCriteria_Included
            ()
        {
            //---------------Set up test pack-------------------
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            int noRecords;
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (def, criteria, orderCriteria, 0, 2, out noRecords);
            //            col.LoadWithLimit(criteria, "Surname", 2);
            ContactPersonTestBO cpnew = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cpnew, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
            Assert.IsFalse(col.Contains(cp2));
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_CriteriaObject_IncludesCriteriaMatchesWithinLimit()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3), "bbb");

            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
            //---------------Assert Precondition ---------------


            //---------------Execute Test ----------------------
            //            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            col.LoadWithLimit(criteria, "Surname", 2);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtStart_LimitEqualsEnd()
            //NoRecords_StartRecords_ContainsAllRecords()
        {
            const int totalRecords = 3;
            const int firstRecord = 0;
            const int limit = totalRecords;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, totalRecords, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtStart_LimitBeforeEnd()
            //_NoRecords_StartRecords_First2Records()
        {
            const int totalRecords = 3;
            const int firstRecord = 0;
            const int limit = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtStart_LimitBeyondEnd()
        {
            const int totalRecords = 4;
            const int firstRecord = 0;
            const int limit = 6;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, totalRecords, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtStart_LimitNegative()
        {
            const int totalRecords = 5;
            const int firstRecord = 0;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, totalRecords, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtStart_LimitZero()
        {
            const int totalRecords = 5;
            const int firstRecord = 0;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAfterStart_LimitEqualsEnd()
            //NoRecords_StartRecords_ContainsAllRecords()
        {
            const int totalRecords = 3;
            const int firstRecord = 1;
            const int limit = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAfterStart_LimitBeforeEnd()
            //_NoRecords_StartRecords_First2Records()
        {
            const int totalRecords = 6;
            const int firstRecord = 2;
            const int limit = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAfterStart_LimitBeyondEnd()
        {
            const int totalRecords = 5;
            const int firstRecord = 3;
            const int limit = 4;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 2, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAfterStart_LimitBeyondEnd_RefreshWithAdditionalBO()
        {
            const int totalRecords = 5;
            const int firstRecord = 3;
            const int limit = 4;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            ContactPersonTestBO[] contactPersonTestBOsPlusOne = new ContactPersonTestBO[totalRecords + 1];
            contactPersonTestBOs.CopyTo(contactPersonTestBOsPlusOne, 0);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            contactPersonTestBOsPlusOne[totalRecords] = ContactPersonTestBO.CreateSavedContactPerson
                ("ZZZZZZZZZZZZZZZZZ");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.AreEqual(totalRecords + 1, contactPersonTestBOsPlusOne.Length);
            //---------------Execute Test ----------------------
            col.Refresh();
            //---------------Test Result -----------------------
            totalNoOfRecords++;
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords + 1, 2 + 1, contactPersonTestBOsPlusOne, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAfterStart_LimitNegative()
        {
            const int totalRecords = 7;
            const int firstRecord = 3;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 4, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAfterStart_LimitZero()
        {
            const int totalRecords = 7;
            const int firstRecord = 3;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtEnd_LimitEqualsEnd()
        {
            const int totalRecords = 4;
            const int firstRecord = totalRecords - 1;
            const int limit = 1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtEnd_LimitBeyondEnd()
        {
            const int totalRecords = 5;
            const int firstRecord = totalRecords - 1;
            const int limit = 3;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 1, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtEnd_LimitBeyondEnd_RefreshWithAdditionalBO()
        {
            const int totalRecords = 5;
            const int firstRecord = totalRecords - 1;
            const int limit = 3;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            ContactPersonTestBO[] contactPersonTestBOsPlusOne = new ContactPersonTestBO[totalRecords + 1];
            contactPersonTestBOs.CopyTo(contactPersonTestBOsPlusOne, 0);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            contactPersonTestBOsPlusOne[totalRecords] = ContactPersonTestBO.CreateSavedContactPerson
                ("ZZZZZZZZZZZZZZZZZ");
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.AreEqual(totalRecords + 1, contactPersonTestBOsPlusOne.Length);
            //---------------Execute Test ----------------------
            col.Refresh();
            //---------------Test Result -----------------------
            totalNoOfRecords++;
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords + 1, 2, contactPersonTestBOsPlusOne, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtEnd_LimitNegative()
        {
            const int totalRecords = 7;
            const int firstRecord = totalRecords - 1;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 1, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAtEnd_LimitZero()
        {
            const int totalRecords = 7;
            const int firstRecord = totalRecords - 1;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAfterEnd_LimitBeyondEnd()
        {
            const int totalRecords = 3;
            const int firstRecord = 5;
            const int limit = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAfterEnd_LimitNegative()
        {
            const int totalRecords = 4;
            const int firstRecord = 4;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstAfterEnd_LimitZero()
        {
            const int totalRecords = 3;
            const int firstRecord = 4;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_FirstNegative_ThrowsError()
        {
            const int totalRecords = 3;
            const int firstRecord = -1;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            try
            {
                int totalNoOfRecords;
                col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
                //---------------Test Result -----------------------
                Assert.Fail("IndexOutOfRangeException exception expected");
            }
            catch (IndexOutOfRangeException ex)
            {
                Assert.AreEqual("FirstRecordToLoad should not be negative.", ex.Message);
            }
        }

        #region GetCollection - LoadWith Limit Generic

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_CriteriaObject_IncludesCriteriaMatchesWithinLimit()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3), "bbb");

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            int totalNoOfRecords;
            //---------------Assert Precondition ---------------
            //---------------Execute Test ----------------------
            //            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (criteria, orderCriteria, 0, 2, out totalNoOfRecords);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
        }


        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtStart_LimitEqualsEnd()
            //NoRecords_StartRecords_ContainsAllRecords()
        {
            const int totalRecords = 3;
            const int firstRecord = 0;
            const int limit = totalRecords;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, totalRecords, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtStart_LimitBeforeEnd()
            //_NoRecords_StartRecords_First2Records()
        {
            const int totalRecords = 3;
            const int firstRecord = 0;
            const int limit = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");

            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtStart_LimitBeyondEnd()
        {
            const int totalRecords = 4;
            const int firstRecord = 0;
            const int limit = 6;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, totalRecords, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtStart_LimitNegative()
        {
            const int totalRecords = 5;
            const int firstRecord = 0;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, totalRecords, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtStart_LimitZero()
        {
            const int totalRecords = 5;
            const int firstRecord = 0;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAfterStart_LimitEqualsEnd()
            //NoRecords_StartRecords_ContainsAllRecords()
        {
            const int totalRecords = 3;
            const int firstRecord = 1;
            const int limit = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAfterStart_LimitBeforeEnd()
            //_NoRecords_StartRecords_First2Records()
        {
            const int totalRecords = 6;
            const int firstRecord = 2;
            const int limit = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAfterStart_LimitBeyondEnd()
        {
            const int totalRecords = 5;
            const int firstRecord = 3;
            const int limit = 4;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 2, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAfterStart_LimitBeyondEnd_RefreshWithAdditionalBO()
        {
            const int totalRecords = 5;
            const int firstRecord = 3;
            const int limit = 4;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            ContactPersonTestBO[] contactPersonTestBOsPlusOne = new ContactPersonTestBO[totalRecords + 1];
            contactPersonTestBOs.CopyTo(contactPersonTestBOsPlusOne, 0);
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            contactPersonTestBOsPlusOne[totalRecords] = ContactPersonTestBO.CreateSavedContactPerson
                ("ZZZZZZZZZZZZZZZZZ");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.AreEqual(totalRecords + 1, contactPersonTestBOsPlusOne.Length);
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            //---------------Test Result -----------------------
            totalNoOfRecords++;
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords + 1, 2 + 1, contactPersonTestBOsPlusOne, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAfterStart_LimitNegative()
        {
            const int totalRecords = 7;
            const int firstRecord = 3;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
//            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 4, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAfterStart_LimitZero()
        {
            const int totalRecords = 7;
            const int firstRecord = 3;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
//            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtEnd_LimitEqualsEnd()
        {
            const int totalRecords = 4;
            const int firstRecord = totalRecords - 1;
            const int limit = 1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
//            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
//            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtEnd_LimitBeyondEnd()
        {
            const int totalRecords = 5;
            const int firstRecord = totalRecords - 1;
            const int limit = 3;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
//            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
//            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 1, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtEnd_LimitBeyondEnd_RefreshWithAdditionalBO()
        {
            const int totalRecords = 5;
            const int firstRecord = totalRecords - 1;
            const int limit = 3;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            ContactPersonTestBO[] contactPersonTestBOsPlusOne = new ContactPersonTestBO[totalRecords + 1];
            contactPersonTestBOs.CopyTo(contactPersonTestBOsPlusOne, 0);
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            contactPersonTestBOsPlusOne[totalRecords] = ContactPersonTestBO.CreateSavedContactPerson
                ("ZZZZZZZZZZZZZZZZZ");
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.AreEqual(totalRecords + 1, contactPersonTestBOsPlusOne.Length);
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            //---------------Test Result -----------------------
            totalNoOfRecords++;
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords + 1, 2, contactPersonTestBOsPlusOne, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtEnd_LimitNegative()
        {
            const int totalRecords = 7;
            const int firstRecord = totalRecords - 1;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
//            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 1, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAtEnd_LimitZero()
        {
            const int totalRecords = 7;
            const int firstRecord = totalRecords - 1;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
//            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
//            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAfterEnd_LimitBeyondEnd()
        {
            const int totalRecords = 3;
            const int firstRecord = 5;
            const int limit = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAfterEnd_LimitNegative()
        {
            const int totalRecords = 4;
            const int firstRecord = 4;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstAfterEnd_LimitZero()
        {
            const int totalRecords = 3;
            const int firstRecord = 4;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }


        [Test]
        public void Test_GetCollection_Generic_LoadWithLimit_FirstNegative_ThrowsError()
        {
            const int totalRecords = 3;
            const int firstRecord = -1;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            try
            {
                int totalNoOfRecords;
                BusinessObjectCollection<ContactPersonTestBO> col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                        (null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
                col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
                //---------------Test Result -----------------------
                Assert.Fail("IndexOutOfRangeException exception expected");
            }
            catch (IndexOutOfRangeException ex)
            {
                Assert.AreEqual("FirstRecordToLoad should not be negative.", ex.Message);
            }
        }

        #endregion

        #region GetCollection With Limit NonGeneric

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_CriteriaObject_IncludesCriteriaMatchesWithinLimit()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3), "bbb");

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            int totalNoOfRecords;
            //---------------Assert Precondition ---------------

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria, orderCriteria, 0, 2, out totalNoOfRecords);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtStart_LimitEqualsEnd()
            //NoRecords_StartRecords_ContainsAllRecords()
        {
            const int totalRecords = 3;
            const int firstRecord = 0;
            const int limit = totalRecords;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");

            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def,null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, totalRecords, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtStart_LimitBeforeEnd()
            //_NoRecords_StartRecords_First2Records()
        {
            const int totalRecords = 3;
            const int firstRecord = 0;
            const int limit = 2;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def,null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtStart_LimitBeyondEnd()
        {
            const int totalRecords = 4;
            const int firstRecord = 0;
            const int limit = 6;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
           
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def,null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, totalRecords, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtStart_LimitNegative()
        {
            const int totalRecords = 5;
            const int firstRecord = 0;
            const int limit = -1;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
           
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");

            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def,null, orderCriteria, firstRecord, limit, out totalNoOfRecords);


            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, totalRecords, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtStart_LimitZero()
        {
            const int totalRecords = 5;
            const int firstRecord = 0;
            const int limit = 0;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);

            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");

            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def,null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAfterStart_LimitEqualsEnd()
            //NoRecords_StartRecords_ContainsAllRecords()
        {
            const int totalRecords = 3;
            const int firstRecord = 1;
            const int limit = 2;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);

            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
                        OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");

           IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def,null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAfterStart_LimitBeforeEnd()
            //_NoRecords_StartRecords_First2Records()
        {
            const int totalRecords = 6;
            const int firstRecord = 2;
            const int limit = 2;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);

            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");

            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def,null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAfterStart_LimitBeyondEnd()
        {
            const int totalRecords = 5;
            const int firstRecord = 3;
            const int limit = 4;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);

            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 2, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAfterStart_LimitBeyondEnd_RefreshWithAdditionalBO()
        {
            const int totalRecords = 5;
            const int firstRecord = 3;
            const int limit = 4;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            ContactPersonTestBO[] contactPersonTestBOsPlusOne = new ContactPersonTestBO[totalRecords + 1];
            contactPersonTestBOs.CopyTo(contactPersonTestBOsPlusOne, 0);
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            contactPersonTestBOsPlusOne[totalRecords] = ContactPersonTestBO.CreateSavedContactPerson
                ("ZZZZZZZZZZZZZZZZZ");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.AreEqual(totalRecords + 1, contactPersonTestBOsPlusOne.Length);
            //---------------Execute Test ----------------------
            col.Refresh();
            //---------------Test Result -----------------------
            totalNoOfRecords++;
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords + 1, 2 + 1, contactPersonTestBOsPlusOne, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAfterStart_LimitNegative()
        {
            const int totalRecords = 7;
            const int firstRecord = 3;
            const int limit = -1;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 4, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAfterStart_LimitZero()
        {
            const int totalRecords = 7;
            const int firstRecord = 3;
            const int limit = 0;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtEnd_LimitEqualsEnd()
        {
            const int totalRecords = 4;
            const int firstRecord = totalRecords - 1;
            const int limit = 1;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtEnd_LimitBeyondEnd()
        {
            const int totalRecords = 5;
            const int firstRecord = totalRecords - 1;
            const int limit = 3;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 1, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtEnd_LimitBeyondEnd_RefreshWithAdditionalBO()
        {
            const int totalRecords = 5;
            const int firstRecord = totalRecords - 1;
            const int limit = 3;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            ContactPersonTestBO[] contactPersonTestBOsPlusOne = new ContactPersonTestBO[totalRecords + 1];
            contactPersonTestBOs.CopyTo(contactPersonTestBOsPlusOne, 0);
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            contactPersonTestBOsPlusOne[totalRecords] = ContactPersonTestBO.CreateSavedContactPerson
                ("ZZZZZZZZZZZZZZZZZ");
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.AreEqual(totalRecords + 1, contactPersonTestBOsPlusOne.Length);
            //---------------Execute Test ----------------------
            col.Refresh();
            //---------------Test Result -----------------------
            totalNoOfRecords++;
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords + 1, 2, contactPersonTestBOsPlusOne, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtEnd_LimitNegative()
        {
            const int totalRecords = 7;
            const int firstRecord = totalRecords - 1;
            const int limit = -1;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 1, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAtEnd_LimitZero()
        {
            const int totalRecords = 7;
            const int firstRecord = totalRecords - 1;
            const int limit = 0;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAfterEnd_LimitBeyondEnd()
        {
            const int totalRecords = 3;
            const int firstRecord = 5;
            const int limit = 2;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAfterEnd_LimitNegative()
        {
            const int totalRecords = 4;
            const int firstRecord = 4;
            const int limit = -1;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstAfterEnd_LimitZero()
        {
            const int totalRecords = 3;
            const int firstRecord = 4;
            const int limit = 0;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            IBusinessObjectCollection col =
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void TestGetCollection_NonGeneric_LoadWithLimit_FirstNegative_ThrowsError()
        {
            const int totalRecords = 3;
            const int firstRecord = -1;
            const int limit = 0;
            IClassDef def = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedContactPeople(totalRecords);
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            try
            {
                int totalNoOfRecords;
                OrderCriteria orderCriteria = new OrderCriteria();
                orderCriteria.Add("Surname");
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(def, null, orderCriteria, firstRecord, limit, out totalNoOfRecords);

                //---------------Test Result -----------------------
                Assert.Fail("IndexOutOfRangeException exception expected");
            }
            catch (IndexOutOfRangeException ex)
            {
                Assert.AreEqual("FirstRecordToLoad should not be negative.", ex.Message);
            }
        }

        #endregion //GetCollection NonGeneric LoadWithLimit

        /// <summary>
        /// Asserts that the results for the collection are as expected
        /// </summary>
        /// <param name="expectedFirstRecord">The expected index for the first record to load.</param>
        /// <param name="expectedLimit">The expected limit for the Collection's Query.</param>
        /// <param name="expectedTotal">The expected total number of Bo's available</param>
        /// <param name="expectedCount">The expected count of the returned collection</param>
        /// <param name="orderedPeople">An ordered array that will be used to validate the items of the collection</param>
        /// <param name="actualCol">The actual Collection</param>
        /// <param name="returnedTotalNoOfRecords">The returned total Number of records to check</param>
        private static void AssertLimitedResultsCorrect
            (int expectedFirstRecord, int expectedLimit, int expectedTotal, int expectedCount,
             ContactPersonTestBO[] orderedPeople, IBusinessObjectCollection actualCol,
             int returnedTotalNoOfRecords)
        {
            Assert.AreEqual
                (expectedTotal, returnedTotalNoOfRecords, "The returned total number of availabe records is incorrect");
            Assert.AreEqual
                (expectedFirstRecord, actualCol.SelectQuery.FirstRecordToLoad,
                 "Collection query FirstRecordToLoad does not match expectation.");
            Assert.AreEqual
                (expectedLimit, actualCol.SelectQuery.Limit, "Collection query limit does not match expectation.");
            Assert.AreEqual(expectedCount, actualCol.Count, "Collection size does not match expectation.");
            int index = expectedFirstRecord;
            foreach (ContactPersonTestBO bo in actualCol)
            {
                Assert.AreSame(orderedPeople[index], bo, "Item in collection does not match expected item.");
                index++;
            }
        }

        /// <summary>
        /// Creates the specifed number of saved Contact People with random Surnames and reurns an array of the 
        /// created items sorted by their surname.
        /// </summary>
        /// <param name="noOfPeople">The number of saved contact perople to create</param>
        /// <returns>Returns an array of the created items sorted by their surname.</returns>
        private static ContactPersonTestBO[] CreateSavedContactPeople(int noOfPeople)
        {
            List<ContactPersonTestBO> createdBos = new List<ContactPersonTestBO>(noOfPeople);
            while (createdBos.Count < noOfPeople)
            {
                createdBos.Add(ContactPersonTestBO.CreateSavedContactPerson(TestUtil.GetRandomString()));
            }
            createdBos.Sort((x, y) => StringComparer.InvariantCultureIgnoreCase.Compare(x.Surname, y.Surname));
            return createdBos.ToArray();
        }

        [Test]
        public void Test_Refresh_LoadWithLimit_FromItem2_Next2Items()
        {
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp4 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO.CreateSavedContactPerson("aaaa");
            ContactPersonTestBO.CreateSavedContactPerson("zzzz");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson("cccc");
            ContactPersonTestBO.CreateSavedContactPerson("ggggg");
            ContactPersonTestBO.CreateSavedContactPerson("bbbbb");
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.SelectQuery = QueryBuilder.CreateSelectQuery(classDef, null);
            col.SelectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(classDef, "Surname");
            col.SelectQuery.FirstRecordToLoad = 2;
            col.SelectQuery.Limit = 2;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.SelectQuery.FirstRecordToLoad);
            Assert.AreEqual(2, col.SelectQuery.Limit);
            Assert.AreEqual(0, col.Count);
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            //---------------Test Result -----------------------    
            Assert.AreEqual(2, col.SelectQuery.FirstRecordToLoad);
            Assert.AreEqual(2, col.SelectQuery.Limit);
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp4, col[1]);
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_NoRecords2_StartRecord1_UsingWhereClause()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO.CreateSavedContactPerson("gggggg");
            ContactPersonTestBO.CreateSavedContactPerson("gggdfasd");
            ContactPersonTestBO.CreateSavedContactPerson("bbbbbb");
            ContactPersonTestBO.CreateSavedContactPerson("zazaza");
            ContactPersonTestBO.CreateSavedContactPerson("zbbbbb");
            ContactPersonTestBO.CreateSavedContactPerson("zccccc");
            ContactPersonTestBO.CreateSavedContactPerson("zddddd");
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            int totalNoOfRecords;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            col.LoadWithLimit("Surname Not Like 'z%'", "Surname", 1, 2, out totalNoOfRecords);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.SelectQuery.FirstRecordToLoad);
            Assert.AreEqual(2, col.SelectQuery.Limit);
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_WithLimit_EqualNumberOfObjects()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = 3;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.AreSame(cpLast, col[2]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_WithLimit_LessObjects()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = 10;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.AreSame(cpLast, col[2]);
        }

//        private static BusinessObjectCollection<ContactPersonTestBO> CreateCol_OneCP(out ContactPersonTestBO cp)
//        {
//            cp = ContactPersonTestBO.CreateSavedContactPerson();
//            return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
//        }
        //
        //        private static void CreateTwoSavedContactPeople()
        //        {
        //            CreateSavedContactPerson();
        //            CreateSavedContactPerson();
        //        }


        private static ContactPersonTestBO CreateSavedContactPerson(string surnameValue, int integerPropertyValue)
        {
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = surnameValue;
            cp.SetPropertyValue("IntegerProperty", integerPropertyValue);
            cp.Save();
            return cp;
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SelectQuery_WithLimit()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = 2;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SelectQuery_WithLimit_Negative()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = -1;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.AreSame(cpLast, col[2]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SelectQuery_WithLimit_Zero()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = 0;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, col.Count);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Typed_RefreshLoadedCollection_UsingLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    ("Surname like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cpNewLikeMatch, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Typed_RefreshLoadedCollection_UsingNotLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    ("Surname not like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.IsFalse(col.Contains(cpNewLikeMatch));
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Untyped_RefreshLoadedCollection_UsingLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (classDef, "Surname like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cpNewLikeMatch, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_UnTyped_RefreshLoadedCollection_UsingNotLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (classDef, "Surname not like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.IsFalse(col.Contains(cpNewLikeMatch));
        }

        [Test]
        public void Test_GetBusinesssObjectCollection_Untyped_GtCriteriaString()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef_W_IntegerProperty();
            ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
            ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
            CreateSavedContactPerson(TestUtil.GetRandomString(), 2);
            ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.GetRandomString(), 3);

            string criteria = "IntegerProperty > " + 3;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.IsFalse(col.Contains(cpEqual));
        }

        [Test]
        public void Test_LoadUsingLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            col.Load("Surname like %SSS%", "Surname");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
        }

        [Test]
        public void Test_LoadUsingNotLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            col.Load("Surname Not like %SSS%", "Surname");
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void Test_CriteriaSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void Test_CriteriaSetUponLoadingCollection_Untyped()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void Test_CriteriaSetUponLoadingCollection_Untyped_Date()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            string stringCriteria = "DateOfBirth = " + now;

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (classDef, stringCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria.FieldValue, col.SelectQuery.Criteria.FieldValue);
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void Test_CriteriaStringSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "searchSurname");
            const string stringCriteria = "Surname = searchSurname";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (classDef, stringCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void Test_CriteriaStringSetUponLoadingCollection_Untyped()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "searchSurname");
            const string stringCriteria = "Surname = searchSurname";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (classDef, stringCriteria);

            //---------------Test Result -----------------------
            Assert.IsNotNull(col.SelectQuery.Criteria);
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaObject_DateTimeToday()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(-1));
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(today, "aaa");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(today, "bbb");
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(1));
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, new DateTimeToday());

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, criteria);


            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaObject_WithOrder()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");


            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (criteria, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaObject_WithOrder_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, criteria, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string surname = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO.CreateSavedContactPerson();
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "Surname = " + surname;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaString_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            const string surname = "TestSurname";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            cp1.Organisation = organisation;
            cp1.Save();
            ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO.CreateSavedContactPerson();
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            string criteria = string.Format
                ("Organisation.OrganisationID = '{0}'", organisation.OrganisationID.Value.ToString("B"));

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp1, col);
        }

        [Test]
        public virtual void Test_CriteriaString_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            Engine.LoadClassDef_IncludingCarAndOwner();
            //            DateTime now = DateTime.Now;
            string surname;
            string regno;
            string engineNo;
            Engine engine = CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
            CreateEngineWithCarWithContact();
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            string criteria = string.Format("Car.Owner.Surname = '{0}'", surname);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<Engine> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(engine, col);
        }

        [Test]
        public virtual void Test_CriteriaString_ThroughRelationship_TwoLevels_SearchOnNULL()
        {
            //---------------Set up test pack-------------------
            Engine.DeleteAllEngines();
            Engine.LoadClassDef_IncludingCarAndOwner();
            string engineNo = TestUtil.GetRandomString();
            Engine engine = Engine.CreateSavedEngine(engineNo);
            string criteria = string.Format("Car.OwnerID is NULL");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<Engine> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(engine, col);
        }


        [Test]
        public virtual void Test_Load_CriteriaString_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            Engine.LoadClassDef_IncludingCarAndOwner();
            //            DateTime now = DateTime.Now;
            string surname;
            string regno;
            string engineNo;
            Engine engine = CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
            CreateEngineWithCarWithContact();
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            string criteria = string.Format("Car.Owner.Surname = '{0}'", surname);
            BusinessObjectCollection<Engine> col = new BusinessObjectCollection<Engine>();

            //---------------Execute Test ----------------------
            col.Load(criteria, "");

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(engine, col);
        }

        protected void CreateEngineWithCarWithContact()
        {
            string surname;
            string regno;
            string engineNo;
            CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
            return;
        }

        private static Engine CreateEngineWithCarWithContact(out string surname, out string regno, out string engineNo)
        {
            regno = TestUtil.GetRandomString();
            engineNo = TestUtil.GetRandomString();
            surname = TestUtil.GetRandomString();
            ContactPerson owner = ContactPerson.CreateSavedContactPerson(surname);
            Car car = Car.CreateSavedCar(regno, owner);
            return Engine.CreateSavedEngine(car, engineNo);
        }

//        private Engine CreateEngineWithCarNoContact(out string regno, out string engineNo)
//        {
//            regno = TestUtil.GetRandomString();
//            engineNo = TestUtil.GetRandomString();
//            Car car = Car.CreateSavedCar(regno);
//            return Engine.CreateSavedEngine(car, engineNo);
//        }


        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaString_Date_Today()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(-1));
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(today, "aaa");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(today, "bbb");
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(1));
            const string criteria = "DateOfBirth = 'Today'";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaString_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
//            DateTime now = DateTime.Now;
            //ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            //ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            //Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string firstName = "fName";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("aaa", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("zzz", firstName);
            const string criteria = "FirstName = " + firstName;

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, criteria);


            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            col.Sort("Surname", true, true);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaString_WithOrder_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
//            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            //            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaString_WithOrderString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
//            DateTime now = DateTime.Now;
//            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
//            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
//            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");
            const string firstName = "fName";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaa", firstName);
            const string criteria = "FirstName = " + firstName;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_CriteriaString_WithOrderString_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            const string firstName = "fName";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaa", firstName);
            const string criteria = "FirstName = " + firstName;

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_GetsSameObjectAsGetBusinessObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria collectionCriteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            Criteria singleCritieria = new Criteria
                ("ContactPersonID", Criteria.ComparisonOp.Equals, cp1.ContactPersonID);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO loadedCp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(singleCritieria);
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (collectionCriteria, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, col[1]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_GetsSameObjectAsGetBusinessObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria collectionCriteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            Criteria singleCritieria = new Criteria
                ("ContactPersonID", Criteria.ComparisonOp.Equals, cp1.ContactPersonID);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO loadedCp =
                (ContactPersonTestBO)
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(cp1.ClassDef, singleCritieria);
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, collectionCriteria, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, col[1]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NullCriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NullCriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();
            const Criteria criteria = null;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SelectQuery()
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

        [Test]
        public void Test_GetBusinessObjectCollection_SelectQuery_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now.AddMinutes(1));
            SelectQuery query = CreateManualSelectQueryOrderedByDateOfBirth(now, cp1);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, query);


            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SortOrder_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria
                (car1engine1.ClassDef, "Car.CarRegNo, EngineNo");

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
        public void Test_GetBusinessObjectCollection_SortOrder_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            new Engine();
            new Car();
            ContactPerson contactPerson1 = ContactPerson.CreateSavedContactPerson("zzzz");
            ContactPerson contactPerson2 = ContactPerson.CreateSavedContactPerson("aaaa");
            Car car1 = Car.CreateSavedCar("2", contactPerson1);
            Car car2 = Car.CreateSavedCar("5", contactPerson2);
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");

            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria
                (car1engine1.ClassDef, "Car.Owner.Surname, EngineNo");
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
        public void Test_GetBusinessObjectCollectionClassDef_SortOrder_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            //DeleteEnginesAndCars();
            new Engine();
            new Car();
            ContactPerson contactPerson1 = ContactPerson.CreateSavedContactPerson("zzzz");
            Car car1 = Car.CreateSavedCar("2", contactPerson1);
            Engine.CreateSavedEngine(car1, "20");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection carsOwned = contactPerson1.GetCarsOwned();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, carsOwned.Count);

            //---------------Tear Down -------------------------     
        }


        [Test]
        public void Test_GetBusinessObjectCollection_SortOrder_ThroughRelationship_Untyped()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria
                (car1engine1.ClassDef, "Car.CarRegNo, EngineNo");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                    (car1engine1.ClassDef, null, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SortOrderString_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
            //OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef,
            //                                                               "Car.CarRegNo, EngineNo");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<Engine> engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>
                    (null, "Car.CarRegNo, EngineNo");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SortOrderString_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            new Engine();
            new Car();
            ContactPerson contactPerson1 = ContactPerson.CreateSavedContactPerson("zzzz");
            ContactPerson contactPerson2 = ContactPerson.CreateSavedContactPerson("aaaa");
            Car car1 = Car.CreateSavedCar("2", contactPerson1);
            Car car2 = Car.CreateSavedCar("5", contactPerson2);
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");

            //---------------Execute Test ----------------------
//            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef,
//                                                                           "Car.Owner.Surname, EngineNo");
            BusinessObjectCollection<Engine> engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>
                    (null, "Car.Owner.Surname, EngineNo");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SortOrderString_ThroughRelationship_Untyped()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
//            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef,
//                                                                           "Car.CarRegNo, EngineNo");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                    (car1engine1.ClassDef, null, "Car.CarRegNo, EngineNo");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_StringCriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            const string criteria = "FirstName = '" + firstName + "'";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, criteria);
            col.Sort("Surname", true, true);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);

            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }


        [Test]
        public void Test_GetBusinessObjectCollection_StringCriteriaObject_Untyped_DateOfBirth()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            string criteria = "DateOfBirth = '" + now + "'";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, criteria);
            col.Sort("Surname", true, true);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_StringCriteriaObject_WithOrder_Untyped_DateOfBirth()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            string criteria = "DateOfBirth = '" + now + "'";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (cp1.ClassDef, criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_LoadAll()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            cpCol.LoadAll();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
        }


        //        [Test]
//        public void Test_LoadUsingLike()
//        {
//            //---------------Set up test pack-------------------
//            ContactPerson.DeleteAllContactPeople();
//            ContactPersonTestBO.LoadDefaultClassDef();
//            //Create data in the database with the 5 contact people two with Search in surname
//            CreateContactPersonInDB();
//            CreateContactPersonInDB();
//            CreateContactPersonInDB();
//            CreateContactPersonInDB_With_SSSSS_InSurname();
//            CreateContactPersonInDB_With_SSSSS_InSurname();
//            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
//
//            //---------------Assert Precondition----------------
//
//            //---------------Execute Test ----------------------
//            col.Load("Surname like %SSS%", "Surname");
//            //---------------Test Result -----------------------
//            Assert.AreEqual(2, col.Count);
//        }
//        [Test]
//        public void Test_LoadUsingNotLike()
//        {
//            //---------------Set up test pack-------------------
//            ContactPerson.DeleteAllContactPeople();
//            ContactPersonTestBO.LoadDefaultClassDef();
//            //Create data in the database with the 5 contact people two with Search in surname
//            CreateContactPersonInDB();
//            CreateContactPersonInDB();
//            CreateContactPersonInDB();
//            CreateContactPersonInDB_With_SSSSS_InSurname();
//            CreateContactPersonInDB_With_SSSSS_InSurname();
//            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
//
//            //---------------Assert Precondition----------------
//
//            //---------------Execute Test ----------------------
//            col.Load("Surname Not like %SSS%", "Surname");
//            //---------------Test Result -----------------------
//            Assert.AreEqual(3, col.Count);
//        }


        [Test]
        public void Test_LoadAll_Loader()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.LoadAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp, col);
            Assert.AreEqual(1, col.PersistedBusinessObjects.Count);
            Assert.IsTrue(col.PersistedBusinessObjects.Contains(cp));
        }

        [Test]
        public void Test_LoadWithOrderBy()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("ggggg");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson("bbbbb");

            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria().Add("Surname");
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria);
            //---------------Test Result -----------------------
            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
        }

        [Test]
        public void Test_LoadWithOrderBy_ManualOrderbyFieldName()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("ggggg");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson("bbbbb");
            OrderCriteria orderCriteria = new OrderCriteria();
            //---------------Execute Test ----------------------
            orderCriteria.Add
                (new OrderCriteria.Field("Surname", "Surname_field", null, OrderCriteria.SortDirection.Ascending));
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (null, orderCriteria);
            //---------------Test Result -----------------------
            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
        }


        [Test]
        public void Test_SetColSelectQuery_null()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                col.SelectQuery = null;
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("A collections select query cannot be set to null", ex.Message);
                StringAssert.Contains("A collections select query cannot be set to null", ex.DeveloperMessage);
            }
        }

        [Test]
        public void Test_GetBusinessObjectCollection_TypedAsBusinessObject_ThrowsError_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            //---------------Execute Test ----------------------
            AssertTypedAsBusinessObjectThrowsCorrectException
                (delegate
                 {
#pragma warning disable 168
                     BusinessObjectCollection<BusinessObject> col = BORegistry.DataAccessor.BusinessObjectLoader.
#pragma warning restore 168
                         GetBusinessObjectCollection<BusinessObject>(criteria);
                 });
        }

        [Test]
        public void Test_GetBusinessObjectCollection_TypedAsBusinessObject_ThrowsError_CriteriaObjectWithOrderBy()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            //---------------Execute Test ----------------------
            AssertTypedAsBusinessObjectThrowsCorrectException
                (delegate
                 {
#pragma warning disable 168
                     BusinessObjectCollection<BusinessObject> col = BORegistry.DataAccessor.BusinessObjectLoader.
#pragma warning restore 168
                         GetBusinessObjectCollection<BusinessObject>(criteria, null);
                 });
        }

        [Test]
        public void Test_GetBusinessObjectCollection_TypedAsBusinessObject_ThrowsError_SelectQuery()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            //---------------Execute Test ----------------------
            AssertTypedAsBusinessObjectThrowsCorrectException
                (delegate
                 {
#pragma warning disable 168
                     BusinessObjectCollection<BusinessObject> col = BORegistry.DataAccessor.BusinessObjectLoader.
#pragma warning restore 168
                         GetBusinessObjectCollection<BusinessObject>((SelectQuery) null);
                 });
        }

        [Test]
        public void Test_GetBusinessObjectCollection_TypedAsBusinessObject_ThrowsError_CriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            //---------------Execute Test ----------------------
            AssertTypedAsBusinessObjectThrowsCorrectException
                (delegate
                 {
#pragma warning disable 168
                     BusinessObjectCollection<BusinessObject> col = BORegistry.DataAccessor.BusinessObjectLoader.
#pragma warning restore 168
                         GetBusinessObjectCollection<BusinessObject>("");
                 });
        }

        [Test]
        public void Test_GetBusinessObjectCollection_TypedAsBusinessObject_ThrowsError_CriteriaStringWithOrderBy()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            //---------------Execute Test ----------------------
            AssertTypedAsBusinessObjectThrowsCorrectException
                (delegate
                 {
#pragma warning disable 168
                     BusinessObjectCollection<BusinessObject> col = BORegistry.DataAccessor.BusinessObjectLoader.
#pragma warning restore 168
                         GetBusinessObjectCollection<BusinessObject>("", "");
                 });
        }

        private static void AssertTypedAsBusinessObjectThrowsCorrectException(MethodInvoker callToInvoke)
        {
            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                callToInvoke();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Exception should have been thrown");
            Assert.IsInstanceOfType
                (typeof (HabaneroDeveloperException), exception, "Should be a HabaneroDeveloperException.");
            HabaneroDeveloperException developerException = (HabaneroDeveloperException) exception;
            Assert.AreEqual(developerException.Message, ExceptionHelper._habaneroDeveloperExceptionUserMessage);
            Assert.AreEqual
                (developerException.DeveloperMessage, ExceptionHelper._loaderGenericTypeMethodExceptionMessage);
        }

        [Test, Ignore("No Test Implemented")]
        public void Test_LoadWithCriteria_MultipleLevels()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------
        }
    }


    


}