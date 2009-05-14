using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOSerialisation
    {
        //Any class that might be serialized must be marked with the SerializableAttribute. 
        //If a class needs to control its serialization process, it can implement the ISerializable interface. 
        //The Formatter calls the GetObjectData at serialization time and populates the supplied 
        //  SerializationInfo with all the data required to represent the object. 
        //The Formatter creates a SerializationInfo with the type of the object in the graph. 
        //  Objects that need to send proxies for themselves can use the FullTypeName and AssemblyName 
        //  methods on SerializationInfo to change the transmitted information.
        [Test]
        public void Test_SerialiseBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefs_OneProp();
            BusinessObject myBO = new MyBO();
            const string dataFile = @"C:\DataFile.dat";
            File.Delete(dataFile);
            
            // Construct a BinaryFormatter and use it 
            // to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            //---------------Assert Precondition----------------
            AssertFileDoesNotExist(dataFile);
            //---------------Execute Test ----------------------
            // Serialize the BO.
            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
            {
                formatter.Serialize(fs, myBO);
            }
            //---------------Test Result -----------------------
            AssertFileHasBeenCreated(dataFile);
        }

        [Test]
        public void Test_DeSerialiseBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefs_OneProp();
            MyBO myBO = new MyBO();
            const string dataFile = @"C:\DataFile.dat";
            File.Delete(dataFile);

            // Construct a BinaryFormatter and use it 
            // to serialize the data to the stream.
            IFormatter formatter = new BinaryFormatter();
            // Serialize the BO.
            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
            {
                formatter.Serialize(fs, myBO);
            }
            BusinessObjectManager.Instance = new BusinessObjectManager();
            //---------------Assert Precondition----------------
            AssertFileHasBeenCreated(dataFile);
            //---------------Execute Test ----------------------
            MyBO deserialisedBO;
            using (Stream stream = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                deserialisedBO = (MyBO) formatter.Deserialize(stream);
            }
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.MyBoID, deserialisedBO.MyBoID);
            Assert.AreEqual( myBO.Status, deserialisedBO.Status);
        }

        [Test]
        public void Test_SerialiseBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefs_OneProp();
            IBusinessObjectCollection myBOCol = GetMyBOColWithTwoBOs();
            const string dataFile = @"C:\DataFile.dat";
            File.Delete(dataFile);
            
            // Construct a BinaryFormatter and use it 
            // to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            //---------------Assert Precondition----------------
            AssertFileDoesNotExist(dataFile);
            Assert.AreEqual(2, myBOCol.Count);
            //---------------Execute Test ----------------------
            // Serialize the BO.
            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
            {
                formatter.Serialize(fs, myBOCol);
            }
            //---------------Test Result -----------------------
            AssertFileHasBeenCreated(dataFile);
        }

        [Test]
        public void Test_DeSerialiseBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefs_OneProp();
            IBusinessObjectCollection myBOCol = GetMyBOColWithTwoBOs();
            const string dataFile = @"C:\DataFile.dat";
            File.Delete(dataFile);

            // Construct a BinaryFormatter and use it 
            // to serialize the data to the stream.
            IFormatter formatter = new BinaryFormatter();
            // Serialize the BO.
            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
            {
                formatter.Serialize(fs, myBOCol);
            }
            BusinessObjectManager.Instance = new BusinessObjectManager();
            //---------------Assert Precondition----------------
            AssertFileHasBeenCreated(dataFile);
            Assert.AreEqual(2, myBOCol.Count);
            //---------------Execute Test ----------------------
            IBusinessObjectCollection deserialisedBOCol;
            using (Stream stream = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                deserialisedBOCol = (IBusinessObjectCollection)formatter.Deserialize(stream);
            }
            //---------------Test Result -----------------------
            Assert.AreEqual(2, deserialisedBOCol.Count);
            Assert.AreEqual(2, deserialisedBOCol.CreatedBusinessObjects.Count);
        } 

//        [Test]
//        public void Test_SerialiseBOPrimaryKey()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            MyBO.LoadClassDefs_OneProp();
//            MyBO myBO = new MyBO();
//            const string dataFile = @"C:\DataFile.dat";
//            File.Delete(dataFile);
//            
//            // Construct a BinaryFormatter and use it 
//            // to serialize the data to the stream.
//            BinaryFormatter formatter = new BinaryFormatter();
//            //---------------Assert Precondition----------------
//            AssertFileDoesNotExist(dataFile);
//            //---------------Execute Test ----------------------
//            // Serialize the BO.
//            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
//            {
//                formatter.Serialize(fs, myBO.ID);
//            }
//            //---------------Test Result -----------------------
//            AssertFileHasBeenCreated(dataFile);
//        }

//        [Test]
//        public void Test_DeSerialiseBOObjectID()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            MyBO.LoadClassDefs_OneProp();
//            MyBO myBO = new MyBO();
//            const string dataFile = @"C:\DataFile.dat";
//            File.Delete(dataFile);
//
//            // Construct a BinaryFormatter and use it 
//            // to serialize the data to the stream.
//            IFormatter formatter = new BinaryFormatter();
//            // Serialize the BO.
//            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
//            {
//                formatter.Serialize(fs, myBO.ID);
//            }
//            BusinessObjectManager.Instance = new BusinessObjectManager();
//            //---------------Assert Precondition----------------
//            AssertFileHasBeenCreated(dataFile);
//            //---------------Execute Test ----------------------
//            IPrimaryKey deserialisedPrimaryKey;
//            using (Stream stream = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                deserialisedPrimaryKey = (IPrimaryKey) formatter.Deserialize(stream);
//            }
//            //---------------Test Result -----------------------
////            Assert.AreEqual(2, deserialisedBOCol.Count);
////            Assert.AreEqual(2, deserialisedBOCol.CreatedBusinessObjects.Count);
//        }

        //TODO Brett 18 Feb 2009: Tests for added, mark for deleted, removed etc.
        //TODO Brett 18 Feb 2009: Tests for serialising the full state of the boprop.

        [Test]
        public void Test_SerialiseAnObject()
        {
            //---------------Set up test pack-------------------
            TrySerialisable trySerial = new TrySerialisable();
            const string dataFile = @"C:\DataFile.dat";
            // Construct a BinaryFormatter and use it 
            // to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            // Serialize the BO.
            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
            {
                formatter.Serialize(fs, trySerial);
            }
            //---------------Test Result -----------------------
            AssertFileHasBeenCreated(dataFile);
        }

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
    [Serializable]
    internal class TrySerialisable
    {
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("someValue", 1);
        }
    }
}