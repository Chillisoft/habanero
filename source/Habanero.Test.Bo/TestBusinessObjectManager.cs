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
using System.Threading;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectManager
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
            ClassDef.ClassDefs.Clear();
            new Address();
            BusinessObjectManager.Instance.ClearLoadedObjects();
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        #endregion

        protected static void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            SetupDataAccessor();
            new TestUsingDatabase().SetupDBConnection();
        }

        // ReSharper disable AccessToStaticMemberViaDerivedType
        [Test]
        public void Test_CreateObjectManager()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
//            Assert.IsInstanceOfType(typeof(BusinessObjectManager), boMan);
        }

        [Test]
        public void Test_AddedToObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);

            //---------------Execute Test ----------------------
            cp.Surname = TestUtil.GetRandomString();
            boMan.Add(cp);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);

            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsTrue(boMan.Contains(cp.ID));
            //Assert.IsTrue(boMan.Contains(cp.ID.AsString_CurrentValue()));
            Assert.IsTrue(boMan.Contains(cp.ID.ObjectID));
            //Assert.AreSame(cp, boMan[cp.ID.AsString_CurrentValue()]);
            Assert.AreSame(cp, boMan[cp.ID.ObjectID]);
            Assert.AreSame(cp, boMan[cp.ID]);
        }

        [Test]
        public void Test_Contains_ByObjectID_True()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, boMan.Count);
            //--------------- Execute Test ----------------------
            bool isContained = boMan.Contains(cp.ID.ObjectID);
            //--------------- Test Result -----------------------
            Assert.IsTrue(isContained);
        }

        [Test]
        public void Test_Contains_ByObjectID_False()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, boMan.Count);
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
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = TestUtil.GetRandomString()};
            boMan.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));

            //---------------Execute Test ----------------------
            boMan.ClearLoadedObjects();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
            Assert.IsFalse(boMan.Contains(cp));
        }


        //[Test]
        //public void Test_NewObjectNotAddedToObjectManager()
        //{
        //    //---------------Set up test pack-------------------
        //    ContactPersonTestBO.LoadDefaultClassDef();
        //    BusinessObjectManager boMan = BusinessObjectManager.Instance;

        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(0, boMan.Count);

        //    //---------------Execute Test ----------------------
        //    new ContactPersonTestBO();

        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(0, boMan.Count);
        //}

        [Test]
        public void Test_RemoveFromObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = TestUtil.GetRandomString()};
            boMan.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));

            //---------------Execute Test ----------------------
            boMan.Remove(cp);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
            Assert.IsFalse(boMan.Contains(cp));
        }

        [Test]
        public void Test_RemoveFromObjectManager_DerigistersForEvent()
        {
            //When the business object is removed from the object manager
            // you should no longer be registered for the events of the bus
            // object (In this case the ID Updated Event.)
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;
            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = TestUtil.GetRandomString()};
            boMan.ClearLoadedObjects();
            boMan.Add(cp);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsFalse(boMan.UpdatedEventCalled);
            //---------------Execute Test ----------------------
            boMan.Remove(cp);
            cp.ContactPersonID = Guid.NewGuid();
            //---------------Test Result -----------------------
            Assert.IsFalse(boMan.UpdatedEventCalled);
            Assert.AreEqual(0, boMan.Count);
        }

        [Test]
        public void Test_ClearLoadedObjects_DerigistersForEvent()
        {
            //When the business object is removed from the object manager
            // you should no longer be registered for the events of the bus
            // object (In this case the ID Updated Event.)
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;
            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = TestUtil.GetRandomString()};
            boMan.ClearLoadedObjects();
            boMan.Add(cp);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsFalse(boMan.UpdatedEventCalled);
            //---------------Execute Test ----------------------
            boMan.ClearLoadedObjects();
            cp.ContactPersonID = Guid.NewGuid();
            //---------------Test Result -----------------------
            Assert.IsFalse(boMan.UpdatedEventCalled);
            Assert.AreEqual(0, boMan.Count);
        }

        [Test]
        public void Test_ObjectManagerIndexers()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = new ContactPersonTestBO();
            boMan.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);

            //---------------Execute Test ----------------------
            //IBusinessObject boFromObjMan_StringID = boMan[cp.ID.AsString_CurrentValue()];
            IBusinessObject boFromObjMan_StringID = boMan[cp.ID.ObjectID];

            IBusinessObject boFromMan_ObjectID = boMan[cp.ID];

            //---------------Test Result -----------------------
            Assert.AreSame(cp, boFromObjMan_StringID);
            Assert.AreSame(cp, boFromMan_ObjectID);
        }

#pragma warning disable 168
        [Test]
        public void Test_ObjManStringIndexer_ObjectDoesNotExistInObjectMan()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            //ContactPersonTestBO cp = new ContactPersonTestBO();
            Guid guid = Guid.NewGuid();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            try
            {
                //IBusinessObject bo = boMan[cp.ID.AsString_CurrentValue()];
                //IBusinessObject bo = boMan[guid.ToString()];
                IBusinessObject bo = boMan[guid];
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("There is an application error please contact your system administrator", ex.Message);
                StringAssert.Contains("There was an attempt to retrieve the object identified by", ex.DeveloperMessage);
            }
        }

        [Test]
        public void Test_ObjManObjectIndexer_ObjectDoesNotExistInObjectMan()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            //ContactPersonTestBO cp = new ContactPersonTestBO();
            BOPrimaryKey boPrimaryKey = new BOPrimaryKey(new PrimaryKeyDef());
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            try
            {
                //IBusinessObject bo = boMan[cp.ID];
                IBusinessObject bo = boMan[boPrimaryKey];
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("There is an application error please contact your system administrator", ex.Message);
                StringAssert.Contains("There was an attempt to retrieve the object identified by", ex.DeveloperMessage);
            }
        }
#pragma warning restore 168

        [Test]
        public void Test_RemoveObjectFromObjectManagerTwice()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = CreateSavedCP();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.AreSame(cp, boMan[cp.ID]);

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
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = TestUtil.GetRandomString()};

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));

            //---------------Execute Test ----------------------
            cp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.IsTrue(boMan.Contains(cp.ID.ObjectID));
            Assert.IsTrue(boMan.Contains(cp.ID.AsString_CurrentValue()));
            //Assert.AreSame(cp, boMan[cp.ID.AsString_CurrentValue()]);
            Assert.AreSame(cp, boMan[cp.ID.ObjectID]);
            Assert.AreSame(cp, boMan[cp.ID]);
        }

        //Test Add TwiceNoProblem if object is same
        [Test]
        public void Test_AddSameObjectTwiceShouldNotCauseError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            cp.Surname = TestUtil.GetRandomString();
            boMan.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);

            //---------------Execute Test ----------------------
            boMan.Add(cp);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);

            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.IsTrue(boMan.Contains(cp.ID.ObjectID));
            Assert.IsTrue(boMan.Contains(cp.ID.AsString_CurrentValue()));
            //Assert.AreSame(cp, boMan[cp.ID.AsString_CurrentValue()]);
            Assert.AreSame(cp, boMan[cp.ID.ObjectID]);
            Assert.AreSame(cp, boMan[cp.ID]);
        }

        [Test]
        public void Test_SettingTheID_CopyOfSameObjectTwiceShould_ThrowError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            //---------------Execute Test ----------------------
            try
            {
                cp2.ContactPersonID = cp.ContactPersonID;
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("There was a serious developer exception.", ex.Message);
                StringAssert.Contains("Two copies of the business object", ex.Message);
                StringAssert.Contains(" were added to the object manager", ex.Message);
            }
        }

        //Test Add TwiceNoProblem if object is same.
        [Test]
        public void Test_Add_ObjectTwiceToObjectManagerDoesNothing()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            ContactPersonTestBO cp = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            //---------------Execute Test ----------------------
            boMan.Add(cp);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
        }

        //Test add second copy of same object throw error.
        [Test]
        public void Test_Add_CopyOfSameObjectTwiceShould_ThrowError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            cp2.ContactPersonID = cp.ContactPersonID;
            boMan.Add(cp);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.AreEqual(cp.ID.ObjectID, cp2.ID.ObjectID);
            //---------------Execute Test ----------------------
            try
            {
                boMan.Add(cp2);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("There was a serious developer exception.", ex.Message);
                StringAssert.Contains("Two copies of the business object", ex.Message);
                StringAssert.Contains(" were added to the object manager", ex.Message);
            }
        }

        //Test save twice
        [Test]
        public void Test_SavedObject_Twice_AddedToObjectManager_Once()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = TestUtil.GetRandomString()};
            cp.Save();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));

            //---------------Execute Test ----------------------

            cp.Surname = TestUtil.GetRandomString();
            cp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.IsTrue(boMan.Contains(cp.ID.ObjectID));
            Assert.IsTrue(boMan.Contains(cp.ID.AsString_CurrentValue()));
            //Assert.AreSame(cp, boMan[cp.ID.AsString_CurrentValue()]);
            Assert.AreSame(cp, boMan[cp.ID.ObjectID]);
            Assert.AreSame(cp, boMan[cp.ID]);
        }

        [Test]
        public void Test_ContainsBusinessObjectReturnsFalseIfReferenceNotEquals()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            boMan.ManuallyDeregisterForIDUpdatedEvent(originalContactPerson);
            ContactPersonTestBO copyContactPerson = new ContactPersonTestBO();
            boMan.ManuallyDeregisterForIDUpdatedEvent(copyContactPerson);
            boMan.ClearLoadedObjects();
            copyContactPerson.ContactPersonID = originalContactPerson.ContactPersonID;
            boMan.Add(copyContactPerson);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(copyContactPerson));
            //---------------Execute Test ----------------------
            bool containsOrigContactPerson = boMan.Contains(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.IsFalse(containsOrigContactPerson);
        }

        [Test]
        public void Test_ContainsBusinessObject_ReturnsFalse_IfIdDoesNotmatch()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            //boMan.AddBusinessObject(originalContactPerson, "SomeNonMatchingID");
            boMan.AddBusinessObject(originalContactPerson, Guid.NewGuid());

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            //---------------Execute Test ----------------------
            bool containsOrigContactPerson = boMan.Contains(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.IsFalse(containsOrigContactPerson);
        }

        [Test]
        public void Test_ContainsBusinessObject_ReturnsTrue_IfReferenceEquals()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            //boMan.AddBusinessObject(originalContactPerson, originalContactPerson.ID.AsString_CurrentValue());
            boMan.AddBusinessObject(originalContactPerson, originalContactPerson.ID.ObjectID);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            //---------------Execute Test ----------------------
            bool containsOrigContactPerson = boMan.Contains(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.IsTrue(containsOrigContactPerson);
        }

        [Test]
        public void Test_ResetObjectIDProperty_UpdatesKeyInObjectManager()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, boMan.Count);
            //--------------- Execute Test ----------------------
            originalContactPerson.ContactPersonID = Guid.NewGuid();
            //--------------- Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
        }

        [Test]
        public void Test_ContainsBusinessObject_ReturnsTrue_IfPreviousKeyValueEqual_And_ReferenceEquals()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
//            boMan.ManuallyDeregisterForIDUpdatedEvent(originalContactPerson);
            originalContactPerson.ContactPersonID = Guid.NewGuid();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);

            //---------------Execute Test ----------------------
            bool containsOrigContactPerson = boMan.Contains(originalContactPerson);

            //---------------Test Result -----------------------
            Assert.IsTrue(containsOrigContactPerson);
        }

        [Test]
        public void Test_ContainsBusinessObject_ReturnsTrue_IfPersistedKeyValueEqual_And_ReferenceEquals()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            boMan.AddBusinessObject(originalContactPerson, originalContactPerson.ID.PreviousObjectID);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            //---------------Execute Test ----------------------
            bool containsOrigContactPerson = boMan.Contains(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.IsTrue(containsOrigContactPerson);
        }

        [Test]
        public void Test_ContainsBusinessObject_ReturnsFalse_IfPersistedKeyValueEqual_And_ReferenceNotEquals()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            boMan.ManuallyDeregisterForIDUpdatedEvent(originalContactPerson);
            originalContactPerson.Props.BackupPropertyValues();
            ContactPersonTestBO copyContactPerson = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            boMan.AddBusinessObject(copyContactPerson, originalContactPerson.ID.ObjectID);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            //---------------Execute Test ----------------------
            bool containsOrigContactPerson = boMan.Contains(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.IsFalse(containsOrigContactPerson);
        }


        [Test]
        public void Test_ContainsBusinessObject_ReturnsFalse_IfPreviousKeyValueEqual_And_ReferenceNotEquals()
        {
            //---------------Set up test pack-------------------
//            Assert.Fail("not yet implemented test");
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();

            ContactPersonTestBO copyContactPerson = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            boMan.ManuallyDeregisterForIDUpdatedEvent(originalContactPerson);
            copyContactPerson.ContactPersonID = originalContactPerson.ContactPersonID;
            copyContactPerson.Props.BackupPropertyValues();
            boMan.Add(copyContactPerson);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(copyContactPerson));
            Assert.AreEqual(originalContactPerson.ID.AsString_PreviousValue(), copyContactPerson.ID.AsString_PreviousValue());
            //---------------Execute Test ----------------------
            bool containsOrigContactPerson = boMan.Contains(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.IsFalse(containsOrigContactPerson);
        }
//
//        [Test]
//        public void Test_ContainsForCompositeKey_ReturnsFalse_IfPreviousKeyValueEqual_And_ReferenceNotEquals()
//        {
//            //---------------Set up test pack-------------------
//            ContactPersonCompositeKey.LoadClassDefs();
//            BusinessObjectManagerStub.SetNewBusinessObjectManager();
//            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;
//
//            ContactPersonCompositeKey origCp = new ContactPersonCompositeKey
//                                                   {
//                                                       PK1Prop1 = TestUtil.GetRandomString(),
//                                                       PK1Prop2 = TestUtil.GetRandomString()
//                                                   };
//
//            ContactPersonCompositeKey copyContactPerson = new ContactPersonCompositeKey();
//            boMan.ClearLoadedObjects();
//            copyContactPerson.PK1Prop1 = origCp.PK1Prop1;
//            copyContactPerson.PK1Prop2 = origCp.PK1Prop2;
//
//            origCp.Props.BackupPropertyValues();
//            origCp.PK1Prop1 = TestUtil.GetRandomString();
//            copyContactPerson.Props.BackupPropertyValues();
//            copyContactPerson.PK1Prop1 = TestUtil.GetRandomString();
//
//            //boMan.AddBusinessObject(copyContactPerson, origCp.ID.AsString_PreviousValue());
//            boMan.AddBusinessObject(copyContactPerson, origCp.ID.ObjectID);
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(1, boMan.Count);
//            Assert.AreEqual(copyContactPerson.ID.AsString_PreviousValue(), origCp.ID.AsString_PreviousValue());
//            Assert.AreNotEqual(copyContactPerson.ID.AsString_CurrentValue(), origCp.ID.AsString_CurrentValue());
//            Assert.AreNotEqual(copyContactPerson.ID.AsString_PreviousValue(), copyContactPerson.ID.AsString_CurrentValue());
//            Assert.IsTrue(boMan.Contains(copyContactPerson));
//            Assert.IsTrue(boMan.Contains(copyContactPerson.ID.ObjectID));
//            Assert.IsTrue(boMan.Contains(copyContactPerson.ID.AsString_PreviousValue()));
//            Assert.IsFalse(boMan.Contains(copyContactPerson.ID.AsString_CurrentValue()));
//            //Assert.AreSame(copyContactPerson, boMan[copyContactPerson.ID.AsString_PreviousValue()]);
//            Assert.AreSame(copyContactPerson, boMan[copyContactPerson.ID.ObjectID]);
//            //---------------Execute Test AsString_PreviousValue
//            bool containsOrigContactPerson = boMan.Contains(origCp);
//
//            //---------------Test Result -----------------------
//            Assert.IsFalse(containsOrigContactPerson);
//        }

//
//        [Test]
//        public void Test_ContainsForCompositeKey_ReturnsTru_IfPreviousKeyValueEqual_And_ReferenceEquals()
//        {
//            //---------------Set up test pack-------------------
//            ContactPersonCompositeKey.LoadClassDefs();
//            BusinessObjectManagerStub.SetNewBusinessObjectManager();
//            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;
//
//            ContactPersonCompositeKey origCp = new ContactPersonCompositeKey
//                                                   {
//                                                       PK1Prop1 = TestUtil.GetRandomString(),
//                                                       PK1Prop2 = TestUtil.GetRandomString()
//                                                   };
//
//            boMan.ClearLoadedObjects();
//
//            origCp.Props.BackupPropertyValues();
//            origCp.PK1Prop1 = TestUtil.GetRandomString();
//
//            //boMan.AddBusinessObject(origCp, origCp.ID.AsString_PreviousValue());
//            boMan.AddBusinessObject(origCp, origCp.ID.ObjectID);
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(1, boMan.Count);
//            Assert.AreNotEqual(origCp.ID.AsString_PreviousValue(), origCp.ID.AsString_CurrentValue());
//            Assert.IsTrue(boMan.Contains(origCp.ID.AsString_PreviousValue()));
//            Assert.IsFalse(boMan.Contains(origCp.ID.AsString_CurrentValue()));
//            //Assert.AreSame(origCp, boMan[origCp.ID.AsString_PreviousValue()]);
//            Assert.AreSame(origCp, boMan[origCp.ID.ObjectID]);
//            //---------------Execute Test AsString_PreviousValue
//            bool containsOrigContactPerson = boMan.Contains(origCp);
//
//            //---------------Test Result -----------------------
//            Assert.IsTrue(containsOrigContactPerson);
//        }
//
//        [Test]
//        public void Test_ContainsForCompositeKey_ReturnsTrue_IfCurrentKeyValueEqual_And_ReferenceEquals()
//        {
//            //---------------Set up test pack-------------------
//            ContactPersonCompositeKey.LoadClassDefs();
//            BusinessObjectManagerStub.SetNewBusinessObjectManager();
//            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;
//
//            ContactPersonCompositeKey origCp = new ContactPersonCompositeKey
//                                                   {
//                                                       PK1Prop1 = TestUtil.GetRandomString(),
//                                                       PK1Prop2 = TestUtil.GetRandomString()
//                                                   };
//
//            boMan.ClearLoadedObjects();
//
//            origCp.Props.BackupPropertyValues();
//            origCp.PK1Prop1 = TestUtil.GetRandomString();
//
//            //boMan.AddBusinessObject(origCp, origCp.ID.AsString_CurrentValue());
//            boMan.AddBusinessObject(origCp, origCp.ID.ObjectID);
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(1, boMan.Count);
//            Assert.AreNotEqual(origCp.ID.AsString_PreviousValue(), origCp.ID.AsString_CurrentValue());
//            Assert.AreNotEqual(origCp.ID.AsString_LastPersistedValue(), origCp.ID.AsString_CurrentValue());
//            Assert.AreNotEqual(origCp.ID.AsString_LastPersistedValue(), origCp.ID.AsString_PreviousValue());
//            Assert.IsTrue(boMan.Contains(origCp.ID.AsString_CurrentValue()));
//            Assert.IsFalse(boMan.Contains(origCp.ID.AsString_PreviousValue()));
//            //Assert.AreSame(origCp, boMan[origCp.ID.AsString_CurrentValue()]);
//            Assert.AreSame(origCp, boMan[origCp.ID.ObjectID]);
//            //---------------Execute Test AsString_PreviousValue
//            bool containsOrigContactPerson = boMan.Contains(origCp);
//
//            //---------------Test Result -----------------------
//            Assert.IsTrue(containsOrigContactPerson);
//        }
//
//        [Test]
//        public void Test_ContainsForCompositeKey_ReturnsTrue_IfLastPersistedKeyValueEqual_And_ReferenceEquals()
//        {
//            //---------------Set up test pack-------------------
//            ContactPersonCompositeKey.LoadClassDefs();
//            BusinessObjectManagerStub.SetNewBusinessObjectManager();
//            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;
//
//            ContactPersonCompositeKey origCp = new ContactPersonCompositeKey
//                                                   {
//                                                       PK1Prop1 = TestUtil.GetRandomString(),
//                                                       PK1Prop2 = TestUtil.GetRandomString()
//                                                   };
//
//            boMan.ClearLoadedObjects();
//
//            origCp.Props.BackupPropertyValues();
//            origCp.PK1Prop1 = TestUtil.GetRandomString();
//
//            //boMan.AddBusinessObject(origCp, origCp.ID.AsString_LastPersistedValue());
//            boMan.AddBusinessObject(origCp, origCp.ID.ObjectID);
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(1, boMan.Count);
//            Assert.AreNotEqual(origCp.ID.AsString_PreviousValue(), origCp.ID.AsString_CurrentValue());
//            Assert.AreNotEqual(origCp.ID.AsString_LastPersistedValue(), origCp.ID.AsString_CurrentValue());
//            Assert.AreNotEqual(origCp.ID.AsString_LastPersistedValue(), origCp.ID.AsString_PreviousValue());
//            Assert.IsTrue(boMan.Contains(origCp.ID.AsString_LastPersistedValue()));
//            Assert.IsFalse(boMan.Contains(origCp.ID.AsString_PreviousValue()));
//            Assert.IsFalse(boMan.Contains(origCp.ID.AsString_CurrentValue()));
//            //Assert.AreSame(origCp, boMan[origCp.ID.AsString_LastPersistedValue()]);
//            Assert.AreSame(origCp, boMan[origCp.ID.ObjectID]);
//            //---------------Execute Test -----------------
//            bool containsOrigContactPerson = boMan.Contains(origCp);
//
//            //---------------Test Result -----------------------
//            Assert.IsTrue(containsOrigContactPerson);
//        }
//
//        [Test]
//        public void Test_ContainsForCompositeKey_ReturnsFalse_IfCurrentKeyValueEqual_And_ReferenceNotEquals()
//        {
//            //---------------Set up test pack-------------------
//            ContactPersonCompositeKey.LoadClassDefs();
//            BusinessObjectManagerStub.SetNewBusinessObjectManager();
//            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;
//
//            ContactPersonCompositeKey origCp = new ContactPersonCompositeKey
//                                                   {
//                                                       PK1Prop1 = TestUtil.GetRandomString(),
//                                                       PK1Prop2 = TestUtil.GetRandomString()
//                                                   };
//            ContactPersonCompositeKey copyContactPerson = new ContactPersonCompositeKey();
//            boMan.ClearLoadedObjects();
//            copyContactPerson.PK1Prop1 = origCp.PK1Prop1;
//            copyContactPerson.PK1Prop2 = origCp.PK1Prop2;
//            //boMan.AddBusinessObject(copyContactPerson, origCp.ID.AsString_CurrentValue());
//            boMan.AddBusinessObject(copyContactPerson, origCp.ID.ObjectID);
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(1, boMan.Count);
//            Assert.IsTrue(boMan.Contains(copyContactPerson));
//            Assert.IsTrue(boMan.Contains(copyContactPerson.ID));
//            Assert.IsTrue(boMan.Contains(copyContactPerson.ID.AsString_CurrentValue()));
//            //Assert.AreSame(copyContactPerson, boMan[copyContactPerson.ID.AsString_CurrentValue()]);
//            Assert.AreSame(copyContactPerson, boMan[copyContactPerson.ID.ObjectID]);
//            Assert.AreEqual(copyContactPerson.ID.AsString_CurrentValue(), origCp.ID.AsString_CurrentValue());
//            //---------------Execute Test AsString_PreviousValue
//            bool containsOrigContactPerson = boMan.Contains(origCp);
//
//            //---------------Test Result -----------------------
//            Assert.IsFalse(containsOrigContactPerson);
//        }
//
//        [Test]
//        public void Test_ContainsForCompositeKey_ReturnsFalse_IfPersistedKeyValueEqual_And_ReferenceNotEquals()
//        {
//            //---------------Set up test pack-------------------
//            ContactPersonCompositeKey.LoadClassDefs();
//            BusinessObjectManagerStub.SetNewBusinessObjectManager();
//            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;
//
//            ContactPersonCompositeKey origCp = new ContactPersonCompositeKey
//                                                   {
//                                                       PK1Prop1 = TestUtil.GetRandomString(),
//                                                       PK1Prop2 = TestUtil.GetRandomString()
//                                                   };
//            ContactPersonCompositeKey copyContactPerson = new ContactPersonCompositeKey();
//            boMan.ClearLoadedObjects();
//            copyContactPerson.PK1Prop1 = origCp.PK1Prop1;
//            copyContactPerson.PK1Prop2 = origCp.PK1Prop2;
//            //boMan.AddBusinessObject(copyContactPerson, origCp.ID.AsString_LastPersistedValue());
//            boMan.AddBusinessObject(copyContactPerson, origCp.ID.ObjectID);
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(1, boMan.Count);
//            Assert.IsTrue(boMan.Contains(copyContactPerson));
//            Assert.IsTrue(boMan.Contains(copyContactPerson.ID.AsString_LastPersistedValue()));
//            //Assert.AreSame(copyContactPerson, boMan[copyContactPerson.ID.AsString_LastPersistedValue()]);
//            Assert.AreSame(copyContactPerson, boMan[copyContactPerson.ID.ObjectID]);
//            Assert.AreEqual(copyContactPerson.ID.AsString_LastPersistedValue(), origCp.ID.AsString_LastPersistedValue());
//            //---------------Execute Test AsString_PreviousValue
//            bool containsOrigContactPerson = boMan.Contains(origCp);
//
//            //---------------Test Result -----------------------
//            Assert.IsFalse(containsOrigContactPerson);
//        }


        [Test]
        public void Test_RemoveBusinessObject_DoesNotRemoveCurrentValue_ReferenceNotEquals()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO otherContactPersonTestBO = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            //boMan.AddBusinessObject(otherContactPersonTestBO, originalContactPerson.ID.AsString_CurrentValue());
            boMan.AddBusinessObject(otherContactPersonTestBO, originalContactPerson.ID.ObjectID);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsFalse(boMan.Contains(originalContactPerson));
            Assert.IsTrue(boMan.Contains(originalContactPerson.ID.ObjectID));
            //---------------Execute Test ----------------------
            boMan.Remove(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(originalContactPerson.ID.ObjectID));
        }

        [Test]
        public void Test_RemoveBusinessObject_DoesNotRemovePreviousvalue_ReferenceNotEquals()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            originalContactPerson.ContactPersonID = Guid.NewGuid();
            ContactPersonTestBO otherContactPersonTestBO = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            boMan.AddBusinessObject(otherContactPersonTestBO, originalContactPerson.ID.PreviousObjectID);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsFalse(boMan.Contains(originalContactPerson));
            Assert.IsTrue(boMan.Contains(originalContactPerson.ID.PreviousObjectID));
            //---------------Execute Test ----------------------
            boMan.Remove(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
        }


        [Test]
        public void Test_RemoveBusinessObject_Removes_AsCurrentValue_ReferenceNotEqual()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            //boMan.AddBusinessObject(originalContactPerson, originalContactPerson.ID.AsString_CurrentValue());
            boMan.AddBusinessObject(originalContactPerson, originalContactPerson.ID.ObjectID);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(originalContactPerson.ID.AsString_CurrentValue()));
            Assert.AreNotEqual(originalContactPerson.ID.AsString_CurrentValue(), originalContactPerson.ID.AsString_LastPersistedValue());
            Assert.AreNotEqual(originalContactPerson.ID.AsString_PreviousValue(), originalContactPerson.ID.AsString_LastPersistedValue());
            //---------------Execute Test ----------------------
            boMan.Remove(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }

        [Test]
        public void Test_RemoveBusinessObject_Removes_AsPreviousValue_ReferenceNotEqual()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO {ContactPersonID = Guid.NewGuid()};
            boMan.ClearLoadedObjects();
            //boMan.AddBusinessObject(originalContactPerson, originalContactPerson.ID.AsString_PreviousValue());
            boMan.AddBusinessObject(originalContactPerson, originalContactPerson.ID.PreviousObjectID);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(originalContactPerson.ID.PreviousObjectID));
            //---------------Execute Test ----------------------
            boMan.Remove(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }

        [Test]
        public void Test_RemoveBusinessObject_ByStringID_DoesNotRemoveIfRefNotAreSame()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO {ContactPersonID = Guid.NewGuid()};
            ContactPersonTestBO anotherContactperson = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            Guid origGuid = originalContactPerson.ID.ObjectID;
            boMan.AddBusinessObject(anotherContactperson, origGuid);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(origGuid));
            Assert.AreNotSame(originalContactPerson, boMan[origGuid]);
            Assert.AreSame(anotherContactperson, boMan[origGuid]);
            //---------------Execute Test ----------------------
            boMan.TestPrivateRemove(origGuid, originalContactPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
        }

        [Test]
        public void Test_ClearsBOManager()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;

            new ContactPersonTestBO();
            new ContactPersonTestBO();

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boMan.Count);
            //---------------Execute Test ----------------------
            boMan.ClearLoadedObjects();
            //---------------Test Result ----------------------- 
            Assert.AreEqual(0, boMan.Count);
        }

        [Test]
        public void Test_RemoveBusinessObject_ByStringID_Removes_IfRefAreSame()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManagerStub.SetNewBusinessObjectManager();

            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub) BusinessObjectManagerStub.Instance;


            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO {ContactPersonID = Guid.NewGuid()};
            boMan.ClearLoadedObjects();
            //string asString_CurrentValue = originalContactPerson.ID.AsString_CurrentValue();
            Guid objectID = originalContactPerson.ID.ObjectID;
            //boMan.AddBusinessObject(originalContactPerson, asString_CurrentValue);
            boMan.AddBusinessObject(originalContactPerson, objectID);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(objectID));
            Assert.AreSame(originalContactPerson, boMan[objectID]);
            //---------------Execute Test ----------------------
            boMan.TestPrivateRemove(objectID, originalContactPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }


        /// <summary>
        /// <see cref="Test_SaveDuplicateObject_DoesNotAddItselfToObjectManager"/>
        /// </summary>
        [Test]
        public void Test_RemoveSecondInstanceOfSameLoadedObjectDoesNotRemoveIt()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            ContactPersonTestBO copyContactPerson = new ContactPersonTestBO();
            boMan.ClearLoadedObjects();
            copyContactPerson.ContactPersonID = originalContactPerson.ContactPersonID;
            BusinessObjectManager.Instance.Add(copyContactPerson);

            //---------------Assert Precondition----------------
            Assert.AreNotSame(originalContactPerson, copyContactPerson);
            Assert.AreEqual(originalContactPerson.ID.ObjectID, copyContactPerson.ID.ObjectID);
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(copyContactPerson));
            Assert.IsFalse(boMan.Contains(originalContactPerson));
            //---------------Execute Test ----------------------
            BusinessObjectManager.Instance.Remove(originalContactPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(copyContactPerson));
            Assert.IsFalse(boMan.Contains(originalContactPerson));
        }

        [Test]
        public void Test_SaveDuplicateObject_DoesNotAddItselfToObjectManager()
        {
            //This scenario is unlikely to ever happen in normal use but is frequently hit during testing.
            //An object that has a reference to it is removed from the object manager (usually via ClearLoadedObjects).
            // A second instance of the same object is now loaded. This new instance is therefore added to the object manager.
            // The first object is saved. This must not remove the second instance of the object from the object manager 
            // and insert a itself.
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO {Surname = "FirstSurname"};
            originalContactPerson.Save();
            IPrimaryKey origCPID = originalContactPerson.ID;
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);
            Assert.IsFalse(boMan.Contains(originalContactPerson));
            //---------------Execute Test Step 1----------------------
            ContactPersonTestBO myContact2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(origCPID);
            //---------------Test Result Step 1-----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(myContact2));
            //---------------Execute Test Step 2----------------------
            originalContactPerson.Surname = TestUtil.GetRandomString();
            originalContactPerson.Save();
            //---------------Test Result Step 1-----------------------
            Assert.AreNotSame(originalContactPerson, myContact2);
            Assert.AreEqual(1, boMan.Count);
            Assert.IsFalse(boMan.Contains(originalContactPerson), "This object should not have been added to the object manager");
            Assert.IsTrue(boMan.Contains(myContact2), "This object should still b in the object manager");
        }

        //Delete object must remove it from object man.
        [Test]
        public void Test_DeleteObject_RemovesFromObjectMan()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = CreateSavedCP();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.AreSame(cp, boMan[cp.ID]);

            //---------------Execute Test ----------------------
            cp.MarkForDelete();
            cp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }

        //Test edit primary key and save.
        [Test]
        public void Test_SaveForCompositeKey_UpdatedObjectMan()
        {
            //---------------Set up test pack-------------------
            ContactPersonCompositeKey.LoadClassDefs();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            boMan.ClearLoadedObjects();

            ContactPersonCompositeKey cp = new ContactPersonCompositeKey
                                               {
                                                   PK1Prop1 = TestUtil.GetRandomString(),
                                                   PK1Prop2 = TestUtil.GetRandomString()
                                               };

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);

            //---------------Execute Test ----------------------
            cp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.IsTrue(boMan.Contains(cp.ID.ObjectID));
            Assert.AreSame(cp, boMan[cp.ID.ObjectID]);
            Assert.AreSame(cp, boMan[cp.ID]);
        }

        [Test]
        public void Test_ChangePrimaryKeyForCompositeKey_Saved_UpdatedObjectMan()
        {
            //---------------Set up test pack-------------------
            ContactPersonCompositeKey.LoadClassDefs();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonCompositeKey cp = new ContactPersonCompositeKey
                                               {
                                                   PK1Prop1 = TestUtil.GetRandomString(),
                                                   PK1Prop2 = TestUtil.GetRandomString()
                                               };
            cp.Save();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.AreSame(cp, boMan[cp.ID]);

            //---------------Execute Test ----------------------
            cp.PK1Prop1 = TestUtil.GetRandomString();
            cp.PK1Prop2 = TestUtil.GetRandomString();
            cp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.IsTrue(boMan.Contains(cp.ID.ObjectID));
            Assert.AreSame(cp, boMan[cp.ID.ObjectID]);
            Assert.AreSame(cp, boMan[cp.ID]);
        }

        [Test]
        public void Test_ChangePrimaryKeyForCompositeKey_ChangeSecondOne_UpdatedObjectMan_ExplicitAdd()
        {
            //---------------Set up test pack-------------------
            ContactPersonCompositeKey.LoadClassDefs();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonCompositeKey cp = new ContactPersonCompositeKey
                                               {
                                                   PK1Prop1 = TestUtil.GetRandomString(),
                                                   PK1Prop2 = TestUtil.GetRandomString()
                                               };
            cp.Save();
            boMan.ClearLoadedObjects();
            boMan.Add(cp);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.AreSame(cp, boMan[cp.ID]);

            //---------------Execute Test ----------------------
            cp.PK1Prop1 = TestUtil.GetRandomString();
            cp.PK1Prop2 = TestUtil.GetRandomString();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.IsTrue(boMan.Contains(cp.ID.ObjectID));
            Assert.AreSame(cp, boMan[cp.ID.ObjectID]);
            Assert.AreSame(cp, boMan[cp.ID]);
        }

        [Test]
        public void Test_ChangePrimaryKeyForCompositeKey_ChangeSecondOne_DoesNotUpdatedObjectMan()
        {
            //---------------Set up test pack-------------------
            ContactPersonCompositeKey.LoadClassDefs();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonCompositeKey cp = new ContactPersonCompositeKey
                                               {
                                                   PK1Prop1 = TestUtil.GetRandomString(),
                                                   PK1Prop2 = TestUtil.GetRandomString()
                                               };
            cp.Save();
            cp.PK1Prop1 = TestUtil.GetRandomString();
            Guid origIdCurrentValue = cp.ID.ObjectID;
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.IsTrue(boMan.Contains(origIdCurrentValue));
            Assert.AreSame(cp, boMan[cp.ID]);

            //---------------Execute Test ----------------------
            cp.PK1Prop2 = TestUtil.GetRandomString();

            //---------------Test Result -----------------------
            Assert.AreEqual(origIdCurrentValue, cp.ID.ObjectID);
            Assert.AreEqual(origIdCurrentValue, cp.ID.PreviousObjectID);
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsTrue(boMan.Contains(cp.ID));
            Assert.IsTrue(boMan.Contains(cp.ID.ObjectID));
            Assert.AreSame(cp, boMan[cp.ID.ObjectID]);
            Assert.AreSame(cp, boMan[cp.ID]);
        }

        // ReSharper disable RedundantAssignment
        // ReSharper disable UseObjectOrCollectionInitializer
        [Test]
        public void Test_ObjectDestructor_RemovesFromObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.ContactPersonID = Guid.NewGuid();
            cp.Surname = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(cp));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(cp.ID));
            //---------------Execute Test ----------------------
            cp = null;
            TestUtil.WaitForGC();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, BusinessObjectManager.Instance.Count);
        }
        // ReSharper restore UseObjectOrCollectionInitializer
        //Created this test to prove that creating an object with the object initialiser still
        // resulted in the business object moving out of scope.
        [Test]
        public void Test_ObjectDestructor_UsingObjectInitialiser_RemovesFromObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = GetContactPerson();
            //            boMan.Add(cp);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(cp));
            Assert.IsTrue(boMan.Contains(cp.ID));

            //---------------Execute Test ----------------------
            cp = null;
            TestUtil.WaitForGC();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, boMan.Count);
        }

        private static ContactPersonTestBO GetContactPerson()
        {
            ContactPersonTestBO cp = new ContactPersonTestBO 
                    {ContactPersonID = Guid.NewGuid() , Surname = TestUtil.GetRandomString()};
            return cp;
        }
        //Test load objects load them into the boMan
        [Test]
        public void Test_LoadObject_UpdateObjectMan()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = CreateSavedCP();
            IPrimaryKey id = cp.ID;

            cp = null;

            TestUtil.WaitForGC();
            boMan.ClearLoadedObjects();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO contactPersonTestBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(id);

            //---------------Test Result -----------------------
            Assert.IsNotNull(contactPersonTestBO);
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(contactPersonTestBO));
            Assert.IsTrue(boMan.Contains(contactPersonTestBO.ID));
            Assert.IsTrue(boMan.Contains(contactPersonTestBO.ID.ObjectID));
            Assert.AreSame(contactPersonTestBO, boMan[contactPersonTestBO.ID.ObjectID]);
            Assert.AreSame(contactPersonTestBO, boMan[contactPersonTestBO.ID]);
        }
        // ReSharper restore RedundantAssignment

        // ReSharper disable RedundantAssignment
        //Test load objects load them into the boMan
        [Test]
        public void Test_LoadObject_UpdateObjectMan_NonGenericLoad()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = CreateSavedCP();
            IPrimaryKey id = cp.ID;

            cp = null;

            TestUtil.WaitForGC();
            boMan.ClearLoadedObjects();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO contactPersonTestBO = (ContactPersonTestBO)BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, id);

            //---------------Test Result -----------------------
            Assert.IsNotNull(contactPersonTestBO);
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(contactPersonTestBO));

            Assert.IsTrue(boMan.Contains(id));
            Assert.IsTrue(boMan.Contains(id.AsString_CurrentValue()));
            Assert.AreSame(contactPersonTestBO, boMan[id.ObjectID]);
            Assert.AreSame(contactPersonTestBO, boMan[id]);
        }
        // ReSharper restore RedundantAssignment

        // ReSharper disable RedundantAssignment
        //Test load objects via colleciton loads into boMan.
        [Test]
        public void Test_LoadObject_ViaCollection_UpdatedObjectMan_NonGeneric()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = CreateSavedCP();
            Guid contactPersonId = cp.ContactPersonID;
            IPrimaryKey id = cp.ID;
            cp = null;

            TestUtil.WaitForGC();
            boMan.ClearLoadedObjects();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("ContactPersonID", Criteria.ComparisonOp.Equals, contactPersonId);
            IBusinessObjectCollection colContactPeople = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, colContactPeople.Count);
            IBusinessObject loadedCP = colContactPeople[0];
            Assert.IsNotNull(loadedCP);

            Assert.AreNotSame(cp, loadedCP);

            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(loadedCP));
            Assert.IsTrue(boMan.Contains(id));
            Assert.IsTrue(boMan.Contains(id.ObjectID));
            Assert.AreSame(loadedCP, boMan[id]);
            Assert.AreSame(loadedCP, boMan[id.ObjectID]);
        }
        // ReSharper restore RedundantAssignment

        //Test load objects via stronglytypedcollection loads into boMan.
        // ReSharper disable RedundantAssignment
        //Test load objects via colleciton loads into boMan.
        [Test]
        public void Test_LoadObject_ViaCollection_UpdatedObjectMan_Generic()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = CreateSavedCP();
            Guid contactPersonId = cp.ContactPersonID;
            IPrimaryKey id = cp.ID;
            cp = null;

            TestUtil.WaitForGC();
            boMan.ClearLoadedObjects();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("ContactPersonID", Criteria.ComparisonOp.Equals, contactPersonId);
            IBusinessObjectCollection colContactPeople =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, colContactPeople.Count);
            IBusinessObject loadedCP = colContactPeople[0];
            Assert.IsNotNull(loadedCP);

            Assert.AreNotSame(cp, loadedCP);

            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(loadedCP));
            Assert.IsTrue(boMan.Contains(id));
            Assert.IsTrue(boMan.Contains(id.ObjectID));
            Assert.AreSame(loadedCP, boMan[id]);
            Assert.AreSame(loadedCP, boMan[id.ObjectID]);
        }
        // ReSharper restore RedundantAssignment

        // ReSharper disable RedundantAssignment
        //Test load via multiple relationship loads into boMan.
        [Test]
        public void Test_LoadObject_MulitpleRelationship_UpdatedObjectMan_Generic()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressTestBOsRelationship();
            new AddressTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = CreateSavedCP();
            AddressTestBO address = new AddressTestBO { ContactPersonID = cp.ContactPersonID };
            address.Save();

            IPrimaryKey contactPersonID = cp.ID;
            IPrimaryKey addresssID = address.ID;
            cp = null;
            address = null;

            TestUtil.WaitForGC();
            boMan.ClearLoadedObjects();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(contactPersonID);
            RelatedBusinessObjectCollection<AddressTestBO> addresses = loadedCP.AddressTestBOs;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, addresses.Count);
            Assert.AreEqual(2, boMan.Count);

            Assert.IsTrue(boMan.Contains(loadedCP));
            Assert.AreSame(loadedCP, boMan[contactPersonID]);

            AddressTestBO loadedAddress = addresses[0];

            Assert.IsTrue(boMan.Contains(loadedAddress));
            Assert.IsTrue(boMan.Contains(addresssID));
            Assert.IsTrue(boMan.Contains(addresssID.ObjectID));
            Assert.AreSame(loadedAddress, boMan[addresssID]);
            Assert.AreSame(loadedAddress, boMan[addresssID.ObjectID]);
        }
        // ReSharper restore RedundantAssignment


        //Test load single relationship loads into boMan.
        // ReSharper disable RedundantAssignment
        //Test load via multiple relationship loads into boMan.
        [Test]
        public void Test_LoadObject_SingleRelationship_UpdatedObjectMan_Generic()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressTestBOsRelationship();
            new AddressTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            ContactPersonTestBO cp = CreateSavedCP();
            AddressTestBO address = new AddressTestBO { ContactPersonID = cp.ContactPersonID };
            address.Save();

            IPrimaryKey contactPersonID = cp.ID;
            IPrimaryKey addresssID = address.ID;
            cp = null;
            address = null;

            TestUtil.WaitForGC();
            boMan.ClearLoadedObjects();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);

            //---------------Execute Test ----------------------
            AddressTestBO loadedAddress = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<AddressTestBO>(addresssID);
            ContactPersonTestBO loadedCP = loadedAddress.ContactPersonTestBO;

            //---------------Test Result -----------------------
            Assert.IsNotNull(loadedCP);
            Assert.AreEqual(2, boMan.Count);

            Assert.IsTrue(boMan.Contains(loadedCP));
            Assert.IsTrue(boMan.Contains(contactPersonID));
            Assert.AreSame(loadedCP, boMan[contactPersonID]);

            Assert.IsTrue(boMan.Contains(loadedAddress));
            Assert.IsTrue(boMan.Contains(addresssID));
            Assert.IsTrue(boMan.Contains(addresssID.ObjectID));
            Assert.AreSame(loadedAddress, boMan[addresssID]);
            Assert.AreSame(loadedAddress, boMan[addresssID.ObjectID]);
        }
        // ReSharper restore RedundantAssignment

        //Testloading objects when already other objects in object manager
        // ReSharper disable RedundantAssignment
        [Test]
        public void Test_LoadObjectWhenAlreadyObjectInObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressTestBOsRelationship();
            new AddressTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;

            AddressTestBO address;
            ContactPersonTestBO cp = CreateSavedCP_WithOneAddresss(out address);

            IPrimaryKey contactPersonID = cp.ID;
            IPrimaryKey addresssID = address.ID;
            cp = null;
            address = null;

            TestUtil.WaitForGC();
            boMan.ClearLoadedObjects();

            AddressTestBO addressOut1;
            AddressTestBO addressOut2;
            AddressTestBO addressOut3;
            CreateSavedCP_WithOneAddresss(out addressOut1);
            CreateSavedCP_WithOneAddresss(out addressOut2);
            CreateSavedCP_WithOneAddresss(out addressOut3);

            //---------------Assert Precondition----------------
            Assert.AreEqual(6, boMan.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(contactPersonID);
            RelatedBusinessObjectCollection<AddressTestBO> addresses = loadedCP.AddressTestBOs;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, addresses.Count);
            Assert.AreEqual(8, boMan.Count);

            Assert.IsTrue(boMan.Contains(loadedCP));
            Assert.AreSame(loadedCP, boMan[contactPersonID]);

            AddressTestBO loadedAddress = addresses[0];

            Assert.IsTrue(boMan.Contains(loadedAddress));
            Assert.IsTrue(boMan.Contains(addresssID));
            Assert.IsTrue(boMan.Contains(addresssID.ObjectID));
            Assert.AreSame(loadedAddress, boMan[addresssID]);
            Assert.AreSame(loadedAddress, boMan[addresssID.ObjectID]);
        }
        [Test]
        public void Test_ReturnSameObjectFromBusinessObjectLoader()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressTestBOsRelationship();
            new AddressTestBO();
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            ContactPersonTestBO originalContactPerson = CreateSavedCP();
            IPrimaryKey id = originalContactPerson.ID;
            originalContactPerson = null;
            boMan.ClearLoadedObjects();
            TestUtil.WaitForGC();

            //load second object from DB to ensure that it is now in the object manager
            ContactPersonTestBO myContact2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(id);

            //---------------Assert Precondition----------------
            Assert.AreNotSame(originalContactPerson, myContact2);

            //---------------Execute Test ----------------------
            ContactPersonTestBO myContact3 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(id);

            //---------------Test Result -----------------------
            Assert.AreNotSame(originalContactPerson, myContact3);
            Assert.AreSame(myContact2, myContact3);
        }


        private static ContactPersonTestBO CreateSavedCP_WithOneAddresss(out AddressTestBO address)
        {
            ContactPersonTestBO cp = CreateSavedCP();
            address = new AddressTestBO { ContactPersonID = cp.ContactPersonID };
            address.Save();
            return cp;
        }

        // ReSharper restore RedundantAssignment


        [Test]
        public void Test3LayerLoadRelated()
        {
            //---------------Set up test pack-------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.DeleteAllContactPeople();
            BORegistry.DataAccessor = new DataAccessorDB();
            OrganisationTestBO.LoadDefaultClassDef();
            TestUtil.WaitForGC();
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            OrganisationTestBO org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();

            //---------------Assert Preconditions --------------
            Assert.AreEqual(3, BusinessObjectManager.Instance.Count);

            //---------------Execute Test ----------------------
            var colContactPeople = org.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople").BusinessObjectCollection;
            ContactPersonTestBO loadedCP = colContactPeople[0];
            var colAddresses = loadedCP.Relationships.GetMultiple<AddressTestBO>("Addresses").BusinessObjectCollection;
            AddressTestBO loadedAdddress = colAddresses[0];

            //---------------Test Result -----------------------
            Assert.AreEqual(3, BusinessObjectManager.Instance.Count);
            Assert.AreEqual(1, colAddresses.Count);
            Assert.AreSame(contactPersonTestBO, loadedCP);
            Assert.AreSame(address, loadedAdddress);
        }

        [Test]
        public void Test_NewObjectInObjectManager()
        {
            //---------------Set up test pack-------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, BusinessObjectManager.Instance.Count);
            //---------------Execute Test ----------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //---------------Test Result -----------------------
            Assert.IsTrue(contactPersonTestBO.Status.IsNew);
            Assert.IsNotNull(contactPersonTestBO.ContactPersonID);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO.ID.AsString_CurrentValue()));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO.ID));
            Assert.IsFalse(BusinessObjectManager.Instance.Contains(contactPersonTestBO.ContactPersonID.ToString("B")));
        }

        [Test]
        public void Test_SavedObjectInObjectManager()
        {
            //---------------Set up test pack-------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO { Surname = TestUtil.GetRandomString() };
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Status.IsNew);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO));
            //---------------Execute Test ----------------------
            contactPersonTestBO.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.Status.IsNew);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO.ID));
        }
        // ReSharper disable RedundantAssignment
        [Test]
        public void Test_ObjectRemovedFromObjectManager()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO));
            //--------------- Execute Test ----------------------
            contactPersonTestBO = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //--------------- Test Result -----------------------
            Assert.AreEqual(0, BusinessObjectManager.Instance.Count);
        }

        // ReSharper restore RedundantAssignment
        [Ignore("This is a known issue where the business object id is reset and then reset again. Two instances of the object will be in the object manager and one will never be removed" +
            " This is not a big issue since this will not cause any wierd behaviour as only a weak reference is held in the object manager")]
        [Test]
        public void Test_NewObject_ObjectManagerUpdated_WhenIdChangedTwice_Guid()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            Guid firstCpID = Guid.NewGuid();
            Guid secondCpId = Guid.NewGuid();
            contactPersonTestBO.ContactPersonID = firstCpID;

            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Status.IsNew);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.AreNotEqual(firstCpID, secondCpId);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO.ID.ObjectID));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO.ID));
            //---------------Execute Test ----------------------

            contactPersonTestBO.ContactPersonID = secondCpId;
            //---------------Test Result -----------------------
            Assert.IsTrue(contactPersonTestBO.Status.IsNew);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO.ID.ObjectID));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPersonTestBO.ID));
        }

        [Test]
        public void Test_NewObjectInObjectManager_Int()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, BusinessObjectManager.Instance.Count);
            //---------------Execute Test ----------------------
            BOWithIntID boWithIntID = new BOWithIntID();
            //---------------Test Result -----------------------
            Assert.IsTrue(boWithIntID.Status.IsNew);
            Assert.IsNull(boWithIntID.IntID);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID.ID.AsString_CurrentValue()));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID.ID));
        }

        [Test]
        public void Test_ChangeKey()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID boWithIntID = new BOWithIntID();
            //--------------- Test Preconditions ----------------
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID));
            //--------------- Execute Test ----------------------
            boWithIntID.IntID = TestUtil.GetRandomInt();
            //--------------- Test Result -----------------------
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID));

        }
        [Test]
        public void Test_SavedObjectInObjectManager_Int()
        {
            //---------------Set up test pack-------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID boWithIntID = new BOWithIntID { IntID = TestUtil.GetRandomInt() };
            //---------------Assert Precondition----------------
            Assert.IsTrue(boWithIntID.Status.IsNew);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID));
            //---------------Execute Test ----------------------
            boWithIntID.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(boWithIntID.Status.IsNew);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID.ID));
        }

        // ReSharper disable RedundantAssignment
        [Test]
        public void Test_ObjectRemovedFromObjectManager_Int()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID boWithIntID = new BOWithIntID();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID));
            //--------------- Execute Test ----------------------
            boWithIntID = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //--------------- Test Result -----------------------
            Assert.AreEqual(0, BusinessObjectManager.Instance.Count);
        }
        // ReSharper restore RedundantAssignment

        [Test]
        public void Test_NewObject_ObjectManagerUpdated_WhenIdChangedOnce()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID boWithIntID = new BOWithIntID();
            int firstIntID = TestUtil.GetRandomInt();
            int secondIntID = TestUtil.GetRandomInt();
            boWithIntID.IntID = firstIntID;

            //---------------Assert Precondition----------------
            Assert.IsTrue(boWithIntID.Status.IsNew);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.AreNotEqual(firstIntID, secondIntID);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID.ID));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID.ID.ObjectID));
            //---------------Execute Test ----------------------

            boWithIntID.IntID = secondIntID;
            //---------------Test Result -----------------------
            Assert.IsTrue(boWithIntID.Status.IsNew);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID.ID.ObjectID));
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(boWithIntID.ID));
        }

        [Test]
        public void Test_ChangeObject_NonObjectIdDoesNot_ChangeKeyInObjectManager()
        {
            //---------------Set up test pack-------------------
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            boMan.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID boWithIntID = new BOWithIntID();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(boWithIntID));
            Guid objectID = boWithIntID.ID.ObjectID;
            Assert.IsTrue(boMan.Contains(objectID));
            //---------------Execute Test ----------------------
            boWithIntID.IntID = 2;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boMan.Count);
            Assert.IsTrue(boMan.Contains(boWithIntID));
            Assert.IsTrue(boMan.Contains(objectID));
            Assert.IsTrue(boMan.Contains(boWithIntID.ID.ObjectID));

        }

        [Test]
        public void Test_TwoObjectTypesWithTheSameIDField_HaveTheSamevalue_CanBeAddedToObjectMan()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            boMan.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID_DifferentType.LoadClassDefWithIntID();
            const int id = 3;
            BOWithIntID boWithIntID = new BOWithIntID { IntID = id };
            boMan.ClearLoadedObjects();
            BOWithIntID_DifferentType boWithIntID_DifferentType = new BOWithIntID_DifferentType { IntID = id };
            boMan.ClearLoadedObjects();
            boMan.Add(boWithIntID);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, boMan.Count);
            //--------------- Execute Test ----------------------
            boMan.Add(boWithIntID_DifferentType);
            //--------------- Test Result -----------------------
            Assert.AreEqual(2, boMan.Count);
            Assert.IsTrue(boMan.Contains(boWithIntID_DifferentType));
            Assert.IsTrue(boMan.Contains(boWithIntID));
        }
        [Test]
        public void Test_TwoObjectTypesWithTheSameIDField_EdidtedToHaveTheSamevalue_CanBeAddedToObjectMan()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            boMan.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID_DifferentType.LoadClassDefWithIntID();
            const int id = 3;
            BOWithIntID boWithIntID = new BOWithIntID { IntID = id };
            BOWithIntID_DifferentType boWithIntID_DifferentType = new BOWithIntID_DifferentType { IntID = 6 };
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(2, boMan.Count);
            //--------------- Execute Test ----------------------
            boWithIntID_DifferentType.IntID = boWithIntID.IntID;
            //--------------- Test Result -----------------------
            Assert.AreEqual(2, boMan.Count);
            Assert.IsTrue(boMan.Contains(boWithIntID_DifferentType));
            Assert.IsTrue(boMan.Contains(boWithIntID));
        }
        public void Test_TwoObjectTypesWithTheSameIDField_EditedToHaveTheSamevalue_CanBeAddedToObjectMan_PreviousPropValue()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub)BusinessObjectManagerStub.Instance;
            boMan.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID_DifferentType.LoadClassDefWithIntID();
            const int id = 3;
            BOWithIntID boWithIntID = new BOWithIntID { IntID = id };
            BOWithIntID_DifferentType boWithIntID_DifferentType = new BOWithIntID_DifferentType { IntID = 6 };
            boWithIntID_DifferentType.IntID = boWithIntID.IntID;
            boMan.ClearLoadedObjects();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(0, boMan.Count);
            //--------------- Execute Test ----------------------
            boMan.AddBusinessObject(boWithIntID, boWithIntID.ID.ObjectID);
            boMan.AddBusinessObject(boWithIntID_DifferentType, boWithIntID_DifferentType.ID.ObjectID);
            //--------------- Test Result -----------------------
            Assert.AreEqual(2, boMan.Count);
            Assert.IsTrue(boMan.Contains(boWithIntID_DifferentType));
            Assert.IsTrue(boMan.Contains(boWithIntID));
        }

        public void Test_TwoObjectTypesWithTheSameIDField_EditedToHaveTheSamevalue_CanBeAddedToObjectMan_AsString_LastPersistedValue()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub)BusinessObjectManagerStub.Instance;
            boMan.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID_DifferentType.LoadClassDefWithIntID();
            const int id = 3;
            BOWithIntID boWithIntID = new BOWithIntID { IntID = id };
            BOWithIntID_DifferentType boWithIntID_DifferentType = new BOWithIntID_DifferentType { IntID = 6 };
            boWithIntID_DifferentType.IntID = boWithIntID.IntID;
            boMan.ClearLoadedObjects();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(0, boMan.Count);
            //--------------- Execute Test ----------------------
            boMan.AddBusinessObject(boWithIntID, boWithIntID.ID.ObjectID);
            boMan.AddBusinessObject(boWithIntID_DifferentType, boWithIntID_DifferentType.ID.ObjectID);
            //--------------- Test Result -----------------------
            Assert.AreEqual(2, boMan.Count);
            Assert.IsTrue(boMan.Contains(boWithIntID_DifferentType));
            Assert.IsTrue(boMan.Contains(boWithIntID));
        }
        [Test]
        public void Test_TestInheritedObjectCanStillGetObjectOutOfManager_HOwDoesKeyKnowType()
        {
            //---------------Set up test pack-------------------
            BusinessObjectManager boMan = BusinessObjectManager.Instance;
            BOWithIntID_Child.LoadClassDefWith_SingleTableInherit();
            const int id = 3;
            BOWithIntID boWithIntID = new BOWithIntID { IntID = id };
            boMan.ClearLoadedObjects();
            BOWithIntID_Child boWithIntID_Child = new BOWithIntID_Child { IntID = id };
            boMan.ClearLoadedObjects();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, boMan.Count);
            //---------------Execute Test ----------------------
            boMan.Add(boWithIntID);
            boMan.Add(boWithIntID_Child);
            //--------------Test Result-------------------------
            Assert.AreEqual(2, boMan.Count);

        }
        [Test]
        public void Test_Find_TwoObjectTypesWithTheSameIDField_HaveSameValue()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManagerStub.SetNewBusinessObjectManager();
            BusinessObjectManagerStub boMan = (BusinessObjectManagerStub)BusinessObjectManagerStub.Instance;
            boMan.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID_DifferentType.LoadClassDefWithIntID();
            const int id = 3;
            BOWithIntID boWithIntID = new BOWithIntID { IntID = id };
            BOWithIntID_DifferentType boWithIntID_DifferentType = new BOWithIntID_DifferentType { IntID = id };
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(2, boMan.Count);
            Assert.IsTrue(boMan.Contains(boWithIntID_DifferentType));
            Assert.IsTrue(boMan.Contains(boWithIntID));
            //--------------- Execute Test ----------------------
            BusinessObjectCollection<BOWithIntID> found = boMan.Find<BOWithIntID>(new Criteria("IntID", Criteria.ComparisonOp.Equals, id));
            //--------------- Test Result -----------------------
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(2, boMan.Count);
            Assert.IsTrue(boMan.Contains(boWithIntID_DifferentType));
            Assert.IsTrue(boMan.Contains(boWithIntID));
        }
        [Test]
        public void Test_Find_NotFound()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, TestUtil.GetRandomString());

            //--------------- Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> found = BusinessObjectManager.Instance.Find<ContactPersonTestBO>(criteria);

            //--------------- Test Result -----------------------
            Assert.AreEqual(0, found.Count);
        }

        [Test]
        public void Test_Find_OneMatch()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            string surname = cp.Surname = TestUtil.GetRandomString();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);

            //--------------- Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> found = BusinessObjectManager.Instance.Find<ContactPersonTestBO>(criteria);

            //--------------- Test Result -----------------------
            Assert.AreEqual(1, found.Count);
            Assert.AreSame(cp, found[0]);

        }

        [Test]
        public void Test_Find_Null_ReturnsAllOfType()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
            ContactPersonTestBO.LoadDefaultClassDef();
#pragma warning disable 168
            ContactPersonTestBO bo1 = new ContactPersonTestBO();  
            ContactPersonTestBO bo2 = new ContactPersonTestBO();
            ContactPersonTestBO bo3 = new ContactPersonTestBO();
#pragma warning restore 168
            //----------------Assert preconditions ---------------
            Assert.AreEqual(3, BusinessObjectManager.Instance.Count);
            //--------------- Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> found = BusinessObjectManager.Instance.Find<ContactPersonTestBO>(null);
            //--------------- Test Result -----------------------
            Assert.AreEqual(3, found.Count);

        }


        private static ContactPersonTestBO CreateSavedCP()
        {
            ContactPersonTestBO cp = new ContactPersonTestBO
                                         {
                                             Surname = TestUtil.GetRandomString(),
                                             FirstName = TestUtil.GetRandomString()
                                         };
            cp.Save();
            return cp;
        }

        public class BusinessObjectManagerStub : BusinessObjectManager
        {
            public bool _updatedEventCalled;

            protected BusinessObjectManagerStub()
            {
            }

            public bool UpdatedEventCalled
            {
                get { return _updatedEventCalled; }
                private set { _updatedEventCalled = value; }
            }

            public static void SetNewBusinessObjectManager()
            {
                _businessObjectManager = new BusinessObjectManagerStub();
                ((BusinessObjectManagerStub) _businessObjectManager).UpdatedEventCalled = false;
            }

            public void ManuallyDeregisterForIDUpdatedEvent(ContactPersonTestBO person)
            {
                DeregisterForIDUpdatedEvent(person);
                ((BusinessObjectManagerStub) _businessObjectManager).UpdatedEventCalled = false;
            }

            //public void AddBusinessObject(IBusinessObject bo, string keyStringToAddBy)
            //{
            //    _loadedBusinessObjects.Add(keyStringToAddBy, new WeakReference(bo));
            //    ((BusinessObjectManagerStub)_businessObjectManager).UpdatedEventCalled = false;
            //}

            public void AddBusinessObject(IBusinessObject bo, Guid keyGuidToAddBy)
            {
                _loadedBusinessObjects.Add(keyGuidToAddBy, new WeakReference(bo));
                ((BusinessObjectManagerStub) _businessObjectManager).UpdatedEventCalled = false;
            }

            protected override void ObjectID_Updated_Handler(object sender, BOEventArgs e)
            {
                base.ObjectID_Updated_Handler(sender, e);
                _updatedEventCalled = true;
            }

            //public void TestPrivateRemove(string asString_CurrentValue, IBusinessObject businessObject)
            //{
            //    this.Remove(asString_CurrentValue, businessObject);
            //}  

            public void TestPrivateRemove(Guid asString_CurrentValue, IBusinessObject businessObject)
            {
                this.Remove(asString_CurrentValue, businessObject);
            }
        }

        // ReSharper restore AccessToStaticMemberViaDerivedType
    }
}