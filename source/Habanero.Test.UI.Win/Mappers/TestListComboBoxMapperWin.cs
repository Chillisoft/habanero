using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    [TestFixture]
    public class TestListComboBoxMapperWin : TestListComboBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestChangeBusinessObjectUpdatesComboBox_WithoutCallingUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());
            mapper.SetList("One|Two|Three|Four");
            Sample s = new Sample();
            s.SampleText = "Three";
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            s.SampleText = "Four";

            //---------------Test Result -----------------------
            Assert.AreEqual("Four", cbx.SelectedItem, "Value is not set.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestChangeBusinessObjectUpdatesComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());
            mapper.SetList("One|Two|Three|Four");
            Sample s = new Sample();
            s.SampleText = "Three";
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            s.SampleText = "Four";
            mapper.UpdateControlValueFromBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual("Four", cbx.SelectedItem, "Value is not set.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetComboBoxUpdatesBO()
        {
            //---------------Set up test pack-------------------
            IComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());
            mapper.SetList("One|Two|Three|Four");
            Sample s = new Sample();
            s.SampleText = "Three";
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cbx.SelectedIndex = 0;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(cbx.SelectedItem, s.SampleText,
                            "BO property value isn't changed when control value is changed.");

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetComboBoxUpdatesBO_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            IComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());
            mapper.SetList("One|Two|Three|Four");
            Sample s = new Sample();
            s.SampleText = "Three";
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cbx.SelectedIndex = 0;
            //---------------Test Result -----------------------
            Assert.AreEqual(cbx.SelectedItem, s.SampleText,
                            "BO property value isn't changed when control value is changed.");

            //---------------Tear Down -------------------------
        }

    }
}