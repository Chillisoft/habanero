//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
using NMock;
using NUnit.Framework;
using Rhino.Mocks;
using BusinessObject=Habanero.BO.BusinessObject;

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
            base.SetupDBConnection();
        }

        [Test]
        public void TestInstantiate()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            string t = bo.GetPropertyValueString("TestProp");
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


        [Test]
        public void TestGetPropertyValueToDisplayWithBOLookupList()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOLookup();
            ContactPerson.LoadDefaultClassDef();
            ContactPerson cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", cp);
            Assert.AreEqual(cp.ContactPersonID, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBOLookupListWithString()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOStringLookup();
            ContactPerson.LoadDefaultClassDef();
            ContactPerson cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "abc");
            Assert.AreEqual(cp.ID.ToString(), bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBOLookupListNull()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOStringLookup();
            ContactPerson.LoadDefaultClassDef();
            ContactPerson cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", null);
            Assert.AreEqual(null, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual(null, bo.GetPropertyValueToDisplay("TestProp2"));
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
            bo.Save();
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
            MockRepository mock = new MockRepository();
            IDatabaseConnection itsConnection = mock.DynamicMock<IDatabaseConnection>();
            Expect.Call(itsConnection.GetConnection())
                .Return(DatabaseConnection.CurrentConnection.GetConnection())
                .Repeat.Times(2);
            Expect.Call(itsConnection.ExecuteSql(null, null))
                .IgnoreArguments()
                .Return(1)
                .Repeat.Times(1);
            mock.ReplayAll();

            BeforeSaveBo bo = (BeforeSaveBo)classDef.CreateNewBusinessObject(itsConnection);
            bo.FirstPart = "foo";
            bo.SecondPart = "bar";
            Assert.AreEqual("", bo.CombinedParts);
            bo.Save();
            Assert.AreEqual("foobar", bo.CombinedParts);
            mock.VerifyAll();
        }



    }
}