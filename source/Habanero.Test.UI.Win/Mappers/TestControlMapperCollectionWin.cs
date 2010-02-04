using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    [TestFixture]
    public class TestControlMapperCollectionWin : TestControlMapperCollection
    {
        protected override IControlFactory GetControlFactory()
        {
            Habanero.UI.Win.ControlFactoryWin factory = new Habanero.UI.Win.ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestChangeControlValues_ChangesBusinessObjectValues()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.TestProp = START_VALUE_1;
            myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

            PanelBuilder factory = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            //---------------Execute Test ----------------------
            ChangeValuesInControls(panelInfo);
            panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.ApplyChangesToBusinessObject();
            panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------

            Assert.AreEqual(CHANGED_VALUE_1, myBO.GetPropertyValue(TEST_PROP_1));
            Assert.AreEqual(CHANGED_VALUE_2, myBO.GetPropertyValue(TEST_PROP_2));

        }
    }
}