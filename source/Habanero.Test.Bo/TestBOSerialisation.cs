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
using System.IO;
//TODO andrew 04 Jan 2011: CF: Removed Serialization
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOSerialisation
    {
        private const string _dataFileName = @"TestData\DataFile.dat";
        // ReSharper disable PossibleNullReferenceException   
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        
            string dataFileDirectoryName = Path.GetDirectoryName(_dataFileName);
            if (!Directory.Exists(dataFileDirectoryName))
            {
                Directory.CreateDirectory(dataFileDirectoryName);
            }
        }
        // ReSharper restore PossibleNullReferenceException
        [TestFixtureTearDown]
        public void TearDownTestFixture()
        {
            string dataFileDirectoryName = Path.GetDirectoryName(_dataFileName);
            if (Directory.Exists(dataFileDirectoryName))
            {
                Directory.Delete(dataFileDirectoryName, true);
            }
        }

        //TODO andrew 04 Jan 2011: CF: Does not support binaryformatter
//        //Any class that might be serialized must be marked with the SerializableAttribute. 
//        //If a class needs to control its serialization process, it can implement the ISerializable interface. 
//        //The Formatter calls the GetObjectData at serialization time and populates the supplied 
//        //  SerializationInfo with all the data required to represent the object. 
//        //The Formatter creates a SerializationInfo with the type of the object in the graph. 
//        //  Objects that need to send proxies for themselves can use the FullTypeName and AssemblyName 
//        //  methods on SerializationInfo to change the transmitted information.
//        [Test]
//        public void Test_SerialiseBusinessObject()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            MyBO.LoadClassDefs_OneProp();
//            BusinessObject myBO = new MyBO();
//            const string dataFile = _dataFileName;
//            File.Delete(dataFile);
            
//            // Construct a BinaryFormatter and use it 
//            // to serialize the data to the stream.
//            BinaryFormatter formatter = new BinaryFormatter();
//            //---------------Assert Precondition----------------
//            AssertFileDoesNotExist(dataFile);
//            //---------------Execute Test ----------------------
//            // Serialize the BO.
//            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
//            {
//                formatter.Serialize(fs, myBO);
//            }
//            //---------------Test Result -----------------------
//            AssertFileHasBeenCreated(dataFile);
//        }

//        [Test]
//        public void Test_DeSerialiseBusinessObject()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            MyBO.LoadClassDefs_OneProp();
//            MyBO myBO = new MyBO();
//            const string dataFile = _dataFileName;
//            File.Delete(dataFile);

//            // Construct a BinaryFormatter and use it 
//            // to serialize the data to the stream.
//            IFormatter formatter = new BinaryFormatter();
//            // Serialize the BO.
//            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
//            {
//                formatter.Serialize(fs, myBO);
//            }
////            BusinessObjectManager.Instance = new BusinessObjectManager();
//            BORegistry.BusinessObjectManager = new BusinessObjectManager();
//            //---------------Assert Precondition----------------
//            AssertFileHasBeenCreated(dataFile);
//            //---------------Execute Test ----------------------
//            MyBO deserialisedBO;
//            using (Stream stream = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                deserialisedBO = (MyBO) formatter.Deserialize(stream);
//            }
//            //---------------Test Result -----------------------
//            Assert.AreEqual(myBO.MyBoID, deserialisedBO.MyBoID);
//            Assert.AreEqual( myBO.Status, deserialisedBO.Status);
//        }

//        [Test]
//        public void Test_SerialiseBusinessObjectCollection()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            MyBO.LoadClassDefs_OneProp();
//            IBusinessObjectCollection myBOCol = GetMyBOColWithTwoBOs();
//            const string dataFile = _dataFileName;
//            File.Delete(dataFile);
            
//            // Construct a BinaryFormatter and use it 
//            // to serialize the data to the stream.
//            BinaryFormatter formatter = new BinaryFormatter();
//            //---------------Assert Precondition----------------
//            AssertFileDoesNotExist(dataFile);
//            Assert.AreEqual(2, myBOCol.Count);
//            //---------------Execute Test ----------------------
//            // Serialize the BO.
//            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
//            {
//                formatter.Serialize(fs, myBOCol);
//            }
//            //---------------Test Result -----------------------
//            AssertFileHasBeenCreated(dataFile);
//        }

//        [Test]
//        public void Test_DeSerialiseBusinessObjectCollection()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            MyBO.LoadClassDefs_OneProp();
//            IBusinessObjectCollection myBOCol = GetMyBOColWithTwoBOs();
//            const string dataFile = _dataFileName;
//            File.Delete(dataFile);

//            // Construct a BinaryFormatter and use it 
//            // to serialize the data to the stream.
//            IFormatter formatter = new BinaryFormatter();
//            // Serialize the BO.
//            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
//            {
//                formatter.Serialize(fs, myBOCol);
//            }
////            BusinessObjectManager.Instance = new BusinessObjectManager();
//            BORegistry.BusinessObjectManager = new BusinessObjectManager();
//            //---------------Assert Precondition----------------
//            AssertFileHasBeenCreated(dataFile);
//            Assert.AreEqual(2, myBOCol.Count);
//            //---------------Execute Test ----------------------
//            IBusinessObjectCollection deserialisedBOCol;
//            using (Stream stream = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                deserialisedBOCol = (IBusinessObjectCollection)formatter.Deserialize(stream);
//            }
//            //---------------Test Result -----------------------
//            Assert.AreEqual(2, deserialisedBOCol.Count);
//            Assert.AreEqual(2, deserialisedBOCol.CreatedBusinessObjects.Count);
//        } 

//        //TODO Brett 18 Feb 2009: Tests for added, mark for deleted, removed etc.
//        //TODO Brett 18 Feb 2009: Tests for serialising the full state of the boprop.

//        [Test]
//        public void Test_SerialiseAnObject()
//        {
//            //---------------Set up test pack-------------------
//            TrySerialisable trySerial = new TrySerialisable();
//            const string dataFile = _dataFileName;
//            // Construct a BinaryFormatter and use it 
//            // to serialize the data to the stream.
//            BinaryFormatter formatter = new BinaryFormatter();
//            //---------------Assert Precondition----------------
//            //---------------Execute Test ----------------------
//            // Serialize the BO.
//            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
//            {
//                formatter.Serialize(fs, trySerial);
//            }
//            //---------------Test Result -----------------------
//            AssertFileHasBeenCreated(dataFile);
//        }

        private static void AssertFileHasBeenCreated(string fullFileName)
        {
            Assert.IsTrue(File.Exists(fullFileName), "The file : " + fullFileName + " should have been created");
        }

        private static void AssertFileDoesNotExist(string fullFileName)
        {
            Assert.IsFalse(File.Exists(fullFileName), "The file : " + fullFileName + " should not exist");
        }

        private static IBusinessObjectCollection GetMyBOColWithTwoBOs()
        {
            BusinessObject myBO = new MyBO();
            BusinessObject myBO2 = new MyBO();
            IBusinessObjectCollection myBOCol = new BusinessObjectCollection<MyBO>();
            myBOCol.Add(myBO);
            myBOCol.Add(myBO2);
            return myBOCol;
        }
    }

    //TODO andrew 04 Jan 2011: CF: Removed Serialization codee
    //[Serializable]
    //internal class TrySerialisable
    //{
    //    public void GetObjectData(SerializationInfo info, StreamingContext context)
    //    {
    //        info.AddValue("someValue", 1);
    //    }
    //}
}