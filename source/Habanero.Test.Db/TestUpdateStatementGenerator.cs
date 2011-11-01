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
using System.Linq;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.BO.ClassDefinition;
using Habanero.Test.BO.SqlGeneration;
using NUnit.Framework;

namespace Habanero.Test.DB.SqlGeneration
{
    [TestFixture]
    public class TestUpdateStatementGenerator: TestUsingDatabase
    {

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
        }

        [Test]
        public void TestDelimitedTableNameWithSpaces()
        {
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();
            TestAutoInc bo = new TestAutoInc();
            bo.Save();
            ClassDef.ClassDefs[typeof(TestAutoInc)].TableName = "test autoinc";
            bo.TestField = TestUtil.GetRandomString();
            UpdateStatementGenerator gen = new UpdateStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            var statementCol = gen.Generate();
            ISqlStatement statement = statementCol.First();
            StringAssert.Contains("`test autoinc`", statement.Statement.ToString());
        }

        [Test]
        public void TestUpdateStatementExcludesNonPersistableProps()
        {
           
            const string newPropName = "NewProp";
            MockBO bo = StatementGeneratorTestHelper.CreateMockBOWithExtraNonPersistableProp(newPropName);
            bo.SetPropertyValue(newPropName, "newvalue323");
            bo.SetPropertyValue("MockBOProp2", "dfggjh");

            UpdateStatementGenerator gen = new UpdateStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            var statementCol = gen.Generate();
            var sqlStatements = statementCol.ToList();
            Assert.AreEqual(1, sqlStatements.Count);
            ISqlStatement statement = sqlStatements[0];
            Assert.IsFalse(statement.Statement.ToString().Contains(newPropName));
        }
        
        [Test]
        public void Test_UpdateSqlStatement()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPerson();
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();

            SingleRelationship<OrganisationTestBO> singleRelationship = contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            singleRelationship.SetRelatedObject(organisationTestBO);
            IRelationship relationship = organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            //TransactionalSingleRelationship_Added tsr = new TransactionalSingleRelationship_Added(singleRelationship);
            UpdateStatementGenerator generator = new UpdateStatementGenerator(contactPersonTestBO, DatabaseConnection.CurrentConnection);
            //---------------Assert PreConditions--------------- 

            //---------------Execute Test ----------------------
            var sql = generator.GenerateForRelationship(relationship, contactPersonTestBO);
            //---------------Test Result -----------------------
            var sqlStatements = sql.ToList();
            Assert.AreEqual(1, sqlStatements.Count);
            Assert.AreEqual("UPDATE `contact_person` SET `OrganisationID` = ?Param0 WHERE `ContactPersonID` = ?Param1", sqlStatements[0].Statement.ToString());
            Assert.AreEqual(organisationTestBO.OrganisationID.Value.ToString("B").ToUpper(), sqlStatements[0].Parameters[0].Value);
            Assert.AreEqual(contactPersonTestBO.ContactPersonID.ToString("B").ToUpper(), sqlStatements[0].Parameters[1].Value);           
        }


        [Test]
        public void Test_UpdateSqlStatement_CompositeKey()
        {
            //---------------Set up test pack-------------------
            TestUsingDatabase.SetupDBDataAccessor();
            Car car = new Car();
            car.Save();

            ContactPersonCompositeKey contactPerson = new ContactPersonCompositeKey();
            contactPerson.PK1Prop1 = TestUtil.GetRandomString();
            contactPerson.PK1Prop2 = TestUtil.GetRandomString();
            contactPerson.Save();

            contactPerson.GetCarsDriven().Add(car);

            SingleRelationship<ContactPersonCompositeKey> singleRelationship = car.Relationships.GetSingle<ContactPersonCompositeKey>("Driver");
            singleRelationship.SetRelatedObject(contactPerson);
            IRelationship relationship = contactPerson.Relationships.GetMultiple<Car>("Driver");
            UpdateStatementGenerator generator = new UpdateStatementGenerator(car, DatabaseConnection.CurrentConnection);
            //---------------Assert PreConditions--------------- 

            //---------------Execute Test ----------------------
            var sql = generator.GenerateForRelationship(relationship, car);
            //---------------Test Result -----------------------
            var sqlStatements = sql.ToList();
            Assert.AreEqual(1, sqlStatements.Count);
            Assert.AreEqual("UPDATE `car_table` SET `Driver_FK1` = ?Param0, `Driver_FK2` = ?Param1 WHERE `CAR_ID` = ?Param2", sqlStatements[0].Statement.ToString());
            Assert.AreEqual(contactPerson.PK1Prop1, sqlStatements[0].Parameters[0].Value);
            Assert.AreEqual(contactPerson.PK1Prop2, sqlStatements[0].Parameters[1].Value);
            Assert.AreEqual(car.CarID.ToString("B").ToUpper(), sqlStatements[0].Parameters[2].Value);

            //---------------Tear Down -------------------------                  
        }
    }
}