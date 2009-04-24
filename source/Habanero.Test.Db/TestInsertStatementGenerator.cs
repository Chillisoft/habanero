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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.SqlGeneration
{
    [TestFixture]
    public class TestInsertStatementGenerator: TestUsingDatabase
    { 

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.SetupDBConnection();
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }


        [Test]
        public void TestSqlStatementType()
        {
            MockBO bo = new MockBO();
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            ISqlStatementCollection statementCol = gen.Generate();
            Assert.AreEqual(1, statementCol.Count);
            Assert.AreSame(typeof(InsertSqlStatement), statementCol[0].GetType());
            
        }

        [Test]
        public void TestSqlStatementTableName()
        {
            MockBO bo = new MockBO();
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            ISqlStatementCollection statementCol = gen.Generate();
            InsertSqlStatement statement = (InsertSqlStatement)statementCol[0];
            Assert.AreEqual("MockBO", statement.TableName);
        }

        [Test]
        public void TestAutoIncrementObjNotApplicable()
        {
            MockBO bo = new MockBO();
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection);
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
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection);
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
            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            ISqlStatementCollection statementCol = gen.Generate();
            InsertSqlStatement statement = (InsertSqlStatement)statementCol[0];

            Assert.AreEqual("INSERT INTO `testautoinc` (`testfield`) VALUES (?Param0)", statement.Statement.ToString());
        }

        [Test]
        public void TestInsertStatementExcludesNonPersistableProps()
        {
            ClassDef.ClassDefs.Clear();
            string newPropName = "NewProp";
            MockBO bo = StatementGeneratorTestHelper.CreateMockBOWithExtraNonPersistableProp(newPropName);

            InsertStatementGenerator gen = new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            ISqlStatementCollection statementCol = gen.Generate();
            InsertSqlStatement statement = (InsertSqlStatement)statementCol[0];
            Assert.IsFalse(statement.Statement.ToString().Contains(newPropName));
        }


        
        [Test]
        public void TestSingleTableInheritanceHierarchy_DifferentDiscriminators()
        {
            //---------------Set up test pack-------------------
            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchyDifferentDiscriminators();
            FilledCircleNoPrimaryKey filledCircle = new FilledCircleNoPrimaryKey();
            InsertStatementGenerator gen = new InsertStatementGenerator(filledCircle, DatabaseConnection.CurrentConnection);


            //---------------Execute Test ----------------------
            ISqlStatementCollection sqlStatementCollection = gen.Generate();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, sqlStatementCollection.Count);
            ISqlStatement sqlStatement = sqlStatementCollection[0];
            string sql = sqlStatement.Statement.ToString();
            StringAssert.Contains("ShapeType", sql);
            StringAssert.Contains("CircleType", sql);
        }

        [Test]
        public void TestSingleTableInheritanceHierarchy_SharedDiscriminators()
        {
            //---------------Set up test pack-------------------
            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
            FilledCircleNoPrimaryKey filledCircle = new FilledCircleNoPrimaryKey();
            InsertStatementGenerator gen = new InsertStatementGenerator(filledCircle, DatabaseConnection.CurrentConnection);
            
            //---------------Execute Test ----------------------
            ISqlStatementCollection sqlStatementCollection = gen.Generate();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, sqlStatementCollection.Count);
            ISqlStatement sqlStatement = sqlStatementCollection[0];
            string sql = sqlStatement.Statement.ToString();
            int index = sql.IndexOf("ShapeType");
            Assert.IsTrue(index > 0);
            index = sql.IndexOf("ShapeType", index + 1);
            Assert.IsTrue(index < 0, "There were two ShapeType fields specified");
            Assert.AreEqual("FilledCircleNoPrimaryKey", sqlStatement.Parameters[0].Value);
        }

        //[Test]
        //public void TestIdAttributeWithMultiplePrimaryKey()
        //{
        //    ClassDef.ClassDefs.Clear();
        //    ContactPersonCompositeKeyInheritor bo = new ContactPersonCompositeKeyInheritor();
        //    InsertStatementGenerator gen =
        //        new InsertStatementGenerator(bo, DatabaseConnection.CurrentConnection.GetConnection());
        //    ISqlStatementCollection statementCol = gen.Generate();
        //}

        //private class ContactPersonCompositeKeyInheritor : ContactPersonCompositeKey
        //{
        //    protected override ClassDef ConstructClassDef()
        //    {
        //        return GetClassDef();
        //    }

        //    private static ClassDef GetClassDef()
        //    {
        //        if (!ClassDef.IsDefined(typeof(ContactPersonCompositeKey)))
        //        {
        //            return CreateClassDefWin();
        //        }
        //        else
        //        {
        //            return ClassDef.ClassDefs[typeof(ContactPersonCompositeKey)];
        //        }
        //    }

        //    private static ClassDef CreateClassDef()
        //    {
        //        PropDefCol lPropDefCol = CreateBOPropDef();
        //        SuperClassDef superClassDef = new SuperClassDef("Habanero.Test", "ContactPersonCompositeKey",
        //            ORMapping.ClassTableInheritance, "PK1Prop1", null);

        //        KeyDefCol keysCol = new KeyDefCol();
        //        PrimaryKeyDef primaryKey = new PrimaryKeyDef();
        //        primaryKey.IsGuidObjectID = false;
        //        primaryKey.Add(lPropDefCol["PK1Prop1"]);
        //        primaryKey.Add(lPropDefCol["PK1Prop2"]);

        //        RelationshipDefCol relDefs = CreateRelationshipDefCol(lPropDefCol);
        //        ClassDef lClassDef =
        //            new ClassDef(typeof(ContactPersonCompositeKey), primaryKey, lPropDefCol, keysCol, relDefs);
        //        lClassDef.HasObjectID = false;
        //        lClassDef.SuperClassDef = superClassDef;
        //        ClassDef.ClassDefs.Add(lClassDef);
        //        return lClassDef;
        //    }
        //}
    }
}
