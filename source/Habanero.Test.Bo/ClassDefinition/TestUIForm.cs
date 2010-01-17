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

using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIForm
    {
        [Test]
        public void Test_Construct_WithUIFormTabs()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = new UIFormTab();
            UIFormTab uiFormTab2 = new UIFormTab();
            UIFormTab uiFormTab3 = new UIFormTab();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            UIForm uiForm = new UIForm(uiFormTab1, uiFormTab2, uiFormTab3);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, uiForm.Count);
            Assert.AreSame(uiFormTab1, uiForm[0]);
            Assert.AreSame(uiFormTab2, uiForm[1]);
            Assert.AreSame(uiFormTab3, uiForm[2]);
        }

        [Test]
        public void TestRemove()
        {
            UIFormTab tab = new UIFormTab();
            UIForm uiForm = new UIForm();
            uiForm.Add(tab);

            Assert.IsTrue(uiForm.Contains(tab));
            uiForm.Remove(tab);
            Assert.IsFalse(uiForm.Contains(tab));
        }

        [Test]
        public void TestCopyTo()
        {
            UIFormTab tab1 = new UIFormTab();
            UIFormTab tab2 = new UIFormTab();
            UIForm uiForm = new UIForm();
            uiForm.Add(tab1);
            uiForm.Add(tab2);

            UIFormTab[] target = new UIFormTab[2];
            uiForm.CopyTo(target, 0);
            Assert.AreEqual(tab1, target[0]);
            Assert.AreEqual(tab2, target[1]);
        }

        // Just gets test coverage up
        [Test]
        public void TestSync()
        {
            UIForm uiForm = new UIForm();
            Assert.AreEqual(typeof(object), uiForm.SyncRoot.GetType());
            Assert.IsFalse(uiForm.IsSynchronized);
        }

        [Test]
        public void TestCloneUIForm()
        {
            //---------------Set up test pack-------------------
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

            //---------------Execute Test ----------------------
            IUIForm clonedForm = uiForm.Clone();

            //---------------Test Result -----------------------
            Assert.IsTrue(uiForm == (UIForm) clonedForm);
            Assert.IsTrue(uiForm.Equals(clonedForm));
            Assert.AreNotSame(uiForm, clonedForm);

            IUIFormTab clonedUIFormTab = clonedForm[0];
            Assert.AreEqual(uiForm[0], clonedUIFormTab,
                              "Should be a deep copy and the columns should be equal but copied");
            Assert.AreNotSame(uiFormTab, clonedUIFormTab, "Should be a deep copy and the columns should be equal but copied (not same)");
            //Verif cloned columns
            Assert.AreEqual(uiFormTab[0], clonedUIFormTab[0]);
            Assert.AreNotSame(uiFormTab[0], clonedUIFormTab[0]);
        }

        [Test]
        public void Test_NotEqualsNull()
        {
            UIForm uiForm1 = new UIForm();
            UIForm uiForm2 = null;
            Assert.IsFalse(uiForm1 == uiForm2);
            Assert.IsTrue(uiForm1 != uiForm2);
            Assert.IsFalse(uiForm1.Equals(uiForm2));
            Assert.AreNotEqual(uiForm1, uiForm2);
        }

        [Test]
        public void Test_NotEqual_DifName()
        {
            //---------------Set up test pack-------------------
            UIForm uiForm1 = new UIForm();
            uiForm1.Title = "Form1";
            UIForm uiForm2 = new UIForm();
            uiForm2.Title = "Form2";

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            bool operatorEquals = uiForm1 == uiForm2;
            bool operatorNotEquals = uiForm1 != uiForm2;
            bool methodEquals = uiForm1.Equals(uiForm2);

            //---------------Test Result -----------------------
            Assert.IsFalse(operatorEquals);
            Assert.IsTrue(operatorNotEquals);
            Assert.IsFalse(methodEquals);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_NotEqual_DifWidth()
        {
            //---------------Set up test pack-------------------
            UIForm uiForm1 = new UIForm();
            uiForm1.Title = "Form1";
            uiForm1.Width = 100;
            UIForm uiForm2 = new UIForm();
            uiForm2.Title = "Form1";
            uiForm2.Width = 200;
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            bool operatorEquals = uiForm1 == uiForm2;
            bool operatorNotEquals = uiForm1 != uiForm2;
            bool methodEquals = uiForm1.Equals(uiForm2);

            //---------------Test Result -----------------------
            Assert.IsFalse(operatorEquals);
            Assert.IsTrue(operatorNotEquals);
            Assert.IsFalse(methodEquals);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_NotSameType()
        {
            //---------------Set up test pack-------------------
            UIForm uiForm1 = new UIForm();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            bool methodEquals = uiForm1.Equals("fedafds");

            //---------------Test Result -----------------------
            Assert.IsFalse(methodEquals);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestEquals_SameTab()
        {
            UIFormTab uiFormTab1 = CreateUIFormTab();

            UIForm uiForm1 = new UIForm();
            uiForm1.Add(uiFormTab1);

            UIForm uiForm2 = new UIForm();
            uiForm2.Add(uiFormTab1);

            Assert.IsTrue(uiForm1 == uiForm2);
            Assert.IsFalse(uiForm1 != uiForm2);
            Assert.IsTrue(uiForm1.Equals(uiForm2));
        }

        [Test]
        public void Test_NotEqual_DiffFormTabCount()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = CreateUIFormTab();

            UIForm uiForm1 = new UIForm();
            uiForm1.Add(uiFormTab1);

            UIForm uiForm2 = new UIForm();
            uiForm2.Add(uiFormTab1);
            uiForm2.Add(CreateUIFormTab());

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            bool operatorEquals = uiForm1 == uiForm2;
            bool operatorNotEquals = uiForm1 != uiForm2;
            bool methodEquals = uiForm1.Equals(uiForm2);

            //---------------Test Result -----------------------
            Assert.IsFalse(operatorEquals);
            Assert.IsTrue(operatorNotEquals);
            Assert.IsFalse(methodEquals);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_NotEqual_DiffTabs()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = CreateUIFormTab();
            uiFormTab1.Name = "tab1";
            UIForm uiForm1 = new UIForm();
            uiForm1.Add(uiFormTab1);
            UIFormTab uiFormTab2 = CreateUIFormTab();
            uiFormTab2.Name = "tab2";
            UIForm uiForm2 = new UIForm();
            uiForm2.Add(uiFormTab2);

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            bool operatorEquals = uiForm1 == uiForm2;
            bool operatorNotEquals = uiForm1 != uiForm2;
            bool methodEquals = uiForm1.Equals(uiForm2);

            //---------------Test Result -----------------------
            Assert.IsFalse(operatorEquals);
            Assert.IsTrue(operatorNotEquals);
            Assert.IsFalse(methodEquals);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Equal_DiffTabs_SameTabName()
        {
            //---------------Set up test pack-------------------
            UIFormTab uiFormTab1 = CreateUIFormTab();
            uiFormTab1.Name = "tab1";
            UIForm uiForm1 = new UIForm();
            uiForm1.Add(uiFormTab1);
            UIFormTab uiFormTab2 = CreateUIFormTab();
            uiFormTab2.Name = "tab1";
            UIForm uiForm2 = new UIForm();
            uiForm2.Add(uiFormTab2);

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            bool operatorEquals = uiForm1 == uiForm2;
            bool operatorNotEquals = uiForm1 != uiForm2;
            bool methodEquals = uiForm1.Equals(uiForm2);

            //---------------Test Result -----------------------
            Assert.IsTrue(operatorEquals);
            Assert.IsFalse(operatorNotEquals);
            Assert.IsTrue(methodEquals);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddUIFormTab()
        {
            //---------------Set up test pack-------------------
            UIForm form = new UIForm();
            UIFormTab tab = new UIFormTab();

            //---------------Assert Precondition----------------
            Assert.IsNull(tab.UIForm);
            //---------------Execute Test ----------------------
            form.Add(tab);
            //---------------Test Result -----------------------
            Assert.AreSame(form, tab.UIForm);
        }

        [Test]
        public void TestUIDef()
        {
            //---------------Set up test pack-------------------
            UIForm uiForm = new UIForm();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            UIDef uiDef = new UIDef("test", uiForm, null);

            //---------------Test Result -----------------------
            Assert.AreSame(uiDef, uiForm.UIDef);
        }

        private UIFormTab CreateUIFormTab()
        {
            UIFormTab uiFormTab1 = new UIFormTab();
            UIFormColumn uiFormColumn = CreateUIFormColumn_2Fields();
            uiFormTab1.Add(uiFormColumn);
            return uiFormTab1;
        }

        public UIFormColumn CreateUIFormColumn_2Fields()
        {
            return CreateUIFormColumn_2Fields("prop1");
        }

        public UIFormColumn CreateUIFormColumn_2Fields(string propName)
        {
            UIFormField field1 =
                new UIFormField("label1", propName, "control", null, null, null, true, null, null, null, LayoutStyle.Label);
            UIFormField field2 = new UIFormField("label2", "prop2", "control", null, null, null, true, null, null, null, LayoutStyle.Label);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            uiFormColumn.Add(field2);
            return uiFormColumn;
        }
    }

}
