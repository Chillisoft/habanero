//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.SqlGeneration;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.SqlGeneration
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
        //            return CreateClassDef();
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
        //        primaryKey.IsObjectID = false;
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
