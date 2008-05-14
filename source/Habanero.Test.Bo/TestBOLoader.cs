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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOLoader : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            SetupDBConnection();
        }

        [SetUp]
        public void SetUp()
        {
            ContactPersonTestBO.CreateSampleData();
        }

        [TearDown]
        public void TearDown()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        [Test]
        public void TestGetBusinessObject()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            Assert.AreEqual("abc", cp.Surname);

            cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc AND FirstName = aa");
            Assert.AreEqual("abc", cp.Surname);

            cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abcde");
            Assert.IsNull(cp);

            cp = (ContactPersonTestBO)BOLoader.Instance.GetBusinessObject(new ContactPersonTestBO(), new Parameter("Surname = invalid"));
            Assert.IsNull(cp);
        }

        [Test, ExpectedException(typeof(UserException))]
        public void TestGetBusinessObjectDuplicatesException_IfTryLoadObjectWithCriteriaThatMatchMoreThanOne()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("FirstName = aa");
        }

        [Test]
        public void TestGetBusinessObjectCollection()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            BusinessObjectCollection<ContactPersonTestBO> col = BOLoader.Instance.GetBusinessObjectCol<ContactPersonTestBO>("FirstName = aa", "Surname");
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual("abc", col[0].Surname);
            Assert.AreEqual("abcd", col[1].Surname);

            IBusinessObjectCollection col2 = BOLoader.Instance.GetBusinessObjectCol(typeof(ContactPersonTestBO), "FirstName = aa", "Surname");
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual("abc", ((ContactPersonTestBO)col2[0]).Surname);
            Assert.AreEqual("abcd", ((ContactPersonTestBO)col2[1]).Surname);

            col = BOLoader.Instance.GetBusinessObjectCol<ContactPersonTestBO>("FirstName = aaaa", "Surname");
            Assert.AreEqual(0, col.Count);
        }

        // Also need to test this works with Sql Server and other Guid forms
        [Test]
        public void TestGetBusinessObjectByIDGuid()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp1 = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            ContactPersonTestBO cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cp1.ContactPersonID);
            Assert.AreEqual(cp1, cp2);
        }

        [Test]
        public void TestGetBusinessObjectByIDInt()
        {
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();

            TestAutoInc tai1 = BOLoader.Instance.GetBusinessObject<TestAutoInc>("TestAutoIncID = 1");
            TestAutoInc tai2 = BOLoader.Instance.GetBusinessObjectByID<TestAutoInc>(tai1.TestAutoIncID.Value);
            Assert.AreEqual(tai1, tai2);
            Assert.AreEqual("testing", tai2.TestField);
        }

        // For this test we pretend for a moment that the Surname column is the primary key
        //   (can change this when we add a table with a real string primary key)
        [Test]
        public void TestGetBusinessObjectByIDString()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithSurnameAsPrimaryKey();

            ContactPersonTestBO cp1 = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            ContactPersonTestBO cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cp1.Surname);
            Assert.AreEqual(cp1, cp2);

            cp1.Surname = "a b";
            cp1.Save();
            cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>("a b");
            Assert.AreEqual(cp1, cp2);
            cp1.Surname = "abc";
            cp1.Save();
        }

        [Test]
        public void TestGetBusinessObjectByIDPrimaryKey()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            
            ContactPersonTestBO cp1 = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            ContactPersonTestBO cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cp1.PrimaryKey);
            Assert.AreEqual(cp1, cp2);
        }

        [Test, ExpectedException(typeof(InvalidPropertyException))]
        public void TestGetBusinessObjectByIDWithMultiplePrimaryKeys()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonCompositeKey.LoadClassDefs();

            BOLoader.Instance.GetBusinessObjectByID<ContactPersonCompositeKey>("someIDvalue");
        }

        [Test]
        public void TestLoadMethod()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp1 = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            cp1.Surname = "def";
            BOLoader.Instance.Load(cp1, new Parameter("Surname = abc"));
            Assert.AreEqual("abc", cp1.Surname);
        }

        [Test]
        public void TestSetDatabaseConnection()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            Assert.IsNotNull(cp.GetDatabaseConnection());
            BOLoader.Instance.SetDatabaseConnection(cp, null);
            Assert.IsNull(cp.GetDatabaseConnection());
        }

        [Test]
        public void TestBOLoaderRefreshCallsAfterLoad()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            Assert.IsFalse(cp.AfterLoadCalled);

            BOLoader.Instance.Refresh(cp);

            Assert.IsTrue(cp.AfterLoadCalled);
        }



        [Test]
        public void TestBoLoaderCallsAfterLoad_LoadingCollection()
        {
            ClassDef.ClassDefs.Clear();
            //--------SetUp testpack --------------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //----Execute Test---------------------------------
            BusinessObjectCollection<ContactPersonTestBO> col = BOLoader.Instance.GetBusinessObjectCol<ContactPersonTestBO>("FirstName = aa", "Surname");
            //----Test Result----------------------------------
            Assert.AreEqual(2, col.Count);
            ContactPersonTestBO bo = col[0];
            Assert.IsTrue(bo.AfterLoadCalled);
        }

        [Test]
        public void TestBoLoaderCallsAfterLoad_ViaSearchCriteria()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            Assert.IsFalse(cp.AfterLoadCalled);
            cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc AND FirstName = aa");
            Assert.IsTrue(cp.AfterLoadCalled);
        }
        [Test]
        public void TestBoLoaderCallsAfterLoad_CompositePrimaryKey()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO cpTemp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.PrimaryKey);
            Assert.IsTrue(cp.AfterLoadCalled);
        }
        [Test]
        public void TestBoLoader_LoadBusinessObjectDeletedByAnotherUser()
        {
            //-------------Setup Test Pack
            ContactPersonTestBO cpTemp = GetContactPersonSavedToDBButNotInObjectManager();

            //-------------Execute test ---------------------
            cpTemp.Delete();
            cpTemp.Save();
            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.PrimaryKey);
            //-------------Test Result ---------------------
            Assert.IsNull(cp);
        }
        [Test]
        [ExpectedException(typeof(BusObjDeleteConcurrencyControlException))]
        public void TestBoLoader_RefreshBusinessObjectDeletedByAnotherUser()
        {
            //-------------Setup Test Pack
            ContactPersonTestBO cpTemp = GetContactPersonSavedToDBButNotInObjectManager();

            //-------------Execute test ---------------------
            cpTemp.Delete();
            cpTemp.Save();
            //-------------Execute Test ---------------------
            try
            {
                BOLoader.Instance.Refresh(cpTemp);
                Assert.Fail();
            }
            //-------------Test Result ---------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("A Error has occured since the object you are trying to refresh has been deleted by another user "));
                throw;
            }
        }
        [Test]
        public void TestBOLoader_RefreshObjects_WhenRetrievingFromObjectManager()
        {
            //-------------Setup Test Pack
            //Create and save a person
            ContactPersonTestBO cpTemp = CreateSavedContactPerson();
            //Clear the object manager so as to simulate a different user
            ContactPersonTestBO.ClearLoadedBusinessObjectBaseCol();
            Assert.AreEqual(0, ContactPersonTestBO.AllLoadedBusinessObjects().Count);
            //Get the person from the object manager so as to ensure that they are loaded 
            // into the object manager.
            ContactPersonTestBO cpTemp2 =
                BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
            //Assert.AreEqual(1, ContactPersonTestBO.AllLoadedBusinessObjects().Count);
            //-------------Execute test ---------------------
            cpTemp.Surname = "New Surname";
            cpTemp.Save();
            Assert.AreNotEqual(cpTemp2.Surname, cpTemp.Surname);

            ContactPersonTestBO cpTemp3 =
                BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
            //Assert.AreEqual(1, ContactPersonTestBO.AllLoadedBusinessObjects().Count);
            //-------------Test Result ---------------------
            Assert.AreEqual(cpTemp.Surname, cpTemp3.Surname);
            Assert.AreSame(cpTemp2.Surname, cpTemp3.Surname);

        }
        [Test]
        public void TestBOLoader_GetObjectFromObjectManager()
        {
            //-------------Setup Test Pack
            //Create and save a person
            ContactPersonTestBO cpTemp = CreateSavedContactPerson();
            //Clear the object manager so as to simulate a different user
            ContactPersonTestBO.ClearLoadedBusinessObjectBaseCol();
            Assert.AreEqual(0, ContactPersonTestBO.AllLoadedBusinessObjects().Count);
            //Get the person from the object manager so as to ensure that they are loaded 
            // into the object manager.
            //-------------Execute test --------------------
            BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
            Assert.AreEqual(1, ContactPersonTestBO.AllLoadedBusinessObjects().Count);

        }
        [Test]
        public void TestBOLoader_GetObjectFromObjectManager_Twice()
        {
            //-------------Setup Test Pack
            //Create and save a person
            ContactPersonTestBO cpTemp = CreateSavedContactPerson();
            //Clear the object manager so as to simulate a different user
            ContactPersonTestBO.ClearLoadedBusinessObjectBaseCol();
            Assert.AreEqual(0, ContactPersonTestBO.AllLoadedBusinessObjects().Count);
            //Get the person from the object manager so as to ensure that they are loaded 
            // into the object manager.
            //-------------Execute test --------------------
            BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
            BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
            //-------------Test Result ---------------------
            Assert.AreEqual(1, ContactPersonTestBO.AllLoadedBusinessObjects().Count);
        }

        [Test]
        public void TestBOLoader_DoesNotRefreshDirtyObjects_WhenRetrievingFromObjectManager()
        {
            //-------------Setup Test Pack
            ContactPersonTestBO cpTemp = CreateSavedContactPerson();

            //-------------Execute test ---------------------
            string newSurnameValue = "New Surname";
            cpTemp.Surname = newSurnameValue;
            cpTemp.AfterLoadCalled = false;
            Assert.IsFalse(cpTemp.AfterLoadCalled); 
            ContactPersonTestBO cpTemp2 =
                BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.ContactPersonID);
            //-------------Test Result ---------------------
            Assert.AreSame(cpTemp2, cpTemp2);
            Assert.AreEqual(newSurnameValue, cpTemp2.Surname);
            Assert.IsFalse(cpTemp2.AfterLoadCalled, "After load should not be called for a dirty object being loaded from DB"); 
        }

        [Test]
        public void TestGetLoadedBusinessObject()
        {
            //Setup
            ContactPersonTestBO contactPersonTestBO = CreateSavedContactPerson();
            //Fixture
            IBusinessObject businessObject = BOLoader.Instance.GetLoadedBusinessObject(contactPersonTestBO.PrimaryKey);
            //Assert
            Assert.AreSame(contactPersonTestBO, businessObject);
        }

        [Test]
        public void TestGetLoadedBusinessObject_DoesNotRefreshNewBo()
        {
            //Setup
            ContactPerson contactPerson = new ContactPerson();
            //Fixture
            IBusinessObject businessObject = BOLoader.Instance.GetLoadedBusinessObject(contactPerson.PrimaryKey);
            //Assert
            Assert.AreSame(contactPerson, businessObject);
        }

        #region HelperMethods

        private static ContactPersonTestBO GetContactPersonSavedToDBButNotInObjectManager()
        {
            ContactPersonTestBO cpTemp = CreateSavedContactPerson();
            //Clear the loaded busiess object so that the we can simulate a user on another machine deleting this object
            BOLoader.Instance.ClearLoadedBusinessObjects();
            return cpTemp;
        }

        private static ContactPersonTestBO CreateSavedContactPerson()
        {
            ClassDef.ClassDefs.Clear();
            BOLoader.Instance.ClearLoadedBusinessObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create a contact person
            ContactPersonTestBO cpTemp = new ContactPersonTestBO();
            cpTemp.Surname = Guid.NewGuid().ToString();
            cpTemp.Save();
            return cpTemp;
        }

        #endregion

    }
}

