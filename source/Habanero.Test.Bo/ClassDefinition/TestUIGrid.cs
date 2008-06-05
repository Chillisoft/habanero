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
    public class TestUIGrid
    {
        [Test]
        public void TestRemove()
        {
            UIGridColumn column = new UIGridColumn("heading", null, null, null, false,
                100, UIGridColumn.PropAlignment.left, null);
            UIGrid uiGrid = new UIGrid();
            uiGrid.Add(column);

            Assert.IsTrue(uiGrid.Contains(column));
            uiGrid.Remove(column);
            Assert.IsFalse(uiGrid.Contains(column));
        }

        [Test]
        public void TestCopyTo()
        {
            UIGridColumn column1 = new UIGridColumn("heading", null, null, null, false,
                100, UIGridColumn.PropAlignment.left, null);
            UIGridColumn column2 = new UIGridColumn("heading", null, null, null, false,
                100, UIGridColumn.PropAlignment.left, null);
            UIGrid uiGrid = new UIGrid();
            uiGrid.Add(column1);
            uiGrid.Add(column2);

            UIGridColumn[] target = new UIGridColumn[2];
            uiGrid.CopyTo(target, 0);
            Assert.AreEqual(column1, target[0]);
            Assert.AreEqual(column2, target[1]);
        }

        // Just gets test coverage up
        [Test]
        public void TestSync()
        {
            UIGrid uiGrid = new UIGrid();
            Assert.AreEqual(typeof(object), uiGrid.SyncRoot.GetType());
            Assert.IsFalse(uiGrid.IsSynchronized);
        }


        [Test]
        public void TestCloneUIGrid()
        {
            UIGridColumn uiGridCol = new UIGridColumn("Head", "Prop", "control", "Assembly",true,100, UIGridColumn.PropAlignment.centre, null);
            UIGrid uiGrid = new UIGrid();
            uiGrid.SortColumn = "Prop";
            uiGrid.Add(uiGridCol);

            //---------------Execute Test ----------------------
            UIGrid clonedGrid = uiGrid.Clone();

            //---------------Test Result -----------------------
            Assert.IsTrue(uiGrid.Equals(clonedGrid));
            Assert.IsTrue(uiGrid == clonedGrid);
            Assert.IsFalse(uiGrid != clonedGrid);
            Assert.AreEqual(uiGrid[0], clonedGrid[0],
                              "Should be a deep copy and the columns should be equal but copied");
            Assert.AreNotSame(uiGrid[0], clonedGrid[0], "Should be a deep copy and the columns should be equal but copied (not same)");
        }

        [Test]
        public void Test_NotEqualsNull()
        {
            UIGrid uiGrid = new UIGrid();
            UIGrid uiGrid2 = null;
            Assert.IsFalse(uiGrid.Equals(uiGrid2));
            Assert.IsFalse(uiGrid == uiGrid2);
            Assert.IsTrue(uiGrid != uiGrid2);
            Assert.AreNotEqual(uiGrid, uiGrid2);
        }

        [Test]
        public void Test_NotEqual_OtherType()
        {
            //---------------Set up test pack-------------------
            UIGrid uiGrid = new UIGrid();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            
            Assert.IsFalse(uiGrid.Equals("BNLJ JOLJ"));
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_EqualHasTheSameGridColumn()
        {
            UIGrid uiGrid = new UIGrid();
            UIGridColumn uiGridColumn = GetUiGridColumn();
            uiGrid.Add(uiGridColumn);

            UIGrid uiGrid2 = new UIGrid();
            //UIGridColumn uiGridColumn2 = GetUiGridColumn();
            uiGrid2.Add(uiGridColumn);

            Assert.IsTrue(uiGrid.Equals(uiGrid2));
            Assert.IsTrue(uiGrid == uiGrid2);
            Assert.IsFalse(uiGrid != uiGrid2);
        }

        [Test]
        public void Test_NotEqual_HasDifferentNumbersOfColumns()
        {
            //---------------Set up test pack-------------------
            UIGrid uiGrid = new UIGrid();
            UIGridColumn uiGridColumn = GetUiGridColumn();
            uiGrid.Add(uiGridColumn);
            uiGrid.Add(GetUiGridColumn());

            UIGrid uiGrid2 = new UIGrid();
            uiGrid2.Add(uiGridColumn);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, uiGrid.Count);
            Assert.AreEqual(1, uiGrid2.Count);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(uiGrid.Equals(uiGrid2));
            Assert.IsFalse(uiGrid == uiGrid2);
            Assert.IsTrue(uiGrid != uiGrid2);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_NotEqualHasTheDifferentPropName_GridColumn()
        {
            UIGrid uiGrid = new UIGrid();
            UIGridColumn uiGridColumn = GetUiGridColumn();
            uiGrid.Add(uiGridColumn);

            UIGrid uiGrid2 = new UIGrid();
            UIGridColumn uiGridColumn2 = GetUiGridColumn("Diff Prop Name");
            uiGrid2.Add(uiGridColumn2);

            Assert.IsFalse(uiGrid.Equals(uiGrid2));
            Assert.IsFalse(uiGrid == uiGrid2);
            Assert.IsTrue(uiGrid != uiGrid2);
        }

        [Test]
        public void Test_EqualHasCopyOfGridColumn()
        {
            //---------------Set up test pack-------------------
            UIGrid uiGrid = new UIGrid();
            UIGridColumn uiGridColumn = GetUiGridColumn();
            uiGrid.Add(uiGridColumn);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            UIGrid uiGrid2 = new UIGrid();
            uiGrid2.Add(uiGridColumn.Clone());
            //---------------Test Result -----------------------
            Assert.IsTrue(uiGrid.Equals(uiGrid2));
            Assert.IsTrue(uiGrid == uiGrid2);
            Assert.IsFalse(uiGrid != uiGrid2);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_NotEqualSortColumn()
        {
            //---------------Set up test pack-------------------
            UIGrid uiGrid1 = new UIGrid();
            UIGrid uiGrid2 = new UIGrid();

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            uiGrid1.SortColumn = "col";
            uiGrid2.SortColumn = "col2";

            //---------------Test Result -----------------------
            Assert.IsFalse(uiGrid1.Equals(uiGrid2));
            Assert.IsFalse(uiGrid1 == uiGrid2);
            Assert.IsTrue(uiGrid1 != uiGrid2);
        }


        private static UIGridColumn GetUiGridColumn()
        {
            return GetUiGridColumn("");
        }

        private static UIGridColumn GetUiGridColumn(string columnProperty)
        {
            return new UIGridColumn("", columnProperty, "", "", false, 0, UIGridColumn.PropAlignment.centre, null);
        }

        //[Test]
        //public void Test_NotEqual_DifName()
        //{
        //    //---------------Set up test pack-------------------
        //    UIForm uiForm1 = new UIForm();
        //    uiForm1.Title = "Form1";
        //    UIForm uiForm2 = new UIForm();
        //    uiForm2.Title = "Form2";

        //    //--------------Assert PreConditions----------------            

        //    //---------------Execute Test ----------------------
        //    bool operatorEquals = uiForm1 == uiForm2;
        //    bool operatorNotEquals = uiForm1 != uiForm2;
        //    bool methodEquals = uiForm1.Equals(uiForm2);

        //    //---------------Test Result -----------------------
        //    Assert.IsFalse(operatorEquals);
        //    Assert.IsTrue(operatorNotEquals);
        //    Assert.IsFalse(methodEquals);
        //    //---------------Tear Down -------------------------          
        //}

        //[Test]
        //public void Test_NotEqual_DifWidth()
        //{
        //    //---------------Set up test pack-------------------
        //    UIForm uiForm1 = new UIForm();
        //    uiForm1.Title = "Form1";
        //    uiForm1.Width = 100;
        //    UIForm uiForm2 = new UIForm();
        //    uiForm2.Title = "Form1";
        //    uiForm2.Width = 200;
        //    //--------------Assert PreConditions----------------            

        //    //---------------Execute Test ----------------------
        //    bool operatorEquals = uiForm1 == uiForm2;
        //    bool operatorNotEquals = uiForm1 != uiForm2;
        //    bool methodEquals = uiForm1.Equals(uiForm2);

        //    //---------------Test Result -----------------------
        //    Assert.IsFalse(operatorEquals);
        //    Assert.IsTrue(operatorNotEquals);
        //    Assert.IsFalse(methodEquals);
        //    //---------------Tear Down -------------------------          
        //}

        //[Test]
        //public void Test_NotSameType()
        //{
        //    //---------------Set up test pack-------------------
        //    UIForm uiForm1 = new UIForm();
        //    //--------------Assert PreConditions----------------            

        //    //---------------Execute Test ----------------------
        //    bool methodEquals = uiForm1.Equals("fedafds");

        //    //---------------Test Result -----------------------
        //    Assert.IsFalse(methodEquals);
        //    //---------------Tear Down -------------------------          
        //}
        //[Test]
        //public void TestEquals_SameTab()
        //{
        //    UIFormTab uiFormTab1 = CreateUIFormTab();

        //    UIForm uiForm1 = new UIForm();
        //    uiForm1.Add(uiFormTab1);

        //    UIForm uiForm2 = new UIForm();
        //    uiForm2.Add(uiFormTab1);

        //    Assert.IsTrue(uiForm1 == uiForm2);
        //    Assert.IsFalse(uiForm1 != uiForm2);
        //    Assert.IsTrue(uiForm1.Equals(uiForm2));
        //}

        //[Test]
        //public void Test_NotEqual_DiffFormTabCount()
        //{
        //    //---------------Set up test pack-------------------
        //    UIFormTab uiFormTab1 = CreateUIFormTab();

        //    UIForm uiForm1 = new UIForm();
        //    uiForm1.Add(uiFormTab1);

        //    UIForm uiForm2 = new UIForm();
        //    uiForm2.Add(uiFormTab1);
        //    uiForm2.Add(CreateUIFormTab());

        //    //--------------Assert PreConditions----------------            

        //    //---------------Execute Test ----------------------
        //    bool operatorEquals = uiForm1 == uiForm2;
        //    bool operatorNotEquals = uiForm1 != uiForm2;
        //    bool methodEquals = uiForm1.Equals(uiForm2);

        //    //---------------Test Result -----------------------
        //    Assert.IsFalse(operatorEquals);
        //    Assert.IsTrue(operatorNotEquals);
        //    Assert.IsFalse(methodEquals);
        //    //---------------Tear Down -------------------------          
        //}

        //[Test]
        //public void Test_NotEqual_DiffTabs()
        //{
        //    //---------------Set up test pack-------------------
        //    UIFormTab uiFormTab1 = CreateUIFormTab();
        //    uiFormTab1.Name = "tab1";
        //    UIForm uiForm1 = new UIForm();
        //    uiForm1.Add(uiFormTab1);
        //    UIFormTab uiFormTab2 = CreateUIFormTab();
        //    uiFormTab2.Name = "tab2";
        //    UIForm uiForm2 = new UIForm();
        //    uiForm2.Add(uiFormTab2);

        //    //--------------Assert PreConditions----------------            

        //    //---------------Execute Test ----------------------
        //    bool operatorEquals = uiForm1 == uiForm2;
        //    bool operatorNotEquals = uiForm1 != uiForm2;
        //    bool methodEquals = uiForm1.Equals(uiForm2);

        //    //---------------Test Result -----------------------
        //    Assert.IsFalse(operatorEquals);
        //    Assert.IsTrue(operatorNotEquals);
        //    Assert.IsFalse(methodEquals);
        //    //---------------Tear Down -------------------------          
        //}

        //[Test]
        //public void Test_Equal_DiffTabs_SameTabName()
        //{
        //    //---------------Set up test pack-------------------
        //    UIFormTab uiFormTab1 = CreateUIFormTab();
        //    uiFormTab1.Name = "tab1";
        //    UIForm uiForm1 = new UIForm();
        //    uiForm1.Add(uiFormTab1);
        //    UIFormTab uiFormTab2 = CreateUIFormTab();
        //    uiFormTab2.Name = "tab1";
        //    UIForm uiForm2 = new UIForm();
        //    uiForm2.Add(uiFormTab2);

        //    //--------------Assert PreConditions----------------            

        //    //---------------Execute Test ----------------------
        //    bool operatorEquals = uiForm1 == uiForm2;
        //    bool operatorNotEquals = uiForm1 != uiForm2;
        //    bool methodEquals = uiForm1.Equals(uiForm2);

        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(operatorEquals);
        //    Assert.IsFalse(operatorNotEquals);
        //    Assert.IsTrue(methodEquals);
        //    //---------------Tear Down -------------------------          
        //}

        [Test]
        public void Test_Indexer_FindBy_PropertyName()
        {
            //---------------Set up test pack-------------------
            UIGrid uiGrid = new UIGrid();
            UIGridColumn uiGridColumn = GetUiGridColumn();
            uiGrid.Add(uiGridColumn);
            UIGridColumn uiGridColumn2 = GetUiGridColumn("Diff Prop Name");
            uiGrid.Add(uiGridColumn2);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, uiGrid.Count);
            //---------------Execute Test ----------------------
            UIGridColumn column = uiGrid[uiGridColumn2.PropertyName];
            //---------------Test Result -----------------------
            Assert.AreEqual(uiGridColumn2, column);
        }

        [Test]
        public void Test_Indexer_FindBy_PropertyName_DoesntExistReturnsNull()
        {
            //---------------Set up test pack-------------------
            UIGrid uiGrid = new UIGrid();
            UIGridColumn uiGridColumn = GetUiGridColumn();
            uiGrid.Add(uiGridColumn);
            UIGridColumn uiGridColumn2 = GetUiGridColumn("Diff Prop Name");
            uiGrid.Add(uiGridColumn2);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, uiGrid.Count);
            //---------------Execute Test ----------------------
            UIGridColumn column = uiGrid["nonexistent property"];
            //---------------Test Result -----------------------
            Assert.IsNull(column);
        }
    }
}