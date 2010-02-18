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

using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlClassDefsLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlClassDefsLoader
    {
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }
#pragma warning disable 618,612
        [Test]
        public void TestLoadClassDefs()
        {
            IClassDefsLoader loader = CreateXmlClassDefsLoader();
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
        public void TestLoadClassDefs_WithParameterLessConstructor_ShouldLoadTwoClasses()
        {
            const string classDefsXml = @"
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
			";
            IClassDefsLoader loader = new XmlClassDefsLoader(classDefsXml, new DtdLoader(), GetDefClassFactory());
            ClassDefCol classDefList =
                loader.LoadClassDefs();
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
        }
        private XmlClassDefsLoader CreateXmlClassDefsLoader()
        {
            return new XmlClassDefsLoader("", new DtdLoader(), GetDefClassFactory());
        }
        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestLoadClassDefs_InheritedClassWithNoPrimaryKey()
        {
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
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
            IClassDef classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
            IClassDef classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
            Assert.IsNotNull(classDefTestClass);
            Assert.IsNotNull(classDefInherited.SuperClassDef);
            //This is expecting a null PrimaryKeyDef because the global ClassDef.Classdefs col is not loaded yet.
            Assert.IsNull(classDefInherited.PrimaryKeyDef);
        }

        /// <summary>
        /// This test was written because the was a problem determining the BO owning the FK with a relationship
        /// where the owning class's PK is inherited.
        /// </summary>
        [Test]
        public void TestLoadClassDefs_InheritedClassWithNoPrimaryKey_WithRelationship()
        {
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
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
            IClassDef classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
            IClassDef classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
            Assert.IsNotNull(classDefTestClass);
            Assert.IsNotNull(classDefInherited.SuperClassDef);
            Assert.IsNull(classDefInherited.PrimaryKeyDef);
            IRelationshipDefCol relDefCol = classDefInherited.RelationshipDefCol;
            Assert.AreEqual(1, relDefCol.Count, "There should be one relationship def from the given xml definition");
            Assert.IsNotNull(relDefCol["TestRelationship"], "'TestRelationship' should be the name of the relationship created");
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
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
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
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            IClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
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
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            IClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
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
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

            IClassDef classDef = classDefList.FindByClassName("TestClass");
            IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
            IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
            IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];

            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
        }
        
        [Test]
        public void Test_Valid_Relationship_Loads_Where_RelatedClass_IsLoaded_After_InitialLoad_NoReverseRelationshipSetup()
        {

            //----------------------Test Setup ----------------------
            XmlClassDefsLoader loader = new XmlClassDefsLoader("", new DtdLoader(), GetDefClassFactory());
            ClassDefCol classDefCol = new ClassDefCol();
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
            classDefCol.Add(loader.LoadClassDefs(classDefsString));
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
            classDefCol.Add(loader.LoadClassDefs(secondClassDefStringToLoad));
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefCol.Count);
            Assert.IsTrue(classDefCol.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefCol.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
        }


        [Test]
        public void Test_CheckRelationships_RelationshipWithTypeParameterChecksAgainstCorrectRelatedClassDef()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" 
                        relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" typeParameter=""Special"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassSpecialID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" typeParameter=""Special"">
							<property  name=""TestRelatedClassID"" type=""Guid"" />
                            <property  name=""TestClassID"" type=""Guid"" />
							<property  name=""TestClassSpecialID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            //--------------------Execute Test-------------------------
            loader.LoadClassDefs(classDefsString);
            //---------------Test Result -----------------------
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
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            //-------------Execute test ---------------------
            try
            {
                loader.LoadClassDefs(xml);
                //---------------Test Result -----------------------
                Assert.Fail("Should have thrown an InvalidXmlDefinitionException");
            }
            catch (InvalidXmlDefinitionException ex)
            {
                Assert.AreEqual("The Class Definition for TestClass -  could not be loaded ", ex.Message);
                Assert.AreEqual("A primaryKey node must have one or more prop nodes", ex.InnerException.Message);
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
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            //-------------Execute test ---------------------
            try
            {
                loader.LoadClassDefs(xml);
                //---------------Test Result -----------------------
                Assert.Fail("Should have thrown an InvalidXmlDefinitionException");
            }
            catch (InvalidXmlDefinitionException ex)
            {
                Assert.AreEqual("The Class Definition for TestClass -  could not be loaded ", ex.Message);
                Assert.AreEqual("A primaryKey node must have one or more prop nodes", ex.InnerException.Message);
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
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            //-------------Execute test ---------------------
            try
            {
                loader.LoadClassDefs(xml);
                //---------------Test Result -----------------------
                Assert.Fail("Should have thrown an InvalidPropertyException");
            }
            catch (InvalidXmlDefinitionException ex)
            {
                Assert.AreEqual("The Class Definition for TestClass -  could not be loaded ", ex.Message);
                Assert.AreEqual("You cannot have more than one property for a primary key that represents an object's Guid ID", ex.InnerException.Message);
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
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            //-------------Execute test ---------------------
            ClassDefCol classDefs = loader.LoadClassDefs(xml);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, classDefs.Count);
            Assert.IsTrue(classDefs.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            IClassDef classDef = classDefs["Habanero.Test.BO.Loaders", "TestClass"];
            Assert.AreEqual(1, classDef.PrimaryKeyDef.Count);
            IPropDef keyPropDef = classDef.PrimaryKeyDef[0];
            Assert.IsFalse(keyPropDef.Compulsory);
            Assert.AreEqual(PropReadWriteRule.ReadWrite, keyPropDef.ReadWriteRule);
        }

        [Test]
        public void Test_Load_WhenClassWithRelatedClassXml_OnlyOneReverseRelationshipIndicated_ShouldLoadEachRel()
        {
            //---------------Set up test pack-------------------
            XmlClassDefsLoader loader = new XmlClassDefsLoader("", new DtdLoader(), new DefClassFactory());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ClassDefCol classDefs = loader.LoadClassDefs(TestClassWithRelatedClassXml_OnlyOneReverseRelationshipIndicated);
            //---------------Test Result -----------------------
            IClassDef classDef = classDefs["FireStarter.Test.Logic", "FireStarter.Test.Logic.Loaders.TestClass"];
            IClassDef relatedClassDef = classDefs["FireStarter.Test.Logic", "FireStarter.Test.Logic.Loaders.TestRelatedClass"];

            Assert.AreEqual(1, classDef.RelationshipDefCol.Count);
            Assert.AreEqual(1, relatedClassDef.RelationshipDefCol.Count);

        private const string TestClassWithRelatedClassXml_OnlyOneReverseRelationshipIndicated =
    @"
                <classes>
					<class name=""FireStarter.Test.Logic.Loaders.TestClass"" assembly=""FireStarter.Test.Logic"" >
						<property  name=""TestClassID"" type=""Guid"" />
                        <property name=""TestRelatedClassID"" type=""Guid"" />
                        <primaryKey>
                            <prop name=""TestClassID""/>
                        </primaryKey>
                        <relationship 
						    name=""TestRelatedClass"" 
						    type=""single"" 
						    relatedClass=""FireStarter.Test.Logic.Loaders.TestRelatedClass"" 
						    relatedAssembly=""FireStarter.Test.Logic"" 
                            reverseRelationship=""TestClasses""
                        >
						        <relatedProperty property=""TestRelatedClassID"" relatedProperty=""TestRelatedClassID"" />
    					</relationship>
					</class>
                    <class name=""FireStarter.Test.Logic.Loaders.TestRelatedClass"" assembly=""FireStarter.Test.Logic""  >
					    <property  name=""TestRelatedClassID"" />
                        <primaryKey>
                            <prop name=""TestRelatedClassID""/>
                        </primaryKey>
                        <relationship 
						    name=""TestClasses"" 
						    type=""multiple"" 
						    relatedClass=""FireStarter.Test.Logic.Loaders.TestClass"" 
						    relatedAssembly=""FireStarter.Test.Logic"" 
                        >
                            <relatedProperty property=""TestRelatedClassID"" relatedProperty=""TestRelatedClassID"" />
    					</relationship>
				    </class>
				</classes>";
        [Test, ExpectedException(typeof(XmlException))]
        public void TestNoRootNodeException()
        {
            XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
            loader.LoadClassDefs(@"<invalidRootNode>");
        }


    }
#pragma warning restore 618,612

}