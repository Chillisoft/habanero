using System;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the Button class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_Button : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateButton();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the CheckBox class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_CheckBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCheckBox();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the CollapsiblePanel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_CollapsiblePanel : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanel();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the CollapsiblePanel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_CollapsiblePanelGroupControl : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanelGroupControl();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the ComboBox class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_ComboBox : TestBaseMethods.TestBaseMethodsWin
    {
        [STAThread]
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateComboBox();
        }
    }


    /// <summary>
    /// This test class tests the base inherited methods of the ComboBoxSelector class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_ComboBoxSelector : TestBaseMethods.TestBaseMethodsWin
    {
        [STAThread]
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateComboBoxSelector();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Control class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_Control : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateControl();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the DateTimePicker class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_DateTimePicker : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateDateTimePicker();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Form class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_Form : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateForm();
        }

        private IFormHabanero CreateForm()
        {
            IControlHabanero control = CreateControl();
            return (IFormHabanero)control;
        }

        [Test]
        public virtual void Test_FormBorderStyle()
        {
            //---------------Set up test pack-------------------
            IFormHabanero formHabanero = CreateForm();
            FormBorderStyle formBorderStyle = TestUtil.GetRandomEnum<FormBorderStyle>();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            formHabanero.FormBorderStyle = formBorderStyle;
            //---------------Test Result -----------------------
            Assert.AreEqual(formBorderStyle, formHabanero.FormBorderStyle);
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the GroupBox class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_GroupBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateGroupBox();
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the GroupBoxGroupControl class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_GroupBoxGroupControl : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateGroupBoxGroupControl();
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the Label class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_Label : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateLabel();
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the ListBox class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_ListBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateListBox();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the ListBoxSelector class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_ListBoxSelector : TestBaseMethods.TestBaseMethodsWin
    {
        [STAThread]
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateListBoxSelector();
        }
    }


    /// <summary>
    /// This test class tests the base inherited methods of the MainTitleIconControl class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_MainTitleIconControl : TestBaseMethods.TestBaseMethodsWin
    {
        private IControlFactory _factory;

        protected override IControlFactory GetControlFactory()
        {
            if (_factory == null)
            {
                _factory = CreateNewControlFactory();
                GlobalUIRegistry.ControlFactory = _factory;
            }
            return _factory;
        }

        protected virtual IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IControlHabanero CreateControl()
        {
            return new MainTitleIconControlWin(GetControlFactory());
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the NumericUpDown class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_NumericUpDown : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateNumericUpDown();
        }



        [Test]
        public void Test_setTextAlignment_Left()
        {
            //---------------Set up test pack-------------------
            INumericUpDown textBox = GetControlFactory().CreateNumericUpDown();
            //---------------Execute Test ----------------------
            textBox.TextAlign = HorizontalAlignment.Left;
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Left, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_setTextAlignment_Center()
        {
            //---------------Set up test pack-------------------
            INumericUpDown textBox = GetControlFactory().CreateNumericUpDown();
            //---------------Execute Test ----------------------
            textBox.TextAlign = HorizontalAlignment.Center;
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Center, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_setTextAlignment_Right()
        {
            //---------------Set up test pack-------------------
            INumericUpDown textBox = GetControlFactory().CreateNumericUpDown();
            //---------------Execute Test ----------------------
            textBox.TextAlign = HorizontalAlignment.Right;
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Right, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Panel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_Panel : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreatePanel();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Panel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_PictureBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreatePictureBox();
        }
    }
      

    /// <summary>
    /// This test class tests the base inherited methods of the ProgressBar class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_ProgressBar : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateProgressBar();
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the RadioButton class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_RadioButton : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateRadioButton("");
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the SplitContainer class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_SplitContainer : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateSplitContainer();
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the Splitter class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_Splitter : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateSplitter();
        }

        [Test]
        public override void TestConversion_DockStyle_None()
        {
            //Splitter does not support setting dock styles at design time.
        }

        [Test]
        public override void TestConversion_DockStyle_Fill()
        {
            //Splitter does not support setting dock styles at design time.
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the TabControl class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_TabControl : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTabControl();
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the TabPage class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_TabPage : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTabPage("");
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the TextBox class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_TextBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTextBox();
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the TreeView class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_TreeView : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTreeView("");
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the UserControl class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_UserControl : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return new UserControlWin();
        }
    }
  
}