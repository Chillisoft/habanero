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
using System.IO;
using System.Linq;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestObjectTreeXmlReader
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
            var reader = new ObjectTreeXmlReader();
            //---------------Test Result -----------------------
            
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Read_ShouldLoadObjectsAsNew_WhenNotInExistingDataStore()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var savedDataStore = new DataStoreInMemory();
            var savedBo = new MyBO();
            var transactionCommitter = new TransactionCommitterInMemory(savedDataStore);
            transactionCommitter.AddBusinessObject(savedBo);
            transactionCommitter.CommitTransaction();
            var writeStream = GetStreamForDataStore(savedDataStore);
            var reader = new ObjectTreeXmlReader();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            var loadedObjects = reader.Read(writeStream);
            //---------------Test Result -----------------------
            var businessObjects = loadedObjects.ToList();
            Assert.AreEqual(1, businessObjects.Count);
            var loadedMyBo = (MyBO)businessObjects[0];
            Assert.AreNotSame(savedBo, loadedMyBo);

            Assert.IsTrue(loadedMyBo.Status.IsNew, "Should not be New");
            Assert.IsFalse(loadedMyBo.Status.IsDeleted, "Should not be Deleted");
        }

        [Test]
        public void Read_ShouldUpdatedExistingObject_WhenFoundInDataAccessor()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var newBo = new MyBO { TestProp = "characters" };
            var stream = GetStreamForBusinessObject(newBo);
            newBo.TestProp = "oldvalue";
            newBo.Save();
            var reader = new ObjectTreeXmlReader();
            //---------------Assert Precondition----------------
            Assert.AreEqual("oldvalue", newBo.TestProp);
            //---------------Execute Test ----------------------
            var loadedObjects = reader.Read(stream);
            //---------------Test Result -----------------------
            var businessObjects = loadedObjects.ToList();
            Assert.AreEqual(1, businessObjects.Count);
            Assert.AreSame(newBo, businessObjects[0]);
            Assert.AreEqual("characters", newBo.TestProp);
        }

        [Test]
        public void Read_ShouldSet_ReadResult()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var writeStream = GetStreamForBusinessObject(new MyBO());
            var reader = new ObjectTreeXmlReader();
            //---------------Execute Test ----------------------
            var loadedObjects = reader.Read(writeStream, new BusinessObjectXmlReaderWithError());
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedObjects.Count());
            Assert.IsNotNull(reader.ReadResult);
            Assert.IsFalse(reader.ReadResult.Successful);
        }

        [Test]
        public void Read_WhenNoPropertyReadExceptions_ShouldSetReadResultSuccessfulTrue_Bug1336()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var toStream = GetStreamForBusinessObject(new MyBO());
            var reader = new ObjectTreeXmlReader();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            reader.Read(toStream);
            //---------------Test Result -----------------------
            Assert.IsNotNull(reader.ReadResult);
            Assert.IsTrue(reader.ReadResult.Successful);
        }

        [Test]
        public void Acceptance_Read_WhenCalledWithPropertyReadExceptions_ShouldSetReadResultSuccessfulFalseAndMessage_Bug1336()
        {
            //---------------Set up test pack-------------------
            var classDef = LoadMyBOClassDefsWithNoUIDefs();
            var bo = new MyBO { TestProp = "characters" };
            var propDef = classDef.PropDefcol["TestProp"];
            classDef.PropDefcol.Remove(propDef);
            var propDefWithDifferentType = propDef.Clone();

            // Change type to force property read exception
            propDefWithDifferentType.PropertyType = typeof(int);
            classDef.PropDefcol.Add(propDefWithDifferentType);
            var toStream = GetStreamForBusinessObject(bo);
            var reader = new ObjectTreeXmlReader();
            //---------------Assert Precondition----------------
            Assert.IsNull(reader.ReadResult);
            //---------------Execute Test ----------------------
            var businessObjects = reader.Read(toStream);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, businessObjects.Count());
            Assert.IsNotNull(reader.ReadResult);
            Assert.IsFalse(reader.ReadResult.Successful);
            StringAssert.Contains("An error occured when attempting to set property 'MyBO.TestProp'.", reader.ReadResult.Message);
        }

        [Test]
        public void Read_WhenCalledWithPropertyReadExceptions_ShouldSetReadResultSuccessfulFalseAndMessage_Bug1336()
        {
            //---------------Set up test pack-------------------
            LoadMyBOClassDefsWithNoUIDefs();
            var bo = new MyBO { TestProp = "characters" };
            var stream = GetStreamForBusinessObject(bo);
            var reader = new ObjectTreeXmlReader();
            var boReader = new BusinessObjectXmlReaderWithError();
            //---------------Assert Precondition----------------
            Assert.IsNull(reader.ReadResult);
            //---------------Execute Test ----------------------
            var businessObjects = reader.Read(stream, boReader);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, businessObjects.Count());
            Assert.IsNotNull(reader.ReadResult);
            Assert.IsFalse(reader.ReadResult.Successful);
            StringAssert.Contains("An error occured when attempting to set property 'MyBO.MyBoID'.", reader.ReadResult.Message);
            StringAssert.Contains("An error occured when attempting to set property 'MyBO.TestProp'.", reader.ReadResult.Message);
            StringAssert.Contains("An error occured when attempting to set property 'MyBO.TestProp2'.", reader.ReadResult.Message);
        }

        [Test]
        public void Read_FromString()
        {
            //---------------Set up test pack-------------------
            var dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            var writer = new DataStoreInMemoryXmlWriter();
            var sb = new StringBuilder();
            writer.Write(sb, dataStore);
            var xml = sb.ToString();

            var reader = new ObjectTreeXmlReader();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.Greater(xml.Length, 100);
            //---------------Execute Test ----------------------
            var businessObjects = reader.Read(xml);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, businessObjects.Count());
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

        private static IClassDef LoadMyBOClassDefsWithNoUIDefs()
        {
            ClassDef.ClassDefs.Clear();
            return MyBO.LoadClassDefsNoUIDef();
        }
        // ReSharper restore InconsistentNaming

    }
}