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
    public class TestSelectStatementGenerator : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void TestLimitClauseAfter()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            SelectStatementGenerator statementGen = new SelectStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            Assert.AreEqual("SELECT `MyBO`.`MyBoID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` FROM `MyBO`", statementGen.Generate(10),
                            "A limit clause should not be appended to the basic select as it has to appear after the search criteria and should thus be added later.");
        }

        [Test]
        public void TestLimitClauseBefore()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            SelectStatementGenerator statementGen = new SelectStatementGenerator(bo, new MyDatabaseConnection());
            Assert.AreEqual("SELECT TOP 10 MyBO.MyBoID, MyBO.TestProp, MyBO.TestProp2 FROM MyBO", statementGen.Generate(10),
                            "A limit clause should be prepended to the basic select.");
        }

        [Test]
        public void TestDelimitedTableNameWithSpaces()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            ClassDef.ClassDefs[typeof (MyBO)].TableName = "My BO";
            SelectStatementGenerator statementGen = new SelectStatementGenerator(bo, new MyDatabaseConnection(true));
            Assert.AreEqual("SELECT [My BO].[MyBoID], [My BO].[TestProp], [My BO].[TestProp2] FROM [My BO]", statementGen.Generate(0));
        }

        [Test]
        public void TestInsertStatementExcludesNonPersistableProps()
        {
            string newPropName = "NewProp";
            MockBO bo = StatementGeneratorTestHelper.CreateMockBOWithExtraNonPersistableProp(newPropName);

            SelectStatementGenerator gen = new SelectStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            string statement = gen.Generate(0);
            Assert.IsFalse(statement.ToString().Contains(newPropName));
        }

        public class MyDatabaseConnection : DatabaseConnection
        {
            private bool _useStandardDelimiters = false;

            public MyDatabaseConnection() : this("test", "test") {}

            public MyDatabaseConnection(string assemblyName, string className)
                : base(assemblyName, className) {}

            public MyDatabaseConnection(bool useStandardDelimiters) : this()
            {
                _useStandardDelimiters = useStandardDelimiters;
            }

            public override string GetLimitClauseForBeginning(int limit)
            {
                return "TOP " + limit;
            }

            public override string LeftFieldDelimiter
            {
                get
                {
                    if (_useStandardDelimiters) return base.LeftFieldDelimiter;
                    else return "";
                }
            }

            public override string RightFieldDelimiter
            {
                get
                {
                    if (_useStandardDelimiters) return base.RightFieldDelimiter;
                    else return "";
                }
            }
        }
    }
}