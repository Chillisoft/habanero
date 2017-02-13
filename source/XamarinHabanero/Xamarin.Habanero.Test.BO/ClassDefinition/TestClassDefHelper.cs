#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestClassDefHelper
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void Test_GetSuperClassClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDefCol classDefCol = new ClassDefCol();
            ClassDef classDef = new ClassDef("Habanero.Test.BO", "UnknownClass", null, null, null, null, null);
            classDefCol.Add(classDef);
            SuperClassDef superClassDef = new SuperClassDef(classDef.AssemblyName, classDef.ClassName, ORMapping.ClassTableInheritance, null, null);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, ClassDef.ClassDefs.Count);
            Assert.AreEqual(1, classDefCol.Count);
            //---------------Execute Test ----------------------
            IClassDef def = ClassDefHelper.GetSuperClassClassDef(superClassDef, classDefCol);
            //---------------Test Result -----------------------
            Assert.IsNotNull(def);
            Assert.AreSame(classDef, def);
        }

        [Test]
        public void Test_GetSuperClassClassDef_WithTypeParameter()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDefCol classDefCol = new ClassDefCol();
            ClassDef classDef1 = new ClassDef("Habanero.Test.BO", "UnknownClass", null, null, null, null, null);
            classDef1.TypeParameter = "TypeParam1";
            classDefCol.Add(classDef1);
            ClassDef classDef2 = new ClassDef("Habanero.Test.BO", "UnknownClass", null, null, null, null, null);
            classDef2.TypeParameter = "TypeParam2";
            classDefCol.Add(classDef2);
            SuperClassDef superClassDef = new SuperClassDef(classDef2.AssemblyName, classDef2.ClassNameExcludingTypeParameter, ORMapping.ClassTableInheritance, null, null);
            superClassDef.TypeParameter = classDef2.TypeParameter;
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, ClassDef.ClassDefs.Count);
            Assert.AreEqual(2, classDefCol.Count);
            //---------------Execute Test ----------------------
            IClassDef def = ClassDefHelper.GetSuperClassClassDef(superClassDef, classDefCol);
            //---------------Test Result -----------------------
            Assert.IsNotNull(def);
            Assert.AreSame(classDef2, def);
        }

        [Test]
        public void Test_GetSuperClassClassDef_NotFound()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = new ClassDef("Habanero.Test.BO", "UnknownClass", null, null, null, null, null);
            ClassDef.ClassDefs.Add(classDef);
            SuperClassDef superClassDef = new SuperClassDef(classDef.AssemblyName, classDef.ClassName, ORMapping.ClassTableInheritance, null, null);
            ClassDefCol classDefCol = new ClassDefCol();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                ClassDefHelper.GetSuperClassClassDef(superClassDef, classDefCol);
                //---------------Test Result -----------------------
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The class definition for the super class with the type " +
                    "'Habanero.Test.BO.UnknownClass' was not found. Check that the class definition " +
                    "exists or that spelling and capitalisation are correct. " +
                    "There are 0 class definitions currently loaded.", ex.Message);
            }
        }

        [Test]
        public void Test_GetPrimaryKeyDef_ClassWithPK()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = new XmlClassLoader(new DtdLoader(), new DefClassFactory()).LoadClass(
                    @"	<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>");
            ClassDefCol classDefCol = new ClassDefCol();
            classDefCol.Add(classDef);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(classDef.PrimaryKeyDef);
            //---------------Execute Test ----------------------
            IPrimaryKeyDef primaryKeyDef = ClassDefHelper.GetPrimaryKeyDef(classDef, classDefCol);
            //---------------Test Result -----------------------
            Assert.IsNotNull(primaryKeyDef);
            Assert.AreSame(classDef.PrimaryKeyDef, primaryKeyDef);
        }

        [Test]
        public void Test_GetPrimaryKeyDef_ClassWithPKFromSuperClass()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef parentDef = new XmlClassLoader(new DtdLoader(), new DefClassFactory()).LoadClass(
                    @"	<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>");
            IClassDef def = new XmlClassLoader(new DtdLoader(), new DefClassFactory()).LoadClass(
                    @"
				<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"">
					<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
					<property  name=""TestProp"" type=""Guid"" />                    
				</class>
			");
            ClassDefCol classDefCol = new ClassDefCol();
            classDefCol.Add(parentDef);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(parentDef.PrimaryKeyDef);
            Assert.IsNotNull(def.SuperClassDef);
            Assert.IsNull(def.PrimaryKeyDef);
            //---------------Execute Test ----------------------
            IPrimaryKeyDef primaryKeyDef = ClassDefHelper.GetPrimaryKeyDef(def, classDefCol);
            //---------------Test Result -----------------------
            Assert.IsNotNull(primaryKeyDef);
            Assert.AreSame(parentDef.PrimaryKeyDef, primaryKeyDef);
        }

    }
}