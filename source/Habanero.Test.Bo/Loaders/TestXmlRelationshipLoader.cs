//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using Habanero.BO;
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
        private const string singleRelationshipString = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
						relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO"" 
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />

					</relationship>";

        private const string multipleRelationshipString = @"
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
            itsLoader = new XmlRelationshipLoader("TestClass");
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
#pragma warning disable 168
        	Type classType = relDef.RelatedObjectClassType;
#pragma warning restore 168
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

        [Test]
        public void TestRelationshipType_Default() 
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipString, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Association, relDef.RelationshipType);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestRelationshipType_Composition() 
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringComposition = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
                        relationshipType=""Composition""
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringComposition, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Composition, relDef.RelationshipType);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestRelationshipType_Multiple_Composition() 
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringComposition = @"
					<relationship 
						name=""TestRelationship"" 
						type=""multiple"" 
                        relationshipType=""Composition""
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            MultipleRelationshipDef relDef = (MultipleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringComposition, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Composition, relDef.RelationshipType);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestRelationshipType_Invalid() 
        {
            //---------------Set up test pack-------------------
            string className = TestUtil.GetRandomString();
            XmlRelationshipLoader loader = new XmlRelationshipLoader(className);
            const string singleRelationshipStringComposition = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
                        relationshipType=""Bob""
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            try
            {
#pragma warning disable 168
                SingleRelationshipDef relDef =
#pragma warning restore 168
                    (SingleRelationshipDef) loader.LoadRelationship(singleRelationshipStringComposition, itsPropDefs);
                Assert.Fail("An error should have been raised as there is no relationship type of Bob");
            //---------------Test Result -----------------------
            } catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains(string.Format("'TestRelationship' on class '{0}'", className), ex.Message);
                StringAssert.Contains("invalid value ('Bob')", ex.Message);
            } 

        }

        [Test]
        public void Test_OwningBOHasForeignKey_Default_Single()
        {
            //---------------Set up test pack-------------------

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipString, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.IsTrue(relDef.OwningBOHasForeignKey);
            //---------------Tear Down -------------------------                  
        }

        [Test]
        public void Test_OwningBOHasForeignKey_Default_Multiple()
        {
            //---------------Set up test pack-------------------

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            MultipleRelationshipDef relDef = (MultipleRelationshipDef)itsLoader.LoadRelationship(multipleRelationshipString, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.IsFalse(relDef.OwningBOHasForeignKey);
            //---------------Tear Down -------------------------                  
        }

        [Test]
        public void Test_OwningBOHasForeignKey()
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringComposition = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                        owningBOHasForeignKey=""false""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringComposition, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.IsFalse(relDef.OwningBOHasForeignKey);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_InsertAction_DoNothing()
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringAssociation = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                        owningBOHasForeignKey=""false""
                        insertAction=""DoNothing""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringAssociation, itsPropDefs);
            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Association, relDef.RelationshipType);
            Assert.AreEqual(InsertParentAction.DoNothing,relDef.InsertParentAction);
        }
        [Test]
        public void Test_InsertAction_InsertRelationship()
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringComposition = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                        owningBOHasForeignKey=""false""
                        insertAction=""InsertRelationship""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringComposition, itsPropDefs);
            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Association, relDef.RelationshipType);
            Assert.AreEqual(InsertParentAction.InsertRelationship, relDef.InsertParentAction);
        }

        [Test]
        public void Test_InsertAction_Default_ShouldBeInsertRelationship()
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringComposition = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringComposition, itsPropDefs);
            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Association, relDef.RelationshipType);
            Assert.AreEqual(InsertParentAction.InsertRelationship, relDef.InsertParentAction);
        }
        [Test]
        public void Test_Multiple_InsertAction_DoNothing()
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringAssociation = @"
					<relationship 
						name=""TestRelationship"" 
						type=""multiple"" 
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                        owningBOHasForeignKey=""false""
                        insertAction=""DoNothing""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            MultipleRelationshipDef relDef = (MultipleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringAssociation, itsPropDefs);
            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Association, relDef.RelationshipType);
            Assert.AreEqual(InsertParentAction.DoNothing,relDef.InsertParentAction);
        } 

        [Test]
        public void Test_Multiple_InsertAction_InsertRelationship()
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringComposition = @"
					<relationship 
						name=""TestRelationship"" 
						type=""multiple"" 
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                        owningBOHasForeignKey=""false""
                        insertAction=""InsertRelationship""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            MultipleRelationshipDef relDef = (MultipleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringComposition, itsPropDefs);
            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Association, relDef.RelationshipType);
            Assert.AreEqual(InsertParentAction.InsertRelationship, relDef.InsertParentAction);
        }

        [Test]
        public void Test_Multiple_InsertAction_Default_ShouldBeInsertRelationship()
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringComposition = @"
					<relationship 
						name=""TestRelationship"" 
						type=""multiple"" 
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            MultipleRelationshipDef relDef = (MultipleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringComposition, itsPropDefs);
            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Association, relDef.RelationshipType);
            Assert.AreEqual(InsertParentAction.InsertRelationship, relDef.InsertParentAction);
        }

        [Test]
        public void Test_ReverseRelationshipDefault()
        {
            //---------------Execute Test ----------------------
            MultipleRelationshipDef relDef = (MultipleRelationshipDef)itsLoader.LoadRelationship(multipleRelationshipString, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.IsTrue(string.IsNullOrEmpty(relDef.ReverseRelationshipName));
        }

        [Test]
        public void Test_ReverseRelationship()
        {
            //---------------Set up test pack-------------------
            const string singleRelationshipStringComposition = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
                        relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
                        owningBOHasForeignKey=""false""
                        reverseRelationship=""MyReverseRelationship""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(singleRelationshipStringComposition, itsPropDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual("MyReverseRelationship", relDef.ReverseRelationshipName);
        }

        [Test]
        public void Test_WithTypeParameter()
        {
            //---------------Set up test pack-------------------
            XmlClassLoader loader = new XmlClassLoader();
            ClassDef personClassDef =
                loader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"" typeParameter=""Human"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(personClassDef);
            

             const string relXml = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
                        relatedClass=""ContactPersonTestBO"" 
						relatedAssembly=""Habanero.Test.BO""
                        typeParameter=""Human""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
             //---------------Assert PreConditions---------------   
             Assert.IsTrue(ClassDef.ClassDefs.Contains("Habanero.Test.BO", "ContactPersonTestBO_Human"));

            //---------------Execute Test ----------------------
            
            SingleRelationshipDef relDef = (SingleRelationshipDef)itsLoader.LoadRelationship(relXml, itsPropDefs);
            //---------------Test Result -----------------------

            Assert.AreSame(personClassDef, relDef.RelatedObjectClassDef);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void Test_WithTimeout()
        {
            //---------------Set up test pack-------------------
            XmlClassLoader loader = new XmlClassLoader();
            ClassDef personClassDef =
                loader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(personClassDef);


            const string relXml = @"
					<relationship 
						name=""TestRelationship"" 
						type=""multiple"" 
                        relatedClass=""ContactPersonTestBO"" 
						relatedAssembly=""Habanero.Test.BO""
                        timeout=""2000""
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";
            //---------------Assert PreConditions---------------   
            //---------------Execute Test ----------------------

            MultipleRelationshipDef relDef = (MultipleRelationshipDef)itsLoader.LoadRelationship(relXml, itsPropDefs);
            //---------------Test Result -----------------------

            Assert.AreSame(personClassDef, relDef.RelatedObjectClassDef);
            Assert.AreEqual(2000, relDef.TimeOut );
            //---------------Tear Down -------------------------          
        }

    }
}
