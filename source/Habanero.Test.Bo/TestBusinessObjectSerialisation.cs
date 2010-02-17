// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Object deserialisedPerson = formatter.Deserialize(memoryStream);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(Person),deserialisedPerson);
        }

        [Test]
        public void Test_SerialiseDeserialise_ReturnsCorrectClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            IClassDef classDef = Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

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
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person) formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreNotSame(deserialisedPerson, originalPerson);
            AssertPersonsAreEqual(originalPerson, deserialisedPerson);
        }

        [Test]
        public void Test_Serialise_AddBOPropAndDeserialise()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            IClassDef personClassDef = Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPerson);
            const string newpropertyName = "NewProperty";
            const string defaultValue = "some Default";
            personClassDef.PropDefcol.Add(new PropDef(newpropertyName, typeof(string),PropReadWriteRule.ReadWrite, defaultValue));

            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person) formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreNotSame(deserialisedPerson, originalPerson);
            AssertPersonsAreEqual(originalPerson, deserialisedPerson);
            Assert.AreEqual(defaultValue, deserialisedPerson.GetPropertyValue(newpropertyName));
        }

        private static void AssertPersonsAreEqual(IBusinessObject originalPerson, IBusinessObject deserialisedPerson)
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
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPerson.Status,deserialisedPerson.Status);
            Assert.AreSame(deserialisedPerson, deserialisedPerson.Status.BusinessObject);
        }

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
            
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

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
        public void TestSerialiseDeserialiseBusinessObjectCollection_EventsAreSetUp()
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

            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            bool eventFired = false;

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BusinessObjectCollection<Person> deserialisedPeople = (BusinessObjectCollection<Person>)formatter.Deserialize(memoryStream);
            deserialisedPeople.BusinessObjectPropertyUpdated += (sender, args) => eventFired = true;
            deserialisedPeople[0].FirstName = "new firstname";

            //---------------Test Result -----------------------
            Assert.IsTrue(eventFired);
   
        }

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

            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Assert PreConditions---------------       
            Assert.AreEqual(2, originalPeople.CreatedBusinessObjects.Count);
            Assert.AreEqual(2, originalPeople.Count);

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BusinessObjectCollection<Person> deserialisedPeople = (BusinessObjectCollection<Person>)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreEqual(originalPeople.CreatedBusinessObjects.Count, deserialisedPeople.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.PersistedBusinessObjects.Count, deserialisedPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.AddedBusinessObjects.Count, deserialisedPeople.AddedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.RemovedBusinessObjects.Count, deserialisedPeople.RemovedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.MarkedForDeleteBusinessObjects.Count, deserialisedPeople.MarkedForDeleteBusinessObjects.Count);

            AssertPersonsAreEqual(deserialisedPeople.CreatedBusinessObjects[0], originalPeople.CreatedBusinessObjects[0]);
            AssertPersonsAreEqual(deserialisedPeople.CreatedBusinessObjects[1], originalPeople.CreatedBusinessObjects[1]);
        }

        [Test]
        public void TestSerialiseDeserialiseBusinessObjectCollection_PersistedAreIncluded()
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
            originalPeople.SaveAll();

            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            
            //---------------Assert PreConditions---------------       
            Assert.AreEqual(2, originalPeople.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BusinessObjectCollection<Person> deserialisedPeople = (BusinessObjectCollection<Person>)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreEqual(originalPeople.PersistedBusinessObjects.Count, deserialisedPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.CreatedBusinessObjects.Count, deserialisedPeople.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.AddedBusinessObjects.Count, deserialisedPeople.AddedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.RemovedBusinessObjects.Count, deserialisedPeople.RemovedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.MarkedForDeleteBusinessObjects.Count, deserialisedPeople.MarkedForDeleteBusinessObjects.Count);

            AssertPersonsAreEqual(deserialisedPeople.PersistedBusinessObjects[0], originalPeople.PersistedBusinessObjects[0]);
            AssertPersonsAreEqual(deserialisedPeople.PersistedBusinessObjects[1], originalPeople.PersistedBusinessObjects[1]);


        }

        [Test]
        public void TestSerialiseDeserialiseBusinessObjectCollection_AddedAreIncluded()
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
            originalPeople.SaveAll();

            Person anotherPerson = new Person();
            anotherPerson.Save();
            originalPeople.Add(anotherPerson);

            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Assert PreConditions---------------       
            Assert.AreEqual(1, originalPeople.AddedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BusinessObjectCollection<Person> deserialisedPeople = (BusinessObjectCollection<Person>)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreEqual(originalPeople.PersistedBusinessObjects.Count, deserialisedPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.CreatedBusinessObjects.Count, deserialisedPeople.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.AddedBusinessObjects.Count, deserialisedPeople.AddedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.RemovedBusinessObjects.Count, deserialisedPeople.RemovedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.MarkedForDeleteBusinessObjects.Count, deserialisedPeople.MarkedForDeleteBusinessObjects.Count);

            AssertPersonsAreEqual(deserialisedPeople.AddedBusinessObjects[0], originalPeople.AddedBusinessObjects[0]);

        }

        [Test]
        public void TestSerialiseDeserialiseBusinessObjectCollection_RemovedAreIncluded()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            BusinessObjectCollection<Person> originalPeople = new BusinessObjectCollection<Person>();
            Person personRemoved = originalPeople.CreateBusinessObject();
            originalPeople.CreateBusinessObject();
            originalPeople.SaveAll();

            originalPeople.Remove(personRemoved);

            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Assert PreConditions---------------       
            Assert.AreEqual(1, originalPeople.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, originalPeople.Count);

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BusinessObjectCollection<Person> deserialisedPeople = (BusinessObjectCollection<Person>)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreEqual(originalPeople.PersistedBusinessObjects.Count, deserialisedPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.CreatedBusinessObjects.Count, deserialisedPeople.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.AddedBusinessObjects.Count, deserialisedPeople.AddedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.RemovedBusinessObjects.Count, deserialisedPeople.RemovedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.MarkedForDeleteBusinessObjects.Count, deserialisedPeople.MarkedForDeleteBusinessObjects.Count);

            AssertPersonsAreEqual(deserialisedPeople.RemovedBusinessObjects[0], originalPeople.RemovedBusinessObjects[0]);

        }

        [Test]
        public void TestSerialiseDeserialiseBusinessObjectCollection_MarkedForDeleteAreIncluded()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            BusinessObjectCollection<Person> originalPeople = new BusinessObjectCollection<Person>();
            Person personToDelete = originalPeople.CreateBusinessObject();
            originalPeople.CreateBusinessObject();
            originalPeople.SaveAll();

            personToDelete.MarkForDelete();

            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Assert PreConditions---------------       
            Assert.AreEqual(1, originalPeople.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, originalPeople.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, originalPeople.Count);

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BusinessObjectCollection<Person> deserialisedPeople = (BusinessObjectCollection<Person>)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreEqual(originalPeople.PersistedBusinessObjects.Count, deserialisedPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.CreatedBusinessObjects.Count, deserialisedPeople.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.AddedBusinessObjects.Count, deserialisedPeople.AddedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.RemovedBusinessObjects.Count, deserialisedPeople.RemovedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.MarkedForDeleteBusinessObjects.Count, deserialisedPeople.MarkedForDeleteBusinessObjects.Count);

            AssertPersonsAreEqual(deserialisedPeople.MarkedForDeleteBusinessObjects[0], originalPeople.MarkedForDeleteBusinessObjects[0]);

        }

        [Test]
        public void TestSerialiseDeserialiseBusinessObjectCollection_HavingAllCollections()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            BusinessObjectCollection<Person> originalPeople = new BusinessObjectCollection<Person>();
            Person persistedPerson = originalPeople.CreateBusinessObject();
            Person deletedPerson = originalPeople.CreateBusinessObject();
            Person removedPerson = originalPeople.CreateBusinessObject();
            originalPeople.SaveAll();

            originalPeople.Remove(removedPerson);
            deletedPerson.MarkForDelete();
            Person createdPerson = originalPeople.CreateBusinessObject();
            Person addedPerson = new Person();
            addedPerson.Save();
            originalPeople.Add(addedPerson);

            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Assert PreConditions---------------       
            Assert.AreEqual(1, originalPeople.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(1, originalPeople.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, originalPeople.AddedBusinessObjects.Count);
            Assert.AreEqual(3, originalPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, originalPeople.CreatedBusinessObjects.Count);
            Assert.AreEqual(3, originalPeople.Count);

            //---------------Execute Test ----------------------
            formatter.Serialize(memoryStream, originalPeople);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BusinessObjectCollection<Person> deserialisedPeople = (BusinessObjectCollection<Person>)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreEqual(originalPeople.PersistedBusinessObjects.Count, deserialisedPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.CreatedBusinessObjects.Count, deserialisedPeople.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.AddedBusinessObjects.Count, deserialisedPeople.AddedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.RemovedBusinessObjects.Count, deserialisedPeople.RemovedBusinessObjects.Count);
            Assert.AreEqual(originalPeople.MarkedForDeleteBusinessObjects.Count, deserialisedPeople.MarkedForDeleteBusinessObjects.Count);

            AssertPersonsAreEqual(deserialisedPeople.MarkedForDeleteBusinessObjects[0], deletedPerson);
            AssertPersonsAreEqual(deserialisedPeople.RemovedBusinessObjects[0], removedPerson);
            AssertPersonsAreEqual(deserialisedPeople.AddedBusinessObjects[0], addedPerson);
            AssertPersonsAreEqual(deserialisedPeople.PersistedBusinessObjects[0], persistedPerson);
            AssertPersonsAreEqual(deserialisedPeople.CreatedBusinessObjects[0], createdPerson);

        }


        [Test]
        public void TestSerialiseDeserialiseBOPropStatusIsIncluded()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            Structure.Car.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            Person originalPerson = Person.CreateSavedPerson();
           
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            originalPerson.FirstName = "Bob";

            //---------------Assert PreConditions---------------   
            Assert.IsTrue(originalPerson.Props["FirstName"].IsDirty);

            //---------------Execute Test ----------------------
           
            formatter.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Person deserialisedPerson = (Person)formatter.Deserialize(memoryStream);

            //---------------Test Result -----------------------
            Assert.IsTrue(deserialisedPerson.Props["FirstName"].IsDirty);
        }
    }
}
