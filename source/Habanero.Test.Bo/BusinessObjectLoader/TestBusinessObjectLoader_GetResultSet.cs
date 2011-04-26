using System;
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
            Assert.AreEqual(1, resultSet.Fields.Count);
            Assert.AreEqual(1, resultSet.Rows.Count);
            Assert.AreEqual(propertyName, resultSet.Fields[0].PropertyName);
            Assert.AreEqual(cp1.Surname, resultSet.Rows[0].RawValues[0]);
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
            Assert.AreEqual(1, resultSet.Fields.Count);
            Assert.AreEqual(1, resultSet.Rows.Count);
            Assert.AreEqual(propertyName, resultSet.Fields[0].PropertyName);
            Assert.AreEqual(cp1.Surname, resultSet.Rows[0].RawValues[0]);
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
            Assert.AreEqual(1, resultSet.Fields.Count);
            Assert.AreEqual(1, resultSet.Rows.Count);
            Assert.AreEqual(propertyName, resultSet.Fields[0].PropertyName);
            Assert.AreEqual(cp1.Surname, resultSet.Rows[0].RawValues[0]);
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
            Assert.AreEqual(firstNameValue, resultSet.Rows[0].RawValues[0]);
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
            var cp1 = new ContactPersonTestBO();
            cp1.Surname = Guid.NewGuid().ToString("N");
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