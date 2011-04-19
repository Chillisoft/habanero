using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataStoreInMemoryXmlReader
    {
        
        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            new Address();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            var reader = new DataStoreInMemoryXmlReader(new MemoryStream());
            //---------------Test Result -----------------------
            
            //---------------Tear Down -------------------------          
        }
     
        [Test]
        public void Test_Read()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var savedDataStore = new DataStoreInMemory();
            savedDataStore.Add(new MyBO());
            var writeStream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            var loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            var reader = new DataStoreInMemoryXmlReader(writeStream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
        }

        [Test]
        public void Test_Read_WhenPropHasBeenRemoved_ShouldReadWithoutProp()
        {
            //---------------Set up test pack-------------------
            IClassDef def = LoadMyBOClassDefsWithNoUIDefs();
            var savedDataStore = new DataStoreInMemory();
            savedDataStore.Add(new MyBO());
            var writeStream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            var loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            var reader = new DataStoreInMemoryXmlReader(writeStream);
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            def.PropDefcol.Remove(def.PropDefcol["TestProp2"]);
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
        }

        [Test]
        public void Test_Read_ShouldLoadPropertiesCorrectly()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var savedDataStore = new DataStoreInMemory();
            var savedBo = new MyBO {TestProp = TestUtil.GetRandomString(), TestProp2 = TestUtil.GetRandomString()};
            var transactionCommitter = new TransactionCommitterInMemory(savedDataStore);
            transactionCommitter.AddBusinessObject(savedBo);
            transactionCommitter.CommitTransaction();
            var writeStream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            var loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            var reader = new DataStoreInMemoryXmlReader(writeStream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
            IBusinessObject loadedBo;
            bool success = loadedDataStore.AllObjects.TryGetValue(savedBo.MyBoID.Value, out loadedBo);
            Assert.IsTrue(success);
            Assert.IsNotNull(loadedBo);
            Assert.IsInstanceOf(typeof(MyBO), loadedBo);
            var loadedMyBo = (MyBO) loadedBo;
            Assert.AreNotSame(savedBo, loadedMyBo);
            Assert.AreEqual(savedBo.MyBoID, loadedMyBo.MyBoID);
            Assert.AreEqual(savedBo.Props["MyBoID"].PersistedPropertyValue, loadedMyBo.Props["MyBoID"].PersistedPropertyValue);
            Assert.AreEqual(savedBo.TestProp, loadedMyBo.TestProp);
            Assert.AreEqual(savedBo.Props["TestProp"].PersistedPropertyValue, loadedMyBo.Props["TestProp"].PersistedPropertyValue);
            Assert.AreEqual(savedBo.TestProp2, loadedMyBo.TestProp2);
            Assert.AreEqual(savedBo.Props["TestProp2"].PersistedPropertyValue, loadedMyBo.Props["TestProp2"].PersistedPropertyValue);
        }

        [Test]
        public void Test_ReadWrite_MultipleObjects()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var savedDataStore = new DataStoreInMemory();
            var bo1 = new MyBO();
            var bo2 = new Car();
            savedDataStore.Add(bo1);
            savedDataStore.Add(bo2);
            var writeStream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            var loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            var reader = new DataStoreInMemoryXmlReader(writeStream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, loadedDataStore.Count);
            Assert.IsNotNull(loadedDataStore.Find<MyBO>(bo1.ID));
            Assert.IsNotNull(loadedDataStore.Find<Car>(bo2.ID));
        }

        [Test]
        public void Test_Read_ShouldLoadObjectsWithCorrectStatus()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var savedDataStore = new DataStoreInMemory();
            var savedBo = new MyBO();
            var transactionCommitter = new TransactionCommitterInMemory(savedDataStore);
            transactionCommitter.AddBusinessObject(savedBo);
            transactionCommitter.CommitTransaction();
            var writeStream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            var loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            var reader = new DataStoreInMemoryXmlReader(writeStream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
            IBusinessObject loadedBo;
            var success = loadedDataStore.AllObjects.TryGetValue(savedBo.MyBoID.GetValueOrDefault(), out loadedBo);
            Assert.IsTrue(success);
            Assert.IsNotNull(loadedBo);
            Assert.IsInstanceOf(typeof(MyBO), loadedBo);
            var loadedMyBo = (MyBO)loadedBo;
            Assert.AreNotSame(savedBo, loadedMyBo);

            Assert.IsFalse(loadedBo.Status.IsNew, "Should not be New");
            Assert.IsFalse(loadedBo.Status.IsDeleted, "Should not be Deleted");
            Assert.IsFalse(loadedBo.Status.IsDirty, "Should not be Dirty");
            Assert.IsFalse(loadedBo.Status.IsEditing, "Should not be Editing");
        }

        [Test]
        public void Test_Read_WhenCalled_ShouldSet_ReadResult()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var savedDataStore = new DataStoreInMemory();
            savedDataStore.Add(new MyBO());
            var writeStream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            var loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            DataStoreInMemoryXmlReader reader = new DataStoreInMemoryReaderWithSetupPropertyException(writeStream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
            Assert.IsNotNull(reader.ReadResult);
        }

        [Test]
        public void Test_Read_WhenCalledAndNoPropertyReadExceptions_ShouldSet_ReadResult_Successful_True_Bug1336()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var bo = new MyBO();
            var reader = new DataStoreInMemoryXmlReader(WriteBoToStream(bo));
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            reader.Read();
            //---------------Test Result -----------------------
            Assert.IsNotNull(reader.ReadResult);
            Assert.IsTrue(reader.ReadResult.Successful);
        }

        [Test]
        public void Acceptance_Test_Read_WhenCalledWithPropertyReadExceptions_ShouldSet_ReadResult_Successful_False_And_Message_Bug1336()
        {
            //---------------Set up test pack-------------------
            var classDef = LoadMyBOClassDefsWithNoUIDefs();
            var bo = new MyBO {TestProp = "characters"};
            var propDef = classDef.PropDefcol["TestProp"];
            classDef.PropDefcol.Remove(propDef);
            var propDefWithDifferentType = propDef.Clone();

            // Change type to force property read exception
            propDefWithDifferentType.PropertyType = typeof(int);
            classDef.PropDefcol.Add(propDefWithDifferentType);
            var reader = new DataStoreInMemoryXmlReader(WriteBoToStream(bo));
            //---------------Assert Precondition----------------
            Assert.IsNull(reader.ReadResult);
            //---------------Execute Test ----------------------
            var businessObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, businessObjects.Count);
            Assert.IsNotNull(reader.ReadResult);
            Assert.IsFalse(reader.ReadResult.Successful);
            StringAssert.Contains("An error occured when attempting to set property 'MyBO.TestProp'.", reader.ReadResult.Message);
        }

        [Test]
        public void Test_Read_WhenCalledWithPropertyReadExceptions_ShouldSet_ReadResult_Successful_False_And_Message_Bug1336()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var bo = new MyBO {TestProp = "characters"};
            var stream = WriteBoToStream(bo);
            var reader = new DataStoreInMemoryReaderWithSetupPropertyException(stream);
            //---------------Assert Precondition----------------
            Assert.IsNull(reader.ReadResult);
            //---------------Execute Test ----------------------
            var businessObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, businessObjects.Count);
            Assert.IsNotNull(reader.ReadResult);
            Assert.IsFalse(reader.ReadResult.Successful);
            StringAssert.Contains("An error occured when attempting to set property 'MyBO.MyBoID'.", reader.ReadResult.Message);
            StringAssert.Contains("An error occured when attempting to set property 'MyBO.TestProp'.", reader.ReadResult.Message);
            StringAssert.Contains("An error occured when attempting to set property 'MyBO.TestProp2'.", reader.ReadResult.Message);
        }

        [Test]
        public void Test_Read_LoadingAnObjectShouldUpdateObjectInDataAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            LoadMyBOClassDefsWithNoUIDefs();
            var newBo = new MyBO { TestProp = "characters" };
            var stream = WriteBoToStream(newBo);
            newBo.TestProp = "oldvalue";
            newBo.Save();
            var reader = new DataStoreInMemoryXmlReader(stream);
            //---------------Assert Precondition----------------
            Assert.AreEqual("oldvalue", newBo.TestProp);
            //---------------Execute Test ----------------------
            var businessObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, businessObjects.Count);
            Assert.AreSame(newBo, businessObjects[newBo.MyBoID.Value]);
            Assert.AreEqual("characters", newBo.TestProp);
        }

        [Test]
        public void ReadFromString()
        {
            //---------------Set up test pack-------------------
            var dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            var writer = new DataStoreInMemoryXmlWriter();
            var sb = new StringBuilder();
            writer.WriteToString(dataStore, sb);
            string xml = sb.ToString();

            var reader = new DataStoreInMemoryXmlReader();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.Greater(xml.Length, 100);
            //---------------Execute Test ----------------------
            var businessObjects =  reader.ReadFromString(xml);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, businessObjects.Count);
        }

        private static MemoryStream WriteBoToStream(MyBO bo)
        {
            var savedDataStore = new DataStoreInMemory();
            savedDataStore.Add(bo);
            var writeStream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            writeStream.Seek(0, SeekOrigin.Begin);
            return writeStream;
        }

        private static IClassDef LoadMyBOClassDefsWithNoUIDefs()
        {
            ClassDef.ClassDefs.Clear();
            return MyBO.LoadClassDefsNoUIDef();
        }
        // ReSharper restore InconsistentNaming

    }


    /// <summary>
    /// This spy class is used to simulate SetupPropertyExceptions.  We want to be able to test the ReadResult property when SetupProperty throws exceptions
    /// </summary>
    public class DataStoreInMemoryReaderWithSetupPropertyException : DataStoreInMemoryXmlReader
    {
        public DataStoreInMemoryReaderWithSetupPropertyException(Stream stream)
            : base(stream)
        {
        }

        protected override void SetupProperty(IBusinessObject bo, string propertyName, string propertyValue)
        {
            throw new Exception("Test property read exception");

        }
    }
}