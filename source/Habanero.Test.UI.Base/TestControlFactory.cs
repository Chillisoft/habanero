using System;
using System.Drawing;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestControlFactory
    {
        protected abstract IControlFactory GetControlFactory();

        //[TestFixture]
        //public class TestControlFactoryWin : TestControlFactory
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new Habanero.UI.Win.ControlFactoryWin();
        //    }

        //}

        [TestFixture]
        public class TestControlFactoryGiz : TestControlFactory
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }
            [Test]
            public void TestCreateCheckBoxGiz()
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
                IControlChilli controlChilli = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.ComboBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlChilli);
                Assert.AreEqual(typeof(Habanero.UI.WebGUI.ComboBoxGiz), controlChilli.GetType());
                //---------------Tear Down -------------------------   
            }
            [Test]
            public void TestCreateControl_ViaType_CreateCheckBox()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlChilli controlChilli = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.CheckBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlChilli);
                Assert.AreEqual(typeof(Habanero.UI.WebGUI.CheckBoxGiz), controlChilli.GetType());
                //---------------Tear Down -------------------------   
            }
            [Test]
            public void TestCreateControl_ViaType_CreateTextBox()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlChilli controlChilli = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.TextBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlChilli);
                Assert.AreEqual(typeof(Habanero.UI.WebGUI.TextBoxGiz), controlChilli.GetType());
                //---------------Tear Down -------------------------   
            }
            [Test]
            public void TestCreateControl_ViaType_CreateListBox()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlChilli controlChilli = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.ListBox));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlChilli);
                Assert.AreEqual(typeof(Habanero.UI.WebGUI.ListBoxGiz), controlChilli.GetType());
                //---------------Tear Down -------------------------   
            }
            [Test]
            public void TestCreateControl_ViaType_CreateDateTimePicker()
            {
                //---------------Set up test pack-------------------
                //---------------Verify test pack-------------------
                //---------------Execute Test ----------------------
                IControlChilli controlChilli = _factory.CreateControl(typeof(Gizmox.WebGUI.Forms.DateTimePicker));
                //---------------Verify Result -----------------------
                Assert.IsNotNull(controlChilli);
                Assert.AreEqual(typeof(Habanero.UI.WebGUI.DateTimePickerGiz), controlChilli.GetType());
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
            Assert.AreEqual(lbl.PreferredWidth + 14, lbl.Width);
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
        public void TestCreateNumericUpDownMoney()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            INumericUpDown upDown = _factory.CreateNumericUpDownCurrency();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
            Assert.AreEqual(2, upDown.DecimalPlaces);
            Assert.AreEqual(Int32.MinValue, upDown.Minimum);
            Assert.AreEqual(Int32.MaxValue, upDown.Maximum);
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
        public void TestCreateDefaultControl()
        {
            //---------------Set up test pack-------------------
            String typeName = "";
            String assemblyName = "";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlChilli control = _factory.CreateControl(typeName, assemblyName);
            //---------------Verify Result -----------------------
            Assert.IsTrue(control is ITextBox);
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
            IControlChilli control = _factory.CreateControl(typeName, assemblyName);
            //---------------Verify Result -----------------------
            Assert.IsTrue(control is Gizmox.WebGUI.Forms.TextBox);
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
            IControlChilli control = _factory.CreateControl(typeName, assemblyName);
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
    }


}