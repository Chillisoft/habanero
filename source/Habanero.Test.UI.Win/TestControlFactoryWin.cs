using System;
using Habanero.Base.Exceptions;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win
{
    /// <summary>
    /// Created for testing purposes
    /// </summary>
    public class CustomDataGridViewColumnWin : System.Windows.Forms.DataGridViewTextBoxColumn
    {
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
                _factory.CreateControl(typeof(System.String));
                //---------------Verify Result -----------------------
            }
            catch (UnknownTypeNameException ex)
            {
                StringAssert.Contains("The control type name System.String does not inherit from System.Windows.Forms.Control", ex.Message);
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

        protected override IControlFactory GetControlFactory()
        {
            IControlFactory factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override int GetBoldTextExtraWidth()
        {
            return 10;
        }

        protected Type GetCustomGridColumnType()
        {
            return typeof(CustomDataGridViewColumnWin);
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
            Habanero.UI.Win.DataGridViewColumnWin columnWin = (Habanero.UI.Win.DataGridViewColumnWin)createdColumn;
            System.Windows.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
            Assert.AreEqual(expectedColumnType, column.GetType());
        }

        [Test]
        public void TestCreateDataGridViewColumn_SpecifyNameAndAssembly()
        {
            //---------------Set up test pack-------------------
            Type columnType = GetCustomGridColumnType();
            string typeName = columnType.Name;  //"CustomDataGridViewColumn";
            const string assemblyName = "Habanero.Test.UI.Win";
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
    }
}