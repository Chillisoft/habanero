//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBusinessObjectCollection.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectCollection : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
        }

        [Test]
        public void TestInstantiate()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            Assert.AreSame(ClassDef.ClassDefs[typeof(MyBO)], col.ClassDef);
        }

        [Test]
        public void TestFindByGuid()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo1 = new MyBO();
            col.Add(bo1);
            col.Add(new MyBO());
            Assert.AreSame(bo1, col.FindByGuid(bo1.MyBoID));
        }

        [Test]
        public void TestCreateLoadSqlStatement_LimitClauseAtEnd()
        {
            MyBO bo1 = new MyBO();
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(bo1, ClassDef.ClassDefs[typeof (MyBO)], null, 10, null);
            Assert.AreEqual("SELECT `MyBO`.`MyBoID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` FROM `MyBO` limit 10", statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_LimitClauseAtBeginning()
        {
            MyBO bo1 = new MyBO();
            bo1.SetDatabaseConnection(new MyDatabaseConnection());
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(bo1, ClassDef.ClassDefs[typeof(MyBO)], null, 10, null);
            Assert.AreEqual("SELECT TOP 10 MyBO.MyBoID, MyBO.TestProp, MyBO.TestProp2 FROM MyBO", statement.Statement.ToString());
        }

        [Test, Ignore("This needs to be fixed some time")]
        public void TestCreateLoadSqlStatement_BlankCriteriaPropertyValue()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo1 = new MyBO();
            string criteria = "TestProp = ''";
            IExpression expression = Expression.CreateExpression(criteria);
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null);
            Assert.AreEqual(@"SELECT `MyBO`.`MyBoID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM `MyBO` " +
                "WHERE (`MyBO`.`TestProp` = ?Param0)",
                statement.Statement.ToString());
        }


        #region Test Related Object Properties in Criteria

        [Test, ExpectedException(typeof(SqlStatementException),
          ExpectedMessage = "The relationship 'MyUnknownRelationship' of the class 'MyBO'" +
                            " referred to in the Business Object Collection load criteria in the parameter " +
                            "'MyUnknownRelationship.MyRelatedTestProp' does not exist.")]
        public void TestCreateLoadSqlStatement_RelatedObjectProperties_RelationshipDoesntExist()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyBO bo1 = new MyBO();
            string criteria = "TestProp = 'Test' and MyUnknownRelationship.MyRelatedTestProp = 'TestValue'";
            IExpression expression = Expression.CreateExpression(criteria);
            BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null);
        }

        [Test, ExpectedException(typeof(SqlStatementException),
          ExpectedMessage = "The relationship 'MyRelationship' of the class 'MyBO' " +
            "referred to in the Business Object Collection load criteria in the parameter " +
            "'MyRelationship.MyRelatedTestProp' refers to the class 'MyRelatedBo' "+
            "from the assembly 'Habanero.Test'. This related class is not found in the loaded class definitions.")]
        public void TestCreateLoadSqlStatement_RelatedObjectProperties_RelationshipClassHasNoDef()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyBO bo1 = new MyBO();
            string criteria = "TestProp = 'Test' and MyRelationship.MyRelatedTestProp = 'TestValue'";
            IExpression expression = Expression.CreateExpression(criteria);
            BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null);
        }

        [Test]
        public void TestCreateLoadSqlStatement_RelatedObjectProperties()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = new MyBO();
            string criteria = "TestProp = 'Test' and MyRelationship.MyRelatedTestProp = 'TestValue'";
            IExpression expression = Expression.CreateExpression(criteria);
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null);
            Assert.AreEqual(@"SELECT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM (`MyBO`) INNER JOIN `MyRelatedBo` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID` " +
                "WHERE (`MyBO`.`TestProp` = ?Param0 AND `MyBOMyRelationship`.`MyRelatedTestProp` = ?Param1)",
                statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_RelatedSingleInheritedObject()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDefWithSingleTableInheritance();
            MyBO bo1 = new MyBO();
            string criteria = "TestProp = 'Test' and MyRelationship.MyRelatedTestProp = 'TestValue'";
            IExpression expression = Expression.CreateExpression(criteria);
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null);
            Assert.AreEqual(@"SELECT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM (`MyBO`) INNER JOIN `MyBO` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID` " +
                "WHERE (`MyBO`.`TestProp` = ?Param0 AND `MyBOMyRelationship`.`MyRelatedTestProp` = ?Param1)",
                statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_RelatedSingleInheritedObjectProperty()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDefWithSingleTableInheritance();
            MyBO bo1 = new MyBO();
            string criteria = "TestProp = 'Test' and MyRelationship.TestProp = 'TestValue'";
            IExpression expression = Expression.CreateExpression(criteria);
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null);
            Assert.AreEqual(@"SELECT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM (`MyBO`) INNER JOIN `MyBO` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID` " +
                "WHERE (`MyBO`.`TestProp` = ?Param0 AND `MyBOMyRelationship`.`TestProp` = ?Param1)",
                statement.Statement.ToString());
        }

        #endregion //Test Related Object Properties in Criteria

        [Test]
        public void TestRestoreAll()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contact1 = new ContactPersonTestBO();
            contact1.Surname = "Soap";
            ContactPersonTestBO contact2 = new ContactPersonTestBO();
            contact2.Surname = "Hope";
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Add(contact1);
            col.Add(contact2);
            col.SaveAll();

            Assert.AreEqual("Soap", col[0].Surname);
            Assert.AreEqual("Hope", col[1].Surname);

            contact1.Surname = "Cope";
            contact2.Surname = "Pope";
            Assert.AreEqual("Cope", col[0].Surname);
            Assert.AreEqual("Pope", col[1].Surname);

            col.RestoreAll();
            Assert.AreEqual("Soap", col[0].Surname);
            Assert.AreEqual("Hope", col[1].Surname);

            contact1.Delete();
            contact2.Delete();
            col.SaveAll();
            Assert.AreEqual(0, col.Count);
        }

        [Test]
        public void TestCreateBusinessObject()
        {
            ContactPersonTestBO.LoadDefaultClassDef(); 
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            Assert.IsTrue(newCP.State.IsNew);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }


        [Test]
        public void TestPersistOfCreatedBusinessObject()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();
           
            newCP.Save();
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }


        public class MyDatabaseConnection : DatabaseConnection
        {
            public MyDatabaseConnection() : base("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection") { }

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