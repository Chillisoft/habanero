// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System;
using System.Collections.Generic;
using System.Drawing;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestControlFactory
    {
        private IControlFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _factory = GetControlFactory();
            ClassDef.ClassDefs.Clear();
            
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }
        protected virtual int GetStandardTextBoxHeight()
        {
            return 21;
        }
        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = new Habanero.UI.Win.ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected virtual int GetBoldTextExtraWidth()
        {
            return 10;
        }

        protected virtual Type GetCustomGridColumnType()
        {
            return typeof(CustomDataGridViewColumnWin);
        }

        protected virtual Type GetMasterGridColumnType()
        {
            return typeof(System.Windows.Forms.DataGridViewColumn);
        }

        protected virtual Type GetMasterTextBoxGridColumnType()
        {
            return typeof(System.Windows.Forms.DataGridViewTextBoxColumn);
        }

        protected virtual Type GetHabaneroMasterGridColumnType()
        {
            return typeof(Habanero.UI.Win.DataGridViewColumnWin);
        }

        protected virtual string GetUINameSpace()
        {
            return "System.Windows.Forms";
        }

        protected virtual void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType)
        {
            Habanero.UI.Win.DataGridViewColumnWin columnWin = (Habanero.UI.Win.DataGridViewColumnWin)createdColumn;
            System.Windows.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
            Assert.AreEqual(expectedColumnType, column.GetType());
        }

        [TestFixture]
        public class TestControlFactoryWin : TestControlFactory
        {
            [Test]
            public void TestCreateCheckBoxWin()
            {
                //---------------Set up test pack-------------------
                //---------------Execute Test ----------------------

                ICheckBox cbx = GetControlFactory().CreateCheckBox();
                //---------------Test Result -----------------------
                Assert.IsFalse(cbx.Checked);    
            }

            [Test]
            public void TestCreateCheckBoxWin_WithDefault()
            {
                //---------------Set up test pack-------------------
                //---------------Execute Test ----------------------

                ICheckBox cbx = GetControlFactory().CreateCheckBox(true);
                //---------------Test Result -----------------------
                Assert.IsTrue(cbx.Checked);
                         
            }

            [Test]
            public void TestCreateControl_ViaType_CreateCombo()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.ComboBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.Win.ComboBoxWin), controlHabanero.GetType());
                Assert.AreEqual(GetStandardTextBoxHeight(), controlHabanero.Height);
            }

            [Test]
            public void TestCreateControl_ViaType_CreateEditableGridControl()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(Habanero.UI.Win.EditableGridControlWin));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.Win.EditableGridControlWin), controlHabanero.GetType());
                  
            }

            [Test]
            public void TestCreateControl_ViaType_CreateCheckBox()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.CheckBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.Win.CheckBoxWin), controlHabanero.GetType());
                  
            }
            [Test]
            public void TestCreateControl_ViaType_CreateTextBox()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.TextBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.Win.TextBoxWin), controlHabanero.GetType());
                  
            }
            [Test]
            public void TestCreateControl_ViaType_CreateListBox()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.ListBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.Win.ListBoxWin), controlHabanero.GetType());
                  
            }
            [Test]
            public void TestCreateControl_ViaType_CreateDateTimePicker()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.DateTimePicker));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.Win.DateTimePickerWin), controlHabanero.GetType());
                  
            }

            [Test]
            public void TestCreateControl_ViaType_NumericUpDown()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                object controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.NumericUpDown));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.Win.NumericUpDownWin), controlHabanero.GetType());
                  
            }

            [Test]
            public void TestLoadWithIncorrectControlLibrary_RaisesAppropriateError()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                try
                {
                    _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.TextBox));
                    //---------------Verify Result -----------------------
                }
                catch (UnknownTypeNameException ex)
                {
                    StringAssert.Contains("The control type name Gizmox.WebGUI.Forms.TextBox does not inherit from System.Windows.Forms.Control", ex.Message);
                }
            }

            

            [Test]
            public void TestCreateSpecifiedControlType()
            {
                //---------------Set up test pack-------------------
                const string typeName = "TextBox";
                const string assemblyName = "System.Windows.Forms";
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero control = _factory.CreateControl(typeName, assemblyName);
                //---------------Verify Result -----------------------
                Assert.IsTrue(control is System.Windows.Forms.TextBox);
                  
            }

            [Test, Ignore("Not implemented for Win")]
            public override void TestCreateDataGridViewColumn_WithTypeName_Image()
            {
                base.TestCreateDataGridViewColumn_WithTypeName_Image();
            }
        }
        [TestFixture]
        public class TestControlFactoryVWG : TestControlFactory
        {
            protected override int GetStandardTextBoxHeight()
            {
                return 20;
            }

            protected override IControlFactory GetControlFactory()
            {
                ControlFactoryVWG factory = new Habanero.UI.VWG.ControlFactoryVWG();
                GlobalUIRegistry.ControlFactory = factory;
                return factory;
            }

            protected override int GetBoldTextExtraWidth()
            {
                return 14;
            }

            protected override Type GetCustomGridColumnType()
            {
                return typeof(CustomDataGridViewColumnVWG);
            }

            protected override Type GetMasterGridColumnType()
            {
                return typeof(Gizmox.WebGUI.Forms.DataGridViewColumn);
            }

            protected override Type GetMasterTextBoxGridColumnType()
            {
                return typeof(Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn);
            }

            protected override Type GetHabaneroMasterGridColumnType()
            {
                return typeof(Habanero.UI.VWG.DataGridViewColumnVWG);
            }

            protected override string GetUINameSpace()
            {
                return "Gizmox.WebGUI.Forms";
            }

            protected override void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType)
            {
                Habanero.UI.VWG.DataGridViewColumnVWG columnWin = (Habanero.UI.VWG.DataGridViewColumnVWG)createdColumn;
                Gizmox.WebGUI.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
                Assert.AreEqual(expectedColumnType, column.GetType());
            }

            [Test]
            public void TestCreateCheckBoxVWG()
            {
                //---------------Set up test pack-------------------
                 //---------------Execute Test ----------------------

                ICheckBox cbx = GetControlFactory().CreateCheckBox(false);
                //---------------Test Result -----------------------
                int expectedHeightAndWidth = GetControlFactory().CreateTextBox().Height;
                Assert.AreEqual(expectedHeightAndWidth, cbx.Height);
                Assert.AreEqual(expectedHeightAndWidth, cbx.Width);
                Assert.IsFalse(cbx.Checked);
                         
            }

            [Test]
            public void TestCreateControl_ViaType_CreateCombo()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.ComboBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.VWG.ComboBoxVWG), controlHabanero.GetType());
                  
            }
            [Test]
            public void TestCreateControl_ViaType_CreateCheckBox()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.CheckBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.VWG.CheckBoxVWG), controlHabanero.GetType());
                  
            }
            [Test]
            public void TestCreateControl_ViaType_CreateTextBox()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.TextBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.VWG.TextBoxVWG), controlHabanero.GetType());
                  
            }
            [Test]
            public void TestCreateControl_ViaType_CreateListBox()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.ListBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.VWG.ListBoxVWG), controlHabanero.GetType());
                  
            }
            [Test]
            public void TestCreateControl_ViaType_CreateDateTimePicker()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.DateTimePicker));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.VWG.DateTimePickerVWG), controlHabanero.GetType());
                  
            }

            [Test]
            public void TestCreateControl_ViaType_NumericUpDown()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero controlHabanero = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.NumericUpDown));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlHabanero);
                Assert.AreEqual(typeof(Habanero.UI.VWG.NumericUpDownVWG), controlHabanero.GetType());
                Assert.AreEqual(_factory.CreateTextBox().Height, controlHabanero.Height);
                  
            }

            [Test]
            public void TestLoadWithIncorrectControlLibrary_RaisesAppropriateError()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                try
                {
                    _factory.CreateControl(typeof (System.Windows.Forms.TextBox));
                    //---------------Verify Result -----------------------
                }
                catch (UnknownTypeNameException ex)
                {
                    StringAssert.Contains("The control type name System.Windows.Forms.TextBox does not inherit from Gizmox.WebGUI.Forms.Control", ex.Message);
                }
                //The control type name System.Windows.Forms.TextBox does not inherit from Gizmox.WebGUI.Forms.Control
            }


            [Test]
            public void TestCreateComboBox()
            {
                //---------------Set up test pack-------------------

                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IComboBox comboBox = _factory.CreateComboBox();
                //---------------Verify Result -----------------------
                Assert.IsNotNull(comboBox);
                Assert.IsTrue(comboBox.TabStop);
                int expectedHeight = _factory.CreateTextBox().Height;
                Assert.AreEqual(expectedHeight, comboBox.Height);
                  
            }
            [Test]
            public void TestCreateSpecifiedControlType()
            {
                //---------------Set up test pack-------------------
                const string typeName = "TextBox";
                const string assemblyName = "Gizmox.WebGUI.Forms";
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero control = _factory.CreateControl(typeName, assemblyName);
                //---------------Verify Result -----------------------
                Assert.IsTrue(control is Gizmox.WebGUI.Forms.TextBox);
                  
            }
            //This has not been implemented for win and is therefore overriden here with an implementation
            [Test]
            public override void TestCreateDataGridViewColumn_WithTypeName_Image()
            {
                //---------------Set up test pack-------------------
                IDataGridViewImageColumn dataGridViewNumericUpDownColumn = GetControlFactory().CreateDataGridViewImageColumn();
                //-------------Assert Preconditions -------------

                //---------------Execute Test ----------------------
                IDataGridViewColumn dataGridViewColumn = GetControlFactory().
                    CreateDataGridViewColumn("DataGridViewImageColumn", null);
                //---------------Test Result -----------------------
                Assert.IsNotNull(dataGridViewColumn);
                Assert.IsInstanceOfType(typeof(IDataGridViewImageColumn), dataGridViewColumn);
                Assert.AreSame(dataGridViewNumericUpDownColumn.GetType(), dataGridViewColumn.GetType());
            }
            [Test, Ignore("Not implemented for VWG")]
            public override void TestCreateDataGridViewColumn_WithTypeName_NumericUpDown()
            {
                base.TestCreateDataGridViewColumn_WithTypeName_NumericUpDown();
            }

            [Test, Ignore("Not implemented for VWG")]
            public override void TestCreateDataGridViewColumn_WithTypeName_DateTimePicker()
            {
                base.TestCreateDataGridViewColumn_WithTypeName_DateTimePicker();
            }
        }



        [Test]
        public void Test_CreateMainMenu()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero mmenu = _factory.CreateMainMenu();
            //---------------Test Result -----------------------
            Assert.IsNotNull(mmenu);
            TestUtil.AssertStringEmpty(mmenu.Name, "mmenu.Name");
        }

        [Test]
        public void Test_CreateMenuItem()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();            
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem mmenu = _factory.CreateMenuItem(name);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mmenu);
            Assert.AreEqual(name, mmenu.Text);
        }

        [Test]
        public void Test_CreateMenuItem_FromHabaneroMenuItem()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu.Item item = new HabaneroMenu.Item(TestUtil.GetRandomString());          
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem mmenu = _factory.CreateMenuItem(item);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mmenu);
            Assert.AreEqual(item.Name, mmenu.Text);
        }

        [Test]
        public void TestCreateLabel_NoText()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ILabel lbl = _factory.CreateLabel();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(lbl);
            Assert.IsFalse(lbl.TabStop);
        }

        [Test]
        public void TestCreateLabel_Text()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string labelText = "test label";
            //---------------Execute Test ----------------------

            ILabel lbl = _factory.CreateLabel(labelText);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(lbl);
            Assert.IsFalse(lbl.TabStop);
            Assert.AreEqual(labelText, lbl.Text);
            Assert.AreNotEqual(labelText, lbl.Name);
            Assert.AreEqual(lbl.PreferredWidth, lbl.Width);
            //TODO_Port_DoTest lbl.FlatStyle = FlatStyle.Standard;
        }

        [Test]
        public void TestCreateLabel_BoldText()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string labelText = "test label";
            //---------------Execute Test ----------------------

            ILabel lbl = _factory.CreateLabel(labelText, true);
            //---------------Verify Result -----------------------
            //Assert.AreEqual(lbl.PreferredWidth + 10, lbl.Width);
            Assert.AreEqual(lbl.PreferredWidth + GetBoldTextExtraWidth(), lbl.Width);
            Font expectedFont = new Font(lbl.Font, FontStyle.Bold);
            Assert.AreEqual(expectedFont, lbl.Font);
              
        }

        [Test]
        public void TestCreateButton()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string buttonText = "test label";
            //---------------Execute Test ----------------------

            IButton button = _factory.CreateButton(buttonText);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(button);
            Assert.IsTrue(button.TabStop);
            Assert.AreEqual(buttonText, button.Text);
            Assert.AreEqual(buttonText, button.Name);
            int expectedButtonWidth = _factory.CreateLabel(buttonText).PreferredWidth + 20;
            Assert.AreEqual(buttonText, button.Name);
            Assert.AreEqual(expectedButtonWidth, button.Width);
            //To_Test: btn.FlatStyle = FlatStyle.System;
        }

        [Test]
        public void TestCreateGroupBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IGroupBox groupBox = _factory.CreateGroupBox();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(groupBox);
            Assert.AreEqual("", groupBox.Text);
            Assert.AreEqual("", groupBox.Name);
            const int expectedWidth = 200;
            Assert.AreEqual(expectedWidth, groupBox.Width);
        }

        [Test]
        public void TestCreateGroupBox_WithText()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string groupBoxText = "test label";
            //---------------Execute Test ----------------------

            IGroupBox groupBox = _factory.CreateGroupBox(groupBoxText);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(groupBox);
            Assert.AreEqual(groupBoxText, groupBox.Text);
            Assert.AreEqual(groupBoxText, groupBox.Name);
            const int expectedWidth = 200;
            Assert.AreEqual(groupBoxText, groupBox.Name);
            Assert.AreEqual(expectedWidth, groupBox.Width);
              
        }

        [Test]
        public void TestCreateDataGridView()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            
            //---------------Execute Test ----------------------

            IDataGridView dataGridView = _factory.CreateDataGridView();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(dataGridView);
              
        }

        [Test]
        public void TestCreateBOEditorForm_BoParam()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO businessObject = new MyBO();
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm boEditorForm = _factory.CreateBOEditorForm(businessObject);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(boEditorForm);
            Assert.AreSame(businessObject, boEditorForm.PanelInfo.BusinessObject);
        }

        [Test]
        public void TestCreateBOEditorForm_BoParam_UiDefNameParam()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO businessObject = new MyBO();
            const string uiDefName = "Alternate";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm boEditorForm = _factory.CreateBOEditorForm(businessObject, uiDefName);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(boEditorForm);
            Assert.AreSame(businessObject, boEditorForm.PanelInfo.BusinessObject);
            Assert.AreEqual(uiDefName, boEditorForm.PanelInfo.UIForm.UIDef.Name);
        }

        [Test]
        public void TestCreateBOEditorForm_BoParam_UiDefNameParam_PostObjectPersistingParam()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO businessObject = new MyBO();
            const string uiDefName = "Alternate";
            PostObjectEditDelegate action = delegate {  };
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm boEditorForm = _factory.CreateBOEditorForm(businessObject, uiDefName, action);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(boEditorForm);
            Assert.AreSame(businessObject, boEditorForm.PanelInfo.BusinessObject);
            Assert.AreEqual(uiDefName, boEditorForm.PanelInfo.UIForm.UIDef.Name);
        }

        [Test]
        public void TestCreateBOEditorForm_BoParam_UiDefNameParam_GroupControlCreator()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO businessObject = new MyBO();
            const string uiDefName = "Alternate";
            GroupControlCreator controlCreator = GetControlFactory().CreateTabControl;
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm boEditorForm = _factory.CreateBOEditorForm(businessObject, uiDefName, controlCreator);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(boEditorForm);
            Assert.AreSame(businessObject, boEditorForm.PanelInfo.BusinessObject);
            Assert.AreEqual(uiDefName, boEditorForm.PanelInfo.UIForm.UIDef.Name);
            Assert.AreSame(controlCreator, boEditorForm.GroupControlCreator);
        }

        [Test]
        public void TestCreateControlWithNullAssemblyName()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl("NumericUpDown", null);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(INumericUpDown), control);
        }

        [Test]
        public void TestCreateButton_WithEventHandler()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string buttonText = "test label";
            bool buttonClicked = false;
            EventHandler handler = delegate { buttonClicked = true; };

            //---------------Execute Test ----------------------

            IButton button = _factory.CreateButton(buttonText, handler);

            button.PerformClick();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(button);
            Assert.IsTrue(button.TabStop);
            Assert.AreEqual(buttonText, button.Text);
            Assert.AreEqual(buttonText, button.Name);
            int expectedButtonWidth = _factory.CreateLabel(buttonText).PreferredWidth + 20;
            Assert.AreEqual(buttonText, button.Name);
            Assert.AreEqual(expectedButtonWidth, button.Width);
            Assert.IsTrue(buttonClicked);
            //To_Test: btn.FlatStyle = FlatStyle.System;
              
        }

        [Test]
        public void TestCreateTextBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ITextBox textBox = _factory.CreateTextBox();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(textBox);
            Assert.IsTrue(textBox.TabStop);
              
        }

        [Test]
        public void TestCreatePasswordTextBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ITextBox textBox = _factory.CreatePasswordTextBox();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(textBox);
            Assert.IsTrue(textBox.TabStop);
            Assert.AreEqual('*', textBox.PasswordChar);
              
        }

        [Test]
        public void TestCreateProgressBar()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IProgressBar progressBar = _factory.CreateProgressBar();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(progressBar);
            Assert.AreEqual(0, progressBar.Minimum);
            Assert.AreEqual(100, progressBar.Maximum);
            Assert.AreEqual(10, progressBar.Step);
            Assert.AreEqual(0, progressBar.Value);
              
        }

        [Test]
        public void TestCreateTreeView()
        {
            //---------------Set up test pack-------------------
            const string treeViewname = "TVNAme";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ITreeView treeView = _factory.CreateTreeView(treeViewname);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(treeView);
            Assert.IsTrue(treeView.TabStop);
            Assert.AreEqual(treeViewname, treeView.Name);
              
        }

        [Test]
        public void TestCreateTreeNode()
        {
            //---------------Set up test pack-------------------
            const string treeNodeName = "TVNodeName";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ITreeNode treeNode = _factory.CreateTreeNode(treeNodeName);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(treeNode);
            Assert.AreEqual(treeNodeName, treeNode.Text);
              
        }

        [Test]
        public void TestCreatePanel()
        {
            //---------------Set up test pack-------------------
            const string pnlName = "PanelName";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IPanel panelName = _factory.CreatePanel(pnlName, GetControlFactory());
            //---------------Verify Result -----------------------
            Assert.IsNotNull(panelName);
            //Assert.IsTrue(treeView.TabStop);

            Assert.AreEqual(pnlName, panelName.Name);
              
        }

        [Test]
        public void TestCreateUserControl()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IUserControlHabanero userControlHabanero = _factory.CreateUserControl();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(userControlHabanero);
              
        }

        [Test]
        public void TestCreateUserControl_WithName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IUserControlHabanero userControlHabanero = _factory.CreateUserControl(name);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(userControlHabanero);
            Assert.AreEqual(name, userControlHabanero.Name);
              
        }

        [Test]
        public void TestCreateDateTimePicker()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDateTimePicker dateTimePicker = _factory.CreateDateTimePicker();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(dateTimePicker);
            //Assert.IsTrue(treeView.TabStop);
              
        }
                [Test]
        public void TestCreateDateTimePicker_DefaultValue()
        {
            //---------------Set up test pack-------------------
                    DateTime testDate = DateTime.Today.AddDays(-3);
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
                    IDateTimePicker dateTimePicker = _factory.CreateDateTimePicker(testDate);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(dateTimePicker);
            Assert.AreEqual(testDate,dateTimePicker.Value);
              
        }

        [Test]
        public void TestCreateMonthPicker()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDateTimePicker dateTimePicker = _factory.CreateMonthPicker();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(dateTimePicker);
            Assert.AreEqual("MMM yyyy", dateTimePicker.CustomFormat);
              
        }

        [Test]
        public void TestCreateRadioButton()
        {
            //---------------Set up test pack-------------------
            string text = TestUtil.GetRandomString();
            int expectedWidth = _factory.CreateLabel(text, false).PreferredWidth + 25;
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IRadioButton radioButton = _factory.CreateRadioButton(text);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(radioButton);
            Assert.AreEqual(text, radioButton.Text);
            Assert.AreEqual(expectedWidth, radioButton.Width);
            Assert.IsFalse(radioButton.Checked);
              
        }

        [Test]
        public void TestCreateNumericUpDownMoney()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            INumericUpDown upDown = _factory.CreateNumericUpDownCurrency();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
            Assert.AreEqual(2, upDown.DecimalPlaces);
            Assert.AreEqual(decimal.MinValue, upDown.Minimum);
            Assert.AreEqual(decimal.MaxValue, upDown.Maximum);
              
        }

        [Test]
        public void TestCreateNumericUpDownInteger()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            INumericUpDown upDown = _factory.CreateNumericUpDownInteger();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
            Assert.AreEqual(0, upDown.DecimalPlaces);
            Assert.AreEqual(Int32.MinValue, upDown.Minimum);
            Assert.AreEqual(Int32.MaxValue, upDown.Maximum);
              
        }

        [Test]
        public void TestCreateNumericUpDown()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            INumericUpDown upDown = _factory.CreateNumericUpDown();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
            Assert.AreEqual(0, upDown.DecimalPlaces);
              
        }

        [Test]
        public void TestCreateDefaultControl()
        {
            //---------------Set up test pack-------------------
            const string typeName = "";
            const string assemblyName = "";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl(typeName, assemblyName);
            //---------------Verify Result -----------------------
            Assert.IsTrue(control is ITextBox);
              
        }

        [Test]
        public void Test_CreateControl()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl();
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
            Assert.AreEqual(100, control.Width);
            Assert.AreEqual(10, control.Height);
        }


        [Test, ExpectedException(typeof(UnknownTypeNameException))]
        public void TestCreateInvalidControlType()
        {
            //---------------Set up test pack-------------------
            const string typeName = "GeewizBox";
            const string assemblyName = "SuperDuper.Components";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            _factory.CreateControl(typeName, assemblyName);
            //---------------Verify Result -----------------------
              
        }

        [Test]
        public void TestCreateControlMapperStrategy()
        {
            //---------------Set up test pack-------------------
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            _factory.CreateControlMapperStrategy();

            //---------------Test Result -----------------------

                     
        }

        [Test]
        public void TestCreateDateRangeComboBox()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IDateRangeComboBox comboBox = GetControlFactory().CreateDateRangeComboBox();
            //---------------Test Result -----------------------
            Assert.IsNotNull(comboBox);
            Assert.AreEqual(12, comboBox.Items.Count);       // Includes blank option in the list
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateDateRangeComboBoxOverload()
        {
            //---------------Set up test pack-------------------
            List<DateRangeOptions> options = new List<DateRangeOptions>();
            options.Add(DateRangeOptions.Today);
            options.Add(DateRangeOptions.Yesterday);

            //---------------Execute Test ----------------------
            IDateRangeComboBox comboBox = GetControlFactory().CreateDateRangeComboBox(options);
            //---------------Test Result -----------------------
            Assert.IsNotNull(comboBox);
            Assert.IsFalse(comboBox.IgnoreTime);
            Assert.IsFalse(comboBox.UseFixedNowDate);
            Assert.AreEqual(0, comboBox.WeekStartOffset);
            Assert.AreEqual(0, comboBox.MonthStartOffset);
            Assert.AreEqual(0, comboBox.YearStartOffset);
            Assert.AreEqual(3, comboBox.Items.Count);          // Includes blank option in the list
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateDataGridViewColumn_SpecifyNameAndAssembly()
        {
            //---------------Set up test pack-------------------
            Type columnType = GetCustomGridColumnType();
            string typeName = columnType.Name;  //"CustomDataGridViewColumn";
            const string assemblyName = "Habanero.Test.UI.Base";
            //---------------Assert Precondition----------------
            Assert.IsTrue(columnType.IsSubclassOf(GetMasterGridColumnType()));
            //---------------Execute Test ----------------------
            IDataGridViewColumn column = GetControlFactory().CreateDataGridViewColumn(typeName, assemblyName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(column);
            Assert.IsInstanceOfType(GetHabaneroMasterGridColumnType(), column);
            AssertGridColumnTypeAfterCast(column, columnType);
        }

        [Test]
        public void TestCreateDataGridViewColumn_DefaultAssembly()
        {
            //---------------Set up test pack-------------------
            const string typeName = "DataGridViewCheckBoxColumn";
            //---------------Assert Precondition----------------
            Assert.IsTrue(typeName.Contains("DataGridViewCheckBoxColumn"));
            //---------------Execute Test ----------------------
            object column = GetControlFactory().CreateDataGridViewColumn(typeName, null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(column);
            Assert.IsInstanceOfType(typeof(IDataGridViewCheckBoxColumn), column);

            string correctAssembly = GetControlFactory().CreateDataGridViewCheckBoxColumn().GetType().AssemblyQualifiedName;
            Assert.AreEqual(correctAssembly, column.GetType().AssemblyQualifiedName);
        }

        [Test]
        public void TestCreateDataGridViewColumn_SpecifyType()
        {
            //---------------Set up test pack-------------------
            Type columnType = GetCustomGridColumnType();
            //---------------Assert Precondition----------------
            Assert.IsTrue(columnType.IsSubclassOf(GetMasterGridColumnType()));
            //---------------Execute Test ----------------------
            IDataGridViewColumn column = GetControlFactory().CreateDataGridViewColumn(columnType);
            //---------------Test Result -----------------------
            Assert.IsNotNull(column);
            Assert.IsInstanceOfType(GetHabaneroMasterGridColumnType(), column);
            AssertGridColumnTypeAfterCast(column, columnType);
        }

        [Test]
        public void TestCreateDataGridViewColumn_NullTypeNameAssumesDefault()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDataGridViewColumn column = GetControlFactory().CreateDataGridViewColumn(null, null);
            //---------------Test Result -----------------------
            Type expectedHabaneroType = GetHabaneroMasterGridColumnType();
            Type expectedMasterType = GetMasterTextBoxGridColumnType();
            Assert.AreEqual(expectedHabaneroType, column.GetType());
            AssertGridColumnTypeAfterCast(column, expectedMasterType);
        }

        [Test]
        public void TestCreateDataGridViewColumn_InvalidTypeNameThrowsException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool errorThrown = false;
            try
            {
                GetControlFactory().CreateDataGridViewColumn("InvalidColumnType", null);
            }
            catch (UnknownTypeNameException)
            {
                errorThrown = true;
            }
            //---------------Test Result -----------------------
            Assert.IsTrue(errorThrown, "Not specifying a type name should throw an exception (defaults must be set further up the chain)");
        }

        [Test]
        public void TestCreateDataGridViewColumn_ExceptionIfDoesNotImplementIDataGridViewColumn()
        {
            //---------------Set up test pack-------------------
            Type wrongType = typeof (MyBO);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool errorThrown = false;
            try
            {
                GetControlFactory().CreateDataGridViewColumn(wrongType);
            }
            catch (UnknownTypeNameException)
            {
                errorThrown = true;
            }
            //---------------Test Result -----------------------
            Assert.IsTrue(errorThrown, "Type must inherit from IDataGridViewColumn");
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_NumericUpDown()
        {
            //---------------Set up test pack-------------------
            IDataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn = GetControlFactory().CreateDataGridViewNumericUpDownColumn();
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IDataGridViewColumn dataGridViewColumn = GetControlFactory().
                CreateDataGridViewColumn("DataGridViewNumericUpDownColumn", null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataGridViewColumn);
            Assert.IsInstanceOfType(typeof(IDataGridViewNumericUpDownColumn), dataGridViewColumn);
            Assert.AreSame(dataGridViewNumericUpDownColumn.GetType(), dataGridViewColumn.GetType());
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_DateTimePicker()
        {
            //---------------Set up test pack-------------------
            IDataGridViewDateTimeColumn dataGridViewNumericUpDownColumn = GetControlFactory().CreateDataGridViewDateTimeColumn();
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IDataGridViewColumn dataGridViewColumn = GetControlFactory().
                CreateDataGridViewColumn("DataGridViewDateTimeColumn", null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataGridViewColumn);
            Assert.IsInstanceOfType(typeof(IDataGridViewDateTimeColumn), dataGridViewColumn);
            Assert.AreSame(dataGridViewNumericUpDownColumn.GetType(), dataGridViewColumn.GetType());
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_CheckBox()
        {
            //---------------Set up test pack-------------------
            IDataGridViewCheckBoxColumn dataGridViewNumericUpDownColumn = GetControlFactory().CreateDataGridViewCheckBoxColumn();
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IDataGridViewColumn dataGridViewColumn = GetControlFactory().
                CreateDataGridViewColumn("DataGridViewCheckBoxColumn", null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataGridViewColumn);
            Assert.IsInstanceOfType(typeof(IDataGridViewCheckBoxColumn), dataGridViewColumn);
            Assert.AreSame(dataGridViewNumericUpDownColumn.GetType(), dataGridViewColumn.GetType());
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_ComboBox()
        {
            //---------------Set up test pack-------------------
            IDataGridViewComboBoxColumn dataGridViewNumericUpDownColumn = GetControlFactory().CreateDataGridViewComboBoxColumn();
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IDataGridViewColumn dataGridViewColumn = GetControlFactory().
                CreateDataGridViewColumn("DataGridViewComboBoxColumn", null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataGridViewColumn);
            Assert.IsInstanceOfType(typeof(IDataGridViewComboBoxColumn), dataGridViewColumn);
            Assert.AreSame(dataGridViewNumericUpDownColumn.GetType(), dataGridViewColumn.GetType());
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_Image()
        {
            //Not implemented in win
        }

        [Test]
        public void Test_Create_GroupBoxGroupControl()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IGroupBoxGroupControl groupBoxGroupControl = GetControlFactory().CreateGroupBoxGroupControl();
            //---------------Test Result -----------------------
            Assert.IsNotNull(groupBoxGroupControl);
        }

        [Test]
        public virtual void Test_Create_ComboBox()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();  
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IComboBox control = factory.CreateComboBox();
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
            Assert.AreEqual(GetStandardTextBoxHeight(), control.Height);
        }
        [Test]
        public virtual void Test_Create_ComboBoxSelector()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOComboBoxSelector control = factory.CreateComboBoxSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
            Assert.AreEqual(GetStandardTextBoxHeight(), control.Height);
        }
        [Test]
        public void Test_Create_CollapsiblePanelSelector()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOCollapsiblePanelSelector control = factory.CreateCollapsiblePanelSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }

        [Test]
        public void Test_Create_BOEditorControl_Generic_WithUIDef()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOPanelEditorControl control = factory.CreateBOEditorControl<ContactPersonTestBO>("default");
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }

        [Test]
        public void Test_Create_BOEditorControl_Generic()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOPanelEditorControl control = factory.CreateBOEditorControl<ContactPersonTestBO>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }
        [Test]
        public void Test_Create_BOEditorControl_NonGeneric_WithUIDef()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOPanelEditorControl control = factory.CreateBOEditorControl(classDef, "default");
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }
        [Test]
        public void Test_Create_BOEditorControl_NonGeneric()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOPanelEditorControl control = factory.CreateBOEditorControl(classDef);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }
        
        [Test]
        public void Test_Create_MainTitleIconControl()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMainTitleIconControl control = factory.CreateMainTitleIconControl();
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }
    }
}
