#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectManagerNull
    {
        #region Setup/Teardown
        // ReSharper disable PossibleNullReferenceException
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
            ClassDef.ClassDefs.Clear();
            //new Address();
            FixtureEnvironment.ResetBORegistryBusinessObjectManager();
            FixtureEnvironment.ClearBusinessObjectManager();
        }
        // ReSharper restore PossibleNullReferenceException

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        #endregion


        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        // ReSharper disable AccessToStaticMemberViaDerivedType
        [Test]
        public void Test_CreateObjectManager()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
//            Assert.IsInstanceOf(typeof(BusinessObjectManager), boMan);
        }

        [Test]
        public void Test_AddedToObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            boMan.Add(cp);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count, "The Business Object should not be added to the Object Manager");
        }

        [Test]
        public void Test_Contains_ByObjectID_ShouldBeFalse()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(0, boMan.Count);
            //--------------- Execute Test ----------------------
            bool isContained = boMan.Contains(cp.ID.ObjectID);
            //--------------- Test Result -----------------------
            Assert.IsFalse(isContained);
        }

        [Test]
        public void Test_Contains_ByObjectID_False()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(0, boMan.Count);
            //--------------- Execute Test ----------------------
            bool isContained = boMan.Contains(Guid.Empty);
            //--------------- Test Result -----------------------
            Assert.IsFalse(isContained);
        }

        [Test]
        public void Test_ClearLoadedObjects()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = TestUtil.GetRandomString()};
            boMan.Add(cp);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);
            //---------------Execute Test ----------------------
            boMan.ClearLoadedObjects();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
            Assert.IsFalse(boMan.Contains(cp));
        }

        [Test]
        public void Test_RemoveFromObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = TestUtil.GetRandomString()};
            boMan.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            boMan.Remove(cp);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
            Assert.IsFalse(boMan.Contains(cp));
        }

        [Test]
        public void Test_ObjectManagerIndexers()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            boMan.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            //IBusinessObject boFromObjMan_StringID = boMan[cp.ID.AsString_CurrentValue()];
            IBusinessObject boFromObjMan_StringID = boMan[cp.ID.ObjectID];

            IBusinessObject boFromMan_ObjectID = boMan[cp.ID];

            //---------------Test Result -----------------------
            Assert.IsNull(boFromObjMan_StringID);
            Assert.IsNull(boFromMan_ObjectID);
        }

#pragma warning disable 168

#pragma warning restore 168

        [Test]
        public void Test_RemoveObjectFromObjectManagerTwice()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();

            ContactPersonTestBO cp = CreateSavedCP();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            boMan.Remove(cp);
            boMan.Remove(cp);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }

        [Test]
        public void Test_SavedObjectAddedToObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();

            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = TestUtil.GetRandomString()};

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            cp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }

        //Test Add TwiceNoProblem if object is same
        [Test]
        public void Test_AddSameObjectTwiceShouldNotCauseError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            cp.Surname = TestUtil.GetRandomString();
            boMan.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            boMan.Add(cp);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }

        [Test]
        public void Test_SettingTheID_CopyOfSameObjectTwiceShould_DoesNotThrowError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            BORegistry.BusinessObjectManager = boMan;
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);
            //---------------Execute Test ----------------------

            cp2.ContactPersonID = cp.ContactPersonID;

            //---------------Test Result -----------------------
            Assert.IsTrue(true, "If gets here then OK");
        }

        //Test Add TwiceNoProblem if object is same.
        [Test]
        public void Test_Add_ObjectTwiceToObjectManagerDoesNothing()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);
            //---------------Execute Test ----------------------
            boMan.Add(cp);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }

        [Test]
        public void Test_ContainsBusinessObject_ReturnsFalse()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);
            //---------------Execute Test ----------------------
            bool containsOrigContactPerson = boMan.Contains(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.IsFalse(containsOrigContactPerson);
        }


        [Test]
        public void Test_RemoveBusinessObject_DoesNothing()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            ContactPersonTestBO originalContactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);
            //---------------Execute Test ----------------------
            boMan.Remove(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }

        [Test]
        public void Test_ClearsBOManager_DoesNothing()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            new ContactPersonTestBO();
            new ContactPersonTestBO();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);
            //---------------Execute Test ----------------------
            boMan.ClearLoadedObjects();
            //---------------Test Result ----------------------- 
            Assert.AreEqual(0, boMan.Count);
        }


        // ReSharper restore RedundantAssignment


        [Test]
        public void Test_Find_NotFound()
        {
            //--------------- Set up test pack ------------------
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            boMan.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, TestUtil.GetRandomString());

            //--------------- Execute Test ----------------------
            IList<ContactPersonTestBO> found = boMan.Find<ContactPersonTestBO>(criteria);

            //--------------- Test Result -----------------------
            Assert.AreEqual(0, found.Count);
        }

        [Test]
        public void Test_Find_OneMatch()
        {
            //--------------- Set up test pack ------------------
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            boMan.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            string surname = cp.Surname = TestUtil.GetRandomString();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);

            //--------------- Execute Test ----------------------
            IList<ContactPersonTestBO> found = boMan.Find<ContactPersonTestBO>(criteria);

            //--------------- Test Result -----------------------
            Assert.AreEqual(0, found.Count);
        }


        [Test]
        public void Test_Find_NonGeneric_NotFound()
        {
            //--------------- Set up test pack ------------------
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            boMan.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, TestUtil.GetRandomString());

            //--------------- Execute Test ----------------------
            //BusinessObjectCollection<ContactPersonTestBO> found = BORegistry.BusinessObjectManager.Find<ContactPersonTestBO>(criteria);
            IList found = boMan.Find(criteria, typeof(ContactPersonTestBO));

            //--------------- Test Result -----------------------
            Assert.AreEqual(0, found.Count);
        }

        [Test]
        public void Test_Find_NonGeneric_OneMatch()
        {
            //--------------- Set up test pack ------------------

            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            string surname = cp.Surname = TestUtil.GetRandomString();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);

            //--------------- Execute Test ----------------------
            IList found = boMan.Find(criteria, typeof(ContactPersonTestBO));

            //--------------- Test Result -----------------------
            Assert.AreEqual(0, found.Count);
        }


        [Test]
        public void Test_FindFirst_OneMatch_ShouldReturnNull()
        {
            //--------------- Set up test pack ------------------
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            boMan.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            string surname = cp.Surname = TestUtil.GetRandomString();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);

            //--------------- Execute Test ----------------------
            IBusinessObject found = boMan.FindFirst<ContactPersonTestBO>(criteria);

            //--------------- Test Result -----------------------
            Assert.IsNull(found);
        }


        [Test]
        public void Test_FindFirst_NonGeneric_OneMatch_ShouldReturnNull()
        {
            //--------------- Set up test pack ------------------
            IBusinessObjectManager boMan = new BusinessObjectManagerNull();
            boMan.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            string surname = cp.Surname = TestUtil.GetRandomString();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);

            //--------------- Execute Test ----------------------
            IBusinessObject found = boMan.FindFirst(criteria, typeof(ContactPersonTestBO));

            //--------------- Test Result -----------------------
//            Assert.AreEqual(1, found.Count);
            Assert.IsNull(found);
        }



        private static ContactPersonTestBO CreateSavedCP()
        {
            ContactPersonTestBO cp = new ContactPersonTestBO
                                         {Surname = TestUtil.GetRandomString(), FirstName = TestUtil.GetRandomString()};
            cp.Save();
            return cp;
        }


        // ReSharper restore AccessToStaticMemberViaDerivedType
    }
}