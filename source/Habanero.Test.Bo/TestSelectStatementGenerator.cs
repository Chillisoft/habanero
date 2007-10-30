using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;
using Habanero.BO.SqlGeneration;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
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
            Assert.AreEqual("SELECT MyBO.MyBoID, MyBO.TestProp, MyBO.TestProp2 FROM MyBO", statementGen.Generate(10),
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

        private class MyDatabaseConnection : DatabaseConnection
        {
            public MyDatabaseConnection() : this("test", "test") {}

            public MyDatabaseConnection(string assemblyName, string className)
                : base(assemblyName, className) {}

            public override string GetLimitClauseForBeginning(int limit)
            {
                return "TOP " + limit;
            }

            public override string LeftFieldDelimiter
            {
                get { return ""; }
            }

            public override string RightFieldDelimiter
            {
                get { return ""; }
            }
        }
    }
}