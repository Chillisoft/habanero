using Habanero.BO.ClassDefinition;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.PanelBuilder
{
    [TestFixture]
    public class TestPanelBuilderWin : TestPanelBuilder
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override Sample.SampleUserInterfaceMapper GetSampleUserInterfaceMapper() { return new Sample.SampleUserInterfaceMapperWin(); }


        [Test]
        public void Test_BuildPanelForTab_Parameter_SetNumericUpDownAlignment()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldsWithAlignment_NumericUpDown();
            Habanero.UI.Base.PanelBuilder panelBuilder = new Habanero.UI.Base.PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual("left", ((UIFormField)singleFieldTab[0][0]).Alignment);
            Assert.AreEqual("right", ((UIFormField)singleFieldTab[0][1]).Alignment);
            Assert.AreEqual("center", ((UIFormField)singleFieldTab[0][2]).Alignment);
            Assert.AreEqual("centre", ((UIFormField)singleFieldTab[0][3]).Alignment);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[1]);
            INumericUpDown control1 = (INumericUpDown)panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Left, control1.TextAlign);

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[Habanero.UI.Base.PanelBuilder.CONTROLS_PER_COLUMN + 1]);
            INumericUpDown control2 = (INumericUpDown)panel.Controls[Habanero.UI.Base.PanelBuilder.CONTROLS_PER_COLUMN + 1];
            Assert.AreEqual(HorizontalAlignment.Right, control2.TextAlign);

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[Habanero.UI.Base.PanelBuilder.CONTROLS_PER_COLUMN * 2 + 1]);
            INumericUpDown control3 = (INumericUpDown)panel.Controls[Habanero.UI.Base.PanelBuilder.CONTROLS_PER_COLUMN * 2 + 1];
            Assert.AreEqual(HorizontalAlignment.Center, control3.TextAlign);

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[Habanero.UI.Base.PanelBuilder.CONTROLS_PER_COLUMN * 3 + 1]);
            INumericUpDown control4 = (INumericUpDown)panel.Controls[Habanero.UI.Base.PanelBuilder.CONTROLS_PER_COLUMN * 3 + 1];
            Assert.AreEqual(HorizontalAlignment.Center, control4.TextAlign);
        }


        //TODO: add tests that label and error provider get tabstop set to false


        //        [Test]
        //        public void Test_Set_IndividualControlCreator_Null_ShouldRaiseError()
        //        {
        //            //---------------Set up test pack-------------------
        //            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
        //            UIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
        //            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
        //            //---------------Assert Precondition----------------
        //            Assert.AreEqual(2, form.Count);
        //            //---------------Execute Test ----------------------
        //            try
        //            {
        //                panelBuilder.SetControlCreators(GetControlFactory().CreateTabControl, null);
        //                Assert.Fail("expected ArgumentNullException");
        //            }
        //                //---------------Test Result -----------------------
        //            catch (ArgumentNullException ex)
        //            {
        //                StringAssert.Contains("Value cannot be null", ex.Message);
        //                StringAssert.Contains("individualControlCreator", ex.ParamName);
        //            }
        //        }

        //[Test, Ignore("This doesn't work in code for some reason")]
        //public void Test_BuildPanelForTab_SetToolTip()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneWithToolTipText();
        //    UIFormTab twoFieldTabOneHasToolTip = classDef.UIDefCol["default"].UIForm[0];
        //    PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
        //    //-------------Assert Preconditions -------------

        //    //---------------Execute Test ----------------------
        //    IPanel panel = panelBuilder.BuildPanel(twoFieldTabOneHasToolTip).Panel;
        //    //---------------Test Result -----------------------
        //    IControlCollection controls = panel.Controls;
        //    ILabel labelWithToolTip =
        //        (ILabel)controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
        //    IControlHabanero controlHabanero =
        //        controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
        //    IToolTip toolTip = GetControlFactory().CreateToolTip();

        //    Assert.AreEqual("Test tooltip text", toolTip.GetToolTip(controlHabanero));
        //}
    }
}