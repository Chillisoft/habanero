using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    public abstract class TestBusinessObjectLoader
    {
        protected abstract IBusinessObjectLoader SetupLoader();
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixture]
        public class TestBusinessObjectLoaderInMemory : TestBusinessObjectLoader
        {
            protected override IBusinessObjectLoader SetupLoader()
            {
                DataStoreInMemory dataStore = new DataStoreInMemory();
                GlobalRegistry.TransactionCommitterFactory = new TransactionCommitterFactoryInMemory(dataStore);
                return new BusinessObjectLoaderInMemory(dataStore);
            }
        }

        [TestFixture]
        public class TestBusinessObjectLoaderDB : TestBusinessObjectLoader
        {
            //TODO: stop this using the BOLoader
            [SetUp]
            public override void SetupTest()
            {
                base.SetupTest();
                ContactPersonTestBO.DeleteAllContactPeople();
            }
            public TestBusinessObjectLoaderDB()
            {
                new TestUsingDatabase().SetupDBConnection();
            }

            protected override IBusinessObjectLoader SetupLoader()
            {
                GlobalRegistry.TransactionCommitterFactory = new TransactionCommitterFactoryDB();
                return new BusinessObjectLoaderDB(DatabaseConnection.CurrentConnection);
            }
        }

        [Test]
        public void TestGetBusinessObjectWhenNotExists()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectLoader loader = SetupLoader();
            ContactPersonTestBO.LoadDefaultClassDef();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                loader.GetBusinessObject<ContactPersonTestBO>(new ContactPersonTestBO().PrimaryKey);
            //---------------Test Result -----------------------
            Assert.IsNull(loadedCP);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObject_PrimaryKey()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectLoader loader = SetupLoader();

            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = loader.GetBusinessObject<ContactPersonTestBO>(cp.PrimaryKey);
            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObject_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectLoader loader = SetupLoader();

            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();

            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, cp.Surname);
            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = loader.GetBusinessObject<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectLoader loader = SetupLoader();

            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.Save();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                loader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCriteriaSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectLoader loader = SetupLoader();
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = loader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.Criteria);
            //---------------Tear Down -------------------------
        }

        [Test, Ignore ("Working on this")]
        public void TestRefreshLoadedCollection()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectLoader loader = SetupLoader();
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.Save();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            BusinessObjectCollection<ContactPersonTestBO> col = loader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            ContactPersonTestBO cp3 = new ContactPersonTestBO();
            cp3.DateOfBirth = now;
            cp3.Surname = Guid.NewGuid().ToString("N");
            cp3.Save();
            //---------------Execute Test ----------------------
            loader.Refresh(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            //---------------Tear Down -------------------------
        }


    }
}
