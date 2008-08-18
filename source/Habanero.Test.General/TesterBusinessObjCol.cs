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
using Habanero.BO.ObjectManager;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for BusinessObjColTester.
    /// </summary>
    [TestFixture]
    public class TesterBusinessObjCol : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            SetupDBConnection();
        }

        /// <summary>
        /// Used by Gui to step through the application. If the reason for failing a test is 
        /// not obvious.
        /// </summary>
        public static void RunTest()
        {
            TesterBusinessObjCol test = new TesterBusinessObjCol();
            //test.CreateTestPack();
            //			test.TestLoadBusinessObjects();
            //			test.TestLoadBusinessObjectsFromObjectManager();
            //			test.TestLoadBusinessObjectsSearchCriteria();
            //			test.TestLoadBusinessObjects();
        }

        private static IPrimaryKey CreateSavedContactPersonWithOneAddress(string addressLine)
        {
            ContactPerson contactPerson = new ContactPerson();
            contactPerson.FirstName = "a";
            contactPerson.Surname = "bb";
            contactPerson.Save();
            IPrimaryKey contactPersonKey = contactPerson.ID;

            Address address = new Address();
            address.AddressLine1 = addressLine;
            address.ContactPersonID = contactPerson.ContactPersonID;
            address.Save();
            return contactPersonKey;
        }

        [Test]
        public void TestLoadBusinessObjects()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(0, myCol.Count);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            BusObjectManager.Instance.ClearLoadedObjects();
            myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(1, myCol.Count);
        }


        [Test]
        public void TestLoadBusinessObjectsFromObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
//            ContactPerson.
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();

            //---------------Assert Precondition----------------
            Assert.AreEqual(myCol.Count, 0);

            //---------------Execute Test ----------------------
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            IPrimaryKey pKey = p.ID;
            BusObjectManager.Instance.ClearLoadedObjects();
            // ReSharper disable RedundantAssignment
            p = null;
// ReSharper restore RedundantAssignment
            TestUtil.WaitForGC();
            p = ContactPerson.GetContactPerson(pKey);
            myCol = ContactPerson.LoadBusinessObjCol();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, myCol.Count);
            Assert.AreSame(p, myCol[0]);
        }

        [Test]
        public void TestLoadBusinessObjectsFromObjectManagerAndFresh()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            IPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();
            BusObjectManager.Instance.ClearLoadedObjects();
            p = ContactPerson.GetContactPerson(pKey);

            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname", OrderCriteria.SortDirection.Ascending);
//            myCol = ContactPerson.LoadBusinessObjCol("", "Surname ASC");
            myCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPerson>(null, orderCriteria);
            Assert.AreEqual(2, myCol.Count);

            Assert.AreSame(p, myCol[1]);
        }

        [Test]
        public void TestLoadBusinessObjectsSearchCriteria()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            IPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();
            BusObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
            p = ContactPerson.GetContactPerson(pKey);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
            Criteria criteria1 = new Criteria("Surname", Criteria.ComparisonOp.Equals, "bb");
//            myCol = ContactPerson.LoadBusinessObjCol("Surname = 'bb'", "Surname");
            myCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPerson>(criteria1);
            Assert.AreEqual(1, myCol.Count);
            Assert.AreSame(p, myCol[0]);
        }

        [Test, Ignore("this needs to be rewritten using new Business Object loader")]
        public void TestLoadBusinessObjectsSearchCriteria_RelatedObjectCriteria()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);

            //-----------------Set up ------------------------------------
            const string addressLine = "Chillisoft";
            IPrimaryKey contactPersonKey = CreateSavedContactPersonWithOneAddress(addressLine);

            ContactPerson.CreateSavedContactPerson();
            BusObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();

            //------------------------Assert Precondition --------------------------------
            Assert.AreEqual(0, BusObjectManager.Instance.Count);

            //------------------------Execute Test ---------------------------------------
            ContactPerson contactPerson = ContactPerson.GetContactPerson(contactPersonKey);


            myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(2, myCol.Count);

            myCol = ContactPerson.LoadBusinessObjCol("Addresses.AddressLine1 = '" + addressLine + "'", "Surname");

            //-------------------------TestResult ---------------------------------------------------
            Assert.AreEqual(1, myCol.Count);
            Assert.AreSame(contactPerson, myCol[0]);
        }

        [Test]
        public void TestLoadBusinessObjectsSearchCriteriaWithOR_AndLike()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            IPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();

            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abcd";
            p.Save();

            BusObjectManager.Instance.ClearLoadedObjects();

            Criteria criteria1 = new Criteria("Surname", Criteria.ComparisonOp.Equals, "bb");
            Criteria criteria2 = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            Criteria criteria = new Criteria(criteria1, Criteria.LogicalOp.Or, criteria2);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
//            myCol = ContactPerson.LoadBusinessObjCol("Surname = 'bb' or Surname = 'abc'", "Surname");
            myCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPerson>(criteria,
                                                                                                            orderCriteria);
            Assert.AreEqual(2, myCol.Count);

            //criteria1 = new Criteria("Surname", Criteria.ComparisonOp.Equals, "bb");
            //criteria2 = new Criteria("Surname", Criteria.ComparisonOp.Like, "abc");
            //criteria = new Criteria(criteria1, Criteria.LogicalOp.Or, criteria2);
            //TODO: Loader does not have an operator like.
            myCol = ContactPerson.LoadBusinessObjCol("Surname = 'bb' or Surname like 'abc%'", "Surname");
            Assert.AreEqual(3, myCol.Count);
        }

        [Test]
        public void TestLoadBusinessObjectsSortOrder()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            IPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();
            BusObjectManager.Instance.ClearLoadedObjects();
            p = ContactPerson.GetContactPerson(pKey);
//            myCol = ContactPerson.LoadBusinessObjCol("", "Surname Desc");
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname", OrderCriteria.SortDirection.Descending);
            myCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPerson>(null, orderCriteria);
            Assert.AreEqual(2, myCol.Count);
            Assert.AreSame(p, myCol[0]);
        }

        [Test]
        public void TestLoadBusinessObjectsWithNowDateStringSearchCriteria()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson contactPerson1 = new ContactPerson();
            contactPerson1.FirstName = "a";
            contactPerson1.Surname = "aaa";
            contactPerson1.DateOfBirth = DateTime.Now.AddMinutes(-1);
            contactPerson1.Save();
            ContactPerson contactPerson2 = new ContactPerson();
            contactPerson2.FirstName = "b";
            contactPerson2.Surname = "bbb";
            contactPerson2.DateOfBirth = DateTime.Now.AddMinutes(-1);
            contactPerson2.Save();
            ContactPerson contactPerson3 = new ContactPerson();
            contactPerson3.FirstName = "c";
            contactPerson3.Surname = "ccc";
            contactPerson3.DateOfBirth = DateTime.Now.AddMinutes(-1);
            contactPerson3.Save();

            //ContactPerson.ClearObjectManager();
            //TODO: Implement this need < 'Now' 
            myCol = ContactPerson.LoadBusinessObjCol("DateOfBirth < 'Now'", "FirstName");
            Assert.AreEqual(3, myCol.Count);
            Assert.AreSame(contactPerson1, myCol[0]);
            Assert.AreSame(contactPerson2, myCol[1]);
            Assert.AreSame(contactPerson3, myCol[2]);
            myCol = ContactPerson.LoadBusinessObjCol("DateOfBirth > 'now'", "FirstName");
            Assert.AreEqual(0, myCol.Count);
            ContactPerson contactPerson4 = new ContactPerson();
            contactPerson4.FirstName = "d";
            contactPerson4.Surname = "ddd";
            contactPerson4.DateOfBirth = DateTime.Now.AddMinutes(5);
            contactPerson4.Save();
            myCol = ContactPerson.LoadBusinessObjCol("DateOfBirth > NOW", "FirstName");
            Assert.AreEqual(1, myCol.Count);
            Assert.AreSame(contactPerson4, myCol[0]);
        }

        [Test]
        public void TestLoadBusinessObjectsWithTodayDateStringSearchCriteria()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson contactPerson1 = new ContactPerson();
            contactPerson1.FirstName = "a";
            contactPerson1.Surname = "aaa";
            contactPerson1.DateOfBirth = DateTime.Today.AddDays(-1);
            contactPerson1.Save();

            ContactPerson contactPerson2 = new ContactPerson();
            contactPerson2.FirstName = "b";
            contactPerson2.Surname = "bbb";
            contactPerson2.DateOfBirth = DateTime.Today;
            contactPerson2.Save();

            ContactPerson contactPerson3 = new ContactPerson();
            contactPerson3.FirstName = "c";
            contactPerson3.Surname = "ccc";
            contactPerson3.DateOfBirth = DateTime.Today.AddDays(1);
            contactPerson3.Save();


            //ContactPerson.ClearObjectManager();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThan, "Today");
//            myCol = ContactPerson.LoadBusinessObjCol("DateOfBirth < 'Today'", "DateOfBirth");
            myCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPerson>(criteria);
            Assert.AreEqual(1, myCol.Count);
            Assert.AreSame(contactPerson1, myCol[0]);

            criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, "Today");
//            myCol = ContactPerson.LoadBusinessObjCol("DateOfBirth = today", "DateOfBirth");
            myCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPerson>(criteria);
            Assert.AreEqual(1, myCol.Count);
            Assert.AreSame(contactPerson2, myCol[0]);

            criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, "Today");
            myCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPerson>(criteria);
            Assert.AreEqual(1, myCol.Count);
            Assert.AreSame(contactPerson3, myCol[0]);

            myCol = ContactPerson.LoadBusinessObjCol("DateOfBirth >= 'TODAY'", "DateOfBirth");
            Assert.AreEqual(2, myCol.Count);
            Assert.AreSame(contactPerson2, myCol[0]);
            Assert.AreSame(contactPerson3, myCol[1]);
        }

        [Test]
        public void TestRefreshBOCol()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<ContactPerson> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            IPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();

            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abcd";
            p.Save();

            BusObjectManager.Instance.ClearLoadedObjects();

            Criteria criteria1 = new Criteria("Surname", Criteria.ComparisonOp.Equals, "bb");
            Criteria criteria2 = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            Criteria criteria = new Criteria(criteria1, Criteria.LogicalOp.Or, criteria2);
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Surname");
//            myCol = ContactPerson.LoadBusinessObjCol("Surname = 'bb' or Surname = 'abc'", "Surname");
            myCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPerson>(criteria,
                                                                                                            orderCriteria);
            Assert.AreEqual(2, myCol.Count);
            //ensure that a new object is created to edit. to simulate multi user editing of objects.
            BusObjectManager.Instance.ClearLoadedObjects();
            p = ContactPerson.GetContactPerson(pKey);

            p.Surname = "zzz";
            p.Save();

            Assert.AreEqual(2, myCol.Count,
                            "The object collection should not have changed since the physical object edited is different.");

            myCol.Refresh();
            Assert.AreEqual(1, myCol.Count,
                            "The object collection should now have fewer object since it has been reloaded from the database.");

            p = myCol[0];
            Assert.AreEqual("abc", p.Surname);
        }
    }
}