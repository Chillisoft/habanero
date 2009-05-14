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
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestSqlStatementList : TestUsingDatabase
    {
        private SqlStatementCollection testCollection;
        private SqlStatement testStatement1;
        private SqlStatement testStatement2;
        private SqlStatementCollection testCollectionWithOneStatement;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            try
            {
                this.SetupDBConnection();
                IDatabaseConnection connection = DatabaseConnection.CurrentConnection;
                testCollection = new SqlStatementCollection();
                testStatement1 = new SqlStatement(connection);
                testStatement2 = new SqlStatement(connection);
                testCollectionWithOneStatement = new SqlStatementCollection(testStatement1);
                testCollection.Add(testStatement1);
                testCollection.Add(testStatement2);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        [Test]
        public void TestCount()
        {
            Assert.AreEqual(2, testCollection.Count, "Count property on SqlStatementList is returning incorrect value.");
        }

        [Test]
        public void TestAddList()
        {
            SqlStatementCollection newCollection = new SqlStatementCollection();
            newCollection.Add(testCollection);
            Assert.AreEqual(2, newCollection.Count, "Adding a list to a SqlStatementList not working properly.");
        }

        [Test]
        public void TestToString()
        {
            Assert.AreEqual(
                testStatement1.ToString() + Environment.NewLine + testStatement2.ToString() + Environment.NewLine,
                testCollection.ToString(), "Tostring of SqlStatementList not working correctly.");
        }

        [Test]
        public void TestConstructorWithOneStatement()
        {
            Assert.AreEqual(1, testCollectionWithOneStatement.Count,
                            "A SqlStatementList created with a SqlStatement should only contain one statement");
            Assert.AreSame(testStatement1, testCollectionWithOneStatement[0]);
        }
    }
}