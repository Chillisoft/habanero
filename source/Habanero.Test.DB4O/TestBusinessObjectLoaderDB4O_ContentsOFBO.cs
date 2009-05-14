using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB4O;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.DB4O
{
    [TestFixture]
    public class TestBusinessObjectLoaderDB4O_ContentsOfBO
    {
        const string _db4oFileName = "TestDB4O.DB";
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader()));

        }

        [SetUp]
        public void Setup()
        {
            if (DB4ORegistry.DB != null) DB4ORegistry.DB.Close();
            if (File.Exists(_db4oFileName)) File.Delete(_db4oFileName);
            DB4ORegistry.DB = Db4oFactory.OpenFile(_db4oFileName);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);

        }

        private void CloseAndOpenDB4OConnection()
        {
            BusinessObjectManager.Instance.ClearLoadedObjects();
            DB4ORegistry.DB.Close();
            DB4ORegistry.DB = Db4oFactory.OpenFile(_db4oFileName);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
        }

        [Test]
        public void Test_LoadedObjectHasCorrectStatus()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { LastName = "Bob" };
            person.Save();
            CloseAndOpenDB4OConnection();
            //---------------Assert Preconditions--------------
            Assert.IsFalse(person.Status.IsDirty);
            Assert.IsFalse(person.Status.IsNew);
            //---------------Execute Test ----------------------

            Person loadedPerson = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Person>(person.ID);

            //---------------Test Result -----------------------
            Assert.IsFalse(loadedPerson.Status.IsDirty);
            Assert.IsFalse(loadedPerson.Status.IsNew);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_LoadedObjectHasCorrectValues()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { LastName = "Bob" };
            person.Save();
            CloseAndOpenDB4OConnection();
            //---------------Assert Preconditions--------------
            //---------------Execute Test ----------------------

            Person loadedPerson = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Person>(person.ID);

            //---------------Test Result -----------------------
            Assert.AreEqual(person.LastName, loadedPerson.LastName);
            Assert.AreEqual(person.PersonID, loadedPerson.PersonID);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_SavingLoadedObjectSavesOriginalObject()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { LastName = "Bob" };
            person.Save();

            CloseAndOpenDB4OConnection();

            //---------------Assert Preconditions--------------
            //---------------Execute Test ----------------------
            Person loadedPerson = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Person>(person.ID);
            loadedPerson.LastName = "Jim";
            loadedPerson.Save();
            CloseAndOpenDB4OConnection();

            Person secondLoadedPerson = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Person>(person.ID);
            //---------------Test Result -----------------------
            Assert.AreEqual(loadedPerson.LastName, secondLoadedPerson.LastName);
            Assert.AreEqual(loadedPerson.PersonID, secondLoadedPerson.PersonID);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_LoadingObjectWithChangedClassDef_NewProperty()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { LastName = "Bob" };
            person.Save();

            ClassDef personClassDef = person.ClassDef;
            CloseAndOpenDB4OConnection();

            string propertyName = TestUtil.GetRandomString();
            string defaultvalue = "defaultValue";
            personClassDef.PropDefcol.Add(new PropDef(propertyName, typeof(string), PropReadWriteRule.ReadWrite, defaultvalue));


            //---------------Assert Preconditions--------------
            //---------------Execute Test ----------------------
            Person loadedPerson = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Person>(person.ID);
            //---------------Test Result -----------------------
            Assert.IsTrue(loadedPerson.Props.Contains(propertyName));
            Assert.AreEqual(defaultvalue, loadedPerson.GetPropertyValue(propertyName));
            //---------------Tear Down -------------------------          
        }

    }
}