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

using System;
using System.Collections.Generic;
using System.Drawing;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestControlFactory
    {
        protected abstract IControlFactory GetControlFactory();
        protected abstract int GetBoldTextExtraWidth();
        protected abstract Type GetCustomGridColumnType();
        protected abstract Type GetMasterGridColumnType();
        protected abstract Type GetMasterTextBoxGridColumnType();
        protected abstract Type GetHabaneroMasterGridColumnType();
        protected abstract string GetUINameSpace();

        protected abstract void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType);

        [TestFixture]
        public class TestControlFactoryWin : TestControlFactory
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }

            protected override int GetBoldTextExtraWidth()
            {
                return 10;
            }

            protected override Type GetCustomGridColumnType()
            {
                return typeof (CustomDataGridViewColumnWin);
            }

            protected override Type GetMasterGridColumnType()
            {
                return typeof(System.Windows.Forms.DataGridViewColumn);
            }

            protected override Type GetMasterTextBoxGridColumnType()
            {
                return typeof(System.Windows.Forms.DataGridViewTextBoxColumn);
            }

            protected override Type GetHabaneroMasterGridColumnType()
            {
                return typeof(Habanero.UI.Win.DataGridViewColumnWin);
            }

            protected override string GetUINameSpace()
            {
                return "System.Windows.Forms";
            }

            protected override void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType)
            {
                Habanero.UI.Win.DataGridViewColumnWin columnWin = (Habanero.UI.Win.DataGridViewColumnWin) createdColumn;
                System.Windows.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
                Assert.AreEqual(expectedColumnType, column.GetType());
            }

            [Test]
            public void TestCreateCheckBoxWin()
            {
                //---------------Set up test pack-------------------
                //---------------Execute Test ----------------------

                ICheckBox cbx = GetControlFactory().CreateCheckBox();
                //---------------Test Result -----------------------
                Assert.IsFalse(cbx.Checked);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestCreateCheckBoxWin_WithDefault()
            {
                //---------------Set up test pack-------------------
                //---------------Execute Test ----------------------

                ICheckBox cbx = GetControlFactory().CreateCheckBox(true);
                //---------------Test Result -----------------------
                Assert.IsTrue(cbx.Checked);
                //---------------Tear Down -------------------------          
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                String typeName = "TextBox";
                String assemblyName = "System.Windows.Forms";
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero control = _factory.CreateControl(typeName, assemblyName);
                //---------------Verify Result -----------------------
                Assert.IsTrue(control is System.Windows.Forms.TextBox);
                //---------------Tear Down -------------------------   
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
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.VWG.ControlFactoryVWG();
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
                //---------------Tear Down -------------------------          
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
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
                //---------------Tear Down -------------------------   
            }
            [Test]
            public void TestCreateSpecifiedControlType()
            {
                //---------------Set up test pack-------------------
                String typeName = "TextBox";
                String assemblyName = "Gizmox.WebGUI.Forms";
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlHabanero control = _factory.CreateControl(typeName, assemblyName);
                //---------------Verify Result -----------------------
                Assert.IsTrue(control is Gizmox.WebGUI.Forms.TextBox);
                //---------------Tear Down -------------------------   
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

        private IControlFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _factory = GetControlFactory();
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
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestCreateLabel_Text()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            string labelText = "test label";
            //---------------Execute Test ----------------------

            ILabel lbl = _factory.CreateLabel(labelText);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(lbl);
            Assert.IsFalse(lbl.TabStop);
            Assert.AreEqual(labelText, lbl.Text);
            Assert.AreNotEqual(labelText, lbl.Name);
            Assert.AreEqual(lbl.PreferredWidth, lbl.Width);
            //TODO_Port_DoTest lbl.FlatStyle = FlatStyle.Standard;
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestCreateLabel_BoldText()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            string labelText = "test label";
            //---------------Execute Test ----------------------

            ILabel lbl = _factory.CreateLabel(labelText, true);
            //---------------Verify Result -----------------------
            //Assert.AreEqual(lbl.PreferredWidth + 10, lbl.Width);
            Assert.AreEqual(lbl.PreferredWidth + GetBoldTextExtraWidth(), lbl.Width);
            Font expectedFont = new Font(lbl.Font, FontStyle.Bold);
            Assert.AreEqual(expectedFont, lbl.Font);
            //---------------Tear Down -------------------------   
        }


        [Test]
        public void TestCreateButton()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            string buttonText = "test label";
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
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestCreateControlWithNullAssemblyName()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl("NumericUpDown", null);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(INumericUpDown), control);
            //---------------Tear down -------------------------

        }

        [Test]
        public void TestCreateButton_WithEventHandler()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            string buttonText = "test label";
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
            //---------------Tear Down -------------------------   
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
            //---------------Tear Down -------------------------   
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
            //---------------Tear Down -------------------------   
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
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestCreateTreeView()
        {
            //---------------Set up test pack-------------------
            string treeViewname = "TVNAme";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ITreeView treeView = _factory.CreateTreeView(treeViewname);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(treeView);
            Assert.IsTrue(treeView.TabStop);
            Assert.AreEqual(treeViewname, treeView.Name);
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestCreateTreeNode()
        {
            //---------------Set up test pack-------------------
            string treeNodeName = "TVNodeName";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ITreeNode treeNode = _factory.CreateTreeNode(treeNodeName);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(treeNode);
            Assert.AreEqual(treeNodeName, treeNode.Text);
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestCreatePanel()
        {
            //---------------Set up test pack-------------------
            string pnlName = "PanelName";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IPanel panelName = _factory.CreatePanel(pnlName,GetControlFactory());
            //---------------Verify Result -----------------------
            Assert.IsNotNull(panelName);
            //Assert.IsTrue(treeView.TabStop);

            Assert.AreEqual(pnlName, panelName.Name);
            //---------------Tear Down -------------------------   
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
            //---------------Tear Down -------------------------   
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
            //---------------Tear Down -------------------------   
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
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestCreateRadioButton()
        {
            //---------------Set up test pack-------------------
            string text = TestUtil.CreateRandomString();
            int expectedWidth = _factory.CreateLabel(text, false).PreferredWidth + 25;
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IRadioButton radioButton = _factory.CreateRadioButton(text);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(radioButton);
            Assert.AreEqual(text, radioButton.Text);
            Assert.AreEqual(expectedWidth, radioButton.Width);
            Assert.IsFalse(radioButton.Checked);
            //---------------Tear Down -------------------------   
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
            //---------------Tear Down -------------------------   
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
            //---------------Tear Down -------------------------   
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
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestCreateDefaultControl()
        {
            //---------------Set up test pack-------------------
            String typeName = "";
            String assemblyName = "";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl(typeName, assemblyName);
            //---------------Verify Result -----------------------
            Assert.IsTrue(control is ITextBox);
            //---------------Tear Down -------------------------   
        }



        [Test, ExpectedException(typeof(UnknownTypeNameException))]
        public void TestCreateInvalidControlType()
        {
            //---------------Set up test pack-------------------
            String typeName = "GeewizBox";
            String assemblyName = "SuperDuper.Components";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl(typeName, assemblyName);
            //---------------Verify Result -----------------------
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestCreateControlMapperStrategy()
        {
            //---------------Set up test pack-------------------
            IControlMapperStrategy strategy = _factory.CreateControlMapperStrategy();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
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
            string assemblyName = "Habanero.Test.UI.Base";
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
            string typeName = "DataGridViewCheckBoxColumn";
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
                IDataGridViewColumn column = GetControlFactory().CreateDataGridViewColumn("InvalidColumnType", null);
            }
            catch (UnknownTypeNameException ex)
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
                object column = GetControlFactory().CreateDataGridViewColumn(wrongType);
            }
            catch (UnknownTypeNameException ex)
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

    }


}