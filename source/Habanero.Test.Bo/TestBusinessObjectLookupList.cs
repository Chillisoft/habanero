//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectLookupList : TestUsingDatabase
    {

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
            ContactPersonTestBO.ClearLoadedBusinessObjectBaseCol();
            ContactPersonTestBO.CreateSampleData();
            ContactPersonTestBO.LoadDefaultClassDef();
            BOLoader.Instance.ClearLoadedBusinessObjects();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        [Test]
        public void TestGetLookupList() 
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof (ContactPersonTestBO));

            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(3, col.Count);
            foreach (object o in col.Values) {
                Assert.AreSame(typeof(ContactPersonTestBO), o.GetType());
            }
        }

        [Test]
        public void TestCallingGetLookupListTwiceOnlyAccessesDbOnce()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO));
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Dictionary<string, object> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreSame(col2, col);
        }

        [Test]
        public void TestTimeout()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPersonTestBO), 100);
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Thread.Sleep(250);
            Dictionary<string, object> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreNotSame(col2, col);
        }

        [Test]
        public void TestCriteria()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "surname='zzz'", "");
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(1, col.Count);
        }

        [Test]
        public void TestTodayDateStringCriteria()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPersonTestBO> myCol = new BusinessObjectCollection<ContactPersonTestBO>();
            myCol.LoadAll();
            Assert.AreEqual(myCol.Count, 0);
            ContactPersonTestBO contactPerson1 = new ContactPersonTestBO();
            contactPerson1.Surname = "aaa";
            contactPerson1.DateOfBirth = DateTime.Today.AddDays(-1);
            contactPerson1.Save();
            ContactPersonTestBO contactPerson2 = new ContactPersonTestBO();
            contactPerson2.Surname = "bbb";
            contactPerson2.DateOfBirth = DateTime.Today;
            contactPerson2.Save();
            ContactPersonTestBO contactPerson3 = new ContactPersonTestBO();
            contactPerson3.Surname = "ccc";
            contactPerson3.DateOfBirth = DateTime.Today.AddDays(1);
            contactPerson3.Save();

            BusinessObjectLookupList businessObjectLookupList;
            Dictionary<string, object> col;

            //ContactPersonTestBO.ClearContactPersonCol();
            businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "DateOfBirth < 'Today'", "");
            col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col.ContainsValue(contactPerson1));
            businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "DateOfBirth = 'today'", "");
            col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col.ContainsValue(contactPerson2));
            businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "DateOfBirth >= TODAY", "");
            col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(2, col.Count);
            Assert.IsTrue(col.ContainsValue(contactPerson2));
            Assert.IsTrue(col.ContainsValue(contactPerson3));
        }

        [Test]
        public void TestNowDateStringCriteria()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPersonTestBO> myCol = new BusinessObjectCollection<ContactPersonTestBO>();
            myCol.LoadAll();
            Assert.AreEqual(myCol.Count, 0);
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

            BusinessObjectLookupList businessObjectLookupList;
            Dictionary<string, object> col;

            //ContactPersonTestBO.ClearContactPersonCol();
            businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "DateOfBirth < 'Now'", "");
            col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(3, col.Count);
            Assert.IsTrue(col.ContainsValue(contactPerson1));
            Assert.IsTrue(col.ContainsValue(contactPerson2));
            Assert.IsTrue(col.ContainsValue(contactPerson3));
            businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "DateOfBirth > 'now'", "");
            col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(0, col.Count);
            ContactPersonTestBO contactPerson4 = new ContactPersonTestBO();
            contactPerson4.Surname = "ddd";
            contactPerson4.DateOfBirth = DateTime.Now.AddMinutes(5);
            contactPerson4.Save();
            businessObjectLookupList = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "DateOfBirth > NOW", "");
            col = businessObjectLookupList.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col.ContainsValue(contactPerson4));
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

        [Test]
        public void TestSortingByDefault()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO");
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            ArrayList items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("abc", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("zzz", items[2].ToString());
        }

        [Test]
        public void TestSortingCollection()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPersonTestBO", "", "surname");
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
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
