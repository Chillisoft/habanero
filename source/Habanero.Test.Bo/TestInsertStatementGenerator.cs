using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.SqlGeneration;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestInsertStatementGenerator: TestUsingDatabase
    {

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void TestSqlStatementType()
        {
            MockBO bo = new MockBO();
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection.GetConnection());
            ISqlStatementCollection statementCol = gen.Generate();
            Assert.AreEqual(1, statementCol.Count);
            Assert.AreSame(typeof(InsertSqlStatement), statementCol[0].GetType());
            
        }

        [Test]
        public void TestSqlStatementTableName()
        {
            MockBO bo = new MockBO();
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection.GetConnection());
            ISqlStatementCollection statementCol = gen.Generate();
            InsertSqlStatement statement = (InsertSqlStatement)statementCol[0];
            Assert.AreEqual("MockBO", statement.TableName);
        }

        [Test]
        public void TestAutoIncrementObjNotApplicable()
        {
            MockBO bo = new MockBO();
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection.GetConnection());
            ISqlStatementCollection statementCol = gen.Generate();
            InsertSqlStatement statement = (InsertSqlStatement)statementCol[0];
            Assert.AreEqual(null, statement.SupportsAutoIncrementingField);
        }

        [Test]
        public void TestAutoIncrementObj()
        {
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();
            TestAutoInc bo = new TestAutoInc();
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection.GetConnection());
            ISqlStatementCollection statementCol = gen.Generate();
            InsertSqlStatement statement = (InsertSqlStatement)statementCol[0];
            Assert.AreSame(typeof(SupportsAutoIncrementingFieldBO), statement.SupportsAutoIncrementingField.GetType());
        }

        [Test]
        public void TestInsertStatementExcludesAutoField()
        {
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();
            TestAutoInc bo = new TestAutoInc();
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection.GetConnection());
            ISqlStatementCollection statementCol = gen.Generate();
            InsertSqlStatement statement = (InsertSqlStatement)statementCol[0];

            Assert.AreEqual("INSERT INTO testautoinc (testfield) VALUES (?Param0)", statement.Statement.ToString());
        }

    }
}
