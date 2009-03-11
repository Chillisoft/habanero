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
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBusinessObject.
    /// </summary>
    [TestFixture]
    public class  TestBusinessObject : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            SetupDBConnection();
        }

        [TearDown]
        public void TearDown()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        [SetUp]
        public void SetupTest()
        {
            SetupDBConnection();
            BORegistry.DataAccessor = new DataAccessorDB();
            ClassDef.ClassDefs.Clear();
            new Address();
        }

        [Test]
        public void TestInstantiate()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            bo.GetPropertyValueString("TestProp");
        }

        [Test]
        public void Test_Instantiate_SetsDefaults()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string testPropDefault = TestUtil.GetRandomString();
            MyBO.LoadDefaultClassDefWithDefault(testPropDefault);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            MyBO bo = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(testPropDefault, bo.TestProp);



        }

        [Test]
        public void Test_Instantiate_DefaultValuesAreBackedUp()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string testPropDefault = TestUtil.GetRandomString();
            MyBO.LoadDefaultClassDefWithDefault(testPropDefault);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            MyBO bo = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(testPropDefault, bo.Props["TestProp"].PersistedPropertyValue);
            Assert.AreEqual(null, bo.Props["TestProp2"].PersistedPropertyValue);
        }

        [Test]
        public void Test_Instantiate_NewObjectIdIsBackedUp()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string testPropDefault = TestUtil.GetRandomString();
            MyBO.LoadDefaultClassDefWithDefault(testPropDefault);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            MyBO bo = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(bo.MyBoID, bo.Props["MyBoID"].PersistedPropertyValue);
            Assert.IsTrue(bo.Props["MyBoID"].IsObjectNew);
        }

        [Test]
        public void Test_Instantiate_NewObjectIdRemainsAfterCancelEdit()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string testPropDefault = TestUtil.GetRandomString();
            MyBO.LoadDefaultClassDefWithDefault(testPropDefault);
            MyBO bo = new MyBO();
            Guid id = bo.MyBoID.Value;
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            bo.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(id, bo.MyBoID);
        }

        //[Test]
        //public void TestIndexer()
        //{
        //    MyBO bo = new MyBO();
        //    bo["TestProp"] = "hello";
        //    Assert.AreEqual("hello", bo.GetPropertyValue("TestProp"));
        //}

        [Test]
        public void TestSettingLookupValueSetsGuid()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithLookup();
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "s1");
            Assert.AreEqual("s1", bo.GetPropertyValueToDisplay("TestProp2"));
            Assert.AreEqual(new Guid("{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"), bo.GetPropertyValue("TestProp2"));
        }


        [Test]
        public void Test_ChangeObjectID_FiresIDUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO bo = ContactPersonTestBO.CreateUnsavedContactPerson();
            bool updatedEventFired = false;
            bo.IDUpdated += ((sender, e) => updatedEventFired = true);
            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedEventFired);
            //---------------Execute Test ----------------------
            bo.ContactPersonID = Guid.NewGuid();
            //---------------Test Result -----------------------
            Assert.IsTrue(updatedEventFired);
        }
        [Test]
        public void Test_BusinessObject_WithBrokenRules_ValidUntil_PropsIsValidCalled()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject)classDef.CreateNewBusinessObject();
            IBOProp boProp = bo.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(boProp.IsValid);
            //---------------Execute Test ----------------------
            string invalidReason;
            bo.Props.IsValid(out invalidReason);
            //---------------Test Result -----------------------
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", boProp.InvalidReason);
            Assert.IsFalse(boProp.IsValid);
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", invalidReason);
        }

        [Test]
        public void Test_BusinessObject_WithBrokenRules_ValidUntil_AfterBOProp_IsValidCalled()
        {
            //For performance reasons it was decided to not run the validation code 
            // every time Isvalid is called on the boProp instead the boProp has a validate method
            // which runs the validation code and sets the valid message and status on the BOProp.
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject)classDef.CreateNewBusinessObject();
            IBOProp boProp = bo.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(boProp.IsValid);
            //---------------Execute Test ----------------------
            bool valid = boProp.IsValid;
            //---------------Test Result -----------------------
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(valid);
        }
        [Test]
        public void Test_BusinessObject_WithBrokenRules_ValidUntilIsValidCalled()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject)classDef.CreateNewBusinessObject();
            IBOProp boProp = bo.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(boProp.IsValid);
            //---------------Execute Test ----------------------
            bo.IsValid();
            //---------------Test Result -----------------------
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", boProp.InvalidReason);
            Assert.IsFalse(boProp.IsValid);
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", bo.Status.IsValidMessage);
            Assert.IsFalse(bo.IsValid());
        }
//        [Test]
//        public void Test_BusinessObject_WithNoBrokenRules_Isvalid_AfterValidateCalled()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            ClassDef classDef = MyBO.LoadClassDefsNoUIDef();
//            BusinessObject bo = (BusinessObject)classDef.CreateNewBusinessObject();
//            IBOProp boProp = bo.Props["TestProp"];
//            //---------------Assert Precondition----------------
//            Assert.IsTrue(boProp.IsValid);
//            //---------------Execute Test ----------------------
//            bo.Validate();
//            //---------------Test Result -----------------------
//            Assert.AreEqual("", boProp.InvalidReason);
//            Assert.IsTrue(boProp.IsValid);
//            Assert.AreEqual("", bo.Status.IsValidMessage);
//            Assert.IsTrue(bo.IsValid());
//        }

        [Test]
        public void Test_BusinessObject_TrySaveThrowsUserError_IfWithBrokenRules()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject)classDef.CreateNewBusinessObject();
            IBOProp boProp = bo.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(boProp.IsValid);
            //---------------Execute Test ----------------------
            try
            {
                bo.Save();
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusObjectInAnInvalidStateException ex)
            {
                StringAssert.Contains("Test Prop' is a compulsory field and has no value", ex.Message);
            }
        }
        [Test]
        public void TestGetPropertyValueToDisplay()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithStringLookup();
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "Started");
            Assert.AreEqual("S", bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("Started", bo.GetPropertyValueToDisplay("TestProp2"));
        }


        // This test is duplicated in TestBoMapper.TestGetPropertyValueToDisplay_BusinessObjectLookupList()
        [Test]
        public void TestGetPropertyValueToDisplayWithBOLookupList()
        {
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOLookup();
            ContactPersonTestBO.LoadDefaultClassDef();

            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            ContactPersonTestBO cp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", cp);
            Assert.AreEqual(cp.ContactPersonID, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }


        [Test]
        public void TestBOLookupListWithString()
        {
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOStringLookup();
            ContactPersonTestBO.LoadDefaultClassDef();

            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            ContactPersonTestBO cp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();

            bo.SetPropertyValue("TestProp2", "abc");
            Assert.AreEqual(cp.ContactPersonID.ToString(), bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBOLookupListNull()
        {
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOStringLookup();
            ContactPersonTestBO.LoadDefaultClassDef();

            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", null);
            Assert.AreEqual(null, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual(null, bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBORuleString()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithStringRule();
            Assert.IsTrue(classDef.PropDefcol.Contains("TestProp"), "TestProp must exist");
            IPropDef propDef = classDef.PropDefcol["TestProp"];
            Assert.GreaterOrEqual(1, propDef.PropRules.Count);
            Assert.AreEqual(1, propDef.PropRules.Count);
            Assert.IsNotNull(propDef.PropRules[0], "TestProp must have a rule");
            string errorMessage = "";
            Assert.IsTrue
                (propDef.PropRules[0].IsPropValueValid("TestProp", "abcde", ref errorMessage),
                 "Property value of length 5 must pass");
            Assert.IsFalse
                (propDef.PropRules[0].IsPropValueValid("TestProp", "abcdef", ref errorMessage),
                 "Property value of length 6 must not pass");
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "abcde");
            string reason;
            Assert.IsTrue
                (bo.IsValid(out reason), "BO should be valid with a TestProp value of 'abcde' but returned : " + reason);
            bo.SetPropertyValue("TestProp", "abcdef");
            Assert.IsFalse(bo.IsValid(), "BO should not be valid with a TestProp value of 'abcdef'");
        }

        [Test]
        public void TestApplyEditResetsPreviousValues()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            MockRepository mock = new MockRepository();
            IDatabaseConnection itsConnection = mock.DynamicMock<IDatabaseConnection>();
            Expect.Call(itsConnection.GetConnection()).Return(DatabaseConnection.CurrentConnection.GetConnection()).
                Repeat.Times(2);
            Expect.Call(itsConnection.ExecuteSql(null, null)).IgnoreArguments().Return(1).Repeat.Times(1);
            mock.ReplayAll();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();

            bo.SetPropertyValue("TestProp", "Goodbye");
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            bo.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual("Goodbye", bo.GetPropertyValueString("TestProp"));
        }

        [Test]
        public void TestSaveUpdatesAutoIncrementingField()
        {
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();

            TestAutoInc bo = new TestAutoInc();
            bo.SetPropertyValue("testfield", "testing 123");
            Assert.IsFalse(bo.TestAutoIncID.HasValue);
            bo.Save();
            Assert.IsNotNull(bo.TestAutoIncID);
            Assert.AreNotEqual(0, bo.TestAutoIncID);
            Assert.IsFalse(bo.Status.IsDirty);
        }

        [Test]
        public void TestSaveWithBeforeSaveImplemented()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = BeforeSaveBo.LoadDefaultClassDef();
            BeforeSaveBo bo = (BeforeSaveBo) classDef.CreateNewBusinessObject();
            bo.FirstPart = "foo";
            bo.SecondPart = "bar";
            Assert.AreEqual("", bo.CombinedParts);
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            committer.CommitTransaction();
            //bo.Save();
            Assert.AreEqual("foobar", bo.CombinedParts);
            //mock.VerifyAll();
        }

        [Test]
        public void TestSave_WithAfterSaveImplemented()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = AfterSaveBO.LoadDefaultClassDef();

            AfterSaveBO bo = (AfterSaveBO) classDef.CreateNewBusinessObject();
            bo.FirstPart = "foo";
            bo.SecondPart = "bar";

            //--------------Assert PreConditions----------------     
            Assert.AreEqual("", bo.CombinedParts);

            //---------------Execute Test ----------------------
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual("foobar", bo.CombinedParts);
        }

        //[Test]
        //public void TestSave_ToDifferentDb()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef.ClassDefs.Clear();
        //    ClassDef classDef = MyBO.LoadDefaultClassDef();
        //    MockRepository mock = new MockRepository();
        //    IDatabaseConnection mockDatabaseConnection;
        //    mockDatabaseConnection = TestTransactionCommitter.GetMockDatabaseConnectionWithExpectations(mock);
        //    MyBO myBO = new MyBO(mockDatabaseConnection);
        //    mock.ReplayAll();
        //    //---------------Test Preconditions ----------------

        //    //---------------Execute Test ----------------------
        //    myBO.Save();

        //    //---------------Test Result -----------------------
        //    mock.VerifyAll();
        //}

        [Test]
        public void TestDeleteObjce_WithAfterSaveImplemented()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = AfterSaveBO.LoadDefaultClassDef();

            AfterSaveBO bo = (AfterSaveBO) classDef.CreateNewBusinessObject();
            bo.FirstPart = "foo";
            bo.SecondPart = "bar";
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            committer.CommitTransaction();
            //--------------Assert PreConditions----------------     
            Assert.AreEqual("foobar", bo.CombinedParts);

            //---------------Execute Test ----------------------
            bo.MarkForDelete();
            committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual("deleted", bo.CombinedParts);
        }

        [Test, ExpectedException(typeof (BusObjDeleteException))]
        public void TestCannotDelete_IsDeletable_False()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            IBusinessObject bo = classDef.CreateNewBusinessObject();
            bo.MarkForDelete();
        }

        [Test]
        public void TestCannotDelete_IsDeletable_False_ExpectMessage()
        {
            try
            {
                TestCannotDelete_IsDeletable_False();
                Assert.Fail();
            }
            catch (BusObjDeleteException ex)
            {
                Assert.IsTrue
                    (ex.Message.Contains
                         ("You cannot delete the 'MyBoNotEditableDeletable', as the IsDeleted is set to false for the object"));
            }
        }

        [Test]
        public void TestDeleteWhenNewThrowsException()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            IBusinessObject bo = classDef.CreateNewBusinessObject();
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsNew);
            //---------------Execute Test ----------------------
            try
            {
                bo.MarkForDelete();
                //---------------Test Result -----------------------
                Assert.Fail("Should have thrown an exception");
            }
            catch (HabaneroDeveloperException ex)
            {
                Assert.AreEqual("This 'My B O' cannot be deleted as it has never existed in the database.", ex.Message);
                Assert.AreEqual
                    ("A 'MyBO' cannot be deleted when its status is new and does not exist in the database.",
                     ex.DeveloperMessage);
            }
        }

        [Test]
        public void Test_MarkForDelete()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.Save();
            bool markForDeleteEventFired = false;
            bo.MarkedForDeletion += delegate { markForDeleteEventFired = true; };

            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsFalse(bo.Status.IsDirty);
            Assert.IsFalse(bo.Status.IsEditing);
            Assert.IsFalse(markForDeleteEventFired);

            //---------------Execute Test ----------------------
            bo.MarkForDelete();

            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsTrue(bo.Status.IsDirty);
            Assert.IsTrue(bo.Status.IsEditing);
            Assert.IsTrue(markForDeleteEventFired);
        }

        [Test, ExpectedException(typeof (BusObjEditableException))]
        public void TestCannotEdit_IsEditable_False()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            bo.TestProp = "new";
        }

        [Test]
        public void TestCannotEdit_IsEditable_False_ExpectMessage()
        {
            try
            {
                TestCannotEdit_IsEditable_False();
                Assert.Fail();
            }
            catch (BusObjEditableException ex)
            {
                Assert.IsTrue
                    (ex.Message.Contains
                         ("You cannot Edit the 'MyBoNotEditableDeletable', as the IsEditable is set to false for the object"));
            }
        }

        [Test]
        public void TestCanDelete_IsDeletable_True()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            bo.Save();
            bo.Deletable = true;
            bo.Editable = true;
            bo.MarkForDelete();
        }

        [Test]
        public void TestCanEdit_IsEditable_True()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            bo.Editable = true;
            bo.TestProp = "new";
        }

        [Test]
        public void TestCanEdit_IsDeletable_False()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            bo.Editable = true;
            bo.Deletable = false;
            bo.TestProp = "new";
        }

        [Test]
        public void TestCanDelete_IsEditable_False()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            bo.Save();
            bo.Editable = false;
            bo.Deletable = true;
            bo.MarkForDelete();
        }

        [Test]
        public void TestPropValueHasChanged()
        {
            int? x = 1;
            int? y = 1;
            Assert.IsFalse(BusinessObject.PropValueHasChanged(x, y));

            object z = 1;
            Assert.IsFalse(BusinessObject.PropValueHasChanged(x, z));

            Assert.IsFalse(BusinessObject.PropValueHasChanged(null, null));
            Assert.IsTrue(BusinessObject.PropValueHasChanged(null, x));
            Assert.IsTrue(BusinessObject.PropValueHasChanged(x, null));
            x = null;
            Assert.IsTrue(BusinessObject.PropValueHasChanged(x, y));
        }

        [Test]
        public void TestPropValueHasChanged_DiffTypes()
        {
            int? x = 1;
            int? y = 1;
            Assert.IsFalse(BusinessObject.PropValueHasChanged(x, y));

            const string z = "1";
            Type type = x.GetType();
            Assert.IsFalse(BusinessObject.PropValueHasChanged(Convert.ChangeType(x,type), Convert.ChangeType(z,type)));

            Assert.IsFalse(BusinessObject.PropValueHasChanged(null, null));
            Assert.IsTrue(BusinessObject.PropValueHasChanged(null, x));
            Assert.IsTrue(BusinessObject.PropValueHasChanged(x, null));
            x = null;
            Assert.IsTrue(BusinessObject.PropValueHasChanged(x, y));
        }

        [Test]
        public void TestSaveUsesFactoryGeneratedTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            BORegistry.DataAccessor = new DataAccessorInMemory();
            //---------------Execute Test ----------------------
            cp.Save();
            //---------------Test Result -----------------------
            ContactPersonTestBO loadedCP =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.ID);
            Assert.IsNotNull(loadedCP);
            Assert.AreSame(cp, loadedCP);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetPropertyValue_NullSource()
        {
            //---------------Set up test pack-------------------
            Engine engine1 = new Engine();
            engine1.EngineNo = "20";
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            object engineNo = engine1.GetPropertyValue(null, "EngineNo");
            //---------------Test Result -----------------------
            Assert.AreEqual(engine1.EngineNo, engineNo);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void TestGetPropertyValue_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            Car car1 = new Car();
            car1.CarRegNo = "5";

            Engine engine1 = new Engine();
            engine1.CarID = car1.CarID;
            engine1.EngineNo = "20";

            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(car1);
            committer.AddBusinessObject(engine1);
            committer.CommitTransaction();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            object carregno = engine1.GetPropertyValue(new Source("Car"), "CarRegNo");
            //---------------Test Result -----------------------
            Assert.AreEqual(car1.CarRegNo, carregno);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void TestGetPropertyValue_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            string surname = TestUtil.GetRandomString();
            new Engine(); new Car(); new ContactPerson();
            ContactPerson owner = ContactPerson.CreateSavedContactPerson(surname);
            Car car = Car.CreateSavedCar("5", owner);
            Engine engine = Engine.CreateSavedEngine(car, "20");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            object fetchedSurname = engine.GetPropertyValue(Source.FromString("Car.Owner"), "Surname");
            //---------------Test Result -----------------------
            Assert.AreEqual(surname, fetchedSurname);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void TestBoStatusEqual()
        {
            //---------------Set up test pack-------------------
            BOStatus boStatus1 = new BOStatus(new Car());
            boStatus1.SetBOFlagValue(BOStatus.Statuses.isDeleted, true);
            boStatus1.SetBOFlagValue(BOStatus.Statuses.isDirty, false);
            boStatus1.SetBOFlagValue(BOStatus.Statuses.isEditing, true);
            boStatus1.SetBOFlagValue(BOStatus.Statuses.isNew, false);

            BOStatus boStatus2 = new BOStatus(new Car());
            boStatus2.SetBOFlagValue(BOStatus.Statuses.isDeleted, true);
            boStatus2.SetBOFlagValue(BOStatus.Statuses.isDirty, false);
            boStatus2.SetBOFlagValue(BOStatus.Statuses.isEditing, true);
            boStatus2.SetBOFlagValue(BOStatus.Statuses.isNew, false);
            //---------------Execute Test ----------------------
            bool equal = boStatus1.Equals(boStatus2);
            //---------------Test Result -----------------------
            Assert.IsTrue(equal);
            Assert.AreEqual(boStatus1.GetHashCode(), boStatus2.GetHashCode());
        }

        [Test]
        public void TestBoStatusEqual_Null()
        {
            //---------------Set up test pack-------------------
            BOStatus boStatus1 = new BOStatus(new Car());
            //---------------Execute Test ----------------------
            bool equal = boStatus1.Equals(null);
            //---------------Test Result -----------------------
            Assert.IsFalse(equal);
        }

        [Test]
        public void TestBoStatusNotEqual()
        {
            //---------------Set up test pack-------------------
            BOStatus boStatus1 = new BOStatus(new Car());
            boStatus1.SetBOFlagValue(BOStatus.Statuses.isDeleted, true);

            BOStatus boStatus2 = new BOStatus(new Car());
            boStatus2.SetBOFlagValue(BOStatus.Statuses.isDeleted, false);
            //---------------Execute Test ----------------------
            bool equal = boStatus1.Equals(boStatus2);
            //---------------Test Result -----------------------
            Assert.IsFalse(equal);
            Assert.AreNotEqual(boStatus1.GetHashCode(), boStatus2.GetHashCode());
        }

        [Test]
        public void Test__UpdatePropValueFiresPropertyUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            Engine engine1 = new Engine();
            bool propertyEventFired = false;
            IBusinessObject eventBusinessObject = null;
            IBOProp eventProp = null;
            engine1.PropertyUpdated +=
                delegate(object sender, BOPropUpdatedEventArgs eventArgs)
                {
                    eventBusinessObject = eventArgs.BusinessObject;
                    eventProp = eventArgs.Prop;
                    propertyEventFired = true;
                };
            //-------------Assert Preconditions -------------
            Assert.IsFalse(propertyEventFired);
            Assert.IsNull(eventBusinessObject);
            Assert.IsNull(eventProp);
            //---------------Execute Test ----------------------
            engine1.EngineNo = "20";
            //---------------Test Result -----------------------
            Assert.IsTrue(propertyEventFired);
            Assert.AreSame(engine1,eventBusinessObject);
            Assert.AreSame(engine1.Props["EngineNo"], eventProp);
        }

        [Test]
        public void Test__UpdatePropValueDoesNotFireUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            Engine engine1 = new Engine();
            bool updatedEventFired = false;
            //-------------Assert Preconditions -------------
            Assert.IsFalse(updatedEventFired);
            //---------------Execute Test ----------------------
            engine1.Updated += delegate { updatedEventFired = true; };
            engine1.EngineNo = "20";
            //---------------Test Result -----------------------
            Assert.IsFalse(updatedEventFired);
        }

        [Test]
        public void Test__SaveFiresUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            Engine engine1 = new Engine();
            bool updatedEventFired = false;

            //-------------Assert Preconditions -------------
            Assert.IsFalse(updatedEventFired);
            //---------------Execute Test ----------------------
            engine1.Updated += delegate { updatedEventFired = true; };
            engine1.EngineNo = "20";
            engine1.Save();

            //---------------Test Result -----------------------
            Assert.IsTrue(updatedEventFired);
        }

        [Test]
        public void Test_SetPropertyValue_WithDateTime()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            DateTime newDateTime = DateTime.Now;
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            contactPersonTestBO.SetPropertyValue("DateOfBirth", newDateTime);
            //---------------Test Result -----------------------
            object value = contactPersonTestBO.GetPropertyValue("DateOfBirth");
            Assert.IsInstanceOfType(typeof(DateTime), value);
            Assert.AreEqual(newDateTime, value);
        }

        [Test]
        public void Test_SetPropertyValue_WithDateTimeString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            DateTime newDateTime = DateTime.Today.Add(new TimeSpan(6, 3, 2));
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            contactPersonTestBO.SetPropertyValue("DateOfBirth", newDateTime.ToString());
            //---------------Test Result -----------------------
            object value = contactPersonTestBO.GetPropertyValue("DateOfBirth");
            Assert.IsInstanceOfType(typeof(DateTime), value);
            Assert.AreEqual(newDateTime, value);
        }


        [Test]
        public void Test_SetPropertyValue_WithDateTimeString_Invalid()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string newDateTime = "31/11/2008";
            IBOProp prop = contactPersonTestBO.Props["DateOfBirth"];
            //-------------Assert Preconditions -------------
            Assert.IsNull(prop.Value);
            Assert.IsTrue(prop.IsValid);
            //---------------Execute Test ----------------------
            try
            {
                contactPersonTestBO.SetPropertyValue("DateOfBirth", newDateTime);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string message = string.Format("{0} cannot be set to {1}. It is not a type of"
                              , "DateOfBirth", newDateTime);
                StringAssert.Contains(message, ex.Message);
                StringAssert.Contains("DateTime", ex.Message);
                object value = contactPersonTestBO.GetPropertyValue("DateOfBirth");
                Assert.IsNull(value);
                Assert.IsTrue(prop.IsValid);
            }
        }

        [Test]
        public void Test_SetPropertyValue_WithEnumString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithEnum();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            contactPersonTestBO.SetPropertyValue("ContactType", "Business");
            //---------------Test Result -----------------------
            object value = contactPersonTestBO.GetPropertyValue("ContactType");
            Assert.IsInstanceOfType(typeof(ContactPersonTestBO.ContactType), value);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Business, value);
        }

        [Test]
        public void Test_SetPropertyValue_WithEnumString_Invalid()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithEnum();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string newValue = "InvalidOption";
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            InvalidPropertyValueException exception = null;
            try
            {
                contactPersonTestBO.SetPropertyValue("ContactType", newValue);
            } catch(InvalidPropertyValueException ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Expected exception of type InvalidPropertyValueException");
            StringAssert.Contains("An error occurred while attempting to convert the loaded property value of 'ContactType' to its specified type of 'Habanero.Test.BO.ContactPersonTestBO+ContactType'. The property value is 'InvalidOption'. See log for details", exception.Message);

//            object value = contactPersonTestBO.GetPropertyValue("ContactType");
//            Assert.IsInstanceOfType(typeof (string), value);
//            Assert.AreEqual(newValue, value);
//            IBOProp prop = contactPersonTestBO.Props["ContactType"];
//            Assert.IsFalse(prop.IsValid);
//            StringAssert.Contains(
//                "for property 'Contact Type' is not valid. It is not a type of ContactPersonTestBO+ContactType.",
//                prop.InvalidReason);

            //Habanero.BO.InvalidPropertyValueException: An error occurred while attempting to convert the loaded property value of 'ContactType' to its specified type of 'Habanero.Test.BO.ContactPersonTestBO+ContactType'. The property value is 'InvalidOption'. See log for details
        }

        [Test]
        public void Test_UpdateDirtyStatusFromProperties()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            BOStatus status = (BOStatus) contactPerson.Status;
            status.SetBOFlagValue(BOStatus.Statuses.isDirty, true);

            //-------------Assert Preconditions -------------
            Assert.IsTrue(contactPerson.Status.IsDirty);

            //---------------Execute Test ----------------------
 
            contactPerson.UpdateDirtyStatusFromProperties();
            //---------------Test Result -----------------------

            Assert.IsFalse(contactPerson.Status.IsDirty);
            //---------------Tear Down -------------------------          
        }
    }
}