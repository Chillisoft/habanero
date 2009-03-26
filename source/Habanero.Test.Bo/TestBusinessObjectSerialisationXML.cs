using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectSerialisationXML
    {
        [Ignore]//deserialise not implemented
        [Test]
        public void Test_XmlSerialiseDeserialise_ReturnsCorrectType()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            //IFormatter formatter = new BinaryFormatter();
            XmlSerializer xs = new XmlSerializer(typeof(Person));
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Execute Test ----------------------
            //formatter.Serialize(memoryStream, originalPerson);
            xs.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Object deserialisedPerson = xs.Deserialize(memoryStream);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(Person), deserialisedPerson);
        }

        [Ignore]//deserialise not implemented
        [Test]
        public void Test_XmlSerialiseDeserialise_ReturnsCorrectClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            ClassDef classDef = Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            XmlSerializer xs = new XmlSerializer(typeof(Person));
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Execute Test ----------------------
            xs.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person)xs.Deserialize(memoryStream);
            //---------------Test Result -----------------------
            Assert.AreSame(classDef, deserialisedPerson.ClassDef);
        }

        [Ignore]//deserialise not implemented
        [Test]
        public void Test_XmlSerialiseAndDeserialise()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            XmlSerializer xs = new XmlSerializer(typeof(Person));
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Execute Test ----------------------
            xs.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person)xs.Deserialize(memoryStream);
            //---------------Test Result -----------------------
            Assert.AreNotSame(deserialisedPerson, originalPerson);
            AssertPersonsAreEqual(originalPerson, deserialisedPerson);
            //not deserialisng
            Assert.AreEqual(originalPerson.FirstName,deserialisedPerson.FirstName);
        }

        [Ignore]//deserialise not implemented
        [Test]
        public void Test_Serialise_AddBOPropAndDeserialise()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            ClassDef personClassDef = Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            XmlSerializer xs = new XmlSerializer(typeof(Person));
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            xs.Serialize(memoryStream, originalPerson);
            const string newpropertyName = "NewProperty";
            const string defaultValue = "some Default";
            personClassDef.PropDefcol.Add(new PropDef(newpropertyName, typeof(string), PropReadWriteRule.ReadWrite, defaultValue));

            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person)xs.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreNotSame(deserialisedPerson, originalPerson);
            AssertPersonsAreEqual(originalPerson, deserialisedPerson);
            Assert.AreEqual(defaultValue, deserialisedPerson.GetPropertyValue(newpropertyName));
        }

        [Ignore]//deserialise not implemented
        [Test]
        public void Test_SerialiseDeserialise_ReturnsCorrectStatus()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            XmlSerializer xs = new XmlSerializer(typeof(Person));
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            xs.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person)xs.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPerson.Status, deserialisedPerson.Status);
            Assert.AreSame(deserialisedPerson, deserialisedPerson.Status.BusinessObject);
        }

        [Ignore]//deserialise not implemented
        [Test]
        public void TestSerialiseDeserialiseBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            BusinessObjectCollection<Person> originalPeople = new BusinessObjectCollection<Person>();
            Person person1 = Person.CreateSavedPerson();
            originalPeople.Add(person1);
            Person person2 = Person.CreateSavedPerson();
            originalPeople.Add(person2);
            Person person3 = Person.CreateSavedPerson();
            originalPeople.Add(person3);

            XmlSerializer xs = new XmlSerializer(typeof(BusinessObjectCollection<Person>));
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            xs.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            //this line causes test to hang/infinite loop?
            BusinessObjectCollection<Person> deserialisedPeople = null;
            //(BusinessObjectCollection<Person>)xs.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);

            Assert.AreNotSame(originalPeople, deserialisedPeople);
            AssertPersonsAreEqual(deserialisedPeople[0], originalPeople[0]);
            AssertPersonsAreEqual(deserialisedPeople[1], originalPeople[1]);
            AssertPersonsAreEqual(deserialisedPeople[2], originalPeople[2]);
        }

        [Ignore]//deserialise not implemented
        [Test]
        public void TestSerialiseDeserialiseBusinessObjectCollection_CreatedBusObjAreIncluded()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            BusinessObjectCollection<Person> originalPeople = new BusinessObjectCollection<Person>();
            originalPeople.CreateBusinessObject();
            originalPeople.CreateBusinessObject();

            XmlSerializer xs = new XmlSerializer(typeof(BusinessObjectCollection<Person>));
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            xs.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            //this line causes test to hang/infinite loop?
            BusinessObjectCollection<Person> deserialisedPeople = null;
            //(BusinessObjectCollection<Person>)xs.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreEqual(originalPeople.CreatedBusinessObjects.Count, deserialisedPeople.CreatedBusinessObjects.Count);

            AssertPersonsAreEqual(deserialisedPeople.CreatedBusinessObjects[0], originalPeople.CreatedBusinessObjects[0]);
            AssertPersonsAreEqual(deserialisedPeople.CreatedBusinessObjects[1], originalPeople.CreatedBusinessObjects[1]);
        }

        //this might go to the implementing class' tests
        [Test]
        public void Test_CreateXmlFile()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();

            //MyBO.LoadClassDefs_Integer_PrimaryKey();

            BusinessObject myBO = new MyBO();
            
            const string fileName = @"C:\xmlBOs.xml";
            File.Delete(fileName);

            //---------------Assert Precondition----------------
            AssertFileDoesNotExist(fileName);
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(MyBO));
            StreamWriter sw = new StreamWriter(fileName);
            xs.Serialize(sw, myBO);
            sw.Close();
            //---------------Test Result -----------------------
            AssertFileHasBeenCreated(fileName);
        }

        [Ignore]//deserialise not implemented
        [Test]
        public void Test_ReadXmlFile()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            
            const string fileName = @"C:\xmlBOs.xml";
            File.Delete(fileName);


            XmlSerializer xs = new XmlSerializer(typeof(MyBO));
            //using (FileStream fs = new FileStream(fileName, FileMode.Create))
            //{
            StreamWriter sw = new StreamWriter(fileName);  
            xs.Serialize(sw, myBO);
            //}
            sw.Close();
            BusinessObjectManager.Instance = new BusinessObjectManager();
            //---------------Assert Precondition----------------
            AssertFileHasBeenCreated(fileName);
            //---------------Execute Test ----------------------
            MyBO deserialisedBO;
            //using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            StreamReader sr=new StreamReader(fileName);
            //this line pulls out a magic guid- not the one in the file?- deserialize had not been implemented
                deserialisedBO = (MyBO)xs.Deserialize(sr);
            //}
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.MyBoID, deserialisedBO.MyBoID);
            //Assert.AreEqual(myBO.Status, deserialisedBO.Status);
        }

        [Test]
        public void Test_WriteXmlFile_BusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDefWithDefault("default prop");
            IBusinessObjectCollection myBOCol = GetMyBOColWithTwoBOs();
            const string dataFile = @"C:\xmlBOCol.xml";
            File.Delete(dataFile);

            XmlSerializer xs = new XmlSerializer(typeof(BusinessObjectCollection<MyBO>));
            //---------------Assert Precondition----------------
            AssertFileDoesNotExist(dataFile);
            Assert.AreEqual(2, myBOCol.Count);
            //---------------Execute Test ----------------------
            // Serialize the BO.
            using (FileStream fs = new FileStream(dataFile, FileMode.Create))
            {
                xs.Serialize(fs, myBOCol);
            }
            //---------------Test Result -----------------------
            AssertFileHasBeenCreated(dataFile);
        }

        private static void AssertPersonsAreEqual(IBusinessObject originalPerson, IBusinessObject deserialisedPerson)
        {
            foreach (IBOProp prop in originalPerson.Props)
            {
                Assert.AreEqual(prop.Value, deserialisedPerson.GetPropertyValue(prop.PropertyName));
            }
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
    //[Serializable]
    //internal class TrySerialisable
    //{
    //    public void GetObjectData(SerializationInfo info, StreamingContext context)
    //    {
    //        info.AddValue("someValue", 1);
    //    }
    //}

}