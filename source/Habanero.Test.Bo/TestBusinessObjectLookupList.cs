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
using System.Drawing;
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
    [TestFixture]
    public abstract class TestBusinessObjectLookupList : TestUsingDatabase
    {
        protected abstract void SetupDataAccessor();
        protected abstract void DeleteAllContactPeople();

        public class TestBusinessObjectLookupListDB : TestBusinessObjectLookupList
        {
            protected override void SetupDataAccessor()
            {
                BORegistry.DataAccessor = new DataAccessorDB();
            }

            protected override void DeleteAllContactPeople()
            {
                ContactPersonTestBO.DeleteAllContactPeople();
            }
        }

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
        protected static string GuidToUpperInvariant(Guid guid)
        {
            return guid.ToString("B").ToUpperInvariant();
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
        public void TestTimeout()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO), 100);
            source.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);
            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Thread.Sleep(250);
            Dictionary<string, string> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreNotSame(col2, col);
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
            Assert.IsTrue(col.ContainsValue(GuidToUpperInvariant(contactPerson1.ID.GetAsGuid())));
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
            Assert.IsTrue(col.ContainsValue(GuidToUpperInvariant(contactPerson2.ID.GetAsGuid())));
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
            Assert.IsTrue(col.ContainsValue(GuidToUpperInvariant(contactPerson3.ID.GetAsGuid())));
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
            Assert.IsTrue(col.ContainsValue(GuidToUpperInvariant(contactPerson1.ID.GetAsGuid())));
            Assert.IsTrue(col.ContainsValue(GuidToUpperInvariant(contactPerson2.ID.GetAsGuid())));
            Assert.IsTrue(col.ContainsValue(GuidToUpperInvariant(contactPerson3.ID.GetAsGuid())));
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
            Assert.IsTrue(col.ContainsValue(GuidToUpperInvariant(contactPerson4.ID.GetAsGuid())));
        }

        [Test]
        public void TestSortAttribute()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO));
            Assert.IsNull(source.Sort);

            source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "", "surname");
            Assert.AreEqual("surname", source.Sort);

            source.Sort = "surname asc";
            Assert.AreEqual("surname asc", source.Sort);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortInvalidProperty()
        {
            new BusinessObjectLookupList("Habanero.Test.BO",
                                         "ContactPersonTestBO", "", "invalidprop");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortInvalidDirection()
        {
            new BusinessObjectLookupList("Habanero.Test.BO",
                                         "ContactPersonTestBO", "", "surname invalidorder");
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
        [Test, Ignore("We are now storing the object and not the GUid so sorting tests need to be reworked")]
//        [Test]
        public void TestSortingByDefault()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO", "ContactPersonTestBO");
            new PropDef("N", typeof(Guid), PropReadWriteRule.ReadWrite, null) {LookupList = source};
            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            ArrayList items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("abc", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("zzz", items[2].ToString());
        }

        [Test, Ignore("We are now storing the object and not the GUid so sorting tests need to be reworked")]
//        [Test]
        public void TestSortingCollection()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "", "surname");
            new PropDef("N", typeof(Guid), PropReadWriteRule.ReadWrite, null) { LookupList = source };

            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            ArrayList items = new ArrayList(col.Values);
            
            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("abc", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("zzz", items[2].ToString());

            source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "", "surname asc");
            col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("abc", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("zzz", items[2].ToString());

            source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "", "surname desc");
            col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("zzz", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("abc", items[2].ToString());

            source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "", "surname des");
            col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("zzz", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("abc", items[2].ToString());
        }
    }
}
