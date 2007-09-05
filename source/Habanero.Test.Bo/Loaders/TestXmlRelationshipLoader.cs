using System;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlRelationshipLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlRelationshipLoader
    {
        private String singleRelationshipString =
            @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
						relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
					>
						<relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />

					</relationship>";

        private String multipleRelationshipString =
            @"
					<relationship 
						name=""TestRelationship"" 
						type=""multiple"" 
						relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
						orderBy=""TestOrder""
					>
						<relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";

        private XmlRelationshipLoader itsLoader;
        private PropDefCol itsPropDefs;

        [SetUp]
        public void SetupTest()
        {
            itsLoader = new XmlRelationshipLoader();
            itsPropDefs = new PropDefCol();
            itsPropDefs.Add(new PropDef("TestProp", typeof (string), PropReadWriteRule.ReadWrite, null));
        }

        [Test]
        public void TestLoadRelationship()
        {
            RelationshipDef relDef = itsLoader.LoadRelationship(singleRelationshipString, itsPropDefs);
            Assert.AreEqual(typeof (SingleRelationshipDef), relDef.GetType(),
                            "The relationship should be of type SingleRelationshipDef");
            Assert.AreEqual(typeof (TestRelatedClass), relDef.RelatedObjectClassType,
                            "The related classtype should be TestRelatedClass");
            Assert.IsTrue(relDef.KeepReferenceToRelatedObject,
                          "By default, the reference should be kept (according to the dtd)");
            Assert.AreEqual(1, relDef.RelKeyDef.Count, "There should be one RelPropDef in the RelKeyDef");
            Assert.IsNotNull(relDef.RelKeyDef["TestProp"],
                             "There should be a RelPropDef with name TestProp in the RelKeyDef");
            Assert.AreEqual("TestRelatedProp", relDef.RelKeyDef["TestProp"].RelatedClassPropName,
                            "The related class prop name should be what's specified in the xml");
        }

        [Test]
        public void TestLoadMultipleRelationship()
        {
            RelationshipDef relDef = itsLoader.LoadRelationship(multipleRelationshipString, itsPropDefs);
            Assert.AreEqual(typeof (MultipleRelationshipDef), relDef.GetType(),
                            "The relationship should be of type MultipleRelationshipDef");
            MultipleRelationshipDef multipleRelDef = (MultipleRelationshipDef) relDef;
            Assert.AreEqual("TestOrder", multipleRelDef.OrderBy);
            //Assert.AreEqual(0, multipleRelDef.MinNoOfRelatedObjects);
            //Assert.AreEqual(-1, multipleRelDef.MaxNoOfRelatedObjects);
            Assert.AreEqual(DeleteParentAction.Prevent, multipleRelDef.DeleteParentAction,
                            "Default delete action according to dtd is Prevent.");
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestInvalidRelationshipType()
        {
            itsLoader.LoadRelationship(singleRelationshipString.Replace("single", "notsingle"), itsPropDefs);
        }

        [Test, ExpectedException(typeof (UnknownTypeNameException))]
        public void TestWithUnknownRelatedType()
        {
            RelationshipDef relDef = itsLoader.LoadRelationship(
                singleRelationshipString.Replace("TestRelatedClass", "NonExistantTestRelatedClass"), itsPropDefs);
        	Type classType = relDef.RelatedObjectClassType;
        }

        [Test]
        public void TestWithTwoRelatedProps()
        {
            itsPropDefs.Add(new PropDef("TestProp2", typeof (string), PropReadWriteRule.ReadWrite, null));
            string relationshipWithTwoProps = singleRelationshipString.Replace
                (@"<relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />",
                 @"<relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
				   <relatedProperty property=""TestProp2"" relatedProperty=""TestRelatedProp2"" />");
            RelationshipDef relDef = itsLoader.LoadRelationship(relationshipWithTwoProps, itsPropDefs);
            Assert.AreEqual(2, relDef.RelKeyDef.Count, "There should be two relatedProperty in the relationship.");
        }
    }
}