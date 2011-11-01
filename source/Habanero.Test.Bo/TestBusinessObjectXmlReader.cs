#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectXmlReader
    {
        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            new Address();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerNull();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            var reader = new BusinessObjectXmlReader();
            //---------------Test Result -----------------------
            
            //---------------Tear Down -------------------------          
        }
     
        [Test]
        public void Read()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var stream = GetStreamForBusinessObject(new MyBO());
            var xmlReader = GetXmlReader(stream);
            var reader = new BusinessObjectXmlReader();
            //---------------Execute Test ----------------------
            var loadedObjects = reader.Read(xmlReader);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedObjects.Count());
        }
        
        [Test]
        public void Read_WhenPropHasBeenRemoved_ShouldReadWithoutProp()
        {
            //---------------Set up test pack-------------------
            IClassDef def = LoadMyBOClassDefsWithNoUIDefs();
            var stream = GetStreamForBusinessObject(new MyBO());
            var xmlReader = GetXmlReader(stream);
            var reader = new BusinessObjectXmlReader();
            const string propertyName = "TestProp2";
            //---------------Execute Test ----------------------
            def.PropDefcol.Remove(def.PropDefcol[propertyName]);
            var loadedObjects = reader.Read(xmlReader);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedObjects.Count());
            Assert.AreEqual(1, reader.PropertyReadExceptions.Count());
            StringAssert.Contains(propertyName, reader.PropertyReadExceptions.First());
            StringAssert.Contains("does not exist in the prop collection", reader.PropertyReadExceptions.First());
        }

        [Test]
        public void Read_ShouldLoadPropertiesCorrectly()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var savedBo = new MyBO {TestProp = TestUtil.GetRandomString(), TestProp2 = TestUtil.GetRandomString()};
            var savedDataStore = new DataStoreInMemory();
            var transactionCommitter = new TransactionCommitterInMemory(savedDataStore);
            transactionCommitter.AddBusinessObject(savedBo);
            transactionCommitter.CommitTransaction();
            var stream = GetStreamForDataStore(savedDataStore);
            var xmlReader = GetXmlReader(stream);
            var reader = new BusinessObjectXmlReader();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            var loadedObjects = reader.Read(xmlReader);
            //---------------Test Result -----------------------
            var businessObjects = loadedObjects.ToList();
            Assert.AreEqual(1, businessObjects.Count());
            var myBos = businessObjects.Select(o => (MyBO)o);
            var matchedBos = myBos.Where(bo => bo.TestProp.Equals(savedBo.TestProp));
             Assert.AreEqual(1, matchedBos.Count());
            var loadedMyBo = matchedBos.First();
            Assert.AreNotSame(savedBo, loadedMyBo);
            Assert.AreEqual(savedBo.MyBoID, loadedMyBo.MyBoID);
            Assert.AreEqual(savedBo.Props["MyBoID"].PersistedPropertyValue, loadedMyBo.Props["MyBoID"].PersistedPropertyValue);
            Assert.AreEqual(savedBo.TestProp, loadedMyBo.TestProp);
            Assert.AreEqual(savedBo.Props["TestProp"].PersistedPropertyValue, loadedMyBo.Props["TestProp"].PersistedPropertyValue);
            Assert.AreEqual(savedBo.TestProp2, loadedMyBo.TestProp2);
            Assert.AreEqual(savedBo.Props["TestProp2"].PersistedPropertyValue, loadedMyBo.Props["TestProp2"].PersistedPropertyValue);
        }

        [Test]
        public void Read_MultipleObjects()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var savedDataStore = new DataStoreInMemory();
            var bo1 = new MyBO();
            var bo2 = new Car();
            savedDataStore.Add(bo1);
            savedDataStore.Add(bo2);
            var stream = GetStreamForDataStore(savedDataStore);
            var xmlReader = GetXmlReader(stream);
            var reader = new BusinessObjectXmlReader();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, savedDataStore.Count);
            //---------------Execute Test ----------------------
            var loadedObjects = reader.Read(xmlReader);
            //---------------Test Result -----------------------
            var businessObjects = loadedObjects.ToList();
            Assert.AreEqual(2, businessObjects.Count);
            Assert.IsNotNull(businessObjects.Find(o => o.ID.Equals(bo1.ID)));
            Assert.IsNotNull(businessObjects.Find(o => o.ID.Equals(bo2.ID)));
        }

        [Test]
        public void Read_ShouldLoadObjectsAsNew()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var savedDataStore = new DataStoreInMemory();
            var savedBo = new MyBO();
            var transactionCommitter = new TransactionCommitterInMemory(savedDataStore);
            transactionCommitter.AddBusinessObject(savedBo);
            transactionCommitter.CommitTransaction();
            var stream = GetStreamForDataStore(savedDataStore);
            var xmlReader = GetXmlReader(stream);
            var reader = new BusinessObjectXmlReader();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            var loadedObjects = reader.Read(xmlReader);
            //---------------Test Result -----------------------
            var businessObjects = loadedObjects.ToList();
            Assert.AreEqual(1, businessObjects.Count);
            var loadedMyBo = (MyBO)businessObjects[0];
            Assert.AreNotSame(savedBo, loadedMyBo);
            Assert.IsTrue(loadedMyBo.Status.IsNew, "Should not be New");
            Assert.IsFalse(loadedMyBo.Status.IsDeleted, "Should not be Deleted");
        }

        private static IClassDef LoadMyBOClassDefsWithNoUIDefs()
        {
            ClassDef.ClassDefs.Clear();
            return MyBO.LoadClassDefsNoUIDef();
        }
      
        private XmlReader GetXmlReader(Stream stream)
        {
            var settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            return XmlReader.Create(stream, settings);
        }

        private MemoryStream GetStreamForBusinessObject(MyBO businessObject)
        {
            var savedDataStore = new DataStoreInMemory();
            savedDataStore.Add(businessObject);
            var writeStream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter();
            writer.Write(writeStream, savedDataStore);
            writeStream.Seek(0, SeekOrigin.Begin);
            return writeStream;
        }

        private MemoryStream GetStreamForDataStore(DataStoreInMemory dataStore)
        {
            var writeStream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter();
            writer.Write(writeStream, dataStore);
            writeStream.Seek(0, SeekOrigin.Begin);
            return writeStream;
        }

    }
}