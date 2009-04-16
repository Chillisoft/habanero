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
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO.TransactionCommitters
{
    [TestFixture]
    public class TestTransactionCommitterDB4O
    {
        private const string DB4O_FILE_NAME = "TestDB4O.DB";

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader()));
        }

        [Test]
        public void Test_CommitTransaction()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB4O committer = CreateDB4OTransactionCommiter(DB4O_FILE_NAME);
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
            TransactionCommitterDB4O committer = CreateDB4OTransactionCommiter(DB4O_FILE_NAME);
            Person person = new Person {LastName = "Bob"};
            committer.AddBusinessObject(person);
            //---------------Assert PreConditions---------------            
            AssertBONotInDataStore(DB4O_FILE_NAME);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            AssertBOInDataStore(DB4O_FILE_NAME, 1, typeof(Person));
        }

        [Test]
        public void Test_CommitTransaction_TwoObjects()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB4O committer = CreateDB4OTransactionCommiter(DB4O_FILE_NAME);
            Person person = new Person {LastName = "Bob"};
            Car car = new Car { CarRegNo="123456"};
            committer.AddBusinessObject(person);
            committer.AddBusinessObject(car);

            //---------------Assert PreConditions---------------  
            AssertBONotInDataStore(DB4O_FILE_NAME);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            AssertBOInDataStore(DB4O_FILE_NAME, 1, typeof(Person));
            AssertBOInDataStore(DB4O_FILE_NAME, 1, typeof(Car));
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_TryRollback()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB4O committer = new StubTransactionCommiterDB4O(DB4O_FILE_NAME);
            if (File.Exists(DB4O_FILE_NAME)) File.Delete(DB4O_FILE_NAME);
            Person person = new Person { LastName = "Bob" };
            committer.AddBusinessObject(person);
            //---------------Assert PreConditions---------------            
            AssertBONotInDataStore(DB4O_FILE_NAME);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            AssertBONotInDataStore(DB4O_FILE_NAME);
            //---------------Tear Down -------------------------    
        }


        private static TransactionCommitterDB4O CreateDB4OTransactionCommiter(string db4oFileName)
        {
            TransactionCommitterDB4O committer = new TransactionCommitterDB4O(db4oFileName);
            if (File.Exists(db4oFileName)) File.Delete(db4oFileName);
            return committer;
        }

        private static void AssertBOInDataStore(string db4oFileName, int count, Type boType) {
            IObjectContainer db = Db4oFactory.OpenFile(db4oFileName);
            try
            {
                Assert.AreEqual(count, db.Query(boType).Count);
            }
            finally
            {
                db.Close();
            }
        }

        private static void AssertBONotInDataStore(string db4oFileName) {
            IObjectContainer emptydb = Db4oFactory.OpenFile(db4oFileName);
            try
            {
                Assert.AreEqual(0, emptydb.Query(typeof(Person)).Count);
            }
            finally
            {
                emptydb.Close();
            }
        }
    }

    internal class StubTransactionCommiterDB4O : TransactionCommitterDB4O{
        public StubTransactionCommiterDB4O(string name) : base(name) {  }
        protected override bool CommitToDatasource() { return false; }
    }
}
