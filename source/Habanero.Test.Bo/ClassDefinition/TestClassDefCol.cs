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
#pragma warning disable 168
        [Test]
        public void TestIndexer_NotFound()
        {
            //---------------Set up test pack-------------------
            ClassDefCol col = new ClassDefCol();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                IClassDef classDef = col["ass", "class"];

                Assert.Fail("Expected error when trying to get a classdef that doesn't exist using the indexer");
            }
            catch (HabaneroDeveloperException ex)
            {
                //---------------Test Result -----------------------
                StringAssert.Contains("No ClassDef has been loaded for ", ex.Message);
            }
        }
#pragma warning restore 168
        [Test]
        public void TestGetsAndSets()
        {
            ClassDefCol col = new ClassDefCol();
            Assert.AreEqual(0, col.Keys.Count);
            Assert.AreEqual(0, col.Values.Count);
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
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
            ClassDef classDef = new ClassDef(typeof (String), null, null, null, null, null, null);
            ClassDefCol col = new ClassDefCol();

            col.Add(classDef);
            Assert.AreEqual(1, col.Count);
            col.Remove(classDef);
            Assert.AreEqual(0, col.Count);

            col.Add(classDef);
            Assert.AreEqual(1, col.Count);
            col.Remove(typeof (String));
            Assert.AreEqual(0, col.Count);
        }

        [Test, ExpectedException(typeof (HabaneroArgumentException))]
        public void TestLoadColClassDefException()
        {
            ClassDefCol.LoadColClassDef(null);
        }

        [Test]
        public void TestNullClassNamespace()
        {
            string nameSpace;
            ClassDefCol.StripOutNameSpace(null, out nameSpace);
            Assert.IsNull(nameSpace);
        }

        private class ClassDefColInheritor : ClassDefCol
        {
            public void CallFinalize()
            {
                FinalizeInstanceFlag();
            }
        }

        [Test]
        public void TestFindByClassName_Found()
        {
            //---------------Set up test pack-------------------
            ClassDefCol col = new ClassDefCol();
            ClassDef classDef1 = new ClassDef("assembly", "class1", null, null, null, null, null);
            ClassDef classDef2 = new ClassDef("assembly", "class2", null, null, null, null, null);
            ClassDef classDef3 = new ClassDef("assembly", "class3", null, null, null, null, null);
            col.Add(classDef1);
            col.Add(classDef2);
            col.Add(classDef3);
            //---------------Execute Test ----------------------
            IClassDef foundClass1 = col.FindByClassName("class1");
            IClassDef foundClass2 = col.FindByClassName("class2");
            IClassDef foundClass3 = col.FindByClassName("class3");
            //---------------Test Result -----------------------
            Assert.AreSame(classDef1, foundClass1);
            Assert.AreSame(classDef2, foundClass2);
            Assert.AreSame(classDef3, foundClass3);
        }

        [Test]
        public void TestFindByClassName_NotFound()
        {
            //---------------Set up test pack-------------------
            ClassDefCol col = new ClassDefCol();
            ClassDef classDef1 = new ClassDef("assembly", "class1", null, null, null, null, null);
            ClassDef classDef2 = new ClassDef("assembly", "class2", null, null, null, null, null);
            ClassDef classDef3 = new ClassDef("assembly", "class3", null, null, null, null, null);
            col.Add(classDef1);
            col.Add(classDef2);
            col.Add(classDef3);
            //---------------Execute Test ----------------------
            IClassDef foundClass = col.FindByClassName("DoesNotExist");
            //---------------Test Result -----------------------
            Assert.IsNull(foundClass);
        }

        [Test]
        public void TestFindByClassName_NotFound_EmptyCol()
        {
            //---------------Set up test pack-------------------
            ClassDefCol col = new ClassDefCol();
            //---------------Execute Test ----------------------
            IClassDef foundClass = col.FindByClassName("DoesNotExist");
            //---------------Test Result -----------------------
            Assert.IsNull(foundClass);
        }

        [Test]
        public void TestIndexer_Get()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = new ClassDef
                (typeof (MockBo), new PrimaryKeyDef(), new PropDefCol(), new KeyDefCol(), new RelationshipDefCol());
            ClassDefCol col = new ClassDefCol();
            col.Add(classDef);
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            IClassDef returnedClassDef = col[typeof (MockBo)];
            //---------------Test Result -----------------------
            Assert.AreEqual(classDef, returnedClassDef);
        }

        private class MockBo : BusinessObject
        {
        }
    }
}