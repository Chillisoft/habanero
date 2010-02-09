using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Mappers
{
    [TestFixture]
    public class TestControlMapperCollectionVWG : TestControlMapperCollection
    {
        protected override IControlFactory GetControlFactory()
        {
            Habanero.UI.VWG.ControlFactoryVWG factory = new Habanero.UI.VWG.ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestChangeControlValues_DoesNotChangeBusinessObjectValues()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.TestProp = START_VALUE_1;
            myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

            Habanero.UI.Base.PanelBuilder factory = new Habanero.UI.Base.PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            //---------------Execute Test ----------------------
            ChangeValuesInControls(panelInfo);
            //---------------Test Result -----------------------

            Assert.AreEqual(START_VALUE_1, myBO.GetPropertyValue(TEST_PROP_1));
            Assert.AreEqual(START_VALUE_2, myBO.GetPropertyValue(TEST_PROP_2));

        }

    }
}