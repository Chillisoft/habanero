using Habanero.Base;
using Habanero.Test.UI.Base.FilterController;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestFilterControlVWG : TestFilterControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }


        [Test]
        public void Test_DefaultLayoutManager()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();

            //---------------Execute Test ----------------------
            //            IControlHabanero control = factory.CreatePanel();
            IFilterControl ctl = factory.CreateFilterControl();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(FlowLayoutManager), ctl.LayoutManager);
        }

        [Test]
        public void Test_SetFilterHeader()
        {
            //---------------Set up test pack-------------------
            IFilterControl ctl = GetControlFactory().CreateFilterControl();
            //---------------Assert Preconditions---------------
            Assert.AreEqual("Filter the Grid", ctl.HeaderText);
            //---------------Execute Test ----------------------
            ctl.HeaderText = "Filter Assets";
            //---------------Test Result -----------------------
            Assert.AreEqual("Filter Assets", ctl.HeaderText);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_SetFilterModeFilterSetsText()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            ctl.FilterMode = FilterModes.Search;
            //---------------Assert Preconditions --------------
            Assert.AreEqual("Search", ctl.FilterButton.Text);
            //---------------Execute Test ----------------------
            ctl.FilterMode = FilterModes.Filter;
            //---------------Test Result -----------------------
            Assert.AreEqual("Filter", ctl.FilterButton.Text);
        }

        [Test]
        public void Test_SetFilterModeSearchSetsText()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            //---------------Assert Preconditions --------------
            Assert.AreEqual("Filter", ctl.FilterButton.Text);
            //---------------Execute Test ----------------------
            ctl.FilterMode = FilterModes.Search;
            //---------------Test Result -----------------------
            Assert.AreEqual("Search", ctl.FilterButton.Text);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_SetFilterGroupBoxSetsText()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            string groupBoxHeaderText = TestUtil.GetRandomString();
            //---------------Assert Preconditions --------------
            Assert.AreEqual("Filter the Grid", ctl.FilterGroupBox.Text);
            //---------------Execute Test ----------------------
            ctl.HeaderText = groupBoxHeaderText;
            //---------------Test Result -----------------------
            Assert.AreEqual(ctl.HeaderText, ctl.FilterGroupBox.Text);
            Assert.AreEqual(groupBoxHeaderText, ctl.FilterGroupBox.Text);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestClearButtonAccessor()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Test Result -----------------------
            Assert.IsNotNull(filterControl.ClearButton);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFilterButtonAccessor()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Test Result -----------------------
            Assert.IsNotNull(filterControl.FilterButton);
            //---------------Tear Down -------------------------
        }
    }
}