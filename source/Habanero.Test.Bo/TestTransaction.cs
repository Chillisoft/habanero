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
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestTransaction : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SetupDBConnection();

            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        [Test]
        public void TestTransactionSuccess()
        {
            ContactPersonTestBO myContact_1 = new ContactPersonTestBO();
            //Edit first object and save
            myContact_1.Surname = "My Surname 1";
            //myContact_1.SetPropertyValue("PK3Prop", null); // set the previous value to null
            Transaction transact = new Transaction(DatabaseConnection.CurrentConnection);
            transact.AddTransactionObject(myContact_1);

            ContactPersonTestBO myContact_2 = new ContactPersonTestBO();
            myContact_2.Surname = "My Surname 2";

            Assert.IsTrue(myContact_2.IsValid());

            transact.AddTransactionObject(myContact_2);

            transact.CommitTransaction();

            Assert.IsFalse(myContact_2.State.IsDirty);
            Assert.IsFalse(myContact_1.State.IsNew);
            Assert.IsFalse(myContact_1.State.IsDirty);
            Assert.IsFalse(myContact_2.State.IsNew);
            Assert.IsTrue(myContact_2.IsValid());

            //Ensure object loaded from DB.
            BusinessObject.ClearLoadedBusinessObjectBaseCol();

            ContactPersonTestBO myContact_3 = ContactPersonTestBO.GetContactPerson(myContact_1.ID);

            Assert.AreEqual(myContact_1.ID, myContact_3.ID);
            Assert.AreEqual(myContact_1.Surname, myContact_3.Surname);

            ContactPersonTestBO myContact_4 = ContactPersonTestBO.GetContactPerson(myContact_2.ID);

            Assert.AreEqual(myContact_2.ID, myContact_4.ID);
            Assert.AreEqual(myContact_2.Surname, myContact_4.Surname);
        }

        [Test]
        public void TestTransactionFail()
        {
            ContactPersonTestBO myContact_1 = new ContactPersonTestBO();
            //Edit first object and save
            myContact_1.Surname = "My Surname 1";
            //myContact_1.SetPropertyValue("PK3Prop", null); // set the previous value to null
            Transaction transact = new Transaction(DatabaseConnection.CurrentConnection);
            transact.AddTransactionObject(myContact_1);

            ContactPersonTestBO myContact_2 = new ContactPersonTestBO();
            myContact_2.Surname = "My Surname 1"; //Should result in a duplicate error when try to persist
            //will result in the commit failing
            Assert.IsTrue(myContact_2.IsValid());
            transact.AddTransactionObject(myContact_2);
            bool errorRaised = false;
            try
            {
                transact.CommitTransaction();
            }
            catch (Exception ex) //todo:check type of error?
            {
                errorRaised = true;
            }

            Assert.IsTrue(errorRaised, "Error should have been raised");

            Assert.IsTrue(myContact_2.State.IsDirty);
            Assert.IsTrue(myContact_1.State.IsNew);
            Assert.IsTrue(myContact_1.State.IsDirty);
            Assert.IsTrue(myContact_2.State.IsNew);
            Assert.IsTrue(myContact_2.IsValid());
            Assert.IsTrue(myContact_2.IsValid());


            //Ensure object loaded from DB.
            BusinessObject.ClearLoadedBusinessObjectBaseCol();
            errorRaised = false;
            try
            {
                ContactPersonTestBO myContact_3 = ContactPersonTestBO.GetContactPerson(myContact_1.ID);
            }
            //Expect this error since the object should not have been persisted to the DB.
            catch (BusinessObjectNotFoundException ex)
            {
                errorRaised = true;
            }
            Assert.IsTrue(errorRaised);


            //Test canceledits to transaction
            transact.CancelEdits();
            Assert.IsTrue(String.IsNullOrEmpty(myContact_1.Surname));
            Assert.IsTrue(String.IsNullOrEmpty(myContact_2.Surname));
            Assert.IsFalse(myContact_2.IsValid());
            Assert.IsFalse(myContact_2.IsValid());
        }

        [Test]
        public void TestOrderOfTransactions()
        {
            
            Transaction tran = new Transaction(DatabaseConnection.CurrentConnection);
            List<ContactPersonTestBO> contactPersons = new List<ContactPersonTestBO>();
            for (int i=0; i<100; i++) {
                ContactPersonTestBO newContactPersonTestBOTestBO = new ContactPersonTestBO();
                newContactPersonTestBOTestBO.SetDatabaseConnection(DatabaseConnection.CurrentConnection);
                contactPersons.Add(newContactPersonTestBOTestBO);
                tran.AddTransactionObject(newContactPersonTestBOTestBO);
            }
            SqlStatementCollection statements = tran.GetPersistSql();
            Assert.AreEqual(100, statements.Count );

            for (int i = 0; i < 100; i++) {
                IDbDataParameter parameter = (IDbDataParameter) statements[i].Parameters[0];
                string id = parameter.Value.ToString();
                Assert.AreEqual(contactPersons[i].ContactPersonID.ToString("B").ToUpper(), id.ToUpper());
            }
        }

        [Test, Ignore]
        public void TestOrderOfCascadeDelete()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            Address address;
            ContactPersonTestBO bo = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            bo.Delete();

            //---------------Execute Test ----------------------
            Transaction t = new Transaction();
            t.AddTransactionObject(bo);
            ITransaction iTransaction = t;
            iTransaction.BeforeCommit(null);
            ISqlStatementCollection statements = t.GetPersistSql();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, statements.Count);
            string expectedDeleteAddress = "DELETE FROM `contact_person_address`";
            string expectedDeleteContactPerson = "DELETE FROM `contact_person`";
            Assert.AreEqual(expectedDeleteAddress, statements[0].Statement.ToString().Substring(0, expectedDeleteAddress.Length));
            Assert.AreEqual(expectedDeleteContactPerson, statements[1].Statement.ToString().Substring(0, expectedDeleteContactPerson.Length));
        }

        [Test, Ignore]
        public void TestDelete3LevelsDeep()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            OrganisationTestBO.LoadDefaultClassDef();

            OrganisationTestBO org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();


            //---------------Execute Test ----------------------

            org.Delete();

            Transaction t = new Transaction();
            t.AddTransactionObject(org);
            ITransaction iTransaction = t;
            iTransaction.BeforeCommit(null);
            ISqlStatementCollection statements = t.GetPersistSql();

            //---------------Test Result -----------------------
            Assert.AreEqual(3, statements.Count);
            string expectedDeleteAddress = "DELETE FROM `contact_person_address`";
            string expectedDeleteContactPerson = "DELETE FROM `contact_person`";
            string expectedDeleteOrganisation = "DELETE FROM `organisation`";
            Assert.AreEqual(expectedDeleteAddress, statements[0].Statement.ToString().Substring(0, expectedDeleteAddress.Length));
            Assert.AreEqual(expectedDeleteContactPerson, statements[1].Statement.ToString().Substring(0, expectedDeleteContactPerson.Length));
            Assert.AreEqual(expectedDeleteOrganisation, statements[2].Statement.ToString().Substring(0, expectedDeleteOrganisation.Length));
        }


    }
}
