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
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestSuperClassDef
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetIDForSingleTableException()
        {
            SuperClassDef superClassDef =
                new SuperClassDef("ass", "class", ORMapping.SingleTableInheritance, "id", null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetIDForConcreteTableException()
        {
            SuperClassDef superClassDef =
                new SuperClassDef("ass", "class", ORMapping.ConcreteTableInheritance, "id", null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetDiscriminatorForConcreteTableException()
        {
            SuperClassDef superClassDef =
                new SuperClassDef("ass", "class", ORMapping.ConcreteTableInheritance, null, "disc");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        //[Test, ExpectedException(typeof(HabaneroDeveloperException))]
        public void TestCantFindSuperClassClassDefException()
        {
            SuperClassDef superClassDef =
                new SuperClassDef("ass", "class", ORMapping.ClassTableInheritance, null, null);
            IClassDef classDef = superClassDef.SuperClassClassDef;
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
                Assert.IsNull(def);// This will never get hit. It is here to state an expectation and to avoid a resharper warning.
            }
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The class definition for the super class with the type " +
                    "'Habanero.Test.BO.UnknownClass' was not found. Check that the class definition " +
                    "exists or that spelling and capitalisation are correct. " +
                    "There are 0 class definitions currently loaded.", ex.Message);
            }
        }

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

            public void SetSuperClassClassDef(ClassDef classDef)
            {
                SuperClassClassDef = classDef;
            }
        }
    }
}
