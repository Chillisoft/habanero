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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO
{ 
    
    public abstract class TestBusinessObjectLookupList : TestUsingDatabase
    {
        protected abstract void SetupDataAccessor();
        protected abstract void DeleteAllContactPeople();
        
       
        [TestFixture]
        public class TestBusinessObjectLookupListMemory : TestBusinessObjectLookupList
        {
            private DataStoreInMemory _dataStore;

            protected override void SetupDataAccessor()
            {
                _dataStore = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(_dataStore);
            }

            protected override void DeleteAllContactPeople()
            {
                _dataStore = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(_dataStore);
            }
        }
        protected static string GuidToString(Guid guid)
        {
            return guid.ToString();//("B").ToUpperInvariant();
        }
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            SetupDataAccessor();
            DeleteAllContactPeople();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.CreateSampleData();
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
        }

        

        [TestFixtureTearDown]
        public void TearDown()
        {
            DeleteAllContactPeople();
        }

        [Test]
        public void TestGetLookupList() 
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof (ContactPersonTestBO));
            source.PropDef = new PropDef("name", typeof (string), PropReadWriteRule.ReadWrite, null);
            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(3, col.Count);
            foreach (string o in col.Values) {
                Assert.AreSame(typeof(string), o.GetType());
                Guid parsedGuid;
                Assert.IsTrue(StringUtilities.GuidTryParse(o, out parsedGuid));
            }
        }

        [Test]
        public void TestCallingGetLookupListTwiceOnlyAccessesDbOnce()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO));
            source.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);
            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Dictionary<string, string> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreSame(col2, col);
        }



        [Test]
        public void TestCriteria()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "Surname='zzz'", "");
            source.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);
            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(1, col.Count);
        }

        [Test]
        public void TestTodayDateStringCriteria_LessThan()
        {
            //-------------Setup Test Pack ------------------
            DeleteAllContactPeople();
            BusinessObjectCollection<ContactPersonTestBO> myCol = new BusinessObjectCollection<ContactPersonTestBO>();
            myCol.LoadAll();
            DateTime today = DateTime.Today;
            ContactPersonTestBO contactPerson1 = ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(-1), "aaa");
            ContactPersonTestBO.CreateSavedContactPerson(today, "bbb");
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(1), "ccc");
            //ContactPersonTestBO.ClearObjectManager();
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                                    "ContactPersonTestBO", "DateOfBirth < 'Today'", "");
            businessObjectLookupList.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);
            //-------------Test Pre-conditions --------------
            Assert.AreEqual(0, myCol.Count);

            //-------------Execute test ---------------------
            Dictionary<string, string> col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);

            //-------------Test Result ----------------------
            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col.ContainsValue(GuidToString(contactPerson1.ID.GetAsGuid())));
        }


        [Test]
        public void TestTodayDateStringCriteria_Equals()
        {
            //-------------Setup Test Pack ------------------
            DeleteAllContactPeople();
            BusinessObjectCollection<ContactPersonTestBO> myCol = new BusinessObjectCollection<ContactPersonTestBO>();
            myCol.LoadAll();
            DateTime today = DateTime.Today;
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(-1), "aaa");
            ContactPersonTestBO contactPerson2 = ContactPersonTestBO.CreateSavedContactPerson(today, "bbb");
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(1), "ccc");
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                                    "ContactPersonTestBO", "DateOfBirth = 'today'", "");
            businessObjectLookupList.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);

            //-------------Test Pre-conditions --------------
            Assert.AreEqual(0, myCol.Count);

            //-------------Execute test ---------------------
            Dictionary<string, string> col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);

            //-------------Test Result ----------------------
            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col.ContainsValue(GuidToString(contactPerson2.ID.GetAsGuid())));
        }

        [Test]
        public void TestTodayDateStringCriteria_GreaterThanOrEqualTo()
        {
            //-------------Setup Test Pack ------------------
            DeleteAllContactPeople();
            BusinessObjectCollection<ContactPersonTestBO> myCol = new BusinessObjectCollection<ContactPersonTestBO>();
            myCol.LoadAll();
            DateTime today = DateTime.Today;
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(-1), "aaa");
            ContactPersonTestBO.CreateSavedContactPerson(today, "bbb");
            ContactPersonTestBO contactPerson3 = ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(1), "ccc");
            //ContactPersonTestBO.ClearObjectManager();
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                                    "ContactPersonTestBO", "DateOfBirth > 'TODAY'", "");
            businessObjectLookupList.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);

            //-------------Test Pre-conditions --------------
            Assert.AreEqual(0, myCol.Count);

            //-------------Execute test ---------------------
            Dictionary<string, string> col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);

            //-------------Test Result ----------------------
            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col.ContainsValue(GuidToString(contactPerson3.ID.GetAsGuid())));
        }

        [Test]
        public void TestNowDateStringCriteria()
        {
            DeleteAllContactPeople();
            BusinessObjectCollection<ContactPersonTestBO> myCol = new BusinessObjectCollection<ContactPersonTestBO>();
            myCol.LoadAll();
            Assert.AreEqual(0, myCol.Count);
            ContactPersonTestBO contactPerson1 = new ContactPersonTestBO();
            contactPerson1.Surname = "aaa";
            contactPerson1.DateOfBirth = DateTime.Now.AddMinutes(-1);
            contactPerson1.Save();
            ContactPersonTestBO contactPerson2 = new ContactPersonTestBO();
            contactPerson2.Surname = "bbb";
            contactPerson2.DateOfBirth = DateTime.Now.AddMinutes(-1);
            contactPerson2.Save();
            ContactPersonTestBO contactPerson3 = new ContactPersonTestBO();
            contactPerson3.Surname = "ccc";
            contactPerson3.DateOfBirth = DateTime.Now.AddMinutes(-1);
            contactPerson3.Save();

            //ContactPersonTestBO.ClearObjectManager();
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                                                                                             "ContactPersonTestBO", "DateOfBirth < 'Now'", "");
            businessObjectLookupList.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);
            Dictionary<string, string> col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(3, col.Count);
            Assert.IsTrue(col.ContainsValue(GuidToString(contactPerson1.ID.GetAsGuid())));
            Assert.IsTrue(col.ContainsValue(GuidToString(contactPerson2.ID.GetAsGuid())));
            Assert.IsTrue(col.ContainsValue(GuidToString(contactPerson3.ID.GetAsGuid())));
            businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "DateOfBirth > 'now'", "")
                   {
                       PropDef = new PropDef ("name", typeof (string), PropReadWriteRule.ReadWrite, null)
                   };
            col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(0, col.Count);
            ContactPersonTestBO contactPerson4 = new ContactPersonTestBO();
            contactPerson4.Surname = "ddd";
            contactPerson4.DateOfBirth = DateTime.Now.AddMinutes(5);
            contactPerson4.Save();
            businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "DateOfBirth > NOW", "");
            businessObjectLookupList.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);

            col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col.ContainsValue(GuidToString(contactPerson4.ID.GetAsGuid())));
        }

        [Test]
        public void Test_LimitToList_Attribute_Default()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO));
            //---------------Test Result -----------------------
            Assert.IsFalse(source.LimitToList);
        }
        [Test]
        public void Test_SetTimeOut_ShouldUpdateNewTimeOut()
        {
            //---------------Set up test pack-------------------
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO));
            const int expectedTimeout = 200000;

            //---------------Assert Precondition----------------
            Assert.AreEqual(10000, source.TimeOut);
            //---------------Execute Test ----------------------
            source.TimeOut = expectedTimeout;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedTimeout, source.TimeOut);
        }

        [Test]
        public void Test_GetLookupList_WhenTimeOutHasNotExpired_ShouldNotReload()
        {
            //---------------Set up test pack-------------------
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO));
            source.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);
            const int timeout = 200000;
            source.TimeOut = timeout;
            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            //---------------Assert Precondition----------------
            Assert.AreEqual(timeout, source.TimeOut);
            Assert.IsNotNull(col);
            //---------------Execute Test ----------------------
            Dictionary<string, string> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
            //---------------Test Result -----------------------
            Assert.AreSame(col, col2, "Both collections should be the same since the timeout has not been reached");
        }

        [Test]
        public void Test_GetIDValueLookupList_WhenTimeOutHasNotExpired_ShouldNotReload()
        {
            //---------------Set up test pack-------------------
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO));
            source.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);
            const int timeout = 200000;
            source.TimeOut = timeout;
            Dictionary<string, string> col = source.GetIDValueLookupList();
            //---------------Assert Precondition----------------
            Assert.AreEqual(timeout, source.TimeOut);
            Assert.IsNotNull(col);
            //---------------Execute Test ----------------------
            Dictionary<string, string> col2 = source.GetIDValueLookupList();
            //---------------Test Result -----------------------
            Assert.AreSame(col, col2, "Both collections should be the same since the timeout has not been reached");
        }
        [Test]
        public void Test_GetLookupList_WhenTimeoutExpired_ShouldReloadList()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO), 100);
            source.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);
            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Thread.Sleep(250);
            Dictionary<string, string> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreNotSame(col2, col);
        }
        [Test]
        public void Test_Constructor_WithLimitToList_AsTrue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            BusinessObjectLookupList source = new BusinessObjectLookupList(
                "Habanero.Test.BO", "ContactPersonTestBO", "", "surname", true);
            //---------------Test Result -----------------------
            Assert.IsTrue(source.LimitToList);
        }

        [Test]
        public void Test_Constructor_WithLimitToList_AsFalse()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            BusinessObjectLookupList source = new BusinessObjectLookupList(
                "Habanero.Test.BO", "ContactPersonTestBO", "", "surname", false);
            //---------------Test Result -----------------------
            Assert.IsFalse(source.LimitToList);
        }

        [Test]
        public void TestSortAttribute()
        {
            //---------------Set up test pack-------------------
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO));
            //---------------Assert Precondition----------------
            Assert.IsNull(source.OrderCriteria);
            //---------------Execute Test ----------------------
            source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "", "surname");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, source.OrderCriteria.Fields.Count);
            OrderCriteria.Field orderField = source.OrderCriteria.Fields[0];
            Assert.AreEqual("surname", orderField.PropertyName);
            Assert.AreEqual(OrderCriteria.SortDirection.Ascending, orderField.SortDirection);
            Assert.AreEqual("ContactPersonTestBO", orderField.Source.Name);
        }

        [Ignore("This should be looked at so that it validates the attributes as early as possible")]
        [Test]
        public void TestSortInvalidProperty()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            InvalidXmlDefinitionException exception = null;
            try
            {
                new BusinessObjectLookupList(
                    "Habanero.Test.BO", "ContactPersonTestBO", "", "invalidprop");
            }
            catch (InvalidXmlDefinitionException ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Exception of type 'InvalidXmlDefinitionException' expected.");
        }

        [Ignore("This should be looked at so that it validates the attributes as early as possible")]
        [Test]
        public void TestSortInvalidDirection()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            InvalidXmlDefinitionException exception = null;
            try
            {
                new BusinessObjectLookupList(
                    "Habanero.Test.BO", "ContactPersonTestBO", "", "surname invalidorder");
            }
            catch (InvalidXmlDefinitionException ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Exception of type 'InvalidXmlDefinitionException' expected.");
        }

        //  This test is excluded because the sort attribute is checked in the middle of
        //  class def loading when not all the defs have been loaded.
        //[Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        //public void TestSortInvalidClassAndAssembly()
        //{
        //    BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
        //        "InvalidClass", "", "surname");
        //    source.Sort = "surname desc";
        //}

        [Test]
        public void TestSortingByDefault()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO", "ContactPersonTestBO");
            new PropDef("N", typeof(Guid), PropReadWriteRule.ReadWrite, null) {LookupList = source};
            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            ArrayList items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            AssertCorrectlySortedBusinessObjectInList(items, 0, "abc");
            AssertCorrectlySortedBusinessObjectInList(items, 1, "abcd");
            AssertCorrectlySortedBusinessObjectInList(items, 2, "zzz");
        }

        private static void AssertCorrectlySortedBusinessObjectInList(ArrayList items, int index, string expected)
        {
            object item = items[index];
            IBusinessObject businessObject = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue(typeof(ContactPersonTestBO), item);
            Assert.AreEqual(expected, businessObject.ToString());
        }

        private static BusinessObjectLookupList GetBusinessObjectLookupListForContactPerson(string sortCriteria)
        {
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupList(
                "Habanero.Test.BO", "ContactPersonTestBO", "", sortCriteria);
            new PropDef("N", typeof(Guid), PropReadWriteRule.ReadWrite, null) { LookupList = businessObjectLookupList };
            return businessObjectLookupList;
        }

        [Test]
        public void TestSortingCollection_PropOnlySpecified()
        {
            //---------------Set up test pack-------------------
            BusinessObjectLookupList businessObjectLookupList = 
                GetBusinessObjectLookupListForContactPerson("surname");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Dictionary<string, string> col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            //---------------Test Result -----------------------
            ArrayList items = new ArrayList(col.Values);
            Assert.AreEqual(3, col.Count);
            object item = items[0];
            IBusinessObject businessObject = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue(typeof(ContactPersonTestBO), item);
            Assert.AreEqual("abc", businessObject.ToString());
            AssertCorrectlySortedBusinessObjectInList(items, 0, "abc");
            AssertCorrectlySortedBusinessObjectInList(items, 1, "abcd");
            AssertCorrectlySortedBusinessObjectInList(items, 2, "zzz");
        }

        [Test]
        public void TestSortingCollection_Ascending()
        {
            //---------------Set up test pack-------------------
            BusinessObjectLookupList businessObjectLookupList =
                GetBusinessObjectLookupListForContactPerson("surname asc");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Dictionary<string, string> col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            //---------------Test Result -----------------------
            ArrayList items = new ArrayList(col.Values);
            Assert.AreEqual(3, col.Count);
            AssertCorrectlySortedBusinessObjectInList(items, 0, "abc");
            AssertCorrectlySortedBusinessObjectInList(items, 1, "abcd");
            AssertCorrectlySortedBusinessObjectInList(items, 2, "zzz");
        }

        [Test]
        public void TestSortingCollection_Descending()
        {
            //---------------Set up test pack-------------------
            BusinessObjectLookupList businessObjectLookupList =
                GetBusinessObjectLookupListForContactPerson("surname desc");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Dictionary<string, string> col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            //---------------Test Result -----------------------
            ArrayList items = new ArrayList(col.Values);
            Assert.AreEqual(3, col.Count);
            AssertCorrectlySortedBusinessObjectInList(items, 0, "zzz");
            AssertCorrectlySortedBusinessObjectInList(items, 1, "abcd");
            AssertCorrectlySortedBusinessObjectInList(items, 2, "abc");

        }

        [Test]
        public void Test_CreateDisplayValueDictionary_Sorted()
        {
            //--------------- Set up test pack ------------------
            MyBO.LoadDefaultClassDef();
            MyBO.DeleteAllMyBos();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
            MyBO myBO1 = new MyBO();
            myBO1.Save();
            MyBO myBO2 = new MyBO();
            myBO2.Save();
            MyBO myBO3 = new MyBO();
            myBO3.Save();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO>();
            myBOs.LoadAll();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Dictionary<string, string> dictionary = BusinessObjectLookupList.CreateDisplayValueDictionary(myBOs, true, Convert.ToString);
            //--------------- Test Result -----------------------
            Assert.AreEqual(3, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsValue(myBO1.ID.ToString()));
            Assert.IsTrue(dictionary.ContainsValue(myBO2.ID.ToString()));
            Assert.IsTrue(dictionary.ContainsValue(myBO3.ID.ToString()));
        } 
        
        [Test]
        public void Test_CreateDisplayValueDictionary_NoSort()
        {
            //--------------- Set up test pack ------------------
            MyBO.LoadDefaultClassDef();
            MyBO.DeleteAllMyBos();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
            MyBO myBO1 = new MyBO();
            myBO1.Save();
            MyBO myBO2 = new MyBO();
            myBO2.Save();
            MyBO myBO3 = new MyBO();
            myBO3.Save();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO>();
            myBOs.LoadAll();            
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Dictionary<string, string> dictionary = BusinessObjectLookupList.CreateDisplayValueDictionary(myBOs, false, Convert.ToString);
            //--------------- Test Result -----------------------
            Assert.AreEqual(3, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsValue(myBO1.ID.ToString()));
            Assert.IsTrue(dictionary.ContainsValue(myBO2.ID.ToString()));
            Assert.IsTrue(dictionary.ContainsValue(myBO3.ID.ToString()));
        }
    }
}
