using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectSerialisation
    {
        [Test]
        public void Test_SerialiseDeserialise_ReturnsCorrectType()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Object deserialisedPerson = formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(Person),deserialisedPerson);
        }

        [Test]
        public void Test_SerialiseDeserialise_ReturnsCorrectClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef classDef = Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreSame(classDef, deserialisedPerson.ClassDef);
        }

        [Test]
        public void Test_SerialiseAndDeserialise()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person) formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreNotSame(deserialisedPerson, originalPerson);
            AssertPersonsAreEqual(originalPerson, deserialisedPerson);
        }

        private void AssertPersonsAreEqual(Person originalPerson, Person deserialisedPerson)
        {
            foreach (IBOProp prop in originalPerson.Props)
            {
                Assert.AreEqual(prop.Value, deserialisedPerson.GetPropertyValue(prop.PropertyName));
            }
        }

        [Test]
        public void Test_SerialiseDeserialise_ReturnsCorrectStatus()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPerson.Status,deserialisedPerson.Status);
        }
        
        [Test]
        public void TestSerialiseDeserialiseBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Person.LoadDefaultClassDef();
            BusinessObjectCollection<Person> originalPeople = new BusinessObjectCollection<Person>();
            Person person1 = Person.CreateSavedPerson();
            originalPeople.Add(person1);
            Person person2 = Person.CreateSavedPerson();
            originalPeople.Add(person2);
            Person person3 = Person.CreateSavedPerson();
            originalPeople.Add(person3);
            
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BusinessObjectCollection<Person> deserialisedPeople = (BusinessObjectCollection<Person>)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);

            Assert.AreNotSame(originalPeople,deserialisedPeople);
            AssertPersonsAreEqual(deserialisedPeople[0], originalPeople[0]);
            AssertPersonsAreEqual(deserialisedPeople[1], originalPeople[1]);
            AssertPersonsAreEqual(deserialisedPeople[2], originalPeople[2]);
        }

        [Test]
        public void TestSerialiseDeserialiseBusinessObjectCollection_CreatedBusObjAreIncluded()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Person.LoadDefaultClassDef();
            BusinessObjectCollection<Person> originalPeople = new BusinessObjectCollection<Person>();
            Person person1 = originalPeople.CreateBusinessObject();
            Person person2 = originalPeople.CreateBusinessObject();

            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BusinessObjectCollection<Person> deserialisedPeople = (BusinessObjectCollection<Person>)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreEqual(originalPeople.CreatedBusinessObjects.Count, deserialisedPeople.CreatedBusinessObjects.Count);

            AssertPersonsAreEqual(deserialisedPeople.CreatedBusinessObjects[0], originalPeople.CreatedBusinessObjects[0]);
            AssertPersonsAreEqual(deserialisedPeople.CreatedBusinessObjects[1], originalPeople.CreatedBusinessObjects[1]);
        }
    }
}
