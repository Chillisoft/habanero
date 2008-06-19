using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    public abstract class TestBusinessObjectLoader
    {
        protected abstract void SetupLoader();
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();

        }

        [TearDown]
        public virtual void TearDownTest()
        {
           
        }

        [TestFixture]
        public class TestBusinessObjectLoaderInMemory : TestBusinessObjectLoader
        {
            protected override void SetupLoader()
            {
                DataStoreInMemory dataStore = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
            }

            [Test]
            public void TestRefreshLoadedCollection_DeletedItem()
            {
                //---------------Set up test pack-------------------
                DataStoreInMemory dataStore = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
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
                BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

                dataStore.Remove(cp2);
                //---------------Execute Test ----------------------
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                Assert.Contains(cp1, col);
                //---------------Tear Down -------------------------
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

            protected override void SetupLoader()
            {
                BORegistry.DataAccessor = new DataAccessorDB();
            }
        }

        [Test]
        public void TestGetBusinessObjectWhenNotExists()
        {
            //---------------Set up test pack-------------------
            SetupLoader();
            ContactPersonTestBO.LoadDefaultClassDef();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(new ContactPersonTestBO().PrimaryKey);
            //---------------Test Result -----------------------
            Assert.IsNull(loadedCP);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObject_PrimaryKey()
        {
            //---------------Set up test pack-------------------
            SetupLoader();

            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.PrimaryKey);
            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObject_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            SetupLoader();

            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();

            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, cp.Surname);
            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            SetupLoader();

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
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
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
            SetupLoader();
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestRefreshLoadedCollection()
        {
            //---------------Set up test pack-------------------
            SetupLoader();
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
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            ContactPersonTestBO cp3 = new ContactPersonTestBO();
            cp3.DateOfBirth = now;
            cp3.Surname = Guid.NewGuid().ToString("N");
            cp3.Save();
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            //---------------Tear Down -------------------------
        }

        [Test, Ignore("working on this")]
        public void TestGetRelatedBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            SetupLoader();
            Address address;
            ContactPersonTestBO cp = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria relationshipCriteria = Criteria.FromRelationship(cp.Relationships["Addresses"]);
            RelatedBusinessObjectCollection<Address> addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<Address>(cp.Relationships["Addresses"]);
            //---------------Test Result -----------------------
            Assert.AreEqual(relationshipCriteria, addresses.SelectQuery.Criteria);
            Assert.AreEqual(1, addresses.Count);
            Assert.Contains(address, addresses);
            //---------------Tear Down -------------------------          
        }

        [Test, Ignore("working on this")]
        public void TestLoadThroughRelationship()
        {
            //---------------Set up test pack-------------------
            SetupLoader();
            Address address;
            ContactPersonTestBO cp = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<Address> addresses = cp.Addresses;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, addresses.Count);
            Assert.Contains(address, addresses);
            //---------------Tear Down -------------------------          
        }

        //[Test]
        //public void TestLoadAll_Loader()
        //{
        //    //---------------Set up test pack-------------------
        //    SetupLoader();
        //    BOLoader.Instance.ClearLoadedBusinessObjects();
        //    ContactPersonTestBO.LoadDefaultClassDef();

        //    ContactPersonTestBO cp = new ContactPersonTestBO();
        //    cp.Save();
        //    //---------------Execute Test ----------------------
        //    BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

        //    col.LoadAll_Loader();
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, col.Count);
        //    Assert.Contains(cp, col);
        //    //---------------Tear Down -------------------------
        //}

        [Test]
        public void TestLoadWithOrderBy()
        {
            //---------------Set up test pack-------------------
            SetupLoader();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.Surname = "eeeee";
            cp1.Save();

            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.Surname = "ggggg";
            cp2.Save();

            ContactPersonTestBO cp3 = new ContactPersonTestBO();
            cp3.Surname = "bbbbb";
            cp3.Save();
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(null, new OrderCriteria("Surname"));
            //---------------Test Result -----------------------

            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
            //---------------Tear Down -------------------------
        }

    }


}
