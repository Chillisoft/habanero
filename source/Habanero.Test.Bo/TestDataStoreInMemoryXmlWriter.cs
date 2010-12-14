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
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataStoreInMemoryXmlWriter
    {
        
        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            new Address();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(new MemoryStream());
            //---------------Test Result -----------------------
            
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Write()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            MemoryStream stream = new MemoryStream();
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(stream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(dataStore);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, stream.Length);
        }

        [Test]
        public void Test_Write_WithXmlWriterSettings()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Auto;
            xmlWriterSettings.NewLineOnAttributes = true;
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(stream, xmlWriterSettings);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(dataStore);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, stream.Length);
        }

        [Test]
        public void Test_Write_WithDictionary()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            Dictionary<Guid, IBusinessObject> dictionary = dataStore.AllObjects;
            MemoryStream stream = new MemoryStream();
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(stream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(dictionary);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, stream.Length);
        }

        [Test]
        public void Test_Write_WithDictionary_WithXmlWriterSettings()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            Dictionary<Guid, IBusinessObject> dictionary = dataStore.AllObjects;
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Auto;
            xmlWriterSettings.NewLineOnAttributes = true;
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(stream, xmlWriterSettings);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(dictionary);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, stream.Length);
        }

        [Test]
        public void Test_Read()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefsNoUIDef();
            DataStoreInMemory savedDataStore = new DataStoreInMemory();
            savedDataStore.Add(new MyBO());
            MemoryStream writeStream = new MemoryStream();
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            DataStoreInMemory loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            DataStoreInMemoryXmlReader reader = new DataStoreInMemoryXmlReader(writeStream);
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
            ClassDef.ClassDefs.Clear();
            IClassDef def = MyBO.LoadClassDefsNoUIDef();
            DataStoreInMemory savedDataStore = new DataStoreInMemory();
            savedDataStore.Add(new MyBO());
            MemoryStream writeStream = new MemoryStream();
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            DataStoreInMemory loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            DataStoreInMemoryXmlReader reader = new DataStoreInMemoryXmlReader(writeStream);
            
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
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefsNoUIDef();
            DataStoreInMemory savedDataStore = new DataStoreInMemory();
            MyBO savedBO = new MyBO();
            savedBO.TestProp = TestUtil.GetRandomString();
            savedBO.TestProp2 = TestUtil.GetRandomString();
            TransactionCommitterInMemory transactionCommitter = new TransactionCommitterInMemory(savedDataStore);
            transactionCommitter.AddBusinessObject(savedBO);
            transactionCommitter.CommitTransaction();
            MemoryStream writeStream = new MemoryStream();
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            DataStoreInMemory loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            DataStoreInMemoryXmlReader reader = new DataStoreInMemoryXmlReader(writeStream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
            IBusinessObject loadedBO;
            bool success = loadedDataStore.AllObjects.TryGetValue(savedBO.MyBoID.Value, out loadedBO);
            Assert.IsTrue(success);
            Assert.IsNotNull(loadedBO);
            Assert.IsInstanceOf(typeof(MyBO), loadedBO);
            MyBO loadedMyBO = (MyBO) loadedBO;
            Assert.AreNotSame(savedBO, loadedMyBO);
            Assert.AreEqual(savedBO.MyBoID, loadedMyBO.MyBoID);
            Assert.AreEqual(savedBO.Props["MyBoID"].PersistedPropertyValue, loadedMyBO.Props["MyBoID"].PersistedPropertyValue);
            Assert.AreEqual(savedBO.TestProp, loadedMyBO.TestProp);
            Assert.AreEqual(savedBO.Props["TestProp"].PersistedPropertyValue, loadedMyBO.Props["TestProp"].PersistedPropertyValue);
            Assert.AreEqual(savedBO.TestProp2, loadedMyBO.TestProp2);
            Assert.AreEqual(savedBO.Props["TestProp2"].PersistedPropertyValue, loadedMyBO.Props["TestProp2"].PersistedPropertyValue);
        }

        [Test]
        public void Test_ReadWrite_MultipleObjects()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefsNoUIDef();
            DataStoreInMemory savedDataStore = new DataStoreInMemory();
            MyBO bo1 = new MyBO();
            Car bo2 = new Car();
            savedDataStore.Add(bo1);
            savedDataStore.Add(bo2);
            MemoryStream writeStream = new MemoryStream();
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            DataStoreInMemory loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            DataStoreInMemoryXmlReader reader = new DataStoreInMemoryXmlReader(writeStream);
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
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefsNoUIDef();
            DataStoreInMemory savedDataStore = new DataStoreInMemory();
            MyBO savedBO = new MyBO();
            TransactionCommitterInMemory transactionCommitter = new TransactionCommitterInMemory(savedDataStore);
            transactionCommitter.AddBusinessObject(savedBO);
            transactionCommitter.CommitTransaction();
            MemoryStream writeStream = new MemoryStream();
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            DataStoreInMemory loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            DataStoreInMemoryXmlReader reader = new DataStoreInMemoryXmlReader(writeStream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
            IBusinessObject loadedBO;
            bool success = loadedDataStore.AllObjects.TryGetValue(savedBO.MyBoID.GetValueOrDefault(), out loadedBO);
            Assert.IsTrue(success);
            Assert.IsNotNull(loadedBO);
            Assert.IsInstanceOf(typeof(MyBO), loadedBO);
            MyBO loadedMyBO = (MyBO)loadedBO;
            Assert.AreNotSame(savedBO, loadedMyBO);

            Assert.IsFalse(loadedBO.Status.IsNew, "Should not be New");
            Assert.IsFalse(loadedBO.Status.IsDeleted, "Should not be Deleted");
            Assert.IsFalse(loadedBO.Status.IsDirty, "Should not be Dirty");
            Assert.IsFalse(loadedBO.Status.IsEditing, "Should not be Editing");
        }

        [Test]
        public void Test_Read_WhenCalled_ShouldSet_ReadResult()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefsNoUIDef();
            DataStoreInMemory savedDataStore = new DataStoreInMemory();
            savedDataStore.Add(new MyBO());
            MemoryStream writeStream = new MemoryStream();
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            DataStoreInMemory loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            DataStoreInMemoryXmlReader reader = new DataStoreInMemoryXmlReaderSpy(writeStream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
            Assert.IsNotNull(reader.ReadResult);
        }

        [Test]
        public void Test_Read_WhenCalledAndNoPropertyReadExceptions_ShouldSet_ReadResult_Successful_True()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefsNoUIDef();
            DataStoreInMemory savedDataStore = new DataStoreInMemory();
            savedDataStore.Add(new MyBO());
            MemoryStream writeStream = new MemoryStream();
            DataStoreInMemoryXmlWriter writer = new DataStoreInMemoryXmlWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            DataStoreInMemory loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            DataStoreInMemoryXmlReaderSpy reader = new DataStoreInMemoryXmlReaderSpy(writeStream);
            reader.SetupPropertyShouldThrowException = false;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
            Assert.IsNotNull(reader.ReadResult);
            Assert.IsTrue(reader.ReadResult.Successful);
        }
    }

    public class DataStoreInMemoryXmlReaderSpy : DataStoreInMemoryXmlReader
    {
        public DataStoreInMemoryXmlReaderSpy(Stream stream) : base(stream)
        {
            SetupPropertyShouldThrowException = false;
        }

        public bool SetupPropertyShouldThrowException { get; set; }
        
        protected override void SetupProperty(IBusinessObject bo, string propertyName, string propertyValue)
        {
            if (SetupPropertyShouldThrowException) throw new Exception("Test property read exception");
            
        }
    }
}