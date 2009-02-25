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

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
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
           
            const string newPropName = "NewProp";
            MockBO bo = StatementGeneratorTestHelper.CreateMockBOWithExtraNonPersistableProp(newPropName);
            bo.SetPropertyValue(newPropName, "newvalue323");
            bo.SetPropertyValue("MockBOProp2", "dfggjh");

            UpdateStatementGenerator gen = new UpdateStatementGenerator(bo, DatabaseConnection.CurrentConnection);
            ISqlStatementCollection statementCol = gen.Generate();
            Assert.AreEqual(1, statementCol.Count);
            ISqlStatement statement = statementCol[0];
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
            ISqlStatementCollection sql = generator.GenerateForRelationship(relationship, contactPersonTestBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, sql.Count);
            Assert.AreEqual("UPDATE `contact_person` SET `OrganisationID` = ?Param0 WHERE `ContactPersonID` = ?Param1", sql[0].Statement.ToString());
            Assert.AreEqual(organisationTestBO.OrganisationID.Value.ToString("B").ToUpper(), sql[0].Parameters[0].Value);
            Assert.AreEqual(contactPersonTestBO.ContactPersonID.ToString("B").ToUpper(), sql[0].Parameters[1].Value);           
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
            ISqlStatementCollection sql = generator.GenerateForRelationship(relationship, car);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, sql.Count);
            Assert.AreEqual("UPDATE `car_table` SET `Driver_FK1` = ?Param0, `Driver_FK2` = ?Param1 WHERE `CAR_ID` = ?Param2", sql[0].Statement.ToString());
            Assert.AreEqual(contactPerson.PK1Prop1, sql[0].Parameters[0].Value);
            Assert.AreEqual(contactPerson.PK1Prop2, sql[0].Parameters[1].Value);
            Assert.AreEqual(car.CarID.ToString("B").ToUpper(), sql[0].Parameters[2].Value);

            //---------------Tear Down -------------------------                  
        }
    }
}
