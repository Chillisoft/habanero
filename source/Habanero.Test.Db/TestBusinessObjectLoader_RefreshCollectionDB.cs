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
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.BO.BusinessObjectLoader;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestBusinessObjectLoader_RefreshCollectionDB :
        TestBusinessObjectLoader_RefreshCollection
    {
        #region Setup/Teardown
        [TestFixtureSetUp]
        public override void TestFixtureSetup()
        {
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
            //Create a new ContactPersonTable with a randomlygenerated guid at end of name.
            
        }

        [TearDown]
        public override void TearDownTest()
        {
            //Drop the newly created ContactPersonTable (see above).
            BOTestUtils.DropNewContactPersonAndAddressTables();
        }

        [SetUp]
        public override void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        protected override void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        protected override void CreateContactPersonTable()
        {
            base.CreateContactPersonTable();
            ContactPersonTestBO.CreateContactPersonTable(GetContactPersonTableName());
        }

        #endregion

        public TestBusinessObjectLoader_RefreshCollectionDB()
        {
            new TestUsingDatabase().SetupDBConnection();
        }

        [Test]
        public void TestRefreshCollectionRefreshesNonDirtyObjects()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorDB();
            ContactPersonTestBO.DeleteAllContactPeople();

            SetupDefaultContactPersonBO();
            var col = new BusinessObjectCollection<ContactPersonTestBO>();

            var cp1 = ContactPersonTestBO.CreateSavedContactPerson();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO.CreateSavedContactPerson();
            col.LoadAll();
            var newSurname = Guid.NewGuid().ToString();
            cp1.Surname = newSurname;
            cp1.Save();
            var secondInstanceOfCP1 = col.Find(cp1.ContactPersonID);

            //--------------------Assert Preconditions----------
            Assert.IsFalse(col.Contains(cp1));
            Assert.AreEqual(3, col.Count);
            Assert.AreEqual(newSurname, cp1.Surname);
            Assert.AreNotSame(secondInstanceOfCP1, cp1);
            Assert.AreNotEqual(newSurname, secondInstanceOfCP1.Surname);
            Assert.IsFalse(cp1.Status.IsDirty);
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreNotSame(secondInstanceOfCP1, cp1);
            Assert.AreEqual(newSurname, secondInstanceOfCP1.Surname);
        }


        [Test]
        public void Test_RefreshCollectionRefreshesNonDirtyObjects()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorDB();
            OrganisationTestBO.DeleteAllOrganisations();
            ContactPersonTestBO.DeleteAllContactPeople();
            SetupDefaultContactPersonBO();
            var col = new BusinessObjectCollection<ContactPersonTestBO>();

            var cp1 = CreateContactPersonTestBO();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();

            CreateContactPersonTestBO();
            CreateContactPersonTestBO();
            col.LoadAll();
            var newSurname = Guid.NewGuid().ToString();
            cp1.Surname = newSurname;
            cp1.Save();
            var secondInstanceOfCP1 = col.Find(cp1.ContactPersonID);

            //--------------------Assert Preconditions----------
            AssertNotContains(cp1, col);
            Assert.AreEqual(newSurname, cp1.Surname);
            Assert.AreNotSame(secondInstanceOfCP1, cp1);
            Assert.AreNotEqual(newSurname, secondInstanceOfCP1.Surname);
            Assert.IsFalse(cp1.Status.IsDirty);
            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreNotSame(secondInstanceOfCP1, cp1);
            Assert.AreEqual(newSurname, secondInstanceOfCP1.Surname);
        }

        private static void AssertNotContains(ContactPersonTestBO cp1, IEnumerable<ContactPersonTestBO> col)
        {
            if(col.Where(bo => ReferenceEquals(bo, cp1)).Count() > 0)
            {
                Assert.Fail("Should not contain object");
            }
        }

        private static ContactPersonTestBO CreateContactPersonTestBO()
        {
            var bo = new ContactPersonTestBO();
            var newSurname = Guid.NewGuid().ToString();
            bo.Surname = newSurname;
            bo.Save();
            return bo;
        }
        [Test, Ignore("Not implemented for DB as parametrized class defs are implemented in a different way (via afterload and updateobjectbeforepersisting)")]
        public override void Test_Refresh_W_ParametrizedClassDef_Typed()
        {

        }

        [Test, Ignore("Not implemented for DB as parametrized class defs are implemented in a different way (via afterload and updateobjectbeforepersisting)")]
        public override void Test_Refresh_W_ParametrizedClassDef_Untyped() { }
    }
}