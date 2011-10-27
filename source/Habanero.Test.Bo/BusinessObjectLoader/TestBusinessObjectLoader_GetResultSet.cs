// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Linq;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    [TestFixture]
    public class TestBusinessObjectLoader_GetResultSet 
    {
        #region Setup/Teardown
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        }

        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            SetupDataAccessor();
			
            TestUtil.WaitForGC();
        }

        #endregion

        private DataStoreInMemory _dataStore;

        protected virtual void SetupDataAccessor()
        {
            _dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(_dataStore);
        }

        [Test]
        public void ShouldReturnResultSetWithCorrectColumnsAndValues()
        {
            //---------------Set up test pack-------------------
            var classDef = ContactPersonTestBO.LoadDefaultClassDef();
            var cp1 = CreateContactPerson();
            var selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Fields.Clear();
            const string propertyName = "Surname";
            selectQuery.Fields.Add(propertyName, QueryBuilder.CreateQueryField(classDef, propertyName));
            //---------------Execute Test ----------------------
            var resultSet = BORegistry.DataAccessor.BusinessObjectLoader.GetResultSet(selectQuery);
            //---------------Test Result -----------------------
            var fields = resultSet.Fields.ToList();
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual(1, fields.Count);
            Assert.AreEqual(1, rows.Count);
            Assert.AreEqual(propertyName, fields[0].PropertyName);
            Assert.AreEqual(cp1.Surname, rows[0].RawValues[0]);
        }
        
        [Test]
        public void ShouldReturnTableWithCorrectColumnsAndValues_WhenCriteriaAreSupplied()
        {
            //---------------Set up test pack-------------------
            var classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime dateTime = DateTime.Now;
            var cp1 = CreateContactPerson(dateTime);
            var selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Fields.Clear();
            const string propertyName = "Surname";
            selectQuery.Fields.Add(propertyName, QueryBuilder.CreateQueryField(classDef, propertyName));
            selectQuery.Criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, dateTime);
            //---------------Execute Test ----------------------
            var resultSet = BORegistry.DataAccessor.BusinessObjectLoader.GetResultSet(selectQuery);
            //---------------Test Result -----------------------
            var fields = resultSet.Fields.ToList();
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual(1, fields.Count);
            Assert.AreEqual(1, rows.Count);
            Assert.AreEqual(propertyName, fields[0].PropertyName);
            Assert.AreEqual(cp1.Surname, rows[0].RawValues[0]);
        }

        [Test]
        public void ShouldGetValuesFromRelatedObjects_WhenFieldsAreFromOtherClasses()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            var addClassDef = AddressTestBO.LoadDefaultClassDef();

            var cp1 = CreateContactPersonWithAddress();
            var selectQuery = QueryBuilder.CreateSelectQuery(addClassDef);
            selectQuery.Fields.Clear();
            const string propertyName = "ContactPersonTestBO.Surname";
            selectQuery.Fields.Add(propertyName, QueryBuilder.CreateQueryField(addClassDef, propertyName));
            //---------------Execute Test ----------------------
            var resultSet = BORegistry.DataAccessor.BusinessObjectLoader.GetResultSet(selectQuery);
            //---------------Test Result -----------------------
            var fields = resultSet.Fields.ToList();
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual(1, fields.Count);
            Assert.AreEqual(1, rows.Count);
            Assert.AreEqual(propertyName, fields[0].PropertyName);
            Assert.AreEqual(cp1.Surname, rows[0].RawValues[0]);
        }

        [Test]
        [Ignore("Peter working on this")]
        public void ShouldMapValuesToCorrectType_WhenPropertyIsNotStringAndDatabaseTypeIs()
        {
            //---------------Set up test pack-------------------
            var classDef = ContactPersonTestBO.LoadDefaultClassDef();
            classDef.PropDefcol["FirstName"].PropertyType = typeof(int);
            var cp1 = new ContactPersonTestBO();
            cp1.Surname = Guid.NewGuid().ToString("N");
            var firstNameValue = 20;
            cp1.SetPropertyValue("FirstName", firstNameValue);
            cp1.Save();
            var selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Fields.Clear();
            selectQuery.Fields.Add("FirstName", QueryBuilder.CreateQueryField(classDef, "FirstName"));
            //---------------Execute Test ----------------------
            var resultSet = BORegistry.DataAccessor.BusinessObjectLoader.GetResultSet(selectQuery);
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual(firstNameValue, rows[0].RawValues[0]);
        }

        [Test]
        public void ShouldOrderResults_WhenOrderCriteriaAreSupplied()
        {
            //---------------Set up test pack-------------------
            var classDef = ContactPersonTestBO.LoadDefaultClassDef();
            var cp1 = CreateContactPerson("zzzz");
            var cp2 = CreateContactPerson("aaaa");
            var selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Fields.Clear();
            const string propertyName = "Surname";
            selectQuery.Fields.Add(propertyName, QueryBuilder.CreateQueryField(classDef, propertyName));
            selectQuery.OrderCriteria.Add(propertyName);
            //---------------Execute Test ----------------------
            var resultSet = BORegistry.DataAccessor.BusinessObjectLoader.GetResultSet(selectQuery);
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual(2, rows.Count);
            Assert.AreEqual(cp2.Surname, rows[0].Values[0]);
            Assert.AreEqual(cp1.Surname, rows[1].Values[0]);
        }

        private ContactPersonTestBO CreateContactPersonWithAddress(DateTime dateOfBirth)
        {
            var person = CreateContactPersonWithAddress();
            person.DateOfBirth = dateOfBirth;
            person.Save();
            return person;
        }
        private ContactPersonTestBO CreateContactPerson(DateTime dateOfBirth)
        {
            var person = CreateContactPerson();
            person.DateOfBirth = dateOfBirth;
            person.Save();
            return person;
        }

        private ContactPersonTestBO CreateContactPerson()
        {
            return CreateContactPerson(Guid.NewGuid().ToString("N"));
        }

        private ContactPersonTestBO CreateContactPerson(string surname)
        {
            var cp1 = new ContactPersonTestBO();
            cp1.Surname = surname;
            cp1.FirstName = Guid.NewGuid().ToString("N");
            cp1.Save();
            return cp1;
        }
        private ContactPersonTestBO CreateContactPersonWithAddress()
        {
            var cp1 = CreateContactPerson();
            var add = new AddressTestBO();
            add.ContactPersonTestBO = cp1;
            add.Save();
            return cp1;
        }
    }
}