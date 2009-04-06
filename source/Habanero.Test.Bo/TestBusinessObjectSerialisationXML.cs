using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// TODO:
    /// - error message for non-recognised attribute, non-recognised classname
    /// - what happens if xml is missing key attributes (eg value for ID)
    /// - invalid xml (eg no closing tags, empty file)
    /// </summary>
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
            Assert.IsInstanceOfType(typeof(Person), deserialisedPerson);
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

        //[Test]
        //public void Test_XmlSerialiseDeserialise_Composition_SingleRelationship()
        //{
        //    //---------------Set up test -----------------------
        //    ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
        //    OrganisationTestBO.LoadDefaultClassDef_SingleRel_NoReverseRelationship();
            
        //    OrganisationTestBO organisation= OrganisationTestBO.CreateUnsavedOrganisation();
        //    organisation.Name = TestUtil.GetRandomString();
        //    ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
        //    contactPerson.Surname = TestUtil.GetRandomString();

        //    //Change relationship type to composite
        //    organisation.ContactPerson = contactPerson;
        //    ISingleRelationship relationship =
        //              (ISingleRelationship)organisation.Relationships["ContactPerson"];
        //    relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;

        //    XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
        //    MemoryStream memoryStream = new MemoryStream();

        //    //make sure we load contact from xml and not from memory
        //    BusinessObjectManager.Instance.ClearLoadedObjects();
            
        //    //---------------Assert pre conditions -------------
        //    Assert.AreEqual(RelationshipType.Composition, organisation.Relationships["ContactPerson"].RelationshipDef.RelationshipType);
        //    Assert.IsNotNull(organisation.Name);
        //    Assert.IsNotNull(contactPerson.Surname);
        //    Assert.AreEqual(0, Broker.GetBusinessObjectCollection<ContactPersonTestBO>("").Count);
        //    //---------------Execute Test ----------------------
        //    xs.Serialize(memoryStream, organisation);

        //    //StreamWriter sw = new StreamWriter("test.xml");
        //    //xs.Serialize(sw, organisation);
        //    //sw.Close();

        //    memoryStream.Seek(0, SeekOrigin.Begin);
        //    OrganisationTestBO deserialisedOrganisation = (OrganisationTestBO)xs.Deserialize(memoryStream);
        //    //---------------Test Result -----------------------
            
        //    Assert.AreNotSame(organisation, deserialisedOrganisation);
        //    //Assert.AreEqual(organisation, deserialisedOrganisation); //gets a new guid id
        //    Assert.AreEqual(organisation.OrganisationID, deserialisedOrganisation.OrganisationID);
        //    Assert.AreEqual(organisation.Name, deserialisedOrganisation.Name);

        //    Assert.IsNotNull(deserialisedOrganisation.ContactPerson);
        //    Assert.AreEqual(organisation.ContactPerson,deserialisedOrganisation.ContactPerson);
        //    Assert.AreEqual(organisation.ContactPerson.Surname,deserialisedOrganisation.ContactPerson.Surname);
        //}










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
        public void Test_SerialiseXml_Composition_SingleRelationship()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_SingleRel_NoReverseRelationship();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();

            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //Change relationship type to composite
            organisation.ContactPerson = contactPerson;
            ISingleRelationship relationship =
                      (ISingleRelationship)organisation.Relationships["ContactPerson"];
            relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;

            MemoryStream memoryStream = new MemoryStream();
            //---------------Assert Precondition----------------
            Assert.AreEqual(RelationshipType.Composition, organisation.Relationships["ContactPerson"].RelationshipDef.RelationshipType);
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



        private static XmlDocument GetXmlDocument(Stream memoryStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            XmlDocument doc = new XmlDocument();
            doc.Load(memoryStream);
            return doc;
        }


        [Test, Ignore]
        public void Test_SerialiseDeserialise_Aggregation()
        {
            //---------------Set up test -----------------------
            //find composition test class
            //there is an aggregation relationship between organisation and contactperson
            //this default class def has contactppl rship
            OrganisationTestBO.LoadDefaultClassDef();
            //i think this is the class def with the correct structure for aggregation with contact person 
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            //create the organisation
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            //create a contact person
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPerson();
            //link between the two classes 
            organisationTestBO.ContactPeople.Add(contactPersonTestBO);
            organisationTestBO.Save();

            XmlSerializer xs = new XmlSerializer(typeof(OrganisationTestBO));
            MemoryStream memoryStream = new MemoryStream();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Assert pre conditions -------------
            Assert.AreEqual(organisationTestBO.ContactPeople[0], contactPersonTestBO);
            //---------------Execute Test ----------------------
            xs.Serialize(memoryStream, organisationTestBO);
            memoryStream.Seek(0, SeekOrigin.Begin);
            OrganisationTestBO deserialisedOrganisation =
                (OrganisationTestBO)xs.Deserialize(memoryStream);
            //---------------Test Result -----------------------
            Assert.AreNotSame(organisationTestBO, deserialisedOrganisation);
            //implementing aggregation would mean that all the objects that are created will be returned.
            //so both organisationTestBO AND contactperson need to be serialized AND deserialized 
            //with only one call to the serialize and deserialize methods.
            Assert.AreEqual(organisationTestBO.ContactPeople[0], deserialisedOrganisation.ContactPeople[0]);
            Assert.AreEqual(organisationTestBO.ContactPeople[0].Surname, deserialisedOrganisation.ContactPeople[0].Surname);
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

        private static void AssertPersonsAreEqual(IBusinessObject originalPerson, IBusinessObject deserialisedPerson)
        {
            foreach (IBOProp prop in originalPerson.Props)
            {
                Assert.AreEqual(prop.Value, deserialisedPerson.GetPropertyValue(prop.PropertyName));
            }
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