using System;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlRelationshipLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlRelationshipLoader
    {
        private String singleRelationshipString =
            @"
					<relationshipDef 
						name=""TestRelationship"" 
						type=""single"" 
						relatedType=""Habanero.Test.Bo.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.Bo""
					>
						<relKeyDef>
							<relProp name=""TestProp"" relatedPropName=""TestRelatedProp"" />
						</relKeyDef>
					</relationshipDef>";

        private String multipleRelationshipString =
            @"
					<relationshipDef 
						name=""TestRelationship"" 
						type=""multiple"" 
						relatedType=""Habanero.Test.Bo.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.Bo""
						orderBy=""TestOrder""
					>
						<relKeyDef>
							<relProp name=""TestProp"" relatedPropName=""TestRelatedProp"" />
						</relKeyDef>
					</relationshipDef>";

        private XmlRelationshipLoader itsLoader;
        private PropDefCol itsPropDefs;

        [SetUp]
        public void SetupTest()
        {
            itsLoader = new XmlRelationshipLoader();
            itsPropDefs = new PropDefCol();
            itsPropDefs.Add(new PropDef("TestProp", typeof (string), PropReadWriteRule.ReadManyWriteMany, null));
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
            Assert.AreEqual(0, multipleRelDef.MinNoOfRelatedObjects);
            Assert.AreEqual(-1, multipleRelDef.MaxNoOfRelatedObjects);
            Assert.AreEqual(DeleteParentAction.PreventDeleteParent, multipleRelDef.DeleteParentAction,
                            "Default delete action according to dtd is PreventDeleteParent.");
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
            itsPropDefs.Add(new PropDef("TestProp2", typeof (string), PropReadWriteRule.ReadManyWriteMany, null));
            string relationshipWithTwoProps = singleRelationshipString.Replace
                (@"<relProp name=""TestProp"" relatedPropName=""TestRelatedProp"" />",
                 @"<relProp name=""TestProp"" relatedPropName=""TestRelatedProp"" />
				   <relProp name=""TestProp2"" relatedPropName=""TestRelatedProp2"" />");
            RelationshipDef relDef = itsLoader.LoadRelationship(relationshipWithTwoProps, itsPropDefs);
            Assert.AreEqual(2, relDef.RelKeyDef.Count, "There should be two relprops in the relkeydef.");
        }
    }
}