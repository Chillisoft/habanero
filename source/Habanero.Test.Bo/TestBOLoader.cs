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
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using NUnit.Framework;

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
        public void TestGetBusinessObjectDuplicatesException()
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

        public void TestBoLoaderCallsAfterLoad_CompositePrimaryKey()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO cpTemp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(cpTemp.PrimaryKey);
            Assert.IsTrue(cp.AfterLoadCalled);
        }
    }
}

