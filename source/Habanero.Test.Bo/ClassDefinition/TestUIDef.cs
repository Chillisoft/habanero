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

using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIDef
    {
        [Test]
        public void TestProtectedSets()
        {
            UIDefInheritorStub uiDef = new UIDefInheritorStub();

            Assert.AreEqual("uidef", uiDef.Name);
            uiDef.SetName("newuidef");
            Assert.AreEqual("newuidef", uiDef.Name);

            UIForm uiForm = new UIForm();
            Assert.IsNull(uiDef.UIForm);
            uiDef.SetUIForm(uiForm);
            Assert.AreEqual(uiForm, uiDef.UIForm);

            UIGrid uiGrid = new UIGrid();
            Assert.IsNull(uiDef.UIGrid);
            uiDef.SetUIGrid(uiGrid);
            Assert.AreEqual(uiGrid, uiDef.UIGrid);
        }

        [Test]
        public void Test_GetFormField()
        {
            //---------------Set up test pack-------------------
            UIDefInheritorStub uiDef = new UIDefInheritorStub();
            uiDef.SetUIForm(GetUiForm());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IUIFormField formField = uiDef.GetFormField("prop1");
            //---------------Test Result -----------------------
            Assert.AreEqual("prop1", formField.PropertyName);
        }

        [Test]
        public void Test_GetFormField_NoPropReturnsNull()
        {
            //---------------Set up test pack-------------------
            UIDefInheritorStub uiDef = new UIDefInheritorStub();
            uiDef.SetUIForm(GetUiForm());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IUIFormField formField = uiDef.GetFormField("propDoesNotExist");
            //---------------Test Result -----------------------
            Assert.AreEqual(null, formField);
        }


        [Test]
        public void TestCloneUIFormNull()
        {
            //---------------Set up test pack-------------------
            UIDef uiDef = new UIDef("Name", null, GetUiGrid());
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IUIDef clonedDef = uiDef.Clone();
            //---------------Test Result -----------------------
            Assert.IsTrue(uiDef == (UIDef) clonedDef);
            Assert.IsTrue(uiDef.Equals(clonedDef));
            Assert.AreNotSame(uiDef, clonedDef);

            Assert.AreSame(null, clonedDef.UIForm);
            Assert.AreNotSame(uiDef.UIGrid, clonedDef.UIGrid);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCloneUIGridNull()
        {
            //---------------Set up test pack-------------------
            UIDef uiDef = new UIDef("Name", GetUiForm(), null);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IUIDef clonedDef = uiDef.Clone();
            //---------------Test Result -----------------------
            Assert.IsTrue(uiDef == (UIDef) clonedDef);
            Assert.IsTrue(uiDef.Equals(clonedDef));
            Assert.AreNotSame(uiDef, clonedDef);

            Assert.AreNotSame(uiDef.UIForm, clonedDef.UIForm);
            Assert.AreSame(null, clonedDef.UIGrid);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCloneUIDef()
        {
            //---------------Set up test pack-------------------
            UIForm uiForm = GetUiForm();

            UIGrid uiGrid = GetUiGrid();

            UIDef uiDef = new UIDef("Name", uiForm, uiGrid);
            //---------------Execute Test ----------------------
            IUIDef clonedDef = uiDef.Clone();

            //---------------Test Result -----------------------
            Assert.IsTrue(uiDef == (UIDef) clonedDef);
            Assert.IsTrue(uiDef.Equals(clonedDef));
            Assert.AreNotSame(uiDef, clonedDef);

            Assert.AreNotSame(uiDef.UIForm, clonedDef.UIForm);
            Assert.AreNotSame(uiDef.UIGrid, clonedDef.UIGrid);
        }


        private static UIGrid GetUiGrid()
        {
            UIGridColumn uiGridCol =
                new UIGridColumn
                    ("Head", "Prop", "control", "Assembly", true, 100, PropAlignment.centre, null);
            UIGrid uiGrid = new UIGrid();
            uiGrid.SortColumn = "Prop";
            uiGrid.Add(uiGridCol);
            return uiGrid;
        }

        [Test]
        public void Test_NotEqualsNull()
        {
            UIDef uiDef = new UIDef("", null, null);
            UIDef uiDef2 = null;
            Assert.IsFalse(uiDef.Equals(uiDef2));
            Assert.IsFalse(uiDef == uiDef2);
            Assert.IsTrue(uiDef != uiDef2);
            Assert.AreNotEqual(uiDef, uiDef2);
        }

        [Test]
        public void Test_NotEqual_OtherType()
        {
            //---------------Set up test pack-------------------
            UIDef uiDef = new UIDef("", null, null);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------

            Assert.IsFalse(uiDef.Equals("BNLJ JOLJ"));
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_NotEqual_UIFormDifferent()
        {
            //---------------Set up test pack-------------------

            UIForm uiForm = GetUiForm();

            UIDef uiDef = new UIDef("DDD", uiForm, null);
            UIDef uiDef2 = new UIDef("DDD", null, null);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------


            //---------------Test Result -----------------------

            Assert.IsFalse(uiDef.Equals(uiDef2));
            Assert.IsFalse(uiDef == uiDef2);
            Assert.IsTrue(uiDef != uiDef2);
            //---------------Tear Down -------------------------          
        }

        private static UIForm GetUiForm()
        {
            UIFormField field1 = new UIFormField("label1", "prop1", "control", null, null, null, true, null, null, null, LayoutStyle.Label);
            UIFormField field2 = new UIFormField("label2", "prop2", "control", null, null, null, true, null, null, null, LayoutStyle.Label);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            uiFormColumn.Add(field2);

            UIFormTab uiFormTab = new UIFormTab("Tab1");
            uiFormTab.Add(uiFormColumn);

            UIForm uiForm = new UIForm();
            uiForm.Add(uiFormTab);
            uiForm.Title = "ddd";
            uiForm.Height = 1;
            uiForm.Width = 3;
            return uiForm;
        }

        [Test]
        public void Test_EqualHasTheSameUIForm()
        {
            UIForm uiForm = GetUiForm();
            UIDef uiDef = new UIDef("DDD", uiForm, null);
            UIDef uiDef2 = new UIDef("DDD", uiForm, null);

            Assert.IsTrue(uiDef.Equals(uiDef2));
            Assert.IsTrue(uiDef == uiDef2);
            Assert.IsFalse(uiDef != uiDef2);
        }

        [Test]
        public void Test_EqualHasIdenticalUIForm()
        {
            UIForm uiForm = GetUiForm();
            UIForm uiForm2 = GetUiForm();
            UIDef uiDef = new UIDef("DDD", uiForm, null);
            UIDef uiDef2 = new UIDef("DDD", uiForm2, null);

            AssertAreEqual(uiDef, uiDef2);
        }

        private static void AssertAreEqual(UIDef uiDef, UIDef uiDef2)
        {
            Assert.IsTrue(uiDef.Equals(uiDef2));
            Assert.IsTrue(uiDef == uiDef2);
            Assert.IsFalse(uiDef != uiDef2);
        }

        private static void AssertNotEqual(UIDef uiDef, UIDef uiDef2)
        {
            Assert.IsFalse(uiDef.Equals(uiDef2));
            Assert.IsFalse(uiDef == uiDef2);
            Assert.IsTrue(uiDef != uiDef2);
        }

        [Test]
        public void Test_NonEqualHasNonEqualUIForm()
        {
            UIForm uiForm = GetUiForm();
            UIForm uiForm2 = GetUiForm();
            uiForm2.Title = "DifferentTitle";
            UIDef uiDef = new UIDef("DDD", uiForm, null);
            UIDef uiDef2 = new UIDef("DDD", uiForm2, null);

            AssertNotEqual(uiDef, uiDef2);
        }


        [Test]
        public void Test_NonEqualHasNonEqualUIGrid()
        {
            UIGrid uiGrid = new UIGrid();
            uiGrid.SortColumn = "sort 1";
            UIGrid uiGrid2 = new UIGrid();
            uiGrid2.SortColumn = "Sort Another";
            UIDef uiDef = new UIDef("DDD", null, uiGrid);
            UIDef uiDef2 = new UIDef("DDD", null, uiGrid2);

            AssertNotEqual(uiDef, uiDef2);
        }

        [Test]
        public void Test_NotEqual_DiffName()
        {
            UIDef uiDef = new UIDef("DDD", null, null);
            UIDef uiDef2 = new UIDef("DDD_1", null, null);

            Assert.IsFalse(uiDef.Equals(uiDef2));
            Assert.IsFalse(uiDef == uiDef2);
            Assert.IsTrue(uiDef != uiDef2);
        }

        [Test]
        public void TestUIDefCol()
        {
            //---------------Set up test pack-------------------
            UIDef uiDef = new UIDef("test", null, null);
            UIDefCol uiDefCol = new UIDefCol();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            uiDef.UIDefCol = uiDefCol;
            //---------------Test Result -----------------------
            Assert.AreSame(uiDefCol, uiDef.UIDefCol);

        }

        [Test]
        public void Test_ClassName_WhenClassDefNull_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            UIDef uiDef = new UIDef("test", null, null);
            //---------------Assert Precondition----------------
            Assert.IsNull(uiDef.ClassDef);
            //---------------Execute Test ----------------------
            string className = uiDef.ClassName;
            //---------------Test Result -----------------------
            Assert.IsEmpty(className);
        }
        [Test]
        public void Test_ClassName_WhenClassDefSet_ShouldReturnClassDefClassName()
        {
            //---------------Set up test pack-------------------
            IUIDef uiDef = new UIDef("test", null, null);
            var classDef = MockRepository.GenerateStub<IClassDef>();
            classDef.ClassName = GetRandomString();
            uiDef.ClassDef = classDef;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(uiDef.ClassDef);
            Assert.IsNotNullOrEmpty(classDef.ClassName);
            //---------------Execute Test ----------------------
            string className = uiDef.ClassName;
            //---------------Test Result -----------------------
            Assert.AreSame(classDef.ClassName, className);
        }

        private static string GetRandomString()
        {
            return TestUtil.GetRandomString();
        }

        // Grants access to protected methods
        private class UIDefInheritorStub : UIDef
        {
            public UIDefInheritorStub()
                : base("uidef", null, null)
            {
            }

            public void SetName(string name)
            {
                Name = name;
            }

            public void SetUIForm(UIForm uiForm)
            {
                UIForm = uiForm;
            }

            public void SetUIGrid(UIGrid uiGrid)
            {
                UIGrid = uiGrid;
            }
        }
    }
}