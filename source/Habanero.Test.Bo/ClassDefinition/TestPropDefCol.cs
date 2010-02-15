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
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestPropDefCol
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddDuplicationException()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDefCol col = new PropDefCol();
            col.Add(propDef);
            col.Add(propDef);
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
        public void TestClonePropDefCol()
        {
            IClassDef originalClassDef = LoadClassDef();
            IPropDefCol newPropDefCol = originalClassDef.PropDefcol.Clone();
            Assert.AreNotSame(newPropDefCol, originalClassDef.PropDefcol);
            Assert.AreEqual(newPropDefCol, originalClassDef.PropDefcol);
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