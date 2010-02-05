using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;


using NUnit.Framework;


namespace Habanero.Test.UI.Base
{
    public abstract class TestComboBoxSelector : TestBOColSelector
    {
        protected override void SetSelectedIndex(IBOColSelectorControl colSelector, int index)
        {
            ((IBOComboBoxSelector)colSelector).ComboBox.SelectedIndex = index;
        }

        protected override int SelectedIndex(IBOColSelectorControl colSelector)
        {
            return ((IBOComboBoxSelector)colSelector).ComboBox.SelectedIndex;
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 1;
        }

        protected override int NumberOfTrailingBlankRows()
        {
            return 0;
        }
        protected override IBOColSelectorControl CreateSelector()
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
        public void Test_Constructor_ShouldSetPreserveSelectedItemToFalse()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOComboBoxSelector selector = (IBOComboBoxSelector) CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsFalse(selector.PreserveSelectedItem);
        }

        [Test]
        public void Test_SelectedBusinessObject_BlankItemSelected_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector selector = (IBOComboBoxSelector)CreateSelector();
            Car car = new Car();
            BusinessObjectCollection<Car> collection = new BusinessObjectCollection<Car> { car };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, -1);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, selector.NoOfItems, "The blank item and one other");
            Assert.AreEqual(-1, SelectedIndex(selector));
            Assert.AreEqual(null, selector.SelectedValue);
            //---------------Execute Test ----------------------
            SetSelectedIndex(selector, 0);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, SelectedIndex(selector));
            Assert.AreEqual("", selector.SelectedValue);
            Assert.IsNull(selector.SelectedBusinessObject);
        }

        [Test]
        public void Test_SelectedBusinessObject_SetToNull_ShouldHaveNothingSelectedInCombo()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector selector = (IBOComboBoxSelector)CreateSelector();
            Car car = new Car();
            BusinessObjectCollection<Car> collection = new BusinessObjectCollection<Car> { car };
            selector.BusinessObjectCollection = collection;
            selector.SelectedBusinessObject = car;
            //---------------Assert Precondition----------------
            Assert.AreSame(car, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            AssertIsNullSelection(selector);
        }

        [Test]
        public void Test_PreserveSelectedItem_AsTrue_WhenSetBusinessObjectCollection_AndSelectedItemInNewCol_ShouldPreserveSelection()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector selector = (IBOComboBoxSelector)CreateSelector();
            selector.PreserveSelectedItem = true;
            Car car = new Car();
            Car car2 = new Car();
            Car car3 = new Car();
            Car car4 = new Car();
            BusinessObjectCollection<Car> collection = new BusinessObjectCollection<Car> { car, car2, car3 };
            selector.BusinessObjectCollection = collection;
            selector.SelectedBusinessObject = car2;
            BusinessObjectCollection<Car> newCollection = new BusinessObjectCollection<Car> { car, car4, car2, car3 };
            //---------------Assert Precondition----------------
            Assert.AreSame(car2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = newCollection;
            //---------------Test Result -----------------------
            Assert.AreSame(car2, selector.SelectedBusinessObject);
        }

        [Test]
        public void Test_PreserveSelectedItem_AsTrue_WhenSetBusinessObjectCollection_AndSelectedItemNotInNewCol_ShouldSelectNull()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector selector = (IBOComboBoxSelector)CreateSelector();
            selector.PreserveSelectedItem = true;
            Car car = new Car();
            Car car2 = new Car();
            Car car3 = new Car();
            Car car4 = new Car();
            BusinessObjectCollection<Car> collection = new BusinessObjectCollection<Car> { car, car2, car3 };
            selector.BusinessObjectCollection = collection;
            selector.SelectedBusinessObject = car2;
            BusinessObjectCollection<Car> newCollection = new BusinessObjectCollection<Car> { car, car4, car3 };
            //---------------Assert Precondition----------------
            Assert.AreSame(car2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = newCollection;
            //---------------Test Result -----------------------
            AssertIsNullSelection(selector);
        }

        [Test]
        public void Test_PreserveSelectedItem_AsFalse_WhenSetBusinessObjectCollection_AndSelectedItemInNewCol_ShouldNotPreserveSelection()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector selector = (IBOComboBoxSelector)CreateSelector();
            selector.AutoSelectFirstItem = false;
            selector.PreserveSelectedItem = false;
            Car car = new Car();
            Car car2 = new Car();
            Car car3 = new Car();
            Car car4 = new Car();
            BusinessObjectCollection<Car> collection = new BusinessObjectCollection<Car> { car, car2, car3 };
            selector.BusinessObjectCollection = collection;
            selector.SelectedBusinessObject = car2;
            BusinessObjectCollection<Car> newCollection = new BusinessObjectCollection<Car> { car, car4, car2, car3 };
            //---------------Assert Precondition----------------
            Assert.AreSame(car2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = newCollection;
            //---------------Test Result -----------------------
            AssertIsNullSelection(selector);
        }

        private static void AssertIsNullSelection(IBOComboBoxSelector selector)
        {
            Assert.IsNull(selector.SelectedBusinessObject);
            Assert.IsNull(selector.SelectedValue);
            Assert.IsNull(selector.SelectedItem);
            Assert.AreEqual("", selector.Text);
            Assert.AreEqual(-1, selector.SelectedIndex);
        }

        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IBusinessObjectCollection col;
            IBOColSelectorControl boColSelector = GetSelectorWith_4_Rows(out col);
            const string propName = "TestProp";
            const int rowIndex = 1;
            MyBO bo = (MyBO) col[rowIndex];
            boColSelector.BusinessObjectCollection = col;
            string origStringValue = ((IBOComboBoxSelector)boColSelector).GetItemText(bo);
            //---------------Verify precondition----------------
            Assert.AreEqual(bo.ToString(), origStringValue);
            //---------------Execute Test ----------------------
            const string newPropValue = "NewValue";
            bo.SetPropertyValue(propName, newPropValue);
            bo.Save();
            //---------------Test Result -----------------------
            string newStringValue = ((IBOComboBoxSelector)boColSelector).GetItemText(bo);
            Assert.AreNotEqual(origStringValue, newStringValue);
            Assert.AreEqual(newPropValue + " - " + bo.MyBoID, newStringValue);
        }

        [Test]
        public void Test_BOPropUpdated_WhenBONotInComboBox_ShouldNotRaiseError_FixBug()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IBusinessObjectCollection col;
            IBOColSelectorControl boColSelector = GetSelectorWith_4_Rows(out col);
            const string propName = "TestProp";
            const int rowIndex = 1;
            MyBO bo = (MyBO)col[rowIndex];
            boColSelector.BusinessObjectCollection = col;
            IBOComboBoxSelector selector = ((IBOComboBoxSelector)boColSelector);
            string origStringValue = selector.GetItemText(bo);
            selector.ComboBox.Items.Remove(bo);
            //---------------Assert Precondition----------------
            Assert.AreEqual(bo.ToString(), origStringValue);
            Assert.AreEqual(4, selector.ComboBox.Items.Count);
            //---------------Execute Test ----------------------
            const string newPropValue = "NewValue";
            bo.SetPropertyValue(propName, newPropValue);
            bo.Save();
            //---------------Test Result -----------------------
            string newBoItemText = selector.GetItemText(bo);
            Assert.AreNotEqual(origStringValue, newBoItemText);
            Assert.AreEqual(newPropValue + " - " + bo.MyBoID, newBoItemText);
        }
    }

   
}
