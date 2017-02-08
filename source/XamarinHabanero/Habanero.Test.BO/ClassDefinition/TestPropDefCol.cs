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
using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestPropDefCol
    {
        [Test]
        public void TestAddDuplicationException()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDefCol col = new PropDefCol {propDef};
            //---------------Execute Test ----------------------
            try
            {
                col.Add(propDef);
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("A property definition with the name 'prop' already exists", ex.Message);
            }
        }
        [Test]
        public void Test_Add_ShouldSetPropDefsClassDef()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDefFake();
            PropDefCol col = new PropDefCol();
            var expectedClassDef = MockRepository.GenerateStub<IClassDef>();
            col.ClassDef = expectedClassDef;
            //---------------Assert Preconditions---------------
            Assert.IsNull(propDef.ClassDef);
            //---------------Execute Test ----------------------
            col.Add(propDef);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedClassDef, propDef.ClassDef);
        }

        [Test]
        public void TestRemove()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDefColInheritor col = new PropDefColInheritor();

            col.CallRemove(propDef);
            col.Add(propDef);
            Assert.AreEqual(1, col.Count);
            col.CallRemove(propDef);
            Assert.AreEqual(0, col.Count);
        }

        [Test]
        public void TestContainsPropDef()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDefColInheritor col = new PropDefColInheritor();

            Assert.IsFalse(col.GetContains(propDef));
            col.Add(propDef);
            Assert.IsTrue(col.GetContains(propDef));
        }

        // Grants access to protected methods
        private class PropDefColInheritor : PropDefCol
        {
            public void CallRemove(PropDef propDef)
            {
                Remove(propDef);
            }

            public bool GetContains(PropDef propDef)
            {
                return Contains(propDef);
            }
        }

        [Test]
        public void Clone()
        {
            //---------------Set up test pack-------------------
            var originalClassDef = LoadClassDef();
            var originalPropDefCol = originalClassDef.PropDefcol;
            //---------------Execute Test ----------------------
            var newPropDefCol = originalPropDefCol.Clone();
            //---------------Test Result -----------------------
            Assert.AreNotSame(newPropDefCol, originalPropDefCol);
            Assert.AreEqual(newPropDefCol, originalPropDefCol);
            Assert.AreSame(newPropDefCol["MyBoID"], originalPropDefCol["MyBoID"]);
        }
        
        [Test]
        public void Clone_Deep()
        {
            //---------------Set up test pack-------------------
            var originalClassDef = LoadClassDef();
            var originalPropDefCol = originalClassDef.PropDefcol;
            //---------------Execute Test ----------------------
            var newPropDefCol = originalPropDefCol.Clone(true);
            //---------------Test Result -----------------------
            Assert.AreNotSame(newPropDefCol, originalPropDefCol);
            Assert.AreEqual(newPropDefCol, originalPropDefCol);
            Assert.AreNotSame(newPropDefCol["MyBoID"], originalPropDefCol["MyBoID"]);
        }

        [Test]
        public void Clone_ShouldSetClassDef()
        {
            //---------------Set up test pack-------------------
            IClassDef originalClassDef = LoadClassDef();
            //---------------Execute Test ----------------------
            IPropDefCol newPropDefCol = originalClassDef.PropDefcol.Clone();
            //---------------Test Result -----------------------
            Assert.AreEqual(newPropDefCol.ClassDef, originalClassDef.PropDefcol.ClassDef);
        }

        [Test]
        public void Clone_DeepClone_ShouldSetClassDef()
        {
            //---------------Set up test pack-------------------
            IClassDef originalClassDef = LoadClassDef();
            //---------------Execute Test ----------------------
            IPropDefCol newPropDefCol = originalClassDef.PropDefcol.Clone(true);
            //---------------Test Result -----------------------
            Assert.AreEqual(newPropDefCol.ClassDef, originalClassDef.PropDefcol.ClassDef);
        }

        [Test]
        public void TestEqualsNull()
        {
            PropDefCol propDefCol1 = new PropDefCol();
            PropDefCol propDefCol2 = null;
            Assert.AreNotEqual(propDefCol1, propDefCol2);
        }

        [Test]
        public void TestEquals()
        {
            PropDefCol propDefCol1 = new PropDefCol();
            PropDef def = new PropDef("bob", typeof (string), PropReadWriteRule.ReadOnly, null);
            propDefCol1.Add(def);
            PropDefCol propDefCol2 = new PropDefCol();
            propDefCol2.Add(def);
            Assert.AreEqual(propDefCol1, propDefCol2);
        }

        [Test]
        public void TestEqualsDifferentType()
        {
            PropDefCol propDefCol1 = new PropDefCol();
            Assert.AreNotEqual(propDefCol1, "bob");
        }

        public static IClassDef LoadClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef def =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Test"" table=""MyRelatedBo"">
					<property  name=""MyRelatedBoID"" />
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" />
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
				</class>
			");
            return def;
        }

        [Test]
        public void Test_Contains_WhenPropNameNull_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            PropDefCol propDefCol1 = new PropDefCol();
            PropDef def = new PropDef("bob", typeof(string), PropReadWriteRule.ReadOnly, null);
            propDefCol1.Add(def);
            string propertyName = null;
            //---------------Assert Precondition----------------
            Assert.Greater(propDefCol1.Count, 0);
            Assert.IsNull(propertyName);
            //---------------Execute Test ----------------------
            bool contains = propDefCol1.Contains(propertyName);
            //---------------Test Result -----------------------
            Assert.IsFalse(contains);
        }
    }
}