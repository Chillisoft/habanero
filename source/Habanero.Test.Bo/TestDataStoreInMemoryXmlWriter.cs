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
using System.Text;
using System.Xml;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    // ReSharper disable InconsistentNaming
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
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            var writer = new DataStoreInMemoryXmlWriter();
            //---------------Test Result -----------------------
            
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Write()
        {
            //---------------Set up test pack-------------------
            var dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            var stream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(stream, dataStore);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, stream.Length);
        }

        [Test]
        public void Write_WithXmlWriterSettings()
        {
            //---------------Set up test pack-------------------
            var dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            var stream = new MemoryStream();
            var xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Auto;
            xmlWriterSettings.NewLineOnAttributes = true;
            var writer = new DataStoreInMemoryXmlWriter(xmlWriterSettings);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(stream, dataStore);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, stream.Length);
        }

        [Test]
        public void Write_WithDictionary()
        {
            //---------------Set up test pack-------------------
            var dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            var dictionary = dataStore.AllObjects;
            var stream = new MemoryStream();
            var writer = new DataStoreInMemoryXmlWriter();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(stream, dictionary);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, stream.Length);
        }

        [Test]
        public void Write_WithDictionary_WithXmlWriterSettings()
        {
            //---------------Set up test pack-------------------
            var dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            var dictionary = dataStore.AllObjects;
            var stream = new MemoryStream();
            var xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Auto;
            xmlWriterSettings.NewLineOnAttributes = true;
            var writer = new DataStoreInMemoryXmlWriter(xmlWriterSettings);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(stream, dictionary);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, stream.Length);
        }

        [Test]
        public void Write_ToString()
        {
            //---------------Set up test pack-------------------
            var dataStore = new DataStoreInMemory();
            dataStore.Add(new Car());
            var writer = new DataStoreInMemoryXmlWriter();
            var sb = new StringBuilder();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreEqual(0, sb.Length);
            //---------------Execute Test ----------------------
            writer.Write(sb, dataStore);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, sb.Length);
        }

    }


}