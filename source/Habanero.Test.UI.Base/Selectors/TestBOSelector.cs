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
    public class TestBOColSelector
    {
        protected virtual IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected virtual void SetSelectedIndex(IBOColSelectorControl colSelector, int index)
        {
            ((IBOComboBoxSelector)colSelector).ComboBox.SelectedIndex = index;
        }

        protected virtual int SelectedIndex(IBOColSelectorControl colSelector)
        {
            return ((IBOComboBoxSelector)colSelector).ComboBox.SelectedIndex;
        }

        protected virtual IBOColSelectorControl CreateSelector()
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
        protected IBOColSelectorControl GetSelectorWith_4_Rows(out IBusinessObjectCollection col)
        {
            col = GetCollectionWith_4_Objects();
            IBOColSelectorControl boColSelector = CreateSelector();
            boColSelector.BusinessObjectCollection = col;
            return boColSelector;
        }
        protected virtual IBusinessObjectCollection GetCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO { TestProp = "b" };
            MyBO cp2 = new MyBO { TestProp = "d" };
            MyBO cp3 = new MyBO { TestProp = "c" };
            MyBO cp4 = new MyBO { TestProp = "a" };
            return new BusinessObjectCollection<MyBO> { cp, cp2, cp3, cp4 };
        }
        protected virtual IBusinessObjectCollection GetCollectionWithNoItems()
        {
            new MyBO();//Purely to load the ClassDefs.
            return new BusinessObjectCollection<MyBO>();
        }

        protected virtual IBusinessObject CreateNewBO()
        {
            return new MyBO();
        }

        protected virtual IBusinessObjectCollection GetCollectionWithOneBO(out IBusinessObject bo)
        {
            bo = new MyBO();
            return new BusinessObjectCollection<MyBO> { (MyBO)bo };
        }

        protected virtual IBusinessObjectCollection GetCollectionWithTowBOs(out IBusinessObject myBO)
        {
            myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            return new BusinessObjectCollection<MyBO> { (MyBO)myBO, myBO2 };
        }

        protected virtual int ActualIndex(int index)
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
        public virtual void Test_SetBOCol()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObjectCollection collection = GetCollectionWithNoItems();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, colSelector.BusinessObjectCollection);
            Assert.AreEqual(NumberOfLeadingBlankRows(), colSelector.NoOfItems, "By default should always put 1 item in blank");
        }


        [Test]
        public virtual void Test_SetBOCol_SetsItemsInSelector()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, collection.Count);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, colSelector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
        }

        [Test]
        public virtual void Test_GetBusinessObjectAtRow_ReturnsTheCorrectBO()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);
            colSelector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, colSelector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject businessObjectAtRow = colSelector.GetBusinessObjectAtRow(ActualIndex(0));
            //---------------Test Result -----------------------
            Assert.AreSame(bo, businessObjectAtRow, "The Business Object at the first row Row should be");
        }


        [Test]
        public virtual void Test_GetBusinessObjectAtRow_0_ReturnsNotNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);
            colSelector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, colSelector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject businessObjectAtRow = colSelector.GetBusinessObjectAtRow(ActualIndex(0));
            //---------------Test Result -----------------------
            Assert.AreSame(bo, businessObjectAtRow, "The business object at the first row selected" );
        }
        [Test]
        public virtual void Test_GetBusinessObjectAtRow_Neg1_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);
            colSelector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, colSelector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject businessObjectAtRow = colSelector.GetBusinessObjectAtRow(-1);
            //---------------Test Result -----------------------
            Assert.IsNull(businessObjectAtRow);
        }

        [Test]
        public virtual void Test_GetBusinessObjectAtRow_GTNoRows_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);
            colSelector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, colSelector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject businessObjectAtRow = colSelector.GetBusinessObjectAtRow(ActualIndex(1));
            //---------------Test Result -----------------------
            Assert.IsNull(businessObjectAtRow);
        }

        [Test]
        public virtual void Test_AddBOToCol_UpdatesItems()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);

            colSelector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreSame(collection, colSelector.BusinessObjectCollection);
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject newBO = CreateNewBO();
            collection.Add(newBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(2), colSelector.NoOfItems, "The blank item and one other");
            Assert.AreSame(bo, colSelector.GetBusinessObjectAtRow(ActualIndex(0)));
            Assert.AreSame(newBO, colSelector.GetBusinessObjectAtRow(ActualIndex(1)));
        }
        [Test]
        public virtual void Test_RemoveBOToCol_UpdatesItems()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out bo);
            IBusinessObject newMyBO = collection[1];
            colSelector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            Assert.AreSame(bo, colSelector.GetBusinessObjectAtRow(ActualIndex(0)));
            Assert.AreSame(newMyBO, colSelector.GetBusinessObjectAtRow(ActualIndex(1)));
            //---------------Execute Test ----------------------
            collection.Remove(bo);
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(1), colSelector.NoOfItems, "The blank item and one other");
            Assert.AreSame(newMyBO, colSelector.GetBusinessObjectAtRow(ActualIndex(0)));
        }


        [Test]
        public virtual void Test_ResetBOCol_ResetsItems()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out bo);
            IBusinessObject newMyBO = collection[1];
            colSelector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            Assert.AreSame(bo, colSelector.GetBusinessObjectAtRow(ActualIndex(0)));
            Assert.AreSame(newMyBO, colSelector.GetBusinessObjectAtRow(ActualIndex(1)));
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = GetCollectionWithNoItems();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(0), colSelector.NoOfItems, "The blank item ");
            Assert.IsNull(colSelector.SelectedBusinessObject);
        }
        [Test]
        public virtual void Test_ResetBOCol_DeregistersForBOChangedEvents()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out bo);

            colSelector.BusinessObjectCollection = collection;
            colSelector.BusinessObjectCollection = GetCollectionWithNoItems();
            //---------------Assert Precondition----------------
            Assert.AreEqual(NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            collection.Add(CreateNewBO());
            //---------------Test Result -----------------------
            Assert.AreEqual(NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
        }

        [Test]
        public virtual void Test_ResetBOCol_ToNullClearsItems()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);
            colSelector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreEqual(ActualNumberOfRows(collection.Count), colSelector.NoOfItems, "The blank item and one other");
            Assert.AreSame(bo, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = null;
            //---------------Test Result -----------------------
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.IsNull(colSelector.BusinessObjectCollection);
            Assert.AreEqual(NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item");
        }
        [Test]
        public virtual void Test_SelectedBusinessObject_ReturnsNullIfNoItemSelected()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);
            colSelector.BusinessObjectCollection = collection;
            colSelector.SelectedBusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = colSelector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNull(selectedBusinessObject);
        }

        [Test]
        public virtual void Test_SelectedBusinessObject_FirstItemSelected_ReturnsItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(0));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            Assert.AreEqual(ActualIndex(0), SelectedIndex(colSelector));
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = colSelector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(bo, selectedBusinessObject);
        }
        [Test]
        public virtual void Test_SelectedBusinessObject_SecondItemSelected_ReturnsItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO);
            IBusinessObject myBO2 = collection[1];
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = colSelector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(myBO2, selectedBusinessObject);
        }
        [Test]
        public virtual void Test_Set_SelectedBusinessObject_SetsItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO);
            IBusinessObject myBO2 = collection[1];
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
//            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreSame(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = myBO;
            //---------------Test Result -----------------------
            Assert.AreSame(myBO, colSelector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(colSelector));
        }

        [Test]
        public virtual void Test_Set_SelectedBusinessObject_Null_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO);
            IBusinessObject myBO2 = collection[1];
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }

        [Test]
        public virtual void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO);
            IBusinessObject myBO2 = collection[1];
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = CreateNewBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualIndex(2), colSelector.NoOfItems, "The blank item");
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }
        [Test]
        public virtual void Test_AutoSelectsFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, colSelector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item");
            Assert.AreSame(myBO, colSelector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(colSelector));
        }

        [Test]
        public virtual void Test_AutoSelectsFirstItem_NoItems()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObjectCollection collection = GetCollectionWithNoItems();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, colSelector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item");
            Assert.AreSame(null, colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }

        [Test]
        public virtual void Test_SelectorFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectCollection col;
            IBOColSelectorControl boColSelector = GetSelectorWith_4_Rows(out col);
            bool itemSelected = false;
            boColSelector.SelectedBusinessObject = null;
            boColSelector.BusinessObjectSelected += (delegate { itemSelected = true; });
            //---------------Execute Test ----------------------
            boColSelector.SelectedBusinessObject = col[1];
            //---------------Test Result -----------------------
            Assert.IsTrue(itemSelected);
        }

        [Test]
        public virtual void Test_Selector_Clear_ClearsItems()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectCollection col;
            IBOColSelectorControl boColSelector = GetSelectorWith_4_Rows(out col);
            //---------------Assert Preconditions --------------
            Assert.IsNotNull(boColSelector.SelectedBusinessObject);
            Assert.IsNotNull(boColSelector.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            boColSelector.Clear();
            //---------------Test Result -----------------------
            Assert.IsNull(boColSelector.BusinessObjectCollection);
            Assert.IsNull(boColSelector.SelectedBusinessObject);
            Assert.AreEqual(0, boColSelector.NoOfItems);
        }
    }
}