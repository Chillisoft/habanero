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
using System.Text;
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
            ContactPerson.LoadDefaultClassDef();
            ContactPerson.DeleteAllContactPeople();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            ContactPerson.DeleteAllContactPeople();
        }

        [Test]
        public void TestTransactionSuccess()
        {
            ContactPerson myContact_1 = new ContactPerson();
            //Edit first object and save
            myContact_1.Surname = "My Surname 1";
            //myContact_1.SetPropertyValue("PK3Prop", null); // set the previous value to null
            Transaction transact = new Transaction(DatabaseConnection.CurrentConnection);
            transact.AddTransactionObject(myContact_1);

            ContactPerson myContact_2 = new ContactPerson();
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

            ContactPerson myContact_3 = ContactPerson.GetContactPerson(myContact_1.ID);

            Assert.AreEqual(myContact_1.ID, myContact_3.ID);
            Assert.AreEqual(myContact_1.Surname, myContact_3.Surname);

            ContactPerson myContact_4 = ContactPerson.GetContactPerson(myContact_2.ID);

            Assert.AreEqual(myContact_2.ID, myContact_4.ID);
            Assert.AreEqual(myContact_2.Surname, myContact_4.Surname);
        }

        [Test]
        public void TestTransactionFail()
        {
            ContactPerson myContact_1 = new ContactPerson();
            //Edit first object and save
            myContact_1.Surname = "My Surname 1";
            //myContact_1.SetPropertyValue("PK3Prop", null); // set the previous value to null
            Transaction transact = new Transaction(DatabaseConnection.CurrentConnection);
            transact.AddTransactionObject(myContact_1);

            ContactPerson myContact_2 = new ContactPerson();
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
                ContactPerson myContact_3 = ContactPerson.GetContactPerson(myContact_1.ID);
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
            List<ContactPerson> contactPersons = new List<ContactPerson>();
            for (int i=0; i<100; i++) {
                ContactPerson newContactPerson = new ContactPerson();
                newContactPerson.SetDatabaseConnection(DatabaseConnection.CurrentConnection);
                contactPersons.Add(newContactPerson);
                tran.AddTransactionObject(newContactPerson);
            }
            SqlStatementCollection statements = tran.GetPersistSql();
            Assert.AreEqual(100, statements.Count );

            for (int i = 0; i < 100; i++) {
                IDbDataParameter parameter = (IDbDataParameter) statements[i].Parameters[0];
                string id = parameter.Value.ToString();
                Assert.AreEqual(contactPersons[i].ContactPersonID.ToString("B").ToUpper(), id.ToUpper());
            }
        }


    }
}
