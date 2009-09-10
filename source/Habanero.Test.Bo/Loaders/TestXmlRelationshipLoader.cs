//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
        private const string SingleRelationshipString = @"
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
						relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO"" 
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />

					</relationship>";

        private const string MultipleRelationshipString = @"
					<relationship 
						name=""TestRelationship"" 
						type=""multiple"" 
						relatedClass=""Habanero.Test.BO.Loaders.TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO""
						orderBy=""TestOrder""
					>
						<relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>";

        private XmlRelationshipLoader _loader;
        private IPropDefCol _propDefs;

        [SetUp]
        public void SetupTest() {
            Initialise();
        }

        protected void Initialise() {
            _loader = new XmlRelationshipLoader(new DtdLoader(), GetDefClassFactory(), "TestClass");
            _propDefs = GetDefClassFactory().CreatePropDefCol();
            _propDefs.Add(GetDefClassFactory().CreatePropDef("TestProp", "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false));
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestLoadRelationship()
        {
            IRelationshipDef relDef = _loader.LoadRelationship(SingleRelationshipString, _propDefs);
            Assert.AreEqual("Habanero.Test.BO.Loaders.TestRelatedClass", relDef.RelatedObjectClassName, "The related classtype should be TestRelatedClass");
            Assert.AreEqual("Habanero.Test.BO", relDef.RelatedObjectAssemblyName, "The related classtype should be TestRelatedClass");
            Assert.IsTrue(relDef.KeepReferenceToRelatedObject, "By default, the reference should be kept (according to the dtd)");
            Assert.AreEqual(1, relDef.RelKeyDef.Count, "There should be one RelPropDef in the RelKeyDef");
            Assert.IsNotNull(relDef.RelKeyDef["TestProp"],
                             "There should be a RelPropDef with name TestProp in the RelKeyDef");
            Assert.AreEqual("TestRelatedProp", relDef.RelKeyDef["TestProp"].RelatedClassPropName,
                            "The related class prop name should be what's specified in the xml");
        }

        [Test]
        public void TestLoadMultipleRelationship()
        {
            IRelationshipDef relDef = _loader.LoadRelationship(MultipleRelationshipString, _propDefs);
            Assert.AreEqual("TestOrder", relDef.OrderCriteriaString);
            Assert.AreEqual(DeleteParentAction.Prevent, relDef.DeleteParentAction,
                            "Default delete action according to dtd is Prevent.");
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestInvalidRelationshipType()
        {
            _loader.LoadRelationship(SingleRelationshipString.Replace("single", "notsingle"), _propDefs);
        }

        [Test]
        public void TestWithTwoRelatedProps()
        {
            _propDefs.Add(GetDefClassFactory().CreatePropDef("TestProp2", "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false));
            string relationshipWithTwoProps = SingleRelationshipString.Replace
                (@"<relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />",
                 @"<relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
				   <relatedProperty property=""TestProp2"" relatedProperty=""TestRelatedProp2"" />");
            IRelationshipDef relDef = _loader.LoadRelationship(relationshipWithTwoProps, _propDefs);
            Assert.AreEqual(2, relDef.RelKeyDef.Count, "There should be two relatedProperty in the relationship.");
        }

        //[Test]
        //public void TestSetKeepReference()
        //{
        //    IRelationshipDef relDef =
        //        _loader.LoadRelationship(
        //        SingleRelationshipString.Replace(@"BO""", @"BO"" keepReference=""false"" "), _propDefs);
        //    Assert.IsFalse(relDef.KeepReferenceToRelatedObject);
        //}

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidDeleteAction()
        {
            _loader.LoadRelationship(
                MultipleRelationshipString.Replace(@"TestOrder""", @"TestOrder"" deleteAction=""invalid"" "),
                _propDefs);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestNoOwnerProperty()
        {
            _loader.LoadRelationship(@"
					<relationship name=""rel"" type=""multiple"" relatedClass=""ass"" relatedAssembly=""ass"">
						<relatedProperty relatedProperty=""TestRelatedProp"" />
					</relationship>",
                _propDefs);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestNoRelatedProperty()
        {
            _loader.LoadRelationship(@"
					<relationship name=""rel"" type=""multiple"" relatedClass=""ass"" relatedAssembly=""ass"">
						<relatedProperty property=""TestProp"" />
					</relationship>",
                _propDefs);
        }

        [Test]
        public void TestDeleteActionDefaultMultiple()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IRelationshipDef relDef = _loader.LoadRelationship(MultipleRelationshipString, _propDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual(DeleteParentAction.Prevent, relDef.DeleteParentAction);
        }

        [Test]
        public void TestDeleteActionDefaultSingle()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IRelationshipDef relDef = _loader.LoadRelationship(SingleRelationshipString, _propDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual(DeleteParentAction.Prevent, relDef.DeleteParentAction);
        }

        [Test]
        public void TestRelationshipType_Default() 
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IRelationshipDef relDef = _loader.LoadRelationship(SingleRelationshipString, _propDefs);

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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringComposition, _propDefs);

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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringComposition, _propDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Composition, relDef.RelationshipType);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestRelationshipType_Invalid() 
        {
            //---------------Set up test pack-------------------
            string className = TestUtil.GetRandomString();
            XmlRelationshipLoader loader = new XmlRelationshipLoader(new DtdLoader(), GetDefClassFactory(), className);
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
                loader.LoadRelationship(singleRelationshipStringComposition, _propDefs);
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
            IRelationshipDef relDef = _loader.LoadRelationship(SingleRelationshipString, _propDefs);

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
            IRelationshipDef relDef = _loader.LoadRelationship(MultipleRelationshipString, _propDefs);

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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringComposition, _propDefs);

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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringAssociation, _propDefs);
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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringComposition, _propDefs);
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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringComposition, _propDefs);
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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringAssociation, _propDefs);
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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringComposition, _propDefs);
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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringComposition, _propDefs);
            //---------------Test Result -----------------------
            Assert.AreEqual(RelationshipType.Association, relDef.RelationshipType);
            Assert.AreEqual(InsertParentAction.InsertRelationship, relDef.InsertParentAction);
        }

        [Test]
        public void Test_ReverseRelationshipDefault()
        {
            //---------------Execute Test ----------------------
            MultipleRelationshipDef relDef = (MultipleRelationshipDef)_loader.LoadRelationship(MultipleRelationshipString, _propDefs);

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
            IRelationshipDef relDef = _loader.LoadRelationship(singleRelationshipStringComposition, _propDefs);

            //---------------Test Result -----------------------
            Assert.AreEqual("MyReverseRelationship", relDef.ReverseRelationshipName);
        }

        [Test]
        public void Test_WithTypeParameter()
        {
            //---------------Set up test pack-------------------
            XmlClassLoader loader = new XmlClassLoader(new DtdLoader(), GetDefClassFactory());
            IClassDef personClassDef =
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

             IRelationshipDef relDef = _loader.LoadRelationship(relXml, _propDefs);
            //---------------Test Result -----------------------

            Assert.AreSame(personClassDef, relDef.RelatedObjectClassDef);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void Test_WithTimeout()
        {
            //---------------Set up test pack-------------------
            XmlClassLoader loader = new XmlClassLoader(new DtdLoader(), GetDefClassFactory());
            IClassDef personClassDef =
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

            IRelationshipDef relDef = _loader.LoadRelationship(relXml, _propDefs);
            //---------------Test Result -----------------------

            Assert.AreSame(personClassDef, relDef.RelatedObjectClassDef);
            Assert.AreEqual(2000, relDef.TimeOut );
            //---------------Tear Down -------------------------          
        }

    }
}
