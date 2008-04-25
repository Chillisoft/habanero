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
    public class TestBusinessObject : TestUsingDatabase
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

        [Test]
        public void TestInstantiate()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            bo.GetPropertyValueString("TestProp");
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
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "s1");
            Assert.AreEqual("s1", bo.GetPropertyValueToDisplay("TestProp2"));
            Assert.AreEqual(new Guid("{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"), bo.GetPropertyValue("TestProp2"));
        }


        [Test]
        public void TestGetPropertyValueToDisplay()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithStringLookup();
            BusinessObject bo = classDef.CreateNewBusinessObject();
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

            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            BusinessObject bo = classDef.CreateNewBusinessObject();
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

            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "abc");
            Assert.AreEqual(cp.ID.ToString(), bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBOLookupListNull()
        {
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOStringLookup();
            ContactPersonTestBO.LoadDefaultClassDef();

            BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            BusinessObject bo = classDef.CreateNewBusinessObject();
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
            PropDef propDef = classDef.PropDefcol["TestProp"];
            Assert.IsNotNull(propDef.PropRule, "TestProp must have a rule");
            string errorMessage = "";
            Assert.IsTrue(propDef.PropRule.isPropValueValid("TestProp", "abcde", ref errorMessage), "Property value of length 5 must pass");
            Assert.IsFalse(propDef.PropRule.isPropValueValid("TestProp", "abcdef", ref errorMessage), "Property value of length 6 must not pass");
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "abcde");
            Assert.IsTrue(bo.IsValid(), "BO should be valid with a TestProp value of 'abcde'");
            bo.SetPropertyValue("TestProp", "abcdef");
            Assert.IsFalse(bo.IsValid(), "BO should not be valid with a TestProp value of 'abcdef'");
        }

        [Test]
        public void TestApplyEditResetsPreviousValues()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
			MockRepository mock = new MockRepository();
        	IDatabaseConnection itsConnection = mock.DynamicMock<IDatabaseConnection>();
            //Mock itsDatabaseConnectionMockControl = new DynamicMock(typeof (IDatabaseConnection));
            //IDatabaseConnection itsConnection = (IDatabaseConnection) itsDatabaseConnectionMockControl.MockInstance;
        	Expect.Call(itsConnection.GetConnection())
				.Return(DatabaseConnection.CurrentConnection.GetConnection())
				.Repeat.Times(2);
			Expect.Call(itsConnection.ExecuteSql(null, null))
                .IgnoreArguments()
				.Return(1)
				.Repeat.Times(1);
			//itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
			//                                                 DatabaseConnection.CurrentConnection.GetConnection());
			//itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
			mock.ReplayAll();

            MyBO bo = (MyBO) classDef.CreateNewBusinessObject(itsConnection);
			//bo.SetPropertyValue("TestProp", "Hello") ;
			//bo.Save() ;


            bo.SetPropertyValue("TestProp", "Goodbye");
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            committer.CommitTransaction();
            bo.Restore();
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
            Assert.IsFalse(bo.State.IsDirty);
        }

        [Test]
        public void TestSaveWithBeforeSaveImplemented()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = BeforeSaveBo.LoadDefaultClassDef();
            //MockRepository mock = new MockRepository();
            //IDatabaseConnection itsConnection = mock.DynamicMock<IDatabaseConnection>();
            //Expect.Call(itsConnection.GetConnection())
            //    .Return(DatabaseConnection.CurrentConnection.GetConnection())
            //    .Repeat.Times(2);
            //Expect.Call(itsConnection.ExecuteSql(null, null))
            //    .IgnoreArguments()
            //    .Return(1)
            //    .Repeat.Times(1);
            //mock.ReplayAll();

            BeforeSaveBo bo = (BeforeSaveBo)classDef.CreateNewBusinessObject();
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

        [Test, ExpectedException(typeof(BusObjDeleteException))]
        public void TestCannotDelete_IsDeletable_False()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.Delete();
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
                Assert.IsTrue(
                    ex.Message.Contains(
                        "You cannot delete the 'MyBoNotEditableDeletable', as the IsDeleted is set to false for the object"));
            }
        }

        [Test, ExpectedException(typeof(BusObjEditableException))]
        public void TestCannotEdit_IsEditable_False()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable)classDef.CreateNewBusinessObject();
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
                Assert.IsTrue(
                    ex.Message.Contains(
                        "You cannot Edit the 'MyBoNotEditableDeletable', as the IsEditable is set to false for the object"));
            }
        }

        [Test]
        public void TestCanDelete_IsDeletable_True()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable)classDef.CreateNewBusinessObject();
            bo.Deletable = true;
            bo.Editable = true;
            bo.Delete();
        }

        [Test]
        public void TestCanEdit_IsEditable_True()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable)classDef.CreateNewBusinessObject();
            bo.Editable = true;
            bo.TestProp = "new";
        }

        [Test]
        public void TestCanEdit_IsDeletable_False()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable)classDef.CreateNewBusinessObject();
            bo.Editable = true;
            bo.Deletable = false;
            bo.TestProp = "new";
        }

        [Test]
        public void TestCanDelete_IsEditable_False()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable)classDef.CreateNewBusinessObject();
            bo.Editable = false;
            bo.Deletable = true;
            bo.Delete();
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
    }
}