using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;


namespace Habanero.Test.UI.Base
{

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
    /// This test class tests the base inherited methods of the ComboBoxSelector class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_ComboBoxSelector : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateComboBoxSelector();
        }
    }

    public class TestComboBoxSelectorVWG : TestComboBoxSelectorWin
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
        protected override IBOSelector CreateSelector()
        {
            return GetControlFactory().CreateComboBoxSelector();
        }
        [Test]
        public virtual void Test_Constructor_nullControlFactory_RaisesError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new ComboBoxSelectorVWG(null);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }
    }
    /// <summary>
    /// This test class tests the ComboBoxSelector class.
    /// </summary>
    [TestFixture]
    public class TestComboBoxSelectorWin : TestBOSelectorWin
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }


        protected override void SetSelectedIndex(IBOSelector selector, int index)
        {
            ((IBOComboBoxSelector)selector).ComboBox.SelectedIndex = index;
        }

        protected override int SelectedIndex(IBOSelector selector)
        {
            return ((IBOComboBoxSelector)selector).ComboBox.SelectedIndex;
        }

        protected override IBOSelector CreateSelector()
        {
            return GetControlFactory().CreateComboBoxSelector();
        }

        [Test]
        public void Test_Constructor_ComboBoxSet()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOComboBoxSelector selector = (IBOComboBoxSelector) CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(selector.ComboBox);
            Assert.IsInstanceOfType(typeof(IComboBox), selector.ComboBox);
        }

        [Test]
        public void Test_SelectedBusinessObject_BlankItemSelected_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector selector = (IBOComboBoxSelector)CreateSelector();
            Car car = new Car();
            BusinessObjectCollection<Car> collection = new BusinessObjectCollection<Car> { car };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, 0);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, selector.NoOfItems, "The blank item and one other");
            Assert.AreEqual(0, SelectedIndex(selector));
            Assert.AreEqual("", selector.SelectedValue);
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = selector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNull(selectedBusinessObject);
        }

        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<MyBO> col;
            IBOSelector boSelector = GetSelectorWith_4_Rows(out col);
            const string propName = "TestProp";
            const int rowIndex = 1;
            MyBO bo = col[rowIndex];
            boSelector.BusinessObjectCollection = col;
            string origStringValue = ((IBOComboBoxSelector)boSelector).GetItemText(bo);
            //---------------Verify precondition----------------
            Assert.AreEqual(bo.ToString(), origStringValue);
            //---------------Execute Test ----------------------
            const string newPropValue = "NewValue";
            bo.SetPropertyValue(propName, newPropValue);
            bo.Save();
            //---------------Test Result -----------------------
            string newStringValue = ((IBOComboBoxSelector)boSelector).GetItemText(bo);
            Assert.AreNotEqual(origStringValue, newStringValue);
            Assert.AreEqual(newPropValue + " - " + bo.MyBoID, newStringValue);
        }

    }
}
