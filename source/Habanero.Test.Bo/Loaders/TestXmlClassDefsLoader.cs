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

using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlClassDefsLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlClassDefsLoader
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
        }

        [Test]
        public void TestLoadClassDefs()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList =
                loader.LoadClassDefs(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>
			");
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
        }

        [Test]
        public void TestLoadClassDefs_InheritedClassWithNoPrimaryKey()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList =
                loader.LoadClassDefs(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestClass2"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClass2ID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClass2ID""/>
                            </primaryKey>
						</class>
						<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                            <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
						</class>
					</classes>
			");
            Assert.AreEqual(3, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass2"), "Class 'TestClass2' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClassInherited"), "Class 'TestClassInherited' should have been loaded.");
            ClassDef classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
            ClassDef classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
            Assert.IsNotNull(classDefTestClass);
            Assert.IsNotNull(classDefInherited.SuperClassDef);
            Assert.IsNull(classDefInherited.PrimaryKeyDef);
        }

        /// <summary>
        /// This test was written because the was a problem determining the BO owning the FK with a relationship
        /// where the owning class's PK is inherited.
        /// </summary>
        [Test]
        public void TestLoadClassDefs_InheritedClassWithNoPrimaryKey_WithRelationship()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList =
                loader.LoadClassDefs(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestClass2"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClass2ID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClass2ID""/>
                            </primaryKey>
						</class>
						<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                            <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
					        <property name=""RelatedTestClassID"" type=""Guid"" />    
                            <relationship name=""TestRelationship"" type=""single"" 
						        relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"">
						        <relatedProperty property=""RelatedTestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>                
						</class>
					</classes>
			");
            Assert.AreEqual(3, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass2"), "Class 'TestClass2' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClassInherited"), "Class 'TestClassInherited' should have been loaded.");
            ClassDef classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
            ClassDef classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
            Assert.IsNotNull(classDefTestClass);
            Assert.IsNotNull(classDefInherited.SuperClassDef);
            Assert.IsNull(classDefInherited.PrimaryKeyDef);
            RelationshipDefCol relDefCol = classDefInherited.RelationshipDefCol;
            Assert.AreEqual(1, relDefCol.Count, "There should be one relationship def from the given xml definition");
            Assert.IsNotNull(relDefCol["TestRelationship"], "'TestRelationship' should be the name of the relationship created");
        }

        [Test]
        public void TestLoadClassDefs_KeyDefinedWithInheritedProperties()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
							<property  name=""TestClassName"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                            <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
                            <key>
                                <prop name=""TestClassName""/>
                            </key>
						</class>
					</classes>
			";
            //-------------Execute test ---------------------
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList = loader.LoadClassDefs(xml);
            //-------------Test Result ----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClassInherited"), "Class 'TestClassInherited' should have been loaded.");
            ClassDef classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
            IPropDef propDef = classDefTestClass.PropDefcol["TestClassName"];
            ClassDef classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
            Assert.IsNotNull(classDefInherited.SuperClassDef);
            Assert.AreEqual(1, classDefInherited.KeysCol.Count);
            KeyDef keyDef = classDefInherited.KeysCol.GetKeyDefAtIndex(0);
            IPropDef keyDefPropDef = keyDef["TestClassName"];
            Assert.AreSame(propDef, keyDefPropDef, "The key's property should have been resolved to be the property of the superclass by the loader.");
        }

        [Test]
        public void TestLoadClassDefs_KeyDefinedWithNonExistantProperty()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property  name=""TestClassID"" type=""Guid"" />
						<property  name=""TestClassName"" />
                        <primaryKey>
                            <prop name=""TestClassID""/>
                        </primaryKey>
					</class>
					<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                        <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
                        <key>
                            <prop name=""DoesNotExist""/>
                        </key>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //-------------Execute test ---------------------
            try
            {
                loader.LoadClassDefs(xml);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'prop' element for the '' key of the 'TestClassInherited' class, the propery 'DoesNotExist' given in the 'name' attribute does not exist for the class or for any of it's superclasses. Either add the property definition or check the spelling and capitalisation of the specified property", ex.Message);
            }
        }
        [Test]
        public void Test_Invalid_Relationship_PropDefDoesNotExistOnOwningClassDef()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"">
						        <relatedProperty property=""PropDefDoesNonExist"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            try
            {
                loader.LoadClassDefs(classDefsString);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The property 'PropDefDoesNonExist' defined "
                          + "in the 'relatedProperty' element in its 'Property' " 
                          + "attribute, which specifies the property in the class " 
                          + "'TestClass' from which the relationship 'TestRelatedClass' will link is not defined in the " 
                          + "class 'TestClass'.", ex.Message);
            }
        }
        [Test]
        public void Test_Invalid_Relationship_PropDefForReverseRelationshipNotSameAsRelationship()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
							<property  name=""OtherFieldID"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" owningBOHasForeignKey=""true"" reverseRelationship=""TestClass"">
						        <relatedProperty property=""OtherFieldID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" reverseRelationship=""TestRelatedClass"" owningBOHasForeignKey=""false"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            try
            {
                loader.LoadClassDefs(classDefsString);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("do not have the same properties defined as the relationship keys", ex.Message);
            }
        }

        [Test]
        public void Test_Invalid_Relationship_PropDefDoesNotExistOnRelatedClassDef()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""PropDefDoesNonExist"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            try
            {
                loader.LoadClassDefs(classDefsString);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'relatedProperty' element " 
                    + "for the 'TestRelatedClass' relationship of the 'TestClass' " 
                    + "class, the property 'PropDefDoesNonExist'", ex.Message);
            }
        }

        [Test]
        public void Test_Invalid_Relationship_SingleSingleRelationships_BothSetAsOwning_NeitherIsPrimaryKey()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
							<property  name=""ForeignKeyProp"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" owningBOHasForeignKey=""true"" reverseRelationship=""TestClass"">
						        <relatedProperty property=""ForeignKeyProp"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" owningBOHasForeignKey=""true"" reverseRelationship=""TestRelatedClass"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""ForeignKeyProp"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            try
            {
                loader.LoadClassDefs(classDefsString);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The relationship 'TestRelatedClass' could not be loaded because the reverse relationship 'TestClass' ", ex.Message);
                StringAssert.Contains("are both set up as owningBOHasForeignKey = true", ex.Message);
            }
        }
        [Test]
        public void Test_Valid_Relationship_SingleSingleRelationships_OnlyOneHasOwningBOHasForeignKey()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" owningBOHasForeignKey=""true"" reverseRelationship=""TestClass"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" reverseRelationship=""TestRelatedClass"" owningBOHasForeignKey=""false"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
            ClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey, "Should have converted this to false");
            ClassDef revesreclassDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef reverserelationshipDef = revesreclassDef.RelationshipDefCol["TestRelatedClass"];
            Assert.IsFalse(reverserelationshipDef.OwningBOHasForeignKey, "Should not have converted this to true");
        }

        [Test]
        public void Test_Valid_Relationship_SingleSingleRelationships_CanDetermine_OwningBOHasForeignKey()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" reverseRelationship=""TestClass"" owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" reverseRelationship=""TestRelatedClass"" owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            ClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            ClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
            IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
        }

        [Test]
        public void Test_Valid_Relationship_SingleSingleRelationships_CanDetermine_OwningBOHasForeignKey_ReverseDoesNotHave()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" reverseRelationship=""TestClass"" owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" reverseRelationship=""TestRelatedClass"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            ClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            ClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
            IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
        }
        [Test]
        public void Test_Valid_Relationship_SingleSingleRelationships_NoReverse_CanDetermine_OwningBOHasForeignKey()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" owningBOHasForeignKey=""true"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
            ClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
        }
        [Test]
        public void Test_Valid_Relationship_SingleSingleRelationships_CanDetermine_OwningBOHasForeignKey_SecondClass()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>

						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" reverseRelationship=""TestRelatedClass"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" reverseRelationship=""TestClass"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            ClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            ClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
            IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];

            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
        }

        [Ignore(" Cardinality does not match foreign and primary key set up.")] //TODO Brett 23 Feb 2009:
        [Test]
        public void Test_Valid_Relationship_1_M_Relationships_PrimaryAndForeignKeyDoesNotMatchRelationshipCardinality()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>

						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""multiple"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" 
                                            reverseRelationship=""TestRelatedClass"" owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" 
                                            reverseRelationship=""TestClass"" owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            Assert.Fail("wRITE TEST THSI should throw validation error");

            ClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            ClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
            IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];

            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
        }

        [Test]
        public void Test_Valid_Relationship_1_1_NoReverse_RelatatedProp_IsPartOfCompositePrimaryKey()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>

						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey isObjectID=""false"">
                                <prop name=""TestRelatedClassID""/>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" 
                                            owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            ClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
            IRelationshipDef relationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];

            Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
        }
        [Test]
        public void Test_Valid_Relationship_1_M_Relationships_CanDetermine_OwningBOHasForeignKey_SecondClass()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""multiple"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" 
                                            reverseRelationship=""TestClass"" owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" 
                                            reverseRelationship=""TestRelatedClass"" owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            ClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            ClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
            IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];

            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
        }
        [Test]
        public void Test_Valid_Relationship_1_M_Relationships_CanDetermine_OwningBOHasForeignKey_SecondClass_NotSetUp()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""multiple"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" 
                                            reverseRelationship=""TestClass"" owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" 
                                            reverseRelationship=""TestRelatedClass"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            ClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            ClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
            IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];

            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
        }

        [Ignore(" Should it set owningBOhasForeignKey = True where it can i.e. where the one is a primary key and the other is not then definitely true")] //TODO  23 Feb 2009:
        [Test]
        public void Test_Valid_Relationship_1_M_Relationships_CanDetermine_OwningBOHasForeignKey_SecondClass_SetAsFalse()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""multiple"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" 
                                            reverseRelationship=""TestClass"" owningBOHasForeignKey=""true"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" 
                                            reverseRelationship=""TestRelatedClass"" owningBOHasForeignKey=""false"" >
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            ClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            ClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
            IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];

            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
        }
        
        [Test]
        public void Test_Valid_Relationship_Loads_Where_RelatedClass_IsLoaded_After_InitialLoad_NoReverseRelationshipSetup()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>
			";
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(classDefsString,new DtdLoader()));
            //--------------------Execute Test-------------------------
            const string secondClassDefStringToLoad = @"
                        <classes>
                            <class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(secondClassDefStringToLoad, new DtdLoader()));
            ClassDefCol classDefList = ClassDef.ClassDefs;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

        }
        [Test]
        public void Test_Invalid_Relationship_SingleSingleRelationships_ReverseRelationshipNotDefined()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" owningBOHasForeignKey=""true"" reverseRelationship=""ReverseRelNonExist"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
					        <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" owningBOHasForeignKey=""true"" reverseRelationship=""TestRelatedClass"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            try
            {
                loader.LoadClassDefs(classDefsString);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The relationship 'TestRelatedClass' could not be loaded for because " 
                        + "the reverse relationship 'ReverseRelNonExist'", ex.Message);
            }
        }
        [Test]
        public void Test_Invalid_Relationship_RelatedClassDefDoesNotExist()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""RelatedClassDoesNotExist"" relatedAssembly=""Habanero.Test.BO.Loaders"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            try
            {
                loader.LoadClassDefs(classDefsString);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The relationship 'TestRelatedClass' could not be loaded for because when trying to retrieve its related class the folllowing", ex.Message);
            }
        }

        [Test]
        public void Test_Force_PrimaryKey_IsObjectID_AsCompulsoryWriteOnce_WithReadWriteRule_ReadWrite()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property name=""TestClassID"" type=""Guid"" readWriteRule=""ReadWrite""/>
                        <primaryKey isObjectID=""true"">
                            <prop name=""TestClassID""/>
                        </primaryKey>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //-------------Execute test ---------------------
            ClassDefCol classDefs = loader.LoadClassDefs(xml);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, classDefs.Count);
            Assert.IsTrue(classDefs.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            ClassDef classDef = classDefs["Habanero.Test.BO.Loaders", "TestClass"];
            Assert.AreEqual(1, classDef.PrimaryKeyDef.Count);
            IPropDef keyPropDef = classDef.PrimaryKeyDef[0];
            Assert.IsTrue(keyPropDef.Compulsory);
            Assert.AreEqual(PropReadWriteRule.WriteNew, keyPropDef.ReadWriteRule);
        }

        [Test]
        public void Test_Force_PrimaryKey_IsObjectID_AsCompulsoryWriteOnce_WithReadWriteRule_WriteNew()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property name=""TestClassID"" type=""Guid"" readWriteRule=""WriteNew""/>
                        <primaryKey isObjectID=""true"">
                            <prop name=""TestClassID""/>
                        </primaryKey>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //-------------Execute test ---------------------
            ClassDefCol classDefs = loader.LoadClassDefs(xml);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, classDefs.Count);
            Assert.IsTrue(classDefs.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            ClassDef classDef = classDefs["Habanero.Test.BO.Loaders", "TestClass"];
            Assert.AreEqual(1, classDef.PrimaryKeyDef.Count);
            IPropDef keyPropDef = classDef.PrimaryKeyDef[0];
            Assert.IsTrue(keyPropDef.Compulsory);
            Assert.AreEqual(PropReadWriteRule.WriteNew, keyPropDef.ReadWriteRule);
        }

        [Test]
        public void Test_Validate_PrimaryKey_IsObjectID_True_NonGuidProp()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property name=""TestClassID""/>
                        <primaryKey isObjectID=""true"">
                            <prop name=""TestClassID""/>
                        </primaryKey>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //-------------Execute test ---------------------
            try
            {
                loader.LoadClassDefs(xml);
                //---------------Test Result -----------------------
                Assert.Fail("Should have thrown an InvalidXmlDefinitionException");
            }
            catch (InvalidXmlDefinitionException ex)
            {
                Assert.AreEqual("In the class called 'TestClass', the primary key is set as IsObjectID but the property 'TestClassID' " +
                    "defined as part of the ObjectID primary key is not a Guid.", ex.Message);
            }
        }

        [Test]
        public void Test_Validate_PrimaryKey_HasNoProps_IsObjectGuid()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property name=""TestClassID""/>
                        <primaryKey isObjectID=""true"">
                        </primaryKey>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //-------------Execute test ---------------------
            try
            {
                loader.LoadClassDefs(xml);
                //---------------Test Result -----------------------
                Assert.Fail("Should have thrown an InvalidXmlDefinitionException");
            }
            catch (InvalidXmlDefinitionException ex)
            {
                Assert.AreEqual("A primaryKey node must have one or more prop nodes", ex.Message);
                //Assert.AreEqual("In the class called 'TestClass', the primary key has no properties defined.", ex.Message);
                //TODO Mark: 09 Feb 2009 - Improve this error message to something similar to above
            }
        }

        [Test]
        public void Test_Validate_PrimaryKey_HasNoProps_IsNotObjectGuid()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property name=""TestClassID""/>
                        <primaryKey isObjectID=""false"">
                        </primaryKey>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //-------------Execute test ---------------------
            try
            {
                loader.LoadClassDefs(xml);
                //---------------Test Result -----------------------
                Assert.Fail("Should have thrown an InvalidXmlDefinitionException");
            }
            catch (InvalidXmlDefinitionException ex)
            {
                Assert.AreEqual("A primaryKey node must have one or more prop nodes", ex.Message);
                //Assert.AreEqual("In the class called 'TestClass', the primary key has no properties defined.", ex.Message);
                //TODO Mark: 09 Feb 2009 - Improve this error message to something similar to above
            }
        }

        [Test]
        public void Test_Validate_PrimaryKey_IsObjectID_True_MultipleProps()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property name=""TestClassID"" type=""Guid""/>
						<property name=""TestRelatedClassID"" type=""Guid"" />
                        <primaryKey isObjectID=""true"">
                            <prop name=""TestClassID""/>
                            <prop name=""TestRelatedClassID""/>
                        </primaryKey>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //-------------Execute test ---------------------
            try
            {
                loader.LoadClassDefs(xml);
                //---------------Test Result -----------------------
                Assert.Fail("Should have thrown an InvalidPropertyException");
            }
            catch (InvalidPropertyException ex)
            {
                Assert.AreEqual("You cannot have more than one property for a primary key that represents an object's Guid ID", ex.Message);
            }
        }

        [Test]
        public void Test_Force_PrimaryKey_IsObjectID_False_AsCompulsoryWriteOnce_WithReadWriteRule_ReadWrite()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property name=""TestClassID"" type=""string"" readWriteRule=""ReadWrite""/>
                        <primaryKey isObjectID=""false"">
                            <prop name=""TestClassID""/>
                        </primaryKey>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            //-------------Execute test ---------------------
            ClassDefCol classDefs = loader.LoadClassDefs(xml);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, classDefs.Count);
            Assert.IsTrue(classDefs.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            ClassDef classDef = classDefs["Habanero.Test.BO.Loaders", "TestClass"];
            Assert.AreEqual(1, classDef.PrimaryKeyDef.Count);
            IPropDef keyPropDef = classDef.PrimaryKeyDef[0];
            Assert.IsFalse(keyPropDef.Compulsory);
            Assert.AreEqual(PropReadWriteRule.ReadWrite, keyPropDef.ReadWriteRule);
        }



        [Test, ExpectedException(typeof(XmlException))]
        public void TestNoRootNodeException()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            loader.LoadClassDefs(@"<invalidRootNode>");
        }
    }


}