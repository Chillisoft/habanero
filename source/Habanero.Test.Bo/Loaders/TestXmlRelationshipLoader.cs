//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
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
            Assert.AreEqual("TestOrder ASC", multipleRelDef.OrderCriteria.ToString());
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

        [Test]
        public void TestSetKeepReference()
        {
            RelationshipDef relDef =
                itsLoader.LoadRelationship(
                singleRelationshipString.Replace(@"BO""", @"BO"" keepReference=""false"" "), itsPropDefs);
            Assert.IsFalse(relDef.KeepReferenceToRelatedObject);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidDeleteAction()
        {
            itsLoader.LoadRelationship(
                multipleRelationshipString.Replace(@"TestOrder""", @"TestOrder"" deleteAction=""invalid"" "),
                itsPropDefs);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestNoOwnerProperty()
        {
            itsLoader.LoadRelationship(@"
					<relationship name=""rel"" type=""multiple"" relatedClass=""ass"" relatedAssembly=""ass"">
						<relatedProperty relatedProperty=""TestRelatedProp"" />
					</relationship>",
                itsPropDefs);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestNoRelatedProperty()
        {
            itsLoader.LoadRelationship(@"
					<relationship name=""rel"" type=""multiple"" relatedClass=""ass"" relatedAssembly=""ass"">
						<relatedProperty property=""TestProp"" />
					</relationship>",
                itsPropDefs);
        }

        [Test]
        public void TestDeleteActionDefaultMultiple()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            MultipleRelationshipDef relDef = (MultipleRelationshipDef)itsLoader.LoadRelationship(multipleRelationshipString, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual(DeleteParentAction.Prevent, relDef.DeleteParentAction);
        }

        [Test]
        public void TestDeleteActionDefaultSingle()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipString, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual(DeleteParentAction.Prevent, relDef.DeleteParentAction);
        }

    }
}