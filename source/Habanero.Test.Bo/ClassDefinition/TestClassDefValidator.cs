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
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
	/// <summary>
	/// Summary description for TestXmlClassDefsLoader.
	/// </summary>
	[TestFixture]
	public class TestClassDefValidator
	{
// ReSharper disable InconsistentNaming
		[TestFixtureSetUp]
		public void SetupFixture()
		{
			ClassDef.ClassDefs.Clear();
		}

	   [Test]
		public void Test_Valid_Relationship()
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
								<relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
							</relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID""  type=""Guid"" />
							<property  name=""TestClassID"" />
							<primaryKey>
								<prop name=""TestRelatedClassID""/>
							</primaryKey>
						</class>
					</classes>
			";
			XmlClassDefsLoader loader = new XmlClassDefsLoader("", new DtdLoader(), GetDefClassFactory());
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator defValidator = new ClassDefValidator(GetDefClassFactory());
			//--------------------Assert Preconditions-----------------
			Assert.AreEqual(2, classDefList.Count);
			Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
			Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
			//--------------------Execute Test-------------------------
			defValidator.ValidateClassDefs(classDefList);
			//--------------------Assert Results ----------------------
		}

		protected virtual IDefClassFactory GetDefClassFactory()
		{
			return new DefClassFactory();
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
			var loader = new XmlClassDefsLoader("", new DtdLoader(), GetDefClassFactory());
			var classDefList = loader.LoadClassDefs(xml);
			var validator = new ClassDefValidator(GetDefClassFactory());
			//--------------------Assert Preconditions-----------------
			var classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
			var propDef = classDefTestClass.PropDefcol["TestClassName"];
			var classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
			var keyDef = classDefInherited.KeysCol.GetKeyDefAtIndex(0);
			var keyDefPropDef = keyDef["TestClassName"];
			Assert.AreNotSame(propDef, keyDefPropDef, "The key's property should have been resolved to be the property of the superclass by the validator.");
			//-------------Execute test ---------------------
			validator.ValidateClassDefs(classDefList);
			//-------------Test Result ----------------------
			var keyDefPropDefAfterValidate = keyDef["TestClassName"];
			Assert.AreSame(propDef, keyDefPropDefAfterValidate, "The key's property should have been resolved to be the property of the superclass by the loader.");

		}

		[Test]
		public void TestLoadClassDefs_KeyDefinedWithNonExistantProperty()
		{
			//-------------Setup Test Pack ------------------
			const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property  name=""TestClassID"" />
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
			XmlClassDefsLoader loader = new XmlClassDefsLoader("", new DtdLoader(), GetDefClassFactory());
			ClassDefCol classDefs = loader.LoadClassDefs(xml);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			//-------------Execute test ---------------------
			try
			{
				validator.ValidateClassDefs(classDefs);
				Assert.Fail("expected Err");
			}
				//---------------Test Result -----------------------
			catch (InvalidXmlDefinitionException ex)
			{
				StringAssert.Contains("In a 'prop' element for the '' key of the 'TestClassInherited' class, " +
									  "the propery 'DoesNotExist' given in the 'name' attribute does not exist for the class or " +
									  "for any of it's superclasses. Either add the property definition or check the spelling and " +
									  "capitalisation of the specified property", ex.Message);
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefs = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			//--------------------Execute Test-------------------------
			try
			{
				validator.ValidateClassDefs(classDefs);
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefs = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			//--------------------Execute Test-------------------------
			try
			{
				validator.ValidateClassDefs(classDefs);
				Assert.Fail("expected Err");
			}
			//---------------Test Result -----------------------
			catch (InvalidXmlDefinitionException ex)
			{
				StringAssert.Contains("do not have the same properties defined as the relationship keys", ex.Message);
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefs = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			//--------------------Execute Test-------------------------
			try
			{
				validator.ValidateClassDefs(classDefs);
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefs = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			//--------------------Execute Test-------------------------
			try
			{
				validator.ValidateClassDefs(classDefs);
				Assert.Fail("expected Err");
			}
			//---------------Test Result -----------------------
			catch (InvalidXmlDefinitionException ex)
			{
				StringAssert.Contains("In a 'relatedProperty' element for the 'TestRelatedClass' relationship of the 'TestClass' class, " +
					"the property 'PropDefDoesNonExist' given in the 'property' attribute does not exist", ex.Message);
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			//--------------------Assert PreConditions---------------   
			IClassDef classDef = classDefList.FindByClassName("TestClass");
			IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
			Assert.IsTrue(relationshipDef.OwningBOHasForeignKey, "This defaults to true");
			IClassDef revesreclassDef = classDefList.FindByClassName("TestClass");
			IRelationshipDef reverserelationshipDef = revesreclassDef.RelationshipDefCol["TestRelatedClass"];
			Assert.IsTrue(reverserelationshipDef.OwningBOHasForeignKey, "This defaults to true");

			//--------------------Execute Test-------------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
			Assert.IsFalse(relationshipDef.OwningBOHasForeignKey, "Should have converted this to false");
			Assert.IsFalse(reverserelationshipDef.OwningBOHasForeignKey, "Should have converted this to false");
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());

			IClassDef classDef = classDefList.FindByClassName("TestClass");
			IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
			IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
			IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];
			
			//--------------------Assert PreConditions---------------   
			Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
			Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
			//--------------------Execute Test-------------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());

			IClassDef classDef = classDefList.FindByClassName("TestClass");
			IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
			IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
			IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];
			//--------------------Assert PreConditions---------------   
			Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
			Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);

			//--------------------Execute Test-------------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			IClassDef classDef = classDefList.FindByClassName("TestClass");
			IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
			//--------------------Assert PreConditions---------------   
			Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
			//--------------------Execute Test-------------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			IClassDef classDef = classDefList.FindByClassName("TestClass");
			IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
			IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
			IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];
			//---------------Assert PreConditions---------------        
			Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
			Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);

			//--------------------Execute Test-------------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
			IRelationshipDef relationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];
			//---------------Assert PreConditions---------------        
			Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
			//--------------------Execute Test-------------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
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
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			IClassDef classDef = classDefList.FindByClassName("TestClass");
			IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
			IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
			IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];
			//---------------Assert PreConditions---------------        
			Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
			Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
			//--------------------Execute Test-------------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
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
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			IClassDef classDef = classDefList.FindByClassName("TestClass");
			IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
			IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
			IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];
			//---------------Assert PreConditions---------------        
			Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
			Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
			//--------------------Execute Test-------------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
			Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
			Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			//--------------------Execute Test-------------------------
			try
			{
				validator.ValidateClassDefs(classDefList);
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			//--------------------Execute Test-------------------------
			try
			{
				validator.ValidateClassDefs(classDefList);
				Assert.Fail("expected Err");
			}
			//---------------Test Result -----------------------
			catch (InvalidXmlDefinitionException ex)
			{
				StringAssert.Contains("The relationship 'TestRelatedClass' could not be loaded because when trying to retrieve its related class the folllowing", ex.Message);
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(xml);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			IClassDef classDef = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
			IPropDef keyPropDef = classDef.PrimaryKeyDef[0];
			//---------------Assert PreConditions---------------        
			Assert.IsFalse(keyPropDef.Compulsory);
			Assert.AreEqual(PropReadWriteRule.ReadWrite, keyPropDef.ReadWriteRule);
			//-------------Execute test ---------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(xml);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());
			IClassDef classDef = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
			IPropDef keyPropDef = classDef.PrimaryKeyDef[0];
			//---------------Assert PreConditions---------------        
			Assert.IsFalse(keyPropDef.Compulsory);
			Assert.AreEqual(PropReadWriteRule.WriteNew, keyPropDef.ReadWriteRule);
			//-------------Execute test ---------------------
			validator.ValidateClassDefs(classDefList);
			//---------------Test Result -----------------------
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			ClassDefCol classDefList = loader.LoadClassDefs(xml);
			ClassDefValidator validator = new ClassDefValidator(GetDefClassFactory());

			//-------------Execute test ---------------------
			try
			{
				validator.ValidateClassDefs(classDefList);
				//---------------Test Result -----------------------
				Assert.Fail("Should have thrown an InvalidXmlDefinitionException");
			}
			catch (InvalidXmlDefinitionException ex)
			{
				Assert.AreEqual("In the class called 'TestClass', the primary key is set as IsObjectID but the property 'TestClassID' " +
					"defined as part of the ObjectID primary key is not a Guid.", ex.Message);
			}
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
			XmlClassDefsLoader loader = CreateXmlClassDefsLoader();
			//--------------------Execute Test-------------------------
			ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
			//---------------Test Result -----------------------
			Assert.AreEqual(2, classDefList.Count);
			Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
			Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");

			Assert.Fail("wRITE TEST THSI should throw validation error");

			IClassDef classDef = classDefList.FindByClassName("TestClass");
			IRelationshipDef relationshipDef = classDef.RelationshipDefCol["TestRelatedClass"];
			IClassDef reverseClassDef = classDefList.FindByClassName("TestRelatedClass");
			IRelationshipDef reverseRelationshipDef = reverseClassDef.RelationshipDefCol["TestClass"];

			Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
			Assert.IsTrue(reverseRelationshipDef.OwningBOHasForeignKey);
		}
		private XmlClassDefsLoader CreateXmlClassDefsLoader()
		{
			return new XmlClassDefsLoader("", new DtdLoader(), GetDefClassFactory());
		}
		
	}
}