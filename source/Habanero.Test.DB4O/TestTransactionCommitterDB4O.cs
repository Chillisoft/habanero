using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB4O;
using Habanero.Test.BO;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.DB4O
{
    [TestFixture]
    public class TestTransactionCommitterDB4O
    {
        protected string _db4oFileName = "";

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader()));

        }

        [SetUp]
        public virtual void Setup()
        {
            _db4oFileName = "TestDB4O.DB";
            if (DB4ORegistry.DB != null) DB4ORegistry.DB.Close();
            if (File.Exists(_db4oFileName)) File.Delete(_db4oFileName);
            DB4ORegistry.DB = Db4oFactory.OpenFile(_db4oFileName);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
           
        }

        [Test]
        public void Test_CommitTransaction()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            Person person = new Person {LastName = "Bob"};
            committer.AddBusinessObject(person);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsFalse(person.Status.IsDirty);
            Assert.IsFalse(person.Status.IsNew);
        }

        [Test]
        public virtual void Test_CommitTransaction_WritesToDBFile()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            Person person = new Person { LastName = "Bob" };
            committer.AddBusinessObject(person);
            //---------------Assert PreConditions---------------            
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            DB4ORegistry.DB.Close();
            DB4ORegistry.DB = Db4oFactory.OpenFile(_db4oFileName);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBOInDataStore(1, typeof(BusinessObjectDTO));
        }

        [Test]
        public virtual void Test_CommitTransaction_TwoObjects()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            Person person = new Person { LastName = "Bob" };
            Structure.Car car = new Structure.Car { RegistrationNo = "123456" };
            committer.AddBusinessObject(person);
            committer.AddBusinessObject(car);

            //---------------Assert PreConditions---------------  
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            DB4ORegistry.DB.Close();
            DB4ORegistry.DB = Db4oFactory.OpenFile(_db4oFileName);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBOInDataStore(2, typeof(BusinessObjectDTO));
            //---------------Tear Down -------------------------          
        }

        [Test]
        public virtual void Test_TryRollback()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter committer = new StubTransactionCommiterDB4O(DB4ORegistry.DB);
            Person person = new Person { LastName = "Bob" };
            committer.AddBusinessObject(person);
            //---------------Assert PreConditions---------------            
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            DB4ORegistry.DB.Close();
            DB4ORegistry.DB = Db4oFactory.OpenFile(_db4oFileName);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
            //---------------Tear Down -------------------------    
        }

        [Test]
        public void Test_CommitTransaction_SetsClassDefFullName()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            Person person = new Person { LastName = "Bob" };
            committer.AddBusinessObject(person);
            //---------------Assert PreConditions---------------   
            Assert.IsTrue(string.IsNullOrEmpty(person.ClassDefName));
            //---------------Execute Test ----------------------
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(typeof(Person).Name, person.ClassDefName);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public virtual void Test_CommitTransaction_Update()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { LastName = "Bob" };

            IBusinessObjectLoader loader = new BusinessObjectLoaderDB4O(DB4ORegistry.DB);
            ITransactionCommitter firstTransactionCommitter = new TransactionCommitterDB4O(DB4ORegistry.DB);
            firstTransactionCommitter.AddBusinessObject(person);
            firstTransactionCommitter.CommitTransaction();

            //---------------Assert Preconditions--------------
            AssertBOInDataStore(1, typeof(BusinessObjectDTO));

            //---------------Execute Test ----------------------
            person.LastName = Guid.NewGuid().ToString("N");
            ITransactionCommitter secondTransactionCommitter = new TransactionCommitterDB4O(DB4ORegistry.DB);
            secondTransactionCommitter.AddBusinessObject(person);
            secondTransactionCommitter.CommitTransaction();
            DB4ORegistry.DB.Close();
            DB4ORegistry.DB = Db4oFactory.OpenFile(_db4oFileName);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBOInDataStore(1, typeof(BusinessObjectDTO));
            Assert.AreSame(person, loader.GetBusinessObject<Person>(person.ID));
            Assert.IsFalse(person.Status.IsDirty);
            //---------------Tear Down -------------------------
        }

        [Test]
        public virtual void Test_CommitTransaction_Delete()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { LastName = "Bob" };

            ITransactionCommitter firstTransactionCommitter = new TransactionCommitterDB4O(DB4ORegistry.DB);
            firstTransactionCommitter.AddBusinessObject(person);
            firstTransactionCommitter.CommitTransaction();

            //---------------Assert Preconditions--------------
            AssertBOInDataStore(1, typeof(BusinessObjectDTO));

            //---------------Execute Test ----------------------
            person.MarkForDelete();
            ITransactionCommitter secondTransactionCommitter = new TransactionCommitterDB4O(DB4ORegistry.DB);
            secondTransactionCommitter.AddBusinessObject(person);
            secondTransactionCommitter.CommitTransaction();
            DB4ORegistry.DB.Close();
            DB4ORegistry.DB = Db4oFactory.OpenFile(_db4oFileName);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
        }

        [Test]
        public void TestDeleteRelated()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            contactPersonTestBO.MarkForDelete();
            TransactionCommitter committer = new TransactionCommitterDB4O(DB4ORegistry.DB);
            committer.AddBusinessObject(contactPersonTestBO);

            //---------------Execute Test ----------------------
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.IsTrue(contactPersonTestBO.Status.IsNew);
            Assert.IsTrue(contactPersonTestBO.Status.IsDeleted);
            Assert.IsTrue(address.Status.IsNew);
            Assert.IsTrue(address.Status.IsDeleted);
        }

        [Test]
        public void TestDereferenceRelatedObjects()
        {
            //The Car has a single relationship to engine. The car->engine relationship is marked 
            // as a dereference related relationship.
            //---------------Set up test pack-------------------

            Car car = new Car();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.Save();

            Engine engine = new Engine();

            engine.SetPropertyValue("EngineNo", "NO111");
            const string carIDProp = "CarID";
            engine.SetPropertyValue(carIDProp, car.GetPropertyValue(carIDProp));
            engine.Save();

            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(engine);
            Assert.AreSame(engine.GetCar(), car);

            //---------------Execute Test ----------------------
            car.MarkForDelete();
            car.Save();

            //---------------Test Result -----------------------
            Assert.IsNull(engine.GetPropertyValue(carIDProp));
            Assert.IsNull(engine.GetCar());
            //---------------Test TearDown -----------------------
        }


        protected static void AssertBOInDataStore(int count, Type boType) {
            Assert.AreEqual(count, DB4ORegistry.DB.Query(boType).Count);
        }

        protected static void AssertBONotInDataStore(Type boType)
        {
            Assert.AreEqual(0, DB4ORegistry.DB.Query(boType).Count);
        }
    }

    public class TestTransactionCommitterDb4OServer : TestTransactionCommitterDB4O
    {
        [SetUp]
        public override void Setup()
        {
            _db4oFileName = "TestDB4OServer.DB";
            if (DB4ORegistry.DB != null)
            {
                if (DB4ORegistry.DB4OServer != null) DB4ORegistry.DB4OServer.Close();
                try
                {
                    DB4ORegistry.DB.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            if (File.Exists(_db4oFileName)) File.Delete(_db4oFileName);
            DB4ORegistry.CreateDB4OServerConfiguration("localhost",1252,_db4oFileName);
            DB4ORegistry.DB4OServer.GrantAccess("bob","bob");
            DB4ORegistry.OpenDB4OClient("bob","bob");
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
        }


        [Test]
        public override void Test_CommitTransaction_WritesToDBFile()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            Person person = new Person { LastName = "Bob" };
            committer.AddBusinessObject(person);
            //---------------Assert PreConditions---------------            
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            CloseConnectionsAndReopen();
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBOInDataStore(1, typeof(BusinessObjectDTO));
        }

        private void CloseConnectionsAndReopen()
        {
            DB4ORegistry.DB.Close();
            DB4ORegistry.DB4OServer.Close();
            DB4ORegistry.CreateDB4OServerConfiguration("localhost", 1252, _db4oFileName);
            DB4ORegistry.DB4OServer.GrantAccess("bob", "bob");
            DB4ORegistry.OpenDB4OClient("bob", "bob");
        }

        [Test]
        public override void Test_CommitTransaction_TwoObjects()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            Person person = new Person { LastName = "Bob" };
            Structure.Car car = new Structure.Car { RegistrationNo = "123456" };
            committer.AddBusinessObject(person);
            committer.AddBusinessObject(car);

            //---------------Assert PreConditions---------------  
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            CloseConnectionsAndReopen();
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBOInDataStore(2, typeof(BusinessObjectDTO));
            //---------------Tear Down -------------------------          
        }

        [Test]
        public override void Test_TryRollback()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter committer = new StubTransactionCommiterDB4O(DB4ORegistry.DB);
            Person person = new Person { LastName = "Bob" };
            committer.AddBusinessObject(person);
            //---------------Assert PreConditions---------------            
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            CloseConnectionsAndReopen();
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
            //---------------Tear Down -------------------------    
        }


        [Test]
        public override void Test_CommitTransaction_Update()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { LastName = "Bob" };

            IBusinessObjectLoader loader = new BusinessObjectLoaderDB4O(DB4ORegistry.DB);
            ITransactionCommitter firstTransactionCommitter = new TransactionCommitterDB4O(DB4ORegistry.DB);
            firstTransactionCommitter.AddBusinessObject(person);
            firstTransactionCommitter.CommitTransaction();

            //---------------Assert Preconditions--------------
            AssertBOInDataStore(1, typeof(BusinessObjectDTO));

            //---------------Execute Test ----------------------
            person.LastName = Guid.NewGuid().ToString("N");
            ITransactionCommitter secondTransactionCommitter = new TransactionCommitterDB4O(DB4ORegistry.DB);
            secondTransactionCommitter.AddBusinessObject(person);
            secondTransactionCommitter.CommitTransaction();
            CloseConnectionsAndReopen();
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBOInDataStore(1, typeof(BusinessObjectDTO));
            Assert.AreSame(person, loader.GetBusinessObject<Person>(person.ID));
            Assert.IsFalse(person.Status.IsDirty);
            //---------------Tear Down -------------------------
        }

        [Test]
        public override void Test_CommitTransaction_Delete()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { LastName = "Bob" };

            ITransactionCommitter firstTransactionCommitter = new TransactionCommitterDB4O(DB4ORegistry.DB);
            firstTransactionCommitter.AddBusinessObject(person);
            firstTransactionCommitter.CommitTransaction();

            //---------------Assert Preconditions--------------
            AssertBOInDataStore(1, typeof(BusinessObjectDTO));

            //---------------Execute Test ----------------------
            person.MarkForDelete();
            ITransactionCommitter secondTransactionCommitter = new TransactionCommitterDB4O(DB4ORegistry.DB);
            secondTransactionCommitter.AddBusinessObject(person);
            secondTransactionCommitter.CommitTransaction();
            CloseConnectionsAndReopen();
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
            //---------------Test Result -----------------------
            AssertBONotInDataStore(typeof(BusinessObjectDTO));
        }

        [TestFixtureTearDown]
        public void TestFixtureFinish()
        {
            DB4ORegistry.DB.Close();
            DB4ORegistry.DB4OServer.Close();
        }
    }

    internal class StubTransactionCommiterDB4O : TransactionCommitterDB4O
    {
        public StubTransactionCommiterDB4O(IObjectContainer objectContainer) : base(objectContainer) { }
        protected override bool CommitToDatasource() { return false; }
    }
}