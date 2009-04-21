using System;
using System.IO;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB4O;
using Habanero.Test.BO;
using Habanero.Test.BO.BusinessObjectLoader;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.DB4O
{
    [TestFixture]
    public class TestBusinessObjectLoaderDB4O : TestBusinessObjectLoader
    {
        string db4oFileStore = "DataStore.db4o";
        protected override void SetupDataAccessor()
        {
            if (DB4ORegistry.DB != null) DB4ORegistry.DB.Close();
            if (File.Exists(db4oFileStore)) File.Delete(db4oFileStore);
            DB4ORegistry.DB = Db4oFactory.OpenFile(db4oFileStore);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
        }

        protected override void DeleteEnginesAndCars()
        {
            // do nothing
        }


        [Test]
        public void Test_CommitTransaction_Load_UsingTreeOfObjects()
        {
            //---------------Set up test pack-------------------
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader()));
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
            AddressTestBO.LoadDefaultClassDef();
            OrganisationTestBO org = new OrganisationTestBO();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateUnsavedContactPerson();
            org.ContactPeople.Add(cp);
            AddressTestBO address = AddressTestBO.CreateUnsavedAddress(cp);

            ITransactionCommitter firstTransactionCommitter = new TransactionCommitterDB4O(DB4ORegistry.DB);
            firstTransactionCommitter.AddBusinessObject(org);

            //---------------Assert Preconditions--------------
            Assert.IsTrue(org.Status.IsDirty);
            Assert.IsTrue(org.Status.IsNew);
            Assert.IsTrue(cp.Status.IsDirty);
            Assert.IsTrue(cp.Status.IsNew);
            Assert.IsTrue(address.Status.IsDirty);
            Assert.IsTrue(address.Status.IsNew);

            //---------------Execute Test ----------------------
            firstTransactionCommitter.CommitTransaction();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            GC.Collect();
            DB4ORegistry.DB.Close();
            DB4ORegistry.DB = Db4oFactory.OpenFile(db4oFileStore);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            OrganisationTestBO loadedOrg = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<OrganisationTestBO>(org.ID);

            //---------------Test Result -----------------------
            Assert.IsFalse(loadedOrg.Status.IsDirty);
            Assert.IsFalse(loadedOrg.Status.IsNew);
            Assert.AreEqual(1, loadedOrg.ContactPeople.Count);
            ContactPersonTestBO loadedContactPerson = loadedOrg.ContactPeople[0];
            Assert.IsFalse(loadedContactPerson.Status.IsDirty);
            Assert.IsFalse(loadedContactPerson.Status.IsNew);
            Assert.AreEqual(1, loadedContactPerson.Addresses.Count);
            AddressTestBO loadedAddress = loadedContactPerson.Addresses[0];
            Assert.IsFalse(loadedAddress.Status.IsDirty);
            Assert.IsFalse(loadedAddress.Status.IsNew);


            //---------------Tear Down -------------------------          
        }

    }
}