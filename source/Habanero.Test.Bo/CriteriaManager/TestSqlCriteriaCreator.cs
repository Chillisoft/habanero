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

using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using Habanero.Test.BO.SqlGeneration;
using NUnit.Framework;

namespace Habanero.Test.BO
{

    [TestFixture]
    public class TestSqlCriteriaCreator : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void Initialise()
        {
            SetupDBConnection();

            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
        }

        [Test]
        public void TestDelimiters()
        {
            SqlCriteriaCreator creator = new SqlCriteriaCreator(new Parameter("TestProp = 1"), new MyBO());
            SqlStatement sql = new SqlStatement(DatabaseConnection.CurrentConnection);
            creator.AppendCriteriaToStatement(sql);
            Assert.AreEqual("MyBO.TestProp = ?Param0", sql.Statement.ToString());

            creator = new SqlCriteriaCreator(new Parameter("TestProp = 1"), new MyBO().ClassDef);
            sql = new SqlStatement(DatabaseConnection.CurrentConnection);
            creator.AppendCriteriaToStatement(sql);
            Assert.AreEqual("MyBO.TestProp = ?Param0", sql.Statement.ToString());

            creator = new SqlCriteriaCreator(
                new Parameter("TestProp = 1"),
                new MyBO().ClassDef,
                new TestSelectStatementGenerator.MyDatabaseConnection(true));
            sql = new SqlStatement(DatabaseConnection.CurrentConnection);
            creator.AppendCriteriaToStatement(sql);
            Assert.AreEqual("[MyBO].[TestProp] = ?Param0", sql.Statement.ToString());
        }
    }

}
