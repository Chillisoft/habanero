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

using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIFormColumn
    {
        [Test]
        public void TestRemove()
        {
            UIFormField field = new UIFormField("label", "prop", "control", null, null, null, true, null, null, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field);

            Assert.IsTrue(uiFormColumn.Contains(field));
            uiFormColumn.Remove(field);
            Assert.IsFalse(uiFormColumn.Contains(field));
        }

        [Test]
        public void TestCopyTo()
        {
            UIFormField field1 = new UIFormField("label", "prop", "control", null, null, null, true, null, null, null);
            UIFormField field2 = new UIFormField("label", "prop", "control", null, null, null, true, null, null, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            uiFormColumn.Add(field2);

            UIFormField[] target = new UIFormField[2];
            uiFormColumn.CopyTo(target, 0);
            Assert.AreEqual(field1, target[0]);
            Assert.AreEqual(field2, target[1]);
        }

        // Just gets test coverage up
        [Test]
        public void TestSync()
        {
            UIFormColumn uiFormColumn = new UIFormColumn();
            Assert.AreEqual(typeof(object), uiFormColumn.SyncRoot.GetType());
            Assert.IsFalse(uiFormColumn.IsSynchronized);
        }

        //TODO:
        //[Test]
        //public void TestClonePropDefCol()
        //{
        //    ClassDef originalClassDef = LoadClassDef();
        //    PropDefCol newPropDefCol = originalClassDef.PropDefcol.Clone();
        //    Assert.AreNotSame(newPropDefCol, originalClassDef.PropDefcol);
        //    Assert.AreEqual(newPropDefCol, originalClassDef.PropDefcol);
        //}

        //[Test]
        //public void TestEqualsNull()
        //{
        //    PropDefCol propDefCol1 = new PropDefCol();
        //    PropDefCol propDefCol2 = null;
        //    Assert.AreNotEqual(propDefCol1, propDefCol2);
        //}

        //[Test]
        //public void TestEquals()
        //{
        //    PropDefCol propDefCol1 = new PropDefCol();
        //    PropDef def = new PropDef("bob", typeof(string), PropReadWriteRule.ReadOnly, null);
        //    propDefCol1.Add(def);
        //    PropDefCol propDefCol2 = new PropDefCol();
        //    propDefCol2.Add(def);
        //    Assert.AreEqual(propDefCol1, propDefCol2);
        //}

        //[Test]
        //public void TestEqualsDifferentType()
        //{
        //    PropDefCol propDefCol1 = new PropDefCol();
        //    Assert.AreNotEqual(propDefCol1, "bob");
        //}

    }

}
