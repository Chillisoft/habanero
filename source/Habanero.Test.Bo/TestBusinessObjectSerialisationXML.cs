using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectSerialisationXML
    {

        [TestFixtureSetUp]
        public void TestSetUp()
        {
            
        }

        [SetUp]
        public void SetUp()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader()));
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [Test]
        public void Test_DeserialiseXml_InvalidXml_ThrowsException()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO OrganisationID="""" Name="""">");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            Exception exceptionThrown = null;
            try
            {
                OrganisationTestBO organisation = (OrganisationTestBO) xs.Deserialize(new StringReader(xml));
            }
            catch (Exception ex)
            {
                exceptionThrown = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exceptionThrown);
            Assert.IsInstanceOf(typeof(InvalidOperationException),exceptionThrown);
            Assert.AreEqual("There is an error in XML document (2, 91).", exceptionThrown.Message);
            Assert.IsTrue(exceptionThrown.InnerException.Message.Contains("Unexpected end of file"));
        }

        [Test]
        public void Test_DeserialiseXml_NonRecognisedClassName_ThrowsException()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <NonRecognisedClass Name=""AName""/>");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            Exception exceptionThrown = null;
            try
            {
                OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            }
            catch (Exception ex)
            {
                exceptionThrown = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exceptionThrown);
            Assert.IsInstanceOf(typeof(InvalidOperationException), exceptionThrown);
            Assert.AreEqual("There is an error in XML document (2, 46).", exceptionThrown.Message);
            Assert.IsTrue(exceptionThrown.InnerException.Message.Contains("was not expected."));
        }

        [Test]
        public void Test_DeserialiseXml_NonRecognisedAttribute_ThrowsException()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO OrganisationID=""83e11f9f-705f-40d6-8cfb-101efcb72727""
                                                                 Age =""42"" />");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            Exception exceptionThrown = null;
            try
            {
                OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            }
            catch (Exception ex)
            {
                exceptionThrown = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exceptionThrown);
            Assert.IsInstanceOf(typeof(InvalidOperationException), exceptionThrown);
            Assert.AreEqual("There is an error in XML document (3, 66).", exceptionThrown.Message);
            Assert.IsTrue(exceptionThrown.InnerException.Message.Contains("The given property name 'Age' does not exist"));
        }

        [Test]
        public void Test_DeserialiseXml_NonRecognisedRelationship_ThrowsException()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_SingleRel_NoReverseRelationship();
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802"" Name="""">
                                                <NonRecognisedRelationship>
                                                    <ContactPersonTestBO ContactPersonID=""1234""
                                                                         Surname=""Smith""
                                                                         OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802""
                                                    />   
                                                </NonRecognisedRelationship>
                                            </OrganisationTestBO>");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            Exception exceptionThrown = null;
            try
            {
                OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            }
            catch (Exception ex)
            {
                exceptionThrown = ex;
            }
            //---------------Test Result -----------------------

            Assert.IsNotNull(exceptionThrown);
            
            Assert.IsInstanceOf(typeof(InvalidOperationException), exceptionThrown);
            Assert.AreEqual("There is an error in XML document (3, 50).", exceptionThrown.Message);
            Assert.IsInstanceOf(typeof(InvalidRelationshipNameException),exceptionThrown.InnerException);
            Assert.IsTrue(exceptionThrown.InnerException.Message.Contains("The relationship 'NonRecognisedRelationship' does not exist on the class 'OrganisationTestBO'."));
        }

        [Test]
        public void Test_DeserialiseXml_NoAttributes()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO />");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            //---------------Test Result -----------------------
            Assert.IsNotNull(organisation);
            Assert.IsNull(organisation.Name);

        }

        [Test]
        public void Test_XmlSerialiseDeserialise_ReturnsCorrectType()
        {
            //---------------Set up test pack-------------------
            Person originalPerson = Person.CreateSavedPerson();
            XmlSerializer xs = new XmlSerializer(typeof(Person));
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Execute Test ----------------------
            xs.Serialize(memoryStream, originalPerson);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Object deserialisedPerson = xs.Deserialize(memoryStream);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(Person), deserialisedPerson);
        }

        [Test]
        public void Test_XmlSerialiseDeserialise_ReturnsCorrectClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
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

        [Test]
        public void Test_XmlSerialiseDeserialise()
        {
            //---------------Set up test pack-------------------
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
            Assert.AreEqual(originalPerson.FirstName, deserialisedPerson.FirstName);
        }

        [Test]
        public void Test_XmlSerialise_AddBOPropAndDeserialise()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
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

        [Test]
        public void Test_XmlSerialiseDeserialise_BusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
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
            BusinessObjectCollection<Person> deserialisedPeople =
                (BusinessObjectCollection<Person>)xs.Deserialize(memoryStream);
            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreNotSame(originalPeople, deserialisedPeople);
            AssertPersonsAreEqual(deserialisedPeople[0], originalPeople[0]);
            AssertPersonsAreEqual(deserialisedPeople[1], originalPeople[1]);
            AssertPersonsAreEqual(deserialisedPeople[2], originalPeople[2]);
        }

        [Test]
        public void Test_XmlSerialiseDeserialise_BusinessObjectCollection_CreatedBusObjAreIncluded()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
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
            BusinessObjectCollection<Person> deserialisedPeople =
                (BusinessObjectCollection<Person>)xs.Deserialize(memoryStream);
            //---------------Test Result -----------------------
            Assert.AreEqual(originalPeople.Count, deserialisedPeople.Count);
            Assert.AreEqual(originalPeople.CreatedBusinessObjects.Count, deserialisedPeople.CreatedBusinessObjects.Count);
            AssertPersonsAreEqual(deserialisedPeople.CreatedBusinessObjects[0], originalPeople.CreatedBusinessObjects[0]);
            AssertPersonsAreEqual(deserialisedPeople.CreatedBusinessObjects[1], originalPeople.CreatedBusinessObjects[1]);
        }

       


        [Test]
        public void Test_SerialiseXml_WritesOneBO_CreatesTwoElements()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            MemoryStream memoryStream = new MemoryStream();
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            xs.Serialize(memoryStream, organisation);
            
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            Assert.AreEqual(2, doc.ChildNodes.Count);
            XmlNode xmlHeaderNode = doc.ChildNodes[0];
            Assert.AreEqual("xml", xmlHeaderNode.Name);
            Assert.AreEqual(0, xmlHeaderNode.ChildNodes.Count);
            
            XmlNode orgNode = doc.ChildNodes[1];
            Assert.AreEqual("OrganisationTestBO", orgNode.Name);
            Assert.AreEqual(0, orgNode.ChildNodes.Count);
        }

        [Test]
        public void Test_SerialiseXml_WritesOneBO_CreatesAttributes()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            organisation.Name = TestUtil.GetRandomString();

            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            MemoryStream memoryStream = new MemoryStream();
            //---------------Execute Test ----------------------
            xs.Serialize(memoryStream, organisation);
            
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            XmlNode personNode = doc.ChildNodes[1];
            Assert.AreEqual(2, personNode.Attributes.Count);

            XmlAttribute idAttribute = personNode.Attributes[0];
            Assert.AreEqual("OrganisationID", idAttribute.Name);
            Assert.AreEqual(organisation.OrganisationID.ToString(), idAttribute.Value);

            XmlAttribute nameAttribute = personNode.Attributes[1];
            Assert.AreEqual("Name", nameAttribute.Name);
            Assert.AreEqual(organisation.Name, nameAttribute.Value);
        }

        [Test]
        public void Test_DeserialiseXml_ReadOneBO()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();

            const string organisationID = "83e11f9f-705f-40d6-8cfb-101efcb72727";
            string organisationName = TestUtil.GetRandomString();
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?><OrganisationTestBO OrganisationID=""{0}"" Name=""{1}""/>",
                organisationID, organisationName);

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            //---------------Test Result -----------------------
            Assert.IsNotNull(organisation);
            Assert.AreEqual(organisationID, organisation.OrganisationID.ToString());
            Assert.AreEqual(organisationName, organisation.Name);
        }

        [Test]
        public void Test_DeserialiseXml_ReadTwoBOs()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();

            const string organisationID = "83e11f9f-705f-40d6-8cfb-101efcb72727";
            const string organisationID2 = "53f21f9f-705f-40d6-8cfb-191efcb73237";
            string organisationName = TestUtil.GetRandomString();
            string organisationName2 = TestUtil.GetRandomString();
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                        <ArrayOfOrganisationTestBO xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                                            xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                        <OrganisationTestBO OrganisationID=""{0}"" Name=""{1}""/>
                                        <OrganisationTestBO OrganisationID=""{2}"" Name=""{3}""/>
                                        </ArrayOfOrganisationTestBO>
                                        ", organisationID, organisationName,organisationID2,organisationName2);
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(BusinessObjectCollection<OrganisationTestBO>));
            BusinessObjectCollection<OrganisationTestBO> organisations =
                (BusinessObjectCollection<OrganisationTestBO>) xs.Deserialize(new StringReader(xml));
            //---------------Test Result -----------------------
            Assert.IsNotNull(organisations);
            Assert.AreEqual(2,organisations.Count);
            OrganisationTestBO org = organisations[0];
            OrganisationTestBO org2 = organisations[1];
            Assert.AreEqual(organisationID, org.OrganisationID.ToString());
            Assert.AreEqual(organisationID2, org2.OrganisationID.ToString());
            Assert.AreEqual(organisationName, org.Name);
            Assert.AreEqual(organisationName2, org2.Name);
        }

        [Test]
        public void Test_SerialiseXml_Composition_SingleRelationship_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_SingleRelationship(RelationshipType.Composition);
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            MemoryStream memoryStream = new MemoryStream();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            xs.Serialize(memoryStream, organisation);
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            XmlNode orgNode = doc.ChildNodes[1];
            Assert.AreEqual(0, orgNode.ChildNodes.Count);
        }

        [Test]
        public void Test_SerialiseXml_Composition_SingleRelationship()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_SingleRelationship(RelationshipType.Composition);

            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            organisation.ContactPerson = contactPerson;

            MemoryStream memoryStream = new MemoryStream();
            //---------------Assert Precondition----------------
            Assert.AreEqual(RelationshipType.Composition,
                            organisation.Relationships["ContactPerson"].RelationshipDef.RelationshipType);
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            xs.Serialize(memoryStream, organisation);
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            XmlNode orgNode = doc.ChildNodes[1];
            Assert.AreEqual(1,orgNode.ChildNodes.Count);
            XmlNode contactPersonsNode = orgNode.ChildNodes[0];
            Assert.AreEqual("ContactPerson", contactPersonsNode.Name);
            
            Assert.AreEqual(1,contactPersonsNode.ChildNodes.Count);
            XmlNode contactPersonNode = contactPersonsNode.ChildNodes[0];
            Assert.AreEqual("ContactPersonTestBO", contactPersonNode.Name);
            
            Assert.AreEqual(5,contactPersonNode.Attributes.Count);
        }

        [Test]
        public void Test_SerialiseXml_Composition_MultipleRelationship_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_MultipleRelationship(RelationshipType.Composition);

            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();

            MemoryStream memoryStream = new MemoryStream();
            //---------------Assert Precondition----------------
            Assert.AreEqual(RelationshipType.Composition,
                            organisation.Relationships["ContactPeople"].RelationshipDef.RelationshipType);
            Assert.AreEqual(0, organisation.ContactPeople.Count);
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            xs.Serialize(memoryStream, organisation);
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            XmlNode orgNode = doc.ChildNodes[1];
            Assert.AreEqual(0, orgNode.ChildNodes.Count);
        }

        [Test]
        public void Test_SerialiseXml_Composition_MultipleRelationship()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_MultipleRelationship(RelationshipType.Composition);

            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            ContactPersonTestBO contactPerson1 = ContactPersonTestBO.CreateUnsavedContactPerson();
            ContactPersonTestBO contactPerson2 = ContactPersonTestBO.CreateUnsavedContactPerson();

            organisation.ContactPeople.Add(contactPerson1);
            organisation.ContactPeople.Add(contactPerson2);

            MemoryStream memoryStream = new MemoryStream();
            //---------------Assert Precondition----------------
            Assert.AreEqual(RelationshipType.Composition,
                            organisation.Relationships["ContactPeople"].RelationshipDef.RelationshipType);
            Assert.AreEqual(2,organisation.ContactPeople.Count);
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            xs.Serialize(memoryStream, organisation);
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            XmlNode orgNode = doc.ChildNodes[1];
            Assert.AreEqual(1, orgNode.ChildNodes.Count);
            XmlNode contactPersonsNode = orgNode.ChildNodes[0];
            Assert.AreEqual("ContactPeople", contactPersonsNode.Name);

            Assert.AreEqual(2, contactPersonsNode.ChildNodes.Count);
            Assert.AreNotEqual(contactPersonsNode.ChildNodes[0],contactPersonsNode.ChildNodes[1]);
        }


        [Test]
        public void Test_SerialiseXml_Composition_MultipleLevels()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoReverseRelationship();
            ContactPersonTestBO.LoadClassDefWithAddresBOsRelationship_AddressReverseRelationshipConfigured();

            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            organisation.ContactPeople.Add(contactPerson);
            AddressTestBO address = new AddressTestBO();
            address.ContactPersonTestBO = contactPerson;
            
            IRelationshipDef contactPeopleRelDef = ClassDef.Get<OrganisationTestBO>().RelationshipDefCol["ContactPeople"];
            contactPeopleRelDef.RelationshipType = RelationshipType.Composition;

            IRelationshipDef addressesRelDef = ClassDef.Get<ContactPersonTestBO>().RelationshipDefCol["AddressTestBOs"];
            addressesRelDef.RelationshipType = RelationshipType.Composition;
            //---------------Assert Precondition----------------
            Assert.AreEqual(RelationshipType.Composition,
                            organisation.Relationships["ContactPeople"].RelationshipDef.RelationshipType);
            Assert.AreEqual(RelationshipType.Composition,
                            contactPerson.Relationships["AddressTestBOs"].RelationshipDef.RelationshipType);

            MemoryStream memoryStream = new MemoryStream();
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            xs.Serialize(memoryStream, organisation);
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            XmlNode orgNode = doc.ChildNodes[1];
            Assert.AreEqual("OrganisationTestBO", orgNode.Name);
            Assert.AreEqual(1, orgNode.ChildNodes.Count);

            XmlNode contactPeopleRelNode = orgNode.ChildNodes[0];
            Assert.AreEqual("ContactPeople", contactPeopleRelNode.Name);
            Assert.AreEqual(1, contactPeopleRelNode.ChildNodes.Count);

            XmlNode contactPersonNode = contactPeopleRelNode.ChildNodes[0];
            Assert.AreEqual("ContactPersonTestBO", contactPersonNode.Name);
            Assert.AreEqual(1, contactPersonNode.ChildNodes.Count);

            XmlNode addressesNode = contactPersonNode.ChildNodes[0];
            Assert.AreEqual("AddressTestBOs", addressesNode.Name);
            Assert.AreEqual(1, addressesNode.ChildNodes.Count);

            XmlNode addressNode = addressesNode.ChildNodes[0];
            Assert.AreEqual("AddressTestBO", addressNode.Name);
            Assert.AreEqual(0, addressNode.ChildNodes.Count);
        }
        
        [Test]
        public void Test_DeserialiseXml_Composition_SingleRelationship_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_SingleRelationship(RelationshipType.Composition);

            const string organisationID = "83e11f9f-705f-40d6-8cfb-101efcb72727";
            string organisationName = TestUtil.GetRandomString();
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO OrganisationID=""{0}"" Name=""{1}""/>",
                organisationID, organisationName);

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            //---------------Test Result -----------------------
            Assert.IsNotNull(organisation);
            Assert.IsNull(organisation.ContactPerson);
        }

        [Test]
        public void Test_DeserialiseXml_Composition_SingleRelationship()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_SingleRel_NoReverseRelationship();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();

            const string contactPersonID = "14c2a7d9-47de-47ab-99b9-ce6834947988";
            string contactPersonSurname = TestUtil.GetRandomString();

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802"" Name="""">
                                                <ContactPerson>
                                                    <ContactPersonTestBO ContactPersonID=""{0}""
                                                                         Surname=""{1}""
                                                                         OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802""
                                                    />   
                                                </ContactPerson>
                                            </OrganisationTestBO>
                                        
                                        ", contactPersonID,contactPersonSurname);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            //---------------Test Result -----------------------
            Assert.IsNotNull(organisation);

            ContactPersonTestBO person = organisation.ContactPerson;
            Assert.IsNotNull(person);
            Assert.AreEqual(contactPersonID,person.ContactPersonID.ToString());
            Assert.AreEqual(contactPersonSurname,person.Surname);
        }

        [Test]
        public void Test_DeserialiseXml_Composition_MultipleRelationship_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_MultipleRelationship(RelationshipType.Composition);

            const string organisationID = "83e11f9f-705f-40d6-8cfb-101efcb72727";
            string organisationName = TestUtil.GetRandomString();
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO OrganisationID=""{0}"" Name=""{1}""/>",
                organisationID, organisationName);

            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            //---------------Test Result -----------------------
            Assert.IsNotNull(organisation);
            Assert.AreEqual(0,organisation.ContactPeople.Count);
        }

        [Test]
        public void Test_DeserialiseXml_Composition__MultipleRelationship_OneRelatedObject()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_MultipleRelationship(RelationshipType.Composition);

            const string contactPersonID = "14c2a7d9-47de-47ab-99b9-ce6834947988";
            string contactPersonSurname = TestUtil.GetRandomString();

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802"" Name="""">
                                                <ContactPeople>
                                                    <ContactPersonTestBO ContactPersonID=""{0}"" Surname=""{1}""
                                                                         OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802""/>
                                                </ContactPeople>
                                            </OrganisationTestBO>
                                        
                                        ", contactPersonID, contactPersonSurname);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            //---------------Test Result -----------------------
            Assert.IsNotNull(organisation);
            Assert.AreEqual(1, organisation.ContactPeople.Count);
            ContactPersonTestBO contactPerson = organisation.ContactPeople[0];

            Assert.AreEqual(contactPersonID, contactPerson.ContactPersonID.ToString());
            Assert.AreEqual(contactPersonSurname, contactPerson.Surname);

        }

        [Test] public void Test_DeserialiseXml_Composition__MultipleRelationship()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_MultipleRelationship(RelationshipType.Composition);

            const string contactPersonID1 = "14c2a7d9-47de-47ab-99b9-ce6834947988";
            const string contactPersonID2 = "e0a6391a-fc34-42af-8100-8c13a686cf42";
            string contactPersonSurname1 = TestUtil.GetRandomString();
            string contactPersonSurname2 = TestUtil.GetRandomString();

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802"" Name="""">
                                                <ContactPeople>
                                                    <ContactPersonTestBO ContactPersonID=""{0}"" Surname=""{1}""
                                                                         OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802""/>
                                                    <ContactPersonTestBO ContactPersonID=""{2}"" Surname=""{3}""
                                                                         OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802""/>   
                                                </ContactPeople>
                                            </OrganisationTestBO>
                                        
                                        ", contactPersonID1, contactPersonSurname1,contactPersonID2,contactPersonSurname2);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            //---------------Test Result -----------------------
            Assert.IsNotNull(organisation);
            Assert.AreEqual(2,organisation.ContactPeople.Count);
            ContactPersonTestBO contactPerson1 = organisation.ContactPeople[0];
            ContactPersonTestBO contactPerson2 = organisation.ContactPeople[1];

            Assert.AreEqual(contactPersonID1,contactPerson1.ContactPersonID.ToString());
            Assert.AreEqual(contactPersonID2,contactPerson2.ContactPersonID.ToString());
            Assert.AreEqual(contactPersonSurname1, contactPerson1.Surname);
            Assert.AreEqual(contactPersonSurname2, contactPerson2.Surname);
        }

        [Test]
        public void Test_SerialiseXml_Aggregation_SingleRelationship()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_SingleRelationship(RelationshipType.Aggregation);

            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            organisation.ContactPerson = contactPerson;

            MemoryStream memoryStream = new MemoryStream();
            //---------------Assert Precondition----------------
            Assert.AreEqual(RelationshipType.Aggregation,
                            organisation.Relationships["ContactPerson"].RelationshipDef.RelationshipType);
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            xs.Serialize(memoryStream, organisation);
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            XmlNode orgNode = doc.ChildNodes[1];
            Assert.AreEqual(1, orgNode.ChildNodes.Count);
            XmlNode contactPersonsNode = orgNode.ChildNodes[0];
            Assert.AreEqual("ContactPerson", contactPersonsNode.Name);

            Assert.AreEqual(1, contactPersonsNode.ChildNodes.Count);
            XmlNode contactPersonNode = contactPersonsNode.ChildNodes[0];
            Assert.AreEqual("ContactPersonTestBO", contactPersonNode.Name);

            Assert.AreEqual(5, contactPersonNode.Attributes.Count);
        }

        [Test]
        public void Test_SerialiseXml_Aggregation_MultipleRelationship()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_MultipleRelationship(RelationshipType.Aggregation);

            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            ContactPersonTestBO contactPerson1 = ContactPersonTestBO.CreateUnsavedContactPerson();
            ContactPersonTestBO contactPerson2 = ContactPersonTestBO.CreateUnsavedContactPerson();

            organisation.ContactPeople.Add(contactPerson1);
            organisation.ContactPeople.Add(contactPerson2);

            MemoryStream memoryStream = new MemoryStream();
            //---------------Assert Precondition----------------
            Assert.AreEqual(RelationshipType.Aggregation,
                            organisation.Relationships["ContactPeople"].RelationshipDef.RelationshipType);
            Assert.AreEqual(2, organisation.ContactPeople.Count);
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            xs.Serialize(memoryStream, organisation);
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            XmlNode orgNode = doc.ChildNodes[1];
            Assert.AreEqual(1, orgNode.ChildNodes.Count);
            XmlNode contactPersonsNode = orgNode.ChildNodes[0];
            Assert.AreEqual("ContactPeople", contactPersonsNode.Name);

            Assert.AreEqual(2, contactPersonsNode.ChildNodes.Count);
            Assert.AreNotEqual(contactPersonsNode.ChildNodes[0], contactPersonsNode.ChildNodes[1]);
        }
       
        [Test]
        public void Test_DeserialiseXml_Aggregation_MultipleLvels()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoReverseRelationship();
            ContactPersonTestBO.LoadClassDefWithAddresBOsRelationship_AddressReverseRelationshipConfigured();
            const string contactPersonID1 = "14c2a7d9-47de-47ab-99b9-ce6834947988";
            string contactPersonSurname1 = TestUtil.GetRandomString();

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <OrganisationTestBO OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802"" Name="""">
                                                <ContactPeople>
                                                    <ContactPersonTestBO ContactPersonID=""{0}"" Surname=""{1}""
                                                                         OrganisationID=""f566ee917a8e4a178c5e4d1f0fbd9802"">

                                                        <AddressTestBOs>
                                                            <AddressTestBO ContactPersonID= ""{0}"" AddressID=""FA2C558E-1847-4dd2-822C-D98FC0FC41ED""
                                                                            AddressLine1=""1 Line""/>
                                                         </AddressTestBOs>
                                                    </ContactPersonTestBO>
                                                </ContactPeople>
                                            </OrganisationTestBO>
                                        
                                        ", contactPersonID1, contactPersonSurname1);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            OrganisationTestBO organisation = (OrganisationTestBO)xs.Deserialize(new StringReader(xml));
            //---------------Test Result -----------------------
            Assert.IsNotNull(organisation);
            Assert.AreEqual(1, organisation.ContactPeople.Count);
            ContactPersonTestBO contactPerson = organisation.ContactPeople[0];
            Assert.AreEqual(1, contactPerson.AddressTestBOs.Count);
            
        }     

        [Test]
        public void Test_SerialiseXml_Association_WritesNothing()
        {
            //---------------Set up test pack-------------------
            LoadClassDefs_SingleRelationship(RelationshipType.Association);

            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            organisation.ContactPerson = contactPerson;

            MemoryStream memoryStream = new MemoryStream();
            //---------------Assert Precondition----------------
            Assert.AreEqual(RelationshipType.Association,
                            organisation.Relationships["ContactPerson"].RelationshipDef.RelationshipType);
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            xs.Serialize(memoryStream, organisation);
            //---------------Test Result -----------------------
            XmlDocument doc = GetXmlDocument(memoryStream);

            XmlNode orgNode = doc.ChildNodes[1];
            Assert.AreEqual(0, orgNode.ChildNodes.Count);
        }
      

        //this might go to the implementing class' tests
        [Test]
        public void Test_WriteXmlFile()
        {
            //---------------Set up test pack-------------------
            MultiPropBO.LoadClassDef();
            MultiPropBO bo = new MultiPropBO();
            const string fileName = @"xmlBOs.xml";
            File.Delete(fileName);
            //---------------Assert Precondition----------------
            AssertFileDoesNotExist(fileName);
            //---------------Execute Test ----------------------
            XmlSerializer xs = new XmlSerializer(typeof(MultiPropBO));
            StreamWriter sw = new StreamWriter(fileName);
            xs.Serialize(sw, bo);
            sw.Close();
            //---------------Test Result -----------------------
            AssertFileHasBeenCreated(fileName);
        }

        [Test]
        public void Test_ReadXmlFile()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MultiPropBO.LoadClassDef();
            MultiPropBO bo = new MultiPropBO();
            const string fileName = @"xmlBOs.xml";
            File.Delete(fileName);
            XmlSerializer xs = new XmlSerializer(typeof(MultiPropBO));
            StreamWriter sw = new StreamWriter(fileName);
            xs.Serialize(sw, bo);
            sw.Close();
            BusinessObjectManager.Instance = new BusinessObjectManager();
            //---------------Assert Precondition----------------
            AssertFileHasBeenCreated(fileName);
            //---------------Execute Test ----------------------
            StreamReader sr = new StreamReader(fileName);
            MultiPropBO deserialisedBO = (MultiPropBO)xs.Deserialize(sr);
            //---------------Test Result -----------------------
            Assert.AreEqual(bo.MultiPropBOID, deserialisedBO.MultiPropBOID);
            Assert.AreEqual(bo.StringProp, deserialisedBO.StringProp);
        }

        [Test]
        public void Test_WriteXmlFile_BusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDefWithDefault("default prop");
            IBusinessObjectCollection myBOCol = GetMyBOColWithTwoBOs();
            const string fileName = @"xmlBOCol.xml";
            File.Delete(fileName);

            XmlSerializer xs = new XmlSerializer(typeof(BusinessObjectCollection<MyBO>));
            //---------------Assert Precondition----------------
            AssertFileDoesNotExist(fileName);
            Assert.AreEqual(2, myBOCol.Count);
            //---------------Execute Test ----------------------
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xs.Serialize(fs, myBOCol);
            }
            //---------------Test Result -----------------------
            AssertFileHasBeenCreated(fileName);
        }

        

        //[Test]
        //public void Test_CreateXmlFile_WAggregation()
        //{
        //    //---------------Set up test pack-------------------

        //    OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
        //    OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
        //    ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
        //    ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPerson();
        //    contactPersonTestBO.OrganisationID = organisationTestBO.OrganisationID;
        //    contactPersonTestBO.Save();

        //    const string fileName = @"C:\xmlBOsAggregation.xml";
        //    File.Delete(fileName);
        //    //---------------Assert Precondition----------------
        //    AssertFileDoesNotExist(fileName);
        //    //---------------Execute Test ----------------------
        //    XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
        //    StreamWriter sw = new StreamWriter(fileName);
        //    xs.Serialize(sw, organisationTestBO);
        //    sw.Close();
        //    //---------------Test Result -----------------------
        //    AssertFileHasBeenCreated(fileName);
        //}

        [Test, Ignore("The code is there (commented out at the bottom) - it uses the bomanager to pull it off, but needs a finally to make sure it gets put back if there is an exception")]
        public void Test_CloneBusinessObject_DifferentID()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();

            OrganisationTestBO organisation = new OrganisationTestBO();
            organisation.Name = TestUtil.GetRandomString();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            //OrganisationTestBO organisationClone = (OrganisationTestBO) organisation.Clone();
            ////---------------Test Result -----------------------
            //Assert.AreNotSame(organisation, organisationClone);
            //Assert.AreEqual(organisation.Name, organisationClone.Name);
            //Assert.AreNotEqual(organisation.OrganisationID, organisationClone.OrganisationID);
        }

        private static void LoadClassDefs_MultipleRelationship(RelationshipType relationshipType)
        {
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();

            IRelationshipDef relationshipDef = ClassDef.Get<OrganisationTestBO>().RelationshipDefCol["ContactPeople"];
            relationshipDef.RelationshipType = relationshipType;
        }

        private static void LoadClassDefs_SingleRelationship(RelationshipType relationshipType)
        {
            OrganisationTestBO.LoadDefaultClassDef_SingleRel_NoReverseRelationship();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse();

           IRelationshipDef relationshipDef = ClassDef.Get<OrganisationTestBO>().RelationshipDefCol["ContactPerson"];
            relationshipDef.RelationshipType = relationshipType;
        }


        private static XmlDocument GetXmlDocument(Stream memoryStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            XmlDocument doc = new XmlDocument();
            doc.Load(memoryStream);
            return doc;
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
}