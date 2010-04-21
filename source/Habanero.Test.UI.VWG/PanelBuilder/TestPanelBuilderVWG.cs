using Habanero.BO.ClassDefinition;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.PanelBuilder
{
    [TestFixture]
    public class TestPanelBuilderVWG : TestPanelBuilder
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override Sample.SampleUserInterfaceMapper GetSampleUserInterfaceMapper() { return new Sample.SampleUserInterfaceMapperVWG(); }

        [Test]
        public void Test_BuildPanelForTab_Parameter_SetAlignment_NumericUpDown()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldsWithNumericUpDown();
            Habanero.UI.Base.PanelBuilder panelBuilder = new Habanero.UI.Base.PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual("right", ((UIFormField)singleFieldTab[0][0]).Alignment);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[1]);
            INumericUpDown control = (INumericUpDown)panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Right, control.TextAlign);
        }
    }
}