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

using System.Collections;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIFormTab
    {
        [Test]
        public void TestRemove()
        {
            UIFormColumn column = new UIFormColumn();
            UIFormTab uiFormTab = new UIFormTab();
            uiFormTab.Add(column);

            Assert.IsTrue(uiFormTab.Contains(column));
            uiFormTab.Remove(column);
            Assert.IsFalse(uiFormTab.Contains(column));
        }

        [Test]
        public void TestCopyTo()
        {
            UIFormColumn column1 = new UIFormColumn();
            UIFormColumn column2 = new UIFormColumn();
            UIFormTab uiFormTab = new UIFormTab();
            uiFormTab.Add(column1);
            uiFormTab.Add(column2);

            UIFormColumn[] target = new UIFormColumn[2];
            uiFormTab.CopyTo(target, 0);
            Assert.AreEqual(column1, target[0]);
            Assert.AreEqual(column2, target[1]);
        }

        // Just gets test coverage up
        [Test]
        public void TestSync()
        {
            UIFormTab uiFormTab = new UIFormTab();
            Assert.AreEqual(typeof (object), uiFormTab.SyncRoot.GetType());
            Assert.IsFalse(uiFormTab.IsSynchronized);
        }

        [Test]
        public void TestUIFormGrid()
        {
            UIFormGrid grid = new UIFormGrid("rel", typeof (MyBO), "correl");
            UIFormTab uiFormTab = new UIFormTab();

            Assert.IsNull(uiFormTab.UIFormGrid);
            uiFormTab.UIFormGrid = grid;
            Assert.AreEqual(grid, uiFormTab.UIFormGrid);
        }


        [Test]
        public void TestCloneUIFormTab()
        {
            //---------------Set up test pack-------------------
            UIFormField field1 = new UIFormField("label1", "prop1", "control", null, null, null, true, null, null, null);
            UIFormField field2 = new UIFormField("label2", "prop2", "control", null, null, null, true, null, null, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            uiFormColumn.Add(field2);

            UIFormTab uiFormTab = new UIFormTab("Tab1");
            uiFormTab.Add(uiFormColumn);

            //---------------Execute Test ----------------------
            UIFormTab clonedFormTab = uiFormTab.Clone();

            //---------------Test Result -----------------------
            Assert.IsTrue(uiFormTab == clonedFormTab);
            Assert.IsTrue(uiFormTab.Equals(clonedFormTab));
            Assert.AreEqual(uiFormTab[0], clonedFormTab[0],
                              "Should be a deep copy and the columns should be equal but copied");
            Assert.AreNotSame(uiFormTab[0], clonedFormTab[0], "Should be a deep copy and the columns should be equal but copied (not same)");
        }

        [Test]
        public void Test_NotEqualsNull()
        {
            UIFormTab uiFormTab1 = new UIFormTab();
            UIFormTab uiFormTab2 = null;
            Assert.IsFalse(uiFormTab2 == uiFormTab1);
            Assert.AreNotEqual(uiFormTab1, uiFormTab2);
        }

        [Test]
        public void TestEquals_SameColumn()
        {
            UIFormTab uiFormTab1 = new UIFormTab();
            UIFormColumn uiFormColumn = CreateUIFormColumn_2Fields();
            UIFormTab uiFormTab2 = new UIFormTab();

            uiFormTab1.Add(uiFormColumn);
            uiFormTab2.Add(uiFormColumn);
            Assert.IsTrue(uiFormTab2 == uiFormTab1);
            Assert.AreEqual(uiFormTab1, uiFormTab2);
        }

        [Test]
        public void TestEquals_EqualColumn()
        {
            UIFormTab uiFormTab1 = new UIFormTab();
            UIFormColumn uiFormColumn1 = CreateUIFormColumn_2Fields();
            UIFormColumn uiFormColumn2 = CreateUIFormColumn_2Fields();
            UIFormTab uiFormTab2 = new UIFormTab();

            uiFormTab1.Add(uiFormColumn1);
            uiFormTab2.Add(uiFormColumn2);
            Assert.IsTrue(uiFormTab2 == uiFormTab1);
            Assert.IsFalse(uiFormTab2 != uiFormTab1);
            Assert.AreEqual(uiFormTab1, uiFormTab2);
        }

        [Test]
        public void TestNotEquals_DifferentField()
        {
            UIFormTab uiFormTab1 = new UIFormTab();
            UIFormColumn uiFormColumn1 = CreateUIFormColumn_2Fields();
            UIFormColumn uiFormColumn2 = CreateUIFormColumn_2Fields("diffProp");
            UIFormTab uiFormTab2 = new UIFormTab();

            uiFormTab1.Add(uiFormColumn1);

            uiFormTab2.Add(uiFormColumn2);

            Assert.IsFalse(uiFormTab2 == uiFormTab1);
            Assert.IsTrue(uiFormTab2 != uiFormTab1);
            Assert.IsFalse(uiFormTab1.Equals(uiFormTab2));
            //Assert.AreNotEqual(uiFormTab1, uiFormTab2);
        }

        [Test]
        public void TestNotEquals_DiffColCount()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = new UIFormTab();
            UIFormColumn uiFormColumn1 = CreateUIFormColumn_2Fields();
            UIFormColumn uiFormColumn2 = CreateUIFormColumn_2Fields("diffProp");
            UIFormTab uiFormTab2 = new UIFormTab();


            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            uiFormTab1.Add(uiFormColumn1);
            uiFormTab1.Add(uiFormColumn2);

            uiFormTab2.Add(uiFormColumn2);
            //---------------Test Result -----------------------
            Assert.IsFalse(uiFormTab2 == uiFormTab1);
            Assert.IsFalse(uiFormTab1.Equals(uiFormTab2));
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestNotEquals_DifferentTabName()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = new UIFormTab("tab1");
            uiFormTab1.Add(CreateUIFormColumn_2Fields());
            UIFormTab uiFormTab2 = new UIFormTab("tab2");
            uiFormTab2.Add(CreateUIFormColumn_2Fields());

            //--------------Assert PreConditions----------------            
            Assert.AreNotEqual(uiFormTab2.Name, uiFormTab1.Name);
            //---------------Execute Test ----------------------
            Assert.IsFalse(uiFormTab2 == uiFormTab1);
            Assert.IsFalse(uiFormTab1.Equals(uiFormTab2));
            //Assert.AreNotEqual(uiFormTab1, uiFormTab2);
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetMaximumFieldCount()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = new UIFormTab("tab1");
            uiFormTab1.Add(CreateUIFormColumn_2Fields());
            uiFormTab1.Add(CreateUIFormColumn_1Field());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int maxFieldCount = uiFormTab1.GetMaxFieldCount();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, maxFieldCount);
        }

        [Test]
        public void TestGetMaxRowsInColumns()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = new UIFormTab("tab1");
            uiFormTab1.Add(CreateUIFormColumn_2FieldsWithRowSpan());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int maxFieldCount = uiFormTab1.GetMaxRowsInColumns();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, maxFieldCount);
        }

        [Test]
        public void TestGetMaxRowsInColumns_RowSpan()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = new UIFormTab("tab1");
            uiFormTab1.Add(CreateUIFormColumn_2FieldsWithRowSpan());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int maxFieldCount = uiFormTab1.GetMaxRowsInColumns();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, maxFieldCount);
        }

        [Test]
        public void TestGetMaxRowsInColumns_RowAndColSpan()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = new UIFormTab("tab1");
            uiFormTab1.Add(CreateUIFormColumn_1FieldWith2RowAnd2ColSpan());
            uiFormTab1.Add(CreateUIFormColumn_1Field());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int maxFieldCount = uiFormTab1.GetMaxRowsInColumns();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, maxFieldCount);
        }

        [Test]
        public void TestGetMaxRowsInColumns_RowAndColSpan_3Cols()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = new UIFormTab("tab1");
            uiFormTab1.Add(CreateUIFormColumn_1FieldWith2RowAnd3ColSpan());
            uiFormTab1.Add(CreateUIFormColumn_1FieldWith2RowAnd2ColSpan());
            uiFormTab1.Add(CreateUIFormColumn_1Field());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int maxFieldCount = uiFormTab1.GetMaxRowsInColumns();
            //---------------Test Result -----------------------
            Assert.AreEqual(5, maxFieldCount);
        }


        [Test]
        public void TestUIForm()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab = new UIFormTab("tab1");
            UIForm form = new UIForm();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            uiFormTab.UIForm = form;
            //---------------Test Result -----------------------
            Assert.AreSame(form, uiFormTab.UIForm);
        }


        public UIFormColumn CreateUIFormColumn_2Fields()
        {
            return CreateUIFormColumn_2Fields("prop1");
        }

        public UIFormColumn CreateUIFormColumn_2FieldsWithRowSpan()
        {
            UIFormField field1 = new UIFormField("label1", "prop1", "control", null, null, null, true, null, null, null);
            Hashtable parameters = new Hashtable();
            parameters.Add("rowSpan", 2);
            UIFormField field2 = new UIFormField("label2", "prop2", "control", null, null, null, true, null, parameters, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            uiFormColumn.Add(field2);
            return uiFormColumn;
        }

        public UIFormColumn CreateUIFormColumn_1FieldWith2RowAnd2ColSpan()
        {
            
            Hashtable parameters = new Hashtable();
            parameters.Add("rowSpan", 2);
            parameters.Add("colSpan", 2);
            UIFormField field1 = new UIFormField("label1", "prop1", "control", null, null, null, true, null, parameters, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            return uiFormColumn;
        }

        public UIFormColumn CreateUIFormColumn_1FieldWith2RowAnd3ColSpan()
        {
            
            Hashtable parameters = new Hashtable();
            parameters.Add("rowSpan", 2);
            parameters.Add("colSpan", 3);
            UIFormField field1 = new UIFormField("label1", "prop1", "control", null, null, null, true, null, parameters, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            return uiFormColumn;
        }

        public UIFormColumn CreateUIFormColumn_1Field()
        {
            return CreateUIFormColumn_1Field("prop1");
        }

        public UIFormColumn CreateUIFormColumn_2Fields(string propName)
        {
            UIFormField field1 =
                new UIFormField("label1", propName, "control", null, null, null, true, null, null, null);
            UIFormField field2 = new UIFormField("label2", "prop2", "control", null, null, null, true, null, null, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            uiFormColumn.Add(field2);
            return uiFormColumn;
        }

        public UIFormColumn CreateUIFormColumn_1Field(string propName)
        {
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(new UIFormField("label1", propName, "control", null, null, null, true, null, null, null));
            return uiFormColumn;
        }

        [Test]
        public void TestEqualsDifferentType()
        {
            UIFormTab uiFormTab1 = new UIFormTab();
            Assert.AreNotEqual(uiFormTab1, "bob");
        }

        [Test]
        public void TestAddColumn()
        {
            //---------------Set up test pack-------------------
            UIFormColumn uiFormColumn = new UIFormColumn();
            UIFormTab uiFormTab = new UIFormTab();
            //---------------Assert Precondition----------------
            Assert.IsNull(uiFormColumn.UIFormTab);
            //---------------Execute Test ----------------------
            uiFormTab.Add(uiFormColumn);
            //---------------Test Result -----------------------
            Assert.AreSame(uiFormTab, uiFormColumn.UIFormTab);

        }
    }
}