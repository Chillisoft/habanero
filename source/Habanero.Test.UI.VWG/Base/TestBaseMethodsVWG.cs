using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Base
{
    public abstract class TestBaseMethodsVWG : TestBaseMethods
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override string GetUnderlyingDockStyleToString(IControlHabanero controlHabanero)
        {
            Gizmox.WebGUI.Forms.Control control = (Gizmox.WebGUI.Forms.Control)controlHabanero;
            return control.Dock.ToString();
        }
    }
  
    /// <summary>
    /// This test class tests the base inherited methods of the Button class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_Button : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_CheckBox : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_CollapsiblePanel : TestBaseMethodsVWG
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
    public partial class TestBaseMethodsVWG_CollapsiblePanelGroupControl : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_ComboBox : TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateComboBox();
        }
    }


    /// <summary>
    /// This test class tests the base inherited methods of the ComboBoxSelector class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_ComboBoxSelector : TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateComboBoxSelector();
        }
    }


    /// <summary>
    /// This test class tests the base inherited methods of the Control class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_Control : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_DateTimePicker : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_Form : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_GroupBox : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_GroupBoxGroupControl : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_Label : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_ListBox : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_ListBoxSelector : TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateListBoxSelector();
        }
    }
    /// <summary>
    /// This test class tests the base inherited methods of the MainTitleIconControl class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_MainTitleIconControl : TestBaseMethodsVWG
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

        protected virtual ControlFactoryVWG CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IControlHabanero CreateControl()
        {
            return new MainTitleIconControlVWG(GetControlFactory());
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the NumericUpDown class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_NumericUpDown : TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateNumericUpDown();
        }

        [Test]
        public void Test_defaultTextAlignment()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            INumericUpDown numericUpDown = GetControlFactory().CreateNumericUpDown();
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Left, numericUpDown.TextAlign);
            //---------------Tear Down -------------------------
        }




        [Test]
        public void Test_setTextAlignment_Center()
        {
            //, Ignore("VWG does not support setting the TextAlign Property. Default value is Left")
        }

        [Test]
        public void Test_setTextAlignment_Right()
        {
            //Ignore("VWG does not support setting the TextAlign Property. Default value is Left")
            //            //---------------Set up test pack-------------------
            //            INumericUpDown numericUpDown = GetControlFactory().CreateNumericUpDown();
            //            //---------------Execute Test ----------------------
            //            numericUpDown.TextAlign = HorizontalAlignment.Right;
            //            //---------------Test Result -----------------------
            //            Assert.AreEqual(HorizontalAlignment.Right, numericUpDown.TextAlign);
            //            //---------------Tear Down -------------------------
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Panel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_Panel : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_PictureBox : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_ProgressBar : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_RadioButton : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_SplitContainer : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_Splitter : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_TabControl : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_TabPage : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_TextBox : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_TreeView : TestBaseMethodsVWG
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
    public class TestBaseMethodsVWG_UserControl : TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return new UserControlVWG();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the DataGridView class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_DataGridView : TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateDataGridView();
        }
    }
}