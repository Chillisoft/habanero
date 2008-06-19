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
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public void TestRefreshCollectionDoesNotRefreshDirtyOject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.DeleteAllContactPeople();
            ContactPersonTestBO.ClearLoadedBusinessObjectBaseCol();
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            
            ContactPersonTestBO cp1 = CreateContactPersonTestBO();
            ContactPersonTestBO cp2 = CreateContactPersonTestBO();
            ContactPersonTestBO cp3 = CreateContactPersonTestBO();
            //--------------------------------------------------

            col.LoadAll();
            string newSurname = Guid.NewGuid().ToString();
            //---------------Execute Test ----------------------
            cp1.Surname = newSurname;
            col.Refresh();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreEqual(newSurname, cp1.Surname);
            Assert.IsTrue(cp1.State.IsDirty);
            //---------------Tear Down -------------------------          
        }

        private static ContactPersonTestBO CreateContactPersonTestBO()
        {
            ContactPersonTestBO bo = new ContactPersonTestBO();
            string newSurname = Guid.NewGuid().ToString();
            bo.Surname = newSurname;
            bo.Save();
            return bo;
        }

        [Test]
        public void TestInstantiate()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            Assert.AreSame(ClassDef.ClassDefs[typeof(MyBO)], col.ClassDef);
        }

        [Test]
        public void TestAddMethod()
        {
            //Setup
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            //Fixture
            col.Add(myBO);
            //Assert
            Assert.AreEqual(1, col.Count, "One object should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
        }

        [Test]
        public void TestAddMethod_WithParamArray()
        {
            //Setup
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            MyBO myBO3 = new MyBO();
            //Fixture
            col.Add(myBO, myBO2, myBO3);
            //Assert
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
            Assert.AreEqual(myBO2, col[1], "Added object should be in the collection");
            Assert.AreEqual(myBO3, col[2], "Added object should be in the collection");
        }

        [Test]
        public void TestAddMethod_WithEnumerable_List()
        {
            //Setup
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            MyBO myBO3 = new MyBO();
            List<MyBO> list = new List<MyBO>();
            list.Add(myBO);
            list.Add(myBO2);
            list.Add(myBO3);
            //Fixture
            col.Add(list);
            //Assert
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
            Assert.AreEqual(myBO2, col[1], "Added object should be in the collection");
            Assert.AreEqual(myBO3, col[2], "Added object should be in the collection");
        }

        [Test]
        public void TestAddMethod_WithEnumerable_Collection()
        {
            //Setup
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            MyBO myBO3 = new MyBO();
            Collection<MyBO> collection = new Collection<MyBO>();
            collection.Add(myBO);
            collection.Add(myBO2);
            collection.Add(myBO3);
            //Fixture
            col.Add(collection);
            //Assert
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
            Assert.AreEqual(myBO2, col[1], "Added object should be in the collection");
            Assert.AreEqual(myBO3, col[2], "Added object should be in the collection");
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
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof (MyBO)], null, 10, null, null);
            Assert.AreEqual("SELECT `MyBO`.`MyBoID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` FROM `MyBO` limit 10", statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_LimitClauseAtBeginning()
        {
            MyBO bo1 = new MyBO();
            bo1.SetDatabaseConnection(new MyDatabaseConnection());
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], null, 10, null, null);
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
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null, null);
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
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null, null);
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
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null, null);
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
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null, null);
            Assert.AreEqual(@"SELECT DISTINCT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM `MyBO` LEFT JOIN `MyRelatedBo` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID` " +
                "WHERE (`MyBO`.`TestProp` = ?Param0 AND `MyBOMyRelationship`.`MyRelatedTestProp` = ?Param1)",
                statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_RelatedObjectPropertiesInOrderBy()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = new MyBO();
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], null, -1, null, "MyRelationship.MyRelatedTestProp");
            //-------------Test Result ----------------------
            Assert.AreEqual(@"SELECT DISTINCT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM `MyBO` LEFT JOIN `MyRelatedBo` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID` " +
                "ORDER BY MyBOMyRelationship.MyRelatedTestProp",
                statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_RelatedObjectPropertiesInOrderBy_ManyLevelsDeep()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDefWithRelationshipBackToMyBo();
            MyBO bo1 = new MyBO();
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            string orderByClause = "MyRelationship.MyRelationshipToMyBo.MyRelationship.MyRelationshipToMyBo.TestProp";
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], null, -1, null, orderByClause);
            //-------------Test Result ----------------------
            Assert.AreEqual(@"SELECT DISTINCT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM (((`MyBO` LEFT JOIN `MyRelatedBo` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID`) " +
                "LEFT JOIN `MyBO` AS `MyBOMyRelationshipMyRelationshipToMyBo` " +
                "ON `MyBOMyRelationship`.`MyBoID` = `MyBOMyRelationshipMyRelationshipToMyBo`.`MyBoID`) " +
                "LEFT JOIN `MyRelatedBo` AS `MyBOMyRelationshipMyRelationshipToMyBoMyRelationship` " +
                "ON `MyBOMyRelationshipMyRelationshipToMyBo`.`RelatedID` = " +
                "`MyBOMyRelationshipMyRelationshipToMyBoMyRelationship`.`MyRelatedBoID`) " +
                "LEFT JOIN `MyBO` AS `MyBOMyRelationshipMyRelationshipToMyBoMyRelationshipMyRelationshipToMyBo` " +
                "ON `MyBOMyRelationshipMyRelationshipToMyBoMyRelationship`.`MyBoID` " +
                "= `MyBOMyRelationshipMyRelationshipToMyBoMyRelationshipMyRelationshipToMyBo`.`MyBoID` " +
                "ORDER BY MyBOMyRelationshipMyRelationshipToMyBoMyRelationshipMyRelationshipToMyBo.TestProp",
                statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_RelatedObjectPropertiesInOrderBy_WithNormalOrderByAsWell()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = new MyBO();
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], null, -1, null, "MyRelationship.MyRelatedTestProp, TestProp");
            //-------------Test Result ----------------------
            Assert.AreEqual(@"SELECT DISTINCT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM `MyBO` LEFT JOIN `MyRelatedBo` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID` " +
                "ORDER BY MyBOMyRelationship.MyRelatedTestProp, TestProp",
                statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_RelatedObjectPropertiesInOrderBy_TwoRelatedProps()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = new MyBO();
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], null, -1, null, "MyRelationship.MyRelatedTestProp, MyRelationship.MyRelatedBoID");
            //-------------Test Result ----------------------
            Assert.AreEqual(@"SELECT DISTINCT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM `MyBO` LEFT JOIN `MyRelatedBo` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID` " +
                "ORDER BY MyBOMyRelationship.MyRelatedTestProp, MyBOMyRelationship.MyRelatedBoID",
                statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_RelatedObjectPropertiesInOrderBy_WithCriteria()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = new MyBO();
            string criteria = "TestProp = 'Test' and MyRelationship.MyRelatedTestProp = 'TestValue'";
            IExpression expression = Expression.CreateExpression(criteria);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null, "MyRelationship.MyRelatedTestProp");
            //-------------Test Result ----------------------
            Assert.AreEqual(@"SELECT DISTINCT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM `MyBO` LEFT JOIN `MyRelatedBo` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID` " +
                "WHERE (`MyBO`.`TestProp` = ?Param0 AND `MyBOMyRelationship`.`MyRelatedTestProp` = ?Param1) " +
                "ORDER BY MyBOMyRelationship.MyRelatedTestProp",
                statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatement_SubRelatedObjectProperties()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDefWithRelationshipBackToMyBo();
            MyBO bo1 = new MyBO();
            string criteria = "TestProp = 'Test' and MyRelationship.MyRelationshipToMyBo.TestProp2 = 'TestValue'";
            IExpression expression = Expression.CreateExpression(criteria);
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null, null);
            Assert.AreEqual(@"SELECT DISTINCT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM (`MyBO` LEFT JOIN `MyRelatedBo` AS `MyBOMyRelationship` " +
                "ON `MyBO`.`RelatedID` = `MyBOMyRelationship`.`MyRelatedBoID`) " +
                "LEFT JOIN `MyBO` AS `MyBOMyRelationshipMyRelationshipToMyBo` " +
                "ON `MyBOMyRelationship`.`MyBoID` = `MyBOMyRelationshipMyRelationshipToMyBo`.`MyBoID` " +
                "WHERE (`MyBO`.`TestProp` = ?Param0 AND `MyBOMyRelationshipMyRelationshipToMyBo`.`TestProp2` = ?Param1)",
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
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null, null);
            Assert.AreEqual(@"SELECT DISTINCT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM `MyBO` LEFT JOIN `MyBO` AS `MyBOMyRelationship` " +
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
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(
                bo1, ClassDef.ClassDefs[typeof(MyBO)], expression, -1, null, null);
            Assert.AreEqual(@"SELECT DISTINCT `MyBO`.`MyBoID`, `MyBO`.`RelatedID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` " +
                "FROM `MyBO` LEFT JOIN `MyBO` AS `MyBOMyRelationship` " +
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

        [Test] 
        public void TestRestoreOfACreatedBusinessObject()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();

            newCP.Restore();
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }





        //TODO: From Brett Restore a parent object should remove all created objects on its relationships
        //  for later when we have these??

        private bool _addedEventFired;

        [Test]
        public void TestAddedEvent_FiringWhenSavingACreatedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            _addedEventFired = false; 
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();


            //---------------Execute Test ----------------------
            newCP.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(_addedEventFired);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddedEvent_NotFiringWhenRefreshing()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            _addedEventFired = false;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();
            newCP.Save();

            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            //---------------Execute Test ----------------------
            cpCol.Refresh();
            //---------------Test Result -----------------------
            Assert.IsFalse(_addedEventFired);
            //---------------Tear Down -------------------------          
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