//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

        [Test]
        public void TestCloneUIFormColumn()
        {
            //---------------Set up test pack-------------------
            UIFormField field1 = new UIFormField("label1", "prop1", "control", null, null, null, true, null, null, null);
            UIFormField field2 = new UIFormField("label2", "prop2", "control", null, null, null, true, null, null, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            uiFormColumn.Add(field2);
            uiFormColumn.Width = 44;

            //---------------Execute Test ----------------------
            UIFormColumn clonedFormColumn = uiFormColumn.Clone();

            //---------------Test Result -----------------------
            Assert.IsTrue(uiFormColumn == clonedFormColumn);

            Assert.IsTrue(uiFormColumn.Equals(clonedFormColumn));
            Assert.AreSame(uiFormColumn[0], clonedFormColumn[0]);

        }

        [Test]
        public void Test_NotEqualsNull()
        {
            UIFormColumn uiFormColumn1 = new UIFormColumn();
            UIFormColumn uiFormColumn2 = null;
            Assert.IsFalse(uiFormColumn1 == uiFormColumn2);
            Assert.IsFalse(uiFormColumn2 == uiFormColumn1);
            Assert.IsFalse(uiFormColumn1.Equals(uiFormColumn2));
        }

        [Test]
        public void TestEquals()
        {
            UIFormColumn uiFormColumn1 = new UIFormColumn();
            UIFormField def = new UIFormField("bob", "bob", "", "", "", "", false, "", null, null);
            uiFormColumn1.Add(def);
            UIFormColumn uiFormColumn2 = new UIFormColumn();
            uiFormColumn2.Add(def);
            Assert.IsTrue(uiFormColumn1 == uiFormColumn2);
            Assert.IsTrue(uiFormColumn2 == uiFormColumn1);
            Assert.IsFalse(uiFormColumn2 != uiFormColumn1);
            Assert.IsTrue(uiFormColumn1.Equals(uiFormColumn2));
        }
        [Test]
        public void Test_NotEquals_SameFirstItemDiffSecondItem()
        {
            UIFormColumn uiFormColumn1 = new UIFormColumn();
            UIFormField def = new UIFormField("bob", "bob", "", "", "", "", false, "", null, null);
            uiFormColumn1.Add(def);
            UIFormColumn uiFormColumn2 = new UIFormColumn();
            uiFormColumn2.Add(def);
            UIFormField def2 = new UIFormField("bob1", "bob1", "", "", "", "", false, "", null, null);
            uiFormColumn2.Add(def2);
            Assert.IsFalse(uiFormColumn1 == uiFormColumn2);
            Assert.IsTrue(uiFormColumn1 != uiFormColumn2);
            Assert.IsFalse(uiFormColumn2 == uiFormColumn1);
            Assert.IsFalse(uiFormColumn1.Equals(uiFormColumn2));

        }

        [Test]
        public void Test_NotEqualsDiffFieldCount()
        {
            //---------------Set up test pack-------------------
            UIFormColumn uiFormColumn1 = new UIFormColumn();
            UIFormField def = new UIFormField("bob", "bob", "", "", "", "", false, "", null, null);
            uiFormColumn1.Add(def);
            UIFormColumn uiFormColumn2 = new UIFormColumn();
            uiFormColumn2.Add(def);
            UIFormField def2 = new UIFormField("bob1", "bob1", "", "", "", "", false, "", null, null);

            uiFormColumn1.Add(def);
            uiFormColumn1.Add(def2);
            uiFormColumn2.Add(def2);
            //--------------Assert PreConditions----------------            
            Assert.AreNotEqual(uiFormColumn1.Count, uiFormColumn2.Count);
            //---------------Execute Test ----------------------
            bool operatorEquals = uiFormColumn1 == uiFormColumn2;
            bool equalsMethod = uiFormColumn1.Equals(uiFormColumn2);
            //---------------Test Result -----------------------
            Assert.IsFalse(operatorEquals);
            Assert.IsFalse(equalsMethod);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_NotEquals()
        {
            UIFormColumn uiFormColumn1 = new UIFormColumn();
            UIFormField def = new UIFormField("bob", "bob", "", "", "", "", false, "", null, null);
            uiFormColumn1.Add(def);
            UIFormColumn uiFormColumn2 = new UIFormColumn();
            UIFormField def2 = new UIFormField("bob1", "bob1", "", "", "", "", false, "", null, null);
            uiFormColumn2.Add(def2);
            Assert.IsFalse(uiFormColumn1 == uiFormColumn2);
            Assert.IsFalse(uiFormColumn2 == uiFormColumn1);
            Assert.IsFalse(uiFormColumn1.Equals(uiFormColumn2));

        }

        [Test]
        public void Test_NotEquals_FormColumnWidthDiff()
        {
            //---------------Set up test pack-------------------
            UIFormColumn uiFormColumn1 = new UIFormColumn(10);
            UIFormColumn uiFormColumn2 = new UIFormColumn(20);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            Assert.IsFalse(uiFormColumn1 == uiFormColumn2);
            Assert.IsFalse(uiFormColumn2 == uiFormColumn1);
            Assert.IsFalse(uiFormColumn1.Equals(uiFormColumn2));
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestEqualsDifferentType()
        {
            UIFormColumn uiFormColumn1 = new UIFormColumn();
            Assert.AreNotEqual(uiFormColumn1, "bob");
        }

    }

}
