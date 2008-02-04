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
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
using NMock;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
    // TODO: - test Guid methods with Sql Server and other Guid formats
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
            ContactPerson.CreateSampleData();
        }

        [TearDown]
        public void TearDown()
        {
            ContactPerson.DeleteAllContactPeople();
        }

        [Test]
        public void TestGetBusinessObject()
        {
            ClassDef.ClassDefs.Clear();
            ContactPerson.LoadDefaultClassDef();

            ContactPerson cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            Assert.AreEqual("abc", cp.Surname);

            cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc AND FirstName = aa");
            Assert.AreEqual("abc", cp.Surname);

            cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abcde");
            Assert.IsNull(cp);

            cp = (ContactPerson)BOLoader.Instance.GetBusinessObject(new ContactPerson(), new Parameter("Surname = invalid"));
            Assert.IsNull(cp);
        }

        [Test, ExpectedException(typeof(UserException))]
        public void TestGetBusinessObjectDuplicatesException()
        {
            ClassDef.ClassDefs.Clear();
            ContactPerson.LoadDefaultClassDef();

            ContactPerson cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("FirstName = aa");
        }

        [Test]
        public void TestGetBusinessObjectCollection()
        {
            ClassDef.ClassDefs.Clear();
            ContactPerson.LoadDefaultClassDef();

            BusinessObjectCollection<ContactPerson> col = BOLoader.Instance.GetBusinessObjectCol<ContactPerson>("FirstName = aa", "Surname");
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual("abc", col[0].Surname);
            Assert.AreEqual("abcd", col[1].Surname);

            BusinessObjectCollection<BusinessObject> col2 = BOLoader.Instance.GetBusinessObjectCol(typeof(ContactPerson), "FirstName = aa", "Surname");
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual("abc", ((ContactPerson)col2[0]).Surname);
            Assert.AreEqual("abcd", ((ContactPerson)col2[1]).Surname);

            col = BOLoader.Instance.GetBusinessObjectCol<ContactPerson>("FirstName = aaaa", "Surname");
            Assert.AreEqual(0, col.Count);
        }

        // Also need to test this works with Sql Server and other Guid forms
        [Test]
        public void TestGetBusinessObjectByIDGuid()
        {
            ClassDef.ClassDefs.Clear();
            ContactPerson.LoadDefaultClassDef();

            ContactPerson cp1 = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            ContactPerson cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(cp1.ContactPersonID);
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
            ContactPerson.LoadClassDefWithSurnameAsPrimaryKey();

            ContactPerson cp1 = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            ContactPerson cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(cp1.Surname);
            Assert.AreEqual(cp1, cp2);

            cp1.Surname = "a b";
            cp1.Save();
            cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>("a b");
            Assert.AreEqual(cp1, cp2);
            cp1.Surname = "abc";
            cp1.Save();
        }

        [Test]
        public void TestGetBusinessObjectByIDPrimaryKey()
        {
            ClassDef.ClassDefs.Clear();
            ContactPerson.LoadClassDefWithCompositePrimaryKey();
            
            ContactPerson cp1 = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            ContactPerson cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(cp1.PrimaryKey);
            Assert.AreEqual(cp1, cp2);
        }

        [Test, ExpectedException(typeof(InvalidPropertyException))]
        public void TestGetBusinessObjectByIDWithMultiplePrimaryKeys()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonCompositeKey.LoadClassDefs();

            ContactPersonCompositeKey cp2 =
                BOLoader.Instance.GetBusinessObjectByID<ContactPersonCompositeKey>("someIDvalue");
        }

        [Test]
        public void TestLoadMethod()
        {
            ClassDef.ClassDefs.Clear();
            ContactPerson.LoadDefaultClassDef();

            ContactPerson cp1 = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            cp1.Surname = "def";
            BOLoader.Instance.Load(cp1, new Parameter("Surname = abc"));
            Assert.AreEqual("abc", cp1.Surname);
        }

        [Test]
        public void TestSetDatabaseConnection()
        {
            ClassDef.ClassDefs.Clear();
            ContactPerson.LoadDefaultClassDef();

            ContactPerson cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            Assert.IsNotNull(cp.GetDatabaseConnection());
            BOLoader.Instance.SetDatabaseConnection(cp, null);
            Assert.IsNull(cp.GetDatabaseConnection());
        }
    }
}

