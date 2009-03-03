using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the ComboBoxSelector class but can be overridden to 
    /// test any class that implements the IBOSelectorControl Interface.
    /// The methods to override are <see cref="GetControlFactory"/><br/> 
    /// <see cref="SetSelectedIndex"/> <br/>
    /// <see cref="SelectedIndex"/><br/>
    /// <see cref="CreateSelector"/><br/>
    /// <see cref="NumberOfLeadingBlankRows"/><br/>
    /// 
    /// You should also override this for the VWG implementation of each control
    /// override the <see cref="GetControlFactory"/> to return a VWG control Factory.
    /// </summary>
    [TestFixture]
    public class TestBOSelector
    {
        protected virtual IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected virtual void SetSelectedIndex(IBOSelectorControl selector, int index)
        {
            ((IBOComboBoxSelector)selector).ComboBox.SelectedIndex = index;
        }

        protected virtual int SelectedIndex(IBOSelectorControl selector)
        {
            return ((IBOComboBoxSelector)selector).ComboBox.SelectedIndex;
        }

        protected virtual IBOSelectorControl CreateSelector()
        {
            return GetControlFactory().CreateComboBoxSelector();
        }
                /// <summary>
        /// The Number of rows that will always be blank for a Selector e.g. 
        /// If the Combo box has a blank item to select from then this will be one
        /// A selector of type grid that allows adding may also have an extra row.
        /// </summary>
        /// <returns></returns>
        protected virtual int NumberOfLeadingBlankRows()
        {
            return 1;
        }
        protected IBOSelectorControl GetSelectorWith_4_Rows(out BusinessObjectCollection<MyBO> col)
        {
            col = CreateCollectionWith_4_Objects();
            IBOSelectorControl boSelector = CreateSelector();
            boSelector.BusinessObjectCollection = col;
            return boSelector;
        }
        protected static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO { TestProp = "b" };
            MyBO cp2 = new MyBO { TestProp = "d" };
            MyBO cp3 = new MyBO { TestProp = "c" };
            MyBO cp4 = new MyBO { TestProp = "a" };
            return new BusinessObjectCollection<MyBO> { cp, cp2, cp3, cp4 };
        }

        protected int ActualIndex(int index)
        {
            return index + NumberOfLeadingBlankRows();
        }

        protected int ActualNumberOfRows(int noOfBOs)
        {
            return noOfBOs + NumberOfLeadingBlankRows();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }
        [Test]
        public void Test_SetBOCol()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            new MyBO(); //So that class defs are loaded
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, selector.BusinessObjectCollection);
            Assert.AreEqual(NumberOfLeadingBlankRows(), selector.NoOfItems, "By default should always put 1 item in blank");
        }


        [Test]
        public void Test_SetBOCol_SetsItemsInSelector()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, collection.Count);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, selector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
        }

        [Test]
        public void Test_GetBusinessObjectAtRow_ReturnsTheCorrectBO()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, selector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject businessObjectAtRow = selector.GetBusinessObjectAtRow(ActualIndex(0));
            //---------------Test Result -----------------------
            Assert.AreSame(myBO, businessObjectAtRow);
        }


        [Test]
        public void Test_GetBusinessObjectAtRow_0_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, selector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject businessObjectAtRow = selector.GetBusinessObjectAtRow(ActualIndex(0));
            //---------------Test Result -----------------------
            Assert.AreSame(myBO, businessObjectAtRow);
        }
        [Test]
        public void Test_GetBusinessObjectAtRow_Neg1_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, selector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject businessObjectAtRow = selector.GetBusinessObjectAtRow(-1);
            //---------------Test Result -----------------------
            Assert.IsNull(businessObjectAtRow);
        }
        [Test]
        public void Test_GetBusinessObjectAtRow_GTNoRows_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, selector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject businessObjectAtRow = selector.GetBusinessObjectAtRow(ActualIndex(1));
            //---------------Test Result -----------------------
            Assert.IsNull(businessObjectAtRow);
        }

        [Test]
        public void Test_AddBOToCol_UpdatesItems()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, selector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            MyBO newMyBO = new MyBO();
            collection.Add(newMyBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(2), selector.NoOfItems, "The blank item and one other");
            Assert.AreSame(myBO, selector.GetBusinessObjectAtRow(ActualIndex(0)));
            Assert.AreSame(newMyBO, selector.GetBusinessObjectAtRow(ActualIndex(1)));
        }
        [Test]
        public virtual void Test_RemoveBOToCol_UpdatesItems()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO newMyBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, newMyBO };
            selector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            Assert.AreSame(myBO, selector.GetBusinessObjectAtRow(ActualIndex(0)));
            Assert.AreSame(newMyBO, selector.GetBusinessObjectAtRow(ActualIndex(1)));
            //---------------Execute Test ----------------------
            collection.Remove(myBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(1), selector.NoOfItems, "The blank item and one other");
            Assert.AreSame(newMyBO, selector.GetBusinessObjectAtRow(ActualIndex(0)));
        }


        [Test]
        public void Test_ResetBOCol_ResetsItems()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO newMyBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, newMyBO };
            selector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            Assert.AreSame(myBO, selector.GetBusinessObjectAtRow(ActualIndex(0)));
            Assert.AreSame(newMyBO, selector.GetBusinessObjectAtRow(ActualIndex(1)));
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = new BusinessObjectCollection<MyBO>();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(0), selector.NoOfItems, "The blank item ");
            Assert.IsNull(selector.SelectedBusinessObject);
        }
        [Test]
        public void Test_ResetBOCol_DeregistersForBOChangedEvents()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO newMyBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, newMyBO };
            selector.BusinessObjectCollection = collection;
            selector.BusinessObjectCollection = new BusinessObjectCollection<MyBO> { new MyBO() };
            //---------------Assert Precondition----------------
            Assert.AreEqual(selector.BusinessObjectCollection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            collection.Add(new MyBO());
            //---------------Test Result -----------------------
            Assert.AreEqual(1 + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
        }
        [Test]
        public void Test_ResetBOCol_ToNullClearsItems()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();

            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreEqual(ActualNumberOfRows(collection.Count), selector.NoOfItems, "The blank item and one other");
            Assert.AreSame(myBO, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = null;
            //---------------Test Result -----------------------
            Assert.IsNull(selector.SelectedBusinessObject);
            Assert.IsNull(selector.BusinessObjectCollection);
            Assert.AreEqual(NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item");
        }
        [Test]
        public virtual void Test_SelectedBusinessObject_ReturnsNullIfNoItemSelected()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            selector.SelectedBusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = selector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNull(selectedBusinessObject);
        }

        [Test]
        public void Test_SelectedBusinessObject_FirstItemSelected_ReturnsItem()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(0));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            Assert.AreEqual(ActualIndex(0), SelectedIndex(selector));
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = selector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(myBO, selectedBusinessObject);
        }
        [Test]
        public virtual void Test_SelectedBusinessObject_SecondItemSelected_ReturnsItem()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = selector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(myBO2, selectedBusinessObject);
        }
        [Test]
        public virtual void Test_Set_SelectedBusinessObject_SetsItem()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
//            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreSame(myBO2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.SelectedBusinessObject = myBO;
            //---------------Test Result -----------------------
            Assert.AreSame(myBO, selector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(selector));
        }

        [Test]
        public virtual void Test_Set_SelectedBusinessObject_Null_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreEqual(myBO2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(selector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(selector));
        }

        [Test]
        public virtual void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreEqual(myBO2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.SelectedBusinessObject = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualIndex(2), selector.NoOfItems, "The blank item");
            Assert.IsNull(selector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(selector));
        }
        [Test]
        public virtual void Test_AutoSelectsFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, selector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(selector));
            Assert.AreEqual(null, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item");
            Assert.AreSame(myBO, selector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(selector));
        }
        [Test]
        public virtual void Test_AutoSelectsFirstItem_NoItems()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            new MyBO();//Purely to load the ClassDefs.
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, selector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(selector));
            Assert.AreEqual(null, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item");
            Assert.AreSame(null, selector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(selector));
        }

        [Test]
        public virtual void Test_SelectorFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IBOSelectorControl boSelector = GetSelectorWith_4_Rows(out col);
            bool itemSelected = false;
            boSelector.SelectedBusinessObject = null;
            boSelector.BusinessObjectSelected += (delegate { itemSelected = true; });
            //---------------Execute Test ----------------------
            boSelector.SelectedBusinessObject = col[1];
            //---------------Test Result -----------------------
            Assert.IsTrue(itemSelected);
        }

        [Test]
        public void Test_Selector_Clear_ClearsItems()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IBOSelectorControl boSelector = GetSelectorWith_4_Rows(out col);
            //---------------Assert Preconditions --------------
            Assert.IsNotNull(boSelector.SelectedBusinessObject);
            Assert.IsNotNull(boSelector.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            boSelector.Clear();
            //---------------Test Result -----------------------
            Assert.IsNull(boSelector.BusinessObjectCollection);
            Assert.IsNull(boSelector.SelectedBusinessObject);
            Assert.AreEqual(0, boSelector.NoOfItems);
        }
    }
}