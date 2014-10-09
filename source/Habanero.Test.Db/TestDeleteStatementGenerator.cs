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
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB.SqlGeneration
{
    [TestFixture]
    public class TestDeleteStatementGenerator: TestUsingDatabase
    {

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void TestDelimitedTableNameWithSpaces()
        {
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();
            TestAutoInc bo = new TestAutoInc();
            ClassDef.ClassDefs[typeof (TestAutoInc)].TableName = "test autoinc";

            DeleteStatementGenerator gen = new DeleteStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            var statementCol = gen.Generate();
            ISqlStatement statement = statementCol.First();
            var tableName = DatabaseConnection.CurrentConnection.SqlFormatter.DelimitTable("test autoinc");
            StringAssert.Contains(tableName, statement.Statement.ToString());
        }
    }
}