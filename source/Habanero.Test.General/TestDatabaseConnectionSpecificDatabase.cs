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

using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public class TestDatabaseConnectionSpecificDatabase : TestUsingDatabase
    {
//        [SetUp]
//        public  void SetupTest()
//        {
//            //Runs every time that any testmethod is executed
//            base.SetupTest();
//        }
//
//        [TestFixtureSetUp]
//        public void TestFixtureSetup()
//        {
//            //Code that is executed before any test is run in this class. If multiple tests
//            // are executed then it will still only be called once.
//        }
//
//        [TearDown]
//        public override void TearDownTest()
//        {
//            //runs every time any testmethod is complete
//            base.TearDownTest();
//        }

        [Test]
        public void Test_CreateFirebirdConnection()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "");            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter formatter = databaseConnection.SqlFormatter;

            //---------------Test Result -----------------------
            Assert.AreEqual("", formatter.LeftFieldDelimiter);
            Assert.AreEqual("", formatter.RightFieldDelimiter);
            Assert.AreEqual("FIRST", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("", formatter.LimitClauseAtEnd);

        }

        [Test]
        public void Test_CreateFirebirdConnection_AltConstructor()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "","");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter formatter = databaseConnection.SqlFormatter;

            //---------------Test Result -----------------------
            Assert.AreEqual("", formatter.LeftFieldDelimiter);
            Assert.AreEqual("", formatter.RightFieldDelimiter);
            Assert.AreEqual("FIRST", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("", formatter.LimitClauseAtEnd);

        }

        [Test]
        public void Test_CreateMySqlConnection()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionMySql("", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter formatter = databaseConnection.SqlFormatter;

            //---------------Test Result -----------------------
            Assert.AreEqual("`", formatter.LeftFieldDelimiter);
            Assert.AreEqual("`", formatter.RightFieldDelimiter);
            Assert.AreEqual("", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("LIMIT", formatter.LimitClauseAtEnd);

        }

        [Test]
        public void Test_CreateMySqlConnection_AltConstructor()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionMySql("", "", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter formatter = databaseConnection.SqlFormatter;

            //---------------Test Result -----------------------
            Assert.AreEqual("`", formatter.LeftFieldDelimiter);
            Assert.AreEqual("`", formatter.RightFieldDelimiter);
            Assert.AreEqual("", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("LIMIT", formatter.LimitClauseAtEnd);

        }
    }
}