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
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.DB4O
{
    [TestFixture]
    public class TestTransactionCommitterDB4O
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
        public void Test_CommitTransaction_WritesToDBFile()
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
        public void Test_CommitTransaction_TwoObjects()
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
        public void Test_TryRollback()
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
        public void Test_CommitTransaction_Update()
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
        public void Test_CommitTransaction_Delete()
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

       
       


        private static void AssertBOInDataStore(int count, Type boType) {
            Assert.AreEqual(count, DB4ORegistry.DB.Query(boType).Count);
        }

        private static void AssertBONotInDataStore(Type boType)
        {
            Assert.AreEqual(0, DB4ORegistry.DB.Query(boType).Count);
        }
    }

    internal class StubTransactionCommiterDB4O : TransactionCommitterDB4O
    {
        public StubTransactionCommiterDB4O(IObjectContainer objectContainer) : base(objectContainer) { }
        protected override bool CommitToDatasource() { return false; }
    }
}