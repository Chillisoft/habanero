using System;
using Habanero.Base.Exceptions;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG
{
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
                _factory.CreateControl(typeof(System.String));
                //---------------Verify Result -----------------------
            }
            catch (UnknownTypeNameException ex)
            {
                StringAssert.Contains("The control type name System.String does not inherit from Gizmox.WebGUI.Forms.Control", ex.Message);
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
}