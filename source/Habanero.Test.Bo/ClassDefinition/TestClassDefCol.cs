//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.BO;
using Habanero.Test;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestClassDefCol
    {
        [TestFixtureSetUp]
        public void TestSetup()
        {
            //Improve test coverage
            ClassDefColInheritor col = new ClassDefColInheritor();
            col.CallFinalize();
        }

        [Test]
        public void TestThisNotFound()
        {
            ClassDefCol col = new ClassDefCol();
            Assert.IsNull(col["ass", "class"]);
        }

        [Test]
        public void TestGetsAndSets()
        {
            ClassDefCol col = new ClassDefCol();
            Assert.AreEqual(0, col.Keys.Count);
            Assert.AreEqual(0, col.Values.Count);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestAddDuplicateException()
        {
            ClassDef classDef = new ClassDef("ass", "class", null, null, null, null, null);
            ClassDefCol col = new ClassDefCol();
            col.Add(classDef);
            col.Add(classDef);
        }

        [Test]
        public void TestRemove()
        {
            ClassDef classDef = new ClassDef(typeof(String), null, null, null, null, null);
            ClassDefCol col = new ClassDefCol();
            
            col.Add(classDef);
            Assert.AreEqual(1, col.Count);
            col.Remove(classDef);
            Assert.AreEqual(0, col.Count);

            col.Add(classDef);
            Assert.AreEqual(1, col.Count);
            col.Remove(typeof(String));
            Assert.AreEqual(0, col.Count);
        }

        [Test, ExpectedException(typeof(HabaneroArgumentException))]
        public void TestLoadColClassDefException()
        {
            ClassDefCol.LoadColClassDef(null);
        }

        [Test]
        public void TestNullClassNamespace()
        {
            string nameSpace = "bob";
            ClassDefCol.StripOutNameSpace(null, out nameSpace);
            Assert.IsNull(nameSpace);
        }

        private class ClassDefColInheritor : ClassDefCol
        {
            public void CallFinalize()
            {
                Finalize();
            }
        }
    }
}