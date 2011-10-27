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
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDatabaseConnectionFirebird
    {
        [Test]
        public void TestCreateParameterNameGenerator()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IParameterNameGenerator generator = databaseConnection.CreateParameterNameGenerator();
            //---------------Test Result -----------------------
            Assert.AreEqual("@", generator.PrefixCharacter);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSqlFormatter()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = databaseConnection.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
            SqlFormatter formatter = (SqlFormatter)defaultSqlFormatter;
            Assert.AreEqual("", formatter.LeftFieldDelimiter);
            Assert.AreEqual("", formatter.RightFieldDelimiter);
            Assert.AreEqual("FIRST", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("", formatter.LimitClauseAtEnd);

        }


        [Test]
        public void Test_CreateCustomConnectionString()
        {
            //---------------Set up test pack-------------------
            string connectString = "";
            DatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "", connectString);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string setConnectionString = databaseConnection.ConnectionString;

            //---------------Test Result -----------------------
            Assert.AreEqual(setConnectionString, connectString);
        }

    }
}