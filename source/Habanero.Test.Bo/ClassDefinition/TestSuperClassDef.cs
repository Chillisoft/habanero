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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO.TransactionCommitters;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestSuperClassDef
    {
        [Test]
        public void TestSetIDForSingleTableException()
        {
            //---------------Execute Test ----------------------
            try
            {
                SuperClassDef superClassDef =
                    new SuperClassDef("ass", "class", ORMapping.SingleTableInheritance, "id", null);

                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("An 'ID' property has been specified for a super-class definition where the OR-mapping type is other than ClassTableInheritance", ex.Message);
            }
        }

        [Test]
        public void TestSetIDForConcreteTableException()
        {
            //---------------Execute Test ----------------------
            try
            {
                SuperClassDef superClassDef =
                    new SuperClassDef("ass", "class", ORMapping.ConcreteTableInheritance, "id", null);

                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("An 'ID' property has been specified for a super-class definition where the OR-mapping type is other than ClassTableInheritance", ex.Message);
            }
        }

        [Test]
        public void TestSetDiscriminatorForConcreteTableException()
        {
            //---------------Execute Test ----------------------
            try
            {
                SuperClassDef superClassDef =
                    new SuperClassDef("ass", "class", ORMapping.ConcreteTableInheritance, null, "disc");

                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("A 'Discriminator' property has been specified for a super-class definition where the OR-mapping type is ConcreteTableInheritance", ex.Message);
            }
        }

        [Test]
        public void TestCantFindSuperClassClassDefException() {
        
            //---------------Set up test pack-------------------
            SuperClassDef superClassDef =
                new SuperClassDef("ass", "class", ORMapping.ClassTableInheritance, null, null);
            //---------------Execute Test ----------------------
            try
            {
                IClassDef classDef = superClassDef.SuperClassClassDef;
                Assert.Fail("Expected to throw an HabaneroDeveloperException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The class definition for the super class with the type 'ass.class' was not found", ex.Message);
            }
        }

        [Test]
        public void TestProtectedGetsAndSets()
        {
            SuperClassDefInheritor superClassDef = new SuperClassDefInheritor();

            Assert.AreEqual(ORMapping.ClassTableInheritance, superClassDef.ORMapping);
            superClassDef.SetORMapping(ORMapping.ConcreteTableInheritance);
            Assert.AreEqual(ORMapping.ConcreteTableInheritance, superClassDef.ORMapping);

            Assert.AreEqual("class", superClassDef.ClassName);
            superClassDef.SetClassName("newclass");
            Assert.AreEqual("newclass", superClassDef.ClassName);

            Assert.AreEqual("ass", superClassDef.AssemblyName);
            superClassDef.SetAssemblyName("newassembly");
            Assert.AreEqual("newassembly", superClassDef.AssemblyName);
            Assert.IsNull(superClassDef.ClassName);

            Assert.IsNull(superClassDef.SuperClassClassDef);
            ClassDef classDef = new ClassDef(typeof(MyBO), null, null, null, null);
            superClassDef.SetSuperClassClassDef(classDef);
            Assert.AreSame(classDef, superClassDef.SuperClassClassDef);

            superClassDef.SetSuperClassClassDef(null);
            Assert.IsNull(superClassDef.SuperClassClassDef);
            Assert.IsNull(superClassDef.AssemblyName);
            Assert.IsNull(superClassDef.ClassName);
        }

        [Test]
        public void Test_SuperClassClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = new ClassDef("Habanero.Test.BO", "UnknownClass", null, null, null, null, null);
            ClassDef.ClassDefs.Add(classDef);
            SuperClassDef superClassDef = new SuperClassDef(classDef.AssemblyName, classDef.ClassName, ORMapping.ClassTableInheritance, null, null);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IClassDef def = superClassDef.SuperClassClassDef;
            //---------------Test Result -----------------------
            Assert.IsNotNull(def);
            Assert.AreSame(classDef, def);
        }

        [Test]
        public void Test_Construct_WithClassDef_ShouldSetClassName()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = new ClassDef(typeof(FakeBO), new PrimaryKeyDef(), new PropDefCol(), null, null);
            ClassDef.ClassDefs.Add(classDef);
            SuperClassDef superClassDef = new SuperClassDef(classDef, ORMapping.ClassTableInheritance);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IClassDef def = superClassDef.SuperClassClassDef;
            //---------------Test Result -----------------------
            Assert.IsNotNull(def);
            Assert.AreSame(classDef, def);
            Assert.AreSame(classDef.ClassName, superClassDef.ClassName);
        }
        [Test]
        public void Test_SuperClassClassDef_WithTypeParameter()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef1 = new ClassDef("Habanero.Test.BO", "UnknownClass", null, null, null, null, null);
            classDef1.TypeParameter = "TypeParam1";
            ClassDef.ClassDefs.Add(classDef1);
            ClassDef classDef2 = new ClassDef("Habanero.Test.BO", "UnknownClass", null, null, null, null, null);
            classDef2.TypeParameter = "TypeParam2";
            ClassDef.ClassDefs.Add(classDef2);
            SuperClassDef superClassDef = new SuperClassDef(classDef2.AssemblyName, classDef2.ClassNameExcludingTypeParameter, ORMapping.ClassTableInheritance, null, null);
            superClassDef.TypeParameter = classDef2.TypeParameter;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IClassDef def = superClassDef.SuperClassClassDef;
            //---------------Test Result -----------------------
            Assert.IsNotNull(def);
            Assert.AreSame(classDef2, def);
        }

#pragma warning disable 168
        [Test]
        public void Test_SuperClassClassDef_NotFound()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            SuperClassDef superClassDef = new SuperClassDef("Habanero.Test.BO", "UnknownClass", ORMapping.ClassTableInheritance, null, null);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                IClassDef def = superClassDef.SuperClassClassDef;
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
#pragma warning restore 168

        // Grants access to protected methods
        private class SuperClassDefInheritor : SuperClassDef
        {
            public SuperClassDefInheritor() :
                base("ass", "class", ORMapping.ClassTableInheritance, null, null)
            {}

            public void SetORMapping(ORMapping orMapping)
            {
                ORMapping = orMapping;
            }

            public void SetAssemblyName(string name)
            {
                AssemblyName = name;
            }

            public void SetClassName(string name)
            {
                ClassName = name;
            }

            public void SetSuperClassClassDef(IClassDef classDef)
            {
                SuperClassClassDef = classDef;
            }
        }

    }
    internal class FakeBO : BusinessObject
    { }
}
