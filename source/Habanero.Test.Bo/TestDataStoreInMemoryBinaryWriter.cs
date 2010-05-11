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
using System.IO;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataStoreInMemoryBinaryWriter
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
            DataStoreInMemoryBinaryWriter writer = new DataStoreInMemoryBinaryWriter(new MemoryStream());
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
            DataStoreInMemoryBinaryWriter writer = new DataStoreInMemoryBinaryWriter(stream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(dataStore);
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
            DataStoreInMemoryBinaryWriter writer = new DataStoreInMemoryBinaryWriter(writeStream);
            writer.Write(savedDataStore);
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            DataStoreInMemory loadedDataStore = new DataStoreInMemory();
            writeStream.Seek(0, SeekOrigin.Begin);
            DataStoreInMemoryBinaryReader reader = new DataStoreInMemoryBinaryReader(writeStream);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, savedDataStore.Count);
            //---------------Execute Test ----------------------
            loadedDataStore.AllObjects = reader.Read();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedDataStore.Count);
        }
    }

}