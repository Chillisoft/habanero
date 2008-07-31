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
using Habanero.BO.ClassDefinition;
using Habanero.BO.SqlGeneration;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.SqlGeneration
{
    [TestFixture]
    public class TestUpdateStatementGenerator: TestUsingDatabase
    {

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        //// TODO: this test awaits the addition of delimiters to MySQL
        //[Test]
        //public void TestDelimitedTableNameWithSpaces()
        //{
        //    ClassDef.ClassDefs.Clear();
        //    TestAutoInc.LoadClassDefWithAutoIncrementingID();
        //    TestAutoInc bo = new TestAutoInc();
        //    ClassDef.ClassDefs[typeof(TestAutoInc)].TableName = "test autoinc";

        //    UpdateStatementGenerator gen = new UpdateStatementGenerator(bo, DatabaseConnection.CurrentConnection);
        //    ISqlStatementCollection statementCol = gen.Generate();
        //    UpdateSqlStatement statement = (UpdateSqlStatement)statementCol[0];
        //    Assert.AreEqual("PUT STUFF HERE", statement.Statement.ToString());
        //}

        [Test]
        public void TestUpdateStatementExcludesNonPersistableProps()
        {
            ClassDef.ClassDefs.Clear();
            string newPropName = "NewProp";
            MockBO bo = StatementGeneratorTestHelper.CreateMockBOWithExtraNonPersistableProp(newPropName);
            bo.SetPropertyValue(newPropName, "newvalue323");
            bo.SetPropertyValue("MockBOProp1", "dfggjh");

            UpdateStatementGenerator gen = new UpdateStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            ISqlStatementCollection statementCol = gen.Generate();
            ISqlStatement statement = statementCol[0];
            Assert.IsFalse(statement.Statement.ToString().Contains(newPropName));
        }


    }
}
