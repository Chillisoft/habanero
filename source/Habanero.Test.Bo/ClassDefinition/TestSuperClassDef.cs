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
        public void TestSetDiscriminatorForClassTableException()
        {
            SuperClassDef superClassDef =
                new SuperClassDef("ass", "class", ORMapping.ClassTableInheritance, null, "disc");
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetDiscriminatorForConcreteTableException()
        {
            SuperClassDef superClassDef =
                new SuperClassDef("ass", "class", ORMapping.ClassTableInheritance, null, "disc");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
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
